using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUrusan.DALS
{
    public class mUrusanDAL : ImUrusan
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string URUSAN;
        private readonly string OPD;
        private readonly string TAHUN;
        public mUrusanDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            URUSAN = httpContextAccessor.HttpContext.Session.GetString("Urusan");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TAHUN = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
        }

        public IList<mUrusanModel> GetAll(int posisi)
        {
            List<mUrusanModel> Result = new List<mUrusanModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_murusangetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtursn", URUSAN);
                    sqlCommand.Parameters.AddWithValue("_posisi", posisi);

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mUrusanModel()
                            {
                                IdUrusan = (dataReader["idurusan"].GetType() != typeof(DBNull) ? (int)dataReader["idurusan"] : 0),
                                KodeUrusan = (dataReader["kodeurusan"].GetType() != typeof(DBNull) ? (string)dataReader["kodeurusan"] : ""),
                                KodeSubUrusan = (dataReader["kodesuburusan"].GetType() != typeof(DBNull) ? (string)dataReader["kodesuburusan"] : ""),
                                NamaUrusan = (dataReader["namaurusan"].GetType() != typeof(DBNull) ? (string)dataReader["namaurusan"] : ""),
                                NamaSubUrusan = (dataReader["namasuburusan"].GetType() != typeof(DBNull) ? (string)dataReader["namasuburusan"] : ""),
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

        public DatabaseActionResultModel Create(mUrusanModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_murusaninsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_nmdturs", ParamD.NamaSubUrusan);
                    sqlCommand.Parameters.AddWithValue("_idpdtpu", ParamD.IdParent == 0 ? ParamD.IdParentU : ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_kddturs", ParamD.KodeSubUrusan);
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

        public DatabaseActionResultModel Update(mUrusanModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_murusanupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddturs", ParamD.IdUrusan);
                    sqlCommand.Parameters.AddWithValue("_nmdturs", ParamD.NamaSubUrusan);
                    sqlCommand.Parameters.AddWithValue("_idpdtpu", ParamD.IdParent == 0 ? ParamD.IdParentU : ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_kddturs", ParamD.KodeSubUrusan);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_murusanremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddturs", ParamD);
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
