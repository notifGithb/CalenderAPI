using System.ComponentModel.DataAnnotations;

namespace ActivityCalender.Entities
{
    public class Kullanici
    {
        public Kullanici()
        {
            OlusturduguEtkinlikler = new HashSet<Etkinlik>();
            KatildigiEtkinlikler = new HashSet<KullaniciEtkinlik>();
        }
        [Key]
        public required string Id { get; set; }
        public required string KullaniciAdi { get; set; }
        public required string Isim { get; set; }
        public required string Soyisim { get; set; }
        public required string KullaniciSifresi { get; set; }

        public ICollection<Etkinlik> OlusturduguEtkinlikler { get; set; } 
        public ICollection<KullaniciEtkinlik> KatildigiEtkinlikler { get; set; }

    }
}
