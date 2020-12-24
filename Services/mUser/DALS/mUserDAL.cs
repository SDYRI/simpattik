using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;

namespace TasikmalayaKota.Simpatik.Web.Services.mUser.DALS
{
    public class mUserDAL : ImUser
    {
        private readonly string ConnectionString;
        private readonly string UID;
        public mUserDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
        }

        public IList<mUserModel> GetAll()
        {
            List<mUserModel> Result = new List<mUserModel>();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_musergetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Add(new mUserModel()
                            {
                                //IdUser = (dataReader["iduser"].GetType() != typeof(DBNull) ? (int)dataReader["iduser"] : 0),
                                IdUser = (dataReader["iduser"].GetType() != typeof(DBNull) ? (string)dataReader["iduser"] : ""),
                                NamaUser = (dataReader["namauser"].GetType() != typeof(DBNull) ? (string)dataReader["namauser"] : ""),
                                NipUser = (dataReader["nipuser"].GetType() != typeof(DBNull) ? (string)dataReader["nipuser"] : ""),
                                JabatanUser = (dataReader["jabatanuser"].GetType() != typeof(DBNull) ? (string)dataReader["jabatanuser"] : ""),
                                GolonganUser = (dataReader["golonganuser"].GetType() != typeof(DBNull) ? (string)dataReader["golonganuser"] : ""),
                                UserName = (dataReader["username"].GetType() != typeof(DBNull) ? (string)dataReader["username"] : ""),
                                //PasswordUser = (dataReader["passworduser"].GetType() != typeof(DBNull) ? (string)dataReader["passworduser"] : ""),
                                //SaltUser = (dataReader["saltuser"].GetType() != typeof(DBNull) ? (string)dataReader["saltuser"] : ""),
                                ListIdOpdUser = (dataReader["listidopduser"].GetType() != typeof(DBNull) ? (string)dataReader["listidopduser"] : ""),
                                ListOpdUser = (dataReader["listopduser"].GetType() != typeof(DBNull) ? (string)dataReader["listopduser"] : ""),
                                //TanggalDibuat = (dataReader["tanggaldibuat"].GetType() != typeof(DBNull) ? (DateTime?)dataReader["tanggaldibuat"] : null),
                                //TanggalModifikasi = (dataReader["tanggalmodifikasi"].GetType() != typeof(DBNull) ? (DateTime?)dataReader["tanggalmodifikasi"] : null),
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

        public UserValidationResultModel UserValidation(UserValidationArgsModel ParamD)
        {
            UserValidationResultModel Result = new UserValidationResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_validasiuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_username", ParamD.UserName);
                    sqlCommand.Parameters.AddWithValue("_password", ParamD.Password);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.IDAkun = dataReader["iduser"].ToString();
                            Result.Nama = dataReader["namauser"].ToString();
                            Result.Nama = dataReader["namauser"].ToString();
                            Result.Nip = dataReader["nipuser"].ToString();
                            Result.Jabatan = dataReader["jabatanuser"].ToString();
                            Result.Golongan = dataReader["golonganuser"].ToString();
                            Result.Tipe = Int16.Parse(dataReader["type"].ToString());
                            Result.Opd = dataReader["listidopduser"].ToString();
                            Result.OpdName = dataReader["listopduser"].ToString();
                            Result.TahunAktif = dataReader["tahunaktif"].ToString();
                            Result.Success = Result.IDAkun != string.Empty ? true : false;
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

        public DatabaseActionResultModel Create(mUserModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_loginuser", ParamD.UserName);
                    sqlCommand.Parameters.AddWithValue("_namauser", ParamD.NamaUser);
                    sqlCommand.Parameters.AddWithValue("_nipuser", ParamD.NipUser);
                    sqlCommand.Parameters.AddWithValue("_jabatanuser", ParamD.JabatanUser);
                    sqlCommand.Parameters.AddWithValue("_golonganuser", ParamD.GolonganUser);
                    sqlCommand.Parameters.AddWithValue("_passworduser", ParamD.PasswordUser == null ? "BPBJ123" : ParamD.PasswordUser);
                    sqlCommand.Parameters.AddWithValue("_idopd", ParamD.ListIdOpdUser);
                    sqlCommand.Parameters.AddWithValue("_tipe", 4);
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Kode = dataReader["kode"].ToString();
                            Result.Success = Result.Kode == "01" ? true : false;
                            Result.Pesan = dataReader["message"].ToString();
                            Result.Data = dataReader["opduser"].ToString();
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

        public DatabaseActionResultModel Update(mUserModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserupdate", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iduser", ParamD.IdUser);
                    sqlCommand.Parameters.AddWithValue("_loginuser", ParamD.UserName);
                    sqlCommand.Parameters.AddWithValue("_namauser", ParamD.NamaUser);
                    sqlCommand.Parameters.AddWithValue("_nipuser", ParamD.NipUser);
                    sqlCommand.Parameters.AddWithValue("_jabatanuser", ParamD.JabatanUser);
                    sqlCommand.Parameters.AddWithValue("_golonganuser", ParamD.GolonganUser);
                    sqlCommand.Parameters.AddWithValue("_passworduser", ParamD.PasswordUser == null ? "BPBJ123" : ParamD.PasswordUser);
                    sqlCommand.Parameters.AddWithValue("_idopd", ParamD.ListIdOpdUser);
                    sqlCommand.Parameters.AddWithValue("_uid", UID);
                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.Kode = dataReader["kode"].ToString();
                            Result.Success = Result.Kode == "01" ? true : false;
                            Result.Pesan = dataReader["message"].ToString();
                            Result.Data = dataReader["opduser"].ToString();
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
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserremove", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iduser", ParamD);
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
