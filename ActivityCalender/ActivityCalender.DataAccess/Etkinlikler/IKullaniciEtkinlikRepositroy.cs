using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public interface IKullaniciEtkinlikRepositroy
    {
        Task KullaniciEtkinlikEkle(List<KullaniciEtkinlik> kullaniciEtkinliks);
        Task KullaniciEtkinlikGuncelle(KullaniciEtkinlik kullaniciEtkinlik);
        Task KullaniciEtkinlikleriSil(List<KullaniciEtkinlik> kullaniciEtkinliks);
        Task<KullaniciEtkinlik?> EtkinlikKullaniciGetir(int etkinlikID, string kullaniciID);
        Task<IEnumerable<Kullanici>> EtkinlikKullanicilariGetir(int etkinlikID);
        Task<IEnumerable<Etkinlik>> EklenenEtkinlikleriGetir(string mevcutKullaniciID);

    }
}
