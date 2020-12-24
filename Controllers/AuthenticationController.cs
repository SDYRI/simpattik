using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ImUser Users;
        private readonly IWebHostEnvironment Env;
        public AuthenticationController(ImUser users, IWebHostEnvironment env)
        {
            Users = users;
            Env = env;
        }

        [Route("")]
        [Route("login")]
        public IActionResult Index()
        {
            //HttpContext.Session.Clear();
            var IDAkun = HttpContext.Session.GetString("IDAkun");
            if (IDAkun != null && IDAkun != string.Empty)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();
            }
        }
        [HttpPost("login/validate")]
        public IActionResult UserValidation(UserValidationArgsModel Args)
        {
            try
            {
                Args.Action = "login";
                UserValidationResultModel Result = Users.UserValidation(Args);
                List<MenuValidationResultModel> dataMenu = new List<MenuValidationResultModel>();

                if (Result.Success && Result.IDAkun != string.Empty)
                {
                    HttpContext.Session.SetString("IDAkun", Result.IDAkun);
                    HttpContext.Session.SetInt32("Tipe", Result.Tipe);
                    HttpContext.Session.SetString("Nama", Result.Nama);
                    HttpContext.Session.SetString("Nip", Result.Nip);
                    HttpContext.Session.SetString("Jabatan", Result.Jabatan);
                    HttpContext.Session.SetString("Golongan", Result.Golongan);
                    HttpContext.Session.SetString("Opd", Result.Opd);
                    HttpContext.Session.SetString("OpdName", Result.OpdName);
                    HttpContext.Session.SetString("TahunAktif", Result.TahunAktif);
                }

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }
        [HttpGet]
        [Route("login/get-data-login")]
        public JsonResult GetDataUser()
        {
            return Json(new
            {
                IDAkun = HttpContext.Session.GetString("IDAkun"),
                Nama = HttpContext.Session.GetString("Nama"),
                Nip = HttpContext.Session.GetString("Nip"),
                Jabatan = HttpContext.Session.GetString("Jabatan"),
                Golongan = HttpContext.Session.GetString("Golongan"),
                Tipe = HttpContext.Session.GetInt32("Tipe"),
            });
        }
        [HttpGet]
        [AllowAnonymous]
        [Route("logout")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Authentication");
        }
    }
}
