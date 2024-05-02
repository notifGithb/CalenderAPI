using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalender.DataAccess.Kullanicilar
{
    public class KullaniciRepository : IKullaniciRepository
    {
        private readonly ActivityCalenderContext _context;
        public KullaniciRepository(ActivityCalenderContext context)
        {
            _context = context;
        }

        public async Task<Kullanici?> KullaniciGetir(string kullaniciID)
        {
            return await _context.Kullanicis.FindAsync(kullaniciID);
        }

        public async Task<IEnumerable<Kullanici>> KullanicilariGetir()
        {
            return await _context.Kullanicis.ToListAsync();
        }
    }
}
