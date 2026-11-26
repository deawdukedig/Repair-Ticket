namespace RepairTicket.Models
{
    /// <summary>
    /// โมเดลข้อมูลใบงานซ่อมสินค้า
    /// </summary>
    public class RepairJob
    {
        public int Id { get; set; }
        public string JobNo { get; set; } = string.Empty;
        public string JobDate { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerPhone { get; set; } = string.Empty;
        public string DeviceSymptom { get; set; } = string.Empty;
    }
}
