namespace Numarataj.DTO.DTOs.SahaCalismasiDTOs
{
    public class CreateSahaCalismasiDto
    {
        public int? KayıtSiraNo { get; set; }
        public DateTime? Tarih { get; set; }
        public string? TcKimlikNo { get; set; } // Eksik alan eklendi
        public string? AdSoyad { get; set; }
        public string? BagimsizBolge { get; set; }
        public string? Mahalle { get; set; }
        public string? DisKapi { get; set; }
        public string? IcKapiNo { get; set; } // Eksik alan eklendi
        public string? Ada { get; set; }
        public string? Parsel { get; set; }
        public string? CaddeSokak { get; set; }
    }
}
