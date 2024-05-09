using ActivityCalender.DataAccess.Repository;
using ActivityCalender.Entities;
using Microsoft.EntityFrameworkCore;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public class EtkinlikRepository : GenericRepository<Etkinlik>, IEtkinlikRepository
    {
        private readonly ActivityCalenderContext _context;

        public EtkinlikRepository(ActivityCalenderContext context) : base(context)
        {
            _context = context;
        }


        //Yeni eklenecek etkinliğin saaat aralığının daha önce eklenen bir etkinlikle çakışıp çakışmadığı kontrol edilir. Çakışırsa true çakışmazsa false döner.
        public async Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik)
        {

            DateTime yeniBaslangicTarihi = DateTime.Parse(etkinlik.BaslangicTarihi + " " + etkinlik.BaslangicSaati);
            DateTime yeniBitisTarihi = DateTime.Parse(etkinlik.BitisTarihi + " " + etkinlik.BitisSaati);

            var mevcutEtkinlikler = await _context.Etkinliks.Where(e => e.OlusturanKullaniciId == etkinlik.OlusturanKullaniciId && e.Id != etkinlik.Id).AsNoTracking().ToListAsync();

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
