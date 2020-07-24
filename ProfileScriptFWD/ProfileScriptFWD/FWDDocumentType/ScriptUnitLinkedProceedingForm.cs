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

    class ScriptUnitLinkedProceedingForm : UimScriptDocument
    {
        public static ArrayList prelist;
        public static ArrayList prelistTable;
        public static string btnname;
        public static int rowSelect ;
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
                //Custom Table
                prelistTable = m.getLabelTable(form);

                //// Dropdown
                List<ScriptMain.LookupDropDownsBankName> list = m.restDropDownBankName(dataContext, "BANKMASTER");
                m.setDropdownBankName(form, list, "ddl_partialWithdrawal_bankName");



            }
            catch (Exception e)
            {
                string error = e.StackTrace;
               
            }
   
        }


        public void ButtonClick(IUimFormControlContext controlContext)
        {
            try
            {
              // MessageBox.Show(flag_rowSelect.ToString());
                ScriptMain m = new ScriptMain();
                IUimDataContext datacontext = controlContext.ParentForm.UimDataContext;
                //set FundName
                m.restFundName(datacontext);
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



        public void ExitControl(IUimFormControlContext controlContext)
        {
            try
            {
                //Lookup
                ScriptMain m = new ScriptMain();
                // m.Lookup(controlContext, getFrom, "policyNo");
                IUimDataContext datacontext = controlContext.ParentForm.UimDataContext;
                m.LookupRestPolicy(controlContext, controlContext.ParentForm, "policyNo");
                m.parseValueDropdown(controlContext, controlContext.ParentForm, "ddl_partialWithdrawal_bankName",1);

            }
            catch
            (Exception e)
            {
                string error = e.StackTrace;
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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tableContext"></param>
        /// <param name="rowIndex"></param>

        public void InsertRow(IUimTableSectionContext tableContext, int rowIndex)
        {
            try
            {
                string fname = tableContext.GetFieldNames()[0];
                //MessageBox.Show("START Insert row fname = "+fname );
                IUimFieldDataContext x = tableContext.GetFieldAt(rowIndex, fname);
                x.SetValue(wform.fundName);
                wform.fundName = string.Empty;
            }
            catch (Exception e)
            {
                string error = e.StackTrace.ToString();

            }
      
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataContext"></param>
        /// <param name="reason"></param>

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
