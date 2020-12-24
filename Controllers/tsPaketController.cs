using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Models;

namespace simpat1k.Controllers
{
    [Route("Paket")]
    public class tsPaketController : Controller
    {
        private readonly ItsPaket _tsPaket;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public tsPaketController(ItsPaket tsPaket, IHttpContextAccessor httpContextAccessor)
        {
            _tsPaket = tsPaket;
            _httpContextAccessor = httpContextAccessor;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            ViewBag.Title = "Paket";
            ViewBag.queryPaket = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketAll")]
        public IActionResult UrlDataBarangsource([FromBody] tsPaketModel dm)
        {
            IEnumerable DataSource = _tsPaket.GetAll(Int32.Parse(dm.jeniskebutuhan));
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
            int count = DataSource.Cast<tsPaketModel>().Count();
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
        [Route("PaketCrud")]
        public ActionResult CrudUpdatePerencanaan([FromBody] CRUDModel<tsPaketModel> value, string action)
        {
            string msg = string.Empty;

            if (value.Action == "insert")
            {
                foreach (var param in value.Params)
                {
                    if (param.Key == "jeniskebutuhan")
                    {
                        value.Value.jeniskebutuhan = param.GetType().GetProperty("Value").GetValue(param).ToString();
                    }
                }

                DatabaseActionResultModel Result = _tsPaket.Create(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "update")
            {
                DatabaseActionResultModel Result = _tsPaket.Update(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "remove")
            {
                DatabaseActionResultModel Result = _tsPaket.Remove(Int32.Parse(value.Key.ToString()));
                msg = Result.Pesan;
            }
            return Json(new { data = value.Value, message = msg });
        }

        #region Paket Barang
        [Route("IndexBarang")]
        public ActionResult IndexBarang()
        {
            ViewBag.Title = "Paket Kebutuhan Barang";
            ViewBag.queryPaket = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketBarangTemplate")]
        public IActionResult PartialTemplatePaketBarang([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Kebutuhan Barang " + value.Value.idpaket;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            #endregion Combobox

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }
        #endregion Paket Barang

        #region Paket Pekerjaan
        [Route("IndexPekerjaan")]
        public ActionResult IndexPekerjaan()
        {
            ViewBag.Title = "Paket Kebutuhan Pekerjaan";
            ViewBag.queryPaket = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketPekerjaanTemplate")]
        public IActionResult PartialTemplatePaketPekerjaan([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Kebutuhan Pekerjaan " + value.Value.idpaket;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            #endregion Combobox

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }
        #endregion Paket Pekerjaan

        #region Paket Jasa Konsultasi
        [Route("IndexKonsultasi")]
        public ActionResult IndexKonsultasi()
        {
            ViewBag.Title = "Paket Kebutuhan Jasa Konsultasi";
            ViewBag.queryPaket = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketKonsultasiTemplate")]
        public IActionResult PartialTemplatePaketKonsultasi([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Kebutuhan Jasa Konsultasi " + value.Value.idpaket;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            #endregion Combobox

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }
        #endregion Paket Jasa Konsultasi

        #region Paket Jasa Lainnya
        [Route("IndexLainnya")]
        public ActionResult IndexLainnya()
        {
            ViewBag.Title = "Paket Kebutuhan Jasa Lainnya";
            ViewBag.queryPaket = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketLainnyaTemplate")]
        public IActionResult PartialTemplatePaketLainnya([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Kebutuhan Jasa Lainnya " + value.Value.idpaket;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            #endregion Combobox

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }
        #endregion Paket Jasa Lainnya
    }
}
