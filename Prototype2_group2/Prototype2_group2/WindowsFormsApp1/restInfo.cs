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
    public partial class restInfo : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();

        public restInfo()
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

        private void restInfo_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT Brand.BrandID, BrandName, RestaurantID, RestName, RestAddress, Employee.EmployeeID, Name " +
                     $"FROM Brand, Restaurant, Employee, Account " +
                     $"WHERE Brand.BrandID = Restaurant.BrandID " +
                     $"AND Restaurant.UserID = Account.UserID AND Employee.EmployeeID = Account.EmployeeID " +
                     $"ORDER BY Brand.BrandID, RestaurantID";
            fillDataGridView1(sqlStr);

            //////////////////////////////////// in the works  //////////////////////////////////////
            button10.Hide(); button7.Hide();
            button6.Hide(); button4.Hide();
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
