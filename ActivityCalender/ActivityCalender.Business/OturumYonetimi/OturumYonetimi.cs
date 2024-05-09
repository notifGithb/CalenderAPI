using ActivityCalender.Business.OturumYonetimi.DTOs;
using ActivityCalender.Business.OturumYonetimi.JWT;
using ActivityCalender.Business.OturumYonetimi.JWT.Token;
using ActivityCalender.DataAccess.Kullanicilar;
using ActivityCalender.Entities;
using AutoMapper;
using System.Security.Cryptography;
using System.Text;

namespace ActivityCalender.Business.OturumYonetimi
{
    public class OturumYonetimi : IOturumYonetimi
    {
        private readonly IMapper _mapper;
        private readonly IJwtServisi _jwtServisi;
        private readonly IKullaniciRepository _kullaniciRepository;

        public OturumYonetimi(IMapper mapper, IJwtServisi jwtServisi, IKullaniciRepository kullaniciRepository)
        {
            _mapper = mapper;
            _jwtServisi = jwtServisi;
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<JwtToken?> GirisYap(KullaniciGirisDto model)
        {
            try
            {
                var byteArray = Encoding.Default.GetBytes(model.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                model.KullaniciSifresi = hashedSifre;
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();

                Kullanici? kullanici = await _kullaniciRepository.GetWhereAsync(k => k.KullaniciAdi == model.KullaniciAdi && k.KullaniciSifresi == model.KullaniciSifresi);
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

                var mevcutKullanici = await _kullaniciRepository.GetWhereAsync(k => k.KullaniciAdi == model.KullaniciAdi);

                if (mevcutKullanici != null)
                {
                    throw new ArgumentException("Bu kullanıcı adı mevcut.", model.KullaniciAdi);
                }
                model.KullaniciAdi = model.KullaniciAdi.Trim().ToLower();
                Kullanici yeniKullanici = _mapper.Map<Kullanici>(model);

                var byteArray = Encoding.Default.GetBytes(yeniKullanici.KullaniciSifresi);
                var hashedSifre = Convert.ToBase64String(SHA256.HashData(byteArray));

                yeniKullanici.KullaniciSifresi = hashedSifre;

                await _kullaniciRepository.AddAsync(yeniKullanici);

            }
            catch (ArgumentNullException)
            {
                throw;
            }
        }
    }
}
