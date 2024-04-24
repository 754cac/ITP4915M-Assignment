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
    public partial class Login : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";

        public Login()
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


        private void btnLogin_Click(object sender, EventArgs e)         // Login
        {
            DataTable dt = new DataTable();
            sqlStr = "SELECT * FROM Account WHERE EmailAddress = @username AND Password = @password";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.SelectCommand.Parameters.Add("@username", OleDbType.VarChar, 30).Value = txtUser.Text;
            dataAdapter.SelectCommand.Parameters.Add("@password", OleDbType.VarChar, 30).Value = textPassword.Text;
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();

            if (dt.Rows.Count == 1)
            {
                menu mainPage = new menu(dt.Rows[0]["EmployeeID"].ToString());
                dt.Clear();
                this.Hide();
                mainPage.ShowDialog();
                this.Show();
                txtUser.Text = "";
                textPassword.Text = "";
            }
            else {
                textPassword.Text = "";
                MessageBox.Show("Invalid User name / Password");
            }
        }

        private void btnExit_Click(object sender, EventArgs e)          // EXIT
        {
            this.Close();
        }

        //////////////////////////////////// Property /////////////////////////////////////////

        private void textPassword_Click(object sender, EventArgs e)
        {
            textPassword.ForeColor = Color.Black;
            textPassword.PasswordChar = '●';
        }
    }
}
