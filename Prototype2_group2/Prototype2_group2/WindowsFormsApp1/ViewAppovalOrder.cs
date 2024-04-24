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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;

namespace WindowsFormsApp1
{
    public partial class ViewAppovalOrder : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();

        Boolean withRestaurantID = false;
        Boolean restaurantSelected = false;
        string restaurantID = "";


        public ViewAppovalOrder()
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

        private void ViewAppovalOrder_Load(object sender, EventArgs e)
        {
            string[] listItem = fillcomboBoxes("RestaurantID", "Restaurant");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    cbPriority.Items.Add(listItem[i]);

            listItem = fillcomboBoxes("RestName", "Restaurant");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox1.Items.Add(listItem[i]);

            sqlStr = $"SELECT Requisition.RequisitionID, OrderLineID, Restaurant.RestaurantID, RestName, ItemID, Quantity, QuantityReceived, OrderLine.Status, ExpectedDate " +
                     $"FROM OrderLine, Requisition , Restaurant " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND Restaurant.RestaurantID = Requisition.RestaurantID " +
                     $"AND DeliveryNoteID is NULL " +
                     $"AND OrderLine.Status = 'Approved' " +
                     $"ORDER BY ExpectedDate";
            sqlSelection(sqlStr, dt);
            dataGridView1.DataSource = dt;

            //////////////////////////////// in the works ////////////////////////////////////////
            button6.Hide(); button4.Hide();
        }

        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dt.Clear();
            sqlStr = $"SELECT Requisition.RequisitionID, OrderLineID, Restaurant.RestaurantID, RestName, ItemID, Quantity, QuantityReceived, OrderLine.Status, ExpectedDate " +
                     $"FROM OrderLine, Requisition , Restaurant " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND Restaurant.RestaurantID = Requisition.RestaurantID " +
                     $"AND DeliveryNoteID is NULL " +
                     $"AND OrderLine.Status = 'Approved' ";

            string idInput = (txtSearch.Text.TrimStart(' ')).TrimStart('0');
            if (string.IsNullOrEmpty(idInput))
            {
                if (withRestaurantID)
                {
                    sqlStr += $"AND Restaurant.RestaurantID = '{cbPriority.Text}' ";
                }
                sqlStr += $"ORDER BY ExpectedDate"; ;
                sqlSelection(sqlStr, dt);
                dataGridView1.DataSource = dt;
            }
            else
            {
                idInput = string.Format("{0:000}", Convert.ToInt32(idInput));
                sqlStr += $" AND Requisition.RequisitionID = '{idInput}'";
                sqlSelection(sqlStr, dt);
                dataGridView1.DataSource = dt;
            }
            cleanUp();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dt.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    restaurantID = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    restaurantSelected = true;
                }
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)        // Create Delivery Note button
        {
            if (restaurantSelected)
            {
                WCreateDeliveryNote createPage = new WCreateDeliveryNote(restaurantID);
                this.Hide();
                createPage.ShowDialog();
                this.Show();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cleanUp();
        }

        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////

        private string[] fillcomboBoxes(string col_name, string table)
        {
            DataTable dt = new DataTable();
            sqlStr = $"SELECT {col_name} FROM {table} ORDER BY RestaurantID";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
            string[] listItem = new string[dt.Rows.Count];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                listItem[i] = dt.Rows[i][col_name].ToString();
            }
            dt.Clear();
            return listItem;
        }

        private void cleanUp()
        {
            txtSearch.Text = "";
            cbPriority.Text = "";
            cbPriority.SelectedIndex = -1;
            comboBox1.Text = "";
            comboBox1.SelectedIndex = -1;
            withRestaurantID = false;
            restaurantSelected = false;
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        ////////////////////////////////////////  Property  ////////////////////////////////////////////////////

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

        private void cbPriority_SelectedIndexChanged(object sender, EventArgs e)        // Restaurant ID
        {
            withRestaurantID = true;
            if (cbPriority.SelectedIndex > -1)
            {
                withRestaurantID = true;
                comboBox1.Text = comboBox1.Items[cbPriority.SelectedIndex].ToString();
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)         // Restaurant Name
        {
            if (comboBox1.SelectedIndex > -1)
            {
                withRestaurantID = true;
                cbPriority.Text = cbPriority.Items[comboBox1.SelectedIndex].ToString();
            }
        }

        
    }
}
