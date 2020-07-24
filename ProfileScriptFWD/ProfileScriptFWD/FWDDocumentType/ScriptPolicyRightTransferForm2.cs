namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;


    class ScriptPolicyRightTransferForm2 : UimScriptDocument
    {
        public static ArrayList prelist;
        public void DocumentLoad(IUimDataContext dataContext)
        {
            dataContext.TaskFinishOnErrorNotAllowed = true;
        }

        /// <summary>
        /// Executes when the Document is first loaded for the task by the Extraction 
        /// module, after all of the runtime setup is complete.
        ///</summary>
        /// <param name="dataContext">The context object for the document.</param>
        public void FormLoad(IUimDataEntryFormContext form)
        {
            try
            {
                IUimDataContext dataContext = form.UimDataContext;
                IUimFieldDataContext hidden = dataContext.FindFieldDataContext("hiddenCOMMONS");
                ScriptMain m = new ScriptMain();
                m.hiddenSection(form, hidden.ValueAsString);
                prelist = m.getLabelField(form);

                List<ScriptMain.LookupDropDownsBankName> list = m.restDropDownBankName(dataContext, "BANKMASTER");
                m.setDropdownBankName(form, list, "ddl_bankName");
                List<ScriptMain.LookupDropDownsProvince> listProvince = m.restDropDownProvince(dataContext, "PROVINCEMASTER"); 
                m.setDropdownProvince(form, listProvince, "dll_entity_address_province");  //ddl_Province ??
            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
        public void ExitControl(IUimFormControlContext controlContext)
        {
            try
            {
                //Lookup
                ScriptMain m = new ScriptMain();
                m.LookupRestPolicy(controlContext, controlContext.ParentForm, "policyNo");
                m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_bankName",1);

                //  Implement to Province - Postcode.(Zipcode)
                //case 1 
                if (controlContext.ControlName == "dll_entity_address_province") //ddl_Province ??
                {
                    //set Value Province
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "dll_entity_address_province", 2);//ddl_Province ??
                    // Fliter Zipcode Post Code

                    string value_postCode = controlContext.ParentForm.UimDataContext.FindFieldDataContext("entity_address_provinceCode").ValueAsString; //Province ??  เพื่อเอา Code ไป search Zipcode
                    // System.Windows.MessageBox.Show("VALUE Post_Code="+ value_postCode);

                    if (!string.IsNullOrEmpty(value_postCode))
                    {
                        // System.Windows.MessageBox.Show("START SET DROPDWON ZIPCODE");

                        List<ScriptMain.LookupDropDownsZipcode> listZipcode = m.restDropDownZipcode(controlContext.ParentForm.UimDataContext, "POSTCODEMASTER", value_postCode);
                        m.setDropdownZipcode(controlContext.ParentForm, listZipcode, "ddl_entity_address_zipCode");// ddl_zipcode ??
                    }



                }
                if (controlContext.ControlName == "ddl_entity_address_zipCode") // ddl_zipcode ??
                {
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_entity_address_zipCode", 1); // ddl_zipcode ??
                }

            }
            catch
            (Exception e)
            {
                string error = e.StackTrace;
            }
        }
        public void DocumentClosing(IUimDataContext dataContext, CloseReasonCode reason)
        {
            try
            {
                ScriptMain m = new ScriptMain();
                m.jsonFormat(dataContext, prelist);
            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
    }
}
