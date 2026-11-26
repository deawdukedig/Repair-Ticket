namespace RepairTicket.UI
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Button btnShopSettings;
        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpJobEntry;
        private System.Windows.Forms.Label lblJobNo;
        private System.Windows.Forms.TextBox txtJobNo;
        private System.Windows.Forms.Label lblJobDate;
        private System.Windows.Forms.TextBox txtJobDate;
        private System.Windows.Forms.Label lblCustomerName;
        private System.Windows.Forms.TextBox txtCustomerName;
        private System.Windows.Forms.Label lblCustomerPhone;
        private System.Windows.Forms.TextBox txtCustomerPhone;
        private System.Windows.Forms.Label lblDeviceSymptom;
        private System.Windows.Forms.TextBox txtDeviceSymptom;
        private System.Windows.Forms.FlowLayoutPanel pnlWordSuggestions;
        private System.Windows.Forms.Label lblPrintFormat;
        private System.Windows.Forms.ComboBox cboPrintFormat;
        private System.Windows.Forms.Label lblTargetPrinter;
        private System.Windows.Forms.ComboBox cboTargetPrinter;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSaveAndPrint;
        private System.Windows.Forms.Button btnPrintAgain;
        private System.Windows.Forms.GroupBox grpSearchAndList;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvRepairJobs;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dgvHeaderStyle = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dgvRowStyle = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.btnShopSettings = new System.Windows.Forms.Button();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpJobEntry = new System.Windows.Forms.GroupBox();
            this.btnPrintAgain = new System.Windows.Forms.Button();
            this.btnSaveAndPrint = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.cboPrintFormat = new System.Windows.Forms.ComboBox();
            this.lblPrintFormat = new System.Windows.Forms.Label();
            this.lblTargetPrinter = new System.Windows.Forms.Label();
            this.cboTargetPrinter = new System.Windows.Forms.ComboBox();
            this.pnlWordSuggestions = new System.Windows.Forms.FlowLayoutPanel();
            this.txtDeviceSymptom = new System.Windows.Forms.TextBox();
            this.lblDeviceSymptom = new System.Windows.Forms.Label();
            this.txtCustomerName = new System.Windows.Forms.TextBox();
            this.lblCustomerName = new System.Windows.Forms.Label();
            this.txtCustomerPhone = new System.Windows.Forms.TextBox();
            this.lblCustomerPhone = new System.Windows.Forms.Label();
            this.txtJobDate = new System.Windows.Forms.TextBox();
            this.lblJobDate = new System.Windows.Forms.Label();
            this.txtJobNo = new System.Windows.Forms.TextBox();
            this.lblJobNo = new System.Windows.Forms.Label();
            this.grpSearchAndList = new System.Windows.Forms.GroupBox();
            this.dgvRepairJobs = new System.Windows.Forms.DataGridView();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpJobEntry.SuspendLayout();
            this.grpSearchAndList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepairJobs)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(50)))), ((int)(((byte)(100)))));
            this.pnlHeader.Controls.Add(this.lblAppTitle);
            this.pnlHeader.Controls.Add(this.btnShopSettings);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1264, 70);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = System.Drawing.Color.White;
            this.lblAppTitle.Location = new System.Drawing.Point(16, 14);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(608, 41);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "🛠️ ระบบออกใบรับซ่อมสินค้า (Repair Ticket System)";
            // 
            // btnShopSettings
            // 
            this.btnShopSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShopSettings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.btnShopSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnShopSettings.FlatAppearance.BorderSize = 0;
            this.btnShopSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShopSettings.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShopSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.btnShopSettings.Location = new System.Drawing.Point(1014, 12);
            this.btnShopSettings.Name = "btnShopSettings";
            this.btnShopSettings.Size = new System.Drawing.Size(238, 46);
            this.btnShopSettings.TabIndex = 1;
            this.btnShopSettings.Text = "⚙️ ตั้งค่าข้อมูลร้าน";
            this.btnShopSettings.UseVisualStyleBackColor = false;
            this.btnShopSettings.Click += new System.EventHandler(this.btnShopSettings_Click);
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 70);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grpJobEntry);
            this.splitMain.Panel1.Padding = new System.Windows.Forms.Padding(12);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.grpSearchAndList);
            this.splitMain.Panel2.Padding = new System.Windows.Forms.Padding(12);
            this.splitMain.Size = new System.Drawing.Size(1264, 785);
            this.splitMain.SplitterDistance = 610;
            this.splitMain.TabIndex = 1;
            // 
            // grpJobEntry
            // 
            this.grpJobEntry.Controls.Add(this.btnPrintAgain);
            this.grpJobEntry.Controls.Add(this.btnSaveAndPrint);
            this.grpJobEntry.Controls.Add(this.btnClear);
            this.grpJobEntry.Controls.Add(this.cboTargetPrinter);
            this.grpJobEntry.Controls.Add(this.lblTargetPrinter);
            this.grpJobEntry.Controls.Add(this.cboPrintFormat);
            this.grpJobEntry.Controls.Add(this.lblPrintFormat);
            this.grpJobEntry.Controls.Add(this.pnlWordSuggestions);
            this.grpJobEntry.Controls.Add(this.txtDeviceSymptom);
            this.grpJobEntry.Controls.Add(this.lblDeviceSymptom);
            this.grpJobEntry.Controls.Add(this.txtCustomerPhone);
            this.grpJobEntry.Controls.Add(this.lblCustomerPhone);
            this.grpJobEntry.Controls.Add(this.txtCustomerName);
            this.grpJobEntry.Controls.Add(this.lblCustomerName);
            this.grpJobEntry.Controls.Add(this.txtJobDate);
            this.grpJobEntry.Controls.Add(this.lblJobDate);
            this.grpJobEntry.Controls.Add(this.txtJobNo);
            this.grpJobEntry.Controls.Add(this.lblJobNo);
            this.grpJobEntry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpJobEntry.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpJobEntry.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.grpJobEntry.Location = new System.Drawing.Point(12, 12);
            this.grpJobEntry.Name = "grpJobEntry";
            this.grpJobEntry.Size = new System.Drawing.Size(586, 761);
            this.grpJobEntry.TabIndex = 0;
            this.grpJobEntry.TabStop = false;
            this.grpJobEntry.Text = "📝 ข้อมูลใบงานรับซ่อม";
            // 
            // btnPrintAgain
            // 
            this.btnPrintAgain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPrintAgain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(162)))), ((int)(((byte)(184)))));
            this.btnPrintAgain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrintAgain.Enabled = false;
            this.btnPrintAgain.FlatAppearance.BorderSize = 0;
            this.btnPrintAgain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrintAgain.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnPrintAgain.ForeColor = System.Drawing.Color.White;
            this.btnPrintAgain.Location = new System.Drawing.Point(366, 700);
            this.btnPrintAgain.Name = "btnPrintAgain";
            this.btnPrintAgain.Size = new System.Drawing.Size(204, 48);
            this.btnPrintAgain.TabIndex = 13;
            this.btnPrintAgain.Text = "🖨️ พิมพ์ใบงานซ้ำ";
            this.btnPrintAgain.UseVisualStyleBackColor = false;
            this.btnPrintAgain.Click += new System.EventHandler(this.btnPrintAgain_Click);
            // 
            // btnSaveAndPrint
            // 
            this.btnSaveAndPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAndPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnSaveAndPrint.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSaveAndPrint.FlatAppearance.BorderSize = 0;
            this.btnSaveAndPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAndPrint.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnSaveAndPrint.ForeColor = System.Drawing.Color.White;
            this.btnSaveAndPrint.Location = new System.Drawing.Point(152, 700);
            this.btnSaveAndPrint.Name = "btnSaveAndPrint";
            this.btnSaveAndPrint.Size = new System.Drawing.Size(208, 48);
            this.btnSaveAndPrint.TabIndex = 12;
            this.btnSaveAndPrint.Text = "💾 บันทึกและพิมพ์";
            this.btnSaveAndPrint.UseVisualStyleBackColor = false;
            this.btnSaveAndPrint.Click += new System.EventHandler(this.btnSaveAndPrint_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnClear.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClear.FlatAppearance.BorderSize = 0;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnClear.ForeColor = System.Drawing.Color.White;
            this.btnClear.Location = new System.Drawing.Point(18, 700);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(126, 48);
            this.btnClear.TabIndex = 11;
            this.btnClear.Text = "🔄 เคลียร์";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // cboPrintFormat
            // 
            this.cboPrintFormat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPrintFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPrintFormat.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboPrintFormat.FormattingEnabled = true;
            this.cboPrintFormat.Items.AddRange(new object[] {
            "📄 กระดาษ A4 (แบ่ง 2 ส่วน: ร้านค้า / ลูกค้า)",
            "🧾 สลิป POS 58mm (Bitmap ภาษาไทยคมชัด)"});
            this.cboPrintFormat.Location = new System.Drawing.Point(220, 588);
            this.cboPrintFormat.Name = "cboPrintFormat";
            this.cboPrintFormat.Size = new System.Drawing.Size(350, 38);
            this.cboPrintFormat.TabIndex = 9;
            this.cboPrintFormat.SelectedIndexChanged += new System.EventHandler(this.cboPrintFormat_SelectedIndexChanged);
            // 
            // lblPrintFormat
            // 
            this.lblPrintFormat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPrintFormat.AutoSize = true;
            this.lblPrintFormat.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblPrintFormat.Location = new System.Drawing.Point(18, 592);
            this.lblPrintFormat.Name = "lblPrintFormat";
            this.lblPrintFormat.Size = new System.Drawing.Size(196, 30);
            this.lblPrintFormat.TabIndex = 8;
            this.lblPrintFormat.Text = "รูปแบบการพิมพ์:";
            // 
            // lblTargetPrinter
            // 
            this.lblTargetPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblTargetPrinter.AutoSize = true;
            this.lblTargetPrinter.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblTargetPrinter.Location = new System.Drawing.Point(18, 644);
            this.lblTargetPrinter.Name = "lblTargetPrinter";
            this.lblTargetPrinter.Size = new System.Drawing.Size(200, 30);
            this.lblTargetPrinter.TabIndex = 10;
            this.lblTargetPrinter.Text = "เครื่องพิมพ์ปลายทาง:";
            // 
            // cboTargetPrinter
            // 
            this.cboTargetPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTargetPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTargetPrinter.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboTargetPrinter.FormattingEnabled = true;
            this.cboTargetPrinter.Location = new System.Drawing.Point(220, 640);
            this.cboTargetPrinter.Name = "cboTargetPrinter";
            this.cboTargetPrinter.Size = new System.Drawing.Size(350, 38);
            this.cboTargetPrinter.TabIndex = 11;
            this.cboTargetPrinter.SelectedIndexChanged += new System.EventHandler(this.cboTargetPrinter_SelectedIndexChanged);
            // 
            // pnlWordSuggestions
            // 
            this.pnlWordSuggestions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlWordSuggestions.AutoScroll = true;
            this.pnlWordSuggestions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(244)))), ((int)(((byte)(250)))));
            this.pnlWordSuggestions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlWordSuggestions.Location = new System.Drawing.Point(18, 488);
            this.pnlWordSuggestions.Name = "pnlWordSuggestions";
            this.pnlWordSuggestions.Padding = new System.Windows.Forms.Padding(4);
            this.pnlWordSuggestions.Size = new System.Drawing.Size(552, 88);
            this.pnlWordSuggestions.TabIndex = 7;
            this.pnlWordSuggestions.WrapContents = true;
            // 
            // txtDeviceSymptom
            // 
            this.txtDeviceSymptom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDeviceSymptom.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtDeviceSymptom.Location = new System.Drawing.Point(18, 290);
            this.txtDeviceSymptom.Multiline = true;
            this.txtDeviceSymptom.Name = "txtDeviceSymptom";
            this.txtDeviceSymptom.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDeviceSymptom.Size = new System.Drawing.Size(552, 186);
            this.txtDeviceSymptom.TabIndex = 6;
            this.txtDeviceSymptom.TextChanged += new System.EventHandler(this.txtDeviceSymptom_TextChanged);
            this.txtDeviceSymptom.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDeviceSymptom_KeyUp);
            // 
            // lblDeviceSymptom
            // 
            this.lblDeviceSymptom.AutoSize = true;
            this.lblDeviceSymptom.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblDeviceSymptom.Location = new System.Drawing.Point(18, 254);
            this.lblDeviceSymptom.Name = "lblDeviceSymptom";
            this.lblDeviceSymptom.Size = new System.Drawing.Size(252, 30);
            this.lblDeviceSymptom.TabIndex = 6;
            this.lblDeviceSymptom.Text = "อุปกรณ์และอาการเสีย:";
            // 
            // txtCustomerName
            // 
            this.txtCustomerName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCustomerName.Location = new System.Drawing.Point(18, 214);
            this.txtCustomerName.Name = "txtCustomerName";
            this.txtCustomerName.Size = new System.Drawing.Size(265, 36);
            this.txtCustomerName.TabIndex = 4;
            // 
            // lblCustomerName
            // 
            this.lblCustomerName.AutoSize = true;
            this.lblCustomerName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCustomerName.Location = new System.Drawing.Point(18, 180);
            this.lblCustomerName.Name = "lblCustomerName";
            this.lblCustomerName.Size = new System.Drawing.Size(124, 30);
            this.lblCustomerName.TabIndex = 3;
            this.lblCustomerName.Text = "ชื่อลูกค้า:";
            // 
            // lblCustomerPhone
            // 
            this.lblCustomerPhone.AutoSize = true;
            this.lblCustomerPhone.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblCustomerPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(40)))), ((int)(((byte)(20)))));
            this.lblCustomerPhone.Location = new System.Drawing.Point(296, 180);
            this.lblCustomerPhone.Name = "lblCustomerPhone";
            this.lblCustomerPhone.Size = new System.Drawing.Size(210, 30);
            this.lblCustomerPhone.TabIndex = 5;
            this.lblCustomerPhone.Text = "📞 เบอร์โทรลูกค้า:";
            // 
            // txtCustomerPhone
            // 
            this.txtCustomerPhone.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCustomerPhone.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(253)))), ((int)(((byte)(240)))));
            this.txtCustomerPhone.Font = new System.Drawing.Font("Segoe UI", 16.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtCustomerPhone.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(160)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.txtCustomerPhone.Location = new System.Drawing.Point(296, 214);
            this.txtCustomerPhone.Name = "txtCustomerPhone";
            this.txtCustomerPhone.Size = new System.Drawing.Size(274, 37);
            this.txtCustomerPhone.TabIndex = 6;
            // 
            // txtJobDate
            // 
            this.txtJobDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJobDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtJobDate.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtJobDate.Location = new System.Drawing.Point(18, 134);
            this.txtJobDate.Name = "txtJobDate";
            this.txtJobDate.ReadOnly = true;
            this.txtJobDate.Size = new System.Drawing.Size(552, 36);
            this.txtJobDate.TabIndex = 3;
            // 
            // lblJobDate
            // 
            this.lblJobDate.AutoSize = true;
            this.lblJobDate.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblJobDate.Location = new System.Drawing.Point(18, 100);
            this.lblJobDate.Name = "lblJobDate";
            this.lblJobDate.Size = new System.Drawing.Size(147, 30);
            this.lblJobDate.TabIndex = 2;
            this.lblJobDate.Text = "วันที่รับซ่อม:";
            // 
            // txtJobNo
            // 
            this.txtJobNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJobNo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.txtJobNo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.txtJobNo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(204)))));
            this.txtJobNo.Location = new System.Drawing.Point(18, 56);
            this.txtJobNo.Name = "txtJobNo";
            this.txtJobNo.ReadOnly = true;
            this.txtJobNo.Size = new System.Drawing.Size(552, 36);
            this.txtJobNo.TabIndex = 1;
            // 
            // lblJobNo
            // 
            this.lblJobNo.AutoSize = true;
            this.lblJobNo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblJobNo.Location = new System.Drawing.Point(18, 24);
            this.lblJobNo.Name = "lblJobNo";
            this.lblJobNo.Size = new System.Drawing.Size(155, 30);
            this.lblJobNo.TabIndex = 0;
            this.lblJobNo.Text = "เลขที่ใบงาน:";
            // 
            // grpSearchAndList
            // 
            this.grpSearchAndList.Controls.Add(this.dgvRepairJobs);
            this.grpSearchAndList.Controls.Add(this.txtSearch);
            this.grpSearchAndList.Controls.Add(this.lblSearch);
            this.grpSearchAndList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpSearchAndList.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.grpSearchAndList.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(40)))), ((int)(((byte)(80)))));
            this.grpSearchAndList.Location = new System.Drawing.Point(12, 12);
            this.grpSearchAndList.Name = "grpSearchAndList";
            this.grpSearchAndList.Size = new System.Drawing.Size(626, 731);
            this.grpSearchAndList.TabIndex = 0;
            this.grpSearchAndList.TabStop = false;
            this.grpSearchAndList.Text = "📋 ค้นหาและประวัติใบงาน";
            // 
            // dgvRepairJobs
            // 
            this.dgvRepairJobs.AllowUserToAddRows = false;
            this.dgvRepairJobs.AllowUserToDeleteRows = false;
            this.dgvRepairJobs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvRepairJobs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRepairJobs.BackgroundColor = System.Drawing.Color.White;
            this.dgvRepairJobs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvRepairJobs.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            dgvHeaderStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvHeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            dgvHeaderStyle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            dgvHeaderStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dgvHeaderStyle.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dgvHeaderStyle.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dgvHeaderStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRepairJobs.ColumnHeadersDefaultCellStyle = dgvHeaderStyle;
            this.dgvRepairJobs.ColumnHeadersHeight = 44;
            this.dgvRepairJobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRowStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dgvRowStyle.BackColor = System.Drawing.SystemColors.Window;
            dgvRowStyle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dgvRowStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dgvRowStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(225)))), ((int)(((byte)(255)))));
            dgvRowStyle.SelectionForeColor = System.Drawing.Color.Black;
            dgvRowStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRepairJobs.DefaultCellStyle = dgvRowStyle;
            this.dgvRepairJobs.EnableHeadersVisualStyles = false;
            this.dgvRepairJobs.Location = new System.Drawing.Point(18, 100);
            this.dgvRepairJobs.MultiSelect = false;
            this.dgvRepairJobs.Name = "dgvRepairJobs";
            this.dgvRepairJobs.ReadOnly = true;
            this.dgvRepairJobs.RowHeadersVisible = false;
            this.dgvRepairJobs.RowTemplate.Height = 44;
            this.dgvRepairJobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRepairJobs.Size = new System.Drawing.Size(592, 648);
            this.dgvRepairJobs.TabIndex = 2;
            this.dgvRepairJobs.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRepairJobs_CellDoubleClick);
            this.dgvRepairJobs.SelectionChanged += new System.EventHandler(this.dgvRepairJobs_SelectionChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtSearch.Location = new System.Drawing.Point(18, 56);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(592, 36);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblSearch.Location = new System.Drawing.Point(18, 24);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(460, 30);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "🔍 ค้นหา (เลขที่ใบงาน, ชื่อลูกค้า หรือเบอร์โทร):";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.ClientSize = new System.Drawing.Size(1264, 855);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.MinimumSize = new System.Drawing.Size(1100, 750);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ระบบออกใบรับซ่อมสินค้า (Repair Ticket System)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpJobEntry.ResumeLayout(false);
            this.grpJobEntry.PerformLayout();
            this.grpSearchAndList.ResumeLayout(false);
            this.grpSearchAndList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRepairJobs)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
