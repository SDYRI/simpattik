﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Services.Middleware.Models
{
    public class enumDataModel
    {
        public string Text { get; set; }
        public string Value { get; set; }

        public List<enumDataModel> TipePaket()
        {
            List<enumDataModel> tipepaket = new List<enumDataModel>();
            tipepaket.Add(new enumDataModel { Text = "Penyedia", Value = "1" });
            tipepaket.Add(new enumDataModel { Text = "Swakelola", Value = "2" });
            return tipepaket;
        }
        public List<enumDataModel> SumberDana()
        {
            List<enumDataModel> sumberdana = new List<enumDataModel>();
            sumberdana.Add(new enumDataModel { Text = "APBN", Value = "APBN" });
            sumberdana.Add(new enumDataModel { Text = "APBD", Value = "APBD" });
            return sumberdana;
        }
        public List<enumDataModel> YaTidak()
        {
            List<enumDataModel> yatidak = new List<enumDataModel>();
            yatidak.Add(new enumDataModel { Text = "Ya", Value = "1" });
            yatidak.Add(new enumDataModel { Text = "Tidak", Value = "0" });
            return yatidak;
        }
        public List<enumDataModel> BanyakTerbatas()
        {
            List<enumDataModel> banyakterbatas = new List<enumDataModel>();
            banyakterbatas.Add(new enumDataModel { Text = "Banyak", Value = "1" });
            banyakterbatas.Add(new enumDataModel { Text = "Terbatas", Value = "2" });
            return banyakterbatas;
        }
        public List<enumDataModel> Pengoperasian()
        {
            List<enumDataModel> pengoperasian = new List<enumDataModel>();
            pengoperasian.Add(new enumDataModel { Text = "Otomatis", Value = "1" });
            pengoperasian.Add(new enumDataModel { Text = "Manual", Value = "2" });
            return pengoperasian;
        }
        public List<enumDataModel> Rekomendasi()
        {
            List<enumDataModel> rekomendasi = new List<enumDataModel>();
            rekomendasi.Add(new enumDataModel { Text = "Direkomendasikan", Value = "1" });
            rekomendasi.Add(new enumDataModel { Text = "Tidak Direkomendasikan", Value = "2" });
            return rekomendasi;
        }
        public List<enumDataModel> BarangMaterial()
        {
            List<enumDataModel> barangmaterial = new List<enumDataModel>();
            barangmaterial.Add(new enumDataModel { Text = "Dalam Negeri", Value = "Dalam Negeri" });
            barangmaterial.Add(new enumDataModel { Text = "Luar Negeri", Value = "Luar Negeri" });
            return barangmaterial;
        }
        public List<enumDataModel> SudahBelum()
        {
            List<enumDataModel> sudahbelum = new List<enumDataModel>();
            sudahbelum.Add(new enumDataModel { Text = "Sudah dilakukan", Value = "1" });
            sudahbelum.Add(new enumDataModel { Text = "Belum dilakukan", Value = "2" });
            return sudahbelum;
        }
        public List<enumDataModel> KompleksSederhana()
        {
            List<enumDataModel> komplekssederhana = new List<enumDataModel>();
            komplekssederhana.Add(new enumDataModel { Text = "Kompleks", Value = "1" });
            komplekssederhana.Add(new enumDataModel { Text = "Sederhana", Value = "2" });
            return komplekssederhana;
        }
        public List<enumDataModel> Revisi()
        {
            List<enumDataModel> komplekssederhana = new List<enumDataModel>();
            komplekssederhana.Add(new enumDataModel { Text = "Revisi", Value = "2" });
            komplekssederhana.Add(new enumDataModel { Text = "Done", Value = "4" });
            return komplekssederhana;
        }
        public List<enumDataModel> Kelayakan()
        {
            List<enumDataModel> tipeuser = new List<enumDataModel>();
            tipeuser.Add(new enumDataModel { Text = "Layak pakai", Value = "Layak pakai" });
            tipeuser.Add(new enumDataModel { Text = "Rusak/ dalam perbaikan", Value = "Rusak/ dalam perbaikan" });
            tipeuser.Add(new enumDataModel { Text = "Tidak dapat digunakan", Value = "Tidak dapat digunakan" });
            return tipeuser;
        }
        public List<enumDataModel> Status()
        {
            List<enumDataModel> status = new List<enumDataModel>();
            status.Add(new enumDataModel { Text = "Draft", Value = "1" });
            status.Add(new enumDataModel { Text = "Revisi", Value = "2" });
            status.Add(new enumDataModel { Text = "Review", Value = "3" });
            status.Add(new enumDataModel { Text = "Done", Value = "4" });
            status.Add(new enumDataModel { Text = "Done", Value = "5" });
            return status;
        }
        public List<enumDataModel> PaPpk()
        {
            List<enumDataModel> pappk = new List<enumDataModel>();
            pappk.Add(new enumDataModel { Text = "PA", Value = "1" });
            pappk.Add(new enumDataModel { Text = "KPA", Value = "2" });
            pappk.Add(new enumDataModel { Text = "PPK", Value = "3" });
            return pappk;
        }
        public List<enumDataModel> Prioritas()
        {
            List<enumDataModel> prioritas = new List<enumDataModel>();
            prioritas.Add(new enumDataModel { Text = "Tinggi", Value = "1" });
            prioritas.Add(new enumDataModel { Text = "Sedang", Value = "2" });
            prioritas.Add(new enumDataModel { Text = "Kecil", Value = "3" });
            return prioritas;
        }
        public List<enumDataModel> TipeSSH()
        {
            List<enumDataModel> tipessh = new List<enumDataModel>();
            tipessh.Add(new enumDataModel { Text = "SSH", Value = "1" });
            tipessh.Add(new enumDataModel { Text = "HSBK", Value = "2" });
            tipessh.Add(new enumDataModel { Text = "ASB", Value = "3" });
            tipessh.Add(new enumDataModel { Text = "SBU", Value = "4" });
            return tipessh;
        }
        public List<enumDataModel> TipeSwakelola()
        {
            List<enumDataModel> tipeswa = new List<enumDataModel>();
            tipeswa.Add(new enumDataModel { Text = "Tipe 1", Value = "1" });
            tipeswa.Add(new enumDataModel { Text = "Tipe 2", Value = "2" });
            tipeswa.Add(new enumDataModel { Text = "Tipe 3", Value = "3" });
            tipeswa.Add(new enumDataModel { Text = "Tipe 4", Value = "4" });
            return tipeswa;
        }
        public List<enumDataModel> Satuan()
        {
            List<enumDataModel> satuan = new List<enumDataModel>();
            satuan.Add(new enumDataModel { Text = "", Value = "0" });
            satuan.Add(new enumDataModel { Text = "Unit", Value = "1" });
            satuan.Add(new enumDataModel { Text = "Orang", Value = "2" });
            satuan.Add(new enumDataModel { Text = "Pax", Value = "3" });
            satuan.Add(new enumDataModel { Text = "Rim", Value = "4" });
            satuan.Add(new enumDataModel { Text = "Buah", Value = "5" });
            satuan.Add(new enumDataModel { Text = "Paket", Value = "6" });
            satuan.Add(new enumDataModel { Text = "Laporan", Value = "7" });
            satuan.Add(new enumDataModel { Text = "Dokumen", Value = "8" });
            satuan.Add(new enumDataModel { Text = "Layanan", Value = "9" });
            satuan.Add(new enumDataModel { Text = "Stel", Value = "10" });
            satuan.Add(new enumDataModel { Text = "Bulan", Value = "11" });
            return satuan;
        }
        public List<enumDataModel> JenisKebutuhan()
        {
            List<enumDataModel> jnskbthan = new List<enumDataModel>();
            jnskbthan.Add(new enumDataModel { Text = "Barang", Value = "1" });
            jnskbthan.Add(new enumDataModel { Text = "Pekerjaan Kontruksi", Value = "2" });
            jnskbthan.Add(new enumDataModel { Text = "Jasa Konsultasi", Value = "3" });
            jnskbthan.Add(new enumDataModel { Text = "Jasa Lainnya", Value = "4" });
            return jnskbthan;
        }
        public List<enumDataModel> TipeUser()
        {
            List<enumDataModel> tipeuser = new List<enumDataModel>();
            tipeuser.Add(new enumDataModel { Text = "Administrator", Value = "1" });
            tipeuser.Add(new enumDataModel { Text = "Auditor", Value = "2" });
            tipeuser.Add(new enumDataModel { Text = "Editor", Value = "3" });
            tipeuser.Add(new enumDataModel { Text = "Review", Value = "4" });
            tipeuser.Add(new enumDataModel { Text = "Viewer", Value = "5" });
            return tipeuser;
        }
        public List<enumDataModel> MetodePemilihan()
        {
            List<enumDataModel> metodepemilihan = new List<enumDataModel>();
            metodepemilihan.Add(new enumDataModel { Text = "Darurat", Value = "1" });
            metodepemilihan.Add(new enumDataModel { Text = "e-Purchasing", Value = "2" });
            metodepemilihan.Add(new enumDataModel { Text = "Kontes", Value = "3" });
            metodepemilihan.Add(new enumDataModel { Text = "Pengadaan Langsung", Value = "4" });
            metodepemilihan.Add(new enumDataModel { Text = "Penunjukan Langsung", Value = "5" });
            metodepemilihan.Add(new enumDataModel { Text = "Sayembara", Value = "6" });
            metodepemilihan.Add(new enumDataModel { Text = "Seleksi", Value = "7" });
            metodepemilihan.Add(new enumDataModel { Text = "Tender", Value = "8" });
            metodepemilihan.Add(new enumDataModel { Text = "Tender Cepat", Value = "9" });
            return metodepemilihan;
        }
    }
}
