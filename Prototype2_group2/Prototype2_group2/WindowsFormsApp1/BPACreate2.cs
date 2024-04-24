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
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;

namespace WindowsFormsApp1
{
    public partial class BPACreate2 : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                      + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt1 = new DataTable();
        DataTable dt2 = new DataTable();
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
        public BPACreate2(string[] array)
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

        private void BPACreate2_Load(object sender, EventArgs e)
        {
            label10.Text = "Create Agreement ID : " + agid;
            string[] listItem = fillcomboBoxes("Category");
            for (int i = 0; i < listItem.Length; i++)
                AGMScbbCategory.Items.Add(listItem[i]);
            listItem = fillcomboBoxes("Subcategory");
            for (int i = 0; i < listItem.Length; i++)
            {

             if (!String.IsNullOrEmpty(listItem[i]))
               AGMScbbSubcategory.Items.Add(listItem[i]);
            }
            fillDataGridView2("SELECT ItemID,ItemName,Category,SubCategory FROM Item");
            fillDataGridView1();

            MessageBox.Show(createdate);

        }

        private void AGMSbtnSearch_Click(object sender, EventArgs e)
        {
            //sqlStr = "SELECT ItemID, ItemName, Category, Subcategory, Unit FROM Item ";
            sqlStr = "SELECT ItemID, ItemName, Category, SubCategory FROM Item ";
            dt2.Clear();
            string idInput = AGMStxtSrearch.Text.TrimStart(' ');
            if (string.IsNullOrWhiteSpace(idInput))
            {
                if (AGMScbbCategory.SelectedIndex > -1)
                {
                    sqlStr += "WHERE Category = '" + AGMScbbCategory.Text + "'";
                }
                if (AGMScbbSubcategory.SelectedIndex > -1)
                {
                    if (AGMScbbCategory.SelectedIndex == -1)
                        sqlStr += " WHERE";
                    else
                        sqlStr += " AND";
                    sqlStr += " SubCategory = '" + AGMScbbSubcategory.Text + "'";
                }
                fillDataGridView2(sqlStr);
            }
            else
            {
                char[] charArr = idInput.ToCharArray();

                if (idInput.Length == 1)
                {
                    if (Convert.ToInt32(charArr[0]) > 48 && Convert.ToInt32(charArr[0]) <= 57)
                    {
                        sqlStr += " WHERE ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                        fillDataGridView2(sqlStr);
                    }
                    else
                        MessageBox.Show("ID start with 1");
                }
                else
                {
                    if (Convert.ToInt32(charArr[1]) >= 48 && Convert.ToInt32(charArr[1]) <= 57)
                    {
                        idInput = idInput.TrimStart('0');
                        if (idInput.Equals(""))
                            MessageBox.Show("ID start with 1");
                        else
                        {
                            sqlStr += " WHERE ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                            fillDataGridView2(sqlStr);
                        }
                    }
                    else
                    {
                        sqlStr += " WHERE ItemName like '%" + idInput + "%'";
                        fillDataGridView2(sqlStr);
                    }
                }
                if (dt2.Rows.Count == 0)
                    MessageBox.Show("No result");
            }
            cleanUp();
        }









       

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dt2.Rows.Count > 0)
            {
                
                if (e.ColumnIndex == 0)
                {
                    
                    AGMrowIndex = Convert.ToInt32(dataGridView2.CurrentRow.Index);
                    AGMitemSelected = true;
                }
            }
        }

        private void AGMSbtnAdd_Click(object sender, EventArgs e)
        {
            string input1 = AGMStxtPrice.Text;
            string input2 = AGMStxtPQuantity.Text;
            int num1, num2;


            if (!AGMitemSelected)
                MessageBox.Show("Select a item");
            else if (string.IsNullOrWhiteSpace(input1) && string.IsNullOrWhiteSpace(input2))
                MessageBox.Show("Input a number / not empty");
            else if (!int.TryParse(input1, out num1) || !int.TryParse(input2, out num2))
                MessageBox.Show("Input a number");
            else
            {
                fillDataGridView1();
                addContractLine(num1, num2);
                fillDataGridView1();
                AGMitemSelected = false;
            }
            AGMStxtPrice.Text = "";
            AGMStxtPQuantity.Text = "";
        }



        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
        e.RowIndex >= 0)
                {
                    sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID = '{dataGridView1.Rows[e.RowIndex].Cells[1].Value}'";
                    sqlExecution(sqlStr);
                    fillDataGridView1();
                }
            
            
        }

        /// /////////////////////////////////////////////////////////////


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

        private void cleanUp()
        {
            AGMStxtSrearch.Text = "";
            AGMScbbCategory.Text = "";
            AGMScbbCategory.SelectedIndex = -1;
            AGMScbbSubcategory.Text = "";
            AGMScbbSubcategory.SelectedIndex = -1;
            AGMStxtPrice.Text = "";
            AGMStxtPQuantity.Text = "";
        }

        private void fillDataGridView1()
        {
            dt1.Clear();
            sqlStr = "SELECT ItemID, ItemName,Category, SubCategory, Price,PromisedQuantity FROM ContractLine_tmp";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt1);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dt1;
        }

        private void fillDataGridView2(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();
            dataGridView2.DataSource = dt2;

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

        private void addContractLine(int num1,int num2)
        {
            
            String itemID = dt2.Rows[AGMrowIndex]["ItemID"].ToString();
            String itemName = dt2.Rows[AGMrowIndex]["ItemName"].ToString();
            String category = dt2.Rows[AGMrowIndex]["Category"].ToString();
            String subcategory = dt2.Rows[AGMrowIndex]["SubCategory"].ToString();
            String price = num1.ToString();
            String qty = num2.ToString();

            if (dt1.Rows.Count > 0)
            {
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    if (dt1.Rows[i]["ItemID"].ToString() == itemID)
                    {
                        sqlStr = $"UPDATE ContractLine_tmp SET PromisedQuantity = '{qty}' WHERE ItemID = '{itemID}'";
                        sqlExecution(sqlStr);
                        sqlStr = $"UPDATE ContractLine_tmp SET Price = '{price}' WHERE ItemID = '{itemID}'";
                        sqlExecution(sqlStr);

                        return;
                    }
                }
                sqlStr = $"INSERT INTO ContractLine_tmp " +
                         $"VALUES('{itemID}', '{itemName}', '{category}', '{subcategory}', '{price}', '{qty}')";
                sqlExecution(sqlStr);

            }
            else
            {
                sqlStr = $"INSERT INTO ContractLine_tmp " +
                         $"VALUES('{itemID}', '{itemName}', '{category}', '{subcategory}', '{price}', '{qty}')";
                sqlExecution(sqlStr);
            }
        }

        private void AGMSbtnSubmit_Click(object sender, EventArgs e)
        {
            condition = AGMStxtCondition.Text.ToString();
            
            if (dt1.Rows.Count > 0)
            {
                sqlStr = $"INSERT INTO Contract VALUES('{agid}','{agtype}', '{createdate}', '{enpdate}', '{supid}', '{staffid}', '{remark}', '{condition}')";
                sqlExecution(sqlStr);

                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    sqlStr = $"INSERT INTO ContractLine VALUES('{dt1.Rows[i]["ItemID"]}', '{agid}','{dt1.Rows[i]["Price"]}','{dt1.Rows[i]["PromisedQuantity"]}', '0')";
                    sqlExecution(sqlStr);

                }


                sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID is not NULL";
                sqlExecution(sqlStr);
                this.Close();

            }
            else
            {
                MessageBox.Show("This Contract do not have any Item! Please insert!");
            }
        }

        private void AGMSbtnPrevious_Click(object sender, EventArgs e)
        {
            this.cleanUp();
            this.Close();

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.cleanUp();
            sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID IS NOT NULL";
            sqlExecution(sqlStr);
            this.Close();

        }



        //////////////////////////////////////////  Property  ///////////////////////////////////////////
        //private void button3_Click(object sender, EventArgs e)
        //{
        //    sqlStr = $"DELETE FROM ContractLine_tmp WHERE ItemID is not NULL";
        //   sqlExecution(sqlStr);
        //   this.Close();
        // }
    }
}
