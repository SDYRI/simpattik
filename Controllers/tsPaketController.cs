using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using Syncfusion.EJ2.Navigations;
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

        public tsPaketController(ItsPaket tsPaket)
        {
            _tsPaket = tsPaket;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            ViewBag.Title = "Paket";
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 1)";
            return View();
        }

        [HttpPost]
        [Route("PaketAll")]
        public IActionResult UrlDataBarangsource([FromBody] tsPaketModel dm)
        {
            IEnumerable DataSource = _tsPaket.GetAll(Int32.Parse(dm.jeniskebutuhan), dm.tipePaket);
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
        [Route("PaketReview")]
        public IActionResult UrlPaketReview([FromBody] tsPaketModel dm)
        {
            IEnumerable DataSource = _tsPaket.GetAll(0, 0);
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
        [Route("PaketStrategis")]
        public IActionResult UrlPaketStrategis([FromBody] tsPaketModel dm)
        {
            IEnumerable DataSource = _tsPaket.GetAllStrategis(0, 0);
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

            foreach (var param in value.Params)
            {
                if ((value.Action == "insert") || (value.Action == "update"))
                {
                    if (param.Key == "jeniskebutuhan")
                    {
                        value.Value.jeniskebutuhan = param.GetType().GetProperty("Value").GetValue(param).ToString();
                    }
                    if (param.Key == "tipePaket")
                    {
                        value.Value.tipePaket = Int32.Parse(param.GetType().GetProperty("Value").GetValue(param).ToString());
                    }

                    value.Value.thanggrn = HttpContext.Session.GetString("TahunAktif");
                }
            }

            if (HttpContext.Session.GetInt32("Tipe") == 3)
            {
                if (value.Action == "insert")
                {
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
                    DatabaseActionResultModel Result = _tsPaket.Remove(value.Key.ToString());
                    msg = Result.Pesan;
                }
            }
            else
            {
                msg = "BERHASIL DISIMPAN";
            }

            return Json(new { data = value.Value, message = msg });
        }

        #region Paket Barang
        [Route("IndexBarang")]
        public ActionResult IndexBarang()
        {
            ViewBag.Title = "Kebutuhan Barang";
            ViewBag.headerPenyedia = new TabHeader { Text = "Penyedia" };
            ViewBag.headerSwakelola = new TabHeader { Text = "Swakelola" };
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 1)";
            ViewBag.querySwakelola = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 2)";
            ViewBag.identifikasi = "/Identifikasi/IndexBarang/";
            return View();
        }

        [HttpPost]
        [Route("PaketBarangTemplate")]
        public IActionResult PartialTemplatePaketBarang([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1, 1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Penyedia Kebutuhan Barang " + value.Value.idpaket;
            value.Value.jeniskebutuhan = "1";
            value.Value.tipePaket = 1;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            #endregion Combobox

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }

        [HttpPost]
        [Route("PaketSwakelolaBarangTemplate")]
        public IActionResult PartialTemplatePaketSwakelolaBarang([FromBody] CRUDModel<tsPaketModel> value)
        {
            var valTemplate = _tsPaket.GetAll(1, 1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Paket Swakelola Kebutuhan Barang " + value.Value.idpaket;
            value.Value.jeniskebutuhan = "1";
            value.Value.tipePaket = 1;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            ViewBag.tipeswakelola = new enumDataModel().TipeSwakelola();
            #endregion Combobox

            return PartialView("_tsPaketSwakelolaBarangTemplate", value.Value);
        }
        #endregion Paket Barang

        #region Paket Pekerjaan
        [Route("IndexPekerjaan")]
        public ActionResult IndexPekerjaan()
        {
            ViewBag.Title = "Kebutuhan Pekerjaan";
            ViewBag.headerPenyedia = new TabHeader { Text = "Penyedia" };
            ViewBag.headerSwakelola = new TabHeader { Text = "Swakelola" };
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 2).addParams('tipePaket', 1)";
            ViewBag.querySwakelola = "new ej.data.Query().addParams('jeniskebutuhan', 2).addParams('tipePaket', 2)";
            ViewBag.identifikasi = "/Identifikasi/IndexPekerjaan/";
            return View("IndexBarang");
        }
        #endregion Paket Pekerjaan

        #region Paket Jasa Konsultasi
        [Route("IndexKonsultasi")]
        public ActionResult IndexKonsultasi()
        {
            ViewBag.Title = "Kebutuhan Jasa Konsultasi";
            ViewBag.headerPenyedia = new TabHeader { Text = "Penyedia" };
            ViewBag.headerSwakelola = new TabHeader { Text = "Swakelola" };
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 3).addParams('tipePaket', 1)";
            ViewBag.querySwakelola = "new ej.data.Query().addParams('jeniskebutuhan', 3).addParams('tipePaket', 2)";
            ViewBag.identifikasi = "/Identifikasi/IndexKonsultasi/";
            return View("IndexBarang");
        }
        #endregion Paket Jasa Konsultasi

        #region Paket Jasa Lainnya
        [Route("IndexLainnya")]
        public ActionResult IndexLainnya()
        {
            ViewBag.Title = "Kebutuhan Jasa Lainnya";
            ViewBag.headerPenyedia = new TabHeader { Text = "Penyedia" };
            ViewBag.headerSwakelola = new TabHeader { Text = "Swakelola" };
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 4).addParams('tipePaket', 1)";
            ViewBag.querySwakelola = "new ej.data.Query().addParams('jeniskebutuhan', 4).addParams('tipePaket', 2)";
            ViewBag.identifikasi = "/Identifikasi/IndexLainnya/";
            return View("IndexBarang");
        }
        #endregion Paket Jasa Lainnya
    }
}
