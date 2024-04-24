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
    public partial class supplierInfo : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();

        public supplierInfo()
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

        private void supplierInfo_Load(object sender, EventArgs e)
        {
            sqlStr = "SELECT * FROM Supplier";
            fillDataGridView1(sqlStr);

            
        }

        private void button3_Click(object sender, EventArgs e)      // Search button
        {
            string supID = (textBox3.Text.TrimStart(' ')).TrimStart('0');
            string supName = textBox2.Text.TrimStart(' ');
            string supTel = textBox4.Text.TrimStart(' ');
            string supAddress = textBox5.Text.TrimStart(' ');
            sqlStr = $"SELECT * FROM Supplier WHERE SupplierID is not NULL ";
            if (!string.IsNullOrEmpty(supID))
                sqlStr += $" AND SupplierID = '{string.Format("{0:000}", Convert.ToInt32(supID))}'";
            if (!string.IsNullOrEmpty(supName))
                sqlStr += $" AND SName like '%{supName}%'";
            if (!string.IsNullOrEmpty(supTel))
                sqlStr += $" AND Phone like '%{supTel}%'";
            if (!string.IsNullOrEmpty(supAddress))
                sqlStr += $" AND Address like '%{supAddress}%'";

            fillDataGridView1(sqlStr);
            cleanUp();
        }

        private void fillDataGridView1(string sql)
        {
            dt.Clear();
            sqlSelection(sql, dt);
            dataGridView1.DataSource = dt;
        }

        private void cleanUp()
        {
            textBox3.Text = "";
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sql, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }


        private void button9_Click(object sender, EventArgs e)      // Minimize
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button8_Click(object sender, EventArgs e)     // Close button 
        {
            this.Close();
        }

        
    }
}
