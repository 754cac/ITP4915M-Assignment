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

namespace WindowsFormsApp1
{
    public partial class pmMain1 : Form
    {
        public Point mouseLocation;
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        

        public pmMain1()
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

        private void pmMain1_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT PurchaseID, ReleaseType, AddressType, AddressID, PurchaseDate, ExpectedDate FROM Purchase ";
            fillDataGridView1(sqlStr);
        }

        //////////////////////////////////////////  Event Handler  ///////////////////////////////////////////
        private void btnSearch_Click(object sender, EventArgs e)
        {
            sqlStr = $"SELECT PurchaseID, ReleaseType, AddressType, AddressID, PurchaseDate, ExpectedDate FROM Purchase ";
            string idInput = (textBox1.Text.TrimStart(' ')).TrimStart('0');
            if (!string.IsNullOrEmpty(idInput))
            {
                idInput = string.Format("{0:000}", Convert.ToInt32(idInput));
                sqlStr += $" WHERE PurchaseID = '{idInput}' "; ;
            }
            else if (cbStatus.SelectedIndex > -1)
            {
                string type = "";
                if (cbStatus.SelectedIndex == 0) type = "BPR";
                if (cbStatus.SelectedIndex == 1) type = "PPO";
                if (cbStatus.SelectedIndex == 2) type = "SPO";
                sqlStr += $" WHERE ReleaseType = '{type}' "; ;
                fillDataGridView1(sqlStr);
            }
            fillDataGridView1(sqlStr);
            cleanUp();
        }
        private void button3_Click(object sender, EventArgs e)      // Create button
        {
            pmrejectline createOrderPage = new pmrejectline();
            this.Hide();
            createOrderPage.ShowDialog();
            this.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e) // EDIT Order
        {
            if (dt.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    string purchaseID = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    pmLine orderDetail = new pmLine(this, purchaseID);
                    this.Hide();
                    orderDetail.ShowDialog();
                    this.Show();
                }
            }
        }











        //////////////////////////////////////////  Own Method  ///////////////////////////////////////////
        private void fillDataGridView1(string sqlStr)
        {
            dt.Clear();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dt;
        }




        private void cleanUp()
        {
            textBox1.Text = "";
            cbStatus.Text = "";
            cbStatus.SelectedIndex = -1;
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }





        //////////////////////////////////////////  Property  ///////////////////////////////////////////
        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            BPAStatus status = new BPAStatus();
            this.Hide();
            status.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)  // Item Report 
        {
            pmItemselect itemReport = new pmItemselect();
            this.Hide();
            itemReport.ShowDialog();
            this.Show();
        }

        private void button4_Click(object sender, EventArgs e)  // BPA Report
        {
            pmBPAselect BPAreport = new pmBPAselect();
            this.Hide();
            BPAreport.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)  // Supp Info
        {
            supplierInfo supplierInfo = new supplierInfo();
            this.Hide();
            supplierInfo.ShowDialog();
            this.Show();
        }
    }
}
