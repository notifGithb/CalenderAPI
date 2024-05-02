using System.ComponentModel.DataAnnotations;

namespace ActivityCalender.Entities
{
    public class KullaniciEtkinlik
    {
        [Key]
        public int Id { get; set; }

        public int EtkinlikId { get; set; }
        public Etkinlik Etkinlik { get; set; }

        public required string KullaniciId { get; set; }
        public Kullanici Kullanici { get; set; }
    }
}
