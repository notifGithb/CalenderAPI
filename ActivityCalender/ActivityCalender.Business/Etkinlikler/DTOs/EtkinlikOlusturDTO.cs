using ActivityCalender.Entities;
using System.ComponentModel;

namespace ActivityCalender.Business.Etkinlikler.DTOs
{
    public class EtkinlikOlusturDTO
    {
        public required string Baslik { get; set; }
        public string? Aciklama { get; set; }

        [DefaultValue("00:00")]
        public required string BaslangicSaati { get; set; }

        [DefaultValue("00:00")]
        public required string BitisSaati { get; set; }

        [DefaultValue("0001-01-01")]
        public required string BaslangicTarihi { get; set; }

        [DefaultValue("0001-01-01")]
        public required string BitisTarihi { get; set; }
        public TekrarEnum TekrarDurumu { get; set; }
    }
}
