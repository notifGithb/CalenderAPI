using ActivityCalender.Business.Etkinlikler;
using ActivityCalender.Business.Etkinlikler.DTOs;
using ActivityCalender.Business.Kullanicilar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityCalender.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class EtkinlikController : ControllerBase
    {
        private readonly IEtkinlikServisi _etkinlikServisi;

        public EtkinlikController(IEtkinlikServisi etkinlikServisi)
        {
            _etkinlikServisi = etkinlikServisi;
        }


        [HttpPost]
        public async Task<IActionResult> EtkinlikOlustur([FromBody] EtkinlikOlusturDTO etkinlikOlusturDTO)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            if (etkinlikOlusturDTO == null) return BadRequest();

            await _etkinlikServisi.EtkinlikOlustur(etkinlikOlusturDTO, mevcutKullaniciID);
            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> EtkinligeKullaniciEkle([FromBody] EtkinlikKullaniciEkleDTO etkinlikKullaniciEkleDTO)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            if (etkinlikKullaniciEkleDTO == null) return BadRequest();

            await _etkinlikServisi.EtkinligeKullaniciEkle(etkinlikKullaniciEkleDTO, mevcutKullaniciID);
            return Ok();
        }


        [HttpDelete]
        public async Task<IActionResult> EtkinliktenKullaniciSil([FromBody] EtkinliktenKullaniciSilDTO etkinliktenKullaniciSilDTO)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            if (etkinliktenKullaniciSilDTO == null) return BadRequest();

            await _etkinlikServisi.EtkinliktenKullaniciSil(etkinliktenKullaniciSilDTO, mevcutKullaniciID);
            return Ok();
        }


        [HttpGet("{etkinlikID}")]
        public async Task<IActionResult> EtkinlikKullanicilariGetir(int etkinlikID)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            IEnumerable<KullaniciGetirDTO> kullanicilar = await _etkinlikServisi.EtkinlikKullanicilariGetir(etkinlikID, mevcutKullaniciID);
            if (kullanicilar == null) return NotFound();

            return Ok(kullanicilar);
        }


        [HttpDelete("{etkinlikID}")]
        public async Task<IActionResult> EtkinlikSil(int etkinlikID)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            await _etkinlikServisi.EtkinlikSil(etkinlikID, mevcutKullaniciID);
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> EtkinlikGuncelle([FromBody] EtkinlikGuncelleDTO etkinlikGuncelleDTO)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            if (etkinlikGuncelleDTO == null) return BadRequest();

            await _etkinlikServisi.EtkinlikGuncelle(etkinlikGuncelleDTO, mevcutKullaniciID);

            return Ok();
        }


        [HttpGet("{etkinlikID}")]
        public async Task<IActionResult> EtkinlikGetir(int etkinlikID)
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            EtkinlikGetirDTO? etkinlik = await _etkinlikServisi.EtkinlikGetir(mevcutKullaniciID, etkinlikID);

            if (etkinlik == null) return NotFound();

            return Ok(etkinlik);
        }


        [HttpGet]
        public async Task<IActionResult> KullaniciEtkinlikleriGetir()
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            IEnumerable<EtkinlikGetirDTO> etkinlikler = await _etkinlikServisi.KullaniciEtkinlikleriGetir(mevcutKullaniciID);

            if (!etkinlikler.Any()) return NotFound();

            return Ok(etkinlikler);
        }


        [HttpGet]
        public async Task<IActionResult> EklenenEtkinlikleriGetir()
        {
            string? mevcutKullaniciID = User.Identity?.Name;
            if (mevcutKullaniciID == null) return Unauthorized();

            IEnumerable<EklenenEtkinlikleriGetirDTO> etkinlikler = await _etkinlikServisi.EklenenEtkinlikleriGetir(mevcutKullaniciID);

            if (!etkinlikler.Any()) return NotFound();

            return Ok(etkinlikler);
        }
    }
}
