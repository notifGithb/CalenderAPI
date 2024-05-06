using ActivityCalender.Business.Kullanicilar.DTOs;

namespace ActivityCalender.Business.Kullanicilar
{
    public interface IKullaniciServisi
    {
        Task<KullaniciGetirDTO?> KullaniciGetir(string kullaniciID);
        Task<IEnumerable<KullaniciGetirDTO>> KullanicilariGetir(string kullaniciID);
    }
}
