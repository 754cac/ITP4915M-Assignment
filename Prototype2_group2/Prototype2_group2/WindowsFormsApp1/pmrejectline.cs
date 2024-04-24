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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class pmrejectline : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"    
                       + "Data Source=database.mdb";
        string sqlStr = "";

        DataTable dtRejected = new DataTable();
        DataTable dtAgreement = new DataTable();
        DataTable dtPurchaseLines = new DataTable();

        Boolean agreement_B = false;
        Boolean agreement_C = false;
        Boolean agreement_P = false;
        Boolean agreementSelected = false;
        Boolean locked = false;
        Boolean withDate = false;

        int rowIndex;
        int count_PLine = 0;

        public pmrejectline()
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

        private void pmrejectline_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT OrderLineID, ItemID, Quantity, OrderLine.Status, ExpectedDate " +
                     $"FROM OrderLine, Requisition " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND OrderLine.Status = 'Rejected' " +
                     $"ORDER BY ExpectedDate";
            sqlSelection(sqlStr, dtRejected);
            dataGridView1.DataSource = dtRejected;

            sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                     $"Price, PromisedQuantity, OrderedQuantity, ConExpiryDate " +
                     $"FROM Item, Contract, ContractLine " +
                     $"WHERE Item.ItemID = ContractLine.ItemID " +
                     $"AND Contract.AgreementID = ContractLine.AgreementID " +
                     $"ORDER BY AgreementType ";
            fillDataGridView2(sqlStr, dtAgreement);

            dtpReqDate.CustomFormat = " ";
            dtpReqDate.Format = DateTimePickerFormat.Custom;
        }

        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // BPA
        {
            if (dtRejected.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 1 && !(agreement_C || agreement_P))
                {
                    string itemID = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    textBox1.Text = itemID;
                    cbStatus.Text = cbStatus.Items[0].ToString();
                    sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                             $"Price, PromisedQuantity, OrderedQuantity, ConExpiryDate " +
                             $"FROM Item, Contract, ContractLine " +
                             $"WHERE Item.ItemID = ContractLine.ItemID " +
                             $"AND Contract.AgreementID = ContractLine.AgreementID " +
                             $"AND AgreementType = 'BPA' " +
                             $"AND Item.ItemID = '{itemID}' ";
                    fillDataGridView2(sqlStr, dtAgreement);
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                     $"Price, PromisedQuantity, OrderedQuantity, ConExpiryDate " +
                     $"FROM Item, Contract, ContractLine " +
                     $"WHERE Item.ItemID = ContractLine.ItemID " +
                     $"AND Contract.AgreementID = ContractLine.AgreementID ";

            if(cbStatus.SelectedIndex == 0)     // BPA
            {
                sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                         $"Price, PromisedQuantity, OrderedQuantity, ConExpiryDate " +
                         $"FROM Item, Contract, ContractLine " +
                         $"WHERE Item.ItemID = ContractLine.ItemID " +
                         $"AND Contract.AgreementID = ContractLine.AgreementID " +
                         $"AND AgreementType = 'BPA' ";
            }
            else if (cbStatus.SelectedIndex == 1)   // PPA
            {
                sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                         $"Price, PromisedQuantity, ConExpiryDate " +
                         $"FROM Item, Contract, ContractLine " +
                         $"WHERE Item.ItemID = ContractLine.ItemID " +
                         $"AND Contract.AgreementID = ContractLine.AgreementID " +
                         $"AND AgreementType = 'PPA' ";
            }
            else if (cbStatus.SelectedIndex == 2)   // CPA
            {
                sqlStr = $"SELECT Contract.AgreementID, AgreementType, Item.ItemID, ItemName, Unit, " +
                         $"Price, ConExpiryDate " +
                         $"FROM Item, Contract, ContractLine " +
                         $"WHERE Item.ItemID = ContractLine.ItemID " +
                         $"AND Contract.AgreementID = ContractLine.AgreementID " +
                         $"AND AgreementType = 'CPA' ";
            }

            string idInput = (textBox1.Text.TrimStart(' ')).TrimStart('0');

            if(int.TryParse(idInput, out int num))
            {
                sqlStr += $" AND Item.ItemID = '{string.Format("{0:000}", Convert.ToInt32(idInput))}' ";
            }
            else
            {
                sqlStr += $" AND ItemName LIKE '%{idInput}%' ";
            }
            sqlStr += $" ORDER BY AgreementType ";
            fillDataGridView2(sqlStr, dtAgreement);
            cleanUp();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)   // Agreement View
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    rowIndex = Convert.ToInt32(dataGridView2.CurrentRow.Index);
                    string agreementType = dtAgreement.Rows[rowIndex]["AgreementType"].ToString();
                    cleanUpAgreementBoolean();
                    if (agreementType.Equals("BPA")) agreement_B  = true;
                    else if (agreementType.Equals("CPA")) agreement_C = true;
                    else if (agreementType.Equals("PPA")) agreement_P = true;
                    agreementSelected = true;
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)      // ADD button
        {
            string input = textBox2.Text;       // Quantity
            int num;

            if (!agreementSelected)
                MessageBox.Show("Please select an agreement first");
            else if (string.IsNullOrWhiteSpace(input))
                MessageBox.Show("Quantity cannot be empty, please input a number");
            else if (!int.TryParse(input, out num))
                MessageBox.Show("Please input a numeric value for the Quantity textbox");
            else
            {
                addPurchaseLine(num);
                fillDataGridView3();
                agreementSelected = false;
            }
            textBox2.Text = "";
        }

        private void dataGridView3_CellContentClick(object sender, DataGridViewCellEventArgs e) // PurchaseLine Table
        {
            if (dtPurchaseLines.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)             // UPDATE
                {
                    string input = dataGridView3.Rows[e.RowIndex].Cells[12].Value.ToString();    // Quantity
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        MessageBox.Show("Quantity cannot be empty, please input a number");
                        dataGridView3.Rows[e.RowIndex].Cells[12].Value = "";
                    }
                    else if (!int.TryParse(input, out int num))
                    {
                        MessageBox.Show("Please input a numeric value for the Quantity textbox");
                        dataGridView3.Rows[e.RowIndex].Cells[12].Value = "";
                    }
                    else
                    {
                        sqlStr = $"UPDATE PurchaseLine_tmp SET Quantity = '{input}' " +
                                 $"WHERE AgreementID = '{dataGridView3.Rows[e.RowIndex].Cells[2].Value}' " +
                                 $" AND ItemID = '{dataGridView3.Rows[e.RowIndex].Cells[6].Value}'";
                        sqlExecution(sqlStr);
                        fillDataGridView3();
                    }
                }
                else if (e.ColumnIndex == 1)        // DELETE
                {
                    sqlStr = $"DELETE FROM PurchaseLine_tmp " +
                             $"WHERE AgreementID = '{dataGridView3.Rows[e.RowIndex].Cells[2].Value}' " +
                             $" AND ItemID = '{dataGridView3.Rows[e.RowIndex].Cells[6].Value}'";
                    sqlExecution(sqlStr);
                    fillDataGridView3();

                    count_PLine--;
                    if(count_PLine == 0)
                    {
                        locked = false;
                        cleanUpAgreementBoolean();
                        cbStatus.Enabled = true;
                        cbStatus.Text = "";
                        cbStatus.SelectedIndex = -1;
                        label4.Text = "Release: ";
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)      // SUBMIT
        {
            if (!withDate)
                MessageBox.Show("Delivery Date cannot be empty.");
            else if (comboBox2.SelectedIndex < 0)
                MessageBox.Show("Destination ID cannot be empty.");
            else {
                if (locked)
                {
                    string purchaseID = getID("PurchaseID", "Purchase");
                    string type = "";
                    if (agreement_B) type = "BPR";
                    else if (agreement_C) type = "SPO";
                    else if (agreement_P) type = "PPO";
                    string addressType = comboBox1.Text.ToString();
                    string addressID = comboBox2.Text.ToString();
                    string today = DateTime.Today.ToShortDateString();
                    string date = dtpReqDate.Value.ToShortDateString();

                    string pLineID = getID("PurchaseLineID", "PurchaseLine");
                    
                    sqlStr = $"INSERT INTO Purchase VALUES('{purchaseID}', '{type}', '{addressType}', '{addressID}', '{today}', '{date}', '1')";
                    sqlExecution(sqlStr);

                    OleDbConnection connection = new OleDbConnection(connStr);
                    OleDbCommand command;
                    connection.Open();
                    for (int i = 0; i < dtPurchaseLines.Rows.Count; i++)
                    {
                        string agreementID = dtPurchaseLines.Rows[i]["AgreementID"].ToString();
                        string supplierID = dtPurchaseLines.Rows[i]["SupplierID"].ToString();
                        string itemID = dtPurchaseLines.Rows[i]["ItemID"].ToString();
                        string price = dtPurchaseLines.Rows[i]["Price"].ToString();
                        string qty = dtPurchaseLines.Rows[i]["Quantity"].ToString();

                        sqlStr = $"INSERT INTO PurchaseLine " +
                                 $"VALUES('{pLineID}', '{purchaseID}', '{agreementID}', '{supplierID}', " +
                                        $"'{itemID}', '{price}', '{qty}', '0')";
                        command = new OleDbCommand(sqlStr, connection);
                        command.ExecuteNonQuery();

                        pLineID = string.Format("{0:000}", int.Parse(pLineID) + 1);
                    }
                    connection.Close();

                    sqlStr = $"DELETE FROM PurchaseLine_tmp WHERE AgreementID is not NULL";
                    sqlExecution(sqlStr);

                    //////////////////////////////////////////////  Wait for previousForm function  ///////////////////////////////////////////////////

                    this.Close();
                }
            }
        }

        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////

        private void fillDataGridView2(string sqlStr, DataTable dt)      // Related Agreement
        {
            dt.Clear();
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
            dataGridView2.DataSource = dt;
        }

        private void fillDataGridView3()
        {
            dtPurchaseLines.Clear();
            sqlStr = "SELECT * FROM PurchaseLine_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtPurchaseLines);
            dataAdapter.Dispose();
            dataGridView3.DataSource = dtPurchaseLines;
        }

        private void addPurchaseLine(int num)
        {
            string agreementID = dtAgreement.Rows[rowIndex]["AgreementID"].ToString();
            string agreementType = dtAgreement.Rows[rowIndex]["AgreementType"].ToString();
            DataTable dt = new DataTable();
            sqlStr = $"SELECT Supplier.SupplierID, SName FROM Contract, Supplier " +
                     $"WHERE Contract.SupplierID = Supplier.SupplierID ";
            sqlSelection(sqlStr, dt);
            string supplierID = dt.Rows[0]["SupplierID"].ToString();
            string supplierName = dt.Rows[0]["SName"].ToString();
            string itemID = dtAgreement.Rows[rowIndex]["ItemID"].ToString();
            string itemName = dtAgreement.Rows[rowIndex]["ItemName"].ToString();
            string unit = dtAgreement.Rows[rowIndex]["Unit"].ToString();
            string price = dtAgreement.Rows[rowIndex]["Price"].ToString();
            string promisedQty = dtAgreement.Rows[rowIndex]["PromisedQuantity"].ToString(); promisedQty = (promisedQty.Length > 0) ? $"'{promisedQty}'" : "NULL";
            string orderedQty = dtAgreement.Rows[rowIndex]["OrderedQuantity"].ToString(); orderedQty = (orderedQty.Length > 0) ? $"'{orderedQty}'" : "NULL";
            string qty = num.ToString();
            string date = dtAgreement.Rows[rowIndex]["ConExpiryDate"].ToString();
            
            if (count_PLine == 0)
            {
                locked = true;

                if (agreement_B)
                {
                    label4.Text = "Blanket Purchase Release: ";
                    cbStatus.Text = cbStatus.Items[0].ToString();
                    cbStatus.SelectedIndex = 0;
                    cbStatus.Enabled = false;
                }
                else if (agreement_C)
                {
                    label4.Text = "Standard Purchase Order: ";
                    cbStatus.Text = cbStatus.Items[2].ToString();
                    cbStatus.SelectedIndex = 2;
                    cbStatus.Enabled = false;
                }
                else if (agreement_P) {
                    label4.Text = "Planned Purchase Order: ";
                    cbStatus.Text = cbStatus.Items[1].ToString();
                    cbStatus.SelectedIndex = 1;
                    cbStatus.Enabled = false;
                }
                dtAgreement.Clear();
                dataGridView2.DataSource = dtAgreement;
            }
            for (int i = 0; i < dtPurchaseLines.Rows.Count; i++)
            {
                if (dtPurchaseLines.Rows[i]["AgreementID"].ToString() == agreementID && dtPurchaseLines.Rows[i]["ItemID"].ToString() == itemID)
                {
                    qty = (int.Parse(dtPurchaseLines.Rows[i]["Quantity"].ToString()) + int.Parse(qty)).ToString();
                    sqlStr = $"UPDATE PurchaseLine_tmp SET Quantity = '{qty}' WHERE AgreementID = '{agreementID}' AND ItemID = '{itemID}' ";
                    sqlExecution(sqlStr);
                    return;
                }
            }
            sqlStr = $"INSERT INTO PurchaseLine_tmp " +
                     $"VALUES('{agreementID}', '{agreementType}', '{supplierID}', '{supplierName}', '{itemID}', '{itemName}', " +
                            $"'{unit}', '{price}', {promisedQty}, {orderedQty}, '{qty}', '{date}')";
            sqlExecution(sqlStr);
            count_PLine++;
        }

        private string[] fillcomboBoxes(string col_name, string table_name, string orderBy)
        {
            DataTable dt_tmp = new DataTable();
            sqlStr = $"SELECT {col_name} FROM {table_name} ORDER BY {orderBy}";
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

        private void cleanUp()
        {
            if (!locked)
            {
                cbStatus.Text = "";
                cbStatus.SelectedIndex = -1;
            }
        }

        private void cleanUpAgreementBoolean()
        {
            agreement_B = false;
            agreement_C = false;
            agreement_P = false;
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

        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            sqlStr = $"DELETE FROM PurchaseLine_tmp WHERE AgreementID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }

        private void dtpReqDate_ValueChanged(object sender, EventArgs e)
        {
            withDate = true;
            dtpReqDate.CustomFormat = "yyyy/MM/dd";
            if (dtpReqDate.Value < DateTime.Today)
                dtpReqDate.Value = DateTime.Today;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)    // Destination Type
        {
            comboBox2.Items.Clear(); comboBox3.Items.Clear();
            comboBox2.ResetText(); comboBox3.ResetText();

            if (comboBox1.SelectedIndex == 0)   // Warehouse
            {
                string[] listItem = fillcomboBoxes("WarehouseID", "Warehouse", "WarehouseID");
                for (int i = 0; i < listItem.Length; i++)
                    comboBox2.Items.Add(listItem[i]);

                listItem = fillcomboBoxes("WareName", "Warehouse", "WarehouseID");
                for (int i = 0; i < listItem.Length; i++)
                    comboBox3.Items.Add(listItem[i]);
            }

            if (comboBox1.SelectedIndex == 1)   // Restaurant
            {
                string[] listItem = fillcomboBoxes("RestaurantID", "Restaurant", "RestaurantID");
                for (int i = 0; i < listItem.Length; i++)
                    comboBox2.Items.Add(listItem[i]);

                listItem = fillcomboBoxes("RestName", "Restaurant", "RestaurantID");
                for (int i = 0; i < listItem.Length; i++)
                    comboBox3.Items.Add(listItem[i]);
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox3.Text = comboBox3.Items[comboBox2.SelectedIndex].ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox2.Text = comboBox2.Items[comboBox3.SelectedIndex].ToString();
        }
    }
}
