using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public interface IEtkinlikRepository : IGenericRepository<Etkinlik>
    {
        Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik);
    }
}
