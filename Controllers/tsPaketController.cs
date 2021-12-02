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
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsPaket.Models;

namespace simpat1k.Controllers
{
    [Route("Paket")]
    public class tsPaketController : Controller
    {
        private readonly ItsPaket _tsPaket;
        private readonly ImTahunAnggaran _mTahunAnggaran;

        public tsPaketController(ItsPaket tsPaket, ImTahunAnggaran mTahunAnggaran)
        {
            _tsPaket = tsPaket;
            _mTahunAnggaran = mTahunAnggaran;
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
            IEnumerable DataSource = _tsPaket.GetAllReview(0, dm.tipePaket, dm.opd);
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
        [Route("PaketReviewPerencanaan")]
        public IActionResult UrlPaketReviewPerencanaan([FromBody] tsPaketModel dm)
        {
            IEnumerable DataSource = _tsPaket.GetAllPerencanaan(0, 0);
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

            if (_mTahunAnggaran.GetUserTahunAktif() == "True")
            {
                if ((HttpContext.Session.GetInt32("Tipe") == 3) && (HttpContext.Session.GetString("Pappk") == "3"))
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
                    if ((HttpContext.Session.GetInt32("Tipe") == 4) && (value.Action == "update"))
                    {
                        DatabaseActionResultModel Result = _tsPaket.UpdateReview(value.Value);
                        msg = Result.Pesan;
                    }
                    else
                    {
                        msg = "GAGAL DISIMPAN";
                    }
                }
            }
            else
            {
                msg = "SEDANG TAHAP REVIU";
            }

            return Json(new { data = value.Value, message = msg });
        }

        [HttpPost]
        [Route("PaketUpdateReviewPerencanaan")]
        public ActionResult CrudUpdateReviewPerencanaan([FromBody] CRUDModel<tsPaketModel> value, string action)
        {
            string msg = string.Empty;

            if ((HttpContext.Session.GetInt32("Tipe") == 4) && (value.Action == "update"))
            {
                DatabaseActionResultModel Result = _tsPaket.UpdateStatusReview(value.Value);
                msg = Result.Pesan;
            }
            else
            {
                msg = "GAGAL DISIMPAN";
            }

            return Json(new { data = value.Value, message = msg });
        }

        #region Paket HPS
        [Route("IndexHps")]
        public IActionResult IndexHps()
        {
            ViewBag.Title = "HPS Strategis";
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 1)";
            ViewBag.identifikasi = "/Hps/IndexHpsStrategis/";
            return View();
        }

