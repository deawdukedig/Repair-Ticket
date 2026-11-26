using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Drawing.Text;
using RepairTicket.Models;

namespace RepairTicket.Services
{
    /// <summary>
    /// ตัวจัดการการพิมพ์สลิปความร้อน POS 58mm ด้วยระบบ Bitmap
    /// ป้องกันปัญหาตัวอักษรภาษาไทยเพี้ยน สระลอย หรือขาดหายบนเครื่องพิมพ์ความร้อน 100%
    /// </summary>
    public class PosSlipPrintDocument : PrintDocument
    {
        private readonly RepairJob _job;
        private readonly ShopSetting _shop;
        private Bitmap? _cachedSlipBitmap;

        public PosSlipPrintDocument(RepairJob job, ShopSetting shop)
        {
            _job = job ?? throw new ArgumentNullException(nameof(job));
            _shop = shop ?? throw new ArgumentNullException(nameof(shop));

            // กำหนดขนาดหน้ากระดาษ 58mm (ความกว้าง ~228 หน่วย ที่ 100 DPI)
            // ความยาวต่อเนื่อง (Roll Paper) ประมาณ 1200-1400 หน่วย
            DefaultPageSettings.PaperSize = new PaperSize("POS58", 228, 1400);
            DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            OriginAtMargins = false;
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            if (e.Graphics == null) return;

            // สร้างภาพ Bitmap สลิปขนาดกว้าง 384 พิกเซล (พื้นที่พิมพ์จริงของหัวพิมพ์ 58mm)
            _cachedSlipBitmap ??= GenerateSlipBitmap();

            // วาดภาพ Bitmap สลิปลงบนหน้ากระดาษของเครื่องพิมพ์ POS
            // ปรับสเกลให้อยู่ในความกว้าง 228 หน่วยของระบบ PrintDocument
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
            
            float targetWidth = 228f;
            float scale = targetWidth / _cachedSlipBitmap.Width;
            float targetHeight = _cachedSlipBitmap.Height * scale;

            e.Graphics.DrawImage(_cachedSlipBitmap, 0, 0, targetWidth, targetHeight);
            e.HasMorePages = false;
        }

        /// <summary>
        /// เรนเดอร์เนื้อหาสลิป 58mm ทั้งหมดลงบนภาพ Bitmap ขนาด 384 พิกเซล
        /// โดยใช้ระบบ GDI+ Typography ของ Windows เพื่อให้ภาษาไทยคมชัดสมบูรณ์แบบ
        /// </summary>
        public Bitmap GenerateSlipBitmap()
        {
            int width = 384; // พิกเซลมาตรฐานของหัวพิมพ์ 58mm (203 DPI)

            // วัดความสูงที่ต้องการก่อนเพื่อสร้าง Bitmap ขนาดพอดี
            float measuredHeight = MeasureTotalHeight(width);
            int totalHeight = Math.Max((int)Math.Ceiling(measuredHeight) + 40, 600);

            Bitmap bmp = new Bitmap(width, totalHeight);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                float currentY = 10f;

                // วาดสำเนาที่ 1: ส่วนของร้านค้า
                currentY = DrawSlipSection(g, currentY, width, "[ ส่วนของร้านค้า ]");

                // วาดเส้นประรอยฉีกคั่นกลาง
                currentY += 15f;
                using (var penDashed = new Pen(Color.Black, 1.5f) { DashStyle = DashStyle.Dash })
                {
                    g.DrawLine(penDashed, 10, currentY, width - 10, currentY);
                }
                using (var cutFont = new Font("Tahoma", 9f, FontStyle.Italic))
                using (var cutBrush = new SolidBrush(Color.FromArgb(80, 80, 80)))
                {
                    string cutText = "✂ - - - - ฉีกตามรอยปรุ - - - - ✂";
                    var sz = g.MeasureString(cutText, cutFont);
                    g.DrawString(cutText, cutFont, cutBrush, (width - sz.Width) / 2f, currentY + 3f);
                }
                currentY += 28f;

                // วาดสำเนาที่ 2: ส่วนของลูกค้า
                currentY = DrawSlipSection(g, currentY, width, "[ ส่วนของลูกค้า ]");

                // เว้นระยะท้ายกระดาษสำหรับฉีก
                currentY += 30f;
            }

            return bmp;
        }

