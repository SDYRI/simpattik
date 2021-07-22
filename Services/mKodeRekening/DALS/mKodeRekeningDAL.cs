using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.DALS
{
    public class mKodeRekeningDAL : ImKodeRekening
    {
        private readonly string ConnectionString;
        private readonly string UID;
        public mKodeRekeningDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
        }

        public IList<mKodeRekeningModel> GetAll(int posisi)
        {
            List<mKodeRekeningModel> Result = new List<mKodeRekeningModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mrekeninggetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_posisi", posisi);

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mKodeRekeningModel()
                            {
                                IdKodeRekening = (dataReader["idrekening"].GetType() != typeof(DBNull) ? (int)dataReader["idrekening"] : 0),
                                KodeAkun = (dataReader["kodeakun"].GetType() != typeof(DBNull) ? (string)dataReader["kodeakun"] : ""),
                                KodeKelompok = (dataReader["kodekelompok"].GetType() != typeof(DBNull) ? (string)dataReader["kodekelompok"] : ""),
                                KodeJenis = (dataReader["kodejenis"].GetType() != typeof(DBNull) ? (string)dataReader["kodejenis"] : ""),
                                KodeObjek = (dataReader["kodeobjek"].GetType() != typeof(DBNull) ? (string)dataReader["kodeobjek"] : ""),
                                KodeRincian = (dataReader["koderincian"].GetType() != typeof(DBNull) ? (string)dataReader["koderincian"] : ""),
                                KodeSubRincian = (dataReader["kodesubrincian"].GetType() != typeof(DBNull) ? (string)dataReader["kodesubrincian"] : ""),
                                KodeRekening = (dataReader["koderekening"].GetType() != typeof(DBNull) ? (string)dataReader["koderekening"] : ""),
                                NamaAkun = (dataReader["namaakun"].GetType() != typeof(DBNull) ? (string)dataReader["namaakun"] : ""),
                                NamaKelompok = (dataReader["namakelompok"].GetType() != typeof(DBNull) ? (string)dataReader["namakelompok"] : ""),
                                NamaJenis = (dataReader["namajenis"].GetType() != typeof(DBNull) ? (string)dataReader["namajenis"] : ""),
                                NamaObjek = (dataReader["namaobjek"].GetType() != typeof(DBNull) ? (string)dataReader["namaobjek"] : ""),
                                NamaRincian = (dataReader["namarincian"].GetType() != typeof(DBNull) ? (string)dataReader["namarincian"] : ""),
                                NamaSubRincian = (dataReader["namasubrincian"].GetType() != typeof(DBNull) ? (string)dataReader["namasubrincian"] : ""),
                                ViewKodeRekening = (dataReader["viewkoderekening"].GetType() != typeof(DBNull) ? (string)dataReader["viewkoderekening"] : ""),
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

        public DatabaseActionResultModel Create(mKodeRekeningModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mrekeninginsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_nmdtrkg", ParamD.NamaSubRincian);
                    sqlCommand.Parameters.AddWithValue("_idprrkg", ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_ktkdrkg", ParamD.KodeSubRincian);
                    sqlCommand.Parameters.AddWithValue("_posisi", ParamD.IdPosisi);
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

        public DatabaseActionResultModel Update(mKodeRekeningModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mrekeningupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtrkg", ParamD.IdKodeRekening);
                    sqlCommand.Parameters.AddWithValue("_nmdtrkg", ParamD.NamaSubRincian);
                    sqlCommand.Parameters.AddWithValue("_idprrkg", ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_ktkdrkg", ParamD.KodeSubRincian);
                    sqlCommand.Parameters.AddWithValue("_posisi", ParamD.IdPosisi);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mrekeningremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtrkg", ParamD);
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
