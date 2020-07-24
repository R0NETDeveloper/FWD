namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using Newtonsoft.Json;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class ScriptExpropriateForm : UimScriptDocument
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
                //Lookup parameter
                List<ScriptMain.LookupDropDownsBankName> list = m.restDropDownBankName(dataContext, "BANKMASTER");
                m.setDropdownBankName(form, list, "ddl_bankName");
                //ddl_bankName
                List<ScriptMain.LookupDropDownsProvince> listProvince = m.restDropDownProvince(dataContext, "PROVINCEMASTER");
                // 
                m.setDropdownProvince(form, listProvince, "ddl_address_province");  //ddl_Province ??
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
          
                m.LookupRestID(controlContext, controlContext.ParentForm, "identityNo");
                m.LookupRestPolicy(controlContext, controlContext.ParentForm, "policyNo");
                m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_bankName",1);

                //  Implement to Province - Postcode.(Zipcode)
                //case 1 
                if (controlContext.ControlName == "ddl_address_province") //ddl_Province ??
                {
                    //set Value Province
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_address_province", 2);//ddl_Province ??
                    // Fliter Zipcode Post Code

                    string value_postCode = controlContext.ParentForm.UimDataContext.FindFieldDataContext("address_provinceCode").ValueAsString; //Province ??  เพื่อเอา Code ไป search Zipcode
                                                                                                                                                       // System.Windows.MessageBox.Show("VALUE Post_Code="+ value_postCode);

                    if (!string.IsNullOrEmpty(value_postCode))
                    {
                        // System.Windows.MessageBox.Show("START SET DROPDWON ZIPCODE");

                        List<ScriptMain.LookupDropDownsZipcode> listZipcode = m.restDropDownZipcode(controlContext.ParentForm.UimDataContext, "POSTCODEMASTER", value_postCode);
                        m.setDropdownZipcode(controlContext.ParentForm, listZipcode, "ddl_address_postalCode");// ddl_zipcode ??
                    }



                }
                if (controlContext.ControlName == "ddl_address_postalCode") // ddl_zipcode ??
                {
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_address_postalCode", 1); // ddl_zipcode ??
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
                m.jsonFormat(dataContext,prelist);
            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
    }
}