        /// <summary>
        /// วาดเนื้อหาสลิป 1 สำเนาบน Canvas
        /// </summary>
        private float DrawSlipSection(Graphics g, float startY, int width, string copyLabel)
        {
            float y = startY;
            float padding = 8f;
            float printableWidth = width - (padding * 2);

            using var fontShopName = new Font("Tahoma", 13f, FontStyle.Bold);
            using var fontShopAddr = new Font("Tahoma", 9f, FontStyle.Regular);
            using var fontTitle = new Font("Tahoma", 11f, FontStyle.Bold);
            using var fontLabel = new Font("Tahoma", 9.5f, FontStyle.Bold);
            using var fontValue = new Font("Tahoma", 9.5f, FontStyle.Regular);
            using var fontSmall = new Font("Tahoma", 8.5f, FontStyle.Regular);

            using var brushBlack = new SolidBrush(Color.Black);
            using var penSolid = new Pen(Color.Black, 1f);
            using var penLight = new Pen(Color.FromArgb(160, 160, 160), 1f);

            // 1. หัวเรื่องสำเนา (กึ่งกลาง)
            var szCopy = g.MeasureString(copyLabel, fontLabel);
            g.DrawString(copyLabel, fontLabel, brushBlack, (width - szCopy.Width) / 2f, y);
            y += 22f;

            // 2. ชื่อร้านค้า (กึ่งกลางตัวหนา)
            using (var sfCenter = new StringFormat { Alignment = StringAlignment.Center })
            {
                var shopRect = new RectangleF(padding, y, printableWidth, 40f);
                g.DrawString(_shop.ShopName, fontShopName, brushBlack, shopRect, sfCenter);
                var szShop = g.MeasureString(_shop.ShopName, fontShopName, (int)printableWidth);
                y += szShop.Height + 2f;

                // ที่อยู่ร้านค้า
                var addrRect = new RectangleF(padding, y, printableWidth, 60f);
                g.DrawString(_shop.ShopAddress, fontShopAddr, brushBlack, addrRect, sfCenter);
                var szAddr = g.MeasureString(_shop.ShopAddress, fontShopAddr, (int)printableWidth);
                y += szAddr.Height + 6f;
            }

            // เส้นคั่น
            g.DrawLine(penLight, padding, y, width - padding, y);
            y += 6f;

            // 3. หัวเรื่องเอกสาร
            string title = "ใบรับซ่อมสินค้า (Repair Slip)";
            var szTitle = g.MeasureString(title, fontTitle);
            g.DrawString(title, fontTitle, brushBlack, (width - szTitle.Width) / 2f, y);
            y += 24f;

            // 4. ข้อมูลใบงาน
            g.DrawString($"เลขที่ใบงาน: {_job.JobNo}", fontLabel, brushBlack, padding, y);
            y += 20f;
            g.DrawString($"วันที่รับซ่อม: {_job.JobDate}", fontValue, brushBlack, padding, y);
            y += 20f;
            g.DrawString($"ชื่อลูกค้า: {_job.CustomerName}", fontLabel, brushBlack, padding, y);
            y += 20f;
            if (!string.IsNullOrWhiteSpace(_job.CustomerPhone))
            {
                g.DrawString($"เบอร์โทร: {_job.CustomerPhone}", fontLabel, brushBlack, padding, y);
                y += 20f;
            }
            y += 4f;

            // 5. อุปกรณ์และอาการเสีย
            g.DrawString("อุปกรณ์/อาการเสีย:", fontLabel, brushBlack, padding, y);
            y += 18f;

            // คำนวณความสูงของกรอบอาการเสีย
            var szSymptom = g.MeasureString(_job.DeviceSymptom, fontValue, (int)(printableWidth - 10));
            float boxHeight = Math.Max(szSymptom.Height + 12f, 40f);

            var boxRect = new RectangleF(padding, y, printableWidth, boxHeight);
            g.DrawRectangle(penSolid, boxRect.X, boxRect.Y, boxRect.Width, boxRect.Height);

            var textRect = new RectangleF(padding + 5f, y + 5f, printableWidth - 10f, boxHeight - 10f);
            using (var sfWrap = new StringFormat { Trimming = StringTrimming.Word })
            {
                g.DrawString(_job.DeviceSymptom, fontValue, brushBlack, textRect, sfWrap);
            }
            y += boxHeight + 16f;

            // 6. ช่องลงชื่อ (2 แถวบนสลิปแคบ)
            g.DrawString("ลงชื่อ .......................................... ผู้รับเครื่อง", fontSmall, brushBlack, padding + 10f, y);
            y += 24f;
            g.DrawString("ลงชื่อ .......................................... ลูกค้า", fontSmall, brushBlack, padding + 10f, y);
            y += 26f;

            // ข้อความขอบคุณ
            string thankYou = "*** ขอบคุณที่ไว้วางใจใช้บริการ ***";
            var szThanks = g.MeasureString(thankYou, fontSmall);
            g.DrawString(thankYou, fontSmall, brushBlack, (width - szThanks.Width) / 2f, y);
            y += 20f;

            return y;
        }

        /// <summary>
        /// วัดขนาดความสูงรวมโดยประมาณ
        /// </summary>
        private float MeasureTotalHeight(int width)
        {
            using Bitmap dummyBmp = new Bitmap(1, 1);
            using Graphics g = Graphics.FromImage(dummyBmp);
            using Font fontValue = new Font("Tahoma", 9.5f);
            var sz = g.MeasureString(_job.DeviceSymptom, fontValue, width - 20);
            
            // 1 สำเนาใช้ความสูงประมาณ 280px + ความสูงของอาการเสีย
            float singleSectionHeight = 280f + sz.Height;
            // 2 สำเนา + เส้นคั่น + margin ท้าย
            return (singleSectionHeight * 2f) + 80f;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _cachedSlipBitmap?.Dispose();
                _cachedSlipBitmap = null;
            }
            base.Dispose(disposing);
        }
    }
}
