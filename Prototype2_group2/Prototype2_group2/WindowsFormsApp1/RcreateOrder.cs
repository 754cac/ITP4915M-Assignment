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
    public partial class RcreateOrder : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();

        restOrderList previousForm;
        string brandID;
        string restaurantID; 

        Boolean itemSelected = false;
        int rowIndex;

        public RcreateOrder()
        {
            InitializeComponent();
        }

        public RcreateOrder(restOrderList previousForm, string brandID, string restaurantID)
        {
            InitializeComponent();
            this.previousForm = previousForm;
            this.brandID = brandID;
            this.restaurantID = restaurantID;
        }

        private void panel3_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel3_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        //////////////////////////////////////////  Event Handler  ///////////////////////////////////////////

        private void RcreateOrder_Load(object sender, EventArgs e)
        {
            string[] listItem = fillcomboBoxes("Category");
            for (int i = 0; i < listItem.Length; i++)
                comboBox2.Items.Add(listItem[i]);

            listItem = fillcomboBoxes("Subcategory");
            for (int i = 0; i < listItem.Length; i++)
            {
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox3.Items.Add(listItem[i]);
            }
        }

        private void button2_Click(object sender, EventArgs e)          //  Search button
        {
            sqlStr = "SELECT VirtualID, Item.ItemID, ItemName, Category, Subcategory, Unit FROM Item, VirtualID " +
                     "WHERE Item.ItemID = VirtualID.ItemID AND BrandID = '" + brandID + "'";
            dt2.Clear();
            string idInput = textBox3.Text.TrimStart(' ');
            if (string.IsNullOrWhiteSpace(idInput))
            {
                if (comboBox2.SelectedIndex > -1)
                    sqlStr += " AND Category = '" + comboBox2.Text + "'";
                if (comboBox3.SelectedIndex > -1)
                    sqlStr += " AND Subcategory = '" + comboBox3.Text + "'";
                fillDataGridView2(sqlStr);
            }
            else
            {
                char[] charArr = idInput.ToCharArray();

                if (idInput.Length == 1)
                {
                    if (Convert.ToInt32(charArr[0]) > 48 && Convert.ToInt32(charArr[0]) <= 57)
                    {
                        sqlStr += " AND Item.ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                        fillDataGridView2(sqlStr);
                    }
                    else
                        MessageBox.Show("Item ID start with 1");
                }
                else
                {
                    if ((Char.ToString(charArr[0]).Equals("V") || Char.ToString(charArr[0]).Equals("v"))
                          && (Convert.ToInt32(charArr[1]) >= 48 && Convert.ToInt32(charArr[1]) <= 57))
                    {
                        idInput = (idInput.Remove(0, 1)).TrimStart('0');
                        sqlStr += " AND VirtualID = '" + getKey() + string.Format("{0:00}", Convert.ToInt32(idInput)) + "'";
                        fillDataGridView2(sqlStr);
                    }
                    else if (Convert.ToInt32(charArr[1]) >= 48 && Convert.ToInt32(charArr[1]) <= 57)
                    {
                        idInput = idInput.TrimStart('0');
                        if (idInput.Equals(""))
                            MessageBox.Show("Item ID start with 1");
                        else
                        {
                            sqlStr += " AND Item.ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                            fillDataGridView2(sqlStr);
                        }
                    }
                    else
                    {
                        sqlStr += " AND ItemName like '%" + idInput + "%'";
                        fillDataGridView2(sqlStr);
                    }
                }
                if (dt2.Rows.Count == 0)
                    MessageBox.Show("No result found");
            }
            cleanUp();
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)     //  Select a item
        {
            if (dt2.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    rowIndex = Convert.ToInt32(dataGridView2.CurrentRow.Index);
                    itemSelected = true;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)          // Add a item 
        {
            string input = textBox2.Text;
            int num;

            if (!itemSelected)
                MessageBox.Show("Please select a item first");
            else if (string.IsNullOrWhiteSpace(input))
                MessageBox.Show("Quantity cannot be empty, please input a number");
            else if (!int.TryParse(input, out num))
                MessageBox.Show("Please input a numeric value for the Quantity textbox");
            else
            {
                fillDataGridView1();
                addOrderLine(num);
                fillDataGridView1();
                itemSelected = false;
            }
            textBox2.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     //  Update / Delete a item
        {
            if (dt1.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)             // UPDATE
                {
                    string input = dataGridView1.Rows[e.RowIndex].Cells[8].Value.ToString();    // Quantity
                    if (string.IsNullOrWhiteSpace(input))
                    {
                        MessageBox.Show("Quantity cannot be empty, please input a number");
                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = "";
                    }
                    else if (!int.TryParse(input, out int num))
                    {
                        MessageBox.Show("Please input a numeric value for the Quantity textbox");
                        dataGridView1.Rows[e.RowIndex].Cells[8].Value = "";
                    }
                    else
                    {
                        sqlStr = $"UPDATE OrderLine_tmp SET Quantity = '{input}' WHERE ItemID = '{dataGridView1.Rows[e.RowIndex].Cells[3].Value}'";
                        sqlExecution(sqlStr);
                        fillDataGridView1();
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    sqlStr = $"DELETE FROM OrderLine_tmp WHERE ItemID = '{dataGridView1.Rows[e.RowIndex].Cells[3].Value}'";
                    sqlExecution(sqlStr);
                    fillDataGridView1();
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)          //  Submit button
        {
            string date = dateTimePicker1.Value.ToShortDateString();
            string rid = getID("RequisitionID", "Requisition");
            string oid = getID("OrderLineID", "OrderLine");

            sqlStr = $"INSERT INTO Requisition VALUES('{rid}', '{DateTime.Today.ToShortDateString()}', '{date}', '{restaurantID}', '1 ', 'Processing')";
            sqlExecution(sqlStr);

            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                sqlStr = $"INSERT INTO OrderLine VALUES('{oid}', '{dt1.Rows[i]["ItemID"]}', '{dt1.Rows[i]["Quantity"]}', '{rid}', null, '0', 'Processing')";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
                oid = string.Format("{0:000}", int.Parse(oid) + 1);
            }
            connection.Close();

            sqlStr = $"DELETE FROM OrderLine_tmp WHERE ItemID is not NULL";
            sqlExecution(sqlStr);

            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestaurantID, Status FROM Requisition " +
                     $"WHERE RequisitionID = '{rid}'";
            previousForm.showEditedRequisition(sqlStr);

            this.Close();
        }
        //////////////////////////////////////////  Own Method  ///////////////////////////////////////////

        private string[] fillcomboBoxes(string col_name)
        {
            DataTable dt = new DataTable();
            sqlStr = $"SELECT DISTINCT {col_name} FROM Item";
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

        private string getKey()
        {
            string firstLetter = "";
            switch (brandID)
            {
                case "001":
                    return "c";
                case "002":
                    return "w";
                case "003":
                    return "j";
                case "004":
                    return "";
            }
            return firstLetter;
        }

        private void fillDataGridView2(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();
            dataGridView2.DataSource = dt2;
            for (int i = 0; i < dt2.Rows.Count; i++)
            {
                dt2.Rows[i]["VirtualID"] = "V" + dt2.Rows[i]["VirtualID"].ToString().Remove(0, 1);
            }
        }

        private void cleanUp()
        {
            textBox3.Text = "";
            comboBox2.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.Text = "";
            comboBox3.SelectedIndex = -1;

            textBox2.Text = "";
        }

        private void fillDataGridView1()
        {
            dt1.Clear();
            sqlStr = "SELECT * FROM OrderLine_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt1);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dt1;
        }

        private void addOrderLine(int num)
        {
            string vid = dt2.Rows[rowIndex]["VirtualID"].ToString(); vid = (vid.Length > 0) ? $"'{vid}'" : "NULL";
            string itemID = dt2.Rows[rowIndex]["ItemID"].ToString();
            string itemName = dt2.Rows[rowIndex]["ItemName"].ToString();
            string category = dt2.Rows[rowIndex]["category"].ToString();
            string subcategory = dt2.Rows[rowIndex]["subCategory"].ToString(); subcategory = (subcategory.Length > 0) ? $"'{subcategory}'" : "NULL";
            string unit = dt2.Rows[rowIndex]["unit"].ToString();
            string qty = num.ToString();

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["ItemID"].ToString() == itemID)
                    {
                        qty = (int.Parse(dt1.Rows[i]["Quantity"].ToString()) + int.Parse(qty)).ToString();
                        sqlStr = $"UPDATE OrderLine_tmp SET Quantity = '{qty}' WHERE ItemID = '{itemID}'";
                        sqlExecution(sqlStr);
                        return;
                    }
                }
                sqlStr = $"INSERT INTO OrderLine_tmp " +
                         $"VALUES({vid}, '{itemID}', '{itemName}', '{category}', {subcategory}, '{unit}', '{qty}')";
                sqlExecution(sqlStr);
            }
            else
            {
                sqlStr = $"INSERT INTO OrderLine_tmp " +
                         $"VALUES({vid}, '{itemID}', '{itemName}', '{category}', {subcategory}, '{unit}', '{qty}')";
                sqlExecution(sqlStr);
            }
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
        
        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            sqlStr = $"DELETE FROM OrderLine_tmp WHERE ItemID is not NULL";
            sqlExecution(sqlStr);
            this.Close();
        }
    }
}
