using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using TasikmalayaKota.Simpatik.Web.Services.tHps.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tHps.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.tHps.DALS
{
    public class tHpsDAL : ItHps
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string OPD;
        public tHpsDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
        }

        public IList<tHpsModel> GetAll()
        {
            List<tHpsModel> Result = new List<tHpsModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tHpsModel()
                            {
                                idhps = (dataReader["ridhps"].GetType() != typeof(DBNull) ? (string)dataReader["ridhps"] : ""),
                                lembaga = (dataReader["rlembaga"].GetType() != typeof(DBNull) ? ((int)dataReader["rlembaga"]).ToString() : ""),
                                opd = (dataReader["ropd"].GetType() != typeof(DBNull) ? ((int)dataReader["ropd"]).ToString() : ""),
                                namaopd = (dataReader["rnamaopd"].GetType() != typeof(DBNull) ? (string)dataReader["rnamaopd"] : ""),
                                pejabat = (dataReader["rpejabat"].GetType() != typeof(DBNull) ? (string)dataReader["rpejabat"] : ""),
                                thanggrn = (dataReader["rthanggrn"].GetType() != typeof(DBNull) ? ((int)dataReader["rthanggrn"]).ToString() : ""),
                                uraianpekerjaan = (dataReader["ruraianpekerjaan"].GetType() != typeof(DBNull) ? (string)dataReader["ruraianpekerjaan"] : ""),
                                volume = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (string)dataReader["rvolume"] : ""),
                                nomatapembayaran = (dataReader["rnomatapembayaran"].GetType() != typeof(DBNull) ? (string)dataReader["rnomatapembayaran"] : ""),
                                satuan = (dataReader["rsatuan"].GetType() != typeof(DBNull) ? (string)dataReader["rsatuan"] : ""),
                                pajak = (dataReader["rpajak"].GetType() != typeof(DBNull) ? (string)dataReader["rpajak"] : ""),
                                jumlahharga = (dataReader["rjumlahharga"].GetType() != typeof(DBNull) ? (string)dataReader["rjumlahharga"] : ""),
                                keteranganhps = (dataReader["rketeranganhps"].GetType() != typeof(DBNull) ? (string)dataReader["rketeranganhps"] : ""),
                                namapaket = (dataReader["rnamapaket"].GetType() != typeof(DBNull) ? (string)dataReader["rnamapaket"] : ""),
                                filehps = (dataReader["rfilehps"].GetType() != typeof(DBNull) ? (string)dataReader["rfilehps"] : ""),
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

        public DatabaseActionResultModel Create(tHpsModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_thanggrn", ParamD.thanggrn == null ? string.Empty : ParamD.thanggrn);
                    sqlCommand.Parameters.AddWithValue("_uraianpekerjaan", ParamD.uraianpekerjaan == null ? string.Empty : ParamD.uraianpekerjaan);
                    sqlCommand.Parameters.AddWithValue("_nomatapembayaran", ParamD.nomatapembayaran == null ? string.Empty : ParamD.nomatapembayaran);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_satuan", ParamD.satuan == null ? string.Empty : ParamD.satuan);
                    sqlCommand.Parameters.AddWithValue("_pajak", ParamD.pajak == null ? string.Empty : ParamD.pajak);
                    sqlCommand.Parameters.AddWithValue("_jumlahharga", ParamD.jumlahharga == null ? string.Empty : ParamD.jumlahharga);
                    sqlCommand.Parameters.AddWithValue("_harga", ParamD.harga == null ? string.Empty : ParamD.harga);
                    sqlCommand.Parameters.AddWithValue("_hargassh", ParamD.hargassh == null ? string.Empty : ParamD.hargassh);
                    sqlCommand.Parameters.AddWithValue("_keteranganhps", ParamD.keteranganhps == null ? string.Empty : ParamD.keteranganhps);
                    sqlCommand.Parameters.AddWithValue("_selisihpagu", ParamD.selisihpagu == null ? string.Empty : ParamD.selisihpagu);
                    sqlCommand.Parameters.AddWithValue("_selisihvolume", ParamD.selisihvolume == null ? string.Empty : ParamD.selisihvolume);
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

        public DatabaseActionResultModel Update(tHpsModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idhps", ParamD.idhps);
                    sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_thanggrn", ParamD.thanggrn == null ? string.Empty : ParamD.thanggrn);
                    sqlCommand.Parameters.AddWithValue("_uraianpekerjaan", ParamD.uraianpekerjaan == null ? string.Empty : ParamD.uraianpekerjaan);
                    sqlCommand.Parameters.AddWithValue("_nomatapembayaran", ParamD.nomatapembayaran == null ? string.Empty : ParamD.nomatapembayaran);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_satuan", ParamD.satuan == null ? string.Empty : ParamD.satuan);
                    sqlCommand.Parameters.AddWithValue("_pajak", ParamD.pajak == null ? string.Empty : ParamD.pajak);
                    sqlCommand.Parameters.AddWithValue("_jumlahharga", ParamD.jumlahharga == null ? string.Empty : ParamD.jumlahharga);
                    sqlCommand.Parameters.AddWithValue("_harga", ParamD.harga == null ? string.Empty : ParamD.harga);
                    sqlCommand.Parameters.AddWithValue("_hargassh", ParamD.hargassh == null ? string.Empty : ParamD.hargassh);
                    sqlCommand.Parameters.AddWithValue("_keteranganhps", ParamD.keteranganhps == null ? string.Empty : ParamD.keteranganhps);
                    sqlCommand.Parameters.AddWithValue("_selisihpagu", ParamD.selisihpagu == null ? string.Empty : ParamD.selisihpagu);
                    sqlCommand.Parameters.AddWithValue("_selisihvolume", ParamD.selisihvolume == null ? string.Empty : ParamD.selisihvolume);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idhps", ParamD);
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

        public DatabaseActionResultModel GetDetailJson()
        {
            //UserModel Model = GetUserByID(ID, 1);

            DatabaseActionResultModel Result = new DatabaseActionResultModel();

            Result.Success = true;
            Result.Pesan = "Get Detail";
            //Result.Payload = Model;

            return Result;
        }
    }
}
