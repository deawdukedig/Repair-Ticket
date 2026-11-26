using System;
using System.IO;
using Microsoft.Data.Sqlite;

namespace RepairTicket.Data
{
    /// <summary>
    /// จัดการการสร้างและเตรียมความพร้อมของฐานข้อมูล SQLite (repair_shop.db)
    /// </summary>
    public static class DatabaseInitializer
    {
        private static readonly string DbFileName = "repair_shop.db";
        public static string ConnectionString => $"Data Source={Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DbFileName)}";

        /// <summary>
        /// ตรวจสอบและสร้างตารางฐานข้อมูลอัตโนมัติเมื่อเปิดโปรแกรม
        /// </summary>
        public static void Initialize()
        {
            using var connection = new SqliteConnection(ConnectionString);
            connection.Open();

            // 1. ตาราง RepairJobs
            string createJobsTable = @"
                CREATE TABLE IF NOT EXISTS RepairJobs (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    JobNo TEXT NOT NULL UNIQUE,
                    JobDate TEXT NOT NULL,
                    CustomerName TEXT NOT NULL,
                    CustomerPhone TEXT NOT NULL DEFAULT '',
                    DeviceSymptom TEXT NOT NULL
                );
                CREATE INDEX IF NOT EXISTS idx_repairjobs_search ON RepairJobs (JobNo, CustomerName, CustomerPhone);
            ";
            using (var cmd = new SqliteCommand(createJobsTable, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // ตรวจสอบและอัปเกรด Schema คอลัมน์ CustomerPhone กรณีมีฐานข้อมูลเดิมอยู่แล้ว
            bool hasCustomerPhone = false;
            using (var checkColCmd = new SqliteCommand("PRAGMA table_info(RepairJobs);", connection))
            using (var colReader = checkColCmd.ExecuteReader())
            {
                while (colReader.Read())
                {
                    string colName = colReader.GetString(1);
                    if (string.Equals(colName, "CustomerPhone", StringComparison.OrdinalIgnoreCase))
                    {
                        hasCustomerPhone = true;
                        break;
                    }
                }
            }

            if (!hasCustomerPhone)
            {
                using var alterCmd = new SqliteCommand("ALTER TABLE RepairJobs ADD COLUMN CustomerPhone TEXT NOT NULL DEFAULT '';", connection);
                alterCmd.ExecuteNonQuery();
            }

            // 2. ตาราง ShopSettings
            string createSettingsTable = @"
                CREATE TABLE IF NOT EXISTS ShopSettings (
                    Id INTEGER PRIMARY KEY CHECK (Id = 1),
                    ShopName TEXT NOT NULL,
                    ShopAddress TEXT NOT NULL,
                    DefaultA4Printer TEXT NOT NULL DEFAULT '',
                    DefaultPosPrinter TEXT NOT NULL DEFAULT '',
                    LastPrintFormat INTEGER NOT NULL DEFAULT 0
                );
            ";
            using (var cmd = new SqliteCommand(createSettingsTable, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // ตรวจสอบและอัปเกรด Schema คอลัมน์เครื่องพิมพ์ของ ShopSettings กรณีมีฐานข้อมูลเดิม
            var existingSettingCols = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);
            using (var checkColCmd = new SqliteCommand("PRAGMA table_info(ShopSettings);", connection))
            using (var colReader = checkColCmd.ExecuteReader())
            {
                while (colReader.Read())
                {
                    existingSettingCols.Add(colReader.GetString(1));
                }
            }

            if (!existingSettingCols.Contains("DefaultA4Printer"))
            {
                using var alterCmd = new SqliteCommand("ALTER TABLE ShopSettings ADD COLUMN DefaultA4Printer TEXT NOT NULL DEFAULT '';", connection);
                alterCmd.ExecuteNonQuery();
            }
            if (!existingSettingCols.Contains("DefaultPosPrinter"))
            {
                using var alterCmd = new SqliteCommand("ALTER TABLE ShopSettings ADD COLUMN DefaultPosPrinter TEXT NOT NULL DEFAULT '';", connection);
                alterCmd.ExecuteNonQuery();
            }
            if (!existingSettingCols.Contains("LastPrintFormat"))
            {
                using var alterCmd = new SqliteCommand("ALTER TABLE ShopSettings ADD COLUMN LastPrintFormat INTEGER NOT NULL DEFAULT 0;", connection);
                alterCmd.ExecuteNonQuery();
            }

            // แทรกค่าเริ่มต้นให้ ShopSettings หากยังไม่มีข้อมูล
            string checkSettings = "SELECT COUNT(*) FROM ShopSettings WHERE Id = 1;";
            using (var cmd = new SqliteCommand(checkSettings, connection))
            {
                long count = (long)(cmd.ExecuteScalar() ?? 0L);
                if (count == 0)
                {
                    string insertSetting = @"
                        INSERT INTO ShopSettings (Id, ShopName, ShopAddress)
                        VALUES (1, 'ร้านซ่อมอุปกรณ์ด่วน (Smart Repair Shop)', '123/45 ถ.สุขุมวิท แขวงคลองเตย เขตคลองเตย กรุงเทพฯ 10110 โทร. 081-234-5678');
                    ";
                    using var insertCmd = new SqliteCommand(insertSetting, connection);
                    insertCmd.ExecuteNonQuery();
                }
            }

            // 3. ตาราง WordDictionary
            string createDictionaryTable = @"
                CREATE TABLE IF NOT EXISTS WordDictionary (
                    WordId INTEGER PRIMARY KEY AUTOINCREMENT,
                    WordText TEXT NOT NULL UNIQUE,
                    UsageCount INTEGER NOT NULL DEFAULT 1
                );
                CREATE INDEX IF NOT EXISTS idx_worddict_lookup ON WordDictionary (WordText, UsageCount DESC);
            ";
            using (var cmd = new SqliteCommand(createDictionaryTable, connection))
            {
                cmd.ExecuteNonQuery();
            }

            // แทรกคำศัพท์ช่างซ่อมตั้งต้น หากคลังคำศัพท์ยังว่างเปล่า
            string checkWords = "SELECT COUNT(*) FROM WordDictionary;";
            using (var cmd = new SqliteCommand(checkWords, connection))
            {
                long count = (long)(cmd.ExecuteScalar() ?? 0L);
                if (count == 0)
                {
                    string[] seedWords = new[]
                    {
                        "ล้างเครื่อง", "กระดาษติด", "สีไม่ครบ",
                        "จอแตก", "เปิดไม่ติด", "ชาร์จไม่เข้า", "แบตเสื่อม", "ลงวินโดวส์ใหม่",
                        "เปลี่ยนอะไหล่", "โดนน้ำ", "จอฟ้า", "เครื่องร้อน",
                        "ติดไวรัส", "เปลี่ยนหน้าจอ", "เปลี่ยนแบตเตอรี่", "เพิ่ม RAM", "อัปเกรด SSD",
                        "พัดลมไม่หมุน", "ลำโพงแตก", "คีย์บอร์ดเสีย", "ทัชสกรีนไม่ติด", "สวิตช์เสีย",
                        "บอร์ดช็อต", "ไฟไม่เข้า", "เครื่องค้าง", "กล้องไม่เปิด", "ไมค์ไม่ดัง"
                    };

                    using var transaction = connection.BeginTransaction();
                    string insertWordSql = @"
                        INSERT OR IGNORE INTO WordDictionary (WordText, UsageCount)
                        VALUES ($word, 5);
                    ";
                    foreach (var word in seedWords)
                    {
                        using var insertCmd = new SqliteCommand(insertWordSql, connection, transaction);
                        insertCmd.Parameters.AddWithValue("$word", word);
                        insertCmd.ExecuteNonQuery();
                    }
                    transaction.Commit();
                }
            }

            // ตรวจสอบและแทรกคำศัพท์สำคัญลำดับแรก (ล้างเครื่อง, กระดาษติด, สีไม่ครบ) ให้มีในฐานข้อมูลเสมอ แม้เป็นฐานข้อมูลเดิม
            string[] priorityWords = new[] { "ล้างเครื่อง", "กระดาษติด", "สีไม่ครบ" };
            using (var transaction = connection.BeginTransaction())
            {
                string insertPrioritySql = @"
                    INSERT OR IGNORE INTO WordDictionary (WordText, UsageCount)
                    VALUES ($word, 10);
                ";
                foreach (var pWord in priorityWords)
                {
                    using var pCmd = new SqliteCommand(insertPrioritySql, connection, transaction);
                    pCmd.Parameters.AddWithValue("$word", pWord);
                    pCmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
        }
    }
}
