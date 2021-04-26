using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Syncfusion.EJ2.Base;
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
        public IActionResult IndexHpsStrategis()
        {
            ViewBag.Title = "HPS Strategis";
            return View();
        }

        [HttpPost]
        [Route("get-detail-json")]
        public ActionResult GetDetailJson(int id)
        {
            try
            {
                return Json(_tsTHps.GetDetailJson());
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
        [Route("HpsCrud")]
        public ActionResult CrudUpdateHps([FromBody] CRUDModel<tHpsModel> value, string action)
        {
            string msg = string.Empty;

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

        [Route("IndexReviewHps")]
        public IActionResult IndexReviewHps()
        {
            ViewBag.Title = "Review HPS ";
            return View();
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

        [AcceptVerbs("Post")]
        [Route("SaveFileHPS")]
        public IActionResult SaveHPS(IList<IFormFile> UploadFilesHps)
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
        [Route("RemoveFileHPS")]
        public IActionResult RemoveHPS(IList<IFormFile> UploadFilesHps)
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
                Response.StatusCode = 200;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed successfully";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
            return Content("");
        }

    }
}
