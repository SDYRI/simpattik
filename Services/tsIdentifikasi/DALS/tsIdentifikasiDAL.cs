using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.DALS
{
    public class tsIdentifikasiDAL : ItsIdentifikasi
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string OPD;
        private readonly string TIPE;
        private readonly string TA;
        public tsIdentifikasiDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TIPE = httpContextAccessor.HttpContext.Session.GetInt32("Tipe").ToString();
            TA = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
        }

        public IList<tsIdentifikasiModel> GetAll(int spesifikasi)
        {
            string paket = "-1";
            if (TIPE == "1" || TIPE == "2")
            {
                paket = "";
            }
            
            List<tsIdentifikasiModel> Result = new List<tsIdentifikasiModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasigetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_paket", paket);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TA));
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            List<sumberDanaModel> sumberDanaAll = JsonConvert.DeserializeObject<List<sumberDanaModel>>(dataReader["rsumberdana"].ToString());
                            List<kondisiModel> kondisiAll = JsonConvert.DeserializeObject<List<kondisiModel>>(dataReader["rkondisilayak"].ToString());
                            List<barangmaterialModel> barangmateriall = JsonConvert.DeserializeObject<List<barangmaterialModel>>(dataReader["rbarangmaterial"].ToString());

                            Result.Add(new tsIdentifikasiModel()
                            {
                                ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? (int)dataReader["rthanggrn"] : 0),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? (int)dataReader["rlembaga"] : 0),
                                idopd = (dataReader["ropd"].GetType() != typeof(DBNull) ? (int)dataReader["ropd"] : 0),
                                opd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                program = (dataReader["rprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rprogram"] : ""),
                                namaprogram = (dataReader["rnamaprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaprogram"] : ""),
                                kegiatan = (dataReader["rkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rkegiatan"] : ""),
                                namakegiatan = (dataReader["rnamakegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamakegiatan"] : ""),
                                subkegiatan = (dataReader["rsubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rsubkegiatan"] : ""),
                                namasubkegiatan = (dataReader["rnamasubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasubkegiatan"] : ""),
                                idbrgkerj = (dataReader["rkoderekening"].GetType() != typeof(DBNull) ? (int)dataReader["rkoderekening"] : 0),
                                outputidentifikasi = (dataReader["routputidentifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["routputidentifikasi"] : ""),
                                jeniskebutuhan = (dataReader["rjeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rjeniskebutuhan"] : ""),
                                namabrgkerj = (dataReader["rnamabrgkerj"].GetType() != typeof(DBNull) ? (string)dataReader["rnamabrgkerj"] : ""),
                                fungsi = (dataReader["rfungsi"].GetType() != typeof(DBNull) ? (string)dataReader["rfungsi"] : ""),
                                jumlahbarang = (dataReader["rjumlahbarang"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarang"] : ""),
                                waktu = (dataReader["rwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktu"] : ""),
                                pihak = (dataReader["rpihak"].GetType() != typeof(DBNull) ? (string)dataReader["rpihak"] : ""),
                                totalwaktu = (dataReader["rtotalwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rtotalwaktu"] : ""),
                                ekatalog = (dataReader["rekatalog"].GetType() != typeof(DBNull) ? (string)dataReader["rekatalog"] : ""),
                                prioritas = (dataReader["rprioritas"].GetType() != typeof(DBNull) ? (string)dataReader["rprioritas"] : ""),
                                perkiraanbiaya = (dataReader["rperkiraanbiaya"].GetType() != typeof(DBNull) ? (Int64)dataReader["rperkiraanbiaya"] : 0),
                                jumlahpegawai = (dataReader["rjumlahpegawai"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahpegawai"] : ""),
                                bebantugas = (dataReader["rbebantugas"].GetType() != typeof(DBNull) ? (string)dataReader["rbebantugas"] : ""),
                                jumlahbarangtersedia = (dataReader["rjumlahbarangtersedia"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangtersedia"] : ""),
                                jumlahbarangsejenis = (dataReader["rjumlahbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangsejenis"] : ""),
                                lokasi = (dataReader["rlokasi"].GetType() != typeof(DBNull) ? (string)dataReader["rlokasi"] : ""),
                                kemudahan = (dataReader["rkemudahan"].GetType() != typeof(DBNull) ? (string)dataReader["rkemudahan"] : ""),
                                produsen = (dataReader["rprodusen"].GetType() != typeof(DBNull) ? (string)dataReader["rprodusen"] : ""),
                                kriteria = (dataReader["rkriteria"].GetType() != typeof(DBNull) ? (string)dataReader["rkriteria"] : ""),
                                tkdn = (dataReader["rtkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rtkdn"] : ""),
                                nilaitkdn = (dataReader["rnilaitkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rnilaitkdn"] : ""),
                                pengiriman = (dataReader["rpengiriman"].GetType() != typeof(DBNull) ? (string)dataReader["rpengiriman"] : ""),
                                pengakutan = (dataReader["rpengakutan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengakutan"] : ""),
                                pemasangan = (dataReader["rpemasangan"].GetType() != typeof(DBNull) ? (string)dataReader["rpemasangan"] : ""),
                                penimbunan = (dataReader["rpenimbunan"].GetType() != typeof(DBNull) ? (string)dataReader["rpenimbunan"] : ""),
                                pengunaan = (dataReader["rpengunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengunaan"] : ""),
                                kebutuhanpelatihan = (dataReader["rkebutuhanpelatihan"].GetType() != typeof(DBNull) ? (string)dataReader["rkebutuhanpelatihan"] : ""),
                                aspek = (dataReader["raspek"].GetType() != typeof(DBNull) ? (string)dataReader["raspek"] : ""),
                                barangsejenis = (dataReader["rbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangsejenis"] : ""),
                                indikasikonsolidasi = (dataReader["rindikasikonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["rindikasikonsolidasi"] : ""),
                                target = (dataReader["rtarget"].GetType() != typeof(DBNull) ? (string)dataReader["rtarget"] : ""),
                                studikelayakan = (dataReader["rstudikelayakan"].GetType() != typeof(DBNull) ? (string)dataReader["rstudikelayakan"] : ""),
                                ded = (dataReader["rded"].GetType() != typeof(DBNull) ? (string)dataReader["rded"] : ""),
                                komplektifitas = (dataReader["rkomplektifitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkomplektifitas"] : ""),
                                tahunpelaksanaan = (dataReader["rtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rtahunpelaksanaan"] : ""),
                                jumlahtahunpelaksanaan = (dataReader["rjumlahtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahtahunpelaksanaan"] : ""),
                                suratijin = (dataReader["rsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rsuratijin"] : ""),
                                nomorsuratijin = (dataReader["rnomorsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rnomorsuratijin"] : ""),
                                barangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                                barangmateriall = barangmateriall,
                                usahakecil = (dataReader["rusahakecil"].GetType() != typeof(DBNull) ? (string)dataReader["rusahakecil"] : ""),
                                pembebasanlahan = (dataReader["rpembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rpembebasanlahan"] : ""),
                                luaspembebasanlahan = (dataReader["rluaspembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rluaspembebasanlahan"] : ""),
                                pemanfaatantanah = (dataReader["rpemanfaatantanah"].GetType() != typeof(DBNull) ? (string)dataReader["rpemanfaatantanah"] : ""),
                                lamawaktu = (dataReader["rlamawaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rlamawaktu"] : ""),
                                administrasipembayaran = (dataReader["radministrasipembayaran"].GetType() != typeof(DBNull) ? (string)dataReader["radministrasipembayaran"] : ""),
                                terdapatpengadaan = (dataReader["rterdapatpengadaan"].GetType() != typeof(DBNull) ? (string)dataReader["rterdapatpengadaan"] : ""),
                                badanusaha = (dataReader["rbadanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rbadanusaha"] : ""),
                                targetsasaran = (dataReader["rtargetsasaran"].GetType() != typeof(DBNull) ? (string)dataReader["rtargetsasaran"] : ""),
                                manfaat = (dataReader["rmanfaat"].GetType() != typeof(DBNull) ? (string)dataReader["rmanfaat"] : ""),
                                kuantitas = (dataReader["rkuantitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkuantitas"] : ""),
                                spesifikasi = (dataReader["rspesifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasi"] : ""),
                                waktupenggunaan = (dataReader["rwaktupenggunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktupenggunaan"] : ""),
                                ketersediaanusaha = (dataReader["rketersediaanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rketersediaanusaha"] : ""),
                                ukurankapasitas = (dataReader["rukurankapasitas"].GetType() != typeof(DBNull) ? (string)dataReader["rukurankapasitas"] : ""),
                                koderup = (dataReader["rkoderup"].GetType() != typeof(DBNull) ? (string)dataReader["rkoderup"] : ""),
                                crtbyus = (dataReader["rcrtbyus"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtbyus"] : ""),
                                //crtdate = (dataReader["rcrtdate"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtdate"] : ""),
                                //mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (string)dataReader["rmdfdate"] : ""),
                                crpengdaan = (dataReader["rcrpengdaan"].GetType() != typeof(DBNull) ? (string)dataReader["rcrpengdaan"] : ""),
                                uraian = (dataReader["ruraian"].GetType() != typeof(DBNull) ? (string)dataReader["ruraian"] : ""),
                                kbki = (dataReader["rkbki"].GetType() != typeof(DBNull) ? (string)dataReader["rkbki"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                pnyswakelola = (dataReader["rpnyswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rpnyswakelola"] : ""),
                                nilaisirup = (dataReader["rnilaisirup"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                satuan = (dataReader["rsatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rsatuan"] : "0"),
                                namasatuan = (dataReader["rnamasatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasatuan"] : ""),
                                sumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                sumberdanal = sumberDanaAll,
                                kondisilayak = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                kondisil = kondisiAll,
                                txtsumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                txtkondisi = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                txtbarangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                            });
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

            return Result;
        }

        public IList<tsIdentifikasiModel> GetAll(int spesifikasi, string paket)
        {
            if ((TIPE == "1" || TIPE == "2") && paket == "-1")
            {
                paket = "";
            }
            else
            {
                if (paket == "")
                {
                    paket = "-1";
                }
            }

            List<tsIdentifikasiModel> Result = new List<tsIdentifikasiModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasigetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_paket", paket);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TA));
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            List<sumberDanaModel> sumberDanaAll = JsonConvert.DeserializeObject<List<sumberDanaModel>>(dataReader["rsumberdana"].ToString());
                            List<kondisiModel> kondisiAll = JsonConvert.DeserializeObject<List<kondisiModel>>(dataReader["rkondisilayak"].ToString());
                            List<barangmaterialModel> barangmateriall = JsonConvert.DeserializeObject<List<barangmaterialModel>>(dataReader["rbarangmaterial"].ToString());

                            Result.Add(new tsIdentifikasiModel()
                            {
                                ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? (int)dataReader["rthanggrn"] : 0),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? (int)dataReader["rlembaga"] : 0),
                                idopd = (dataReader["ropd"].GetType() != typeof(DBNull) ? (int)dataReader["ropd"] : 0),
                                opd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                program = (dataReader["rprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rprogram"] : ""),
                                namaprogram = (dataReader["rnamaprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaprogram"] : ""),
                                kegiatan = (dataReader["rkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rkegiatan"] : ""),
                                namakegiatan = (dataReader["rnamakegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamakegiatan"] : ""),
                                subkegiatan = (dataReader["rsubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rsubkegiatan"] : ""),
                                namasubkegiatan = (dataReader["rnamasubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasubkegiatan"] : ""),
                                idbrgkerj = (dataReader["rkoderekening"].GetType() != typeof(DBNull) ? (int)dataReader["rkoderekening"] : 0),
                                outputidentifikasi = (dataReader["routputidentifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["routputidentifikasi"] : ""),
                                jeniskebutuhan = (dataReader["rjeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rjeniskebutuhan"] : ""),
                                namabrgkerj = (dataReader["rnamabrgkerj"].GetType() != typeof(DBNull) ? (string)dataReader["rnamabrgkerj"] : ""),
                                fungsi = (dataReader["rfungsi"].GetType() != typeof(DBNull) ? (string)dataReader["rfungsi"] : ""),
                                jumlahbarang = (dataReader["rjumlahbarang"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarang"] : ""),
                                waktu = (dataReader["rwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktu"] : ""),
                                pihak = (dataReader["rpihak"].GetType() != typeof(DBNull) ? (string)dataReader["rpihak"] : ""),
                                totalwaktu = (dataReader["rtotalwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rtotalwaktu"] : ""),
                                ekatalog = (dataReader["rekatalog"].GetType() != typeof(DBNull) ? (string)dataReader["rekatalog"] : ""),
                                prioritas = (dataReader["rprioritas"].GetType() != typeof(DBNull) ? (string)dataReader["rprioritas"] : ""),
                                perkiraanbiaya = (dataReader["rperkiraanbiaya"].GetType() != typeof(DBNull) ? (Int64)dataReader["rperkiraanbiaya"] : 0),
                                jumlahpegawai = (dataReader["rjumlahpegawai"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahpegawai"] : ""),
                                bebantugas = (dataReader["rbebantugas"].GetType() != typeof(DBNull) ? (string)dataReader["rbebantugas"] : ""),
                                jumlahbarangtersedia = (dataReader["rjumlahbarangtersedia"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangtersedia"] : ""),
                                jumlahbarangsejenis = (dataReader["rjumlahbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangsejenis"] : ""),
                                lokasi = (dataReader["rlokasi"].GetType() != typeof(DBNull) ? (string)dataReader["rlokasi"] : ""),
                                kemudahan = (dataReader["rkemudahan"].GetType() != typeof(DBNull) ? (string)dataReader["rkemudahan"] : ""),
                                produsen = (dataReader["rprodusen"].GetType() != typeof(DBNull) ? (string)dataReader["rprodusen"] : ""),
                                kriteria = (dataReader["rkriteria"].GetType() != typeof(DBNull) ? (string)dataReader["rkriteria"] : ""),
                                tkdn = (dataReader["rtkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rtkdn"] : ""),
                                nilaitkdn = (dataReader["rnilaitkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rnilaitkdn"] : ""),
                                pengiriman = (dataReader["rpengiriman"].GetType() != typeof(DBNull) ? (string)dataReader["rpengiriman"] : ""),
                                pengakutan = (dataReader["rpengakutan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengakutan"] : ""),
                                pemasangan = (dataReader["rpemasangan"].GetType() != typeof(DBNull) ? (string)dataReader["rpemasangan"] : ""),
                                penimbunan = (dataReader["rpenimbunan"].GetType() != typeof(DBNull) ? (string)dataReader["rpenimbunan"] : ""),
                                pengunaan = (dataReader["rpengunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengunaan"] : ""),
                                kebutuhanpelatihan = (dataReader["rkebutuhanpelatihan"].GetType() != typeof(DBNull) ? (string)dataReader["rkebutuhanpelatihan"] : ""),
                                aspek = (dataReader["raspek"].GetType() != typeof(DBNull) ? (string)dataReader["raspek"] : ""),
                                barangsejenis = (dataReader["rbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangsejenis"] : ""),
                                indikasikonsolidasi = (dataReader["rindikasikonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["rindikasikonsolidasi"] : ""),
                                target = (dataReader["rtarget"].GetType() != typeof(DBNull) ? (string)dataReader["rtarget"] : ""),
                                studikelayakan = (dataReader["rstudikelayakan"].GetType() != typeof(DBNull) ? (string)dataReader["rstudikelayakan"] : ""),
                                ded = (dataReader["rded"].GetType() != typeof(DBNull) ? (string)dataReader["rded"] : ""),
                                komplektifitas = (dataReader["rkomplektifitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkomplektifitas"] : ""),
                                tahunpelaksanaan = (dataReader["rtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rtahunpelaksanaan"] : ""),
                                jumlahtahunpelaksanaan = (dataReader["rjumlahtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahtahunpelaksanaan"] : ""),
                                suratijin = (dataReader["rsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rsuratijin"] : ""),
                                nomorsuratijin = (dataReader["rnomorsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rnomorsuratijin"] : ""),
                                barangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                                barangmateriall = barangmateriall,
                                usahakecil = (dataReader["rusahakecil"].GetType() != typeof(DBNull) ? (string)dataReader["rusahakecil"] : ""),
                                pembebasanlahan = (dataReader["rpembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rpembebasanlahan"] : ""),
                                luaspembebasanlahan = (dataReader["rluaspembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rluaspembebasanlahan"] : ""),
                                pemanfaatantanah = (dataReader["rpemanfaatantanah"].GetType() != typeof(DBNull) ? (string)dataReader["rpemanfaatantanah"] : ""),
                                lamawaktu = (dataReader["rlamawaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rlamawaktu"] : ""),
                                administrasipembayaran = (dataReader["radministrasipembayaran"].GetType() != typeof(DBNull) ? (string)dataReader["radministrasipembayaran"] : ""),
                                terdapatpengadaan = (dataReader["rterdapatpengadaan"].GetType() != typeof(DBNull) ? (string)dataReader["rterdapatpengadaan"] : ""),
                                badanusaha = (dataReader["rbadanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rbadanusaha"] : ""),
                                targetsasaran = (dataReader["rtargetsasaran"].GetType() != typeof(DBNull) ? (string)dataReader["rtargetsasaran"] : ""),
                                manfaat = (dataReader["rmanfaat"].GetType() != typeof(DBNull) ? (string)dataReader["rmanfaat"] : ""),
                                kuantitas = (dataReader["rkuantitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkuantitas"] : ""),
                                spesifikasi = (dataReader["rspesifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasi"] : ""),
                                waktupenggunaan = (dataReader["rwaktupenggunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktupenggunaan"] : ""),
                                ketersediaanusaha = (dataReader["rketersediaanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rketersediaanusaha"] : ""),
                                ukurankapasitas = (dataReader["rukurankapasitas"].GetType() != typeof(DBNull) ? (string)dataReader["rukurankapasitas"] : ""),
                                koderup = (dataReader["rkoderup"].GetType() != typeof(DBNull) ? (string)dataReader["rkoderup"] : ""),
                                crtbyus = (dataReader["rcrtbyus"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtbyus"] : ""),
                                //crtdate = (dataReader["rcrtdate"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtdate"] : ""),
                                //mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (string)dataReader["rmdfdate"] : ""),
                                crpengdaan = (dataReader["rcrpengdaan"].GetType() != typeof(DBNull) ? (string)dataReader["rcrpengdaan"] : ""),
                                uraian = (dataReader["ruraian"].GetType() != typeof(DBNull) ? (string)dataReader["ruraian"] : ""),
                                kbki = (dataReader["rkbki"].GetType() != typeof(DBNull) ? (string)dataReader["rkbki"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                pnyswakelola = (dataReader["rpnyswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rpnyswakelola"] : ""),
                                nilaisirup = (dataReader["rnilaisirup"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                satuan = (dataReader["rsatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rsatuan"] : "0"),
                                namasatuan = (dataReader["rnamasatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasatuan"] : ""),
                                sumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                sumberdanal = sumberDanaAll,
                                kondisilayak = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                kondisil = kondisiAll,
                                txtsumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                txtkondisi = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                txtbarangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                            });
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

            return Result;
        }

        public IList<tsIdentifikasiModel> GetAllLaporan(int tipepaket)
        {
            List<tsIdentifikasiModel> Result = new List<tsIdentifikasiModel>();
            List<enumDataModel> jenisKebutuhan = new enumDataModel().JenisKebutuhan();
            List<enumDataModel> metodepemilihan = new enumDataModel().MetodePemilihan();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasigetallpenetapan", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", 0);
                    sqlCommand.Parameters.AddWithValue("_tipepaket", tipepaket);
                    sqlCommand.Parameters.AddWithValue("_paket", "");
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TA));
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            List<sumberDanaModel> sumberDanaAll = JsonConvert.DeserializeObject<List<sumberDanaModel>>(dataReader["rsumberdana"].ToString());
                            List<kondisiModel> kondisiAll = JsonConvert.DeserializeObject<List<kondisiModel>>(dataReader["rkondisilayak"].ToString());
                            List<barangmaterialModel> barangmateriall = JsonConvert.DeserializeObject<List<barangmaterialModel>>(dataReader["rbarangmaterial"].ToString());

                            Result.Add(new tsIdentifikasiModel()
                            {
                                no = (dataReader["rno"].GetType() != typeof(DBNull) ? (int)dataReader["rno"] : 0),
                                ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? (int)dataReader["rthanggrn"] : 0),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? (int)dataReader["rlembaga"] : 0),
                                idopd = (dataReader["ropd"].GetType() != typeof(DBNull) ? (int)dataReader["ropd"] : 0),
                                opd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                subopd = (dataReader["rnamasubopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasubopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                program = (dataReader["rprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rprogram"] : ""),
                                namaprogram = (dataReader["rnamaprogram"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaprogram"] : ""),
                                kegiatan = (dataReader["rkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rkegiatan"] : ""),
                                namakegiatan = (dataReader["rnamakegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamakegiatan"] : ""),
                                subkegiatan = (dataReader["rsubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rsubkegiatan"] : ""),
                                namasubkegiatan = (dataReader["rnamasubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasubkegiatan"] : ""),
                                idbrgkerj = (dataReader["rkoderekening"].GetType() != typeof(DBNull) ? (int)dataReader["rkoderekening"] : 0),
                                outputidentifikasi = (dataReader["routputidentifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["routputidentifikasi"] : ""),
                                jeniskebutuhan = jenisKebutuhan.FirstOrDefault(x => x.Value == (string)dataReader["rjeniskebutuhan"]).Text,
                                namabrgkerj = (dataReader["rnamabrgkerj"].GetType() != typeof(DBNull) ? (string)dataReader["rnamabrgkerj"] : ""),
                                fungsi = (dataReader["rfungsi"].GetType() != typeof(DBNull) ? (string)dataReader["rfungsi"] : ""),
                                jumlahbarang = (dataReader["rjumlahbarang"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarang"] : ""),
                                waktu = (dataReader["rwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktu"] : ""),
                                pihak = ((dataReader["rpihak"].GetType() != typeof(DBNull) && ((string)dataReader["rtipepaket"] == "Swakelola ")) ? (string)dataReader["rpihak"] : ""),
                                totalwaktu = (dataReader["rtotalwaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rtotalwaktu"] : ""),
                                ekatalog = (dataReader["rekatalog"].GetType() != typeof(DBNull) ? (string)dataReader["rekatalog"] : ""),
                                prioritas = (dataReader["rprioritas"].GetType() != typeof(DBNull) ? (string)dataReader["rprioritas"] : ""),
                                perkiraanbiaya = (dataReader["rperkiraanbiaya"].GetType() != typeof(DBNull) ? (Int64)dataReader["rperkiraanbiaya"] : 0),
                                jumlahpegawai = (dataReader["rjumlahpegawai"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahpegawai"] : ""),
                                bebantugas = (dataReader["rbebantugas"].GetType() != typeof(DBNull) ? (string)dataReader["rbebantugas"] : ""),
                                jumlahbarangtersedia = (dataReader["rjumlahbarangtersedia"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangtersedia"] : ""),
                                jumlahbarangsejenis = (dataReader["rjumlahbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahbarangsejenis"] : ""),
                                lokasi = (dataReader["rlokasikerja"].GetType() != typeof(DBNull) ? (string)dataReader["rlokasikerja"] : ""),
                                kemudahan = (dataReader["rkemudahan"].GetType() != typeof(DBNull) ? (string)dataReader["rkemudahan"] : ""),
                                produsen = (dataReader["rprodusen"].GetType() != typeof(DBNull) ? (string)dataReader["rprodusen"] : ""),
                                kriteria = (dataReader["rkriteriapaket"].GetType() != typeof(DBNull) ? (string)dataReader["rkriteriapaket"] : ""),
                                tkdn = (dataReader["rtkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rtkdn"] : ""),
                                nilaitkdn = (dataReader["rnilaitkdn"].GetType() != typeof(DBNull) ? (string)dataReader["rnilaitkdn"] : ""),
                                pengiriman = (dataReader["rpengiriman"].GetType() != typeof(DBNull) ? (string)dataReader["rpengiriman"] : ""),
                                pengakutan = (dataReader["rpengakutan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengakutan"] : ""),
                                pemasangan = (dataReader["rpemasangan"].GetType() != typeof(DBNull) ? (string)dataReader["rpemasangan"] : ""),
                                penimbunan = (dataReader["rpenimbunan"].GetType() != typeof(DBNull) ? (string)dataReader["rpenimbunan"] : ""),
                                pengunaan = (dataReader["rpengunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rpengunaan"] : ""),
                                kebutuhanpelatihan = (dataReader["rkebutuhanpelatihan"].GetType() != typeof(DBNull) ? (string)dataReader["rkebutuhanpelatihan"] : ""),
                                aspek = (dataReader["raspek"].GetType() != typeof(DBNull) ? (string)dataReader["raspek"] : ""),
                                barangsejenis = (dataReader["rbarangsejenis"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangsejenis"] : ""),
                                indikasikonsolidasi = (dataReader["rindikasikonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["rindikasikonsolidasi"] : ""),
                                target = (dataReader["rtarget"].GetType() != typeof(DBNull) ? (string)dataReader["rtarget"] : ""),
                                studikelayakan = (dataReader["rstudikelayakan"].GetType() != typeof(DBNull) ? (string)dataReader["rstudikelayakan"] : ""),
                                ded = (dataReader["rded"].GetType() != typeof(DBNull) ? (string)dataReader["rded"] : ""),
                                komplektifitas = (dataReader["rkomplektifitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkomplektifitas"] : ""),
                                tahunpelaksanaan = (dataReader["rtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rtahunpelaksanaan"] : ""),
                                jumlahtahunpelaksanaan = (dataReader["rjumlahtahunpelaksanaan"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahtahunpelaksanaan"] : ""),
                                suratijin = (dataReader["rsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rsuratijin"] : ""),
                                nomorsuratijin = (dataReader["rnomorsuratijin"].GetType() != typeof(DBNull) ? (string)dataReader["rnomorsuratijin"] : ""),
                                barangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                                barangmateriall = barangmateriall,
                                usahakecil = (dataReader["rpelakuusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rpelakuusaha"] : ""),
                                pembebasanlahan = (dataReader["rpembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rpembebasanlahan"] : ""),
                                luaspembebasanlahan = (dataReader["rluaspembebasanlahan"].GetType() != typeof(DBNull) ? (string)dataReader["rluaspembebasanlahan"] : ""),
                                pemanfaatantanah = (dataReader["rpemanfaatantanah"].GetType() != typeof(DBNull) ? (string)dataReader["rpemanfaatantanah"] : ""),
                                lamawaktu = (dataReader["rlamawaktu"].GetType() != typeof(DBNull) ? (string)dataReader["rlamawaktu"] : ""),
                                administrasipembayaran = (dataReader["radministrasipembayaran"].GetType() != typeof(DBNull) ? (string)dataReader["radministrasipembayaran"] : ""),
                                terdapatpengadaan = (dataReader["rterdapatpengadaan"].GetType() != typeof(DBNull) ? (string)dataReader["rterdapatpengadaan"] : ""),
                                badanusaha = (dataReader["rbadanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rbadanusaha"] : ""),
                                targetsasaran = (dataReader["rtargetsasaran"].GetType() != typeof(DBNull) ? (string)dataReader["rtargetsasaran"] : ""),
                                manfaat = (dataReader["rmanfaat"].GetType() != typeof(DBNull) ? (string)dataReader["rmanfaat"] : ""),
                                kuantitas = (dataReader["rkuantitas"].GetType() != typeof(DBNull) ? (string)dataReader["rkuantitas"] : ""),
                                spesifikasi = (dataReader["rspesifikasi"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasi"] : ""),
                                spesifikasipaket = (dataReader["rspesifikasipaket"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasipaket"] : ""),
                                waktupenggunaan = (dataReader["rwaktupenggunaan"].GetType() != typeof(DBNull) ? (string)dataReader["rwaktupenggunaan"] : ""),
                                ketersediaanusaha = (dataReader["rketersediaanusaha"].GetType() != typeof(DBNull) ? (string)dataReader["rketersediaanusaha"] : ""),
                                ukurankapasitas = (dataReader["rukurankapasitas"].GetType() != typeof(DBNull) ? (string)dataReader["rukurankapasitas"] : ""),
                                koderup = (dataReader["rkoderup"].GetType() != typeof(DBNull) ? (string)dataReader["rkoderup"] : ""),
                                crtbyus = (dataReader["rcrtbyus"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtbyus"] : ""),
                                //crtdate = (dataReader["rcrtdate"].GetType() != typeof(DBNull) ? (string)dataReader["rcrtdate"] : ""),
                                //mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (string)dataReader["rmdfdate"] : ""),
                                crpengdaan = (dataReader["rcrpengdaan"].GetType() != typeof(DBNull) ? (string)dataReader["rcrpengdaan"] : ""),
                                uraian = (dataReader["ruraian"].GetType() != typeof(DBNull) ? (string)dataReader["ruraian"] : ""),
                                kbki = (dataReader["rkbki"].GetType() != typeof(DBNull) ? (string)dataReader["rkbki"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                tipeswakelolapaket = (dataReader["rtipeswakelolapaket"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelolapaket"] : ""),
                                pnyswakelola = (dataReader["rpnyswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rpnyswakelola"] : ""),
                                nilaisirup = (dataReader["rnilaisirup"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                satuan = (dataReader["rsatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rsatuan"] : "0"),
                                namasatuan = (dataReader["rnamasatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamasatuan"] : ""),
                                sumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                sumberdanal = sumberDanaAll,
                                kondisilayak = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                kondisil = kondisiAll,
                                txtsumberdana = (dataReader["rsumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rsumberdana"] != "[]" ? (string)dataReader["rsumberdana"] : "[0]" : "[0]"),
                                txtkondisi = (dataReader["rkondisilayak"].GetType() != typeof(DBNull) ? (string)dataReader["rkondisilayak"] != "[]" ? (string)dataReader["rkondisilayak"] : "[0]" : "[0]"),
                                txtbarangmaterial = (dataReader["rbarangmaterial"].GetType() != typeof(DBNull) ? (string)dataReader["rbarangmaterial"] != "[]" ? (string)dataReader["rbarangmaterial"] : "[0]" : "[0]"),
                                nilaisumberdanaint = (dataReader["rnilaisumberdana"].GetType() != typeof(DBNull) ? (Int64)dataReader["rnilaisumberdanaint"] : 0),
                                valuesumberdana = (dataReader["rvaluesumberdana"].GetType() != typeof(DBNull) ? (string)dataReader["rvaluesumberdana"] : ""),
                                pemanfaatanmulai = (dataReader["rpemanfaatanmulai"].GetType() != typeof(DBNull) ? (string)dataReader["rpemanfaatanmulai"] : ""),
                                pemanfaatanakhir = (dataReader["rpemanfaatanakhir"].GetType() != typeof(DBNull) ? (string)dataReader["rpemanfaatanakhir"] : ""),
                                pelaksanaanmulai = (dataReader["rpelaksanaanmulai"].GetType() != typeof(DBNull) ? (string)dataReader["rpelaksanaanmulai"] : ""),
                                pelaksanaanakhir = (dataReader["rpelaksanaanakhir"].GetType() != typeof(DBNull) ? (string)dataReader["rpelaksanaanakhir"] : ""),
                                pemilihanmulai = (dataReader["rpemilihanmulai"].GetType() != typeof(DBNull) ? (string)dataReader["rpemilihanmulai"] : ""),
                                pemilihanakhir = (dataReader["rpemilihanakhir"].GetType() != typeof(DBNull) ? (string)dataReader["rpemilihanakhir"] : ""),
                                tipepaketnama = (dataReader["rtipepaket"].GetType() != typeof(DBNull) ? (string)dataReader["rtipepaket"] : ""),
                                namapaket = (dataReader["rnamapaket"].GetType() != typeof(DBNull) ? (string)dataReader["rnamapaket"] : ""),
                                metodepemilihan = (dataReader["rmetodepemilihan"].GetType() != typeof(DBNull) ? metodepemilihan.FirstOrDefault(x => x.Value == (string)dataReader["rmetodepemilihan"]).Text : ""),
                                namappk = (dataReader["rnamappk"].GetType() != typeof(DBNull) ? (string)dataReader["rnamappk"] : ""),
                                nipppk = (dataReader["rnip"].GetType() != typeof(DBNull) ? (string)dataReader["rnip"] : ""),
                                namajeniskebutuhan = (dataReader["rnamajeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rnamajeniskebutuhan"] : ""),
                                volumepekerjaan = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (string)dataReader["rvolume"] : ""),
                            });
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

            return Result;
        }

        public IList<SuratPenetapanModel> GetAllDasar()
        {
            List<SuratPenetapanModel> Result = new List<SuratPenetapanModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mddasargetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TA));
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new SuratPenetapanModel()
                            {
                                opd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                tahun = (dataReader["rtahun"].GetType() != typeof(DBNull) ? (int)dataReader["rtahun"] : 0),
                                urutdasar = (dataReader["rurutdasar"].GetType() != typeof(DBNull) ? (string)dataReader["rurutdasar"] : ""),
                                ketdasar = (dataReader["rketdasar"].GetType() != typeof(DBNull) ? (string)dataReader["rketdasar"] : ""),
                                tipedasar = (dataReader["rtipedasar"].GetType() != typeof(DBNull) ? (string)dataReader["rtipedasar"] : ""),
                                namappk = (dataReader["rnamappk"].GetType() != typeof(DBNull) ? (string)dataReader["rnamappk"] : ""),
                                nipppk = (dataReader["rnip"].GetType() != typeof(DBNull) ? (string)dataReader["rnip"] : ""),
                            });
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }

            return Result;
        }

        public DatabaseActionResultModel Create(tsIdentifikasiModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasiinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_thanggrn", ParamD.thanggrn == 0 ? int.Parse(TA) : ParamD.thanggrn);
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket == null ? string.Empty : ParamD.idpaket);
                    sqlCommand.Parameters.AddWithValue("_koderekening", ParamD.idbrgkerj == 0 ? 0 : ParamD.idbrgkerj);
                    sqlCommand.Parameters.AddWithValue("_jeniskebutuhan", ParamD.jeniskebutuhan == null ? string.Empty : ParamD.jeniskebutuhan);
                    sqlCommand.Parameters.AddWithValue("_namabrgkerj", ParamD.namabrgkerj == null ? string.Empty : ParamD.namabrgkerj);
                    sqlCommand.Parameters.AddWithValue("_kondisilayak", ParamD.kondisilayak == "[0]" ? "[]" : ParamD.kondisilayak);
                    sqlCommand.Parameters.AddWithValue("_sumberdana", ParamD.sumberdana == "[0]" ? "[]" : ParamD.sumberdana);
                    sqlCommand.Parameters.AddWithValue("_barangmaterial", ParamD.barangmaterial == "[0]" ? "[]" : ParamD.barangmaterial);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_program", ParamD.program == null ? string.Empty : ParamD.program);
                    sqlCommand.Parameters.AddWithValue("_kegiatan", ParamD.kegiatan == null ? string.Empty : ParamD.kegiatan);
                    sqlCommand.Parameters.AddWithValue("_subkegiatan", ParamD.subkegiatan == null ? string.Empty : ParamD.subkegiatan);
                    sqlCommand.Parameters.AddWithValue("_outputidentifikasi", ParamD.outputidentifikasi == null ? string.Empty : ParamD.outputidentifikasi);
                    sqlCommand.Parameters.AddWithValue("_fungsi", ParamD.fungsi == null ? string.Empty : ParamD.fungsi);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarang", ParamD.jumlahbarang == null ? string.Empty : ParamD.jumlahbarang);
                    sqlCommand.Parameters.AddWithValue("_waktu", ParamD.waktu == null ? string.Empty : ParamD.waktu);
                    sqlCommand.Parameters.AddWithValue("_pihak", ParamD.pihak == null ? string.Empty : ParamD.pihak);
                    sqlCommand.Parameters.AddWithValue("_totalwaktu", ParamD.totalwaktu == null ? string.Empty : ParamD.totalwaktu);
                    sqlCommand.Parameters.AddWithValue("_ekatalog", ParamD.ekatalog == null ? string.Empty : ParamD.ekatalog);
                    sqlCommand.Parameters.AddWithValue("_prioritas", ParamD.prioritas == null ? string.Empty : ParamD.prioritas);
                    sqlCommand.Parameters.AddWithValue("_perkiraanbiaya", (Int64)ParamD.perkiraanbiaya == 0 ? 0 : (Int64)ParamD.perkiraanbiaya);
                    sqlCommand.Parameters.AddWithValue("_jumlahpegawai", ParamD.jumlahpegawai == null ? string.Empty : ParamD.jumlahpegawai);
                    sqlCommand.Parameters.AddWithValue("_bebantugas", ParamD.bebantugas == null ? string.Empty : ParamD.bebantugas);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarangtersedia", ParamD.jumlahbarangtersedia == null ? string.Empty : ParamD.jumlahbarangtersedia);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarangsejenis", ParamD.jumlahbarangsejenis == null ? string.Empty : ParamD.jumlahbarangsejenis);
                    sqlCommand.Parameters.AddWithValue("_lokasi", ParamD.lokasi == null ? string.Empty : ParamD.lokasi);
                    sqlCommand.Parameters.AddWithValue("_kemudahan", ParamD.kemudahan == null ? string.Empty : ParamD.kemudahan);
                    sqlCommand.Parameters.AddWithValue("_produsen", ParamD.produsen == null ? string.Empty : ParamD.produsen);
                    sqlCommand.Parameters.AddWithValue("_kriteria", ParamD.kriteria == null ? string.Empty : ParamD.kriteria);
                    sqlCommand.Parameters.AddWithValue("_tkdn", ParamD.tkdn == null ? string.Empty : ParamD.tkdn);
                    sqlCommand.Parameters.AddWithValue("_nilaitkdn", ParamD.nilaitkdn == null ? string.Empty : ParamD.nilaitkdn);
                    sqlCommand.Parameters.AddWithValue("_pengiriman", ParamD.pengiriman == null ? string.Empty : ParamD.pengiriman);
                    sqlCommand.Parameters.AddWithValue("_pengakutan", ParamD.pengakutan == null ? string.Empty : ParamD.pengakutan);
                    sqlCommand.Parameters.AddWithValue("_pemasangan", ParamD.pemasangan == null ? string.Empty : ParamD.pemasangan);
                    sqlCommand.Parameters.AddWithValue("_penimbunan", ParamD.penimbunan == null ? string.Empty : ParamD.penimbunan);
                    sqlCommand.Parameters.AddWithValue("_pengunaan", ParamD.pengunaan == null ? string.Empty : ParamD.pengunaan);
                    sqlCommand.Parameters.AddWithValue("_kebutuhanpelatihan", ParamD.kebutuhanpelatihan == null ? string.Empty : ParamD.kebutuhanpelatihan);
                    sqlCommand.Parameters.AddWithValue("_aspek", ParamD.aspek == null ? string.Empty : ParamD.aspek);
                    sqlCommand.Parameters.AddWithValue("_barangsejenis", ParamD.barangsejenis == null ? string.Empty : ParamD.barangsejenis);
                    sqlCommand.Parameters.AddWithValue("_indikasikonsolidasi", ParamD.indikasikonsolidasi == null ? string.Empty : ParamD.indikasikonsolidasi);
                    sqlCommand.Parameters.AddWithValue("_target", ParamD.target == null ? string.Empty : ParamD.target);
                    sqlCommand.Parameters.AddWithValue("_studikelayakan", ParamD.studikelayakan == null ? string.Empty : ParamD.studikelayakan);
                    sqlCommand.Parameters.AddWithValue("_ded", ParamD.ded == null ? string.Empty : ParamD.ded);
                    sqlCommand.Parameters.AddWithValue("_komplektifitas", ParamD.komplektifitas == null ? string.Empty : ParamD.komplektifitas);
                    sqlCommand.Parameters.AddWithValue("_tahunpelaksanaan", ParamD.tahunpelaksanaan == null ? string.Empty : ParamD.tahunpelaksanaan);
                    sqlCommand.Parameters.AddWithValue("_jumlahtahunpelaksanaan", ParamD.jumlahtahunpelaksanaan == null ? string.Empty : ParamD.jumlahtahunpelaksanaan);
                    sqlCommand.Parameters.AddWithValue("_suratijin", ParamD.suratijin == null ? string.Empty : ParamD.suratijin);
                    sqlCommand.Parameters.AddWithValue("_nomorsuratijin", ParamD.nomorsuratijin == null ? string.Empty : ParamD.nomorsuratijin);
                    sqlCommand.Parameters.AddWithValue("_usahakecil", ParamD.usahakecil == null ? string.Empty : ParamD.usahakecil);
                    sqlCommand.Parameters.AddWithValue("_pembebasanlahan", ParamD.pembebasanlahan == null ? string.Empty : ParamD.pembebasanlahan);
                    sqlCommand.Parameters.AddWithValue("_luaspembebasanlahan", ParamD.luaspembebasanlahan == null ? string.Empty : ParamD.luaspembebasanlahan);
                    sqlCommand.Parameters.AddWithValue("_pemanfaatantanah", ParamD.pemanfaatantanah == null ? string.Empty : ParamD.pemanfaatantanah);
                    sqlCommand.Parameters.AddWithValue("_lamawaktu", ParamD.lamawaktu == null ? string.Empty : ParamD.lamawaktu);
                    sqlCommand.Parameters.AddWithValue("_administrasipembayaran", ParamD.administrasipembayaran == null ? string.Empty : ParamD.administrasipembayaran);
                    sqlCommand.Parameters.AddWithValue("_terdapatpengadaan", ParamD.terdapatpengadaan == null ? string.Empty : ParamD.terdapatpengadaan);
                    sqlCommand.Parameters.AddWithValue("_badanusaha", ParamD.badanusaha == null ? string.Empty : ParamD.badanusaha);
                    sqlCommand.Parameters.AddWithValue("_targetsasaran", ParamD.targetsasaran == null ? string.Empty : ParamD.targetsasaran);
                    sqlCommand.Parameters.AddWithValue("_manfaat", ParamD.manfaat == null ? string.Empty : ParamD.manfaat);
                    sqlCommand.Parameters.AddWithValue("_kuantitas", ParamD.kuantitas == null ? string.Empty : ParamD.kuantitas);
                    sqlCommand.Parameters.AddWithValue("_spesifikasi", ParamD.spesifikasi == null ? string.Empty : ParamD.spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_waktupenggunaan", ParamD.waktupenggunaan == null ? string.Empty : ParamD.waktupenggunaan);
                    sqlCommand.Parameters.AddWithValue("_ketersediaanusaha", ParamD.ketersediaanusaha == null ? string.Empty : ParamD.ketersediaanusaha);
                    sqlCommand.Parameters.AddWithValue("_ukurankapasitas", ParamD.ukurankapasitas == null ? string.Empty : ParamD.ukurankapasitas);
                    sqlCommand.Parameters.AddWithValue("_koderup", ParamD.koderup == null ? string.Empty : ParamD.koderup);
                    sqlCommand.Parameters.AddWithValue("_crpengdaan", ParamD.crpengdaan == null ? string.Empty : ParamD.crpengdaan);
                    sqlCommand.Parameters.AddWithValue("_uraian", ParamD.uraian == null ? string.Empty : ParamD.uraian);
                    sqlCommand.Parameters.AddWithValue("_kbki", ParamD.kbki == null ? string.Empty : ParamD.kbki);
                    sqlCommand.Parameters.AddWithValue("_tipeswakelola", ParamD.tipeswakelola == null ? string.Empty : ParamD.tipeswakelola);
                    sqlCommand.Parameters.AddWithValue("_pnyswakelola", ParamD.pnyswakelola == null ? string.Empty : ParamD.pnyswakelola);
                    sqlCommand.Parameters.AddWithValue("_nilaisirup", ParamD.nilaisirup == 0 ? 0 : ParamD.nilaisirup);
                    sqlCommand.Parameters.AddWithValue("_satuan", ParamD.jeniskebutuhan == "1" && ParamD.satuan == null ? "1" : ParamD.satuan == null ? "0" : ParamD.satuan);
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Kode = dataReader["kode"].ToString();
                            Result.Success = Result.Kode == "01" ? true : false;
                            Result.Pesan = dataReader["message"].ToString();
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
            return Result;
        }

        public DatabaseActionResultModel Update(tsIdentifikasiModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasiupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_ididetifikasi", ParamD.ididetifikasi);
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket);
                    sqlCommand.Parameters.AddWithValue("_koderekening", ParamD.idbrgkerj == 0 ? 0 : ParamD.idbrgkerj);
                    sqlCommand.Parameters.AddWithValue("_jeniskebutuhan", ParamD.jeniskebutuhan == null ? string.Empty : ParamD.jeniskebutuhan);
                    sqlCommand.Parameters.AddWithValue("_namabrgkerj", ParamD.namabrgkerj == null ? string.Empty : ParamD.namabrgkerj);
                    sqlCommand.Parameters.AddWithValue("_kondisilayak", ParamD.kondisilayak == "[0]" ? "[]" : ParamD.kondisilayak);
                    sqlCommand.Parameters.AddWithValue("_sumberdana", ParamD.sumberdana == "[0]" ? "[]" : ParamD.sumberdana);
                    sqlCommand.Parameters.AddWithValue("_barangmaterial", ParamD.barangmaterial == "[0]" ? "[]" : ParamD.barangmaterial);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_program", ParamD.program == null ? string.Empty : ParamD.program);
                    sqlCommand.Parameters.AddWithValue("_kegiatan", ParamD.kegiatan == null ? string.Empty : ParamD.kegiatan);
                    sqlCommand.Parameters.AddWithValue("_subkegiatan", ParamD.subkegiatan == null ? string.Empty : ParamD.subkegiatan);
                    sqlCommand.Parameters.AddWithValue("_outputidentifikasi", ParamD.outputidentifikasi == null ? string.Empty : ParamD.outputidentifikasi);
                    sqlCommand.Parameters.AddWithValue("_fungsi", ParamD.fungsi == null ? string.Empty : ParamD.fungsi);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarang", ParamD.jumlahbarang == null ? string.Empty : ParamD.jumlahbarang);
                    sqlCommand.Parameters.AddWithValue("_waktu", ParamD.waktu == null ? string.Empty : ParamD.waktu);
                    sqlCommand.Parameters.AddWithValue("_pihak", ParamD.pihak == null ? string.Empty : ParamD.pihak);
                    sqlCommand.Parameters.AddWithValue("_totalwaktu", ParamD.totalwaktu == null ? string.Empty : ParamD.totalwaktu);
                    sqlCommand.Parameters.AddWithValue("_ekatalog", ParamD.ekatalog == null ? string.Empty : ParamD.ekatalog);
                    sqlCommand.Parameters.AddWithValue("_prioritas", ParamD.prioritas == null ? string.Empty : ParamD.prioritas);
                    sqlCommand.Parameters.AddWithValue("_perkiraanbiaya", (Int64)ParamD.perkiraanbiaya == 0 ? 0 : (Int64)ParamD.perkiraanbiaya);
                    sqlCommand.Parameters.AddWithValue("_jumlahpegawai", ParamD.jumlahpegawai == null ? string.Empty : ParamD.jumlahpegawai);
                    sqlCommand.Parameters.AddWithValue("_bebantugas", ParamD.bebantugas == null ? string.Empty : ParamD.bebantugas);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarangtersedia", ParamD.jumlahbarangtersedia == null ? string.Empty : ParamD.jumlahbarangtersedia);
                    sqlCommand.Parameters.AddWithValue("_jumlahbarangsejenis", ParamD.jumlahbarangsejenis == null ? string.Empty : ParamD.jumlahbarangsejenis);
                    sqlCommand.Parameters.AddWithValue("_lokasi", ParamD.lokasi == null ? string.Empty : ParamD.lokasi);
                    sqlCommand.Parameters.AddWithValue("_kemudahan", ParamD.kemudahan == null ? string.Empty : ParamD.kemudahan);
                    sqlCommand.Parameters.AddWithValue("_produsen", ParamD.produsen == null ? string.Empty : ParamD.produsen);
                    sqlCommand.Parameters.AddWithValue("_kriteria", ParamD.kriteria == null ? string.Empty : ParamD.kriteria);
                    sqlCommand.Parameters.AddWithValue("_tkdn", ParamD.tkdn == null ? string.Empty : ParamD.tkdn);
                    sqlCommand.Parameters.AddWithValue("_nilaitkdn", ParamD.nilaitkdn == null ? string.Empty : ParamD.nilaitkdn);
                    sqlCommand.Parameters.AddWithValue("_pengiriman", ParamD.pengiriman == null ? string.Empty : ParamD.pengiriman);
                    sqlCommand.Parameters.AddWithValue("_pengakutan", ParamD.pengakutan == null ? string.Empty : ParamD.pengakutan);
                    sqlCommand.Parameters.AddWithValue("_pemasangan", ParamD.pemasangan == null ? string.Empty : ParamD.pemasangan);
                    sqlCommand.Parameters.AddWithValue("_penimbunan", ParamD.penimbunan == null ? string.Empty : ParamD.penimbunan);
                    sqlCommand.Parameters.AddWithValue("_pengunaan", ParamD.pengunaan == null ? string.Empty : ParamD.pengunaan);
                    sqlCommand.Parameters.AddWithValue("_kebutuhanpelatihan", ParamD.kebutuhanpelatihan == null ? string.Empty : ParamD.kebutuhanpelatihan);
                    sqlCommand.Parameters.AddWithValue("_aspek", ParamD.aspek == null ? string.Empty : ParamD.aspek);
                    sqlCommand.Parameters.AddWithValue("_barangsejenis", ParamD.barangsejenis == null ? string.Empty : ParamD.barangsejenis);
                    sqlCommand.Parameters.AddWithValue("_indikasikonsolidasi", ParamD.indikasikonsolidasi == null ? string.Empty : ParamD.indikasikonsolidasi);
                    sqlCommand.Parameters.AddWithValue("_target", ParamD.target == null ? string.Empty : ParamD.target);
                    sqlCommand.Parameters.AddWithValue("_studikelayakan", ParamD.studikelayakan == null ? string.Empty : ParamD.studikelayakan);
                    sqlCommand.Parameters.AddWithValue("_ded", ParamD.ded == null ? string.Empty : ParamD.ded);
                    sqlCommand.Parameters.AddWithValue("_komplektifitas", ParamD.komplektifitas == null ? string.Empty : ParamD.komplektifitas);
                    sqlCommand.Parameters.AddWithValue("_tahunpelaksanaan", ParamD.tahunpelaksanaan == null ? string.Empty : ParamD.tahunpelaksanaan);
                    sqlCommand.Parameters.AddWithValue("_jumlahtahunpelaksanaan", ParamD.jumlahtahunpelaksanaan == null ? string.Empty : ParamD.jumlahtahunpelaksanaan);
                    sqlCommand.Parameters.AddWithValue("_suratijin", ParamD.suratijin == null ? string.Empty : ParamD.suratijin);
                    sqlCommand.Parameters.AddWithValue("_nomorsuratijin", ParamD.nomorsuratijin == null ? string.Empty : ParamD.nomorsuratijin);
                    sqlCommand.Parameters.AddWithValue("_usahakecil", ParamD.usahakecil == null ? string.Empty : ParamD.usahakecil);
                    sqlCommand.Parameters.AddWithValue("_pembebasanlahan", ParamD.pembebasanlahan == null ? string.Empty : ParamD.pembebasanlahan);
                    sqlCommand.Parameters.AddWithValue("_luaspembebasanlahan", ParamD.luaspembebasanlahan == null ? string.Empty : ParamD.luaspembebasanlahan);
                    sqlCommand.Parameters.AddWithValue("_pemanfaatantanah", ParamD.pemanfaatantanah == null ? string.Empty : ParamD.pemanfaatantanah);
                    sqlCommand.Parameters.AddWithValue("_lamawaktu", ParamD.lamawaktu == null ? string.Empty : ParamD.lamawaktu);
                    sqlCommand.Parameters.AddWithValue("_administrasipembayaran", ParamD.administrasipembayaran == null ? string.Empty : ParamD.administrasipembayaran);
                    sqlCommand.Parameters.AddWithValue("_terdapatpengadaan", ParamD.terdapatpengadaan == null ? string.Empty : ParamD.terdapatpengadaan);
                    sqlCommand.Parameters.AddWithValue("_badanusaha", ParamD.badanusaha == null ? string.Empty : ParamD.badanusaha);
                    sqlCommand.Parameters.AddWithValue("_targetsasaran", ParamD.targetsasaran == null ? string.Empty : ParamD.targetsasaran);
                    sqlCommand.Parameters.AddWithValue("_manfaat", ParamD.manfaat == null ? string.Empty : ParamD.manfaat);
                    sqlCommand.Parameters.AddWithValue("_kuantitas", ParamD.kuantitas == null ? string.Empty : ParamD.kuantitas);
                    sqlCommand.Parameters.AddWithValue("_spesifikasi", ParamD.spesifikasi == null ? string.Empty : ParamD.spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_waktupenggunaan", ParamD.waktupenggunaan == null ? string.Empty : ParamD.waktupenggunaan);
                    sqlCommand.Parameters.AddWithValue("_ketersediaanusaha", ParamD.ketersediaanusaha == null ? string.Empty : ParamD.ketersediaanusaha);
                    sqlCommand.Parameters.AddWithValue("_ukurankapasitas", ParamD.ukurankapasitas == null ? string.Empty : ParamD.ukurankapasitas);
                    sqlCommand.Parameters.AddWithValue("_koderup", ParamD.koderup == null ? string.Empty : ParamD.koderup);
                    sqlCommand.Parameters.AddWithValue("_crpengdaan", ParamD.crpengdaan == null ? string.Empty : ParamD.crpengdaan);
                    sqlCommand.Parameters.AddWithValue("_uraian", ParamD.uraian == null ? string.Empty : ParamD.uraian);
                    sqlCommand.Parameters.AddWithValue("_kbki", ParamD.kbki == null ? string.Empty : ParamD.kbki);
                    sqlCommand.Parameters.AddWithValue("_tipeswakelola", ParamD.tipeswakelola == null ? string.Empty : ParamD.tipeswakelola);
                    sqlCommand.Parameters.AddWithValue("_pnyswakelola", ParamD.pnyswakelola == null ? string.Empty : ParamD.pnyswakelola);
                    sqlCommand.Parameters.AddWithValue("_nilaisirup", ParamD.nilaisirup == 0 ? 0 : ParamD.nilaisirup);
                    sqlCommand.Parameters.AddWithValue("_satuan", ParamD.jeniskebutuhan == "1" && ParamD.satuan == null ? "1" : ParamD.satuan == null ? "0" : ParamD.satuan);
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Kode = dataReader["kode"].ToString();
                            Result.Success = Result.Kode == "01" ? true : false;
                            Result.Pesan = dataReader["message"].ToString();
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
            return Result;
        }

        public DatabaseActionResultModel Remove(int identifikasi, string paket)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsidentifikasiremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_ididetifikasi", identifikasi);
                    sqlCommand.Parameters.AddWithValue("_idpaket", paket);
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Kode = dataReader["kode"].ToString();
                            Result.Success = Result.Kode == "01" ? true : false;
                            Result.Pesan = dataReader["message"].ToString();
                        }
                    }
                }
            }
            catch (Exception Exception)
            {
                throw Exception;
            }
            return Result;
        }
    }
}
