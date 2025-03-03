using System.ComponentModel.DataAnnotations;
namespace Numarataj.DTO.DTOs.ResmiKurumDTOs
{
    public class ResultResmiKurumDto
    {
        [Key]
        public int BelgeNoId { get; set; }
        public DateTime Tarih { get; set; }
        public string? TcKimlikNo { get; set; }
        public string? AdSoyad { get; set; }
        public string? Telefon { get; set; }
        public string? Mahalle { get; set; }
        public string? CaddeSokak { get; set; }
        public string? DisKapi { get; set; }
        public string? IcKapiNo { get; set; } // Türkçe karakter düzeltildi
        public string? SiteAdi { get; set; }
        public string? BagimsizBolge { get; set; }
        public string? EskiAdres { get; set; }
        public string? BlokAdi { get; set; }
        public string? AdresNo { get; set; } // Tür düzeltildi
        public string? IsYeriUnvani { get; set; }
        public string? IsyeriSayisi { get; set; } // Tür düzeltildi
        public string? Pafta { get; set; }
        public string? Ada { get; set; }
        public string? Parsel { get; set; }
        public string? EsBina { get; set; } // EsBine yerine EsBina yazıldı
    }
}
