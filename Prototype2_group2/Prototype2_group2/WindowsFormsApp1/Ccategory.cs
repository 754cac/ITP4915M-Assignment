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
    public partial class Ccategory : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt1 = new DataTable();
        OleDbDataAdapter dataAdapter;

        string nextItemID;

        public Ccategory()
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

        private void Ccategory_Load(object sender, EventArgs e)
        {
            setNextItemID();
            textBox3.Text = nextItemID;

            string[] listItem = fillcomboBoxes("Category");
            for (int i = 0; i < listItem.Length; i++)
            {
                if (!String.IsNullOrEmpty(listItem[i]))
                {
                    comboBox2.Items.Add(listItem[i]);
                    comboBox5.Items.Add(listItem[i]);
                }
            }
            listItem = fillcomboBoxes("Subcategory");
            for (int i = 0; i < listItem.Length; i++)
            {
                if (!String.IsNullOrEmpty(listItem[i]))
                {
                    comboBox3.Items.Add(listItem[i]);
                    comboBox1.Items.Add(listItem[i]);
                }
            }
            listItem = fillcomboBoxes("Unit");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox4.Items.Add(listItem[i]);

            button3.Visible = false;                //  UPDATE button

            fillDataGridView1("SELECT * FROM Item");

            ////////////////////////////////////////////// in the works  ///////////////////////////////////
            button7.Hide(); button11.Hide(); button6.Hide();
        }

        private void button10_Click(object sender, EventArgs e)         // Virtual ID button
        {
            virtualID vidPage = new virtualID();
            this.Hide();
            vidPage.ShowDialog();
            this.Show();
        }

        private void button13_Click(object sender, EventArgs e)         // Search button
        {
            sqlStr = "SELECT * FROM Item WHERE ItemID is not NULL";
            string idInput = textBox2.Text.TrimStart(' ');
            if (string.IsNullOrEmpty(idInput) || idInput.Equals("Item ID / Name"))
            {
                if (comboBox2.SelectedIndex > -1)
                    sqlStr += " AND Category = '" + comboBox2.Text + "'";
                if (comboBox3.SelectedIndex > -1)
                    sqlStr += " AND Subcategory = '" + comboBox3.Text + "'";
                fillDataGridView1(sqlStr);
            }
            else {
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
                else {
                    if (Convert.ToInt32(charArr[1]) >= 48 && Convert.ToInt32(charArr[1]) <= 57)
                    {
                        idInput = idInput.TrimStart('0');
                        if (idInput.Equals(""))
                            MessageBox.Show("Item ID start with 1");
                        else
                        {
                            sqlStr += " AND Item.ItemID = '" + string.Format("{0:000}", Convert.ToInt32(idInput)) + "'";
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

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)        // Edit button
        {
            if (dt1.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    textBox3.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    textBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
                    textBox1.ForeColor = Color.Black;
                    comboBox5.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
                    comboBox5.ForeColor = Color.Black;
                    comboBox1.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    comboBox1.ForeColor = Color.Black;
                    comboBox4.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();
                    comboBox4.ForeColor = Color.Black;
                    button12.Visible = false;
                    button3.Visible = true;
                }
            }
        }

        private void button14_Click(object sender, EventArgs e)         // Reset button
        {
            reset();
        }

        private void button12_Click(object sender, EventArgs e)         // New button
        {
            if (textBox1.Text.Equals("Item Name"))
                MessageBox.Show("Please enter the item name !");
            else
            {
                string category = (comboBox5.Text.Equals("Category") || comboBox5.SelectedIndex == -1) ? "NULL" : $"'{comboBox5.Text}'";
                string subcategory = (comboBox1.Text.Equals("Subcategory") || comboBox1.SelectedIndex == -1) ? "NULL" : $"'{comboBox1.Text}'";
                string unit = (comboBox4.Text.Equals("Unit") || comboBox4.SelectedIndex == -1) ? "NULL" : $"'{comboBox4.Text}'";

                sqlStr = $"INSERT INTO Item VALUES('{string.Format("{0:000}", int.Parse(textBox3.Text))}', '{textBox1.Text}', {category}, {subcategory}, {unit})";
                sqlExecution(sqlStr);

                sqlStr = $"SELECT * FROM Item WHERE ItemID = '{string.Format("{0:000}", int.Parse(textBox3.Text))}'";
                fillDataGridView1(sqlStr);
                reset();
            }
        }

        private void button3_Click(object sender, EventArgs e)          // Update
        {
            if (textBox1.Text.Equals("Item Name"))
                MessageBox.Show("Please enter the item name !");
            else
            {
                string category = (comboBox5.Text.Equals("Category") || comboBox5.SelectedIndex == -1) ? "NULL" : $"'{comboBox5.Text}'";
                string subcategory = (comboBox1.Text.Equals("Subcategory") || comboBox1.SelectedIndex == -1) ? "NULL" : $"'{comboBox1.Text}'";
                string unit = (comboBox4.Text.Equals("Unit") || comboBox4.SelectedIndex == -1) ? "NULL" : $"'{comboBox4.Text}'";

                sqlStr = $"UPDATE Item SET ItemName = '{textBox1.Text}'," +
                                         $"Category = {category}," +
                                      $"Subcategory = {subcategory}," +
                                             $"Unit = {unit} " +
                                     $"WHERE ItemID = '{string.Format("{0:000}", int.Parse(textBox3.Text))}'";
                sqlExecution(sqlStr);
                fillDataGridView1($"SELECT * FROM Item WHERE ItemID = '{string.Format("{0:000}", int.Parse(textBox3.Text))}'");
                reset();
            }
        }

        ///////////////////////////////////////////// Own Methons //////////////////////////////////////////

        private void setNextItemID()
        {
            OleDbConnection connection = new OleDbConnection(connStr);
            sqlStr = "SELECT MAX(ItemID) FROM Item";
            connection.Open();
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            nextItemID = string.Format("{0:000}", Convert.ToInt32((int.Parse(command.ExecuteScalar().ToString()) + 1)));
            connection.Close();
        }

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

        private void fillDataGridView1(string sql)
        {
            dt1.Clear();
            executeSelection(sql);
            dataGridView1.DataSource = dt1;
        }

        private void cleanUp() {
            textBox2.Text = "Item ID / Name";
            textBox2.ForeColor = Color.FromArgb(213, 202, 209);
            comboBox2.Text = "";
            comboBox2.SelectedIndex = -1;
            comboBox3.Text = "";
            comboBox3.SelectedIndex = -1;
        }

        private void reset()
        {
            setNextItemID();
            textBox3.Text = nextItemID;
            textBox1.Text = "Item Name";
            textBox1.ForeColor = Color.FromArgb(213, 202, 209);
            comboBox5.SelectedIndex = -1;
            comboBox5.Text = "Category";
            comboBox5.ForeColor = Color.FromArgb(213, 202, 209);
            comboBox1.SelectedIndex = -1;
            comboBox1.Text = "Subcategory";
            comboBox1.ForeColor = Color.FromArgb(213, 202, 209);
            comboBox4.SelectedIndex = -1;
            comboBox4.Text = "Unit";
            comboBox4.ForeColor = Color.FromArgb(213, 202, 209);
            button12.Visible = true;
            button3.Visible = false;
        }

        public void executeSelection(string sql)
        {
            sqlStr = sql;
            dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt1);
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

        private void textBox1_Enter(object sender, EventArgs e)         // txt item name
        {
            textBox1.Text = "";
            textBox1.ForeColor = Color.Black;
        }

        private void textBox1_Leave(object sender, EventArgs e)         // txt item name
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = "Item Name";
                textBox1.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        private void comboBox5_Enter(object sender, EventArgs e)
        {
            comboBox5.Text = "";
            comboBox5.ForeColor = Color.Black;
        }

        private void comboBox5_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox5.Text))
            {
                comboBox5.Text = "Category";
                comboBox5.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {
            comboBox1.Text = "";
            comboBox1.ForeColor = Color.Black;
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox1.Text))
            {
                comboBox1.Text = "Subcategory";
                comboBox1.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        private void comboBox4_Enter(object sender, EventArgs e)
        {
            comboBox4.Text = "";
            comboBox4.ForeColor = Color.Black;
        }

        private void comboBox4_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(comboBox4.Text))
            {
                comboBox4.Text = "Unit";
                comboBox4.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        private void button8_Click(object sender, EventArgs e)          // Close button
        {
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)          // Logout
        {
            this.Close();
        }

        
    }
}
