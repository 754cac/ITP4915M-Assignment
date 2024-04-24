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
    public partial class CeditCategory : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt1 = new DataTable();

        virtualID prePage;
        string intent;
        string selectedVid;
        string selectedBrandID;
        string selectedBrandName;
        string selectedItemID;

        public CeditCategory()
        {
            InitializeComponent();
        }

        public CeditCategory(virtualID prePage, string intent, string selectedVid, string selectedBrandID, string selectedBrandName, string selectedItemID)
        {
            InitializeComponent();
            this.prePage = prePage;
            this.intent = intent;
            this.selectedVid = selectedVid;
            this.selectedBrandID = selectedBrandID;
            this.selectedBrandName = selectedBrandName;
            this.selectedItemID = selectedItemID;
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

        private void CeditCategory_Load(object sender, EventArgs e)
        {
            string[] listItem;

            if (intent.Equals("EDIT")) {
                label10.Text = "Edit Virtual ID";
                textBox1.Text = selectedVid;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox1.Text = selectedBrandID;
                comboBox1.Enabled = false;
                comboBox3.DropDownStyle = ComboBoxStyle.DropDown;
                comboBox3.Text = selectedBrandName;
                comboBox3.Enabled = false;
                textBox4.Text = selectedItemID;
            }
            else if (intent.Equals("Create"))      
            {
                label10.Text = "Create Virtual ID";

                listItem = fillcomboBoxes("BrandID", "Brand");
                for (int i = 0; i < listItem.Length; i++)
                    comboBox1.Items.Add(listItem[i]);

                DataTable dt = new DataTable();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter("SELECT BrandName FROM Brand ORDER BY BrandID", connStr);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    comboBox3.Items.Add(dt.Rows[i]["BrandName"].ToString());
                }dt.Clear();
            }
            listItem = fillcomboBoxes("Category", "Item");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox4.Items.Add(listItem[i]);

            listItem = fillcomboBoxes("Subcategory", "Item");
            for (int i = 0; i < listItem.Length; i++)
            {
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox2.Items.Add(listItem[i]);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)        // Branch ID comboBox
        {
            comboBox3.Text = comboBox3.Items[comboBox1.SelectedIndex].ToString();

            OleDbConnection connection = new OleDbConnection(connStr);                 // Virtual ID autofill
            sqlStr = $"SELECT MAX(VirtualID) FROM VirtualID WHERE BrandID = '{comboBox1.Text}'";
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            string letter = command.ExecuteScalar().ToString().Substring(0, 1);
            string id = command.ExecuteScalar().ToString().Remove(0, 1);
            textBox1.Text = letter + (int.Parse(id) + 1).ToString();
            connection.Close();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)         // Brand Name comboBox
        {
            comboBox1.Text = comboBox1.Items[comboBox3.SelectedIndex].ToString();
        }

        private void button3_Click(object sender, EventArgs e)          // Search item
        {
            sqlStr = "SELECT * FROM Item WHERE ItemID is not NULL";
                     
            dt1.Clear();
            string idInput = (textBox2.Text.TrimStart(' ')).TrimStart('0');
            if (string.IsNullOrWhiteSpace(idInput)  || textBox2.Text.Equals("Item ID / Name"))
            {
                if (comboBox4.SelectedIndex > -1)
                    sqlStr += " AND Category = '" + comboBox4.Text + "'";
                if (comboBox2.SelectedIndex > -1)
                    sqlStr += " AND Subcategory = '" + comboBox2.Text + "'";
                fillDataGridView1(sqlStr);        
            }
            else
            {
                char[] charArr = idInput.ToCharArray();

                if (idInput.Length == 1)
                {
                    if (Convert.ToInt32(charArr[0]) > 48 && Convert.ToInt32(charArr[0]) <= 57)
                    {
                        sqlStr += " AND Item.ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                        fillDataGridView1(sqlStr);
                    }
                    else
                        MessageBox.Show("Item ID start with 1");
                }
                else
                {   
                    if (Convert.ToInt32(charArr[1]) >= 48 && Convert.ToInt32(charArr[1]) <= 57)
                    {
                        idInput = idInput.TrimStart('0');
                        if (idInput.Equals(""))
                            MessageBox.Show("Item ID start with 1");
                        else
                        {
                            sqlStr += " AND ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
                            fillDataGridView1(sqlStr);
                        }
                    }
                    else
                    {
                        sqlStr += " AND ItemName like '%" + idInput + "%'";
                        fillDataGridView1(sqlStr);
                    }
                }
                if (dt1.Rows.Count == 0)
                    MessageBox.Show("No result found");
            }
            cleanUp();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)    // select a item
        {
            if (dt1.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    textBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
            }
        }

        private void button5_Click(object sender, EventArgs e)          // Submit button
        {
            if (intent.Equals("EDIT"))
            {
                sqlStr = $"UPDATE VirtualID SET ItemID = '{textBox4.Text}' WHERE VirtualID = '{textBox1.Text}'";
                sqlExecution(sqlStr);
                sqlStr = "SELECT VirtualID, VirtualID.BrandID, Brand.BrandName, VirtualID.ItemID, ItemName, Subcategory " +
                         "FROM Item, VirtualID, Brand " +
                         "WHERE Item.ItemID = VirtualID.ItemID AND Brand.BrandID = VirtualID.BrandID";
                sqlStr += $" AND VirtualID = '{textBox1.Text}'";
            }
            else {
                string itemID = (textBox4.Text.Length > 0) ? $"'{textBox4.Text}'" : "'001'";

                sqlStr = $"INSERT INTO VirtualID VALUES('{textBox1.Text}', '{comboBox1.Text}', {itemID})";
                sqlExecution(sqlStr);

                sqlStr = "SELECT VirtualID, VirtualID.BrandID, Brand.BrandName, VirtualID.ItemID, ItemName, Subcategory " +
                         "FROM Item, VirtualID, Brand " +
                         "WHERE Item.ItemID = VirtualID.ItemID AND Brand.BrandID = VirtualID.BrandID";  
                sqlStr += $" AND VirtualID = '{textBox1.Text}'";
            }
            prePage.dt2.Clear();
            prePage.fillDataGridView2(sqlStr + " ORDER BY VirtualID");
            this.Close();
        }

        private string[] fillcomboBoxes(string col_name, string table)
        {
            DataTable dt = new DataTable();
            sqlStr = $"SELECT DISTINCT {col_name} FROM {table}";
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

        private void fillDataGridView1(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt1);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dt1;
        }

        private void cleanUp()
        {
            textBox2.Text = "Item ID / Name";
            textBox2.ForeColor = Color.FromArgb(213, 202, 209);
            comboBox4.Text = "";
            comboBox4.SelectedIndex = -1;
            comboBox2.Text = "";
            comboBox2.SelectedIndex = -1;
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

        private void button7_Click(object sender, EventArgs e)          // EXIT
        {
            this.Close();
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = "";
            textBox2.ForeColor = Color.Black;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox2.Text))
            {
                textBox2.Text = "Item ID / Name";
                textBox2.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        
    }
}
