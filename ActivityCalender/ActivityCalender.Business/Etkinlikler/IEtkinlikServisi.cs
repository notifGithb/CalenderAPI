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
        Task<EtkinlikGetirDTO?> KullaniciEtkinligiGetir(string mevcutKullaniciID, int etkinlikID);
        Task EtkinligeKullaniciEkle(EtkinligeKullaniciEkleDTO etkinligeKullaniciEkleDTO, string mevcutKullaniciID);
        Task EtkinliktenDavetliKullanicilariSil(EtkinliktenKullaniciSilDTO etkinliktenKullaniciSilDTO, string mevcutKullaniciID);
        Task<IEnumerable<KullaniciGetirDTO>> EtkinligeDavetliKullanicilariGetir(int etkinlikID, string mevcutKullaniciID);
        Task<IEnumerable<EklenenEtkinlikleriGetirDTO>> EklenenEtkinlikleriGetir(string mevcutKullaniciID);
    }
}
