using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.DALS
{
    public class mTahunAnggaranDAL : ImTahunAnggaran
    {
        private readonly string ConnectionString;
        private readonly string UID;
        public mTahunAnggaranDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
        }

        public IList<mTahunAnggaranModel> GetAll()
        {
            List<mTahunAnggaranModel> Result = new List<mTahunAnggaranModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mtahunanggarangetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mTahunAnggaranModel()
                            {
                                TahunAnggaran = (dataReader["tahunanggaran"].GetType() != typeof(DBNull) ? (int)dataReader["tahunanggaran"] : 0),
                                TahunAnggaranOld = (dataReader["tahunanggaran"].GetType() != typeof(DBNull) ? (int)dataReader["tahunanggaran"] : 0),
                                TahunAnggaranAktif = (dataReader["tahunaktif"].GetType() != typeof(DBNull) ? (bool)dataReader["tahunaktif"] : false)
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

        public DatabaseActionResultModel Create(mTahunAnggaranModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mtahunanggaraninsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_tahun", ParamD.TahunAnggaran);
                    sqlCommand.Parameters.AddWithValue("_aktif", ParamD.TahunAnggaranAktif);
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

        public DatabaseActionResultModel Update(mTahunAnggaranModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_mtahunanggaranupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_tahun", ParamD.TahunAnggaran);
                    sqlCommand.Parameters.AddWithValue("_tahunold", ParamD.TahunAnggaranOld);
                    sqlCommand.Parameters.AddWithValue("_aktif", ParamD.TahunAnggaranAktif);
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
