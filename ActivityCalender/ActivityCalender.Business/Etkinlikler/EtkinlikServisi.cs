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
        private readonly IKullaniciRepository _kullaniciRepository;
        private readonly IKullaniciEtkinlikRepositroy _kullaniciEtkinlikRepositroy;
        private readonly IMapper _mapper;

        public EtkinlikServisi(IEtkinlikRepository etkinlikRepository, IKullaniciRepository kullaniciRepository, IKullaniciEtkinlikRepositroy kullaniciEtkinlikRepositroy, IMapper mapper)
        {
            _etkinlikRepository = etkinlikRepository;
            _kullaniciRepository = kullaniciRepository;
            _kullaniciEtkinlikRepositroy = kullaniciEtkinlikRepositroy;
            _mapper = mapper;
        }


        public async Task EtkinlikOlustur(EtkinlikOlusturDTO etkinlikOlusturDTO, string mevcutKullaniciID)
        {
            try
            {
                if (TarihDogrula(etkinlikOlusturDTO.BaslangicTarihi, etkinlikOlusturDTO.BaslangicSaati, etkinlikOlusturDTO.BitisTarihi, etkinlikOlusturDTO.BitisSaati))
                {
                    Etkinlik? etkinlikOlustur = _mapper.Map<Etkinlik>(etkinlikOlusturDTO);
                    etkinlikOlustur.OlusturanKullaniciId = mevcutKullaniciID;

                    if (await _etkinlikRepository.EtkinlikTarihKontrol(etkinlikOlustur)) throw new Exception("Girilen Tarih Araliginda Etkinlik Kaydi Bulunmaktadir.");

                    await _etkinlikRepository.EtkinlikOlustur(etkinlikOlustur);
                }
                else
                {
                    throw new Exception();
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
                Etkinlik? etkinlikSil = await _etkinlikRepository.KullaniciEtkinlikGetir(etkinlikID, mevcutKullaniciID);

                if (etkinlikSil == null) throw new Exception("Kullanıcını Silinecek Etkinlik Kaydı Bulunamadı.");

                await _etkinlikRepository.EtkinlikSil(etkinlikSil);
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
                    Etkinlik? etkinlik = await _etkinlikRepository.KullaniciEtkinlikGetir(etkinlikGuncelleDTO.Id, mevcutKullaniciID);
                    if (etkinlik == null) throw new Exception("Guncellenecek Etkinlik Kaydi Bulunamadi.");

                    Etkinlik? etkinlikGuncelle = _mapper.Map<Etkinlik>(etkinlikGuncelleDTO);


                    etkinlikGuncelle.OlusturanKullaniciId = mevcutKullaniciID;
                    await _etkinlikRepository.EtkinlikGuncelle(etkinlikGuncelle);
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

        public async Task<EtkinlikGetirDTO?> EtkinlikGetir(string mevcutKullaniciID, int etkinlikID)
        {
            return _mapper.Map<EtkinlikGetirDTO>(await _etkinlikRepository.EtkinlikGetir(mevcutKullaniciID, etkinlikID));
        }

        public async Task<IEnumerable<EtkinlikGetirDTO>> KullaniciEtkinlikleriGetir(string mevcutKullaniciID)
        {
            return _mapper.Map<IEnumerable<EtkinlikGetirDTO>>(await _etkinlikRepository.KullaniciEtkinlikleriGetir(mevcutKullaniciID));
        }

        public async Task EtkinligeKullaniciEkle(EtkinlikKullaniciEkleDTO etkinlikKullaniciEkleDTO, string mevcutKullaniciID)
        {
            try
            {
                List<KullaniciEtkinlik> kullaniciEtkinlikListesi = new();

                if (await _etkinlikRepository.KullaniciEtkinlikGetir(etkinlikKullaniciEkleDTO.EtkinlikId, mevcutKullaniciID) != null)
                {
                    foreach (var KullaniciId in etkinlikKullaniciEkleDTO.KullaniciIds)
                    {
                        if (await _kullaniciRepository.KullaniciGetir(KullaniciId) != null)
                        {
                            KullaniciEtkinlik kullaniciEtkinlik = new()
                            {
                                KullaniciId = KullaniciId,
                                EtkinlikId = etkinlikKullaniciEkleDTO.EtkinlikId
                            };
                            kullaniciEtkinlikListesi.Add(kullaniciEtkinlik);
                        }
                    }
                    await _kullaniciEtkinlikRepositroy.KullaniciEtkinlikEkle(kullaniciEtkinlikListesi);
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

        public async Task EtkinliktenKullaniciSil(EtkinliktenKullaniciSilDTO etkinliktenKullaniciSilDTO, string mevcutKullaniciID)
        {
            try
            {
                if (await _etkinlikRepository.KullaniciEtkinlikGetir(etkinliktenKullaniciSilDTO.EtkinlikID, mevcutKullaniciID) != null)
                {
                    List<KullaniciEtkinlik> kullaniciEtkinlikListesi = new();
                    foreach (var kullaniciId in etkinliktenKullaniciSilDTO.KullaniciIDs)
                    {
                        KullaniciEtkinlik? kullaniciEtkinlik = await _kullaniciEtkinlikRepositroy.EtkinlikKullaniciGetir(etkinliktenKullaniciSilDTO.EtkinlikID, kullaniciId);
                        if (kullaniciEtkinlik != null)
                        {
                            kullaniciEtkinlikListesi.Add(kullaniciEtkinlik);
                        }
                    }
                    await _kullaniciEtkinlikRepositroy.KullaniciEtkinlikleriSil(kullaniciEtkinlikListesi);
                }
                else { throw new Exception("Kullanıcını Kayıtlı Etkinliği Bulunamadı."); }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<KullaniciGetirDTO>> EtkinlikKullanicilariGetir(int etkinlikID, string mevcutKullaniciID)
        {
            try
            {
                if (await _etkinlikRepository.KullaniciEtkinlikGetir(etkinlikID, mevcutKullaniciID) != null)
                {
                    return _mapper.Map<IEnumerable<KullaniciGetirDTO>>(await _kullaniciEtkinlikRepositroy.EtkinlikKullanicilariGetir(etkinlikID));
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
            string baslangıc = $"{baslangicTarihi} {baslangicSaati}";
            string bitis = $"{bitisTarihi} {bitisSaati}";


            if (DateTime.TryParse(baslangıc, out _) && DateTime.TryParse(bitis, out _))
            {
                if (DateTime.Parse(baslangıc) < DateTime.Parse(bitis))
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
