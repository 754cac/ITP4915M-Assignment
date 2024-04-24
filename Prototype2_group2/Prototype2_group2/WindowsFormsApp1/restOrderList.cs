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
    public partial class restOrderList : Form
    {
        public Point mouseLocation;

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        private DataTable dtReqID = new DataTable();
        private DataTable dtOrderLine = new DataTable();
        private DataTable dtRequest = new DataTable();
        DataTable dt = new DataTable();

        string employeeID;
        string restaurantID;       
        string brandID;                   
        string requisitionID;

        Boolean withReqID = false;
        Boolean withStatus = false;
        Boolean withDate = false;

        Boolean processing = false;
        Boolean rejected = false;
        Boolean approved = false;
        Boolean delivering = false;

        public restOrderList()
        {
            InitializeComponent();
        }

        public restOrderList(string employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
        }

        private void mouse_Down(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void mouse_Move(object sender, MouseEventArgs e)
        {
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void restOrderList_Load(object sender, EventArgs e)
        {
            setRestaurantID();
            setBrandID();

            OleDbConnection connection = new OleDbConnection(connStr);
            connection.Open();
            requisitionInitialization(connection);
            connection.Close();

            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestaurantID, Status FROM Requisition " +
                     $"WHERE RestaurantID = '{restaurantID}'";
            sqlSelection(sqlStr, dtRequest);
            dataGridView1.DataSource = dtRequest;
            cleanUp();
            //////////////////////////////// in the works ////////////////////////////////////////
            button10.Hide(); button6.Hide(); button4.Hide();
        }

        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////
        
        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtRequest.Clear();
            sqlStr = $"SELECT RequisitionID, RequestDate, ExpectedDate, RestaurantID, Status FROM Requisition " +
                     $"WHERE RestaurantID = '{restaurantID}'";
            string idInput = (txtSearch.Text.TrimStart(' ')).TrimStart('0');
            if (!int.TryParse(idInput, out int num) && !idInput.Equals("Requisition ID"))
                MessageBox.Show("Please input a requisition ID for searching");
            else
            {
                if (!string.IsNullOrEmpty(idInput) && !idInput.Equals("Requisition ID")) // Searching bar not empty
                {
                    idInput = string.Format("{0:000}", Convert.ToInt32(idInput));
                    sqlStr += " AND RequisitionID = @reqID";
                    withReqID = true;
                }
                if (cbStatus.SelectedIndex > -1)
                {
                    sqlStr += " AND Status = @status";
                    withStatus = true;
                }
                if (withDate)
                    sqlStr += " AND RequestDate = @date";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
                if (withReqID)
                    dataAdapter.SelectCommand.Parameters.Add("@reqID", OleDbType.VarChar, 30).Value = idInput;
                if (withStatus)
                    dataAdapter.SelectCommand.Parameters.Add("@status", OleDbType.VarChar, 30).Value = cbStatus.Text;
                if (withDate)
                    dataAdapter.SelectCommand.Parameters.Add("@date", OleDbType.Date, 30).Value = dtpReqDate.Value.ToShortDateString();

                dataAdapter.Fill(dtRequest);
                dataAdapter.Dispose();
                dataGridView1.DataSource = dtRequest;
            }
            cleanUp();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cleanUp();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            RcreateOrder createOrder = new RcreateOrder(this, brandID, restaurantID);
            this.Hide();
            createOrder.ShowDialog();
            this.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)     // EDIT
        {
            if (dtRequest.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    requisitionID = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                    restaurantID = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
                    form4 orderDetail = new form4(this, requisitionID, restaurantID);
                    this.Hide();
                    orderDetail.ShowDialog();
                    this.Show();
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)         // Delivery Note
        {
            WDeliveryNote notesPage = new WDeliveryNote(employeeID);
            this.Hide();
            notesPage.ShowDialog();
            this.Show();
        }
        ////////////////////////////////////////  Own Methods  ////////////////////////////////////////////////////

        public void setRestaurantID()
        {
            sqlStr = "SELECT UserID FROM Account WHERE EmployeeID = '" + employeeID + "'";
            executeSql(sqlStr);
            sqlStr = "SELECT RestaurantID FROM Restaurant WHERE UserID = '" + dt.Rows[0]["UserID"].ToString() + "'";
            dt.Clear();
            executeSql(sqlStr);
            restaurantID = dt.Rows[0]["RestaurantID"].ToString();
            dt.Clear();
        }

        public void setBrandID()
        {
            sqlStr = "SELECT BrandID FROM Restaurant WHERE RestaurantID = '" + restaurantID + "'";
            executeSql(sqlStr);
            brandID = dt.Rows[0]["BrandID"].ToString();
            dt.Clear();
        }

        private void requisitionInitialization(OleDbConnection connection)
        {
            sqlStr = $"SELECT RequisitionID FROM Requisition WHERE RestaurantID = '{restaurantID}'";
            sqlSelection(sqlStr, dtReqID);
            for (int i = 0; i < dtReqID.Rows.Count; i++)
            {
                requisitionID = dtReqID.Rows[i]["RequisitionID"].ToString();

                sqlStr = $"SELECT OrderLine.Status FROM OrderLine WHERE RequestID = '{requisitionID}'";
                sqlSelection(sqlStr, dtOrderLine);

                for (int j = 0; j < dtOrderLine.Rows.Count; j++)
                {
                    if (dtOrderLine.Rows[j]["Status"].ToString().Equals("Rejected"))
                    {
                        updateStatus(connection, requisitionID, "Rejected");
                        rejected = true;
                        break;
                    }
                    else if (dtOrderLine.Rows[j]["Status"].ToString().Equals("Processing"))
                    {
                        processing = true;
                    }
                    else if (dtOrderLine.Rows[j]["Status"].ToString().Equals("Approved"))
                    {
                        approved = true;
                    }
                    else if (dtOrderLine.Rows[j]["Status"].ToString().Equals("Delivering"))
                    {
                        delivering = true;
                    }
                }
                if (rejected)
                    booleanReset();
                else if (processing)
                {
                    updateStatus(connection, requisitionID, "Processing");
                    booleanReset();
                }
                else if (approved)
                {
                    updateStatus(connection, requisitionID, "Approved");
                    booleanReset();
                }
                else if (delivering)
                {
                    updateStatus(connection, requisitionID, "Delivering");
                    booleanReset();
                }
                else
                {
                    updateStatus(connection, requisitionID, "Completed");
                    booleanReset();
                }
                dtOrderLine.Clear();
            }
            dtReqID.Clear();
        }

        private void updateStatus(OleDbConnection connection, string requisitionID, string status)
        {
            sqlStr = $"UPDATE Requisition SET Status = '{status}' " +
                     $"WHERE RequisitionID = '{requisitionID}'";
            OleDbCommand command = new OleDbCommand(sqlStr, connection);
            command.ExecuteNonQuery();
        }

        private void booleanReset()
        {
            processing = false;
            rejected = false;
            approved = false;
            delivering = false;
        }

        private void cleanUp()
        {
            txtSearch.Text = "Requisition ID";
            txtSearch.ForeColor = Color.FromArgb(213, 202, 209);
            cbPriority.SelectedIndex = -1;
            cbPriority.Text = "";
            cbStatus.SelectedIndex = -1;
            cbStatus.Text = "";
            dtpReqDate.CustomFormat = " ";
            dtpReqDate.Format = DateTimePickerFormat.Custom;
            withReqID = false;
            withStatus = false;
            withDate = false;
        }

        private void sqlSelection(string sql, DataTable dt)
        {
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        public void executeSql(string sql)
        {
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dt);
            dataAdapter.Dispose();
        }

        public void showEditedRequisition(string sql) {
            dtRequest.Clear();
            sqlStr = sql;
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtRequest);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dtRequest;
        }
        ////////////////////////////////////////  Property  ////////////////////////////////////////////////////

        private void dtpReqDate_ValueChanged(object sender, EventArgs e)
        {   
            withDate = true;
            dtpReqDate.CustomFormat = "yyyy/MM/dd";
        }

        private void txtSearch_Enter(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            txtSearch.ForeColor = Color.Black;
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                txtSearch.Text = "Requisition ID";
                txtSearch.ForeColor = Color.FromArgb(213, 202, 209);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)          // Close button
        {
            this.Close();
        }

    }
}
