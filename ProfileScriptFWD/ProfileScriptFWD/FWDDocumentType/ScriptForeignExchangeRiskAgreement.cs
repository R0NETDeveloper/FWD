namespace Custom.InputAccel.UimScript
{
    using Emc.InputAccel.UimScript;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;

    class ScriptForeignExchangeRiskAgreement : UimScriptDocument
    {
        public static ArrayList prelist;
        public static ArrayList prelistTable;
        public static string btnname;
        public static int rowSelect;
        public bool flag_rowSelect = false;

        FormLookUp wform = new FormLookUp();
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
                //Cumtom Table
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
                m.LookupRestID(controlContext, controlContext.ParentForm, "identityNo");
                m.LookupRestPolicy(controlContext, controlContext.ParentForm, "policyNo");
            }
            catch
            (Exception e)
            {
                string error = e.StackTrace;
            }
        }
        public void ButtonClick(IUimFormControlContext controlContext)
        {
            try
            {
                /// connection DB
                ScriptMain m = new ScriptMain();
                IUimDataContext datacontext = controlContext.ParentForm.UimDataContext;
                //set fund name

                m.restFundName(datacontext);
                //dt_grid = m.setTEST();

                wform.ShowDialog();
                btnname = controlContext.ControlName.Remove(0, 4);
                IUimTableSectionContext table = controlContext.ParentForm.UimDataContext.FindTableSection(btnname);
                int row = table.RowCount;

                // Check click ok or cancel
                if (wform.flag_btn)
                {

                    if (row == 0)
                    {
                        // MessageBox.Show("STAGE 1");
                        table.InsertNewRow(0);
                        InsertRow(table, row);
                    }
                    else
                    {
                        if (flag_rowSelect)
                        {
                            // MessageBox.Show("STAGE 2");

                            if (row == rowSelect)
                            {
                                // MessageBox.Show("STAGE 2.1");
                                table.InsertNewRow(row);
                            }
                            InsertRow(table, rowSelect);
                        }
                        else
                        {
                            // MessageBox.Show("STAGE 3");
                            table.InsertNewRow(row);
                            InsertRow(table, row);
                        }
                    }

                }

                wform.fundName = string.Empty;
                flag_rowSelect = false;



            }
            catch (Exception e)
            {
                string error = e.StackTrace;
                MessageBox.Show(e.ToString());
            }
        }

        public void EnterControl(IUimFormControlContext controlContext)
        {
            try
            {

                if (controlContext.ControlName.StartsWith("c_"))
                {
                    flag_rowSelect = true;
                    rowSelect = controlContext.TableRowIndex;
                    //MessageBox.Show("ControlName = " + controlContext.ControlName + "Select row = " + rowSelect.ToString());

                }



            }
            catch
            (Exception e)
            {
                string error = e.StackTrace;
            }
        }

        public void InsertRow(IUimTableSectionContext tableContext, int rowIndex)
        {
            string fname = tableContext.GetFieldNames()[0];
            //MessageBox.Show("START Insert row fname = "+fname );
            IUimFieldDataContext x = tableContext.GetFieldAt(rowIndex, fname);
            x.SetValue(wform.fundName);
            wform.fundName = string.Empty;
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
