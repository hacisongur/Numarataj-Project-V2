using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Numarataj.DataAccess.Context;
using Numarataj.DTO.DTOs.MergedDataDtos;
using Numarataj.WebUI.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Numarataj.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly NumaratajDbContext _context;

        // HomeController constructor to inject the DbContext
        public HomeController(NumaratajDbContext context)
        {
            _context = context;
        }

        // Index method to get data and send to the view
        public async Task<IActionResult> Index()
        {
            // Retrieve data from different tables in the database
            var ozelIsyeriData = await _context.OzelIsyeri.ToListAsync();
            var adresTespitData = await _context.AdresTespit.ToListAsync();
            var sahaCalismasiData = await _context.SahaCalismasi.ToListAsync();
            var resmiKurumData = await _context.ResmiKurum.ToListAsync();
            var yeniBinaData = await _context.YeniBina.ToListAsync();

            // Initialize a list to merge data from all the tables
            var mergedData = new List<MergedDataDto>();

            // Add data from OzelIsyeri table to mergedData
            mergedData.AddRange(ozelIsyeriData.Select(x => new MergedDataDto
            {
                BelgeNoId = x.BelgeNoId,
                TcKimlikNo = x.TcKimlikNo,
                AdSoyad = x.AdSoyad,
                Tarih = x.Tarih.HasValue ? x.Tarih.Value : DateTime.MinValue,
                Telefon = x.Telefon,
                Mahalle = x.Mahalle,
                CaddeSokak = x.CaddeSokak,
                DisKapi = x.DisKapi,
                TableName = "OzelIsyeri", // Tablo Adý
                Type = 2
            }));

            // Add data from AdresTespit table to mergedData
            mergedData.AddRange(adresTespitData.Select(x => new MergedDataDto
            {
                BelgeNoId = x.BelgeNoId,
                TcKimlikNo = x.TcKimlikNo,
                AdSoyad = x.AdSoyad,
                Tarih = x.Tarih.HasValue ? x.Tarih.Value : DateTime.MinValue,
                Telefon = x.Telefon,
                Mahalle = x.Mahalle,
                CaddeSokak = x.CaddeSokak,
                DisKapi = x.DisKapi,
                TableName = "AdresTespit", // Tablo Adý
                Type = 1
            }));

            // Add data from SahaCalismasi table to mergedData
            mergedData.AddRange(sahaCalismasiData.Select(x => new MergedDataDto
            {
                BelgeNoId = x.BelgeNoId,
                TcKimlikNo = x.TcKimlikNo,
                AdSoyad = x.AdSoyad,
                Tarih = x.Tarih.HasValue ? x.Tarih.Value : DateTime.MinValue,
                Telefon = x.Telefon,
                Mahalle = x.Mahalle,
                CaddeSokak = x.CaddeSokak,
                DisKapi = x.DisKapi,
                TableName = "SahaCalismasi", // Tablo Adý
                Type = 3
            }));

            // Add data from ResmiKurum table to mergedData
            mergedData.AddRange(resmiKurumData.Select(x => new MergedDataDto
            {
                BelgeNoId = x.BelgeNoId,
                TcKimlikNo = x.TcKimlikNo,
                AdSoyad = x.AdSoyad,
                Tarih = x.Tarih.HasValue ? x.Tarih.Value : DateTime.MinValue,
                Telefon = x.Telefon,
                Mahalle = x.Mahalle,
                CaddeSokak = x.CaddeSokak,
                DisKapi = x.DisKapi,
                TableName = "ResmiKurum", // Tablo Adý
                Type = 4
            }));

            // Add data from YeniBina table to mergedData
            mergedData.AddRange(yeniBinaData.Select(x => new MergedDataDto
            {
                BelgeNoId = x.BelgeNoId,
                TcKimlikNo = x.TcKimlikNo,
                AdSoyad = x.AdSoyad,
                Tarih = x.Tarih.HasValue ? x.Tarih.Value : DateTime.MinValue,
                Telefon = x.Telefon,
                Mahalle = x.Mahalle,
                CaddeSokak = x.CaddeSokak,
                DisKapi = x.DisKapi,
                TableName = "YeniBina", // Tablo Adý
                Type = 5
            }));

            // Send the merged data to the View
            return View(mergedData);
        }

        // Privacy method
        public IActionResult Privacy()
        {
            return View();
        }

        // Error method to handle errors
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
