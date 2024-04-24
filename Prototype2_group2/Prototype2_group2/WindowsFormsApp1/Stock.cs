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
    public partial class Stock : Form
    {
        private DataTable dt = new DataTable();
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        private DataGridViewRow stockRow;
        OleDbConnection connection;
        OleDbCommand command;
        string sqlStr = "";
        public Stock()
        {
            InitializeComponent();
            connection = new OleDbConnection(connStr);
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

        private void button8_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void Stock_Load(object sender, EventArgs e)
        {
            UpdateGrid("SELECT Inventory.ItemID, Item.ItemName, Item.Category, Inventory.WarehouseID, Warehouse.WareName, Inventory.Quantity, Item.Unit, Inventory.ExpireDate"
                                  + " FROM Item, Inventory, Warehouse WHERE Inventory.ItemID = Item.ItemID AND Inventory.WarehouseID = Warehouse.WarehouseID");
        }

        public void UpdateGrid(string sql)
        {
            try
            {
                DataTable dt = new DataTable();
                OleDbDataAdapter dataAdapter =
                            new OleDbDataAdapter(sql, connStr);
                dataAdapter.Fill(dt);
                dataAdapter.Dispose();
                dataGridView1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            UpdateGrid("SELECT Inventory.ItemID, Item.ItemName, Item.Category, Inventory.WarehouseID, Warehouse.WareName, Inventory.Quantity, Item.Unit, Inventory.ExpireDate"
                                 + " FROM Item, Inventory, Warehouse WHERE Inventory.ItemID = Item.ItemID AND Inventory.WarehouseID = Warehouse.WarehouseID");
            textBox3.Text = "Item ID";
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                stockRow = dataGridView1.Rows[e.RowIndex];
                textBox4.Text = stockRow.Cells[0].Value.ToString();
                textBox5.Text = stockRow.Cells[1].Value.ToString();
                textBox2.Text = stockRow.Cells[4].Value.ToString();
                textBox7.Text = stockRow.Cells[6].Value.ToString();
            }
            else
            {
                textBox4.Text = "";
                textBox5.Text = "";
                textBox2.Text = "";
                textBox7.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox6.Text == "")
            {
                MessageBox.Show("Empty Amount!!\n" + "Please try again!");
            }
            else
            {
                
                int UpdateAmount;
                UpdateAmount = int.Parse(textBox6.Text);
                if (UpdateAmount <= 0)
                {
                    MessageBox.Show("Invalid Amount!!\n" + "Please try again!");
                }
                else
                {
                    if (stockRow == null)
                    {
                        MessageBox.Show("You have not select item!!\nPlease try again!");
                    }
                    else
                    {
                        connection.Open();
                        sqlStr = $"UPDATE Inventory SET Quantity = '{textBox6.Text}' " +
                                 $"WHERE ItemID = '{stockRow.Cells[0].Value.ToString()}' " +
                                $"And WarehouseID = '{stockRow.Cells[3].Value.ToString()}'";
                        command = new OleDbCommand(sqlStr, connection);
                        command.ExecuteNonQuery();
                        connection.Close();

                        MessageBox.Show("Update Sccessful");
                        UpdateGrid("SELECT Inventory.ItemID, Item.ItemName, Item.Category, Inventory.WarehouseID, Warehouse.WareName, Inventory.Quantity, Item.Unit, Inventory.ExpireDate"
                                     + " FROM Item, Inventory, Warehouse WHERE Inventory.ItemID = Item.ItemID AND Inventory.WarehouseID = Warehouse.WarehouseID");
                    }
                }

            }
            textBox6.Clear();


        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)         // Order List
        {
            ViewAppovalOrder ordersPage = new ViewAppovalOrder();
            this.Hide();
            ordersPage.ShowDialog();
            this.Show();

        }

        private void button2_Click(object sender, EventArgs e)      // Delivery Notes
        {
            WDeliveryNote1 deliveryNotePage = new WDeliveryNote1();
            this.Hide();
            deliveryNotePage.ShowDialog();
            this.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            restInfo restInfo = new restInfo();
            this.Hide();
            restInfo.ShowDialog();
            this.Show();
        }
    }
    }

