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
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using System.Text;
using System.Security.Cryptography;
using Syncfusion.EJ2.Navigations;
namespace TasikmalayaKota.Simpatik.Web.Services.mUser.DALS
{
    public class mUserDAL : ImUser
    {
        private readonly string ConnectionString;
        private readonly string PAPPK;
        private readonly string UID;
        private readonly string OPD;
        private readonly string TAHUN;
        private readonly string Folder;
        public mUserDAL(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            ConnectionString = configuration.GetConnectionString("SimpatikConnection");
            PAPPK = httpContextAccessor.HttpContext.Session.GetString("Pappk");
            UID = httpContextAccessor.HttpContext.Session.GetString("IDAkun");
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TAHUN = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
        }

        public IList<mUserModel> GetAll()
        {
            List<mUserModel> Result = new List<mUserModel>();
            List<enumDataModel> tipeUser = new enumDataModel().TipeUser();
            List<enumDataModel> pappkUser = new enumDataModel().PaPpk();

            Int32 _pappk = 3;
            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_musergetall", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtopd", PAPPK == "0" ? "0" : OPD);
                    sqlCommand.Parameters.AddWithValue("_pappk", Int32.Parse(PAPPK) == 0 ? 0 : _pappk);

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
                                TipeIdUser = (dataReader["typeuser"].GetType() != typeof(DBNull) ? (int)dataReader["typeuser"] : 0),
                                TipeUser = tipeUser.FirstOrDefault(x => x.Value == ((int)dataReader["typeuser"]).ToString())?.Text,
                                PappkIdUser = (dataReader["pappkuser"].GetType() != typeof(DBNull) ? (int)dataReader["pappkuser"] : 0),
                                PappkUser = pappkUser.FirstOrDefault(x => x.Value == ((int)dataReader["pappkuser"]).ToString())?.Text,
                                JabatanUser = (dataReader["jabatanuser"].GetType() != typeof(DBNull) ? (string)dataReader["jabatanuser"] : ""),
                                GolonganUser = (dataReader["golonganuser"].GetType() != typeof(DBNull) ? (string)dataReader["golonganuser"] : ""),
                                UserName = (dataReader["username"].GetType() != typeof(DBNull) ? (string)dataReader["username"] : ""),
                                FileSK = (dataReader["filesk"].GetType() != typeof(DBNull) ? (string)dataReader["filesk"] : ""),
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

        public mUserModel GetById()
        {
            mUserModel Result = new mUserModel();

            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserbyid", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iddtuser", UID);

