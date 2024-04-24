using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace WindowsFormsApp1
{
    public partial class BPACreate : Form
    {
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";

        string agrType = "";
        string agrID = "";
        DateTime createDate ;
        DateTime enpiryDate;
        string remark = "";
        string staffID = "";
        string supID = "";

        
        


        DataTable dt1 = new DataTable();
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);


        public BPACreate()
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
            Environment.Exit(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void BPACreate_Load(object sender, EventArgs e)
        {
            agreementIDAuto();
            supplierName();
            cbSupName.SelectedIndex = 0;
            cbType.SelectedIndex = 0;

            button2.Hide(); button7.Hide(); button6.Hide(); button4.Hide();
        }

        private void agreementIDAuto()
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            sqlStr = "SELECT MAX(AgreementID) FROM Contract ";
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            String id = command.ExecuteScalar().ToString();
            TxtAgreementID.Text = (int.Parse(id) + 1).ToString("D3");
            connection.Close();
            agrID = (int.Parse(id) + 1).ToString("D3");
        }

        private void supplierName()
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            sqlStr = "SELECT SName FROM Supplier";
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            connection.Open();
            OleDbDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                cbSupName.Items.Add(reader["SName"].ToString());
            }
            connection.Close();
        }




        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == 0) { agrType = "BPA"; }
            if (cbType.SelectedIndex == 1) { agrType = "SPO"; }
            if (cbType.SelectedIndex == 2) { agrType = "CPA"; }
            if (cbType.SelectedIndex == 3) { agrType = "PPO"; }

        }

        private void cbSupName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSupName.SelectedItem != null)
            {
                OleDbConnection connection = new OleDbConnection(connStr);
                sqlStr = "SELECT SupplierID FROM Supplier WHERE SName = '" + cbSupName.SelectedItem.ToString() + "'";
                OleDbCommand command = new OleDbCommand(sqlStr, connection);
                connection.Open();
                String supplierID = command.ExecuteScalar().ToString();
                txtSupID.Text = supplierID;
                supID = txtSupID.Text;
                connection.Close();
            }
        }

        private void dtpCreate_ValueChanged(object sender, EventArgs e)
        {
            createDate = dtpCreate.Value;
        }

        private void dtpEnpiryDate_ValueChanged(object sender, EventArgs e)
        {
            enpiryDate = dtpEnpiryDate.Value;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            remark=txtTerms.Text;
            string employeeID = txtBStaffID.Text;
            if (CheckEmployeeIDExists(employeeID))
            {

                staffID = txtBStaffID.Text;
                string[] array = new string[] { agrID, agrType, supID, staffID, remark, createDate.ToString(), enpiryDate.ToString() };
                this.Hide();
                BPACreate2 form2 = new BPACreate2(array);
                form2.ShowDialog();

                form2 = null;
                this.Show();
                agreementIDAuto();



            }
            else
            {
                MessageBox.Show("EmployeeID does not exist.");
            }


        }

        private bool CheckEmployeeIDExists(string employeeID)
        {
            bool exists = false;
            OleDbConnection connection = new OleDbConnection(connStr);
            string sqlStr = "SELECT COUNT(*) FROM Employee WHERE EmployeeID = @EmployeeID";
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            command.Parameters.AddWithValue("@EmployeeID", employeeID);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                exists = true;
            }
            connection.Close();
            return exists;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID IS NOT NULL";
            sqlExecution(sqlStr);
            this.Close();
        }
        private void sqlExecution(string sql)
        {
            string sqlStr = sql;
            OleDbConnection connection = new OleDbConnection(connStr);
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }


    }



}

