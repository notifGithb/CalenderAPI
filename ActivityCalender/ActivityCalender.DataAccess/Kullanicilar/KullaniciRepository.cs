using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Kullanicilar
{
    public class KullaniciRepository : GenericRepository<Kullanici>, IKullaniciRepository
    {
        public KullaniciRepository(ActivityCalenderContext context) : base(context)
        {
        }
    }
}
