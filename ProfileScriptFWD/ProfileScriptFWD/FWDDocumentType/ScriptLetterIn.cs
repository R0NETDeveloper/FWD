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
    public class ScriptLetterIn : UimScriptDocument
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

            }
            catch (Exception e)
            {
                string error = e.StackTrace;
            }
        }
    }
}