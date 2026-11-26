namespace RepairTicket.Models
{
    /// <summary>
    /// โมเดลข้อมูลคลังคำศัพท์ช่างซ่อมสำหรับระบบแนะนำอัตโนมัติ
    /// </summary>
    public class WordDictionaryItem
    {
        public int WordId { get; set; }
        public string WordText { get; set; } = string.Empty;
        public int UsageCount { get; set; } = 1;
    }
}
