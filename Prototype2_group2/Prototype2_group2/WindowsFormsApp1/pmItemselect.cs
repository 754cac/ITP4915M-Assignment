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
    public partial class pmItemselect : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        DataTable dt2 = new DataTable();
        OleDbDataAdapter dataAdapter;
        OleDbConnection connection;
        string itemID = "";
        private DataGridViewRow itemRow;


        public pmItemselect()
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

        private void pmItemselect_Load(object sender, EventArgs e)
        {
            UpdateGrid("SELECT * FROM Item");
            dt2.Clear();
            sqlStr = "SELECT DISTINCT(Category) FROM Item";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();

            foreach (DataRow row in dt2.Rows)
            {
                cbcategory.Items.Add(row["Category"].ToString());
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            UpdateGrid($"SELECT * FROM Item WHERE ItemID LIKE '%{txtSearch.Text}%'");
            txtSearch.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            UpdateGrid($"SELECT * FROM Item WHERE Category LIKE '%{cbcategory.Text}%'");
            cbcategory.SelectedItem = null;
            cbcategory.Text = null;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (itemID != "")
            {
                this.Hide();
                itemreport form2 = new itemreport(itemID);
                form2.ShowDialog();
                form2 = null;
                this.Show();
                UpdateGrid("SELECT * FROM Item");
                cbcategory.SelectedItem = null;
                cbcategory.Text = "";
                txtSearch.Text = "";
            }
            else
            {
                MessageBox.Show("Please select the item.");
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            itemRow = dataGridView1.Rows[e.RowIndex];
            itemID = itemRow.Cells[0].Value.ToString();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            UpdateGrid("SELECT * FROM Item");
        }
    }
}
