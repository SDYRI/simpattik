using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Extensions;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tsPaket.DALS
{
    public class tsPaketDAL : ItsPaket
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string OPD;
        private readonly string TIPE;
        private readonly string TAHUN;
        public tsPaketDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TIPE = httpContextAccessor.HttpContext.Session.GetInt32("Tipe").ToString();
            TAHUN = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
        }

        public IList<tsPaketModel> GetAll(int spesifikasi, int tipePaket)
        {
            List<tsPaketModel> Result = new List<tsPaketModel>();
            List<enumDataModel> statuspaket = new enumDataModel().Status();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_tipe", tipePaket);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TAHUN));
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tsPaketModel()
                            {
                                //ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? ((int)dataReader["rlembaga"]).ToString() : ""),
                                opd = (dataReader["ropd"].GetType() != typeof(DBNull) ? ((int)dataReader["ropd"]).ToString() : ""),
                                namaopd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? ((int)dataReader["rthanggrn"]).ToString() : ""),
                                nmpaket = (dataReader["rnmpaket"].GetType() != typeof(DBNull) ? (string)dataReader["rnmpaket"] : ""),
                                namakonsolidasi = (dataReader["rnamakonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["rnamakonsolidasi"] : ""),
                                volume = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (string)dataReader["rvolume"] : ""),
                                uraian = (dataReader["ruraianpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ruraianpaket"] : ""),
                                spesifikasi = (dataReader["rspesifikasipaket"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasipaket"] : ""),
                                prodlmnegeri = (dataReader["rprodlmnegeri"].GetType() != typeof(DBNull) ? (string)dataReader["rprodlmnegeri"] : ""),
                                ushkecil = (dataReader["rushkecil"].GetType() != typeof(DBNull) ? (string)dataReader["rushkecil"] : ""),
                                pradpa = (dataReader["rpradpa"].GetType() != typeof(DBNull) ? (string)dataReader["rpradpa"] : ""),
                                mtdpemilihanstlh = (dataReader["rmtdpemilihanstlh"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstlh"] : ""),
                                jeniskebutuhan = (dataReader["rjeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rjeniskebutuhan"] : ""),
                                pemanfaatanmulai = (dataReader["rpemanfaatanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanmulai"] : new DateTime()),
                                pemanfaatanakhir = (dataReader["rpemanfaatanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanakhir"] : new DateTime()),
                                pelaksanaanmulai = (dataReader["rpelaksanaanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanmulai"] : new DateTime()),
                                pelaksanaanakhir = (dataReader["rpelaksanaanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanakhir"] : new DateTime()),
                                pemilihanmulai = (dataReader["rpemilihanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanmulai"] : new DateTime()),
                                pemilihanakhir = (dataReader["rpemilihanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanakhir"] : new DateTime()),
                                mtdpemilihanstblm = (dataReader["rmtdpemilihanstblm"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstblm"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                paketstrategis = (dataReader["rpaketstrategis"].GetType() != typeof(DBNull) ? (string)dataReader["rpaketstrategis"] : ""),
                                penyelengaraswakelola = (dataReader["rpenyelengaraswakelola"].GetType() != typeof(DBNull) ? (int)dataReader["rpenyelengaraswakelola"] : 0),
                                pagusblm = (dataReader["rpagusblm"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagusblm"] : 0),
                                pagustlh = (dataReader["rpagustlh"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagustlh"] : 0),
                                nilaibelanjalangsung = (!(dataReader["rnilaibelanjalangsung"] is DBNull) ? (int)dataReader["rnilaibelanjalangsung"] : 0),
                                nilaisirup = (!(dataReader["rnilaisirup"] is DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                idkonsolidasi = (dataReader["ridkonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["ridkonsolidasi"] : ""),
                                keteranganmetode = (dataReader["rketeranganmetode"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganmetode"] : ""),
                                keteranganpagu = (dataReader["rketeranganpagu"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganpagu"] : ""),
                                statuspaket = statuspaket.FirstOrDefault(x => x.Value == (string)dataReader["rstatuspaket"]).Text,
                                mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rmdfdate"] : new DateTime()),
                                nilaiselisih = (!(dataReader["rnilaibelanjalangsung"] is DBNull) ? (int)dataReader["rnilaibelanjalangsung"] : 0) - (!(dataReader["rnilaisirup"] is DBNull) ? (int)dataReader["rnilaisirup"] : 0),
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

        public IList<tsPaketModel> GetAllReview(int spesifikasi, int tipePaket, string opdReview)
        {
            List<tsPaketModel> Result = new List<tsPaketModel>();
            List<enumDataModel> statuspaket = new enumDataModel().Status();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", opdReview);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_tipe", tipePaket);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TAHUN));
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tsPaketModel()
                            {
                                //ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? ((int)dataReader["rlembaga"]).ToString() : ""),
                                opd = (dataReader["ropd"].GetType() != typeof(DBNull) ? ((int)dataReader["ropd"]).ToString() : ""),
                                namaopd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? ((int)dataReader["rthanggrn"]).ToString() : ""),
                                nmpaket = (dataReader["rnmpaket"].GetType() != typeof(DBNull) ? (string)dataReader["rnmpaket"] : ""),
                                namakonsolidasi = (dataReader["rnamakonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["rnamakonsolidasi"] : ""),
                                volume = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (string)dataReader["rvolume"] : ""),
                                uraian = (dataReader["ruraianpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ruraianpaket"] : ""),
                                spesifikasi = (dataReader["rspesifikasipaket"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasipaket"] : ""),
                                prodlmnegeri = (dataReader["rprodlmnegeri"].GetType() != typeof(DBNull) ? (string)dataReader["rprodlmnegeri"] : ""),
                                ushkecil = (dataReader["rushkecil"].GetType() != typeof(DBNull) ? (string)dataReader["rushkecil"] : ""),
                                pradpa = (dataReader["rpradpa"].GetType() != typeof(DBNull) ? (string)dataReader["rpradpa"] : ""),
                                mtdpemilihanstlh = (dataReader["rmtdpemilihanstlh"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstlh"] : ""),
                                jeniskebutuhan = (dataReader["rjeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rjeniskebutuhan"] : ""),
                                pemanfaatanmulai = (dataReader["rpemanfaatanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanmulai"] : new DateTime()),
                                pemanfaatanakhir = (dataReader["rpemanfaatanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanakhir"] : new DateTime()),
                                pelaksanaanmulai = (dataReader["rpelaksanaanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanmulai"] : new DateTime()),
                                pelaksanaanakhir = (dataReader["rpelaksanaanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanakhir"] : new DateTime()),
                                pemilihanmulai = (dataReader["rpemilihanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanmulai"] : new DateTime()),
                                pemilihanakhir = (dataReader["rpemilihanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanakhir"] : new DateTime()),
                                mtdpemilihanstblm = (dataReader["rmtdpemilihanstblm"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstblm"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                paketstrategis = (dataReader["rpaketstrategis"].GetType() != typeof(DBNull) ? (string)dataReader["rpaketstrategis"] : ""),
                                penyelengaraswakelola = (dataReader["rpenyelengaraswakelola"].GetType() != typeof(DBNull) ? (int)dataReader["rpenyelengaraswakelola"] : 0),
                                pagusblm = (dataReader["rpagusblm"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagusblm"] : 0),
                                pagustlh = (dataReader["rpagustlh"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagustlh"] : 0),
                                nilaibelanjalangsung = (!(dataReader["rnilaibelanjalangsung"] is DBNull) ? (int)dataReader["rnilaibelanjalangsung"] : 0),
                                nilaisirup = (!(dataReader["rnilaisirup"] is DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                idkonsolidasi = (dataReader["ridkonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["ridkonsolidasi"] : ""),
                                keteranganmetode = (dataReader["rketeranganmetode"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganmetode"] : ""),
                                keteranganpagu = (dataReader["rketeranganpagu"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganpagu"] : ""),
                                statuspaket = statuspaket.FirstOrDefault(x => x.Value == (string)dataReader["rstatuspaket"]).Text,
                                mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rmdfdate"] : new DateTime()),
                                nilaiselisih = (!(dataReader["rnilaibelanjalangsung"] is DBNull) ? (int)dataReader["rnilaibelanjalangsung"] : 0) - (!(dataReader["rnilaisirup"] is DBNull) ? (int)dataReader["rnilaisirup"] : 0),
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

        public IList<tsPaketModel> GetAllPerencanaan(int spesifikasi, int tipePaket)
        {
            List<tsPaketModel> Result = new List<tsPaketModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsreviewwperencanaangetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TAHUN));
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tsPaketModel()
                            {
                                //ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? ((int)dataReader["rlembaga"]).ToString() : ""),
                                opd = (dataReader["ropd"].GetType() != typeof(DBNull) ? BethaCrypto.Encrypt(((int)dataReader["ropd"]).ToString()) : ""),
                                namaopd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                statuspaket = (dataReader["rstatuspaket"].GetType() != typeof(DBNull) ? (string)dataReader["rstatuspaket"] : ""),
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

        public IList<tsPaketModel> GetAllStrategis(int spesifikasi, int tipePaket)
        {
            List<tsPaketModel> Result = new List<tsPaketModel>();
            List<enumDataModel> statushps = new enumDataModel().Status();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketgetallstrategis", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_kebutuhan", spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_tipe", tipePaket);
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TAHUN));
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tsPaketModel()
                            {
                                //ididetifikasi = (dataReader["rididetifikasi"].GetType() != typeof(DBNull) ? (int)dataReader["rididetifikasi"] : 0),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? ((int)dataReader["rlembaga"]).ToString() : ""),
                                opd = (dataReader["ropd"].GetType() != typeof(DBNull) ? ((int)dataReader["ropd"]).ToString() : ""),
                                namaopd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? ((int)dataReader["rthanggrn"]).ToString() : ""),
                                nmpaket = (dataReader["rnmpaket"].GetType() != typeof(DBNull) ? (string)dataReader["rnmpaket"] : ""),
                                volume = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (string)dataReader["rvolume"] : ""),
                                uraian = (dataReader["ruraianpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ruraianpaket"] : ""),
                                spesifikasi = (dataReader["rspesifikasipaket"].GetType() != typeof(DBNull) ? (string)dataReader["rspesifikasipaket"] : ""),
                                prodlmnegeri = (dataReader["rprodlmnegeri"].GetType() != typeof(DBNull) ? (string)dataReader["rprodlmnegeri"] : ""),
                                ushkecil = (dataReader["rushkecil"].GetType() != typeof(DBNull) ? (string)dataReader["rushkecil"] : ""),
                                pradpa = (dataReader["rpradpa"].GetType() != typeof(DBNull) ? (string)dataReader["rpradpa"] : ""),
                                mtdpemilihanstlh = (dataReader["rmtdpemilihanstlh"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstlh"] : ""),
                                jeniskebutuhan = (dataReader["rjeniskebutuhan"].GetType() != typeof(DBNull) ? (string)dataReader["rjeniskebutuhan"] : ""),
                                pemanfaatanmulai = (dataReader["rpemanfaatanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanmulai"] : new DateTime()),
                                pemanfaatanakhir = (dataReader["rpemanfaatanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemanfaatanakhir"] : new DateTime()),
                                pelaksanaanmulai = (dataReader["rpelaksanaanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanmulai"] : new DateTime()),
                                pelaksanaanakhir = (dataReader["rpelaksanaanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpelaksanaanakhir"] : new DateTime()),
                                pemilihanmulai = (dataReader["rpemilihanmulai"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanmulai"] : new DateTime()),
                                pemilihanakhir = (dataReader["rpemilihanakhir"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rpemilihanakhir"] : new DateTime()),
                                mtdpemilihanstblm = (dataReader["rmtdpemilihanstblm"].GetType() != typeof(DBNull) ? (string)dataReader["rmtdpemilihanstblm"] : ""),
                                tipeswakelola = (dataReader["rtipeswakelola"].GetType() != typeof(DBNull) ? (string)dataReader["rtipeswakelola"] : ""),
                                paketstrategis = (dataReader["rpaketstrategis"].GetType() != typeof(DBNull) ? (string)dataReader["rpaketstrategis"] : ""),
                                penyelengaraswakelola = (dataReader["rpenyelengaraswakelola"].GetType() != typeof(DBNull) ? (int)dataReader["rpenyelengaraswakelola"] : 0),
                                pagusblm = (dataReader["rpagusblm"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagusblm"] : 0),
                                pagustlh = (dataReader["rpagustlh"].GetType() != typeof(DBNull) ? (Int64)dataReader["rpagustlh"] : 0),
                                nilaibelanjalangsung = (dataReader["rnilaibelanjalangsung"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaibelanjalangsung"] : 0),
                                nilaisirup = (dataReader["rnilaisirup"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaisirup"] : 0),
                                idkonsolidasi = (dataReader["ridkonsolidasi"].GetType() != typeof(DBNull) ? (string)dataReader["ridkonsolidasi"] : ""),
                                keteranganmetode = (dataReader["rketeranganmetode"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganmetode"] : ""),
                                keteranganpagu = (dataReader["rketeranganpagu"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganpagu"] : ""),
                                statushps = statushps.FirstOrDefault(x => x.Value == (string)dataReader["rstatushps"]).Text,
                                mdfdate = (dataReader["rmdfdate"].GetType() != typeof(DBNull) ? (DateTime)dataReader["rmdfdate"] : new DateTime()),
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

        public DatabaseActionResultModel Create(tsPaketModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            DateTime? _pemanfaatanmulai, _pemanfaatanakhir, _pemilihanmulai, _pemilihanakhir;
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketinsert", sqlConnection))
                {
                    _pemanfaatanmulai = ParamD.pemanfaatanmulai == null ? ParamD.pelaksanaanmulai : ParamD.pemanfaatanmulai;
                    _pemanfaatanakhir = ParamD.pemanfaatanakhir == null ? ParamD.pelaksanaanakhir : ParamD.pemanfaatanakhir;
                    _pemilihanmulai = ParamD.pemilihanmulai == null ? ParamD.pelaksanaanmulai : ParamD.pemilihanmulai;
                    _pemilihanakhir = ParamD.pemilihanakhir == null ? ParamD.pelaksanaanakhir : ParamD.pemilihanakhir;

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_thanggrn", ParamD.thanggrn == null ? string.Empty : ParamD.thanggrn);
                    sqlCommand.Parameters.AddWithValue("_tipepaket", ParamD.tipePaket);
                    sqlCommand.Parameters.AddWithValue("_nmpaket", ParamD.nmpaket == null ? string.Empty : ParamD.nmpaket);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_uraian", ParamD.uraian == null ? string.Empty : ParamD.uraian);
                    sqlCommand.Parameters.AddWithValue("_spesifikasi", ParamD.spesifikasi== null ? string.Empty : ParamD.spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_prodlmnegeri", ParamD.prodlmnegeri == null ? string.Empty : ParamD.prodlmnegeri);
                    sqlCommand.Parameters.AddWithValue("_ushkecil", ParamD.ushkecil == null ? string.Empty : ParamD.ushkecil);
                    sqlCommand.Parameters.AddWithValue("_pradpa", ParamD.pradpa == null ? string.Empty : ParamD.pradpa);
                    sqlCommand.Parameters.AddWithValue("_mtdpemilihanstblm", ParamD.mtdpemilihanstblm == null ? string.Empty : ParamD.mtdpemilihanstblm);
                    sqlCommand.Parameters.AddWithValue("_jeniskebutuhan", ParamD.jeniskebutuhan == null ? string.Empty : ParamD.jeniskebutuhan);
                    sqlCommand.Parameters.AddWithValue("_pemanfaatanmulai", (_pemanfaatanmulai.Value.Year.ToString() +"-"+ _pemanfaatanmulai.Value.Month.ToString() +"-"+ _pemanfaatanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemanfaatanakhir", (_pemanfaatanakhir.Value.Year.ToString() + "-" + _pemanfaatanakhir.Value.Month.ToString() + "-" + _pemanfaatanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pelaksanaanmulai", ParamD.pelaksanaanmulai == null ? "" : (ParamD.pelaksanaanmulai.Value.Year.ToString() + "-" + ParamD.pelaksanaanmulai.Value.Month.ToString() + "-" + ParamD.pelaksanaanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pelaksanaanakhir", ParamD.pelaksanaanakhir == null ? "" : (ParamD.pelaksanaanakhir.Value.Year.ToString() + "-" + ParamD.pelaksanaanakhir.Value.Month.ToString() + "-" + ParamD.pelaksanaanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemilihanmulai", (_pemilihanmulai.Value.Year.ToString() + "-" + _pemilihanmulai.Value.Month.ToString() + "-" + _pemilihanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemilihanakhir", (_pemilihanakhir.Value.Year.ToString() + "-" + _pemilihanakhir.Value.Month.ToString() + "-" + _pemilihanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_tipeswakelola", ParamD.tipeswakelola == null ? "1" : ParamD.tipeswakelola);
                    sqlCommand.Parameters.AddWithValue("_paketstrategis", ParamD.paketstrategis == null ? "0" : ParamD.paketstrategis);
                    sqlCommand.Parameters.AddWithValue("_pnyelgraswakelola", ParamD.penyelengaraswakelola);
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

        public DatabaseActionResultModel Update(tsPaketModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            DateTime? _pemanfaatanmulai, _pemanfaatanakhir, _pemilihanmulai, _pemilihanakhir;
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketupdate", sqlConnection))
                {
                    _pemanfaatanmulai = ParamD.pemanfaatanmulai == null ? ParamD.pelaksanaanmulai : ParamD.pemanfaatanmulai;
                    _pemanfaatanakhir = ParamD.pemanfaatanakhir == null ? ParamD.pelaksanaanakhir : ParamD.pemanfaatanakhir;
                    _pemilihanmulai = ParamD.pemilihanmulai == null ? ParamD.pelaksanaanmulai : ParamD.pemilihanmulai;
                    _pemilihanakhir = ParamD.pemilihanakhir == null ? ParamD.pelaksanaanakhir : ParamD.pemilihanakhir;

                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket);
                    sqlCommand.Parameters.AddWithValue("_nmpaket", ParamD.nmpaket == null ? string.Empty : ParamD.nmpaket);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_prodlmnegeri", ParamD.prodlmnegeri == null ? string.Empty : ParamD.prodlmnegeri);
                    sqlCommand.Parameters.AddWithValue("_ushkecil", ParamD.ushkecil == null ? string.Empty : ParamD.ushkecil);
                    sqlCommand.Parameters.AddWithValue("_pradpa", ParamD.pradpa == null ? string.Empty : ParamD.pradpa);
                    sqlCommand.Parameters.AddWithValue("_mtdpemilihanstlh", ParamD.mtdpemilihanstlh == null ? string.Empty : ParamD.mtdpemilihanstlh);
                    sqlCommand.Parameters.AddWithValue("_jeniskebutuhan", ParamD.jeniskebutuhan == null ? string.Empty : ParamD.jeniskebutuhan);
                    sqlCommand.Parameters.AddWithValue("_pemanfaatanmulai", (_pemanfaatanmulai.Value.Year.ToString() + "-" + _pemanfaatanmulai.Value.Month.ToString() + "-" + _pemanfaatanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemanfaatanakhir", (_pemanfaatanakhir.Value.Year.ToString() + "-" + _pemanfaatanakhir.Value.Month.ToString() + "-" + _pemanfaatanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pelaksanaanmulai", ParamD.pelaksanaanmulai == null ? "" : (ParamD.pelaksanaanmulai.Value.Year.ToString() + "-" + ParamD.pelaksanaanmulai.Value.Month.ToString() + "-" + ParamD.pelaksanaanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pelaksanaanakhir", ParamD.pelaksanaanakhir == null ? "" : (ParamD.pelaksanaanakhir.Value.Year.ToString() + "-" + ParamD.pelaksanaanakhir.Value.Month.ToString() + "-" + ParamD.pelaksanaanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemilihanmulai", (_pemilihanmulai.Value.Year.ToString() + "-" + _pemilihanmulai.Value.Month.ToString() + "-" + _pemilihanmulai.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_pemilihanakhir", (_pemilihanakhir.Value.Year.ToString() + "-" + _pemilihanakhir.Value.Month.ToString() + "-" + _pemilihanakhir.Value.Day.ToString()));
                    sqlCommand.Parameters.AddWithValue("_mtdpemilihanstblm", ParamD.mtdpemilihanstblm == null ? string.Empty : ParamD.mtdpemilihanstblm);
                    sqlCommand.Parameters.AddWithValue("_idkonsolidasi", ParamD.idkonsolidasi == null ? string.Empty : ParamD.idkonsolidasi);
                    sqlCommand.Parameters.AddWithValue("_tipepaket", ParamD.tipePaket);
                    sqlCommand.Parameters.AddWithValue("_uraianpaket", ParamD.uraian == null ? string.Empty : ParamD.uraian);
                    sqlCommand.Parameters.AddWithValue("_spesifikasipaket", ParamD.spesifikasi == null ? string.Empty : ParamD.spesifikasi);
                    sqlCommand.Parameters.AddWithValue("_tipeswakelola", ParamD.tipeswakelola == null ? "1" : ParamD.tipeswakelola);
                    sqlCommand.Parameters.AddWithValue("_paketstrategis", ParamD.paketstrategis == null ? "0" : ParamD.paketstrategis);
                    sqlCommand.Parameters.AddWithValue("_penyelengaraswakelola", ParamD.penyelengaraswakelola);
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

        public DatabaseActionResultModel UpdateReview(tsPaketModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketupdatereview", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket);
                    sqlCommand.Parameters.AddWithValue("_nmpaket", ParamD.nmpaket == null ? string.Empty : ParamD.nmpaket);
                    sqlCommand.Parameters.AddWithValue("_keteranganmetode", ParamD.keteranganmetode == null ? string.Empty : ParamD.keteranganmetode);
                    sqlCommand.Parameters.AddWithValue("_keteranganpagu", ParamD.keteranganpagu == null ? string.Empty : ParamD.keteranganpagu);
                    sqlCommand.Parameters.AddWithValue("_revisi", ParamD.statuspaket);
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

        public DatabaseActionResultModel UpdateStatusReview(tsPaketModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketupdatestatusreview", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", BethaCrypto.Decrypt(ParamD.opd));
                    sqlCommand.Parameters.AddWithValue("_tahun", int.Parse(TAHUN));
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

        public DatabaseActionResultModel Remove(string ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tspaketremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD);
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
