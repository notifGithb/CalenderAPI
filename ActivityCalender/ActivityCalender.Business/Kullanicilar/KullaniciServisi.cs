using ActivityCalender.Business.Kullanicilar.DTOs;
using ActivityCalender.DataAccess.Kullanicilar;
using AutoMapper;

namespace ActivityCalender.Business.Kullanicilar
{
    public class KullaniciServisi : IKullaniciServisi
    {
        private readonly IMapper _mapper;
        private readonly IKullaniciRepository _kullaniciRepository;

        public KullaniciServisi(IMapper mapper, IKullaniciRepository kullaniciRepository)
        {
            _mapper = mapper;
            _kullaniciRepository = kullaniciRepository;
        }

        public async Task<KullaniciGetirDTO?> KullaniciGetir(string kullaniciID)
        {
            return _mapper.Map<KullaniciGetirDTO>(await _kullaniciRepository.GetWhereAsync(k => k.Id == kullaniciID));
        }

        public async Task<IEnumerable<KullaniciGetirDTO>> KullanicilariGetir(string kullaniciID)
        {
            return _mapper.Map<IEnumerable<KullaniciGetirDTO>>(await _kullaniciRepository.GetAllWhereAsync(k => k.Id != kullaniciID));
        }
    }
}
