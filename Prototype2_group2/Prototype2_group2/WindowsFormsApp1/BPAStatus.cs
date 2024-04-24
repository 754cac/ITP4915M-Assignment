using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class BPAStatus : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        private DataTable dt = new DataTable();
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        OleDbConnection connection;
        OleDbCommand command;
        string sqlStr = "";
        string agreementID = "";
        private DataGridViewRow agrRow;

        public BPAStatus()
        {
            InitializeComponent();
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BPAStatus_Load(object sender, EventArgs e)
        {
            UpdateGrid("SElECT AgreementID,AgreementType,ConExpiryDate,CreateDate,SupplierID,EmployeeID FROM Contract");

            ////////////////////// in the works //////////////
            button2.Hide(); button7.Hide(); button6.Hide(); button4.Hide();
        }

        public void UpdateGrid(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                OleDbDataAdapter dataAdapter =
                            new OleDbDataAdapter(sql, connStr);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (agreementID!="")
            {

                this.Hide();
                BPAEdit form2 = new BPAEdit(agreementID);
                form2.ShowDialog();
                form2 = null;
                this.Show();
                UpdateGrid("SElECT AgreementID,AgreementType,ConExpiryDate,CreateDate,SupplierID,EmployeeID FROM Contract");

            }
            else
            {
                MessageBox.Show("Please select the contract.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            agrRow = dataGridView1.Rows[e.RowIndex];
            agreementID = agrRow.Cells[0].Value.ToString();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            string idInput= txtSearch.Text;
            if (!string.IsNullOrEmpty(txtSearch.Text))
                UpdateGrid($"SElECT AgreementID,AgreementType,ConExpiryDate,CreateDate,SupplierID,EmployeeID FROM Contract WHERE SupplierID LIKE '%{idInput}%' OR AgreementID LIKE '%{idInput}%'");
            else
                UpdateGrid("SElECT AgreementID,AgreementType,ConExpiryDate,CreateDate,SupplierID,EmployeeID FROM Contract");
            txtSearch.Text = "";
        }

        private void btncreate_Click(object sender, EventArgs e)
        {
            this.Hide();
            BPACreate form3 = new BPACreate();
            form3.ShowDialog();
            form3 = null;
            this.Show();
            UpdateGrid("SElECT AgreementID,AgreementType,ConExpiryDate,CreateDate,SupplierID,EmployeeID FROM Contract");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            supplierInfo supplierInfo = new supplierInfo();
            this.Hide();
            supplierInfo.ShowDialog();
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)      // Mapping button
        {
            RequisitionMapping mappingPage = new RequisitionMapping();
            this.Hide();
            mappingPage.ShowDialog();
            this.Show();
        }
    }
}
