using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;
using RepairTicket.Models;

namespace RepairTicket.Data
{
    /// <summary>
    /// จัดการคำสั่ง CRUD และค้นหาข้อมูลใบงานซ่อมในตาราง RepairJobs
    /// </summary>
    public class RepairJobRepository
    {
        private readonly string _connectionString;

        public RepairJobRepository(string? connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseInitializer.ConnectionString;
        }

        /// <summary>
        /// บันทึกใบงานซ่อมใหม่เข้าสู่ฐานข้อมูล
        /// </summary>
        public int Insert(RepairJob job)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                INSERT INTO RepairJobs (JobNo, JobDate, CustomerName, CustomerPhone, DeviceSymptom)
                VALUES ($jobNo, $jobDate, $customerName, $customerPhone, $deviceSymptom);
                SELECT last_insert_rowid();
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$jobNo", job.JobNo);
            cmd.Parameters.AddWithValue("$jobDate", job.JobDate);
            cmd.Parameters.AddWithValue("$customerName", job.CustomerName);
            cmd.Parameters.AddWithValue("$customerPhone", job.CustomerPhone ?? string.Empty);
            cmd.Parameters.AddWithValue("$deviceSymptom", job.DeviceSymptom);

            var result = cmd.ExecuteScalar();
            int newId = Convert.ToInt32(result);
            job.Id = newId;
            return newId;
        }

        /// <summary>
        /// อัปเดตข้อมูลใบงานซ่อมเดิม
        /// </summary>
        public void Update(RepairJob job)
        {
            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                UPDATE RepairJobs
                SET JobNo = $jobNo,
                    JobDate = $jobDate,
                    CustomerName = $customerName,
                    CustomerPhone = $customerPhone,
                    DeviceSymptom = $deviceSymptom
                WHERE Id = $id;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$id", job.Id);
            cmd.Parameters.AddWithValue("$jobNo", job.JobNo);
            cmd.Parameters.AddWithValue("$jobDate", job.JobDate);
            cmd.Parameters.AddWithValue("$customerName", job.CustomerName);
            cmd.Parameters.AddWithValue("$customerPhone", job.CustomerPhone ?? string.Empty);
            cmd.Parameters.AddWithValue("$deviceSymptom", job.DeviceSymptom);

            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ดึงรายการใบงานซ่อมทั้งหมด เรียงลำดับจากงานล่าสุดไปเก่าสุด
        /// </summary>
        public List<RepairJob> GetAll()
        {
            var list = new List<RepairJob>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT Id, JobNo, JobDate, CustomerName, CustomerPhone, DeviceSymptom
                FROM RepairJobs
                ORDER BY Id DESC;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(new RepairJob
                {
                    Id = reader.GetInt32(0),
                    JobNo = reader.GetString(1),
                    JobDate = reader.GetString(2),
                    CustomerName = reader.GetString(3),
                    CustomerPhone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    DeviceSymptom = reader.GetString(5)
                });
            }

            return list;
        }

        /// <summary>
        /// ค้นหาใบงานตามเลขที่ใบงาน, ชื่อลูกค้า หรือเบอร์โทรลูกค้า เรียงจากงานล่าสุดไปเก่าสุด
        /// </summary>
        public List<RepairJob> Search(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return GetAll();
            }

            var list = new List<RepairJob>();

            using var connection = new SqliteConnection(_connectionString);
            connection.Open();

            string sql = @"
                SELECT Id, JobNo, JobDate, CustomerName, CustomerPhone, DeviceSymptom
                FROM RepairJobs
                WHERE JobNo LIKE $q OR CustomerName LIKE $q OR CustomerPhone LIKE $q
                ORDER BY Id DESC;
            ";

            using var cmd = new SqliteCommand(sql, connection);
            cmd.Parameters.AddWithValue("$q", $"%{keyword.Trim()}%");

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add(new RepairJob
                {
                    Id = reader.GetInt32(0),
                    JobNo = reader.GetString(1),
                    JobDate = reader.GetString(2),
                    CustomerName = reader.GetString(3),
                    CustomerPhone = reader.IsDBNull(4) ? string.Empty : reader.GetString(4),
                    DeviceSymptom = reader.GetString(5)
                });
            }

            return list;
        }

        /// <summary>
        /// คำนวณและสร้างเลขที่ใบงานถัดไปในรูปแบบ "YYMMDD-hhmmss" เช่น 261119-100524
        /// พร้อมตรวจสอบความซ้ำซ้อนในฐานข้อมูล (Unique Guarantee)
        /// ใช้ InvariantCulture เพื่อให้ได้เลขปี 2 หลักมาตรฐาน (เช่น 26) เสมอกันทุกเครื่อง
        /// </summary>
        public string GenerateNextJobNo()
        {
            var now = DateTime.Now;
            string jobNo = now.ToString("yyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);

            try
            {
                using var connection = new SqliteConnection(_connectionString);
                connection.Open();

                // ตรวจสอบความซ้ำซ้อนในฐานข้อมูล (เผื่อกรณีสร้างในวินาทีเดียวกัน)
                string checkSql = "SELECT COUNT(*) FROM RepairJobs WHERE JobNo = $jobNo;";
                int counter = 0;
                while (true)
                {
                    using var cmd = new SqliteCommand(checkSql, connection);
                    cmd.Parameters.AddWithValue("$jobNo", jobNo);
                    long count = (long)(cmd.ExecuteScalar() ?? 0L);
                    if (count == 0)
                    {
                        break;
                    }
                    counter++;
                    jobNo = now.AddSeconds(counter).ToString("yyMMdd-HHmmss", System.Globalization.CultureInfo.InvariantCulture);
                }
            }
            catch (SqliteException)
            {
                // หากตารางยังไม่ได้ Initialize ให้คืนค่า jobNo ได้ทันทีอย่างปลอดภัย
            }

            return jobNo;
        }
    }
}
