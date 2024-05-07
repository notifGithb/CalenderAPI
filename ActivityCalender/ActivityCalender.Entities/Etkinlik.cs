using System.ComponentModel.DataAnnotations;

namespace ActivityCalender.Entities
{
    public sealed class Etkinlik
    {
        public Etkinlik()
        {
            KatilanKullanicilar = new HashSet<KullaniciEtkinlik>();
        }

        [Key]
        public int Id { get; set; }
        public required string Baslik { get; set; }
        public string? Aciklama { get; set; }
        public required string BaslangicSaati { get; set; }
        public required string BitisSaati { get; set; }
        public required string BaslangicTarihi { get; set; }
        public required string BitisTarihi { get; set; }
        public TekrarEnum TekrarDurumu { get; set; }

        public required Kullanici OlusturanKullanici { get; set; }
        public required string OlusturanKullaniciId { get; set; }

        public ICollection<KullaniciEtkinlik> KatilanKullanicilar { get; set; }

    }
}
