using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Services.mOpd.DALS
{
    public class mOpdDAL : ImOpd
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string OPD;
        private readonly string TAHUN;
        public mOpdDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TAHUN = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
        }

        public IList<mOpdModel> GetAll(int posisi)
        {
            List<mOpdModel> Result = new List<mOpdModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mopdgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_posisi", posisi);
                    sqlCommand.Parameters.AddWithValue("_idurusan", "0");

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mOpdModel()
                            {
                                IdOpd = (dataReader["idopd"].GetType() != typeof(DBNull) ? (int)dataReader["idopd"] : 0),
                                KodeOpd = (dataReader["kodeopd"].GetType() != typeof(DBNull) ? (string)dataReader["kodeopd"] : ""),
                                KodeSubOpd = (dataReader["kodesubopd"].GetType() != typeof(DBNull) ? (string)dataReader["kodesubopd"] : ""),
                                NamaOpd = (dataReader["namaopd"].GetType() != typeof(DBNull) ? (string)dataReader["namaopd"] : ""),
                                NamaSubOpd = (dataReader["namasubopd"].GetType() != typeof(DBNull) ? (string)dataReader["namasubopd"] : ""),
                                ListIdUrusan = (dataReader["listidurusanopd"].GetType() != typeof(DBNull) ? dataReader["listidurusanopd"].ToString().Split(",").Select(s => int.Parse(s)).ToList() : null),
                                ListIdUrusanCb = (dataReader["listidurusanopd"].GetType() != typeof(DBNull) ? (string)dataReader["listidurusanopd"] : ""),
                                ListUrusan = (dataReader["listurusanopd"].GetType() != typeof(DBNull) ? (string)dataReader["listurusanopd"] : ""),
                                IdParent = (dataReader["idparent"].GetType() != typeof(DBNull) ? (int)dataReader["idparent"] : 0),
                                IdPosisi = (dataReader["idposisi"].GetType() != typeof(DBNull) ? (int)dataReader["idposisi"] : 0),
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

        public IList<mOpdModel> GetAllUrusan(int posisi, string idurusan)
        {
            List<mOpdModel> Result = new List<mOpdModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mopdgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_posisi", posisi);
                    sqlCommand.Parameters.AddWithValue("_idurusan", idurusan == null ? "0" : idurusan);

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mOpdModel()
                            {
                                IdOpd = (dataReader["idopd"].GetType() != typeof(DBNull) ? (int)dataReader["idopd"] : 0),
                                KodeOpd = (dataReader["kodeopd"].GetType() != typeof(DBNull) ? (string)dataReader["kodeopd"] : ""),
                                KodeSubOpd = (dataReader["kodesubopd"].GetType() != typeof(DBNull) ? (string)dataReader["kodesubopd"] : ""),
                                NamaOpd = (dataReader["namaopd"].GetType() != typeof(DBNull) ? (string)dataReader["namaopd"] : ""),
                                NamaSubOpd = (dataReader["namasubopd"].GetType() != typeof(DBNull) ? (string)dataReader["namasubopd"] : ""),
                                ListIdUrusan = (dataReader["listidurusanopd"].GetType() != typeof(DBNull) ? dataReader["listidurusanopd"].ToString().Split(",").Select(s => int.Parse(s)).ToList() : null),
                                ListIdUrusanCb = (dataReader["listidurusanopd"].GetType() != typeof(DBNull) ? (string)dataReader["listidurusanopd"] : ""),
                                ListUrusan = (dataReader["listurusanopd"].GetType() != typeof(DBNull) ? (string)dataReader["listurusanopd"] : ""),
                                IdParent = (dataReader["idparent"].GetType() != typeof(DBNull) ? (int)dataReader["idparent"] : 0),
                                IdPosisi = (dataReader["idposisi"].GetType() != typeof(DBNull) ? (int)dataReader["idposisi"] : 0),
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

        public DatabaseActionResultModel Create(mOpdModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mopdinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_nmdtopd", ParamD.NamaSubOpd);
                    sqlCommand.Parameters.AddWithValue("_idpropd", ParamD.IdParent == 0 ? ParamD.IdParentU : ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_ktkdopd", ParamD.KodeSubOpd);
                    sqlCommand.Parameters.AddWithValue("_posisi", ParamD.IdPosisi);
                    sqlCommand.Parameters.AddWithValue("_idurusan", ParamD.ListIdUrusanCb);
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

        public DatabaseActionResultModel Update(mOpdModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mopdupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtopd", ParamD.IdOpd);
                    sqlCommand.Parameters.AddWithValue("_nmdtopd", ParamD.NamaSubOpd);
                    sqlCommand.Parameters.AddWithValue("_idpropd", ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_ktkdopd", ParamD.KodeSubOpd);
                    sqlCommand.Parameters.AddWithValue("_posisi", ParamD.IdPosisi);
                    sqlCommand.Parameters.AddWithValue("_idurusan", ParamD.ListIdUrusanCb);
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

        public DatabaseActionResultModel Remove(int ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mopdremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtopd", ParamD);
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
