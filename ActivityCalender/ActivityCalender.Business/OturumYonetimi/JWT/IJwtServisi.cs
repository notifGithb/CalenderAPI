using ActivityCalender.Business.OturumYonetimi.JWT.Token;
using ActivityCalender.Entities;

namespace ActivityCalender.Business.OturumYonetimi.JWT
{
    public interface IJwtServisi
    {
        JwtToken JwtTokenOlustur(Kullanici kullanici);

    }
}
