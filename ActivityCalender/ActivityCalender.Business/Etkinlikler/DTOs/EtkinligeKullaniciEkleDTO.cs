﻿namespace ActivityCalender.Business.Etkinlikler.DTOs
{
    public class EtkinligeKullaniciEkleDTO
    {
        public int EtkinlikId { get; set; }
        public required List<string> KullaniciIds { get; set; }

    }
}
