using ActivityCalender.Entities;

namespace ActivityCalender.Business.Etkinlikler.DTOs
{
    public class EtkinlikGetirDTO
    {
        public int Id { get; set; }
        public required string Baslik { get; set; }
        public string? Aciklama { get; set; }
        public required string BaslangicSaati { get; set; }
        public required string BitisSaati { get; set; }
        public required string BaslangicTarihi { get; set; }
        public required string BitisTarihi { get; set; }
        public TekrarEnum TekrarDurumu { get; set; }
    }
}
