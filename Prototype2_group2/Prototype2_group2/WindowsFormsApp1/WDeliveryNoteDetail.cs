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

namespace WindowsFormsApp1
{
    public partial class WDeliveryNoteDetail : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dtNoteLines = new DataTable();

        WDeliveryNote previousForm;
        string deliveryNoteID;
        string restaurantID;

        public WDeliveryNoteDetail()
        {
            InitializeComponent();
        }

        public WDeliveryNoteDetail(WDeliveryNote previousForm, string deliveryNoteID, string restaurantID)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.deliveryNoteID = deliveryNoteID;
            this.restaurantID = restaurantID;
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

        private void WDeliveryNoteDetail_Load(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM DeliveryNoteLines_tmp WHERE OrderLineID is not NULL";
            sqlExecution(sqlStr);

            DataTable dt = new DataTable();
            sqlStr = $"SELECT DeliveryNoteDate, RegistrationPlateID, ContactNo FROM DeliveryNote " +
                     $"WHERE DeliveryNoteID = '{deliveryNoteID}'";
            sqlSelection(sqlStr, dt);

            textBox3.Text = deliveryNoteID;
            textBox1.Text = dt.Rows[0]["DeliveryNoteDate"].ToString();
            textBox2.Text = dt.Rows[0]["RegistrationPlateID"].ToString();
            textBox4.Text = dt.Rows[0]["ContactNo"].ToString();

            sqlStr = $"SELECT OrderLineID, Item.ItemID, ItemName, Category, Subcategory, Unit, Quantity, QuantityReceived, Status " +
                     $"FROM OrderLine, Item " +
                     $"WHERE OrderLine.ItemID = Item.ItemID " +
                     $"AND DeliveryNoteID = '{deliveryNoteID}' ";
            sqlSelection(sqlStr, dtNoteLines);

            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            for (int i = 0; i < dtNoteLines.Rows.Count; i++)
            {
                sqlStr = $"INSERT INTO DeliveryNoteLines_tmp " +
                         $"VALUES('{dtNoteLines.Rows[i]["OrderLineID"]}', '{dtNoteLines.Rows[i]["ItemID"]}', " +
                                $"'{dtNoteLines.Rows[i]["ItemName"]}', '{dtNoteLines.Rows[i]["Category"]}', " +
                                $"'{dtNoteLines.Rows[i]["Subcategory"]}', '{dtNoteLines.Rows[i]["Unit"]}', " +
                                $"'{dtNoteLines.Rows[i]["Quantity"]}', '{dtNoteLines.Rows[i]["QuantityReceived"]}', " +
                                $"'{dtNoteLines.Rows[i]["Status"]}')";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
            dataGridView1.DataSource = dtNoteLines;
        }


        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // UPDATE Cell Button
        {
            if (dtNoteLines.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string str = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                    if (int.TryParse(str, out int qty))
                    {
                        if (str.Equals(dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString()))   // Qty == QtyReceived
                            sqlStr = $"UPDATE DeliveryNoteLines_tmp SET QuantityReceived = '{qty}', Status = 'Received' " +
                                     $"WHERE OrderLineID = '{id}'";
                        else
                            sqlStr = $"UPDATE DeliveryNoteLines_tmp SET QuantityReceived = '{qty}', Status = 'Delivering' " +
                                     $"WHERE OrderLineID = '{id}'";
                        sqlExecution(sqlStr);
                        fillDataGridView1();
                    }
                    else
                    {
                        MessageBox.Show("Please input a numeric value for Quantity Received.");
                        fillDataGridView1();
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)      // UPDATE Button
        {
            for (int i = 0; i < dtNoteLines.Rows.Count; i++)
            {
                string id = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string str = dataGridView1.Rows[i].Cells[8].Value.ToString();
                if (int.TryParse(str, out int qty))
                {
                    if (str.Equals(dataGridView1.Rows[i].Cells[7].Value.ToString()))   // Qty == QtyReceived
                        sqlStr = $"UPDATE DeliveryNoteLines_tmp SET QuantityReceived = '{qty}', Status = 'Received' " +
                                 $"WHERE OrderLineID = '{id}'";
                    else
                        sqlStr = $"UPDATE DeliveryNoteLines_tmp SET QuantityReceived = '{qty}', Status = 'Delivering' " +
                                 $"WHERE OrderLineID = '{id}'";
                    sqlExecution(sqlStr);
                }
                else
                {
                    MessageBox.Show($"Order ID: {id}, please input a numeric value for Quantity Received.");
                }
            }
            fillDataGridView1();
        }

        private void button4_Click(object sender, EventArgs e)      // Confirm
        {
            Boolean completed = true;

            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            for (int i = 0; i < dtNoteLines.Rows.Count; i++)
            {
                string id = dataGridView1.Rows[i].Cells[1].Value.ToString();
                string qty = dataGridView1.Rows[i].Cells[8].Value.ToString();
                string status = dataGridView1.Rows[i].Cells[9].Value.ToString();

                if(!status.Equals("Received"))
                    completed = false;

                sqlStr = $"UPDATE OrderLine SET QuantityReceived = '{qty}', Status = '{status}' WHERE OrderLineID = '{id}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            sqlStr = $"DELETE FROM DeliveryNoteLines_tmp WHERE OrderLineID is not NULL";
            command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();

            if (completed)
            {
                DataTable dt = new DataTable();
                sqlStr = $"SELECT Name FROM DeliveryNote, Restaurant, Account, Employee " +
                         $"WHERE DeliveryNote.RestaurantID = Restaurant.RestaurantID " +
                         $"AND Restaurant.UserID = Account.UserID " +
                         $"AND Account.EmployeeID = Employee.EmployeeID " +
                         $"AND DeliveryNote.RestaurantID = '{restaurantID}'";
                sqlSelection(sqlStr, dt);
                string signature = dt.Rows[0]["Name"].ToString();

                sqlStr = $"UPDATE DeliveryNote SET RestaurantSignature = '{signature}' WHERE DeliveryNoteID = '{deliveryNoteID}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            else
            {
                sqlStr = $"UPDATE DeliveryNote SET RestaurantSignature = NULL WHERE DeliveryNoteID = '{deliveryNoteID}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();

            previousForm.showEditedDeliveryNote();
            this.Close();
        }

        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////
        private void fillDataGridView1()
        {
            dtNoteLines.Clear();
            sqlStr = "SELECT * FROM DeliveryNoteLines_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtNoteLines);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dtNoteLines;
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM DeliveryNoteLines_tmp WHERE OrderLineID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

    }
}
