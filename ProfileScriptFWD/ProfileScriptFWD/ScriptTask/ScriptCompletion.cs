namespace Custom.InputAccel.UimTaskScript
{
    using Emc.InputAccel.UimScript;
    using System;
    using System.Windows;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Emc.InputAccel.CaptureFlow;
    using Emc.InputAccel.CaptureClient;
    using UimScript;
    using Newtonsoft.Json;
    using System.Collections;

    class ScriptCompletion : UimScriptTask
    {
        public override void TaskLoad(IUimNodeData taskNode)
        {

            //MessageBox.Show("Task Load");
            //MessageBox.Show(taskNode.ValueSet.ReadString("hiddenCommons"));
            /*IUimDataContext[] context = taskNode.GetDocuments();
            foreach(IUimDataContext s in context)
            {
                //MessageBox.Show(s.StepCustomValue);
                this.Commons = Convert.ToBoolean(s.StepCustomValue);
            }*/


        }
        public override TaskFinishAction BeforeTaskFinish(IUimNodeData taskNode, CloseReasonCode reasonCode)
        {
            /*MessageBox.Show("Task Finish");
            IUimDataContext[] context = taskNode.GetDocuments();
            IUimFieldDataContext[] checkbox;
            ArrayList list = new ArrayList();
            foreach (IUimDataContext c in context)
            {
                checkbox = c.GetFieldDataContextArray();
                foreach(IUimFieldDataContext cb in checkbox)
                {
                    //MessageBox.Show(cb.Name);
                    if (cb.Name.StartsWith("suitabilityQ"))
                    {
                        list.Add(cb.Name);
                        MessageBox.Show(cb.Name);
                    }
                }
            }*/

            /*IUimFieldDataContext checkbox1 = dataContext.FindFieldDataContext("suitabilityQ1_S_1");
            IUimFieldDataContext checkbox2 = dataContext.FindFieldDataContext("suitabilityQ1_english");
            IUimFieldDataContext text = dataContext.FindFieldDataContext("suitabilityQ1");
            string[] v1 = checkbox1.Name.Split('_');
            string[] v2 = checkbox2.Name.Split('_');
            //MessageBox.Show(checkbox1.Name);
            //MessageBox.Show(checkbox1.Value.ToString());

            //MessageBox.Show(checkbox2.Name);
            //MessageBox.Show(checkbox2.Value.ToString());
            //text.SetValue(v1[1] + "-" + v2[1]);
            //MessageBox.Show(text.Value.ToString());
            ArrayList list = new ArrayList();
            //JsonObject json = new JsonObject();
            if (checkbox1.Value.ToString() == "1")
            {
                list.Add(v1[1]);
            }
            if (checkbox2.Value.ToString() == "1")
            {
                list.Add(v2[1]);
            }
            string jsonresult = JsonConvert.SerializeObject(list);
            text.SetValue(jsonresult);*/
            return TaskFinishAction.Finish;
        }

    }
}
