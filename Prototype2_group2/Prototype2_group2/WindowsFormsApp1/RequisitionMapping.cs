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
    public partial class RequisitionMapping : Form
    {
        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        DataTable dtOrderLine = new DataTable();
        DataTable dtInventory = new DataTable();

        string itemID;
        int requestQty;
        string warehouseID;
        string updatedQty;

        public RequisitionMapping()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dtOrderLine.Clear();
            OleDbConnection connection = new OleDbConnection(connStr);
            OleDbCommand command;
            connection.Open();

            sqlStr = "SELECT * FROM OrderLine WHERE Status = 'Processing'";
            sqlSelection(sqlStr, dtOrderLine);

            for (int i = 0; i < dtOrderLine.Rows.Count; i++)
            {
                itemID = dtOrderLine.Rows[i]["ItemID"].ToString();
                requestQty = int.Parse(dtOrderLine.Rows[i]["Quantity"].ToString());
                dtInventory.Clear();
                sqlStr = $"SELECT * FROM Inventory WHERE ItemID = '{itemID}' ORDER BY ExpireDate";
                sqlSelection(sqlStr, dtInventory);
                for (int j = 0; j < dtInventory.Rows.Count; j++)
                {
                    int inventoryQty = int.Parse(dtInventory.Rows[j]["Quantity"].ToString());
                    if (inventoryQty >= requestQty)
                    {
                        warehouseID = dtInventory.Rows[j]["WarehouseID"].ToString();
                        updatedQty = (inventoryQty - requestQty).ToString();
                        sqlStr = $"UPDATE Inventory SET Quantity = '{updatedQty}' WHERE ItemID = '{itemID}' " +
                                                                             $"AND WarehouseID = '{warehouseID}'";
                        command = new OleDbCommand(sqlStr, connection);
                        command.ExecuteNonQuery();
                        dtOrderLine.Rows[i]["Status"] = "Approved";
                        break;
                    }   // will be improved later on 
                    dtOrderLine.Rows[i]["Status"] = "Rejected";
                }
            }

            for (int i = 0; i < dtOrderLine.Rows.Count; i++)
            {
                sqlStr = $"UPDATE OrderLine SET Status = '{dtOrderLine.Rows[i]["Status"]}' " +
                                   $"WHERE OrderLineID = '{dtOrderLine.Rows[i]["OrderLineID"]}'";
                command = new OleDbCommand(sqlStr, connection);
                command.ExecuteNonQuery();
            }
            connection.Close();
            MessageBox.Show("Mapping finish !");
        }


        private void sqlSelection(string sqlStr, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
