namespace RepairTicket.Models
{
    /// <summary>
    /// โมเดลข้อมูลการตั้งค่าข้อมูลร้านค้าและการจำค่าอุปกรณ์พิมพ์
    /// </summary>
    public class ShopSetting
    {
        public int Id { get; set; } = 1;
        public string ShopName { get; set; } = string.Empty;
        public string ShopAddress { get; set; } = string.Empty;
        public string DefaultA4Printer { get; set; } = string.Empty;
        public string DefaultPosPrinter { get; set; } = string.Empty;
        public int LastPrintFormat { get; set; } = 0; // 0 = A4, 1 = POS 58mm
    }
}
