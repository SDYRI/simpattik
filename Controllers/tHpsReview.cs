using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Navigations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using TasikmalayaKota.Simpatik.Web.Services.tHps.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tHps.Models;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    [Route("Hps")]
    public class tHpsReview : Controller
    {
        private readonly IWebHostEnvironment HostEnvironment;
        private readonly ItHps _tsTHps;
        private readonly string _Folder;

        public tHpsReview(ItHps tsTHps, IWebHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
            _tsTHps = tsTHps;
            _Folder = "filehps";
        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("IndexMe")]
        public IActionResult IndexHps()
        {
            ViewBag.Title = "HPS ";
            return View();
        }

        [Route("IndexHpsStrategis/{id}")]
        public IActionResult IndexHpsStrategis(string id)
        {
            ViewBag.Title = "HPS Strategis";
            ViewBag.queryHps = "new ej.data.Query().addParams('idpaket', '" + id + "')";
            return View();
        }


        [Route("IndexReviewHps/{id}")]
        public IActionResult IndexReviewHps(string id)
        {
            ViewBag.Title = "Review HPS ";
            ViewBag.headerFilehps = new TabHeader { Text = "File HPS" };
            ViewBag.headerReviewhps = new TabHeader { Text = "Review HPS" };
            ViewBag.queryHps = "new ej.data.Query().addParams('idpaket', '" + id + "')";
            ViewBag.pathDownload = "/Hps/DownloadFileHps/";
            return View();
        }

        [HttpPost]
        [Route("get-detail-json")]
        public ActionResult GetDetailJson([FromBody] mSshModel dm)
        {
            try
            {
                return Json(_tsTHps.GetDetailJson(dm.typessh, dm.idssh));
            }
            catch (Exception exception)
            {
                return StatusCode(500, exception.Message);
            }
        }

        [HttpPost]
        [Route("HpsAll")]
        public IActionResult UrlDatasource([FromBody] tHpsModel dm)
        {
            IEnumerable DataSource = _tsTHps.GetAll();
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
            int count = DataSource.Cast<tHpsModel>().Count();
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
        [Route("ReviewHpsAll")]
        public IActionResult UrlDataReviewsource([FromBody] tHpsReviewModel dm)
        {
            IEnumerable DataSource = _tsTHps.GetReviewAll(dm.idpaket);
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
            int count = DataSource.Cast<tHpsReviewModel>().Count();
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
        [Route("SshAll")]
        public IActionResult UrlDataSshsource([FromBody] mSshModel dm)
        {
            IEnumerable DataSource = _tsTHps.SshGetAll();
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
            int count = DataSource.Cast<mSshModel>().Count();
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
        [Route("HpsCrud")]
        public ActionResult CrudUpdateHps([FromBody] CRUDModel<tHpsModel> value, string action)
        {
            string msg = string.Empty;

            foreach (var param in value.Params)
            {
                if ((value.Action == "insert") || (value.Action == "update"))
                {
                    if (param.Key != "idpaket")
                    {
                        continue;
                    }
                    value.Value.idpaket = param.GetType().GetProperty("Value").GetValue(param).ToString();
                }
            }

            if (value.Action == "insert")
            {
                DatabaseActionResultModel Result = _tsTHps.Create(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "update")
            {
                DatabaseActionResultModel Result = _tsTHps.Update(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "remove")
            {
                DatabaseActionResultModel Result = _tsTHps.Remove(value.Key.ToString());
                msg = Result.Pesan;
            }

            return Json(new { data = value.Value, message = msg });
        }

        [HttpPost]
        [Route("ReviewHpsCrud")]
        public ActionResult CrudUpdateReviewHps([FromBody] CRUDModel<tHpsReviewModel> value, string action)
        {
            string msg = string.Empty;

            foreach (var param in value.Params)
            {
                if ((value.Action == "insert") || (value.Action == "update"))
                {
                    if (param.Key != "idpaket")
                    {
                        continue;
                    }
                    value.Value.idpaket = param.GetType().GetProperty("Value").GetValue(param).ToString();
                }
            }

            if (value.Action == "insert")
            {
                DatabaseActionResultModel Result = _tsTHps.Create(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "update")
            {
                DatabaseActionResultModel Result = _tsTHps.Update(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "remove")
            {
                DatabaseActionResultModel Result = _tsTHps.RemoveReview(value.Key.ToString());
                msg = Result.Pesan;
            }

            return Json(new { data = value.Value, message = msg });
        }

        [HttpPost]
        [Route("HpsTemplate")]
        public IActionResult PartialTemplatePaketBarang([FromBody] CRUDModel<tHpsModel> value)
        {
            var valTemplate = _tsTHps.GetAll();
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Hps " + value.Value.idpaket;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            #endregion Combobox

            return PartialView("_tsHpsTemplate", value.Value);
        }

        [HttpPost]
        [Route("HpsTemplateStrategis")]
        public IActionResult PartialTemplateHpsStrategis([FromBody] CRUDModel<tHpsModel> value)
        {
            var valTemplate = _tsTHps.GetAll();
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Hps " + value.Value.idpaket;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            #endregion Combobox

            return PartialView("_tsHpsStrategisTemplate", value.Value);
        }

        [HttpPost]
        [Route("HpsTemplateReviewStrategis")]
        public IActionResult PartialTemplateReviewHpsStrategis([FromBody] CRUDModel<tHpsReviewModel> value)
        {
            var valTemplate = _tsTHps.GetReviewAll(value.Value.idpaket);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Review Hps " + value.Value.idpaket;

            ViewBag.sortDropdown = "Ascending";
            ViewBag.querySsh = "new ej.data.Query().select(['idssh', 'namassh']).take(10).requiresCount()";

            #region Enable
            if (HttpContext.Session.GetInt32("Tipe") == 2)
            {
                ViewBag.truefalse = true;
            }
            else
            {
                ViewBag.truefalse = false;
            }
            #endregion Enable

            #region Combobox
            ViewBag.ssh = new enumDataModel().TipeSSH();
            #endregion Combobox

            return PartialView("_tsReviewHpsStrategisTemplate", value.Value);
        }

        [AcceptVerbs("Post")]
        [Route("SaveFileHPS")]
        public void SaveHPS(IList<IFormFile> UploadFilesHps)
        {
            try
            {
                foreach (var file in UploadFilesHps)
                {
                    string FilePath = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = FilePath + $@"\{filename}";
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
                        Response.ContentType = "application/json; charset=utf-8";
                        Response.StatusCode = 409;
                        Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists.";
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 404;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File failed to upload.";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [AcceptVerbs("Post")]
        [Route("RemoveFileHPS")]
        public void RemoveHPS(IList<IFormFile> UploadFilesHps)
        {
            try
            {
                foreach (var file in UploadFilesHps)
                {
                    string FilePath = Path.Combine(HostEnvironment.WebRootPath, _Folder);
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    filename = FilePath + $@"\{filename}";
                    if (System.IO.File.Exists(filename))
                    {
                        System.IO.File.Delete(filename);
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.StatusCode = 404;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed failed";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }

        [Route("DownloadFileHps/{fileName}")]
        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string path = Path.Combine(this.HostEnvironment.WebRootPath, _Folder, fileName);

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

    }
}
