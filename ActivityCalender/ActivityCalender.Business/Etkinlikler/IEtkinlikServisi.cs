using ActivityCalender.Business.Etkinlikler.DTOs;
using ActivityCalender.Business.Kullanicilar.DTOs;

namespace ActivityCalender.Business.Etkinlikler
{
    public interface IEtkinlikServisi
    {
        Task EtkinlikOlustur(EtkinlikOlusturDTO etkinlikOlusturDTO, string mevcutKullaniciID);
        Task EtkinlikGuncelle(EtkinlikGuncelleDTO etkinlikGuncelleDTO, string mevcutKullaniciID);
        Task EtkinlikSil(int etkinlikID, string mevcutKullaniciID);
        Task<IEnumerable<EtkinlikGetirDTO>> KullaniciEtkinlikleriGetir(string mevcutKullaniciID);
        Task<EtkinlikGetirDTO?> EtkinlikGetir(string mevcutKullaniciID, int etkinlikID);
        Task EtkinligeKullaniciEkle(EtkinlikKullaniciEkleDTO etkinlikKullaniciEkleDTO, string mevcutKullaniciID);
        Task EtkinliktenKullaniciSil(EtkinliktenKullaniciSilDTO etkinliktenKullaniciSilDTO, string mevcutKullaniciID);
        Task<IEnumerable<KullaniciGetirDTO>> EtkinlikKullanicilariGetir(int etkinlikID, string mevcutKullaniciID);

        Task<IEnumerable<EklenenEtkinlikleriGetirDTO>> EklenenEtkinlikleriGetir(string mevcutKullaniciID);
    }
}
