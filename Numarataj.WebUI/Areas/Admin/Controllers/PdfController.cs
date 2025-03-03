using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Numarataj.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Numarataj.DTO.DTOs.BasePdfDTOs;
using Microsoft.AspNetCore.Identity;
using Numarataj.Entity.Entities;

namespace Numarataj.WebUI.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin,Personel")]
    [Area("Admin")]
    public class PdfController : Controller
    {
        private readonly NumaratajDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PdfController(NumaratajDbContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<IActionResult> GeneratePdf(int id, string title, string pdfName, int type, bool isTcHidden = false)
        {
            var currentUser = await _userManager.GetUserAsync(User); // Kullanıcıyı al
            string currentUserFullName = currentUser?.FullName;
            var ozelisyeriValues = await _context.OzelIsyeri.FirstOrDefaultAsync(x => x.BelgeNoId == id);
            var adresTespitResponse = _mapper.Map<ResultPdfDto>(ozelisyeriValues);

            if (type == 1)
            {
                var adresTespitValues = await _context.AdresTespit.FirstOrDefaultAsync(x => x.BelgeNoId == id);
                adresTespitResponse = _mapper.Map<ResultPdfDto>(adresTespitValues);
            }
            if (type == 2)
            {
                ozelisyeriValues = await _context.OzelIsyeri.FirstOrDefaultAsync(x => x.BelgeNoId == id);
                adresTespitResponse = _mapper.Map<ResultPdfDto>(ozelisyeriValues);
            }
            if (type == 3)
            {
                var sahacalismasiValues = await _context.SahaCalismasi.FirstOrDefaultAsync(x => x.BelgeNoId == id);
                adresTespitResponse = _mapper.Map<ResultPdfDto>(sahacalismasiValues);
            }
            if (type == 4)
            {
                var resmikurumValues = await _context.ResmiKurum.FirstOrDefaultAsync(x => x.BelgeNoId == id);
                adresTespitResponse = _mapper.Map<ResultPdfDto>(resmikurumValues);
            }
            if (type == 5)
            {
                var yenibinaValues = await _context.YeniBina.FirstOrDefaultAsync(x => x.BelgeNoId == id);
                adresTespitResponse = _mapper.Map<ResultPdfDto>(yenibinaValues);
            }
            if (adresTespitResponse == null)
            {
                return NotFound(); // Eğer veri yoksa 404 döndür
            }

            using (MemoryStream stream = new MemoryStream())
            {
                // PDF belgesi oluşturma
                Document pdfDoc = new Document(PageSize.A4, 50, 50, 25, 25);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Türkçe karakterler için Arial Unicode MS fontunu kullanıyoruz
                string arialUnicodeMSPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIALUNI.TTF");
                BaseFont bfArialUniCode = BaseFont.CreateFont(arialUnicodeMSPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);

                // Font ayarları
                Font titleFont = new Font(bfArialUniCode, 16, Font.BOLD);
                Font headerFont = new Font(bfArialUniCode, 12, Font.BOLD);
                Font bodyFont = new Font(bfArialUniCode, 10, Font.NORMAL);
                Font kalinFont = new Font(bfArialUniCode, 20, Font.BOLD);

                // ** Logo Ekleyin (Sol Üst Kısım) **
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/matrix-admin-master/assets/images/logo-rapor.png");
                iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);

                logo.ScaleToFit(110f, 60f); // Genişlik 100, yükseklik 50 olacak şekilde ayarlandı
                logo.SetAbsolutePosition(43, pdfDoc.PageSize.Height - 100); // Sol üst köşeye, hafif aşağıda
                pdfDoc.Add(logo);
                // Belge Başlıkları
                var tCParagraph = new Paragraph(new Chunk("T.C.", titleFont)) { Alignment = Element.ALIGN_CENTER };
                pdfDoc.Add(tCParagraph);

                var belediyeBaskamligiParagraph = new Paragraph(new Chunk("BATMAN BELEDİYESİ", titleFont)) { Alignment = Element.ALIGN_CENTER };
                pdfDoc.Add(belediyeBaskamligiParagraph);

                var imarMudurluguParagraph = new Paragraph(new Chunk("İMAR VE ŞEHİRCİLİK MÜDÜRLÜĞÜ", headerFont)) { Alignment = Element.ALIGN_CENTER };
                pdfDoc.Add(imarMudurluguParagraph);

                var etutProjeParagraph = new Paragraph(new Chunk("NUMARATAJ BİRİMİ", bodyFont)) { Alignment = Element.ALIGN_CENTER };
                pdfDoc.Add(etutProjeParagraph);
                pdfDoc.Add(new Paragraph("\n"));
                pdfDoc.Add(new Paragraph("\n"));

               

                PdfPTable numaratajTable = new PdfPTable(3);
                numaratajTable.WidthPercentage = 100;
                numaratajTable.SetWidths(new float[] { 1, 3, 1 }); // İlk ve son sütun dar, orta sütun geniş

                // "Belge No" hücresini sola hizala
                numaratajTable.AddCell(new PdfPCell(new Phrase("Belge No : " + (adresTespitResponse.BelgeNoId.ToString() ?? ""), bodyFont))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    HorizontalAlignment = Element.ALIGN_LEFT // Sol hizalama
                });
                numaratajTable.AddCell(new PdfPCell(new Phrase("NUMARATAJ BELGESİ", new Font(bfArialUniCode, 16, Font.BOLD, BaseColor.BLACK)))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    HorizontalAlignment = Element.ALIGN_CENTER, // Ortala
                    VerticalAlignment = Element.ALIGN_MIDDLE, // Dikeyde ortala
                    Padding = 4, // Hücre içindeki boşluk
                    BackgroundColor = new BaseColor(211, 211, 211) // Açık gri renk (RGB değerleri ile)
                });


                // "Tarih" hücresini sağa hizala
                numaratajTable.AddCell(new PdfPCell(new Phrase("Tarih : " + DateTime.Now.ToString("dd/MM/yyyy"), bodyFont))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    HorizontalAlignment = Element.ALIGN_RIGHT // Sağ hizalama
                });


                // Tabloyu PDF belgesine ekle
                pdfDoc.Add(numaratajTable);

                // Boşluk ekle
                pdfDoc.Add(new Paragraph("\n"));

                PdfPTable specialBusinessTable = new PdfPTable(1)
                {
                    WidthPercentage = 100
                };

                // Başlık hücresini oluşturuyoruz
                PdfPCell titleCell = new PdfPCell(new Phrase(title, headerFont))
                {
                    Border = Rectangle.BOX, // Kenar çizgisi ekle
                    Padding = 5, // İçerik etrafında boşluk (yüksekliği etkileyebilir)
                    MinimumHeight = 20, // Hücrenin minimum yüksekliği
                    HorizontalAlignment = Element.ALIGN_CENTER, // Yatayda ortala
                    VerticalAlignment = Element.ALIGN_MIDDLE, // Dikeyde ortala (yazıyı tam ortada yapar)
                    BackgroundColor = new BaseColor(240, 240, 240) // Arka plan rengini açık gri yapıyoruz
                };
                PdfPTable titleTable = new PdfPTable(1)
                {
                    WidthPercentage = 100 // Tablo genişliğini %100 yapıyoruz
                };
                titleTable.AddCell(titleCell);
                pdfDoc.Add(titleTable);
                pdfDoc.Add(new Paragraph("\n"));

                // Tabloyu oluşturuyoruz
                PdfPTable headerTable = new PdfPTable(1)
                {
                    WidthPercentage = 100// Tablo genişliğini %100 yapıyoruz
                };
                PdfPCell headerCell = new PdfPCell(new Phrase("KİMLİK BİLGİLERİ", headerFont))
                {
                    Border = Rectangle.NO_BORDER,
                    Padding = 4,
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    VerticalAlignment = Element.ALIGN_LEFT,
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                headerTable.AddCell(headerCell);
                pdfDoc.Add(headerTable);


                // Kimlik bilgileri tablosu
                PdfPTable kimlikTable = new PdfPTable(2);
                kimlikTable.WidthPercentage = 100;
                if (!isTcHidden)
                {
                    kimlikTable.AddCell(new PdfPCell(new Phrase("TC Kimlik No:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                    kimlikTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.TcKimlikNo, bodyFont)) { Padding = 8, MinimumHeight = 8 });
                }
                kimlikTable.AddCell(new PdfPCell(new Phrase("Ad/Soyad:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                kimlikTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.AdSoyad, bodyFont)) { Padding = 8, MinimumHeight = 8 });
                kimlikTable.AddCell(new PdfPCell(new Phrase("Telefon:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                kimlikTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.Telefon ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });

                pdfDoc.Add(kimlikTable);
                // Tabloyu oluşturuyoruz
                PdfPTable adresHeaderTable = new PdfPTable(1)
                {
                    WidthPercentage = 100 // Tablo genişliğini %100 yapıyoruz
                };

                pdfDoc.Add(new Paragraph(" ") { SpacingAfter = 0.1f });
                PdfPCell adresHeaderCell = new PdfPCell(new Phrase("ADRES BİLGİLERİ", headerFont))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    Padding = 4, // İçerik etrafında boşluk
                    HorizontalAlignment = Element.ALIGN_LEFT, // Yazıyı sola hizala
                    VerticalAlignment = Element.ALIGN_LEFT, // Dikeyde sola hizala
                    BackgroundColor = new BaseColor(240, 240, 240) // Arka plan rengi
                };
                adresHeaderTable.AddCell(adresHeaderCell);
                pdfDoc.Add(adresHeaderTable);



                PdfPTable adresTable = new PdfPTable(2);
                adresTable.WidthPercentage = 100;

                adresTable.AddCell(new PdfPCell(new Phrase("Adres No (UAVT) :", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.AdresNo?.ToString() ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase("Mahalle:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.Mahalle ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase("Cadde/Sokak/Bulvar:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.CaddeSokak ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase("Dış Kapı:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.DisKapi ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase("İç Kapı No:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.IcKapiNo ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });


                adresTable.AddCell(new PdfPCell(new Phrase("Yapı Adı/Site Adı:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.SiteAdi?.ToString() ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });

                // Adres Table'ı için koşul ekleyin
                if (type == 2)
                {
                    adresTable.AddCell(new PdfPCell(new Phrase("İş Yeri Ünvanı:", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                    adresTable.AddCell(new PdfPCell(new Phrase(adresTespitResponse.IsYeriUnvani ?? "", bodyFont)) { Padding = 8, MinimumHeight = 8 });
                }

               


                pdfDoc.Add(adresTable);
                // TAPU BİLGİLERİ başlığı için tabloyu oluşturuyoruz
                PdfPTable tapuHeaderTable = new PdfPTable(1)
                {
                    WidthPercentage = 100 // Tablo genişliğini %100 yapıyoruz
                };
                pdfDoc.Add(new Paragraph(" ") { SpacingAfter = 0.1f });

                PdfPCell tapuHeaderCell = new PdfPCell(new Phrase("TAPU BİLGİLERİ", headerFont))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    Padding = 4, // İçerik etrafında boşluk
                    HorizontalAlignment = Element.ALIGN_LEFT, // Yazıyı sola hizala
                    VerticalAlignment = Element.ALIGN_LEFT, // Dikeyde sola hizala
                    BackgroundColor = new BaseColor(240, 240, 240) // Arka plan rengi
                };
                tapuHeaderTable.AddCell(tapuHeaderCell);
                pdfDoc.Add(tapuHeaderTable);


                PdfPTable tapuTable = new PdfPTable(2)
                {
                    WidthPercentage = 100 // Tablo genişliği (%100)
                };
                tapuTable.SetWidths(new float[] { 1f, 1f }); // Sütun genişlikleri eşit (ortalamak için)
                tapuTable.AddCell(new PdfPCell(new Phrase("Ada/Parsel:", bodyFont))
                {
                    Padding = 8,
                    MinimumHeight = 8
                });
                tapuTable.AddCell(new PdfPCell(new Phrase($"{adresTespitResponse.Ada ?? ""} / {adresTespitResponse.Parsel ?? ""}", bodyFont))
                {
                    Padding = 8,
                    MinimumHeight = 8
                });


                pdfDoc.Add(tapuTable);

                // ADRES DEĞİŞİKLİĞİ BİLGİLERİ başlığı için tabloyu oluşturuyoruz
                PdfPTable adresDegisiklikHeaderTable = new PdfPTable(1)
                {
                    WidthPercentage = 100 // Tablo genişliğini %100 yapıyoruz
                };
                pdfDoc.Add(new Paragraph(" ") { SpacingAfter = 0.1f });
                PdfPCell adresDegisiklikHeaderCell = new PdfPCell(new Phrase("ADRES DEĞİŞİKLİĞİ BİLGİLERİ", headerFont))
                {
                    Border = Rectangle.NO_BORDER, // Kenar çizgisi yok
                    Padding = 4, // İçerik etrafında boşluk
                    HorizontalAlignment = Element.ALIGN_LEFT, // Yazıyı sola hizala
                    VerticalAlignment = Element.ALIGN_MIDDLE, // Dikeyde ortala (yazının tam ortasında olacak)
                    BackgroundColor = new BaseColor(240, 240, 240) // Arka plan rengini açık gri yapıyoruz
                };
                adresDegisiklikHeaderTable.AddCell(adresDegisiklikHeaderCell);
                pdfDoc.Add(adresDegisiklikHeaderTable);

                string eskiAdres = adresTespitResponse?.EskiAdres ?? ""; // 📌 Eski adresi modelden al

                PdfPTable adresDegisiklikTable = new PdfPTable(2); // 📌 2 sütunlu tablo (başlık ve değer için)
                adresDegisiklikTable.WidthPercentage = 100;

                // 📌 "Eski Adres:" başlığını ekle (her zaman görünecek)
                adresDegisiklikTable.AddCell(new PdfPCell(new Phrase(
                    "Eski Adres:",
                    new Font(bfArialUniCode, 10, Font.NORMAL)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 10
                });

                // 📌 Eğer eski adres doluysa sadece adresi yazdır, boşsa tamamen boş bırak
                adresDegisiklikTable.AddCell(new PdfPCell(new Phrase(
                    !string.IsNullOrEmpty(eskiAdres) ? eskiAdres : "", // Eğer boşsa hiçbir şey yazılmayacak
                    new Font(bfArialUniCode, 10, Font.NORMAL)))
                {
                    HorizontalAlignment = Element.ALIGN_LEFT,
                    Padding = 10
                });

                // 📌 Tabloyu PDF'e ekle
                pdfDoc.Add(adresDegisiklikTable);
                pdfDoc.Add(new Paragraph("\n"));
                pdfDoc.Add(new Paragraph("\n"));



                Paragraph signatureParagraph = new Paragraph
                {
                    Alignment = Element.ALIGN_CENTER // Sağa hizalama
                };

                signatureParagraph.Add(new Chunk($"{currentUserFullName}\n", new Font(bfArialUniCode, 12, Font.BOLD)));
                signatureParagraph.Add(new Chunk("NUMARATAJ PERSONELİ", new Font(bfArialUniCode, 10, Font.NORMAL)));

                // Paragraph'ı PDF'e ekle
                pdfDoc.Add(signatureParagraph);


                PdfPTable table = new PdfPTable(1)
                {
                    TotalWidth = pdfDoc.PageSize.Width - pdfDoc.LeftMargin - pdfDoc.RightMargin,
                    LockedWidth = true,
                    SpacingBefore = 10f,
                    SpacingAfter = 10f
                };
                pdfDoc.Add(new Paragraph("\n"));
                pdfDoc.Add(new Paragraph("\n")); 
                // Hücre oluşturma ve ayarlama
                PdfPCell cell = new PdfPCell(new Phrase("Batman Belediyesi Şirinevler Mah. Atatürk Bulvarı No:2/Z-4 Batman Tel:(0488)2132759", new Font(bfArialUniCode, 10, Font.NORMAL)))
                {
                    BorderColor = BaseColor.BLACK,
                    BorderWidth = 0.5f,
                    Padding = 5,
                    HorizontalAlignment = Element.ALIGN_LEFT
                };
                table.AddCell(cell);
                pdfDoc.Add(table);
                // Filigran eklemek için
                PdfContentByte canvas = writer.DirectContent; // DirectContent ile yazının önüne filigran ekleriz

                // Daha şeffaf bir gri renk ve daha küçük yazı
                Font watermarkFont = new Font(bfArialUniCode, 130, Font.BOLD, new BaseColor(200, 200, 200)); // Gri renk
                var watermarkPhrase = new Phrase("NUMARATAJ", watermarkFont);

                // Filigranın daha şeffaf olması için biraz opaklık ekliyoruz
                BaseColor watermarkColor = new BaseColor(200, 200, 200, 100); // (200, 200, 200) gri rengini seçip, dördüncü parametreyi (opaklık) 100 ile şeffaf yapıyoruz
                Font watermarkFontTransparent = new Font(bfArialUniCode, 70, Font.BOLD, watermarkColor); // Opaklığı düşük font

                // Filigran metnini, sayfanın ortasına ve 45 derece açıyla yerleştiriyoruz
                var transparentWatermarkPhrase = new Phrase("NUMARATAJ", watermarkFontTransparent);

                ColumnText.ShowTextAligned(
                    canvas,
                    Element.ALIGN_CENTER,
                    transparentWatermarkPhrase,
                    pdfDoc.PageSize.Width / 2,
                    pdfDoc.PageSize.Height / 2,
                    45 // Açı
                );


                // PDF belgesini kapat
                pdfDoc.Close();
                writer.Close();

                // PDF içeriğini byte dizisi olarak al ve dosya olarak döndür
                byte[] pdfContent = stream.ToArray();
                return File(pdfContent, "application/pdf", pdfName);

            }
        }
    }
}
