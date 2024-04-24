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
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class pmLine : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();
        DataTable dtPurchaseLines = new DataTable();

        pmMain1 previousForm;
        string purchaseID = "005";

        public pmLine()
        {
            InitializeComponent();
        }
        public pmLine(pmMain1 previousForm, string purchaseID)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.purchaseID = purchaseID;
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
        private void pmLine_Load(object sender, EventArgs e)
        {
            ////////////////////// Basic information //////////////////////
            dt = new DataTable();
            sqlStr = $"SELECT * FROM Purchase WHERE PurchaseID = '{purchaseID}'";
            sqlSelection(sqlStr, dt);
            textBox1.Text = dt.Rows[0]["PurchaseID"].ToString();

            string type = dt.Rows[0]["ReleaseType"].ToString();
            if (type == "BPR") type = "Blanket Purchase Release";
            else if (type == "PPO") type = "Planned Purchase Order";
            else if (type == "SPO") type = "Standard Purchase Order";
            textBox2.Text = type;

            string destination = dt.Rows[0]["AddressType"].ToString();
            textBox3.Text = destination;
            textBox4.Text = dt.Rows[0]["AddressID"].ToString();
            textBox5.Text = dt.Rows[0]["PurchaseDate"].ToString();
            textBox6.Text = dt.Rows[0]["ExpectedDate"].ToString();

            string col_name = "";
            if (destination.Equals("Warehouse")) col_name = "WareName";
            if (destination.Equals("Restaurant")) col_name = "RestName";
            sqlStr = $"SELECT {col_name} FROM {destination} WHERE "+destination+$"ID = '{dt.Rows[0]["AddressID"].ToString()}'";
            dt = new DataTable();
            sqlSelection(sqlStr, dt);
            
            textBox7.Text = dt.Rows[0][0].ToString();
            ///////////////////////////////////// purchase lines /////////////////////////
            dt = new DataTable();
            sqlStr = $"SELECT * FROM PurchaseLine WHERE PurchaseID = '{purchaseID}'";
            sqlSelection(sqlStr, dt);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sqlStr = $"INSERT INTO PurchaseLineDetails_tmp " +
                             $"VALUES('{dt.Rows[i]["PurchaseLineID"].ToString()}', " +
                             $"'{dt.Rows[i]["AgreementID"].ToString()}', " +
                             $"'{dt.Rows[i]["SupplierID"].ToString()}', " +
                             $"'{dt.Rows[i]["ItemID"].ToString()}', " +
                             $"'{dt.Rows[i]["Price"].ToString()}', " +
                             $"'{dt.Rows[i]["Quantity"].ToString()}', " +
                             $"'{dt.Rows[i]["QuantityReceived"].ToString()}')";
                    sqlExecution(sqlStr);
                }
            }
            fillDataGridView1();
        }


        //////////////////////////////////////////  Event Handler  ///////////////////////////////////////////
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // PurchaseLines View
        {
            if (dtPurchaseLines.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)             // UPDATE
                {
                    string qty = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
                    if (string.IsNullOrWhiteSpace(qty))
                    {
                        MessageBox.Show("Quantity cannot be empty, please input a number");
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "";
                    }
                    else if (!int.TryParse(qty, out int num))
                    {
                        MessageBox.Show("Please input a numeric value for the Quantity textbox");
                        dataGridView1.Rows[e.RowIndex].Cells[7].Value = "";
                    }
                    else
                    {
                        sqlStr = $"UPDATE PurchaseLineDetails_tmp SET Quantity = '{qty}' " +
                                 $"WHERE PurchaseLineID = '{dataGridView1.Rows[e.RowIndex].Cells[2].Value}'";
                        sqlExecution(sqlStr);
                        fillDataGridView1();
                    }

                }
                else if (e.ColumnIndex == 1)        // MORE
                {
                    string supplierID = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    DataTable dt = new DataTable();
                    sqlStr = $"SELECT SName, Phone FROM Supplier WHERE SupplierID = '{supplierID}'";
                    sqlSelection(sqlStr, dt);
                    textBox8.Text = supplierID;
                    textBox9.Text = dt.Rows[0]["SName"].ToString();
                    textBox10.Text = dt.Rows[0]["Phone"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)      // Submit button
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            for (int i = 0; i < dtPurchaseLines.Rows.Count; i++)
            {
                string quantity = dtPurchaseLines.Rows[i]["Quantity"].ToString();
                sqlStr = $"UPDATE PurchaseLine SET Quantity = '{quantity}' " +
                         $"WHERE PurchaseLineID = '{dtPurchaseLines.Rows[i]["PurchaseLineID"]}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            sqlStr = $"DELETE FROM PurchaseLineDetails_tmp WHERE PurchaseLineID is not NULL";
            command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();
            connection.Close();

            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestaurantID, Status FROM Requisition " +
                     $"WHERE RequisitionID = ''";
            this.Close();
        }



        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////

        private void fillDataGridView1()
        {
            dtPurchaseLines.Clear();
            sqlStr = "SELECT * FROM PurchaseLineDetails_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtPurchaseLines);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dtPurchaseLines;
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






        private void btnSearch_Click(object sender, EventArgs e)       // Back Button
        {
            sqlStr = $"DELETE FROM PurchaseLineDetails_tmp WHERE PurchaseLineID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        
    }
}
