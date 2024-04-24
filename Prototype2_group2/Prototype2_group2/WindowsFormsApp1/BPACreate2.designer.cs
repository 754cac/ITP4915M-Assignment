namespace WindowsFormsApp1
{
    partial class BPACreate2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.AGMSbtnSubmit = new System.Windows.Forms.Button();
            this.AGMStxtSrearch = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.AGMScbbCategory = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.AGMScbbSubcategory = new System.Windows.Forms.ComboBox();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.itemID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.itemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.category = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.subcategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.select = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.AGMStxtPQuantity = new System.Windows.Forms.TextBox();
            this.AGMSbtnAdd = new System.Windows.Forms.Button();
            this.AGMStxtCondition = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.AGMSbtnSearch = new System.Windows.Forms.Button();
            this.AGMSbtnPrevious = new System.Windows.Forms.Button();
            this.AGMStxtPrice = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnBack = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PromisedQuantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.delete = new System.Windows.Forms.DataGridViewButtonColumn();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(187)))), ((int)(((byte)(38)))), ((int)(((byte)(73)))));
            this.panel3.Controls.Add(this.label10);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(2572, 144);
            this.panel3.TabIndex = 1;
            this.panel3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseDown);
            this.panel3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseMove);
            this.panel3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panel3_MouseUp);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label10.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.label10.Location = new System.Drawing.Point(46, 36);
            this.label10.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(584, 62);
            this.label10.TabIndex = 33;
            this.label10.Text = "Create Agreement ID";
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 10F);
            this.textBox1.Location = new System.Drawing.Point(587, 192);
            this.textBox1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.textBox1.MaximumSize = new System.Drawing.Size(4, 10);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(0, 38);
            this.textBox1.TabIndex = 4;
            // 
            // AGMSbtnSubmit
            // 
            this.AGMSbtnSubmit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.AGMSbtnSubmit.FlatAppearance.BorderSize = 0;
            this.AGMSbtnSubmit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AGMSbtnSubmit.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AGMSbtnSubmit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMSbtnSubmit.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AGMSbtnSubmit.Location = new System.Drawing.Point(1978, 1328);
            this.AGMSbtnSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AGMSbtnSubmit.Name = "AGMSbtnSubmit";
            this.AGMSbtnSubmit.Size = new System.Drawing.Size(323, 80);
            this.AGMSbtnSubmit.TabIndex = 6;
            this.AGMSbtnSubmit.Text = "Submit";
            this.AGMSbtnSubmit.UseVisualStyleBackColor = false;
            this.AGMSbtnSubmit.Click += new System.EventHandler(this.AGMSbtnSubmit_Click);
            // 
            // AGMStxtSrearch
            // 
            this.AGMStxtSrearch.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AGMStxtSrearch.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMStxtSrearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMStxtSrearch.Location = new System.Drawing.Point(59, 286);
            this.AGMStxtSrearch.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMStxtSrearch.Multiline = true;
            this.AGMStxtSrearch.Name = "AGMStxtSrearch";
            this.AGMStxtSrearch.Size = new System.Drawing.Size(468, 72);
            this.AGMStxtSrearch.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label2.Location = new System.Drawing.Point(46, 190);
            this.label2.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 62);
            this.label2.TabIndex = 13;
            this.label2.Text = "Item ID / Name :";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label4.Location = new System.Drawing.Point(121, 508);
            this.label4.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(301, 62);
            this.label4.TabIndex = 17;
            this.label4.Text = "Category :";
            // 
            // AGMScbbCategory
            // 
            this.AGMScbbCategory.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMScbbCategory.FormattingEnabled = true;
            this.AGMScbbCategory.ItemHeight = 61;
            this.AGMScbbCategory.Location = new System.Drawing.Point(507, 502);
            this.AGMScbbCategory.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMScbbCategory.Name = "AGMScbbCategory";
            this.AGMScbbCategory.Size = new System.Drawing.Size(505, 69);
            this.AGMScbbCategory.TabIndex = 19;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label5.Location = new System.Drawing.Point(41, 594);
            this.label5.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(396, 62);
            this.label5.TabIndex = 20;
            this.label5.Text = "Subcategory :";
            // 
            // AGMScbbSubcategory
            // 
            this.AGMScbbSubcategory.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMScbbSubcategory.FormattingEnabled = true;
            this.AGMScbbSubcategory.ItemHeight = 61;
            this.AGMScbbSubcategory.Location = new System.Drawing.Point(507, 594);
            this.AGMScbbSubcategory.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMScbbSubcategory.Name = "AGMScbbSubcategory";
            this.AGMScbbSubcategory.Size = new System.Drawing.Size(505, 69);
            this.AGMScbbSubcategory.TabIndex = 21;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeColumns = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView2.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.itemID,
            this.itemName,
            this.category,
            this.subcategory,
            this.select});
            this.dataGridView2.Location = new System.Drawing.Point(917, 192);
            this.dataGridView2.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersVisible = false;
            this.dataGridView2.RowHeadersWidth = 62;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(1445, 282);
            this.dataGridView2.TabIndex = 26;
            this.dataGridView2.VirtualMode = true;
            this.dataGridView2.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellContentClick);
            // 
            // itemID
            // 
            this.itemID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemID.DataPropertyName = "itemID";
            this.itemID.Frozen = true;
            this.itemID.HeaderText = "Item ID";
            this.itemID.MinimumWidth = 8;
            this.itemID.Name = "itemID";
            this.itemID.Width = 110;
            // 
            // itemName
            // 
            this.itemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.itemName.DataPropertyName = "ItemName";
            this.itemName.Frozen = true;
            this.itemName.HeaderText = "Item Name";
            this.itemName.MinimumWidth = 8;
            this.itemName.Name = "itemName";
            this.itemName.Width = 111;
            // 
            // category
            // 
            this.category.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.category.DataPropertyName = "Category";
            this.category.Frozen = true;
            this.category.HeaderText = "Category";
            this.category.MinimumWidth = 8;
            this.category.Name = "category";
            this.category.Width = 110;
            // 
            // subcategory
            // 
            this.subcategory.DataPropertyName = "SubCategory";
            this.subcategory.HeaderText = "Subcategory";
            this.subcategory.MinimumWidth = 10;
            this.subcategory.Name = "subcategory";
            // 
            // select
            // 
            this.select.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.select.HeaderText = "";
            this.select.MinimumWidth = 8;
            this.select.Name = "select";
            this.select.Width = 110;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label7.Location = new System.Drawing.Point(50, 658);
            this.label7.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(203, 62);
            this.label7.TabIndex = 27;
            this.label7.Text = "Items :";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label8.Location = new System.Drawing.Point(1135, 610);
            this.label8.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(546, 62);
            this.label8.TabIndex = 28;
            this.label8.Text = "Promised Quantity :";
            // 
            // AGMStxtPQuantity
            // 
            this.AGMStxtPQuantity.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AGMStxtPQuantity.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMStxtPQuantity.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMStxtPQuantity.Location = new System.Drawing.Point(1738, 612);
            this.AGMStxtPQuantity.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMStxtPQuantity.Multiline = true;
            this.AGMStxtPQuantity.Name = "AGMStxtPQuantity";
            this.AGMStxtPQuantity.Size = new System.Drawing.Size(342, 72);
            this.AGMStxtPQuantity.TabIndex = 29;
            // 
            // AGMSbtnAdd
            // 
            this.AGMSbtnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.AGMSbtnAdd.FlatAppearance.BorderSize = 0;
            this.AGMSbtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AGMSbtnAdd.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AGMSbtnAdd.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMSbtnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AGMSbtnAdd.Location = new System.Drawing.Point(2141, 612);
            this.AGMSbtnAdd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AGMSbtnAdd.Name = "AGMSbtnAdd";
            this.AGMSbtnAdd.Size = new System.Drawing.Size(210, 72);
            this.AGMSbtnAdd.TabIndex = 30;
            this.AGMSbtnAdd.Text = "ADD";
            this.AGMSbtnAdd.UseVisualStyleBackColor = false;
            this.AGMSbtnAdd.Click += new System.EventHandler(this.AGMSbtnAdd_Click);
            // 
            // AGMStxtCondition
            // 
            this.AGMStxtCondition.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AGMStxtCondition.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMStxtCondition.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMStxtCondition.Location = new System.Drawing.Point(423, 1156);
            this.AGMStxtCondition.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMStxtCondition.Multiline = true;
            this.AGMStxtCondition.Name = "AGMStxtCondition";
            this.AGMStxtCondition.Size = new System.Drawing.Size(1003, 252);
            this.AGMStxtCondition.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label9.Location = new System.Drawing.Point(46, 1144);
            this.label9.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(342, 62);
            this.label9.TabIndex = 32;
            this.label9.Text = "Conditions :";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.Color.White;
            this.dataGridView1.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial Rounded MT Bold", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.Price,
            this.PromisedQuantity,
            this.delete});
            this.dataGridView1.Location = new System.Drawing.Point(54, 728);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 62;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(2297, 390);
            this.dataGridView1.TabIndex = 33;
            this.dataGridView1.VirtualMode = true;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // AGMSbtnSearch
            // 
            this.AGMSbtnSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.AGMSbtnSearch.FlatAppearance.BorderSize = 0;
            this.AGMSbtnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AGMSbtnSearch.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AGMSbtnSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMSbtnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AGMSbtnSearch.Location = new System.Drawing.Point(587, 286);
            this.AGMSbtnSearch.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AGMSbtnSearch.Name = "AGMSbtnSearch";
            this.AGMSbtnSearch.Size = new System.Drawing.Size(295, 72);
            this.AGMSbtnSearch.TabIndex = 34;
            this.AGMSbtnSearch.Text = "Search";
            this.AGMSbtnSearch.UseVisualStyleBackColor = false;
            this.AGMSbtnSearch.Click += new System.EventHandler(this.AGMSbtnSearch_Click);
            // 
            // AGMSbtnPrevious
            // 
            this.AGMSbtnPrevious.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.AGMSbtnPrevious.FlatAppearance.BorderSize = 0;
            this.AGMSbtnPrevious.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AGMSbtnPrevious.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AGMSbtnPrevious.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMSbtnPrevious.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.AGMSbtnPrevious.Location = new System.Drawing.Point(1599, 1328);
            this.AGMSbtnPrevious.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.AGMSbtnPrevious.Name = "AGMSbtnPrevious";
            this.AGMSbtnPrevious.Size = new System.Drawing.Size(323, 80);
            this.AGMSbtnPrevious.TabIndex = 35;
            this.AGMSbtnPrevious.Text = "previous";
            this.AGMSbtnPrevious.UseVisualStyleBackColor = false;
            this.AGMSbtnPrevious.Click += new System.EventHandler(this.AGMSbtnPrevious_Click);
            // 
            // AGMStxtPrice
            // 
            this.AGMStxtPrice.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AGMStxtPrice.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.AGMStxtPrice.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.AGMStxtPrice.Location = new System.Drawing.Point(1738, 510);
            this.AGMStxtPrice.Margin = new System.Windows.Forms.Padding(7, 6, 7, 6);
            this.AGMStxtPrice.Multiline = true;
            this.AGMStxtPrice.Name = "AGMStxtPrice";
            this.AGMStxtPrice.Size = new System.Drawing.Size(342, 72);
            this.AGMStxtPrice.TabIndex = 37;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 20F);
            this.label1.Location = new System.Drawing.Point(1512, 518);
            this.label1.Margin = new System.Windows.Forms.Padding(7, 0, 7, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 62);
            this.label1.TabIndex = 36;
            this.label1.Text = "Price :";
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBack.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            this.btnBack.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnBack.Location = new System.Drawing.Point(54, 1332);
            this.btnBack.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(323, 76);
            this.btnBack.TabIndex = 88;
            this.btnBack.Text = "Cencel";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ItemID";
            this.dataGridViewTextBoxColumn2.HeaderText = "Item ID";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ItemName";
            this.dataGridViewTextBoxColumn3.HeaderText = "Item Name";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Category";
            this.dataGridViewTextBoxColumn4.HeaderText = "Category";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "SubCategory";
            this.dataGridViewTextBoxColumn5.HeaderText = "Subcategory";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 8;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "Price";
            this.Price.MinimumWidth = 10;
            this.Price.Name = "Price";
            // 
            // PromisedQuantity
            // 
            this.PromisedQuantity.DataPropertyName = "PromisedQuantity";
            this.PromisedQuantity.HeaderText = "Promised Quantity";
            this.PromisedQuantity.MinimumWidth = 10;
            this.PromisedQuantity.Name = "PromisedQuantity";
            // 
            // delete
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("新細明體", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(136)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(213)))), ((int)(((byte)(202)))), ((int)(((byte)(209)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(70)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.White;
            this.delete.DefaultCellStyle = dataGridViewCellStyle3;
            this.delete.HeaderText = "";
            this.delete.MinimumWidth = 8;
            this.delete.Name = "delete";
            this.delete.Text = "Delete";
            this.delete.UseColumnTextForButtonValue = true;
            // 
            // BPACreate2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(13F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2572, 1612);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.AGMStxtPrice);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AGMSbtnPrevious);
            this.Controls.Add(this.AGMSbtnSearch);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.AGMStxtCondition);
            this.Controls.Add(this.AGMSbtnAdd);
            this.Controls.Add(this.AGMStxtPQuantity);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.AGMScbbSubcategory);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.AGMScbbCategory);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AGMStxtSrearch);
            this.Controls.Add(this.AGMSbtnSubmit);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.panel3);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "BPACreate2";
            this.Text = "Form2";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.BPACreate2_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button AGMSbtnSubmit;
        private System.Windows.Forms.TextBox AGMStxtSrearch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox AGMScbbCategory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox AGMScbbSubcategory;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox AGMStxtPQuantity;
        private System.Windows.Forms.Button AGMSbtnAdd;
        private System.Windows.Forms.TextBox AGMStxtCondition;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button AGMSbtnSearch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button AGMSbtnPrevious;
        private System.Windows.Forms.TextBox AGMStxtPrice;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemID;
        private System.Windows.Forms.DataGridViewTextBoxColumn itemName;
        private System.Windows.Forms.DataGridViewTextBoxColumn category;
        private System.Windows.Forms.DataGridViewTextBoxColumn subcategory;
        private System.Windows.Forms.DataGridViewCheckBoxColumn select;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn PromisedQuantity;
        private System.Windows.Forms.DataGridViewButtonColumn delete;
    }
}