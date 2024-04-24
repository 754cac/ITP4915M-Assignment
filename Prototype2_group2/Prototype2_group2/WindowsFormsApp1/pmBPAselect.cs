using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.OleDb;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security.AccessControl;

namespace WindowsFormsApp1
{
    public partial class pmBPAselect : Form
    {

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        OleDbDataAdapter dataAdapter;
        OleDbConnection connection;
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        string agreementID = "";
        private DataGridViewRow agrRow;


        public pmBPAselect()
        {
            InitializeComponent();
            connection = new OleDbConnection(connStr);
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



        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            this.Close();
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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

        private void pmBPAselect_Load(object sender, EventArgs e)
        {
            UpdateGrid("SELECT AgreementID, AgreementType, CreateDate, SupplierID, EmployeeID FROM Contract WHERE AgreementType = 'BPA'");
            dt2.Clear();
            sqlStr = "SELECT SupplierID FROM Supplier";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();

            foreach (DataRow row in dt2.Rows)
            {
                cbsupplier.Items.Add(row["SupplierID"].ToString());
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateGrid($"SELECT AgreementID, AgreementType, CreateDate, SupplierID, EmployeeID FROM Contract  WHERE AgreementType = 'BPA' AND AgreementID LIKE '%{txtSearch.Text}%'");
            txtSearch.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateGrid($"SELECT AgreementID, AgreementType, CreateDate, SupplierID, EmployeeID FROM Contract  WHERE AgreementType = 'BPA' AND SupplierID = '{cbsupplier.Text}'");
            cbsupplier.SelectedItem = null;
            cbsupplier.Text = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UpdateGrid("SELECT AgreementID, AgreementType, CreateDate, SupplierID, EmployeeID FROM Contract WHERE AgreementType = 'BPA'");
        }

        private void btnbpareport_Click(object sender, EventArgs e)
        {
            if (agreementID != "")
            {

                this.Hide();
                BPAReport form2 = new BPAReport(agreementID);
                form2.ShowDialog();
                form2 = null;
                this.Show();
                UpdateGrid("SELECT AgreementID, AgreementType, CreateDate, SupplierID, EmployeeID FROM Contract WHERE AgreementType = 'BPA'");
                cbsupplier.SelectedItem = null;
                cbsupplier.Text = "";
                txtSearch.Text = "";

            }
            else
            {
                MessageBox.Show("Please select the contract.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                agrRow = dataGridView1.Rows[e.RowIndex];
                agreementID = agrRow.Cells[0].Value.ToString();
            }
        }
    }
}
