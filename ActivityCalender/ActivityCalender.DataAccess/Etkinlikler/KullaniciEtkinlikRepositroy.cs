using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public class KullaniciEtkinlikRepositroy : GenericRepository<KullaniciEtkinlik>, IKullaniciEtkinlikRepositroy
    {
        private readonly ActivityCalenderContext _context;
        public KullaniciEtkinlikRepositroy(ActivityCalenderContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Kullanici>> EtkinligeDavetliKullanicilariGetir(int etkinlikID)
        {
            return await _context.KullaniciEtkinliks
                .Where(e => e.EtkinlikId == etkinlikID)
                .Select(e => e.Kullanici)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Etkinlik>> EklenenEtkinlikleriGetir(string mevcutKullaniciID)
        {
            return await _context.KullaniciEtkinliks
                .Where(e => e.KullaniciId == mevcutKullaniciID)
                .Include(e => e.Etkinlik.OlusturanKullanici)
                .Select(e => e.Etkinlik)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
