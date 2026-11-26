using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;
using RepairTicket.Data;
using RepairTicket.Models;
using RepairTicket.Services;

namespace RepairTicket.UI
{
    public partial class Form1 : Form
    {
        private readonly RepairJobRepository _jobRepo;
        private readonly ShopSettingRepository _settingRepo;
        private readonly WordDictionaryRepository _wordRepo;

        private ShopSetting _currentSettings;
        private RepairJob? _selectedJob;
        private bool _isUpdatingSuggestion = false;
        private bool _isLoadingPrinterSettings = false;

        public Form1()
        {
            InitializeComponent();

            _jobRepo = new RepairJobRepository();
            _settingRepo = new ShopSettingRepository();
            _wordRepo = new WordDictionaryRepository();
            _currentSettings = _settingRepo.GetSettings();

            // ตรวจสอบรายชื่อเครื่องพิมพ์ใหม่แบบเรียลไทม์เมื่อคลิกเปิด Dropdown (กรณีเพิ่งเสียบสาย USB)
            cboTargetPrinter.DropDown += (s, e) => LoadInstalledPrinters();
        }

        private void Form1_Load(object? sender, EventArgs e)
        {
            _isLoadingPrinterSettings = true;
            try
            {
                // โหลดข้อมูลร้านค้าและการตั้งค่าเครื่องพิมพ์ที่จำไว้จากฐานข้อมูล SQLite
                _currentSettings = _settingRepo.GetSettings();

                // โหลดรายชื่อเครื่องพิมพ์ที่ติดตั้งในระบบ
                LoadInstalledPrinters();

                // โหลดรูปแบบการพิมพ์ที่เคยเลือกไว้ล่าสุด (0 = A4, 1 = POS 58mm)
                int lastFormat = Math.Clamp(_currentSettings.LastPrintFormat, 0, 1);
                cboPrintFormat.SelectedIndex = lastFormat;

                // เลือกเครื่องพิมพ์ที่บันทึกไว้สำหรับรูปแบบการพิมพ์นี้
                ApplyRememberedPrinterForCurrentFormat();

                // โหลดตารางงานซ่อม
                LoadJobs();

                // เตรียมฟอร์มสำหรับออกงานใหม่
                PrepareNewJob();

                // แสดงคำแนะนำคำศัพท์เริ่มต้นในแถบ Gboard
                UpdateWordSuggestions("");
            }
            finally
            {
                _isLoadingPrinterSettings = false;
            }
        }

