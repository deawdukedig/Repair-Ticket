using System;
using System.Windows.Forms;
using RepairTicket.Data;
using RepairTicket.Models;

namespace RepairTicket.UI
{
    public partial class SettingsForm : Form
    {
        private readonly ShopSettingRepository _settingRepo;
        public ShopSetting CurrentSettings { get; private set; }

        public SettingsForm(ShopSettingRepository? settingRepo = null)
        {
            InitializeComponent();
            _settingRepo = settingRepo ?? new ShopSettingRepository();
            CurrentSettings = _settingRepo.GetSettings();
            LoadData();
        }

        private void LoadData()
        {
            txtShopName.Text = CurrentSettings.ShopName;
            txtShopAddress.Text = CurrentSettings.ShopAddress;
        }

        private void btnSave_Click(object? sender, EventArgs e)
        {
            string name = txtShopName.Text.Trim();
            string address = txtShopAddress.Text.Trim();

            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show(
                    "กรุณากรอกชื่อร้านค้า",
                    "ข้อมูลไม่ครบถ้วน",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtShopName.Focus();
                return;
            }

            try
            {
                CurrentSettings.ShopName = name;
                CurrentSettings.ShopAddress = address;

                _settingRepo.SaveSettings(CurrentSettings);

                MessageBox.Show(
                    "บันทึกข้อมูลร้านค้าเรียบร้อยแล้ว",
                    "สำเร็จ",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"เกิดข้อผิดพลาดในการบันทึกข้อมูล: {ex.Message}",
                    "ข้อผิดพลาด",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
        }

        private void btnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
