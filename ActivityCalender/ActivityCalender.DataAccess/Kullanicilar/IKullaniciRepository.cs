using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Kullanicilar
{
    public interface IKullaniciRepository
    {
        Task<Kullanici?> KullaniciGetir(string kullaniciID);
        Task<IEnumerable<Kullanici>> KullanicilariGetir();

    }
}
