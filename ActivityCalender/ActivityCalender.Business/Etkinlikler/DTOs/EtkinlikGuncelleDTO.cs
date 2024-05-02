﻿using ActivityCalender.Entities;
using System.ComponentModel;

namespace ActivityCalender.Business.Etkinlikler.DTOs
{
    public class EtkinlikGuncelleDTO
    {
        public int Id { get; set; }
        public required string Baslik { get; set; }
        public string? Aciklama { get; set; }

        [DefaultValue("00:00")]
        public required string BaslangicSaati { get; set; }

        [DefaultValue("00:00")]
        public required string BitisSaati { get; set; }

        [DefaultValue("01/01/0001")]
        public required string BaslangicTarihi { get; set; }

        [DefaultValue("01/01/0001")]
        public required string BitisTarihi { get; set; }
        public TekrarEnum TekrarDurumu { get; set; }

    }
}
