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
                .OrderBy(e => DateTime.Parse(e.BaslangicTarihi))
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

            DateTime yeniBaslangicTarihi = DateTime.ParseExact(etkinlik.BaslangicTarihi + " " + etkinlik.BaslangicSaati, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            DateTime yeniBitisTarihi = DateTime.ParseExact(etkinlik.BitisTarihi + " " + etkinlik.BitisSaati, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var mevcutEtkinlikler = await _context.Etkinliks.AsNoTracking().Where(e => e.OlusturanKullaniciId == etkinlik.OlusturanKullaniciId).ToListAsync();

            //Kullanıcının hiç etkinlik oluşturmadığı durum.
            if (!mevcutEtkinlikler.Any()) return false;

            //Veritabanına kayıtlı etkinlikler arasında yeni eklenecek etkinlikle aynı zaman dilimine ait etkinlik olup olmadığı kontrol edilirx.
            bool gecerli = mevcutEtkinlikler.Any(e =>
                DateTime.ParseExact(e.BaslangicTarihi + " " + e.BaslangicSaati, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture) < yeniBitisTarihi &&
                yeniBaslangicTarihi < DateTime.ParseExact(e.BitisTarihi + " " + e.BitisSaati, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture)
            );

            return gecerli;
        }
    }
}
