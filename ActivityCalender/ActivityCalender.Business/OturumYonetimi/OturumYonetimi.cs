using ActivityCalender.Business.OturumYonetimi.DTOs;
using ActivityCalender.Business.OturumYonetimi.JWT;
using ActivityCalender.Business.OturumYonetimi.JWT.Token;
using ActivityCalender.DataAccess;
using ActivityCalender.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace ActivityCalender.Business.OturumYonetimi
{
    public class OturumYonetimi : IOturumYonetimi
    {
        private readonly IMapper _mapper;
        private readonly ActivityCalenderContext _context;
        private readonly IJwtServisi _jwtServisi;

        public OturumYonetimi(IMapper mapper, ActivityCalenderContext context, IJwtServisi jwtServisi)
        {
            _mapper = mapper;
            _context = context;
            _jwtServisi = jwtServisi;
        }

        public async Task<JwtToken?> GirisYap(KullaniciGirisDto model)
        {
            try
            {
                var sha = SHA256.Create();
                var byteArray = Encoding.Default.GetBytes(model.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(sha.ComputeHash(byteArray));

                model.KullaniciSifresi = hashedSifre;
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();

                Kullanici? kullanici = await _context.Kullanicis.Where(k => k.KullaniciSifresi == model.KullaniciSifresi && k.KullaniciAdi == model.KullaniciAdi).FirstOrDefaultAsync();
                if (kullanici == null)
                {
                    return null;
                }

                JwtToken token = _jwtServisi.JwtTokenOlustur(kullanici);

                return token;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task KayitOl(KullaniciKayitDto model)
        {
            try
            {
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model), "Model boş olamaz.");
                }
                var mevcutKullanici = await _context.Kullanicis.FirstOrDefaultAsync(k => k.KullaniciAdi == model.KullaniciAdi);
                if (mevcutKullanici != null)
                {
                    throw new ArgumentException("Bu kullanıcı adı mevcut.", model.KullaniciAdi);
                }
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();
                Kullanici yeniKullanici = _mapper.Map<Kullanici>(model);

                var sha = SHA256.Create();
                var byteArray = Encoding.Default.GetBytes(yeniKullanici.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(sha.ComputeHash(byteArray));

                yeniKullanici.KullaniciSifresi = hashedSifre;
                await _context.Kullanicis.AddAsync(yeniKullanici);
                await _context.SaveChangesAsync();

            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
