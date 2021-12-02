using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Extensions;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly ImUser Users;
        private readonly IWebHostEnvironment Env;
        private readonly ImTahunAnggaran _mTahunAnggaran;

        public AuthenticationController(ImUser users, IWebHostEnvironment env, ImTahunAnggaran mTahunAnggaran)
        {
            Users = users;
            Env = env;
            _mTahunAnggaran = mTahunAnggaran;
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
                string TahunUserAktif = _mTahunAnggaran.GetUserTahunAktif();
                //List<MenuValidationResultModel> dataMenu = new List<MenuValidationResultModel>();

                #region toolbar
                List<string> toolbar = (TahunUserAktif == "False" && Result.Tipe != 1) ? SetToolbarNonAktif() : SetToolbar(Result.Tipe, Result.PaPpk);
                List<string> toolbarTahun = (TahunUserAktif == "False" && Result.Tipe != 1) ? SetToolbarNonAktif() : SetToolbarTahun(Result.Tipe);
                List<string> toolbarMaster = (TahunUserAktif == "False" && Result.Tipe != 1) ? SetToolbarNonAktif() : SetToolbarMaster(Result.Tipe, Result.PaPpk);
                List<string> toolbarAuditor = (TahunUserAktif == "False" && Result.Tipe != 1) ? SetToolbarNonAktif() : SetToolbarAuditor(Result.Tipe);
                List<string> toolbarMasterPPK = (TahunUserAktif == "False" && Result.Tipe != 1) ? SetToolbarNonAktif() : SetToolbarMasterPPK(Result.Tipe, Result.PaPpk);
                #endregion toolbar

                if (Result.Success && Result.IDAkun != string.Empty)
                {
                    HttpContext.Session.SetString("IDAkun", Result.IDAkun);
                    HttpContext.Session.SetInt32("Tipe", Result.Tipe);
                    HttpContext.Session.SetString("Nama", Result.Nama);
                    HttpContext.Session.SetString("Nip", Result.Nip);
                    HttpContext.Session.SetString("Jabatan", Result.Jabatan);
                    HttpContext.Session.SetString("Golongan", Result.Golongan);
                    HttpContext.Session.SetString("Urusan", Result.Urusan);
                    HttpContext.Session.SetString("Opd", Result.Opd);
                    HttpContext.Session.SetString("OpdName", Result.OpdName);
                    HttpContext.Session.SetString("TahunAktif", Result.TahunAktif);
                    HttpContext.Session.SetString("TahunUserAktif", TahunUserAktif);
                    HttpContext.Session.SetString("Pappk", Result.PaPpk);
                    HttpContext.Session.SetComplexData("Toolbar", toolbar);
                    HttpContext.Session.SetComplexData("ToolbarTahun", toolbarTahun);
                    HttpContext.Session.SetComplexData("ToolbarMaster", toolbarMaster);
                    HttpContext.Session.SetComplexData("ToolbarAuditor", toolbarAuditor);
                    HttpContext.Session.SetComplexData("ToolbarMasterPPK", toolbarMasterPPK);

                    SimpattikGlobals.SetPengumuman(Result.Tipe);
                }

                return RedirectToAction("Index", "Authentication");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        private List<string> SetToolbarNonAktif()
        {
            List<string> setToolbarNonAktif = new List<string>();
            setToolbarNonAktif.Add("Search");

            return setToolbarNonAktif;
        }

        private List<string> SetToolbarTahun(int typeUser)
        {
            List<string> setToolbarTahun = new List<string>();
            if (typeUser == 1)
            {
                setToolbarTahun.Add("Search");
                setToolbarTahun.Add("Add");
                setToolbarTahun.Add("Edit");
                setToolbarTahun.Add("ExcelExport");
                setToolbarTahun.Add("PdfExport");
            }
            else
            {
                setToolbarTahun.Add("Search");
            }

            return setToolbarTahun;
        }

        private List<string> SetToolbar(int typeUser, string pappkUser)
        {
            List<string> setToolbar = new List<string>();
            if ((typeUser == 3) && (pappkUser == "3"))
            {
                setToolbar.Add("Search");
                setToolbar.Add("Add");
                setToolbar.Add("Edit");
                setToolbar.Add("Delete");
                //setToolbar.Add("Update");
                //setToolbar.Add("Cancel");
                setToolbar.Add("ExcelExport");
                setToolbar.Add("PdfExport");
            }
            else if (typeUser == 2)
            {
                setToolbar.Add("Search");
                setToolbar.Add("ExcelExport");
                setToolbar.Add("PdfExport");
            }
            else
            {
                setToolbar.Add("Search");
            }

            return setToolbar;
        }

        private List<string> SetToolbarMaster(int typeUser, string pappkUser)
        {
            List<string> setToolbarMaster = new List<string>();
            if (typeUser == 1)
            {
                setToolbarMaster.Add("Search");
                setToolbarMaster.Add("Add");
                setToolbarMaster.Add("Edit");
                setToolbarMaster.Add("Delete");
                //setToolbarMaster.Add("Update");
                //setToolbarMaster.Add("Cancel");
                setToolbarMaster.Add("ExcelExport");
                setToolbarMaster.Add("PdfExport");
            }
            else if (((typeUser == 5) && (pappkUser == "1")) || ((typeUser == 5) && (pappkUser == "2")))
            {
                setToolbarMaster.Add("Search");
                setToolbarMaster.Add("Add");
                setToolbarMaster.Add("Edit");
                setToolbarMaster.Add("Delete");
                //setToolbarMaster.Add("Update");
                //setToolbarMaster.Add("Cancel");
                setToolbarMaster.Add("ExcelExport");
                setToolbarMaster.Add("PdfExport");
            }
            else
            {
                setToolbarMaster.Add("Search");
            }

            return setToolbarMaster;
        }

        private List<string> SetToolbarMasterPPK(int typeUser, string pappkUser)
        {
            List<string> setToolbarMaster = new List<string>();
            if (typeUser == 1)
            {
                setToolbarMaster.Add("Search");
                setToolbarMaster.Add("Add");
                setToolbarMaster.Add("Edit");
                setToolbarMaster.Add("Delete");
                //setToolbarMaster.Add("Update");
                //setToolbarMaster.Add("Cancel");
                setToolbarMaster.Add("ExcelExport");
                setToolbarMaster.Add("PdfExport");
            }
            else if (((typeUser == 5) && (pappkUser == "1")) || ((typeUser == 5) && (pappkUser == "2")))
            {
                setToolbarMaster.Add("Search");
                setToolbarMaster.Add("Edit");
                //setToolbarMaster.Add("Update");
                //setToolbarMaster.Add("Cancel");
                setToolbarMaster.Add("ExcelExport");
                setToolbarMaster.Add("PdfExport");
            }
            else
            {
                setToolbarMaster.Add("Search");
            }

            return setToolbarMaster;
        }

        private List<string> SetToolbarAuditor(int typeUser)
        {
            List<string> setToolbarAuditor = new List<string>();
            if ((typeUser == 2) || (typeUser == 4))
            {
                setToolbarAuditor.Add("Search");
                setToolbarAuditor.Add("Add");
                setToolbarAuditor.Add("Edit");
                setToolbarAuditor.Add("Delete");
                //setToolbarAuditor.Add("Update");
                //setToolbarAuditor.Add("Cancel");
                setToolbarAuditor.Add("ExcelExport");
                setToolbarAuditor.Add("PdfExport");
            }
            else
            {
                setToolbarAuditor.Add("Search");
            }

            return setToolbarAuditor;
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
