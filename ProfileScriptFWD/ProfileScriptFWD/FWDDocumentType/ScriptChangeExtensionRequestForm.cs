namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    class ScriptChangeExtensionRequestForm : UimScriptDocument
    {
        public static ArrayList prelist;
        public static ArrayList prelistTable;

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
                //Custom Table
                prelistTable = m.getLabelTable(form);


                //set Dropdowmn
                List<ScriptMain.LookupDropDownsBankName> listBank = m.restDropDownBankName(dataContext, "BANKMASTER");
                m.setDropdownBankName(form, listBank, "ddl_changeICP_bankName");

                List<ScriptMain.LookupDropDownsProvince> listProvince = m.restDropDownProvince(dataContext, "PROVINCEMASTER");
                // 
                m.setDropdownProvince(form, listProvince, "ddl_changeAddress_province");  //ddl_Province ??
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
                m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_changeICP_bankName",1);


                if (controlContext.ControlName == "ddl_changeAddress_province") //ddl_Province ??
                {
                    // Set Null Zipcode at here
                    m.setDropdownNull(controlContext.ParentForm, "ddl_changeAddress_zipCode");
                    m.parseValueEmpty(controlContext.ParentForm, "ddl_changeAddress_zipCode");//แก้ตรงนี้ <-------
                    //System.Windows.MessageBox.Show("VALUE Null!!");

                    //set Value Province
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_changeAddress_province",2);//ddl_Province ??
                    // Fliter Zipcode Post Code

                    string value_postCode = controlContext.ParentForm.UimDataContext.FindFieldDataContext("changeAddress_provinceCode").ValueAsString; //Province ??  เพื่อเอา Code ไป search Zipcode
                    // System.Windows.MessageBox.Show("VALUE Post_Code="+ value_postCode);

                    if (!string.IsNullOrEmpty(value_postCode))
                    {
                        // System.Windows.MessageBox.Show("START SET DROPDOWN ZIPCODE");
                        List<ScriptMain.LookupDropDownsZipcode> listZipcode = new List<ScriptMain.LookupDropDownsZipcode>();
                        listZipcode = m.restDropDownZipcode(controlContext.ParentForm.UimDataContext, "POSTCODEMASTER",value_postCode);
                        m.setDropdownZipcode(controlContext.ParentForm, listZipcode, "ddl_changeAddress_zipCode");// ddl_zipcode ??
                    }
                }
                if (controlContext.ControlName == "ddl_changeAddress_zipCode") // ddl_zipcode ??
                {
                    m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_changeAddress_zipCode",1); // ddl_zipcode ??
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
                //Custom Table
                m.jsonFormatTable(dataContext,prelistTable);
            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
    }
}
