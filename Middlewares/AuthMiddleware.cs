using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using TasikmalayaKota.Simpatik.Web.Helpers;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate Next;
        private IMiddlewares Middleware;
        private IConfiguration Configuration { get; }

        public AuthMiddleware(RequestDelegate next, IMiddlewares middleware, IConfiguration configuration)
        {
            Next = next;
            Middleware = middleware;
            Configuration = configuration;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            //ByPassAuth(httpContext);
            SlugRedirect(httpContext);

            var path = httpContext.Request.Path;
            var isLogin = httpContext.Session.GetString("IDAkun") != null;
            var UserId = httpContext.Session.GetString("IDAkun");
            var isAjax = httpContext.Request.IsAjaxRequest();

            if (!path.HasValue || path.Value == "/")
            {
                if (isLogin)
                {
                    if (!isAjax)
                    {
                        httpContext.Response.Redirect("/home/index");
                    }
                }
                else
                {
                    if (!isAjax)
                    {
                        httpContext.Response.Redirect("/login");
                    }
                }
            }
            else if (path.Value.StartsWith("/login") || path.Value.StartsWith("/register") || path.Value.StartsWith("/reset-password"))
            {
                if (isLogin)
                {
                    if (!isAjax)
                    {
                        httpContext.Response.Redirect("/home/index");
                    }
                }
            }
            else
            {
                if (isLogin)
                {
                    if (!path.Value.StartsWith("/css") && !path.Value.StartsWith("/fonts") && !path.Value.StartsWith("/images") && !path.Value.StartsWith("/js") && !path.Value.StartsWith("/lib") && !path.Value.StartsWith("/vendors"))
                    {
                        // if (!Middleware.GetAksesAksiadditional(path.ToString().Substring(1).ToLower(), UserId))
                        // {
                        //     httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;
                        //     return;
                        // }
                    }
                }
                else
                {
                    httpContext.Response.Redirect("/login");
                    //if (!isAjax)
                    //{
                    //    httpContext.Response.Redirect("/login");
                    //}
                    //else
                    //{
                    //    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    //    return;
                    //}
                }
            }

            await Next(httpContext);
        }

        private void SlugRedirect(HttpContext httpContext)
        {
            var path = httpContext.Request.Path;
            var pathBase = httpContext.Request.PathBase.Value;
            string slug = Configuration.GetValue<string>("SimpatikSettings:WebsiteSLUG");

            slug = (slug == "") ? "" : "/" + slug;

            if (pathBase != slug)
            {
                httpContext.Response.Redirect(slug + path.Value);
            }
        }

        private void ByPassAuth(HttpContext httpContext)
        {
            if (httpContext.Session.GetString("IDAkun") == null && Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                httpContext.Session.SetString("IDAkun", "ed5cdf92-1bdb-11eb-8fc7-00d861e22061");
                httpContext.Session.SetInt32("Tipe", 0);
                httpContext.Session.SetString("Nama", "SDY_RI");
                httpContext.Session.SetString("Nip", "139");
                httpContext.Session.SetString("Jabatan", "SDY");
                httpContext.Session.SetString("Golongan", "SDY");
                httpContext.Session.SetString("Urusan", "3, 38, 37");
                httpContext.Session.SetString("Opd", "1, 43, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 37, 42, 36, 44, 38, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35");
                httpContext.Session.SetString("OpdName", "BADAN KEPEGAWAIAN PENDIDIKAN DAN PELATIHAN DAERAH, SEKRETARIAT DAERAH, BADAN KESATUAN BANGSA DAN POLITIK, BADAN PENANGGULANGAN BENCANA DAERAH, BADAN PENDAPATAN DAERAH, BADAN PENGELOLA KEUANGAN DAN ASET DAERAH, BADAN PENGELOLA PAJAK DAN RETRIBUSI DAERAH, BADAN PERENCANAAN PEMBANGUNAN PENELITIAN DAN PENGEMBANGAN DAERAH, DINAS KEPEMUDAAN OLAH RAGA KEBUDAYAAN DAN PARIWISATA, DINAS KEPENDUDUKAN DAN PENCATATAN SIPIL, DINAS KETAHANAN PANGAN PERTANIAN DAN PERIKANAN, DINAS KOMUNIKASI DAN INFORMATIKA, DINAS LINGKUNGAN HIDUP, SEKRETARIAT DPRD, Barang dan Jasa, SEKDA, Bagian Umum, DINAS KESEHATAN, DINAS KOPERASI USAHA MIKRO KECIL DAN MENENGAH PERINDUSTRIAN DAN PERDAGANGAN, DINAS PEKERJAAN UMUM DAN PENATAAN RUANG, DINAS PENANAMAN MODAL DAN PELAYANAN TERPADU SATU PINTU, DINAS PENDIDIKAN, DINAS PENGENDALIAN PENDUDUK KELUARGA BERENCANA PEMBERDAYAAN PEREMPUAN DAN PERLINDUNGAN ANAK, DINAS PERHUBUNGAN, DINAS PERPUSTAKAAN DAN KEARSIPAN DAERAH, DINAS PERUMAHAN RAKYAT DAN KAWASAN PERMUKIMAN, DINAS SOSIAL, DINAS TENAGA KERJA, INSPEKTORAT, KECAMATAN BUNGURSARI, KECAMATAN CIBEUREUM, KECAMATAN CIHIDEUNG, KECAMATAN CIPEDES, KECAMATAN INDIHIANG, KECAMATAN KAWALU, KECAMATAN MANGKUBUMI, KECAMATAN PURBARATU, KECAMATAN TAMANSARI, KECAMATAN TAWANG, RUMAH SAKIT UMUM DAERAH DR SOEKARDJO, SATUAN POLISI PAMONG PRAJA PEMADAM KEBAKARAN");
                httpContext.Session.SetString("TahunAktif", "2021");
                httpContext.Session.SetString("Pappk", "0");
                httpContext.Session.SetString("ActionAuthorise", "#@EMAIL.INDEX#@, #@HOME.INDEX#@, #@HOME.PRIVACY#@, #@USER.INDEX#@, #@USER.INDEX.POST#@, #@USER.EDIT#@, #@USER.EDIT.POST#@");

            }
        }
    }
}
