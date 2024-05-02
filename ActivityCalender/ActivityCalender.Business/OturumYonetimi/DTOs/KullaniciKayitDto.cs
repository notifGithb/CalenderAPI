using System.ComponentModel.DataAnnotations;

namespace ActivityCalender.Business.OturumYonetimi.DTOs
{
    public class KullaniciKayitDto
    {
        [Required(ErrorMessage = "Kullanıcı Adı zorunlu")]
        public required string KullaniciAdi { get; set; }

        [Required(ErrorMessage = "Isim zorunlu")]
        public required string Isim { get; set; }

        [Required(ErrorMessage = "Soyisim zorunlu")]
        public required string Soyisim { get; set; }

        [Required(ErrorMessage = "Sifre zorunlu")]
        [DataType(DataType.Password)]
        public required string KullaniciSifresi { get; set; }

        [Required(ErrorMessage = "Şifre tekrarı zorunlu")]
        [DataType(DataType.Password)]
        [Compare("KullaniciSifresi", ErrorMessage = "Girmiş olduğunuz parola birbiri ile eşleşmiyor.")]
        public required string KullaniciSifresiTekrar { get; set; }
    }
}
