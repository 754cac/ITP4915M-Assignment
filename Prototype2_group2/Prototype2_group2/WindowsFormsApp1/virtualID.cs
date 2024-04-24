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
    public partial class virtualID : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        public DataTable dt2 = new DataTable();

        Boolean itemSelected = false;
        string selectedVid;
        string selectedBrandID;
        string selectedBrandName;
        string selectedItemID;

        public virtualID()
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

        private void virtualID_Load(object sender, EventArgs e)
        {
            string[] listItem = fillcomboBoxes("Category", "Item");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox3.Items.Add(listItem[i]);

            listItem = fillcomboBoxes("Subcategory", "Item");
            for (int i = 0; i < listItem.Length; i++)
            {
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox6.Items.Add(listItem[i]);
            }
            listItem = fillcomboBoxes("BrandID", "Brand");
            for (int i = 0; i < listItem.Length; i++)
                if (!String.IsNullOrEmpty(listItem[i]))
                    comboBox1.Items.Add(listItem[i]);

            ////////////////////////////////////////////////////// in the works //////////////////////////////////////
            button6.Hide(); button4.Hide();
        }

        private void button7_Click(object sender, EventArgs e)          // Category button
        {
            Ccategory cMainPage = new Ccategory();
            this.Hide();
            cMainPage.ShowDialog();
            this.Show();
        }

        private void button13_Click(object sender, EventArgs e)         // Search button
        {
            sqlStr = "SELECT VirtualID, VirtualID.BrandID, Brand.BrandName, VirtualID.ItemID, ItemName, Subcategory " +
                     "FROM Item, VirtualID, Brand " +
                     "WHERE Item.ItemID = VirtualID.ItemID AND Brand.BrandID = VirtualID.BrandID";
            dt2.Clear();
            string idInput = textBox2.Text.TrimStart(' ');
            string vid = (textBox3.Text.TrimStart(' ')).TrimStart('0');
            string itemID = (textBox4.Text.TrimStart(' ')).TrimStart('0');

            if (string.IsNullOrEmpty(vid))
            {
                checkCondition(idInput, itemID);
                fillDataGridView2(sqlStr + " ORDER BY VirtualID");
                if (dt2.Rows.Count == 0)
                    MessageBox.Show("No result found");
            }
            else
            {
                for(int i = 0; i < 3; i++)
                {
                    string[] vh = {"c", "j", "w"};
                    sqlStr = "SELECT VirtualID, VirtualID.BrandID, Brand.BrandName, VirtualID.ItemID, ItemName, Subcategory " +
                             "FROM Item, VirtualID, Brand " +
                             "WHERE Item.ItemID = VirtualID.ItemID AND Brand.BrandID = VirtualID.BrandID";
                    checkCondition(idInput, itemID);
                    sqlStr += " AND VirtualID = '" + vh[i] + string.Format("{0:00}", Convert.ToInt32(vid)) + "'";
                    fillDataGridView2(sqlStr + " ORDER BY VirtualID");
                }
                if (dt2.Rows.Count == 0)
                    MessageBox.Show("No result found");
            }
            cleanUp();
        }

        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)        // Select a item
        {
            if (dt2.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0 && e.RowIndex > -1)
                {
                    itemSelected = true;
                    selectedVid = dataGridView2.Rows[e.RowIndex].Cells[1].Value.ToString();
                    selectedBrandID = dataGridView2.Rows[e.RowIndex].Cells[2].Value.ToString();
                    selectedBrandName = dataGridView2.Rows[e.RowIndex].Cells[3].Value.ToString();
                    selectedItemID = dataGridView2.Rows[e.RowIndex].Cells[4].Value.ToString();
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)             // New button
        {
            CeditCategory createVid = new CeditCategory(this, "Create", "", "", "", "");
            createVid.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)          // Edit button
        {
            if (itemSelected)
            {
                CeditCategory editVid = new CeditCategory(this, "EDIT", selectedVid, selectedBrandID, selectedBrandName, selectedItemID);
                itemSelected = false;
                editVid.ShowDialog();
            }
            else MessageBox.Show("Please select a virtual ID first");
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

        private void checkCondition(string idInput, string itemID) {
            if (!string.IsNullOrWhiteSpace(idInput) && !idInput.Equals("Item Name"))
                sqlStr += " AND ItemName like '%" + idInput + "%'";
            if (comboBox3.SelectedIndex > -1)
                sqlStr += " AND Item.Category = '" + comboBox3.Text + "'";
            if (comboBox6.SelectedIndex > -1)
                sqlStr += " AND Subcategory = '" + comboBox6.Text + "'";
            if (comboBox1.SelectedIndex > -1)
                sqlStr += " AND Brand.BrandID = '" + comboBox1.Text + "'";
            if (!string.IsNullOrEmpty(textBox4.Text))
                sqlStr += " AND Item.ItemID = '" + string.Format("{0:000}", Convert.ToInt32(itemID)) + "'";
        }

        public void fillDataGridView2(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();
            dataGridView2.DataSource = dt2;
        }

        private void cleanUp()
        {
            textBox2.Text = "Item Name";
            textBox2.ForeColor = Color.FromArgb(213, 202, 209);
            textBox3.Text = "";
            textBox4.Text = "";

            comboBox3.Text = "";
            comboBox3.SelectedIndex = -1;
            comboBox6.Text = "";
            comboBox6.SelectedIndex = -1;
            comboBox1.Text = "";
            comboBox1.SelectedIndex = -1;
        }

        private void button8_Click(object sender, EventArgs e)          // Close button
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
                textBox2.Text = "Item Name";
                textBox2.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

    }
}
