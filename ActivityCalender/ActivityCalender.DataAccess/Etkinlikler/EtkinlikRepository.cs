using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public class EtkinlikRepository : IEtkinlikRepository
    {
        private readonly ActivityCalenderContext _context;
        public EtkinlikRepository(ActivityCalenderContext context)
        {
            _context = context;
        }


        public async Task EtkinlikOlustur(Etkinlik etkinlik)
        {
            _context.Etkinliks.Add(etkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task EtkinlikSil(Etkinlik etkinlik)
        {
            _context.Etkinliks.Remove(etkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task EtkinlikGuncelle(Etkinlik etkinlik)
        {
            _context.Etkinliks.Update(etkinlik);
            await _context.SaveChangesAsync();
        }

        public async Task<Etkinlik?> EtkinlikGetir(string kullaniciID, int etkinlikID)
        {
            return await _context.Etkinliks.AsNoTracking().Where(e => e.OlusturanKullaniciId == kullaniciID && e.Id == etkinlikID).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Etkinlik>> KullaniciEtkinlikleriGetir(string kullaniciID)
        {
            return await _context.Etkinliks.AsNoTracking().Where(e => e.OlusturanKullaniciId == kullaniciID).ToListAsync();
        }

        public async Task<Etkinlik?> KullaniciEtkinlikGetir(int etkinlikID, string kullaniciID)
        {
            return await _context.Etkinliks.AsNoTracking().Where(e => e.Id == etkinlikID && e.OlusturanKullaniciId == kullaniciID).OrderBy(e => e.BaslangicTarihi).FirstOrDefaultAsync();
        }

        public async Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik)
        {
            DateTime yeniBaslangicTarihi = DateTime.Parse(etkinlik.BaslangicTarihi + " " + etkinlik.BaslangicSaati);
            DateTime yeniBitisTarihi = DateTime.Parse(etkinlik.BitisTarihi + " " + etkinlik.BitisSaati);

            var mevcutEtkinlikler = await _context.Etkinliks.AsNoTracking().Where(e => e.OlusturanKullaniciId == etkinlik.OlusturanKullaniciId).ToListAsync();

            if (!mevcutEtkinlikler.Any()) return false;

            bool gecerli = mevcutEtkinlikler.Any(e =>
                DateTime.Parse(e.BaslangicTarihi + " " + e.BaslangicSaati) < yeniBitisTarihi &&
                yeniBaslangicTarihi < DateTime.Parse(e.BitisTarihi + " " + e.BitisSaati)
            );

            return gecerli;
        }
    }
}