        [Route("IndexReviewHps")]
        public IActionResult IndexReviewHps()
        {
            ViewBag.Title = "HPS Strategis";
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 1)";
            ViewBag.identifikasi = "/Hps/IndexReviewHps/";
            return View();
        }
        #endregion

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
            ViewBag.Title = "Paket Penyedia Kebutuhan Barang " + value.Value.idpaket;
            ViewBag.sortDropdown = "Ascending";
            value.Value.jeniskebutuhan = "1";
            value.Value.tipePaket = 1;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;
            ViewBag.queryPaket = "new ej.data.Query().select(['nmpaket', 'idpaket']).take(10).requiresCount()";

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            ViewBag.revisi = new enumDataModel().Revisi();
            #endregion Combobox

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
                if (value.Value.keteranganmetode != string.Empty)
                {
                    ViewBag.truefalsemetode = true;
                }
                else
                {
                    ViewBag.truefalsemetode = false;
                }
            }
            else
            {
                ViewBag.truefalse = true;
                ViewBag.truefalsemetode = true;
            }
            #endregion Enable

            #region EnableKeterangan
            if (((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4)) && value.Value.statuspaket == "Review")
            {
                if ((value.Value.keteranganmetode == string.Empty) || (value.Value.keteranganmetode == null) || (value.Value.keteranganpagu == string.Empty) || (value.Value.keteranganpagu == null))
                {
                    ViewBag.truefalserevisi = false;
                }
                else
                {
                    ViewBag.truefalserevisi = true;
                }

                ViewBag.truefalseket = true;
            }
            else
            {
                ViewBag.truefalseket = false;
                ViewBag.truefalserevisi = false;
            }

            if ((HttpContext.Session.GetInt32("Tipe") == 3) || (HttpContext.Session.GetInt32("Tipe") == 5))
            {
                if ((value.Value.statuspaket == "Revisi") || (value.Value.statuspaket == "Done"))
                {
                    if ((value.Value.keteranganmetode == string.Empty) || (value.Value.keteranganmetode == null))
                    {
                        ViewBag.truefalsemetode = true;
                    }
                    else
                    {
                        ViewBag.truefalsemetode = false;
                    }
                }
                else
                {
                    ViewBag.truefalsemetode = true;
                }
            }
            else
            {
                ViewBag.truefalsemetode = false;
            }

            if ((HttpContext.Session.GetInt32("Tipe") == 3) || (HttpContext.Session.GetInt32("Tipe") == 5))
            {
                if ((value.Value.statuspaket == "Revisi") || (value.Value.statuspaket == "Done"))
                {
                    if ((value.Value.keteranganpagu == string.Empty) || (value.Value.keteranganpagu == null))
                    {
                        ViewBag.truefalsepagu = true;
                    }
                    else
                    {
                        ViewBag.truefalsepagu = false;
                    }
                }
                else
                {
                    ViewBag.truefalsepagu = true;
                }
            }
            else
            {
                ViewBag.truefalsepagu = false;
            }
            #endregion EnableKeterangan

            return PartialView("_tsPaketBarangTemplate", value.Value);
        }

        [HttpPost]
        [Route("PaketSwakelolaBarangTemplate")]
        public IActionResult PartialTemplatePaketSwakelolaBarang([FromBody] CRUDModel<tsPaketModel> value)
        {
            ViewBag.Title = "Paket Swakelola Kebutuhan Barang " + value.Value.idpaket;
            ViewBag.sortDropdown = "Ascending";
            value.Value.jeniskebutuhan = "1";
            value.Value.tipePaket = 1;
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.namaopd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;
            ViewBag.queryPaket = "new ej.data.Query().select(['nmpaket', 'idpaket']).take(10).requiresCount()";

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.metodepemilihan = new enumDataModel().MetodePemilihan();
            ViewBag.tipeswakelola = new enumDataModel().TipeSwakelola();
            ViewBag.revisi = new enumDataModel().Revisi();
            #endregion Combobox

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
                if (value.Value.keteranganmetode != string.Empty)
                {
                    ViewBag.truefalsemetode = true;
                }
                else
                {
                    ViewBag.truefalsemetode = false;
                }
            }
            else
            {
                ViewBag.truefalse = true;
                ViewBag.truefalsemetode = true;
            }
            #endregion Enable

            #region EnableKeterangan
            if (((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4)) && value.Value.statuspaket == "Review")
            {
                if ((value.Value.keteranganmetode == string.Empty) || (value.Value.keteranganmetode == null) || (value.Value.keteranganpagu == string.Empty) || (value.Value.keteranganpagu == null))
                {
                    ViewBag.truefalserevisi = false;
                }
                else
                {
                    ViewBag.truefalserevisi = true;
                }

                ViewBag.truefalseket = true;
            }
            else
            {
                ViewBag.truefalseket = false;
                ViewBag.truefalserevisi = false;
            }

            if ((HttpContext.Session.GetInt32("Tipe") == 3) || (HttpContext.Session.GetInt32("Tipe") == 5))
            {
                if ((value.Value.statuspaket == "Revisi") || (value.Value.statuspaket == "Done"))
                {
                    if ((value.Value.keteranganmetode == string.Empty) || (value.Value.keteranganmetode == null))
                    {
                        ViewBag.truefalsemetode = true;
                    }
                    else
                    {
                        ViewBag.truefalsemetode = false;
                    }
                }
                else
                {
                    ViewBag.truefalsemetode = true;
                }
            }
            else
            {
                ViewBag.truefalsemetode = false;
            }

            if ((HttpContext.Session.GetInt32("Tipe") == 3) || (HttpContext.Session.GetInt32("Tipe") == 5))
            {
                if ((value.Value.statuspaket == "Revisi") || (value.Value.statuspaket == "Done"))
                {
                    if ((value.Value.keteranganpagu == string.Empty) || (value.Value.keteranganpagu == null))
                    {
                        ViewBag.truefalsepagu = true;
                    }
                    else
                    {
                        ViewBag.truefalsepagu = false;
                    }
                }
                else
                {
                    ViewBag.truefalsepagu = true;
                }
            }
            else
            {
                ViewBag.truefalsepagu = false;
            }
            #endregion EnableKeterangan

            return PartialView("_tsPaketSwakelolaBarangTemplate", value.Value);
        }
        #endregion Paket Barang

        #region Paket Pekerjaan
        [Route("IndexPekerjaan")]
        public ActionResult IndexPekerjaan()
        {
            ViewBag.Title = "Kebutuhan Pekerjaan Kontruksi";
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
