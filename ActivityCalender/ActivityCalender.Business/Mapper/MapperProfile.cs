using ActivityCalender.Business.Etkinlikler.DTOs;
using ActivityCalender.Business.Kullanicilar.DTOs;
using ActivityCalender.Business.OturumYonetimi.DTOs;
using ActivityCalender.Entities;
using AutoMapper;

namespace ActivityCalender.Business.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Kullanici, KullaniciGirisDto>().ReverseMap();
            CreateMap<Kullanici, KullaniciKayitDto>().ReverseMap();
            CreateMap<Kullanici, KullaniciGetirDTO>().ReverseMap();

            CreateMap<Etkinlik, EtkinlikOlusturDTO>().ReverseMap();
            CreateMap<Etkinlik, EtkinlikGuncelleDTO>().ReverseMap();
            CreateMap<Etkinlik, EtkinlikGetirDTO>().ReverseMap();
            CreateMap<Etkinlik, EklenenEtkinlikleriGetirDTO>()
                .ForMember(dest => dest.EkleyenKullaniciId, opt => opt.MapFrom(src => src.OlusturanKullaniciId))
                .ForMember(dest => dest.EkleyenKullaniciAdi, opt => opt.MapFrom(src => src.OlusturanKullanici.KullaniciAdi));
        }
    }
}
