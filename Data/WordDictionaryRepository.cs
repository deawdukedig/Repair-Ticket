using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RepairTicket.Models;

namespace RepairTicket.Data
{
    /// <summary>
    /// จัดการคลังคำศัพท์ช่างซ่อมสำหรับระบบแนะนำอัตโนมัติ (Gboard-style Word Suggestion)
    /// </summary>
    public class WordDictionaryRepository
    {
        private readonly string _connectionString;
        private static readonly string[] PriorityWords = new[] { "ล้างเครื่อง", "กระดาษติด", "สีไม่ครบ" };

        public WordDictionaryRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }

        /// <summary>
        /// ค้นหาคำศัพท์แนะนำแบบเรียลไทม์จากคำสุดท้ายที่กำลังพิมพ์
        /// จัดลำดับคำสำคัญ "ล้างเครื่อง", "กระดาษติด", "สีไม่ครบ" ไว้หน้าสุดเมื่อตรงกับการค้นหา
        /// </summary>
        public List<string> GetSuggestions(string token, int limit = 8)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return GetTopWords(limit);
            }

            var results = new List<string>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT WordText
                FROM WordDictionary
                WHERE WordText LIKE $pattern
                ORDER BY 
                    CASE 
                        WHEN WordText = 'ล้างเครื่อง' THEN 1
                        WHEN WordText = 'กระดาษติด' THEN 2
                        WHEN WordText = 'สีไม่ครบ' THEN 3
                        ELSE 4
                    END ASC,
                    UsageCount DESC, 
                    length(WordText) ASC
                LIMIT $limit;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$pattern", $"%{token.Trim()}%");
            cmd.Parameters.AddWithValue("$limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader.GetString(0));
            }

            // หากไม่พบผลลัพธ์ตรง ให้แสดงคำยอดนิยมสำรอง
            if (results.Count == 0)
            {
                return GetTopWords(limit);
            }

            return results;
        }

        /// <summary>
        /// ดึงคำศัพท์ที่ถูกใช้งานบ่อยที่สุด เพื่อเป็นคำแนะนำเริ่มต้น
        /// จัดลำดับคำสำคัญ "ล้างเครื่อง", "กระดาษติด", "สีไม่ครบ" ไว้หน้าสุดเสมอ
        /// </summary>
        public List<string> GetTopWords(int limit = 8)
        {
            var results = new List<string>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT WordText
                FROM WordDictionary
                ORDER BY 
                    CASE 
                        WHEN WordText = 'ล้างเครื่อง' THEN 1
                        WHEN WordText = 'กระดาษติด' THEN 2
                        WHEN WordText = 'สีไม่ครบ' THEN 3
                        ELSE 4
                    END ASC,
                    UsageCount DESC, 
                    length(WordText) ASC
                LIMIT $limit;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$limit", limit);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                results.Add(reader.GetString(0));
            }

            // รับประกันความถูกต้อง: หากยังไม่มีคำสำคัญในผลลัพธ์ ให้แทรกไว้หน้าสุดเสมอ
            for (int i = PriorityWords.Length - 1; i >= 0; i--)
            {
                string pWord = PriorityWords[i];
                if (!results.Contains(pWord))
                {
                    results.Insert(0, pWord);
                }
            }

            if (results.Count > limit)
            {
                results = results.GetRange(0, limit);
            }

            return results;
        }

        /// <summary>
        /// ระบบเรียนรู้คำศัพท์อัตโนมัติ: บันทึกคำศัพท์ใหม่หรือเพิ่มความถี่คำเดิม (UsageCount)
        /// </summary>
        public void LearnWords(IEnumerable<string> words)
        {
            if (words == null) return;

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();
            using var transaction = connection.BeginTransaction();

            string sql = @"
                INSERT INTO WordDictionary (WordText, UsageCount)
                VALUES ($word, 1)
                ON CONFLICT(WordText) DO UPDATE SET
                    UsageCount = WordDictionary.UsageCount + 1;
            ";

            foreach (var rawWord in words)
            {
                string word = rawWord.Trim();
                // กรองเฉพาะคำที่มีความยาว 2 ตัวอักษรขึ้นไป และไม่ใช่ตัวเลขล้วน
                if (word.Length >= 2 && !long.TryParse(word, out _))
                {
                    using var cmd = new SqliteCommand(sql, connection, transaction);
                    cmd.Parameters.AddWithValue("$word", word);
                    cmd.ExecuteNonQuery();
                }
            }

            transaction.Commit();
        }
    }
}
