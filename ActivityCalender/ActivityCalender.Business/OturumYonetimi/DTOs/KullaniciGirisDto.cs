using System.ComponentModel.DataAnnotations;

namespace ActivityCalender.Business.OturumYonetimi.DTOs
{
    public class KullaniciGirisDto
    {
        [Required(ErrorMessage = "Isim zorunlu")]
        public required string KullaniciAdi { get; set; }


        [Required(ErrorMessage = "Sifre zorunlu")]
        [DataType(DataType.Password)]
        public required string KullaniciSifresi { get; set; }
    }
}
