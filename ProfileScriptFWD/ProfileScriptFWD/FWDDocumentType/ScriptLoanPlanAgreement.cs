namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using System.Collections;
    using System.Windows.Forms;
    using System.Json;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The custom script class for the document type 
    /// <Document Type Name as defined in Captiva Designer>.
    ///</summary>
    public class ScriptLoanPlanAgreement : UimScriptDocument
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
                // m.LookupIDCardSoap(controlContext, controlContext.ParentForm, "identityNo");
                m.LookupRestID(controlContext, controlContext.ParentForm, "identityNo");
                m.LookupRestPolicy(controlContext, controlContext.ParentForm, "policyNo");
                m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_bankName",1);
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