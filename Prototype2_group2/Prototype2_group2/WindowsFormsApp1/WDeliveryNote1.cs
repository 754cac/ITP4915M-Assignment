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

namespace WindowsFormsApp1
{
    public partial class WDeliveryNote1 : Form
    {
        String obtype;

        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        private DataTable dt = new DataTable();
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";

        private DataGridViewRow stockRow;
        OleDbConnection connection;
        OleDbCommand command;

        string sqlStr = "";

        public WDeliveryNote1()
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtSearch.Clear();


        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearch.Text == "")
            {
                UpdateGrid("SELECT * FROM DeliveryNote");
            }
            else
            {
                UpdateGrid($"SELECT * FROM DeliveryNote WHERE DeliveryNoteID LIKE '%{txtSearch.Text.ToString()}%' OR RestaurantID LIKE '%{txtSearch.Text.ToString()}%'");
            }

 
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

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void WDeliveryNote_Load(object sender, EventArgs e)
        {
            UpdateGrid("SELECT * FROM DeliveryNote");

        }


    }
}
