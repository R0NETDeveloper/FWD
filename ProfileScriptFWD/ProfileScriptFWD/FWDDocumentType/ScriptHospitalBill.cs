namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;

    class ScriptHospitalBill : UimScriptDocument
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
                m.jsonFormatTable(dataContext, prelistTable);
            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
    }
}
