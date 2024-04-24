using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class BPAEdit : Form
    {
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
               + "Data Source=database.mdb";
        string sqlStr = "";

        string agreementID="001";    //testing

        string agrType = "";
        string agrID = "";
        DateTime createDate;
        DateTime enpiryDate;
        string remark = "";
        string staffID = "";
        string supID = "";

        DataTable dt1 = new DataTable();
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        public BPAEdit(string agreementID)
        {
            InitializeComponent();
            this.agreementID = agreementID;
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

        private void DisplayData(string agreementID)
        {
            string sqlStr = "SELECT c.AgreementID, c.AgreementType, c.ConExpiryDate, c.SupplierID, c.EmployeeID, c.Remark, s.SName, e.Name " +
                            "FROM Contract c , Supplier s , Employee e " +
                            "WHERE  c.EmployeeID=e.EmployeeID AND c.SupplierID=s.SupplierID AND c.AgreementID=@AgreementID  ";
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                OleDbCommand cmd = new OleDbCommand(sqlStr, conn);
                cmd.Parameters.AddWithValue("@AgreementID", agreementID);
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    TxtAgreementID.Text = reader["AgreementID"].ToString();
                    cbType.Text = reader["AgreementType"].ToString();
                    cbSupName.Text = reader["SName"].ToString();
                    txtBStaffID.Text = reader["EmployeeID"].ToString();
                    txtTerms.Text = reader["Remark"].ToString();
                    txtSupID.Text = reader["SupplierID"].ToString();
                    dtpEnpiryDate.Value = DateTime.Parse(reader["ConExpiryDate"].ToString());
                }
                else
                {
                    MessageBox.Show("AgreementID not found.");
                }
                reader.Close();
            }
        }

        private void BPAEdit_Load(object sender, EventArgs e)
        {
            DisplayData(agreementID);
            supplierName();

            button2.Hide(); button7.Hide(); button6.Hide(); button4.Hide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            remark = txtTerms.Text;
            string employeeID = txtBStaffID.Text;
            if (CheckAgreementExists(agreementID) == true)
            {
                if (CheckEmployeeIDExists(employeeID))
                {
                    staffID = txtBStaffID.Text;
                    agrID = TxtAgreementID.Text;
                    string[] array = new string[] { agrID, agrType, supID, staffID, remark, createDate.ToString(), enpiryDate.ToString() };
                    this.Hide();
                    BPAEdit2 form2 = new BPAEdit2(array);
                    form2.ShowDialog();
                    form2 = null;
                    this.Show();


                }
                else
                {
                    MessageBox.Show("EmployeeID does not exist.");
                }
            }
            else
            {
                MessageBox.Show("Agreement is not exist");
                this.Close();
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

        private void dtpEnpiryDate_ValueChanged(object sender, EventArgs e)
        {
            enpiryDate = dtpEnpiryDate.Value;
        }

        private void dtpCreate_ValueChanged(object sender, EventArgs e)
        {
            createDate = dtpCreate.Value;
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

        private void cbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbType.SelectedIndex == 0) { agrType = "BPA"; }
            if (cbType.SelectedIndex == 1) { agrType = "SPO"; }
            if (cbType.SelectedIndex == 2) { agrType = "CPA"; }
            if (cbType.SelectedIndex == 3) { agrType = "PPO"; }
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

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private bool CheckAgreementExists(string agrID)
        {
            bool exists = false;
            OleDbConnection connection = new OleDbConnection(connStr);
            string sqlStr = $"SELECT COUNT(*) FROM Contract WHERE AgreementID = @agrID";
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            command.Parameters.AddWithValue("@agrID", agrID);
            connection.Open();
            int count = (int)command.ExecuteScalar();
            if (count > 0)
            {
                exists = true;
            }
            connection.Close();
            return exists;
        }
    }

}
