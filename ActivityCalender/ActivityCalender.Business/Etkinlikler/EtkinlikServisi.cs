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
                if (TarihDogrula(etkinlikOlusturDTO.BaslangicTarihi, etkinlikOlusturDTO.BaslangicSaati, etkinlikOlusturDTO.BitisTarihi, etkinlikOlusturDTO.BitisSaati))
                {
                    Etkinlik etkinlikOlustur = _mapper.Map<Etkinlik>(etkinlikOlusturDTO);
                    etkinlikOlustur.OlusturanKullaniciId = mevcutKullaniciID;

                    if (await _etkinlikRepository.EtkinlikTarihKontrol(etkinlikOlustur)) throw new Exception("Girilen Tarih Araliginda Etkinlik Kaydi Bulunmaktadir.");

                    //await _etkinlikRepository.EtkinlikOlustur(etkinlikOlustur);
                    await _etkinlikRepository.AddAsync(etkinlikOlustur);
                }
                else
                {
                    throw new Exception("Tarih ve Zaman Bilgileri Doğrulanamadı.");
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

                //await _etkinlikRepository.EtkinlikSil(etkinlikSil);
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
                if (TarihDogrula(etkinlikGuncelleDTO.BaslangicTarihi, etkinlikGuncelleDTO.BaslangicSaati, etkinlikGuncelleDTO.BitisTarihi, etkinlikGuncelleDTO.BitisSaati))
                {
                    Etkinlik? etkinlik = await _etkinlikRepository.GetWhereAsync(e => e.Id == etkinlikGuncelleDTO.Id && e.OlusturanKullaniciId == mevcutKullaniciID) ?? throw new Exception("Guncellenecek Etkinlik Kaydi Bulunamadi.");

                    Etkinlik etkinlikGuncelle = _mapper.Map<Etkinlik>(etkinlikGuncelleDTO);
                    etkinlikGuncelle.OlusturanKullaniciId = mevcutKullaniciID;

                    if (await _etkinlikRepository.EtkinlikTarihKontrol(etkinlikGuncelle)) throw new Exception("Girilen Tarih Araliginda Etkinlik Kaydi Bulunmaktadir.");

                    await _etkinlikRepository.Update(etkinlikGuncelle);
                }
                else
                {
                    throw new Exception("Tarih ve Saat Format Hatası.");
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

        public async Task EtkinligeKullaniciEkle(EtkinligeKullaniciEkleDTO etkinlikKullaniciEkleDTO, string mevcutKullaniciID)
        {
            try
            {
                List<KullaniciEtkinlik> kullaniciEtkinlikListesi = new();

                if (await _etkinlikRepository.GetWhereAsync(e => e.OlusturanKullaniciId == mevcutKullaniciID && e.Id == etkinlikKullaniciEkleDTO.EtkinlikId) != null)
                {
                    foreach (var kullaniciId in etkinlikKullaniciEkleDTO.KullaniciIds)
                    {

                        if (await _kullaniciRepository.GetWhereAsync(k => k.Id == kullaniciId) != null)
                        {
                            KullaniciEtkinlik kullaniciEtkinlik = new()
                            {
                                KullaniciId = kullaniciId,
                                EtkinlikId = etkinlikKullaniciEkleDTO.EtkinlikId
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
                if (await _etkinlikRepository.GetAllWhereAsync(e => e.Id == etkinliktenKullaniciSilDTO.EtkinlikID && e.OlusturanKullaniciId == mevcutKullaniciID) != null)
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
                if (await _etkinlikRepository.GetAllWhereAsync(e => e.Id == etkinlikID && e.OlusturanKullaniciId == mevcutKullaniciID) != null)
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

        private static bool TarihDogrula(string baslangicTarihi, string baslangicSaati, string bitisTarihi, string bitisSaati)
        {
            string baslangic = $"{baslangicTarihi} {baslangicSaati}";
            string bitis = $"{bitisTarihi} {bitisSaati}";

            if (DateTime.TryParse(baslangic, out _) && DateTime.TryParse(bitis, out _))
            {
                if (DateTime.Parse(baslangic) < DateTime.Parse(bitis))
                {
                    return true;
                }
                return false;
            }
            else
            {
                return false;
            }
        }
    }
}
