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

namespace WindowsFormsApp1
{
    public partial class itemreport : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dt = new DataTable();

        string itemID;

        public itemreport(String itemid)
        {
            InitializeComponent();
            this.itemID = itemid;
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

        private void itemreport_Load(object sender, EventArgs e)
        {
            sqlStr = $"SELECT * FROM Item WHERE ItemID = '{itemID}' ";
            sqlSelection(sqlStr, dt);
            textBox3.Text = itemID;
            textBox1.Text = dt.Rows[0]["ItemName"].ToString();
            textBox2.Text = dt.Rows[0]["Category"].ToString();
            textBox4.Text = dt.Rows[0]["Unit"].ToString();

            dt = new DataTable();
            sqlStr = $"SELECT SUM(Quantity) FROM Inventory WHERE ItemID = '{itemID}' GROUP BY ItemID ";
            sqlSelection(sqlStr, dt);
            textBox5.Text = dt.Rows[0][0].ToString();

            dt = new DataTable();
            sqlStr = $"SELECT SUM(Quantity) FROM Requisition, OrderLine " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND ItemID = @itemID " +
                     $"AND RequestDate BETWEEN @date AND @today ";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.SelectCommand.Parameters.Add("@itemID", OleDbType.VarChar, 30).Value = itemID;
            dataAdapter.SelectCommand.Parameters.Add("@today", OleDbType.Date, 30).Value = DateTime.Today.AddMonths(-1);
            dataAdapter.SelectCommand.Parameters.Add("@today", OleDbType.Date, 30).Value = DateTime.Today.ToShortDateString();
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();

            if (dt.Rows[0][0].ToString() == "") textBox6.Text = "0";
            else textBox6.Text = dt.Rows[0][0].ToString();

            dt = new DataTable();
            sqlStr = $"SELECT SUM(Quantity) FROM Requisition, OrderLine " +
                     $"WHERE Requisition.RequisitionID = OrderLine.RequestID " +
                     $"AND ItemID = @itemID " +
                     $"AND RequestDate BETWEEN @date AND @today ";
            dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.SelectCommand.Parameters.Add("@itemID", OleDbType.VarChar, 30).Value = itemID;
            dataAdapter.SelectCommand.Parameters.Add("@today", OleDbType.Date, 30).Value = DateTime.Today.AddYears(-1);
            dataAdapter.SelectCommand.Parameters.Add("@today", OleDbType.Date, 30).Value = DateTime.Today.ToShortDateString();
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();

            if (dt.Rows[0][0].ToString() == "") textBox7.Text = "0";
            else textBox7.Text = dt.Rows[0][0].ToString();
        }


        //////////////////////////////////////////  Own Method  ///////////////////////////////////////////
        
        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        //////////////////////////////////////////  Property  ///////////////////////////////////////////
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        
    }
}
