using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public class KullaniciEtkinlikRepositroy : IKullaniciEtkinlikRepositroy
    {
        private readonly ActivityCalenderContext _context;
        public KullaniciEtkinlikRepositroy(ActivityCalenderContext context)
        {
            _context = context;
        }
        public async Task KullaniciEtkinlikEkle(List<KullaniciEtkinlik> kullaniciEtkinliks)
        {
            await _context.KullaniciEtkinliks.AddRangeAsync(kullaniciEtkinliks);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<KullaniciEtkinlik>> KullaniciEtkinlikleriGetir(int etkinlikID)
        {
            return await _context.KullaniciEtkinliks.AsNoTracking().Include(e => e.Kullanici).Where(e => e.EtkinlikId == etkinlikID).ToListAsync();
        }

        public async Task KullaniciEtkinlikGuncelle(KullaniciEtkinlik kullaniciEtkinlik)
        {
            _context.KullaniciEtkinliks.Update(kullaniciEtkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task EtkinliktenDavetliKullanicilariSil(List<KullaniciEtkinlik> kullaniciEtkinliks)
        {
            _context.KullaniciEtkinliks.RemoveRange(kullaniciEtkinliks);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Kullanici>> EtkinligeDavetliKullanicilariGetir(int etkinlikID)
        {
            return await _context.KullaniciEtkinliks
                .Where(e => e.EtkinlikId == etkinlikID)
                .Select(e => e.Kullanici)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<KullaniciEtkinlik?> EtkinlikKullaniciGetir(int etkinlikID, string kullaniciID)
        {
            return await _context.KullaniciEtkinliks
                .Where(e => e.EtkinlikId == etkinlikID && e.KullaniciId == kullaniciID)
                .AsNoTracking()
                .FirstOrDefaultAsync();
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
