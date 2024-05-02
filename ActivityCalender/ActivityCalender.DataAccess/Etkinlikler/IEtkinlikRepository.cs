﻿using ActivityCalender.Entities;

namespace ActivityCalender.DataAccess.Etkinlikler
{
    public interface IEtkinlikRepository
    {
        Task EtkinlikOlustur(Etkinlik etkinlik);
        Task EtkinlikGuncelle(Etkinlik etkinlik);
        Task EtkinlikSil(Etkinlik etkinlik);
        Task<IEnumerable<Etkinlik>> KullaniciEtkinlikleriGetir(string kullaniciID);
        Task<Etkinlik?> EtkinlikGetir(string kullaniciID, int etkinlikID);
        Task<Etkinlik?> KullaniciEtkinlikGetir(int etkinlikID, string kullaniciID);
        Task<bool> EtkinlikTarihKontrol(Etkinlik etkinlik);

    }
}