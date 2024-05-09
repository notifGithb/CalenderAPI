using ActivityCalender.Business.Etkinlikler.DTOs;
using ActivityCalender.Business.Kullanicilar.DTOs;
using ActivityCalender.DataAccess.Etkinlikler;
using ActivityCalender.DataAccess.Kullanicilar;
using ActivityCalender.Entities;
using AutoMapper;

namespace ActivityCalender.Business.Etkinlikler
{
    public class EtkinlikServisi : IEtkinlikServisi
    {
        private readonly IEtkinlikRepository _etkinlikRepository;
        private readonly IKullaniciEtkinlikRepositroy _kullaniciEtkinlikRepositroy;
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IMapper _mapper;


        public EtkinlikServisi(
            IEtkinlikRepository etkinlikRepository,
            IKullaniciEtkinlikRepositroy kullaniciEtkinlikRepositroy,
            IKullaniciRepository kullananiciRepository,
            IMapper mapper)
        {
            _etkinlikRepository = etkinlikRepository;
            _kullaniciEtkinlikRepositroy = kullaniciEtkinlikRepositroy;
            _kullaniciRepository = kullananiciRepository;
            _mapper = mapper;
        }


        public async Task EtkinlikOlustur(EtkinlikOlusturDTO etkinlikOlusturDTO, string mevcutKullaniciID)
        {
            try
            {
                Etkinlik etkinlikOlustur = _mapper.Map<Etkinlik>(etkinlikOlusturDTO);
                etkinlikOlustur.OlusturanKullaniciId = mevcutKullaniciID;

                if (await _etkinlikRepository.EtkinlikTarihKontrol(etkinlikOlustur))
                {
                    await _etkinlikRepository.AddAsync(etkinlikOlustur);
                }
                else
                {
                    throw new Exception("Girilen Tarih Araliginda Etkinlik Kaydi Bulunmaktadir.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EtkinlikSil(int etkinlikID, string mevcutKullaniciID)
        {
            try
            {
                Etkinlik? etkinlikSil = await _etkinlikRepository.GetWhereAsync(e => e.Id == etkinlikID && e.OlusturanKullaniciId == mevcutKullaniciID) ?? throw new Exception("Kullanıcını Silinecek Etkinlik Kaydı Bulunamadı.");

                await _etkinlikRepository.Remove(etkinlikSil);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EtkinlikGuncelle(EtkinlikGuncelleDTO etkinlikGuncelleDTO, string mevcutKullaniciID)
        {
            try
            {
                Etkinlik etkinlikGuncelle = _mapper.Map<Etkinlik>(etkinlikGuncelleDTO);
                etkinlikGuncelle.OlusturanKullaniciId = mevcutKullaniciID;

                if (!await _etkinlikRepository.AnyAsync(e => e.Id == etkinlikGuncelleDTO.Id && e.OlusturanKullaniciId == mevcutKullaniciID)) throw new Exception("Guncellenecek Etkinlik Kaydi Bulunamadi.");

                if (await _etkinlikRepository.EtkinlikTarihKontrol(etkinlikGuncelle))
                {
                    await _etkinlikRepository.Update(etkinlikGuncelle);
                }
                else
                {
                    throw new Exception("Girilen Tarih Araliginda Etkinlik Kaydi Bulunmaktadir.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<EtkinlikGetirDTO?> KullaniciEtkinligiGetir(string mevcutKullaniciID, int etkinlikID)
        {
            return _mapper.Map<EtkinlikGetirDTO>(await _etkinlikRepository.GetWhereAsync(e => e.OlusturanKullaniciId == mevcutKullaniciID && e.Id == etkinlikID));
        }

        public async Task<IEnumerable<EtkinlikGetirDTO>> KullaniciEtkinlikleriGetir(string mevcutKullaniciID)
        {
            return _mapper.Map<IEnumerable<EtkinlikGetirDTO>>(await _etkinlikRepository.GetAllWhereAsync(e => e.OlusturanKullaniciId == mevcutKullaniciID));
        }

        public async Task EtkinligeKullaniciEkle(EtkinligeKullaniciEkleDTO etkinligeKullaniciEkleDTO, string mevcutKullaniciID)
        {
            try
            {
                if (await _etkinlikRepository.AnyAsync(e => e.OlusturanKullaniciId == mevcutKullaniciID && e.Id == etkinligeKullaniciEkleDTO.EtkinlikId))
                {
                    List<KullaniciEtkinlik> kullaniciEtkinlikListesi = new();

                    foreach (var kullaniciId in etkinligeKullaniciEkleDTO.KullaniciIds)
                    {

                        if (await _kullaniciRepository.GetWhereAsync(k => k.Id == kullaniciId) != null)
                        {
                            KullaniciEtkinlik kullaniciEtkinlik = new()
                            {
                                KullaniciId = kullaniciId,
                                EtkinlikId = etkinligeKullaniciEkleDTO.EtkinlikId
                            };
                            kullaniciEtkinlikListesi.Add(kullaniciEtkinlik);
                        }
                    }
                    await _kullaniciEtkinlikRepositroy.AddRangeAsync(kullaniciEtkinlikListesi);
                }
                else
                {
                    throw new Exception("Kullanıcını Kayıtlı Etkinliği Bulunamadı.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task EtkinliktenDavetliKullanicilariSil(EtkinliktenKullaniciSilDTO etkinliktenKullaniciSilDTO, string mevcutKullaniciID)
        {
            try
            {
                if (await _etkinlikRepository.AnyAsync(e => e.Id == etkinliktenKullaniciSilDTO.EtkinlikID && e.OlusturanKullaniciId == mevcutKullaniciID))
                {
                    List<KullaniciEtkinlik> kullaniciEtkinlikListesi = new();

                    foreach (var kullaniciId in etkinliktenKullaniciSilDTO.KullaniciIDs)
                    {

                        KullaniciEtkinlik? kullaniciEtkinlik = await _kullaniciEtkinlikRepositroy.GetWhereAsync(e => e.EtkinlikId == etkinliktenKullaniciSilDTO.EtkinlikID && e.KullaniciId == kullaniciId);
                        if (kullaniciEtkinlik != null)
                        {
                            kullaniciEtkinlikListesi.Add(kullaniciEtkinlik);
                        }
                    }

                    await _kullaniciEtkinlikRepositroy.RemoveRange(kullaniciEtkinlikListesi);
                }
                else { throw new Exception("Kullanıcını Kayıtlı Etkinliği Bulunamadı."); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<KullaniciGetirDTO>> EtkinligeDavetliKullanicilariGetir(int etkinlikID, string mevcutKullaniciID)
        {
            try
            {
                if (await _etkinlikRepository.AnyAsync(e => e.Id == etkinlikID && e.OlusturanKullaniciId == mevcutKullaniciID))
                {
                    return _mapper.Map<IEnumerable<KullaniciGetirDTO>>(await _kullaniciEtkinlikRepositroy.EtkinligeDavetliKullanicilariGetir(etkinlikID));
                }
                else
                {
                    throw new Exception("Kullanıcını Kayıtlı Etkinliği Bulunamadı.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<EklenenEtkinlikleriGetirDTO>> EklenenEtkinlikleriGetir(string mevcutKullaniciID)
        {
            return _mapper.Map<IEnumerable<EklenenEtkinlikleriGetirDTO>>(await _kullaniciEtkinlikRepositroy.EklenenEtkinlikleriGetir(mevcutKullaniciID));
        }






        //private static bool EtkinlikTarihiDogrula(Etkinlik etkinlik)
        //{
        //    string baslangic = $"{etkinlik.BaslangicTarihi} {etkinlik.BaslangicSaati}";
        //    string bitis = $"{etkinlik.BitisTarihi} {etkinlik.BitisSaati}";

        //    if (DateTime.TryParse(baslangic, out _) && DateTime.TryParse(bitis, out _))
        //    {
        //        if (DateTime.Parse(baslangic) < DateTime.Parse(bitis))
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
    }
}
