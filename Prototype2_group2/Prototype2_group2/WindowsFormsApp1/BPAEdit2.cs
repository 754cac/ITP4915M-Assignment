using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class BPAEdit2 : Form
    {

        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                      + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt1 = new DataTable(); 
        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();


        string agid;
        string agtype;
        string supid;
        string staffid;
        string remark;
        string createdate;
        string enpdate;
        string condition;


        Boolean AGMitemSelected = false;
        int AGMrowIndex;
        public BPAEdit2(string[] array)
        {
            InitializeComponent();
            this.agid = array[0];
            this.agtype = array[1];
            this.supid = array[2];
            this.staffid = array[3];
            this.remark = array[4];
            this.createdate = array[5];
            this.enpdate = array[6];

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

        private void BPAEdit2_Load(object sender, EventArgs e)
        {
            label10.Text = "Edit Agreement ID : " + agid;
            fillDataGridView1(agid);
            DisplayData(agid);

            if (agtype == "CPA")
            {
                label2.Visible = false;
                EAtxtqty.Visible = false;
                EAtxtqty.Text = "";
                EAtxtqty.ReadOnly = true;
            }
            else
            {
                label2.Visible = true;
                EAtxtqty.Visible = true;
                EAtxtqty.Text = "";
                EAtxtqty.ReadOnly = false;

            }


        }

        private void fillDataGridView1(string agid)
        {
            dt1.Clear();
            sqlStr = $"SELECT ContractLine.ItemID, Item.ItemName, Item.Category, Item.SubCategory, ContractLine.Price,ContractLine.PromisedQuantity FROM ContractLine,Item WHERE ContractLine.ItemID = Item.ItemID AND ContractLine.AgreementID = '{agid}'";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt1);
            dataAdapter.Dispose();

            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                sqlStr = $"INSERT INTO ContractLine_tmp VALUES('{dt1.Rows[i]["ItemID"]}','{dt1.Rows[i]["ItemName"]}', '{dt1.Rows[i]["Category"]}','{dt1.Rows[i]["SubCategory"]}','{dt1.Rows[i]["Price"]}','{dt1.Rows[i]["PromisedQuantity"]}')";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();

            dt2.Clear();
            sqlStr = $"SELECT ItemID, ItemName,Category, SubCategory, Price,PromisedQuantity FROM ContractLine_tmp";
            OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter1.Fill(dt2);
            dataAdapter1.Dispose();
            dataGridView1.DataSource = dt2;





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

        private void EAbtnChange_Click(object sender, EventArgs e)
        {
            string input1 = EAtxtqty.Text;
            string input2 = EAtxtPrice.Text;
            int num1, num2;

            if (agtype != "CPA")
            {
                if (!AGMitemSelected)
                    MessageBox.Show("Select a item");
                else if (string.IsNullOrWhiteSpace(input1) && string.IsNullOrWhiteSpace(input2))
                    MessageBox.Show("Input a number / not empty");
                else if (!int.TryParse(input1, out num1) || !int.TryParse(input2, out num2))
                    MessageBox.Show("Input a number");
                else
                {
                    fillDataGridView1second();
                    editContractLine(num2,num1);
                    fillDataGridView1second();
                    AGMitemSelected = false;
                }
            }
            else
            {
                if (!AGMitemSelected)
                    MessageBox.Show("Select a item");
                else if ( string.IsNullOrWhiteSpace(input2))
                    MessageBox.Show("Input a number / not empty");
                else if (!int.TryParse(input2, out num2))
                    MessageBox.Show("Input a number");
                else
                {
                    fillDataGridView1second();
                    editContractLineForCPA(num2);
                    fillDataGridView1second();
                    AGMitemSelected = false;
                }
            }
            EAtxtqty.Text = "";
            EAtxtPrice.Text = "";
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        var senderGrid = (DataGridView)sender;
            if (dt2.Rows.Count > 0)
            {

                if (e.ColumnIndex >= 0 && senderGrid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn)
                {

                    AGMrowIndex = Convert.ToInt32(dataGridView1.CurrentRow.Index);
                    AGMitemSelected = true;
                }
            }

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                    e.RowIndex >= 0)
            {
                condition = txtCondition.Text;
                dt4.Clear();
                sqlStr = $"SELECT * FROM PurchaseLine WHERE ItemID = '{dataGridView1.Rows[e.RowIndex].Cells[2].Value}'";
                OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter(sqlStr, connStr);
                dataAdapter1.Fill(dt4);
                dataAdapter1.Dispose();

                if (dt4.Rows.Count > 0)
                {
                    MessageBox.Show("This Contract Item have Purchase order!!!");
                }
                else {
                    sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID = '{dataGridView1.Rows[e.RowIndex].Cells[2].Value}'";
                    sqlExecution(sqlStr);
                    fillDataGridView1second();
                }

            }
        }

        private void fillDataGridView1second()
        {
            dt2.Clear();
            sqlStr = "SELECT ItemID, ItemName,Category,SubCategory, Price,PromisedQuantity FROM ContractLine_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dt2;
        }

        private void editContractLine(int num1, int num2)
        {

            String itemID = dt2.Rows[AGMrowIndex]["ItemID"].ToString();
            String price = num1.ToString();
            String qty = num2.ToString();

            
                sqlStr = $"UPDATE  ContractLine_tmp SET Price = '{price}', PromisedQuantity = '{qty}' WHERE ItemID = '{itemID}'";
                sqlExecution(sqlStr);

        }

        private void editContractLineForCPA(int num1)
        {

            String itemID = dt2.Rows[AGMrowIndex]["ItemID"].ToString();
            String price = num1.ToString();


            sqlStr = $"UPDATE  ContractLine_tmp SET Price = '{price}' WHERE ItemID = '{itemID}'";
            sqlExecution(sqlStr);

        }

        private void EAbtnUpdate_Click(object sender, EventArgs e)
        {
            condition = txtCondition.Text;
            dt3.Clear();
            sqlStr = $"SELECT ItemID, ItemName,Category, SubCategory, Price,PromisedQuantity FROM ContractLine_tmp";
            OleDbDataAdapter dataAdapter1 = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter1.Fill(dt3);
            dataAdapter1.Dispose();

            if (dt3.Rows.Count > 0)
            {
                sqlStr = $"UPDATE Contract SET ConExpiryDate = '{enpdate}', EmployeeID = '{staffid}', Remark = '{remark}', Condition = '{condition}' WHERE AgreementID = '{agid}'";
                sqlExecution(sqlStr);

                OleDbConnection connection = new OleDbConnection(connStr);
                OleDbCommand command;
                connection.Open();

                sqlStr = $"DELETE FROM ContractLine WHERE AgreementID='{agid}' AND ItemID NOT IN (SELECT ItemID FROM ContractLine_tmp WHERE ItemID IS NOT NULL)";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();

                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    sqlStr = $"UPDATE ContractLine SET Price='{dt3.Rows[i]["Price"]}', PromisedQuantity='{dt3.Rows[i]["PromisedQuantity"]}' WHERE ItemID='{dt3.Rows[i]["ItemID"]}' AND AgreementID='{agid}'";
                    command = new OleDbCommand(sqlStr, connection);
                    command.ExecuteNonQuery();

                }
                connection.Close();

                sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID IS NOT NULL";
                sqlExecution(sqlStr);
                this.Close();

                
            }
            else
            {
                MessageBox.Show("Delete the Contract due to NO item!");
                sqlStr = $"DELETE FROM ContractLine WHERE AgreementID = '{agid}'";
                sqlExecution(sqlStr);
                sqlStr = $"DELETE FROM Contract WHERE AgreementID = '{agid}'";
                sqlExecution(sqlStr);
                this.Close();
            }
        }

        private void DisplayData(string agreementID)
        {
            string sqlStr = $"SELECT Condition FROM Contract WHERE AgreementID = '{agreementID}'";
            using (OleDbConnection conn = new OleDbConnection(connStr))
            {
                OleDbCommand cmd = new OleDbCommand(sqlStr, conn);
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    txtCondition.Text = reader["Condition"].ToString();
                }
                reader.Close();
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID IS NOT NULL";
            sqlExecution(sqlStr);
            this.Close();
            
        }


    }
}
