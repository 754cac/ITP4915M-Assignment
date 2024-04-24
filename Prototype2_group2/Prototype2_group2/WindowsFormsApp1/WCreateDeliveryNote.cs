using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class WCreateDeliveryNote : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        DataTable dtOrderLines = new DataTable();
        DataTable dtNoteLines = new DataTable();

        string restuarantID = "";
        string orderLineID = "";

        Boolean orderSelected = false;
        Boolean withDate = false;

        public WCreateDeliveryNote()
        {
            InitializeComponent();
        }

        public WCreateDeliveryNote(string restaurantID)
        {
            InitializeComponent();
            this.restuarantID = restaurantID;
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

        private void WCreateDeliveryNote_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT * FROM Restaurant WHERE RestaurantID = '{restuarantID}'";
            sqlSelection(sqlStr, dt);
            textBox2.Text = dt.Rows[0]["RestaurantID"].ToString();
            textBox3.Text = dt.Rows[0]["RestName"].ToString();
            textBox5.Text = dt.Rows[0]["RestAddress"].ToString();

            sqlStr = $"SELECT OrderLineID, Restaurant.RestaurantID, Item.ItemID, itemName, Quantity, OrderLine.Status, ExpectedDate " +
                     $"FROM OrderLine, Requisition , Restaurant, Item " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND Restaurant.RestaurantID = Requisition.RestaurantID " +
                     $"AND Item.ItemID = OrderLine.ItemID " +
                     $"AND DeliveryNoteID is NULL " +
                     $"AND OrderLine.Status = 'Approved' " +
                     $"AND Restaurant.RestaurantID = '{restuarantID}' " +
                     $"ORDER BY ExpectedDate ";
            sqlSelection(sqlStr, dtOrderLines);
            dataGridView1.DataSource = dtOrderLines;

            dtpReqDate.CustomFormat = " ";
            dtpReqDate.Format = DateTimePickerFormat.Custom;

            string[] listItem = fillcomboBoxes("RegistrationPlateID");
            for (int i = 0; i < listItem.Length; i++)
                comboBox1.Items.Add(listItem[i]);
        }

        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // Approved Orderlines
        {
            if (dtOrderLines.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    orderLineID = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    orderSelected = true;
                }
            }
        }

        private void btnADD_Click(object sender, EventArgs e)
        {
            if (orderSelected)
            {
                dt = new DataTable();
                sqlStr = $"SELECT * FROM DeliveryNoteCreation_tmp WHERE OrderLineID = '{orderLineID}'";
                sqlSelection(sqlStr, dt);
                if (dt.Rows.Count == 0)
                {
                    dt = new DataTable();
                    sqlStr = $"SELECT OrderLineID, Restaurant.RestaurantID, Item.ItemID, itemName, Unit, Quantity, OrderLine.Status, ExpectedDate " +
                             $"FROM OrderLine, Requisition , Restaurant, Item " +
                             $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                             $"AND Restaurant.RestaurantID = Requisition.RestaurantID " +
                             $"AND Item.ItemID = OrderLine.ItemID " +
                             $"AND OrderLineID = '{orderLineID}' ";
                    sqlSelection(sqlStr, dt);

                    string orderID = dt.Rows[0]["OrderLineID"].ToString();
                    string restID = dt.Rows[0]["RestaurantID"].ToString();
                    string itemID = dt.Rows[0]["ItemID"].ToString();
                    string itemName = dt.Rows[0]["ItemName"].ToString();
                    string unit = dt.Rows[0]["Unit"].ToString();
                    string qty = dt.Rows[0]["Quantity"].ToString();
                    string status = dt.Rows[0]["Status"].ToString();
                    string date = dt.Rows[0]["ExpectedDate"].ToString();

                    sqlStr = $"INSERT INTO DeliveryNoteCreation_tmp VALUES('{orderID}', '{restID}', '{itemID}', '{itemName}', " +
                                                                         $"'{unit}',  '{qty}',  '{status}',  '{date}')";
                    sqlExecution(sqlStr);
                    fillDataGridView2();
                }
                else MessageBox.Show("This record already exists.");
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtNoteLines.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    sqlStr = $"DELETE FROM DeliveryNoteCreation_tmp WHERE OrderLineID = '{dataGridView2.Rows[e.RowIndex].Cells[1].Value}'";
                    sqlExecution(sqlStr);
                    fillDataGridView2();
                }
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (!withDate)
                MessageBox.Show("Delivery Date cannot be empty.");
            else if (comboBox1.SelectedIndex < 0)
                MessageBox.Show("Registration Plate ID cannot be empty.");
            else if (String.IsNullOrEmpty(textBox7.Text.TrimStart(' ')))
            {
                MessageBox.Show("Contact Number cannot be empty.");
                textBox7.Text = "";
            }
            else
            {
                if (dtNoteLines.Rows.Count > 0)
                {
                    string deliveryNoteID = getID("DeliveryNoteID", "DeliveryNote");
                    string date = dtpReqDate.Value.ToShortDateString();
                    string rPlateID = comboBox1.Text.ToString();
                    string contactNum = textBox7.Text.Trim();

                    sqlStr = $"INSERT INTO DeliveryNote VALUES('{deliveryNoteID}', '{date}', '{restuarantID}', '{rPlateID}', '{contactNum}', NULL, 1)";
                    sqlExecution(sqlStr);

                    OleDbConnection connection = new OleDbConnection(connStr);
                    OleDbCommand command;
                    connection.Open();
                    for (int i = 0; i < dtNoteLines.Rows.Count; i++)
                    {
                        sqlStr = $"UPDATE OrderLine SET Status = 'Delivering', DeliveryNoteID = '{deliveryNoteID}' WHERE OrderLineID = '{dtNoteLines.Rows[i]["OrderLineID"]}'";
                        command = new OleDbCommand(sqlStr, connection);
                        command.ExecuteNonQuery();
                    }
                    connection.Close();

                    sqlStr = $"DELETE FROM DeliveryNoteCreation_tmp WHERE OrderLineID is not NULL";
                    sqlExecution(sqlStr);

                    

                    this.Close();
                }
                
            }
        }

        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////
        private string[] fillcomboBoxes(string col_name)
        {
            DataTable dt_tmp = new DataTable();
            sqlStr = $"SELECT DISTINCT {col_name} FROM DeliveryNote";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt_tmp);
            dataAdapter.Dispose();
            string[] listItem = new string[dt_tmp.Rows.Count];
            for (int i = 0; i < dt_tmp.Rows.Count; i++)
            {
                listItem[i] = dt_tmp.Rows[i][col_name].ToString();
            }
            dt_tmp.Clear();
            return listItem;
        }

        private void fillDataGridView2()
        {
            dtNoteLines.Clear();
            sqlStr = "SELECT * FROM DeliveryNoteCreation_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtNoteLines);
            dataAdapter.Dispose();
            dataGridView2.DataSource = dtNoteLines;
        }

        private string getID(string tid, string tName)
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            sqlStr = $"SELECT MAX({tid}) FROM {tName}";
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            string id = command.ExecuteScalar().ToString();
            connection.Close();

            return string.Format("{0:000}", int.Parse(id) + 1);
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }
        private void sqlExecution(string sql)
        {
            string sqlStr = sql;
            OleDbConnection connection = new OleDbConnection(connStr);
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();
            connection.Close();
        }


        ////////////////////////////////////////  Property  ////////////////////////////////////////////////////

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM DeliveryNoteCreation_tmp WHERE OrderLineID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dtpReqDate_ValueChanged(object sender, EventArgs e)
        {
            withDate = true;
            dtpReqDate.CustomFormat = "yyyy/MM/dd";
            if (dtpReqDate.Value < DateTime.Today)
                dtpReqDate.Value = DateTime.Today;
        }

       
    }
}