        /// <summary>
        /// ตรวจสอบและโหลดรายชื่อเครื่องพิมพ์ทั้งหมดที่ติดตั้งใน Windows แบบ Dynamic
        /// พร้อมคงค่าที่กำลังเลือกอยู่ไว้หากเครื่องพิมพ์นั้นยังคงออนไลน์ในระบบ
        /// </summary>
        private void LoadInstalledPrinters()
        {
            string currentSelection = cboTargetPrinter.SelectedItem?.ToString() ?? "";

            cboTargetPrinter.Items.Clear();
            cboTargetPrinter.Items.Add("🖨️ (เครื่องพิมพ์เริ่มต้น / Default Printer)");

            try
            {
                foreach (string printer in PrinterSettings.InstalledPrinters)
                {
                    cboTargetPrinter.Items.Add(printer);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error querying installed printers: {ex.Message}");
            }

            // คืนค่าที่กำลังเลือกไว้เดิม หากยังมีอยู่ในระบบ
            if (!string.IsNullOrEmpty(currentSelection))
            {
                for (int i = 0; i < cboTargetPrinter.Items.Count; i++)
                {
                    if (string.Equals(cboTargetPrinter.Items[i]?.ToString(), currentSelection, StringComparison.OrdinalIgnoreCase))
                    {
                        cboTargetPrinter.SelectedIndex = i;
                        return;
                    }
                }
            }

            if (cboTargetPrinter.Items.Count > 0)
            {
                cboTargetPrinter.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// เลือกเครื่องพิมพ์ใน ComboBox ตามค่าที่จำไว้ในฐานข้อมูลสำหรับรูปแบบการพิมพ์ปัจจุบัน
        /// </summary>
        private void ApplyRememberedPrinterForCurrentFormat()
        {
            bool is58mm = (cboPrintFormat.SelectedIndex == 1);
            string savedPrinter = is58mm ? _currentSettings.DefaultPosPrinter : _currentSettings.DefaultA4Printer;

            if (!string.IsNullOrWhiteSpace(savedPrinter))
            {
                // ค้นหาชื่อเครื่องพิมพ์ที่จำไว้ในรายชื่อเครื่องพิมพ์ที่ติดตั้ง
                for (int i = 0; i < cboTargetPrinter.Items.Count; i++)
                {
                    if (string.Equals(cboTargetPrinter.Items[i]?.ToString(), savedPrinter, StringComparison.OrdinalIgnoreCase))
                    {
                        cboTargetPrinter.SelectedIndex = i;
                        return;
                    }
                }
            }

            // หากเป็นโหมดสลิป 58mm และยังไม่มีเครื่องพิมพ์ที่บันทึกไว้ ให้ช่วยตรวจหาเครื่องพิมพ์ POS อัตโนมัติ
            if (is58mm)
            {
                for (int i = 1; i < cboTargetPrinter.Items.Count; i++)
                {
                    string pName = cboTargetPrinter.Items[i]?.ToString()?.ToLower() ?? "";
                    if (pName.Contains("58") || pName.Contains("pos") || pName.Contains("thermal") || pName.Contains("receipt"))
                    {
                        cboTargetPrinter.SelectedIndex = i;
                        return;
                    }
                }
            }

            // หากไม่มี ให้เลือกเครื่องพิมพ์เริ่มต้น (Index 0)
            if (cboTargetPrinter.Items.Count > 0)
            {
                cboTargetPrinter.SelectedIndex = 0;
            }
        }

        private void cboPrintFormat_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isLoadingPrinterSettings) return;

            _isLoadingPrinterSettings = true;
            try
            {
                // สลับไปเลือกเครื่องพิมพ์ที่จำไว้สำหรับรูปแบบที่เลือก (A4 หรือ POS 58mm)
                ApplyRememberedPrinterForCurrentFormat();

                // บันทึกรูปแบบการพิมพ์ล่าสุดลงฐานข้อมูล
                _currentSettings.LastPrintFormat = cboPrintFormat.SelectedIndex;
                _settingRepo.SavePrinterPreferences(
                    _currentSettings.DefaultA4Printer,
                    _currentSettings.DefaultPosPrinter,
                    _currentSettings.LastPrintFormat
                );
            }
            finally
            {
                _isLoadingPrinterSettings = false;
            }
        }

        private void cboTargetPrinter_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (_isLoadingPrinterSettings) return;

            string selectedPrinterName = "";
            if (cboTargetPrinter.SelectedIndex > 0 && cboTargetPrinter.SelectedItem != null)
            {
                selectedPrinterName = cboTargetPrinter.SelectedItem.ToString() ?? "";
            }

            bool is58mm = (cboPrintFormat.SelectedIndex == 1);
            if (is58mm)
            {
                _currentSettings.DefaultPosPrinter = selectedPrinterName;
            }
            else
            {
                _currentSettings.DefaultA4Printer = selectedPrinterName;
            }

            _currentSettings.LastPrintFormat = cboPrintFormat.SelectedIndex;

            // บันทึกค่าเครื่องพิมพ์ที่เลือกถาวรลงฐานข้อมูล SQLite repair_shop.db ทันที
            _settingRepo.SavePrinterPreferences(
                _currentSettings.DefaultA4Printer,
                _currentSettings.DefaultPosPrinter,
                _currentSettings.LastPrintFormat
            );
        }

