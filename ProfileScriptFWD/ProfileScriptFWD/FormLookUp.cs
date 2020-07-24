using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Custom.InputAccel.UimScript
{
    public partial class FormLookUp : Form
    {
        public string fundName { get; set; }
        public  bool flag_btn { get; set; }
        public DataTable dt;
        public DataSet ds;


        
        public FormLookUp()
        {
            InitializeComponent();
        }
        private void btnOK_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(fundName))
            { MessageBox.Show("Please select!");  }
            else
            {
                flag_btn = true;
               // dataGridView1.ClearSelection();
                this.Close();
            }
         

        }
        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            flag_btn = false;
            //dataGridView1.ClearSelection();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void FormLookUp_Load(object sender, EventArgs e)
        {
            dt = ScriptMain.dt;
           // dt = ScriptUnitLinkedProceedingForm.dt_grid;
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //dataGridView1.ClearSelection();
            fundName = dataGridView1.Rows[0].Cells[0].Value.ToString();

        }



        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //Search
            dt = ScriptMain.dt;
           // dt = ScriptUnitLinkedProceedingForm.dt_grid;
            string nameCol = dt.Columns[1].ColumnName;
            dt.DefaultView.RowFilter = string.Format(  " {1} LIKE '%{0}%'", textBox1.Text,nameCol);

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var value = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            fundName = value;
       
        }


    }
}
