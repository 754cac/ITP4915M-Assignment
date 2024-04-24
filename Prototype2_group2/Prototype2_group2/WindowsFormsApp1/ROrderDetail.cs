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
    public partial class form4 : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dtOrderDetail = new DataTable();

        restOrderList previousForm;
        string requisitionID;
        string restaurantID;

        int del_count = 0;
        string [] deletedItem;
        string preDelectedItem;

        public form4()
        {
            InitializeComponent();
        }

        public form4(restOrderList previousForm, string requisitionID, string restaurantID)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.requisitionID = requisitionID;
            this.restaurantID = restaurantID;
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

        private void form4_Load(object sender, EventArgs e)
        {
            fillOrderDetail();
            button2.Hide();
        }

        //////////////////////////////////////////  Event Handler  ///////////////////////////////////////////
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // DELETE / UPDATE
        {
            if (dtOrderDetail.Rows.Count > 0 && e.RowIndex > -1) {
                string status = dataGridView1.Rows[e.RowIndex].Cells[10].Value.ToString();
                if (e.ColumnIndex == 0)      // DELETE
                {
                    if (status.Equals("Rejected") || status.Equals("Processing"))
                    {
                        DialogResult response = MessageBox.Show("Are you sure to delete this item?", "Confirm Delete!", MessageBoxButtons.YesNo);
                        if(response == DialogResult.Yes)
                        {
                            string id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                            sqlStr = $"DELETE FROM OrderDetail WHERE OrderLineID = '{id}'";
                            sqlExecution(sqlStr);
                            fillDataGridView1();
                            deletedItem[del_count++] = preDelectedItem = id;
                            button2.Show();
                        }
                    }
                    else
                    {
                        MessageBox.Show($"The order which is {status} cannot be deleted!");
                    }
                }
                else if (e.ColumnIndex == 1) // UPDATE
                {
                    string id = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    if (status.Equals("Processing") || status.Equals("Rejected")) {
                        string str = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();
                        if (int.TryParse(str, out int qty)) {
                            if(status.Equals("Processing"))
                                sqlStr = $"UPDATE OrderDetail SET Quantity = '{qty}' WHERE OrderLineID = '{id}'";
                            else if(status.Equals("Rejected"))
                                sqlStr = $"UPDATE OrderDetail SET Quantity = '{qty}', Status = 'Processing' WHERE OrderLineID = '{id}'";
                            sqlExecution(sqlStr);
                            fillDataGridView1();
                        }
                        else {
                            MessageBox.Show("Please input a numeric value for Quantity");
                            fillDataGridView1();
                        }
                    }
                    else                        // Approved
                    {
                        string str = dataGridView1.Rows[e.RowIndex].Cells[9].Value.ToString();
                        if (int.TryParse(str, out int receivedQty))
                        {
                            sqlStr = $"UPDATE OrderDetail SET QuantityReceived = '{receivedQty}' WHERE OrderLineID = '{id}'";
                            sqlExecution(sqlStr);
                            fillDataGridView1();
                        }
                        else
                        {
                            MessageBox.Show("Please input a numeric value for Quantity Received");
                            fillDataGridView1();
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)          // Undo button
        {
           DataTable dt = new DataTable();
           sqlStr = $"SELECT OrderLineID, Item.ItemID, ItemName, Category, Subcategory, Unit, Quantity, QuantityReceived, Status " +
                    $"From OrderLine, Item WHERE Item.ItemID = OrderLine.ItemID AND RequestID = '{requisitionID}' " +
                    $"AND OrderLineID = '{preDelectedItem}'";
            sqlSelection(sqlStr, dt);

            sqlStr = $"INSERT INTO OrderDetail " +
                     $"VALUES('{dt.Rows[0]["OrderLineID"]}', '{dt.Rows[0]["ItemID"]}', '{dt.Rows[0]["ItemName"]}', " +
                            $"'{dt.Rows[0]["Category"]}', '{dt.Rows[0]["Subcategory"]}', '{dt.Rows[0]["Unit"]}', " +
                            $"'{dt.Rows[0]["Quantity"]}', '{dt.Rows[0]["QuantityReceived"]}', '{dt.Rows[0]["Status"]}')";
            sqlExecution(sqlStr);
            fillDataGridView1();
            button2.Hide();
            del_count--;
        }

        private void button4_Click(object sender, EventArgs e)      // Comfirm button
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            if (dtOrderDetail.Rows.Count <= 0)
            {
                for (int i = 0; i < del_count; i++)
                {
                    sqlStr = $"DELETE FROM OrderLine WHERE OrderLineID = '{deletedItem[i]}'";
                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();
                }
                sqlStr = $"DELETE FROM Requisition WHERE RequisitionID = '{requisitionID}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            else
            {
                for (int i = 0; i < dtOrderDetail.Rows.Count; i++)
                {
                    string orderlineID = dtOrderDetail.Rows[i]["OrderLineID"].ToString(); 
                    string quantity = dtOrderDetail.Rows[i]["Quantity"].ToString();
                    string quantityReceived = dtOrderDetail.Rows[i]["QuantityReceived"].ToString();
                    string status = dtOrderDetail.Rows[i]["Status"].ToString();
                    sqlStr = $"UPDATE OrderLine SET Quantity = '{quantity}' " +
                                        $", QuantityReceived = '{quantityReceived}' " +
                                                  $", Status = '{status}' WHERE OrderLineID = '{orderlineID}'";
                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();
                }

                for (int i = 0; i < del_count; i++)
                {
                    sqlStr = $"DELETE FROM OrderLine WHERE OrderLineID = '{deletedItem[i]}'";
                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();
                }
                sqlStr = $"UPDATE Requisition SET Status = 'Processing' WHERE RequisitionID = '{requisitionID}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            sqlStr = $"DELETE FROM OrderDetail WHERE OrderLineID is not NULL";
            command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();
            connection.Close();

            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestaurantID, Status FROM Requisition " +
                     $"WHERE RequisitionID = '{requisitionID}'";
            previousForm.showEditedRequisition(sqlStr);

            this.Close();
        }

        //////////////////////////////////////////  Own Method  ///////////////////////////////////////////

        private void fillOrderDetail() {
            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestAddress FROM Requisition, Restaurant " +
                     $"WHERE Restaurant.RestaurantID = Requisition.RestaurantID " +
                     $"AND RequisitionID = '{requisitionID}' AND Restaurant.RestaurantID = '{restaurantID}'";
            sqlSelection(sqlStr, dtOrderDetail);

            textBox3.Text = requisitionID;
            textBox1.Text = dtOrderDetail.Rows[0]["RequestDate"].ToString();
            textBox2.Text = dtOrderDetail.Rows[0]["RestAddress"].ToString();
            textBox4.Text = Convert.ToDateTime(dtOrderDetail.Rows[0]["ExpectedDate"]).ToShortDateString();

            dtOrderDetail = new DataTable();

            sqlStr = $"SELECT OrderLineID, Item.ItemID, ItemName, Category, Subcategory, Unit, Quantity, QuantityReceived, Status " +
                     $"From OrderLine, Item WHERE Item.ItemID = OrderLine.ItemID AND RequestID = '{requisitionID}'";
            sqlSelection(sqlStr, dtOrderDetail);
            deletedItem = new string[dtOrderDetail.Rows.Count];

            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            for (int i = 0; i < dtOrderDetail.Rows.Count; i++)
            {
                sqlStr = $"INSERT INTO OrderDetail " +
                         $"VALUES('{dtOrderDetail.Rows[i]["OrderLineID"]}', '{dtOrderDetail.Rows[i]["ItemID"]}', '{dtOrderDetail.Rows[i]["ItemName"]}', " +
                                $"'{dtOrderDetail.Rows[i]["Category"]}', '{dtOrderDetail.Rows[i]["Subcategory"]}', '{dtOrderDetail.Rows[i]["Unit"]}', " +
                                $"'{dtOrderDetail.Rows[i]["Quantity"]}', '{dtOrderDetail.Rows[i]["QuantityReceived"]}', '{dtOrderDetail.Rows[i]["Status"]}')";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
            dataGridView1.DataSource = dtOrderDetail;
        }

        private void fillDataGridView1()
        {
            dtOrderDetail.Clear();
            sqlStr = "SELECT * FROM OrderDetail";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtOrderDetail);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dtOrderDetail;
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

        //////////////////////////////////////////  Property  ///////////////////////////////////////////
        private void btnExit_Click(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM OrderDetail WHERE OrderLineID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)      // Cancer button
        {
            sqlStr = $"DELETE FROM OrderDetail WHERE OrderLineID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        
    }
}
