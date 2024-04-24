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
    public partial class WDeliveryNote : Form
    {
        private bool dragging = false;
        private Point startPoint = new Point(0, 0);

        string connStr = "Provider=Microsoft.Jet.OLEDB.4.0;"
                       + "Data Source=database.mdb";
        string sqlStr = "";
        private DataTable dt = new DataTable();
        private DataTable dtDeliveryNote = new DataTable();

        string employeeID;
        string restaurantID;
        string deliveryNoteID = "";

        Boolean withDate = false;

        public WDeliveryNote()
        {
            InitializeComponent();
        }

        public WDeliveryNote(string employeeID)
        {
            InitializeComponent();
            this.employeeID = employeeID;
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

        private void WDeliveryNote_Load(object sender, EventArgs e)
        {
            setRestaurantID();
            sqlStr = $"SELECT DeliveryNoteID, DeliveryNoteDate, RegistrationPlateID, ContactNo, RestaurantSignature FROM DeliveryNote " +
                     $"WHERE RestaurantID = '{restaurantID}'";
            sqlSelection(sqlStr, dtDeliveryNote);
            dataGridView1.DataSource = dtDeliveryNote;
            cleanUp();
            //////////////////////////////// in the works ////////////////////////////////////////
            button10.Hide(); button6.Hide(); button4.Hide(); button11.Hide();
        }

        ////////////////////////////////////////  Event Handlers  //////////////////////////////////////////////////

        private void btnSearch_Click(object sender, EventArgs e)
        {
            dtDeliveryNote.Clear();
            sqlStr = $"SELECT DeliveryNoteID, DeliveryNoteDate, RegistrationPlateID, ContactNo, RestaurantSignature FROM DeliveryNote " +
                     $"WHERE RestaurantID = '{restaurantID}'";
            string idInput = (txtSearch.Text.TrimStart(' ')).TrimStart('0');
            if (withDate)
            {
                sqlStr += " AND DeliveryNoteDate = @date";
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
                dataAdapter.SelectCommand.Parameters.Add("@date", OleDbType.Date, 30).Value = dtpReqDate.Value.ToShortDateString();
                dataAdapter.Fill(dtDeliveryNote);
                dataAdapter.Dispose();
                dataGridView1.DataSource = dtDeliveryNote;
            }
            else if (string.IsNullOrEmpty(idInput))
            {
                sqlSelection(sqlStr, dtDeliveryNote);
                dataGridView1.DataSource = dtDeliveryNote;
            }
            else if (int.TryParse(idInput, out int num))
            {
                idInput = string.Format("{0:000}", Convert.ToInt32(idInput));
                sqlStr += $" AND DeliveryNoteID = '{idInput}'";
                sqlSelection(sqlStr, dtDeliveryNote);
                dataGridView1.DataSource = dtDeliveryNote;
            }
            else
                MessageBox.Show("Please input a Delivery Note ID for searching");

            cleanUp();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            cleanUp();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dtDeliveryNote.Rows.Count > 0 && e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    deliveryNoteID = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                }
            }
        }

        private void button7_Click(object sender, EventArgs e)          // OrderList Button
        {
            restOrderList orderListPage = new restOrderList(employeeID);
            this.Hide();
            orderListPage.ShowDialog();
            this.Show();
        }

        private void btnCreate_Click(object sender, EventArgs e)        // Detail Button
        {
            if(deliveryNoteID != "")
            {
                WDeliveryNoteDetail noteDetailsPage = new WDeliveryNoteDetail(this, deliveryNoteID, restaurantID);
                this.Hide();
                noteDetailsPage.ShowDialog();
                this.Show();
            }
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

        private void cleanUp()
        {
            txtSearch.Text = "";
            dtpReqDate.CustomFormat = " ";
            dtpReqDate.Format = DateTimePickerFormat.Custom;
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

        public void showEditedDeliveryNote()
        {
            dtDeliveryNote.Clear();
            sqlStr = $"SELECT DeliveryNoteID, DeliveryNoteDate, RegistrationPlateID, ContactNo, RestaurantSignature FROM DeliveryNote " +
                     $"WHERE DeliveryNoteID = '{deliveryNoteID}'";
            OleDbDataAdapter dataAdapter = new OleDbDataAdapter(sqlStr, connStr);
            dataAdapter.Fill(dtDeliveryNote);
            dataAdapter.Dispose();
            dataGridView1.DataSource = dtDeliveryNote;
        }

        ////////////////////////////////////////  Property  ////////////////////////////////////////////////////

        private void panel3_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnHide_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void dtpReqDate_ValueChanged(object sender, EventArgs e)
        {
            withDate = true;
            dtpReqDate.CustomFormat = "yyyy/MM/dd";
        }

        
    }
}
