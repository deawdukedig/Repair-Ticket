using System;
using Microsoft.Data.Sqlite;
using RepairTicket.Models;

namespace RepairTicket.Data
{
    /// <summary>
    /// จัดการการอ่านและบันทึกข้อมูลร้านค้าในตาราง ShopSettings
    /// </summary>
    public class ShopSettingRepository
    {
        private readonly string _connectionString;

        public ShopSettingRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }

        /// <summary>
        /// ดึงข้อมูลร้านค้าปัจจุบัน (แถว Id = 1)
        /// </summary>
        public ShopSetting GetSettings()
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = "SELECT Id, ShopName, ShopAddress, DefaultA4Printer, DefaultPosPrinter, LastPrintFormat FROM ShopSettings WHERE Id = 1 LIMIT 1;";
            using var cmd = new SqliteCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                return new ShopSetting
                {
                    Id = reader.GetInt32(0),
                    ShopName = reader.GetString(1),
                    ShopAddress = reader.GetString(2),
                    DefaultA4Printer = reader.IsDBNull(3) ? string.Empty : reader.GetString(3),
                    DefaultPosPrinter = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    LastPrintFormat = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                };
            }

            // ค่าเริ่มต้นสำรองหากยังไม่มีข้อมูล
            return new ShopSetting
            {
                Id = 1,
                ShopName = "ร้านซ่อมอุปกรณ์ด่วน (Smart Repair Shop)",
                ShopAddress = "123/45 ถ.สุขุมวิท แขวงคลองเตย เขตคลองเตย กรุงเทพฯ 10110 โทร. 081-234-5678",
                DefaultA4Printer = string.Empty,
                DefaultPosPrinter = string.Empty,
                LastPrintFormat = 0
            };
        }

        /// <summary>
        /// บันทึกหรืออัปเดตข้อมูลร้านค้า
        /// </summary>
        public void SaveSettings(ShopSetting setting)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                INSERT INTO ShopSettings (Id, ShopName, ShopAddress, DefaultA4Printer, DefaultPosPrinter, LastPrintFormat)
                VALUES (1, $name, $address, $a4, $pos, $format)
                ON CONFLICT(Id) DO UPDATE SET
                    ShopName = excluded.ShopName,
                    ShopAddress = excluded.ShopAddress,
                    DefaultA4Printer = excluded.DefaultA4Printer,
                    DefaultPosPrinter = excluded.DefaultPosPrinter,
                    LastPrintFormat = excluded.LastPrintFormat;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$name", setting.ShopName.Trim());
            cmd.Parameters.AddWithValue("$address", setting.ShopAddress.Trim());
            cmd.Parameters.AddWithValue("$a4", setting.DefaultA4Printer ?? string.Empty);
            cmd.Parameters.AddWithValue("$pos", setting.DefaultPosPrinter ?? string.Empty);
            cmd.Parameters.AddWithValue("$format", setting.LastPrintFormat);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// บันทึกเฉพาะการตั้งค่าเครื่องพิมพ์ที่เลือก
        /// </summary>
        public void SavePrinterPreferences(string a4Printer, string posPrinter, int lastFormat)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                UPDATE ShopSettings
                SET DefaultA4Printer = $a4,
                    DefaultPosPrinter = $pos,
                    LastPrintFormat = $format
                WHERE Id = 1;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$a4", a4Printer ?? string.Empty);
            cmd.Parameters.AddWithValue("$pos", posPrinter ?? string.Empty);
            cmd.Parameters.AddWithValue("$format", lastFormat);
            cmd.ExecuteNonQuery();
        }
    }
}
