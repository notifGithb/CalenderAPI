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


        public async Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik)
        {
            // Tarih ve saat formatlarını kontrol et
            if (!DateTime.TryParseExact(etkinlik.BaslangicTarihi + " " + etkinlik.BaslangicSaati, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime yeniBaslangicTarihi))
            {
                throw new Exception("Başlangıç Tarihi ve Saati Doğrulanamadı.");
            }

            if (!DateTime.TryParseExact(etkinlik.BitisTarihi + " " + etkinlik.BitisSaati, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime yeniBitisTarihi))
            {
                throw new Exception("Bitiş Tarihi ve Saati Doğrulanamadı.");
            }

            // Başlangıç tarihinin bitiş tarihinden önce olduğundan emin ol
            if (yeniBaslangicTarihi >= yeniBitisTarihi)
            {
                throw new Exception("Başlangıç Tarihi Bitiş Tarihinden Sonra ve Aynı Olamaz.");
            }

            // Mevcut etkinlikleri getir
            var mevcutEtkinlikler = await _context.Etkinliks
                .Where(e => e.OlusturanKullaniciId == etkinlik.OlusturanKullaniciId && e.Id != etkinlik.Id)
                .AsNoTracking()
                .ToListAsync();

            // Kullanıcının hiç etkinliği yoksa, yeni etkinlik eklenebilir
            if (!mevcutEtkinlikler.Any())
            {
                return true;
            }

            //Kayıt edilmek istenen etkinliğin zaman aralığında veritabanında etkinlik kaydının olup olmadığı kontrol edilir.
            bool gecerli = mevcutEtkinlikler.Any(e =>
            {
                DateTime.TryParseExact(e.BaslangicTarihi + " " + e.BaslangicSaati, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime baslangic);
                DateTime.TryParseExact(e.BitisTarihi + " " + e.BitisSaati, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime bitis);

                return
                (yeniBaslangicTarihi <= baslangic && (bitis <= yeniBitisTarihi || yeniBitisTarihi < bitis) && baslangic <= yeniBitisTarihi) ||
                (baslangic <= yeniBaslangicTarihi && (bitis < yeniBitisTarihi || yeniBitisTarihi <= bitis) && yeniBaslangicTarihi <= bitis);

            });

            return !gecerli;
        }
    }
}
