using System;
using System.Windows.Forms;
using RepairTicket.Data;
using RepairTicket.UI;

namespace RepairTicket
{
    internal static class Program
    {
        /// <summary>
        /// จุดเริ่มต้นหลักของแอปพลิเคชัน (Application Entry Point)
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 1. ตั้งค่าการรองรับ High DPI และ Visual Styles สำหรับ .NET 8.0 Windows Forms
            ApplicationConfiguration.Initialize();

            try
            {
                // 2. ตรวจสอบและสร้างฐานข้อมูล SQLite repair_shop.db พร้อมตารางอัตโนมัติ
                DatabaseInitializer.Initialize();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"ไม่สามารถเริ่มต้นฐานข้อมูล SQLite ได้:\n{ex.Message}",
                    "ข้อผิดพลาดฐานข้อมูล",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                return;
            }

            // 3. เริ่มการทำงานของหน้าจอหลัก Form1
            Application.Run(new Form1());
        }
    }
}
