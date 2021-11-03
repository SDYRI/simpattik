using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using System.Dynamic;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using TasikmalayaKota.Simpatik.Web.Models;
using System;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System.Security.Principal;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Models;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http.Features;
using Syncfusion.EJ2.Inputs;
using System.Net.Http.Headers;
using System.Runtime.InteropServices.ComTypes;
using TasikmalayaKota.Simpatik.Web.Extensions;
using Newtonsoft.Json.Linq;
using Syncfusion.Pdf.Parsing;
using static System.Net.WebRequestMethods;
using Syncfusion.EJ2.PdfViewer;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace simpat1k.Controllers
{
    [Route("UserMaster")]
    public class mUserController : Controller
    {
        private readonly IWebHostEnvironment HostEnvironment;
        private IMemoryCache _cache;
        private readonly ImUser _mUser;
        private readonly ImOpd _mOpd;
        private readonly string _Folder;

        public mUserController(ImUser mUser, ImOpd mOpd, IWebHostEnvironment hostEnvironment, IMemoryCache cache)
        {
            HostEnvironment = hostEnvironment;
            _mUser = mUser;
            _mOpd = mOpd;
            _cache = cache;
            _Folder = "filesk/";
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Tipe") == 1)
            {
                ViewBag.Title = "Master User";
                ViewBag.pathDownload = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                return View();
            }
            else if (((HttpContext.Session.GetInt32("Tipe") == 5) && (HttpContext.Session.GetString("Pappk") == "1")) || ((HttpContext.Session.GetInt32("Tipe") == 5) && (HttpContext.Session.GetString("Pappk") == "2")))
            {
                ViewBag.Title = "Master User PPK";
                return View("IndexPpk");
            }
            else
            {
                return RedirectToAction("HandleError", "ErrorExecute", new { code = 404 });
            }
        }

        [Route("IndexPpkMe")]
        public IActionResult IndexPpk()
        {
            ViewBag.Title = "Master User PPK";
            return View();
        }

        [Route("IndexProfileMe")]
        public IActionResult IndexProfile(mUserModel value)
        {
            var valTemplate = _mUser.GetById();
            value = valTemplate;

            ViewBag.Title = "Profile User";
            return View(value);
        }

        [HttpPost]
        [Route("UserMasterAll")]
        public IActionResult UrlDatasource([FromBody] mUserModel dm)
        {
            IEnumerable DataSource = _mUser.GetAll();
            DataOperations operation = new DataOperations();
            if (dm.Search != null && dm.Search.Count > 0)
            {
                DataSource = operation.PerformSearching(DataSource, dm.Search);  //Search
            }
            if (dm.Sorted != null && dm.Sorted.Count > 0) //Sorting
            {
                DataSource = operation.PerformSorting(DataSource, dm.Sorted);
            }
            if (dm.Where != null && dm.Where.Count > 0) //Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
            }
            int count = DataSource.Cast<mUserModel>().Count();
            if (dm.Skip != 0)
            {
                DataSource = operation.PerformSkip(DataSource, dm.Skip);   //Paging
            }
            if (dm.Take != 0)
            {
                DataSource = operation.PerformTake(DataSource, dm.Take);
            }
            return dm.RequiresCounts ? Json(new { result = DataSource, count = count }) : Json(DataSource);
        }

        [HttpPost]
        [Route("UserMasterCrud")]
        public IActionResult CrudUpdate([FromBody] CRUDModel<mUserModel> value, string action, IList<IFormFile> fileupload, IFormCollection form)
        {
            string msg = string.Empty;
            bool sukses = false;
            try
            {
                if (ModelState.IsValid)
                {
                    //if ((HttpContext.Session.GetInt32("Tipe") == 1) || ((HttpContext.Session.GetInt32("Tipe") == 5) && (HttpContext.Session.GetString("Pappk") == "1")) || ((HttpContext.Session.GetInt32("Tipe") == 5) && (HttpContext.Session.GetString("Pappk") == "2")))
                    if (HttpContext.Session.GetInt32("Tipe") == 1)
                    {
                        if ((HttpContext.Session.GetInt32("Tipe") == 1) && (value.Action == "insert"))
                        {
                            DatabaseActionResultModel Result = _mUser.Create(value.Value);
                            msg = Result.Pesan;
                            sukses = Result.Success;
                        }
                        else if ((HttpContext.Session.GetInt32("Tipe") == 1) && (value.Action == "update"))
                        {
                            DatabaseActionResultModel Result = _mUser.Update(value.Value);
                            value.Value.ListOpdUser = Result.Data.ToString();
                            msg = Result.Pesan;
                            sukses = Result.Success;
                        }
                        else if ((HttpContext.Session.GetInt32("Tipe") == 1) && (value.Action == "remove"))
                        {
                            DatabaseActionResultModel Result = _mUser.Remove(value.Key.ToString());
                            msg = Result.Pesan;
                            sukses = Result.Success;
                        }
                        if (sukses)
                        {
                            foreach (var file in fileupload)
                            {
                                if (fileupload != null)
                                {
                                    string FilePath = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                                    if (!Directory.Exists(FilePath))
                                    {
                                        Directory.CreateDirectory(FilePath);
                                    }

                                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                                    filename = FilePath + $"{filename}";
                                    if (!System.IO.File.Exists(filename))
                                    {
                                        using (FileStream fs = System.IO.File.Create(filename))
                                        {
                                            file.CopyTo(fs);
                                            fs.Flush();
                                        }
                                    }
                                    else
                                    {
                                        Response.Clear();
                                        Response.StatusCode = 204;
                                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        msg = "GAGAL DISIMPAN";
                    }

                    return Json(new { data = value.Value, message = msg });
                }
                else
                {
                    return Json(new { data = value.Value, message = ModelState.Values.ToArray()[1].Errors });
                }
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [HttpPost]
        public IActionResult UserValidation(mUserModel Args)
        {
            string msg = string.Empty;
            try
            {
                var valTemplate = _mUser.GetById();
                Args.UserName = valTemplate.UserName;

                DatabaseActionResultModel Result = _mUser.UpdateProfile(Args);
                msg = Result.Pesan;

                return RedirectToAction("IndexProfileMe", "UserMaster");
            }
            catch (Exception exception)
            {
                return BadRequest(exception.Message);
            }
        }

        [AcceptVerbs("Post")]
        [Route("SaveSK")]
        public IActionResult SaveSK(IList<IFormFile> UploadFilesSK)
        {
            try
            {
                foreach (var file in UploadFilesSK)
                {
                    string FilePath = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                    string extension = Path.GetExtension(ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"'));
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //var filename = System.Guid.NewGuid().ToString() + extension;
                    filename = FilePath + $"{filename}";
                    if (!System.IO.File.Exists(filename))
                    {
                        using (FileStream fs = System.IO.File.Create(filename))
                        {
                            file.CopyTo(fs);
                            fs.Flush();
                        }
                    }
                    else
                    {
                        Response.Clear();
                        Response.StatusCode = 204;
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 204;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "No Content";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
            return Content("");
        }

        [AcceptVerbs("Post")]
        [Route("RemoveSK")]
        public IActionResult RemoveSK(IList<IFormFile> UploadFilesSK)
        {
            try
            {
                foreach (var file in UploadFilesSK)
                {
                    string FilePath = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = FilePath + $"{filename}";
                    if (!System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
            return Content("");
        }

        [HttpPost]
        [Route("UserMasterTemplate")]
        public IActionResult PartialTemplateUser([FromBody] CRUDModel<mUserModel> value)
        {
           ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.Title = "Master User " + value.Value.NamaUser;
            ViewBag.id = 2; //value.Value.ListIdOpdUser;

            #region Combobox
            ViewBag.pappk = new enumDataModel().PaPpk();
            ViewBag.tipeuser = new enumDataModel().TipeUser();
            #endregion Combobox

            return PartialView("_mUserTemplate", value.Value);
        }

        [HttpPost]
        [Route("UserMasterTemplatePpk")]
        public IActionResult PartialTemplateUserPpk([FromBody] CRUDModel<mUserModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.Title = "Master User " + value.Value.NamaUser;
            ViewBag.id = 2;
            ViewBag.idOPD = HttpContext.Session.GetString("Opd");

            return PartialView("_mUserTemplatePpk", value.Value);
        }

        [Route("DownloadFileSk")]
        public IActionResult GetDocumentFileSk(string fileskname)
        {
            ViewBag.Title = fileskname;
            ViewBag.DocumentPath = Path.Combine(HostEnvironment.WebRootPath, _Folder, fileskname);
            return View("~/Views/PdfViewer/Default.cshtml");
        }
    }
}