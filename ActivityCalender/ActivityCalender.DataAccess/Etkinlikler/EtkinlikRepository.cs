using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public class EtkinlikRepository : GenericRepository<Etkinlik>, IEtkinlikRepository
    {
        private readonly ActivityCalenderContext _context;

        public EtkinlikRepository(ActivityCalenderContext context) : base(context)
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


        public async Task<IEnumerable<Etkinlik>> KullaniciEtkinlikleriGetir(string kullaniciID)
        {
            return await _context.Etkinliks
                .AsNoTracking()
                .Where(e => e.OlusturanKullaniciId == kullaniciID)
                .ToListAsync();
        }

        public async Task<Etkinlik?> KullaniciEtkinligiGetir(int etkinlikID, string kullaniciID)
        {
            return await _context.Etkinliks
                .AsNoTracking()
                .Where(e => e.Id == etkinlikID && e.OlusturanKullaniciId == kullaniciID)
                .FirstOrDefaultAsync();
        }

        //Yeni eklenecek etkinliğin saaat aralığının daha önce eklenen bir etkinlikle çakışıp çakışmadığı kontrol edilir. Çakışırsa true çakışmazsa false döner.
        public async Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik)
        {

            DateTime yeniBaslangicTarihi = DateTime.Parse(etkinlik.BaslangicTarihi + " " + etkinlik.BaslangicSaati);
            DateTime yeniBitisTarihi = DateTime.Parse(etkinlik.BitisTarihi + " " + etkinlik.BitisSaati);

            var mevcutEtkinlikler = await _context.Etkinliks.AsNoTracking().Where(e => e.OlusturanKullaniciId == etkinlik.OlusturanKullaniciId).ToListAsync();

            //Kullanıcının hiç etkinlik oluşturmadığı durum.
            if (!mevcutEtkinlikler.Any()) return false;

            //Veritabanına kayıtlı etkinlikler arasında yeni eklenecek etkinlikle aynı zaman dilimine ait etkinlik olup olmadığı kontrol edilirx.
            bool gecerli = mevcutEtkinlikler.Any(e =>
                DateTime.Parse(e.BaslangicTarihi + " " + e.BaslangicSaati) < yeniBitisTarihi &&
                yeniBaslangicTarihi < DateTime.Parse(e.BitisTarihi + " " + e.BitisSaati)
            );

            return gecerli;
        }
    }
}
