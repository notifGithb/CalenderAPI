using ActivityCalender.Business.Kullanicilar.DTOs;
using ActivityCalender.DataAccess.Kullanicilar;
using AutoMapper;

namespace ActivityCalender.Business.Kullanicilar
{
    public class KullaniciServisi : IKullaniciServisi
    {
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IMapper _mapper;

        public KullaniciServisi(IKullaniciRepository kullaniciRepository, IMapper mapper)
        {
            _kullaniciRepository = kullaniciRepository;
            _mapper = mapper;

        }

        public async Task<KullaniciGetirDTO?> KullaniciGetir(string kullaniciID)
        {
            return _mapper.Map<KullaniciGetirDTO>(await _kullaniciRepository.KullaniciGetir(kullaniciID));
        }

        public async Task<IEnumerable<KullaniciGetirDTO>> KullanicilariGetir(string kullaniciID)
        {
            return _mapper.Map<IEnumerable<KullaniciGetirDTO>>(await _kullaniciRepository.KullanicilariGetir(kullaniciID));
        }
    }
}