        /// <summary>
        /// เตรียมหน้าจอสำหรับสร้างงานใหม่ ล้างฟอร์มและสร้างเลขที่ใบงานถัดไป
        /// </summary>
        private void PrepareNewJob()
        {
            _selectedJob = null;
            txtJobNo.Text = _jobRepo.GenerateNextJobNo();
            txtJobDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            txtCustomerName.Clear();
            txtCustomerPhone.Clear();
            txtDeviceSymptom.Clear();
            btnPrintAgain.Enabled = false;

            btnSaveAndPrint.Text = "💾 บันทึกและพิมพ์";
            btnSaveAndPrint.BackColor = Color.FromArgb(40, 167, 69); // สีเขียวสำหรับงานใหม่

            txtCustomerName.Focus();
            UpdateWordSuggestions("");
        }

        /// <summary>
        /// โหลดรายการใบงานทั้งหมดขึ้น DataGridView
        /// </summary>
        private void LoadJobs(string keyword = "")
        {
            try
            {
                var jobs = string.IsNullOrWhiteSpace(keyword) 
                    ? _jobRepo.GetAll() 
                    : _jobRepo.Search(keyword);

                dgvRepairJobs.DataSource = null;
                dgvRepairJobs.DataSource = jobs;

                // จัดแต่งคอลัมน์ให้อ่านง่าย ชัดเจน
                if (dgvRepairJobs.Columns["Id"] != null)
                {
                    dgvRepairJobs.Columns["Id"]!.Visible = false;
                }
                if (dgvRepairJobs.Columns["JobNo"] != null)
                {
                    dgvRepairJobs.Columns["JobNo"]!.HeaderText = "เลขที่ใบงาน";
                    dgvRepairJobs.Columns["JobNo"]!.Width = 190;
                }
                if (dgvRepairJobs.Columns["JobDate"] != null)
                {
                    dgvRepairJobs.Columns["JobDate"]!.HeaderText = "วันที่รับซ่อม";
                    dgvRepairJobs.Columns["JobDate"]!.Width = 180;
                }
                if (dgvRepairJobs.Columns["CustomerName"] != null)
                {
                    dgvRepairJobs.Columns["CustomerName"]!.HeaderText = "ชื่อลูกค้า";
                    dgvRepairJobs.Columns["CustomerName"]!.Width = 180;
                }
                if (dgvRepairJobs.Columns["CustomerPhone"] != null)
                {
                    dgvRepairJobs.Columns["CustomerPhone"]!.HeaderText = "เบอร์โทรลูกค้า";
                    dgvRepairJobs.Columns["CustomerPhone"]!.Width = 160;
                }
                if (dgvRepairJobs.Columns["DeviceSymptom"] != null)
                {
                    dgvRepairJobs.Columns["DeviceSymptom"]!.HeaderText = "อุปกรณ์และอาการเสีย";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการโหลดข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Gboard-Style Word Suggestion System

        private void txtDeviceSymptom_TextChanged(object? sender, EventArgs e)
        {
            if (_isUpdatingSuggestion) return;
            RefreshCurrentTokenSuggestions();
        }

        private void txtDeviceSymptom_KeyUp(object? sender, KeyEventArgs e)
        {
            if (_isUpdatingSuggestion) return;
            // เมื่อเลื่อนเคอร์เซอร์หรือกดแป้นพิมพ์ ให้อัปเดตคำแนะนำตามคำปัจจุบัน
            RefreshCurrentTokenSuggestions();
        }

        private void RefreshCurrentTokenSuggestions()
        {
            string currentToken = ExtractCurrentToken();
            UpdateWordSuggestions(currentToken);
        }

        /// <summary>
        /// ดึงคำสุดท้ายที่กำลังพิมพ์ (Current Token) ก่อนตำแหน่งเคอร์เซอร์
        /// </summary>
        private string ExtractCurrentToken()
        {
            string text = txtDeviceSymptom.Text;
            int caretIndex = txtDeviceSymptom.SelectionStart;

            if (caretIndex <= 0 || string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            int safeLength = Math.Min(caretIndex, text.Length);
            string sub = text.Substring(0, safeLength);

            char[] delimiters = new[] { ' ', '\r', '\n', ',', '+', '/', '(', ')', '-', ':', ';' };
            int lastDelim = sub.LastIndexOfAny(delimiters);

            if (lastDelim < 0)
            {
                return sub.Trim();
            }

            return sub.Substring(lastDelim + 1).Trim();
        }

        /// <summary>
        /// อัปเดตปุ่มคำแนะนำใน FlowLayoutPanel
        /// </summary>
        private void UpdateWordSuggestions(string token)
        {
            pnlWordSuggestions.SuspendLayout();
            pnlWordSuggestions.Controls.Clear();

            List<string> suggestions = _wordRepo.GetSuggestions(token, 8);

            foreach (string word in suggestions)
            {
                Button btnPill = new Button
                {
                    Text = $"💡 {word}",
                    AutoSize = true,
                    Height = 36,
                    Font = new Font("Segoe UI", 13.5F, FontStyle.Regular),
                    BackColor = Color.White,
                    ForeColor = Color.FromArgb(0, 102, 204),
                    Cursor = Cursors.Hand,
                    FlatStyle = FlatStyle.Flat,
                    Margin = new Padding(3, 2, 3, 2),
                    Tag = word
                };

                btnPill.FlatAppearance.BorderColor = Color.FromArgb(180, 205, 235);
                btnPill.FlatAppearance.BorderSize = 1;
                btnPill.FlatAppearance.MouseOverBackColor = Color.FromArgb(230, 242, 255);

                btnPill.Click += (s, ev) =>
                {
                    if (s is Button b && b.Tag is string w)
                    {
                        ApplySuggestion(w);
                    }
                };

                pnlWordSuggestions.Controls.Add(btnPill);
            }

            pnlWordSuggestions.ResumeLayout(true);
        }

        /// <summary>
        /// นำคำแนะนำที่คลิกไปแทนที่คำสุดท้ายที่กำลังพิมพ์ พร้อมเว้นวรรคอัตโนมัติ
        /// </summary>
        private void ApplySuggestion(string word)
        {
            _isUpdatingSuggestion = true;
            try
            {
                string text = txtDeviceSymptom.Text;
                int caretIndex = txtDeviceSymptom.SelectionStart;
                int safeLength = Math.Min(caretIndex, text.Length);
                string beforeCaret = text.Substring(0, safeLength);
                string afterCaret = text.Substring(safeLength);

                char[] delimiters = new[] { ' ', '\r', '\n', ',', '+', '/', '(', ')', '-', ':', ';' };
                int lastDelim = beforeCaret.LastIndexOfAny(delimiters);

                string prefix = lastDelim >= 0 ? beforeCaret.Substring(0, lastDelim + 1) : "";

                // แทนที่คำเดิมด้วยคำที่เลือก พร้อมเติมเว้นวรรคท้ายคำ " "
                string replacement = word + " ";
                txtDeviceSymptom.Text = prefix + replacement + afterCaret;

                // เลื่อนเคอร์เซอร์ไปต่อท้ายคำที่เพิ่งใส่
                int newCaret = prefix.Length + replacement.Length;
                txtDeviceSymptom.SelectionStart = Math.Min(newCaret, txtDeviceSymptom.Text.Length);
                txtDeviceSymptom.SelectionLength = 0;
                txtDeviceSymptom.Focus();
            }
            finally
            {
                _isUpdatingSuggestion = false;
            }

            // รีเฟรชคำแนะนำคำถัดไปทันที
            RefreshCurrentTokenSuggestions();
        }

        #endregion

        #region Actions & Event Handlers

        private void btnShopSettings_Click(object? sender, EventArgs e)
        {
            using var dlg = new SettingsForm(_settingRepo);
            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                _currentSettings = dlg.CurrentSettings;
            }
        }

        private void btnClear_Click(object? sender, EventArgs e)
        {
            PrepareNewJob();
        }

        private void btnSaveAndPrint_Click(object? sender, EventArgs e)
        {
            string customerName = txtCustomerName.Text.Trim();
            string customerPhone = txtCustomerPhone.Text.Trim();
            string symptom = txtDeviceSymptom.Text.Trim();

            if (string.IsNullOrWhiteSpace(customerName))
            {
                MessageBox.Show("กรุณากรอกชื่อลูกค้า", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtCustomerName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(symptom))
            {
                MessageBox.Show("กรุณากรอกอุปกรณ์และอาการเสีย", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDeviceSymptom.Focus();
                return;
            }

            try
            {
                bool isNew = (_selectedJob == null);

                if (isNew)
                {
                    string jobNo = string.IsNullOrWhiteSpace(txtJobNo.Text) ? _jobRepo.GenerateNextJobNo() : txtJobNo.Text.Trim();
                    txtJobNo.Text = jobNo;

                    _selectedJob = new RepairJob
                    {
                        JobNo = jobNo,
                        JobDate = txtJobDate.Text.Trim(),
                        CustomerName = customerName,
                        CustomerPhone = customerPhone,
                        DeviceSymptom = symptom
                    };
                    _jobRepo.Insert(_selectedJob);
                }
                else
                {
                    _selectedJob!.CustomerName = customerName;
                    _selectedJob.CustomerPhone = customerPhone;
                    _selectedJob.DeviceSymptom = symptom;
                    _jobRepo.Update(_selectedJob);
                }

                // ระบบเรียนรู้คำศัพท์อัตโนมัติ: ตัดคำในช่องอาการเสียแล้วบันทึกเข้า WordDictionary
                char[] splitChars = new[] { ' ', ',', '\n', '\r', '/', '+', '(', ')', '-' };
                string[] words = symptom.Split(splitChars, StringSplitOptions.RemoveEmptyEntries);
                _wordRepo.LearnWords(words);

                // สั่งพิมพ์ใบรับซ่อมตามรูปแบบที่เลือก
                ExecutePrintWorkflow(_selectedJob);

                // รีโหลดรายการใน DataGridView
                LoadJobs(txtSearch.Text);

                // เปลี่ยนสถานะเป็นโหมดแก้ไข และเปิดปุ่มพิมพ์ซ้ำ
                btnSaveAndPrint.Text = "💾 บันทึกข้อมูลแก้ไข";
                btnSaveAndPrint.BackColor = Color.FromArgb(0, 102, 204);
                btnPrintAgain.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {ex.Message}", "ข้อผิดพลาด", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrintAgain_Click(object? sender, EventArgs e)
        {
            if (_selectedJob == null)
            {
                MessageBox.Show("กรุณาเลือกใบงานที่ต้องการพิมพ์จากตาราง", "แจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ExecutePrintWorkflow(_selectedJob);
        }

        /// <summary>
        /// สั่งเปิดหน้าต่างตัวอย่างการพิมพ์ (Print Preview) ตามรูปแบบกระดาษและเครื่องพิมพ์ปลายทางที่เลือก
        /// </summary>
        private void ExecutePrintWorkflow(RepairJob job)
        {
            try
            {
                bool is58mm = (cboPrintFormat.SelectedIndex == 1);

                if (is58mm)
                {
                    // พิมพ์สลิปความร้อน POS 58mm ด้วยระบบ Bitmap (ภาษาไทยคมชัด 100%)
                    using var doc = new PosSlipPrintDocument(job, _currentSettings);
                    ApplyTargetPrinter(doc);

                    using var preview = new PrintPreviewDialog
                    {
                        Document = doc,
                        Text = $"ตัวอย่างการพิมพ์สลิป 58mm - {job.JobNo}",
                        StartPosition = FormStartPosition.CenterParent,
                        Width = 540,
                        Height = 780
                    };
                    preview.ShowDialog(this);
                }
                else
                {
                    // พิมพ์กระดาษ A4 แนวตั้ง แบ่งครึ่งบนและล่าง (Y = 584)
                    using var doc = new TicketPrintDocument(job, _currentSettings);
                    ApplyTargetPrinter(doc);

                    using var preview = new PrintPreviewDialog
                    {
                        Document = doc,
                        Text = $"ตัวอย่างการพิมพ์ใบรับซ่อม A4 - {job.JobNo}",
                        StartPosition = FormStartPosition.CenterParent,
                        Width = 920,
                        Height = 800
                    };
                    preview.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"เกิดข้อผิดพลาดในระบบการพิมพ์: {ex.Message}", "ข้อผิดพลาดการพิมพ์", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// กำหนดเครื่องพิมพ์ปลายทางให้เอกสารตามที่ผู้ใช้เลือกใน ComboBox
        /// พร้อมระบบ Fallback อัตโนมัติกรณีเครื่องพิมพ์ที่เลือกถูกลบ เปลี่ยนชื่อ หรือไม่พร้อมใช้งาน
        /// </summary>
        private void ApplyTargetPrinter(PrintDocument doc)
        {
            if (cboTargetPrinter.SelectedIndex > 0 && cboTargetPrinter.SelectedItem != null)
            {
                string chosenPrinter = cboTargetPrinter.SelectedItem.ToString() ?? "";
                if (!string.IsNullOrWhiteSpace(chosenPrinter))
                {
                    doc.PrinterSettings.PrinterName = chosenPrinter;

                    // Fallback ตรวจสอบความถูกต้องของเครื่องพิมพ์ (Validity Check)
                    if (!doc.PrinterSettings.IsValid)
                    {
                        // หากเครื่องพิมพ์ที่เลือกไว้ถูกลบออกจาก Windows หรือชื่อไม่ถูกต้อง
                        // ให้ Fallback กลับไปใช้ Default Printer ของระบบทันทีเพื่อป้องกันโปรแกรมแครช
                        doc.PrinterSettings = new PrinterSettings();
                        if (cboTargetPrinter.Items.Count > 0)
                        {
                            cboTargetPrinter.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void txtSearch_TextChanged(object? sender, EventArgs e)
        {
            LoadJobs(txtSearch.Text);
        }

        private void dgvRepairJobs_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                PopulateSelectedJobFromGrid();
            }
        }

        private void dgvRepairJobs_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvRepairJobs.SelectedRows.Count > 0 && dgvRepairJobs.CurrentRow != null && dgvRepairJobs.CurrentRow.Index >= 0)
            {
                PopulateSelectedJobFromGrid();
            }
        }

        /// <summary>
        /// นำข้อมูลแถวที่เลือกใน DataGridView ขึ้นมาแสดงในช่องกรอกข้อมูล
        /// </summary>
        private void PopulateSelectedJobFromGrid()
        {
            if (dgvRepairJobs.CurrentRow?.DataBoundItem is RepairJob job)
            {
                _selectedJob = job;
                txtJobNo.Text = job.JobNo;
                txtJobDate.Text = job.JobDate;
                txtCustomerName.Text = job.CustomerName;
                txtCustomerPhone.Text = job.CustomerPhone;
                txtDeviceSymptom.Text = job.DeviceSymptom;

                btnSaveAndPrint.Text = "💾 บันทึกข้อมูลแก้ไข";
                btnSaveAndPrint.BackColor = Color.FromArgb(0, 102, 204);
                btnPrintAgain.Enabled = true;

                RefreshCurrentTokenSuggestions();
            }
        }

        #endregion
    }
}
