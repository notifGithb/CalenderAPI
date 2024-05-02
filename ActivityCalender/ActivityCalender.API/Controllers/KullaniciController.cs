using ActivityCalender.Business.Kullanicilar;
using ActivityCalender.Business.Kullanicilar.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActivityCalender.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class KullaniciController : ControllerBase
    {
        private readonly IKullaniciServisi _kullaniciServisi;

        public KullaniciController(IKullaniciServisi kullaniciServisi)
        {
            _kullaniciServisi = kullaniciServisi;
        }

        [HttpGet]
        public async Task<IActionResult> MevcutKullaniciGetir()
        {
            string? mevcutKullaniciID = User.Identity?.Name;

            if (mevcutKullaniciID == null) return Unauthorized();

            KullaniciGetirDTO? kullanici = await _kullaniciServisi.KullaniciGetir(mevcutKullaniciID);

            if (kullanici != null)
            {
                return Ok(kullanici);
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> KullanicilariGetir()
        {
            string? mevcutKullaniciID = User.Identity?.Name;

            if (mevcutKullaniciID == null) return Unauthorized();

            IEnumerable<KullaniciGetirDTO>? kullanicilar = await _kullaniciServisi.KullanicilariGetir(mevcutKullaniciID);

            if (kullanicilar.Any())
            {
                return Ok(kullanicilar);
            }
            return NotFound();
        }
    }
}
