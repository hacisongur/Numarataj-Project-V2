
namespace Numarataj.DTO.DTOs.MergedDataDtos
{
    public class MergedDataDto
    {
        public int BelgeNoId { get; set; }
        public string TcKimlikNo { get; set; }
        public string AdSoyad { get; set; }
        public DateTime? Tarih { get; set; }
        public string Telefon { get; set; }
        public string Mahalle { get; set; }
        public string CaddeSokak { get; set; }
        public string DisKapi { get; set; }
        public string TableName { get; set; } // Yeni Tablo Adı Özelliği
        public int Type { get; set; }
    }
}
