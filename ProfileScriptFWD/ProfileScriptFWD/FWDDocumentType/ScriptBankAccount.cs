namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.CaptureClient;
    using Emc.InputAccel.UimScript;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class ScriptBankAccount : UimScriptDocument
    {
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
            IUimDataContext dataContext = form.UimDataContext;
            IUimFieldDataContext hidden = dataContext.FindFieldDataContext("hiddenCOMMONS");
            ScriptMain m = new ScriptMain();
            m.hiddenSection(form, hidden.ValueAsString);

           
            List<ScriptMain.LookupDropDownsBankName> list = m.restDropDownBankName(dataContext, "BANKMASTER");
            
            m.setDropdownBankName(form, list, "ddl_bankName");
            //ddl_bankName
        }
        public void ExitControl(IUimFormControlContext controlContext)
        {
            try
            {
                //Lookup
                ScriptMain m = new ScriptMain();
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
            ScriptMain m = new ScriptMain();
            //m.jsonFormat(dataContext);
        }
    }
}
