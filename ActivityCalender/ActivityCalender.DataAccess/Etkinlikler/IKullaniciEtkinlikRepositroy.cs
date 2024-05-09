using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public interface IKullaniciEtkinlikRepositroy : IGenericRepository<KullaniciEtkinlik>
    {
        Task<IEnumerable<Kullanici>> EtkinligeDavetliKullanicilariGetir(int etkinlikID);
        Task<IEnumerable<Etkinlik>> EklenenEtkinlikleriGetir(string mevcutKullaniciID);

    }
}
