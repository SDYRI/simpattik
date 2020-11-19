using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Services.Middleware.DALS
{
    public class MiddlewareDAL : IMiddlewares
    {
        private readonly IConfiguration Configuration;
        private readonly string ConnectionString;
        
        public MiddlewareDAL(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = configuration.GetConnectionString("UserConnection");
        }

        public bool GetAksesAksiadditional(string Controller, string Aksi, string AksiTambahan, string UserId)
        {
            bool Result = true;
            //bool Result = false;
            //try
            //{
            //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            //    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[stp_ValidasiAksi]", sqlConnection))
            //    {
            //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@Controller", Controller);
            //        sqlCommand.Parameters.AddWithValue("@Aksi", Aksi);
            //        sqlCommand.Parameters.AddWithValue("@AksiTambahan", AksiTambahan);
            //        sqlCommand.Parameters.AddWithValue("@UserId", UserId);

            //        sqlConnection.Open();
            //        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                Result = (bool)dataReader[0];
            //            }
            //        }
            //    }
            //}
            //catch (Exception Exception)
            //{
            //    throw Exception;
            //}
            return Result;
        }

        public bool GetAksesAksi(string Controller, string Aksi, int Type)
        {
            bool Result = true;
            //bool Result = false;
            //try
            //{
            //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            //    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[stp_ValidasiAksi]", sqlConnection))
            //    {
            //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@Controller", Controller);
            //        sqlCommand.Parameters.AddWithValue("@Aksi", Aksi);
            //        sqlCommand.Parameters.AddWithValue("@AksiTambahan", null);
            //        sqlCommand.Parameters.AddWithValue("@Type", Type);

            //        sqlConnection.Open();
            //        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                Result = (bool)dataReader[0];
            //            }
            //        }
            //    }
            //}
            //catch (Exception Exception)
            //{
            //    throw Exception;
            //}
            return Result;
        }

        public MiddlewareModel GetControllerIndex(string _Param)
        {
            MiddlewareModel Result = new MiddlewareModel();
            //try
            //{
            //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            //    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[stp_GetControllerIndex]", sqlConnection))
            //    {
            //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@NamaIndex", _Param);
                    
            //        sqlConnection.Open();
            //        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                Result.NamaIndex = dataReader["NamaIndex"].ToString();
            //            }
            //        }
            //    }
            //}
            //catch (Exception Exception)
            //{
            //    throw Exception;
            //}
            return Result;
        }

        public MiddlewareUserModel GetUserController(int idAkun, string _Param)
        {
            MiddlewareUserModel Result = new MiddlewareUserModel();
            //try
            //{
            //    using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            //    using (SqlCommand sqlCommand = new SqlCommand("[dbo].[stp_GetUserController]", sqlConnection))
            //    {
            //        sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            //        sqlCommand.Parameters.AddWithValue("@IdAkun", idAkun);
            //        sqlCommand.Parameters.AddWithValue("@LinkUrl", _Param);

            //        sqlConnection.Open();
            //        using (SqlDataReader dataReader = sqlCommand.ExecuteReader())
            //        {
            //            while (dataReader.Read())
            //            {
            //                Result.Id = (int)dataReader["Id"];
            //                Result.IdAkun = (int)dataReader["IdAkun_FK"];
            //                Result.LinkUrl = dataReader["LinkUrl"].ToString();
            //            }
            //        }
            //    }
            //}
            //catch (Exception Exception)
            //{
            //    throw Exception;
            //}
            return Result;
        }
    }
}