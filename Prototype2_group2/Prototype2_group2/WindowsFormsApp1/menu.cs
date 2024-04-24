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
    public partial class menu : Form
    {
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        
        string employeeID;
        string jobTitle;

        public menu()
        {
            InitializeComponent();
        }

        public menu(string employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
        }

        private void menu_Load(object sender, EventArgs e)
        {
            if (permissionMatching("Restaurant Manager"))
            {
                jobTitle = "Restaurant Manager";
                button10.Hide(); button7.Hide(); button11.Hide(); button12.Hide();
            }
            else if (permissionMatching("Category Manager"))
            {
                jobTitle = "Category Manager";
                button2.Hide(); button10.Hide(); button11.Hide(); button12.Hide();
            }
            else if (permissionMatching("Warehouse Clerk"))
            {
                jobTitle = "Warehouse Clerk";
                button2.Hide(); button10.Hide(); button11.Hide(); button12.Hide();
            }
            else if (permissionMatching("Purchase Manager"))
            {
                jobTitle = "Purchase Manager";
                button2.Hide(); button7.Hide(); button11.Hide(); button12.Hide();
            }
            else if (permissionMatching("Purchase Clerk"))
            {
                jobTitle = "Purchase Clerk";
                button2.Hide(); button7.Hide(); button11.Hide(); button12.Hide();
            }
            else if (permissionMatching("Admin"))
            {
                jobTitle = "Admin";
                button2.Hide(); button10.Hide(); button7.Hide(); button11.Hide();
            }
            ////////////////////////////////////////// in the works /////////////////////////////////////////////
            button6.Hide();
        }


        private void button2_Click(object sender, EventArgs e)          // Restaurant
        {
            if (jobTitle.Equals("Restaurant Manager"))
            {
                restOrderList restaurantMain = new restOrderList(employeeID);
                this.Hide();
                restaurantMain.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("You don't have the permission.");
        }

        private void button10_Click(object sender, EventArgs e)         // Purchase
        {
            if (jobTitle.Equals("Purchase Manager"))
            {
                pmMain1 purchaseMainPage = new pmMain1();
                this.Hide();
                purchaseMainPage.ShowDialog();
                this.Show();
            }
            else if (jobTitle.Equals("Purchase Clerk"))
            {
                RequisitionMapping mapping = new RequisitionMapping();
                this.Hide();    
                mapping.ShowDialog();   
                this.Show();
            }
            else MessageBox.Show("You don't have the permission.");
        }

        private void button7_Click(object sender, EventArgs e)          // Warehouse (stock)
        {
            if (jobTitle.Equals("Category Manager"))
            {
                Ccategory warehouseMain = new Ccategory();
                this.Hide();
                warehouseMain.ShowDialog();
                this.Show();
            }
            else if (jobTitle.Equals("Warehouse Clerk")) {
                Stock warehouseMain = new Stock();
                this.Hide();
                warehouseMain.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("You don't have the permission.");
        }

        private void button12_Click(object sender, EventArgs e)         // Admin
        {
            if (jobTitle.Equals("Admin"))
            {
                staffInfo adminMain = new staffInfo();
                this.Hide();
                adminMain.ShowDialog();
                this.Show();
            }
            else MessageBox.Show("You don't have the permission.");
        }



        public Boolean permissionMatching(string jobTitle) {
            sqlStr = "SELECT JobTitle FROM Employee WHERE EmployeeID = '" + employeeID + "'";
            executeSql(sqlStr);
            string jt = dt.Rows[0]["JobTitle"].ToString();
            if (jobTitle == jt)
                return true;
            else return false;
        }

        public void executeSql(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        private void button4_Click(object sender, EventArgs e)      // Logout
        {
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            this.Close();
        }

        
    }
}
