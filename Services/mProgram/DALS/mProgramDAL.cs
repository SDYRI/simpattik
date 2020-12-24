using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mProgram.DALS
{
    public class mProgramDAL : ImProgram
    {
        private readonly string ConnectionString;
        private readonly string UID;
        private readonly string OPD;
        public mProgramDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
        }

        public IList<mProgramModel> GetAll(int posisi)
        {
            List<mProgramModel> Result = new List<mProgramModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mprogramgetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtursn", "0");
                    sqlCommand.Parameters.AddWithValue("_posisi", posisi);

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mProgramModel()
                            {
                                IdUrusan = (dataReader["idursn"].GetType() != typeof(DBNull) ? (int)dataReader["idursn"] : 0),
                                NamaUrusan = (dataReader["namaurusan"].GetType() != typeof(DBNull) ? (string)dataReader["namaurusan"] : ""),
                                IdProgram = (dataReader["idprogram"].GetType() != typeof(DBNull) ? (int)dataReader["idprogram"] : 0),
                                KodeProgram = (dataReader["kodeprogram"].GetType() != typeof(DBNull) ? (string)dataReader["kodeprogram"] : ""),
                                KodeKegiatan = (dataReader["kodekegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["kodekegiatan"] : ""),
                                KodeSubkegiatan = (dataReader["kodesubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["kodesubkegiatan"] : ""),
                                NamaProgram = (dataReader["namaprogram"].GetType() != typeof(DBNull) ? (string)dataReader["namaprogram"] : ""),
                                NamaKegiatan = (dataReader["namakegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["namakegiatan"] : ""),
                                NamaSubkegiatan = (dataReader["namasubkegiatan"].GetType() != typeof(DBNull) ? (string)dataReader["namasubkegiatan"] : ""),
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

        public DatabaseActionResultModel Create(mProgramModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mprograminsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtopd", ParamD.IdUrusan);
                    sqlCommand.Parameters.AddWithValue("_nmdtprg", ParamD.NamaSubkegiatan);
                    sqlCommand.Parameters.AddWithValue("_idpdtpg", ParamD.IdParent == 0 ? ParamD.IdParentU : ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_kddtprg", ParamD.KodeSubkegiatan);
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

        public DatabaseActionResultModel Update(mProgramModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mprogramupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtprg", ParamD.IdProgram);
                    sqlCommand.Parameters.AddWithValue("_iddtopd", ParamD.IdUrusan);
                    sqlCommand.Parameters.AddWithValue("_nmdtprg", ParamD.NamaSubkegiatan);
                    sqlCommand.Parameters.AddWithValue("_idpdtpg", ParamD.IdParent);
                    sqlCommand.Parameters.AddWithValue("_kddtprg", ParamD.KodeSubkegiatan);
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mprogramremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtprg", ParamD);
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
