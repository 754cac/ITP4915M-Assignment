using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WindowsFormsApp1
{
    public partial class BPAReport : Form
    {
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                      + "Data Source=database.mdb";
        string sqlStr = "";

        DataTable dt2 = new DataTable();
        DataTable dt3 = new DataTable();
        DataTable dt4 = new DataTable();
        String agrID;

        OleDbDataAdapter dataAdapter;
        OleDbConnection connection;
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        public BPAReport(String Agreement)
        {
            InitializeComponent();
            this.agrID = Agreement;
            connection = new OleDbConnection(connStr);
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            dragging = true;
            startPoint = new Point(e.X, e.Y);
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.startPoint.X, p.Y - this.startPoint.Y);
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void BPAReport_Load(object sender, EventArgs e)
        {
            sqlExecution("DELETE FROM BPAreport WHERE ItemID IS NOT NULL");
            dataGridView1.Rows.Clear();
            dt2.Clear();
            sqlStr = $"SELECT * FROM Contract WHERE AgreementID = '{agrID}'";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt2);
            dataAdapter.Dispose();
            foreach (DataRow row2 in dt2.Rows)
            {
                textBox3.Text = row2["AgreementID"].ToString();
                textBox1.Text = row2["SupplierID"].ToString();
                textBox2.Text = row2["EmployeeID"].ToString();
                textBox4.Text = row2["CreateDate"].ToString();
                textBox5.Text = row2["ConExpiryDate"].ToString();

            }

                dt3.Clear();
                sqlStr = $"SELECT ContractLine.ItemID,Item.ItemName, ContractLine.Price, ContractLine.OrderedQuantity" +
                    $" FROM ContractLine, Item WHERE ContractLine.ItemID = Item.ItemID" +
                    $" AND AgreementID = '{agrID}'";
                OleDbDataAdapter dataAdapter3 = new OleDbDataAdapter(sqlStr, connStr);
                dataAdapter3.Fill(dt3);
                dataAdapter3.Dispose();
            if (dt3.Rows.Count > 0)
            {
                foreach (DataRow row3 in dt3.Rows)
                {
                    double totalBPA = 0;
                    double totalCPA = 0;
                    double Profit = 0;
                    totalBPA = (double)double.Parse(row3["Price"].ToString()) * double.Parse(row3["OrderedQuantity"].ToString());
                    dt4.Clear();
                    sqlStr = $"SELECT ContractLine.Price" +
                        $" FROM ContractLine, Contract WHERE ContractLine.AgreementID = Contract.AgreementID" +
                        $" AND ContractLine.ItemID = '{row3["ItemID"].ToString()}' AND Contract.AgreementType = 'CPA'";
                    OleDbDataAdapter dataAdapter4 = new OleDbDataAdapter(sqlStr, connStr);
                    dataAdapter4.Fill(dt4);
                    dataAdapter4.Dispose();
                    if (dt4.Rows.Count > 0)
                    {
                        DataRow row4 = dt4.Rows[0];
                        totalCPA = (double)double.Parse(row4["Price"].ToString()) * double.Parse(row3["OrderedQuantity"].ToString());

                        Profit = totalCPA - totalBPA;

                        sqlExecution($"INSERT INTO BPAreport VALUES('{row3["ItemID"].ToString()}', '{row3["ItemName"].ToString()}'" +
                            $", '{row3["OrderedQuantity"].ToString()}', '{totalBPA}', '{totalCPA}', '{Profit}')");
                    }
                    else
                    {
                        sqlExecution($"INSERT INTO BPAreport VALUES('{row3["ItemID"].ToString()}', '{row3["ItemName"].ToString()}'" +
                            $", '{row3["OrderedQuantity"].ToString()}', '{totalBPA}', '0', '0')");
                    }


                }
            }
            else
            {
                sqlExecution("DELETE FROM BPAreport WHERE ItemID IS NOT NULL");
            }

            UpdateGrid("SELECT * FROM BPAreport");




        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
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
    }
}
