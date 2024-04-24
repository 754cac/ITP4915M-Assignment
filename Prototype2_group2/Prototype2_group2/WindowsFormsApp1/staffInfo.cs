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

namespace WindowsFormsApp1
{
    public partial class staffInfo : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();

        public staffInfo()
        {
            InitializeComponent();
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void staffInfo_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT EmployeeID, Name, JobTitle, HireDate, Department.DepartmentID, DepartmentName FROM Employee, Department " +
                     $"WHERE Department.DepartmentID = Employee.DepartmentID ORDER BY Department.DepartmentID, JobTitle";

            fillDataGridView1(sqlStr);
            ///////////////////////////////////////// in the works //////////////////////////////////////////
            button2.Hide(); button10.Hide(); button7.Hide(); button11.Hide(); button6.Hide();
        }

        ///////////////////////////////////////// in the works //////////////////////////////////////////
        private void btnSearch_Click(object sender, EventArgs e)       // Search button
        {

        }


        private void fillDataGridView1(string sql)
        {
            dt.Clear();
            sqlSelection(sql, dt);
            dataGridView1.DataSource = dt;
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        private void button8_Click(object sender, EventArgs e)      // Close button
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)      // Logout button
        {
            this.Close();
        }

        
    }
}
