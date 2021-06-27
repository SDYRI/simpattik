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
        private readonly string TA;
        public tHpsDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TA = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
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
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
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

        public IList<tHpsReviewModel> GetReviewAll(string idPaket)
        {
            List<tHpsReviewModel> Result = new List<tHpsReviewModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsreviewgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idpaket", idPaket != null ? idPaket : "0");
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new tHpsReviewModel()
                            {
                                idrvwhps = (dataReader["ridrvwhps"].GetType() != typeof(DBNull) ? (int)dataReader["ridrvwhps"] : 0),
                                idssh = (dataReader["ridssh"].GetType() != typeof(DBNull) ? (string)dataReader["ridssh"] : ""),
                                namassh = (dataReader["rnamassh"].GetType() != typeof(DBNull) ? (string)dataReader["rnamassh"] : ""),
                                volume = (dataReader["rvolume"].GetType() != typeof(DBNull) ? (int)dataReader["rvolume"] : 0),
                                pagu = (dataReader["rpagu"].GetType() != typeof(DBNull) ? (int)dataReader["rpagu"] : 0),
                                nilaissh = (dataReader["rnilaissh"].GetType() != typeof(DBNull) ? (int)dataReader["rnilaissh"] : 0),
                                hrgpasar = (dataReader["rhrgpasar"].GetType() != typeof(DBNull) ? (int)dataReader["rhrgpasar"] : 0),
                                slshpagu = (dataReader["rslshpagu"].GetType() != typeof(DBNull) ? (int)dataReader["rslshpagu"] : 0),
                                slshvolume = (dataReader["rslshvolume"].GetType() != typeof(DBNull) ? (int)dataReader["rslshvolume"] : 0),
                                jenisssh = (dataReader["rjenisssh"].GetType() != typeof(DBNull) ? (string)dataReader["rjenisssh"] : ""),
                                idpaket = (dataReader["ridpaket"].GetType() != typeof(DBNull) ? (string)dataReader["ridpaket"] : ""),
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

        public IList<mSshModel> SshGetAll()
        {
            List<mSshModel> Result = new List<mSshModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_msshgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mSshModel()
                            {
                                typessh = (dataReader["typessh"].GetType() != typeof(DBNull) ? (int)dataReader["typessh"] : 0),
                                idssh = (dataReader["idssh"].GetType() != typeof(DBNull) ? (int)dataReader["idssh"] : 0),
                                namassh = (dataReader["namassh"].GetType() != typeof(DBNull) ? (string)dataReader["namassh"] : ""),
                                hargassh = (dataReader["hargassh"].GetType() != typeof(DBNull) ? (Int64)dataReader["hargassh"] : 0),
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

        #region fileHPS
        public DatabaseActionResultModel Create(tHpsModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsstrategisinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    sqlCommand.Parameters.AddWithValue("_thanggrn", TA);
                    sqlCommand.Parameters.AddWithValue("_uraianpekerjaan", ParamD.uraianpekerjaan == null ? string.Empty : ParamD.uraianpekerjaan);
                    //sqlCommand.Parameters.AddWithValue("_nomatapembayaran", ParamD.nomatapembayaran == null ? string.Empty : ParamD.nomatapembayaran);
                    //sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    //sqlCommand.Parameters.AddWithValue("_satuan", ParamD.satuan == null ? string.Empty : ParamD.satuan);
                    //sqlCommand.Parameters.AddWithValue("_pajak", ParamD.pajak == null ? string.Empty : ParamD.pajak);
                    sqlCommand.Parameters.AddWithValue("_jumlahharga", ParamD.jumlahharga == null ? string.Empty : ParamD.jumlahharga);
                    //sqlCommand.Parameters.AddWithValue("_harga", ParamD.harga == null ? string.Empty : ParamD.harga);
                    //sqlCommand.Parameters.AddWithValue("_hargassh", ParamD.hargassh == null ? string.Empty : ParamD.hargassh);
                    //sqlCommand.Parameters.AddWithValue("_keteranganhps", ParamD.keteranganhps == null ? string.Empty : ParamD.keteranganhps);
                    //sqlCommand.Parameters.AddWithValue("_selisihpagu", ParamD.selisihpagu == null ? string.Empty : ParamD.selisihpagu);
                    //sqlCommand.Parameters.AddWithValue("_selisihvolume", ParamD.selisihvolume == null ? string.Empty : ParamD.selisihvolume);
                    sqlCommand.Parameters.AddWithValue("_filehps", ParamD.filehps == null ? string.Empty : ParamD.filehps);
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket == null ? string.Empty : ParamD.idpaket);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsstrategisupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idhps", ParamD.idhps);
                    //sqlCommand.Parameters.AddWithValue("_lembaga", "1");
                    //sqlCommand.Parameters.AddWithValue("_opd", OPD);
                    //sqlCommand.Parameters.AddWithValue("_pejabat", ParamD.pejabat == null ? string.Empty : ParamD.pejabat);
                    //sqlCommand.Parameters.AddWithValue("_thanggrn", ParamD.thanggrn == null ? string.Empty : ParamD.thanggrn);
                    sqlCommand.Parameters.AddWithValue("_uraianpekerjaan", ParamD.uraianpekerjaan == null ? string.Empty : ParamD.uraianpekerjaan);
                    //sqlCommand.Parameters.AddWithValue("_nomatapembayaran", ParamD.nomatapembayaran == null ? string.Empty : ParamD.nomatapembayaran);
                    //sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == null ? string.Empty : ParamD.volume);
                    //sqlCommand.Parameters.AddWithValue("_satuan", ParamD.satuan == null ? string.Empty : ParamD.satuan);
                    //sqlCommand.Parameters.AddWithValue("_pajak", ParamD.pajak == null ? string.Empty : ParamD.pajak);
                    sqlCommand.Parameters.AddWithValue("_jumlahharga", ParamD.jumlahharga == null ? string.Empty : ParamD.jumlahharga);
                    //sqlCommand.Parameters.AddWithValue("_harga", ParamD.harga == null ? string.Empty : ParamD.harga);
                    //sqlCommand.Parameters.AddWithValue("_hargassh", ParamD.hargassh == null ? string.Empty : ParamD.hargassh);
                    //sqlCommand.Parameters.AddWithValue("_keteranganhps", ParamD.keteranganhps == null ? string.Empty : ParamD.keteranganhps);
                    //sqlCommand.Parameters.AddWithValue("_selisihpagu", ParamD.selisihpagu == null ? string.Empty : ParamD.selisihpagu);
                    //sqlCommand.Parameters.AddWithValue("_selisihvolume", ParamD.selisihvolume == null ? string.Empty : ParamD.selisihvolume);
                    sqlCommand.Parameters.AddWithValue("_filehps", ParamD.filehps == null ? string.Empty : ParamD.filehps);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tshpsstrategisremove", sqlConnection))
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
        #endregion fileHPS

        #region reviewHPS
        public DatabaseActionResultModel Create(tHpsReviewModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsreviewhpsstrategisinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idpaket", ParamD.idpaket == null ? string.Empty : ParamD.idpaket);
                    sqlCommand.Parameters.AddWithValue("_tipessh", ParamD.jenisssh == null ? "1" : ParamD.jenisssh);
                    sqlCommand.Parameters.AddWithValue("_idssh", ParamD.idssh == null ? "1" : ParamD.idssh);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == 0 ? 0 : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_pagu", ParamD.pagu == 0 ? 0: ParamD.pagu);
                    sqlCommand.Parameters.AddWithValue("_hargapasar", ParamD.hrgpasar == 0 ? 0 : ParamD.hrgpasar);
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

        public DatabaseActionResultModel Update(tHpsReviewModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsreviewhpsstrategisupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idrvwhps", ParamD.idrvwhps);
                    sqlCommand.Parameters.AddWithValue("_tipessh", ParamD.jenisssh == null ? "1" : ParamD.jenisssh);
                    sqlCommand.Parameters.AddWithValue("_idssh", ParamD.idssh == null ? "1" : ParamD.idssh);
                    sqlCommand.Parameters.AddWithValue("_volume", ParamD.volume == 0 ? 0 : ParamD.volume);
                    sqlCommand.Parameters.AddWithValue("_pagu", ParamD.pagu == 0 ? 0 : ParamD.pagu);
                    sqlCommand.Parameters.AddWithValue("_hargapasar", ParamD.hrgpasar == 0 ? 0 : ParamD.hrgpasar);
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

        public DatabaseActionResultModel RemoveReview(string ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_tsreviewhpsstrategisremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_idrvwhps", ParamD);
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
        #endregion ReviewHPS

        public mSshModelJson GetDetailJson(int tipe, int id)
        {
            mSshModelJson Result = new mSshModelJson();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_msshbyid", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_tipe", tipe);
                    sqlCommand.Parameters.AddWithValue("_id", id);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.hargassh = (Int64)dataReader["hargassh"];
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