                    sqlConnection.Open();
                    using (NpgsqlDataReader dataReader = sqlCommand.ExecuteReader())
                    {
                        while (dataReader.Read())
                        {
                            Result.NamaUser = (!(dataReader["namauser"] is DBNull) ? (string)dataReader["namauser"] : "");
                            Result.NipUser = (!(dataReader["nipuser"] is DBNull) ? (string)dataReader["nipuser"] : "");
                            Result.JabatanUser = (!(dataReader["jabatanuser"] is DBNull) ? (string)dataReader["jabatanuser"] : "");
                            Result.GolonganUser = (!(dataReader["golonganuser"] is DBNull) ? (string)dataReader["golonganuser"] : "");
                            Result.UserName = (!(dataReader["username"] is DBNull) ? (string)dataReader["username"] : "");
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
            var _value = string.Empty;
            string hash = string.Empty;

            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_saltuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_uid", ParamD.UserName);
                    sqlConnection.Open();
                    _value = sqlCommand.ExecuteScalar().ToString();
                }

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    //From String to byte array
                    byte[] sourceBytes = Encoding.UTF8.GetBytes("simpattik" + ParamD.UserName + "tasikmalayakota" + ParamD.Password + "bethasolution" + _value);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_validasiuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_username", ParamD.UserName);
                    sqlCommand.Parameters.AddWithValue("_password", hash);
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
                            Result.Urusan = dataReader["listurusanuser"].ToString();
                            Result.Opd = dataReader["listidopduser"].ToString();
                            Result.OpdName = dataReader["listopduser"].ToString();
                            Result.TahunAktif = dataReader["tahunaktif"].ToString();
                            Result.UserAktif = (bool)dataReader["useraktif"];
                            Result.PaPpk = dataReader["pappkuser"].ToString();
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
            var _value = string.Empty;
            string hash = string.Empty;

            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_saltuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_uid", "0");
                    sqlConnection.Open();
                    _value = sqlCommand.ExecuteScalar().ToString();
                }

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    //From String to byte array
                    byte[] sourceBytes = Encoding.UTF8.GetBytes("simpattik" + ParamD.UserName + "tasikmalayakota" + ParamD.PasswordUser + "bethasolution" + _value);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserinsert", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_loginuser", ParamD.UserName);
                    sqlCommand.Parameters.AddWithValue("_namauser", ParamD.NamaUser);
                    sqlCommand.Parameters.AddWithValue("_nipuser", ParamD.NipUser);
                    sqlCommand.Parameters.AddWithValue("_jabatanuser", ParamD.JabatanUser);
                    sqlCommand.Parameters.AddWithValue("_golonganuser", ParamD.GolonganUser);
                    sqlCommand.Parameters.AddWithValue("_saltuser", _value);
                    sqlCommand.Parameters.AddWithValue("_passworduser", ParamD.PasswordUser == null ? "BPBJ123" : hash);
                    sqlCommand.Parameters.AddWithValue("_idopd", ParamD.ListIdOpdUser);
                    sqlCommand.Parameters.AddWithValue("_tipe", ParamD.TipeIdUser);
                    sqlCommand.Parameters.AddWithValue("_pappk", ParamD.PappkIdUser);
                    sqlCommand.Parameters.AddWithValue("_filesk", ParamD.FileSK);
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
            var _value = string.Empty;
            string hash = string.Empty;

            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_saltuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_uid", "0");
                    sqlConnection.Open();
                    _value = sqlCommand.ExecuteScalar().ToString();
                }

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    //From String to byte array
                    byte[] sourceBytes = Encoding.UTF8.GetBytes("simpattik" + ParamD.UserName + "tasikmalayakota" + ParamD.PasswordUser + "bethasolution" + _value);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }

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
                    sqlCommand.Parameters.AddWithValue("_saltuser", ParamD.PasswordUser == null ? "BPBJ123" : _value);
                    sqlCommand.Parameters.AddWithValue("_passworduser", ParamD.PasswordUser == null ? "BPBJ123" : hash);
                    sqlCommand.Parameters.AddWithValue("_idopd", ParamD.ListIdOpdUser);
                    sqlCommand.Parameters.AddWithValue("_tipe", ParamD.TipeIdUser);
                    sqlCommand.Parameters.AddWithValue("_pappk", ParamD.PappkIdUser);
                    sqlCommand.Parameters.AddWithValue("_filesk", ParamD.FileSK);
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

        public DatabaseActionResultModel UpdateProfile(mUserModel ParamD)
        {
            DatabaseActionResultModel Result = new DatabaseActionResultModel();
            var _value = string.Empty;
            string hash = string.Empty;

            try
            {
                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_saltuser", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_uid", "0");
                    sqlConnection.Open();
                    _value = sqlCommand.ExecuteScalar().ToString();
                }

                using (SHA256 sha256Hash = SHA256.Create())
                {
                    //From String to byte array
                    byte[] sourceBytes = Encoding.UTF8.GetBytes("simpattik" + ParamD.UserName + "tasikmalayakota" + ParamD.PasswordUser + "bethasolution" + _value);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty);
                }

                using (NpgsqlConnection sqlConnection = new NpgsqlConnection(ConnectionString))
                using (NpgsqlCommand sqlCommand = new NpgsqlCommand("public.stp_muserupdateprofile", sqlConnection))
                {
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.Parameters.AddWithValue("_iduser", UID);
                    sqlCommand.Parameters.AddWithValue("_saltuser", ParamD.PasswordUser == null ? "BPBJ123" : _value);
                    sqlCommand.Parameters.AddWithValue("_passworduser", ParamD.PasswordUser == null ? "BPBJ123" : hash);
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
