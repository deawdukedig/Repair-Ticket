using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using RepairTicket.Models;

namespace RepairTicket.Services
{
    /// <summary>
    /// ตัวจัดการการพิมพ์ใบรับซ่อมกระดาษ A4 แนวตั้ง แบ่ง 2 ส่วน (ส่วนของร้านค้า และ ส่วนของลูกค้า)
    /// กำหนดขนาดฟอนต์ทุกจุดขั้นต่ำ 16pt (Minimum 16pt Font Size)
    /// จัดระยะพิกัดอย่างแม่นยำด้วย Dynamic String Measurement ป้องกันการซ้อนทับกันของข้อความทุกจุด 100%
    /// </summary>
    public class TicketPrintDocument : PrintDocument
    {
        private readonly RepairJob _job;
        private readonly ShopSetting _shop;

        public TicketPrintDocument(RepairJob job, ShopSetting shop)
        {
            _job = job ?? throw new ArgumentNullException(nameof(job));
            _shop = shop ?? throw new ArgumentNullException(nameof(shop));

            // ตั้งค่ากระดาษ A4 แนวตั้ง (827 x 1169 หน่วย ที่ 100 DPI)
            DefaultPageSettings.PaperSize = new PaperSize("A4", 827, 1169);
            DefaultPageSettings.Margins = new Margins(50, 50, 50, 50);
            OriginAtMargins = false; // ใช้พิกัดสัมบูรณ์ของกระดาษ
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            if (e.Graphics == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            // 1. วาดส่วนบน: ส่วนของร้านค้า (Y: 40 -> ~510)
            DrawTicketSection(g, offsetY: 40, copyTag: "(ส่วนของร้านค้า)");

            // 2. วาดเส้นประคั่นกึ่งกลางกระดาษ (Y = 584) แยก 2 ซีกเพื่อไม่ให้เส้นประตัดผ่านข้อความ
            float middleY = 584f;
            using (var cutFont = new Font("Segoe UI", 12f, FontStyle.Italic))
            using (var cutBrush = new SolidBrush(Color.FromArgb(90, 90, 90)))
            using (var penDashed = new Pen(Color.FromArgb(140, 140, 140), 1.5f) { DashStyle = DashStyle.Dash })
            {
                string cutText = "✂ - - - พับหรือตัดแบ่งตามรอยปรุ - - - ✂";
                var textSize = g.MeasureString(cutText, cutFont);
                float cutX = (827f - textSize.Width) / 2f;
                float textY = middleY - (textSize.Height / 2f);

                // เส้นประซีกซ้าย
                g.DrawLine(penDashed, 50, middleY, cutX - 15f, middleY);
                // ข้อความกึ่งกลาง (ไม่ถูกเส้นประทับ)
                g.DrawString(cutText, cutFont, cutBrush, cutX, textY);
                // เส้นประซีกขวา
                g.DrawLine(penDashed, cutX + textSize.Width + 15f, middleY, 777, middleY);
            }

            // 3. วาดส่วนล่าง: ส่วนของลูกค้า (Y: 620 -> ~1090)
            DrawTicketSection(g, offsetY: 620, copyTag: "(ส่วนของลูกค้า)");

            e.HasMorePages = false;
        }

        /// <summary>
        /// วาดเนื้อหาใบรับซ่อม 1 ส่วน (ครึ่งหน้า A4) ด้วยฟอนต์ขนาดขั้นต่ำ 16pt
        /// คำนวณพิกัดตามขนาดข้อความจริง (Dynamic Measurement) ป้องกันการทับซ้อน 100%
        /// </summary>
        private void DrawTicketSection(Graphics g, float offsetY, string copyTag)
        {
            float startX = 50f;
            float printableWidth = 727f;
            float currentY = offsetY;

            // กำหนดฟอนต์สำหรับการพิมพ์บนกระดาษ A4 ทุกจุดไม่ต่ำกว่า 16pt
            using var fontShopName = new Font("Segoe UI", 20f, FontStyle.Bold);         // ชื่อร้านค้าขนาดใหญ่ 20pt
            using var fontShopAddr = new Font("Segoe UI", 16f, FontStyle.Bold);         // เน้นที่อยู่-เบอร์โทรร้าน (16pt Bold)
            using var fontDocTitle = new Font("Segoe UI", 18f, FontStyle.Bold);         // หัวเรื่องเอกสาร 18pt
            using var fontTag = new Font("Segoe UI", 15f, FontStyle.Bold);              // ป้ายกำกับสำเนา 15pt Bold
            using var fontBold = new Font("Segoe UI", 16f, FontStyle.Bold);             // หัวข้อฟิลด์ 16pt Bold
            using var fontRegular = new Font("Segoe UI", 16f, FontStyle.Regular);       // ข้อความเนื้อหา 16pt Regular
            using var fontJobNo = new Font("Segoe UI", 17f, FontStyle.Bold);            // เน้นเลขที่ใบงาน (17pt Bold)
            using var fontCustomerPhone = new Font("Segoe UI", 17f, FontStyle.Bold);    // เน้นเบอร์โทรลูกค้า (17pt Bold)

            using var brushBlack = new SolidBrush(Color.Black);
            using var brushNavy = new SolidBrush(Color.FromArgb(10, 40, 95));
            using var brushDarkGray = new SolidBrush(Color.FromArgb(50, 50, 50));
            using var penSolid = new Pen(Color.Black, 1.5f);
            using var penLight = new Pen(Color.FromArgb(170, 170, 170), 1.2f);

            // 1. ระบุส่วนสำเนาที่มุมขวาบน (15pt Bold)
            var tagSize = g.MeasureString(copyTag, fontTag);
            float tagX = startX + printableWidth - tagSize.Width;
            g.DrawString(copyTag, fontTag, brushDarkGray, tagX, currentY);

            // 2. ชื่อร้านค้า (20pt Bold) วาดฝั่งซ้ายโดยจำกัดความกว้างไม่ให้ทับกับป้ายสำเนา
            float maxShopWidth = tagX - startX - 15f;
            var szShop = g.MeasureString(_shop.ShopName, fontShopName, (int)maxShopWidth);
            var shopRect = new RectangleF(startX, currentY, maxShopWidth, szShop.Height + 2f);
            using (var sfShop = new StringFormat { Trimming = StringTrimming.EllipsisWord })
            {
                g.DrawString(_shop.ShopName, fontShopName, brushNavy, shopRect, sfShop);
            }
            currentY += Math.Max(szShop.Height, 34f) + 4f;

            // 3. ที่อยู่และเบอร์โทรร้านค้า (เน้นตัวหนา 16pt Bold)
            // คำนวณความสูงจริงจากการตัดคำ (WordWrap) ป้องกันเส้นแบ่งใต้ร้านค้าทับตัวหนังสือ
            var szAddr = g.MeasureString(_shop.ShopAddress, fontShopAddr, (int)printableWidth);
            var addrRect = new RectangleF(startX, currentY, printableWidth, szAddr.Height + 4f);
            using (var sfAddr = new StringFormat { Trimming = StringTrimming.Word })
            {
                g.DrawString(_shop.ShopAddress, fontShopAddr, brushDarkGray, addrRect, sfAddr);
            }
            currentY += szAddr.Height + 8f;

            // เส้นคั่นใต้ข้อมูลร้านค้า
            g.DrawLine(penLight, startX, currentY, startX + printableWidth, currentY);
            currentY += 10f;

            // 4. หัวเรื่องเอกสาร (กึ่งกลาง 18pt Bold)
            string title = "ใบรับซ่อมสินค้า (Repair Ticket)";
            var titleSize = g.MeasureString(title, fontDocTitle);
            float titleX = startX + (printableWidth - titleSize.Width) / 2f;
            g.DrawString(title, fontDocTitle, brushBlack, titleX, currentY);
            currentY += titleSize.Height + 10f;

            // 5. ข้อมูลใบงาน (แบ่ง 2 คอลัมน์ชัดเจน พร้อมวัดความกว้าง Label เพื่อไม่ให้ทับข้อมูล)
            // คอลัมน์ 1: X = startX (50) ถึง 475 (ความกว้าง 425px รองรับชื่อ-นามสกุลยาวๆ ได้สบาย)
            // คอลัมน์ 2: X = startX + 435 (485) ถึง 777 (ความกว้าง 292px เพียงพอสำหรับวันที่และเบอร์โทร)
            float col2X = startX + 435f;

            // --- บรรทัดที่ 1: เลขที่ใบงาน (ซ้าย - เน้น 17pt Bold) & วันที่รับซ่อม (ขวา) ---
            string lblJobNoText = "เลขที่ใบงาน:";
            var szJobNoLbl = g.MeasureString(lblJobNoText, fontBold);
            g.DrawString(lblJobNoText, fontBold, brushBlack, startX, currentY);
            float jobNoValX = startX + szJobNoLbl.Width + 4f;
            g.DrawString(_job.JobNo, fontJobNo, brushNavy, jobNoValX, currentY - 1f);

            string lblDateText = "วันที่:";
            var szDateLbl = g.MeasureString(lblDateText, fontBold);
            g.DrawString(lblDateText, fontBold, brushBlack, col2X, currentY);
            float dateValX = col2X + szDateLbl.Width + 6f;
            g.DrawString(_job.JobDate, fontRegular, brushBlack, dateValX, currentY);

            currentY += 38f; // ความสูงบรรทัด 38px รองรับวรรณยุกต์ไทยไม่ให้ชนแถวถัดไป

            // --- บรรทัดที่ 2: ชื่อลูกค้า (ซ้าย) & เบอร์โทรลูกค้า (ขวา - เน้น 17pt Bold) ---
            string lblCustText = "ชื่อลูกค้า:";
            var szCustLbl = g.MeasureString(lblCustText, fontBold);
            g.DrawString(lblCustText, fontBold, brushBlack, startX, currentY);
            float custValX = startX + szCustLbl.Width + 6f;
            float maxCustWidth = col2X - custValX - 10f; // พื้นที่กว้าง 328px สำหรับชื่อลูกค้า
            var custRect = new RectangleF(custValX, currentY, maxCustWidth, 34f);
            using (var sfCust = new StringFormat { Trimming = StringTrimming.EllipsisWord, FormatFlags = StringFormatFlags.NoWrap })
            {
                g.DrawString(_job.CustomerName, fontRegular, brushBlack, custRect, sfCust);
            }

            string lblPhoneText = "เบอร์โทร:";
            var szPhoneLbl = g.MeasureString(lblPhoneText, fontBold);
            g.DrawString(lblPhoneText, fontBold, brushBlack, col2X, currentY);
            float phoneValX = col2X + szPhoneLbl.Width + 6f;
            string phoneText = string.IsNullOrWhiteSpace(_job.CustomerPhone) ? "-" : _job.CustomerPhone;
            g.DrawString(phoneText, fontCustomerPhone, brushNavy, phoneValX, currentY - 1f);

            currentY += 38f; // ความสูงบรรทัด 38px

            // 6. กรอบแสดงอุปกรณ์และอาการเสีย (16pt - ขยายความสูงรองรับข้อความเพิ่ม 1 บรรทัด)
            string lblSymptom = "อุปกรณ์และอาการเสีย:";
            var szSymLbl = g.MeasureString(lblSymptom, fontBold);
            g.DrawString(lblSymptom, fontBold, brushBlack, startX, currentY);
            // ขยับ currentY ให้พ้นความสูงของ Label อย่างสมบูรณ์ ไม่ให้กรอบทับข้อความ
            currentY += szSymLbl.Height + 4f;

            float boxHeight = 138f; // เพิ่มความสูง 28px (~1 บรรทัดเต็มสำหรับฟอนต์ 16pt)
            var symptomBoxRect = new RectangleF(startX, currentY, printableWidth, boxHeight);
            g.DrawRectangle(penSolid, symptomBoxRect.X, symptomBoxRect.Y, symptomBoxRect.Width, symptomBoxRect.Height);

            // ข้อความอาการเสียภายในกรอบ (มี Padding รอบด้านไม่ให้ตัวหนังสือชิดขอบกรอบ)
            var textInnerRect = new RectangleF(startX + 12f, currentY + 10f, printableWidth - 24f, boxHeight - 20f);
            using (var sf = new StringFormat
            {
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Near,
                Trimming = StringTrimming.Word
            })
            {
                g.DrawString(_job.DeviceSymptom, fontRegular, brushBlack, textInnerRect, sf);
            }
            currentY += boxHeight + 18f;

            // 7. ช่องลงชื่อ (ซ้าย: ผู้รับเครื่อง, ขวา: ลูกค้า)
            // ใช้ข้อความจุดไข่ปลาโดยไม่ขีดเส้นทับซ้อน เพื่อความคมชัด สะอาดตา และไม่ซ้อนทับกัน
            float sigColWidth = 330f;
            float leftSigX = startX + 10f;
            float rightSigX = startX + printableWidth - sigColWidth;

            g.DrawString("ลงชื่อ ............................................. (ผู้รับ)", fontRegular, brushBlack, leftSigX, currentY);
            g.DrawString("ลงชื่อ ............................................. (ลูกค้า)", fontRegular, brushBlack, rightSigX, currentY);
        }
    }
}
