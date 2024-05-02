using ActivityCalender.Business.OturumYonetimi.DTOs;
using ActivityCalender.Business.OturumYonetimi.JWT.Token;

namespace ActivityCalender.Business.OturumYonetimi
{
    public interface IOturumYonetimi
    {
        Task KayitOl(KullaniciKayitDto model);

        Task<JwtToken?> GirisYap(KullaniciGirisDto model);

    }
}
