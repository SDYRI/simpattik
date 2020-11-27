using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using TasikmalayaKota.Simpatik.Web.Models;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.tsIdentifikasi.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace simpat1k.Controllers
{
    [Route("Identifikasi")]
    public class tsIdentifikasiController : Controller
    {
        private readonly ItsIdentifikasi _tsIdentifikasi;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public tsIdentifikasiController(ItsIdentifikasi tsIdentifikasi, IHttpContextAccessor httpContextAccessor)
        {
            _tsIdentifikasi = tsIdentifikasi;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Perencanaan Pengadaan
        [Route("IndexMe")]
        public ActionResult Index()
        {
            ViewBag.Title = "Identifikasi";
            return View();
        }

        [HttpPost]
        [Route("PerencanaanPengadaanAll")]
        public IActionResult UrlDatasource([FromBody] tsIdentifikasiModel dm)
        {
            IEnumerable DataSource = _tsIdentifikasi.GetAll();
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
            int count = DataSource.Cast<tsIdentifikasiModel>().Count();
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
        [Route("IdentifikasiBarangAll")]
        public IActionResult UrlDataBarangsource([FromBody] tsIdentifikasiModel dm)
        {
            IEnumerable DataSource = _tsIdentifikasi.GetAll(Int32.Parse(dm.jeniskebutuhan));
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
            int count = DataSource.Cast<tsIdentifikasiModel>().Count();
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
        [Route("PerencanaanPengadaanCrud")]
        public ActionResult CrudUpdatePerencanaan([FromBody] CRUDModel<tsIdentifikasiModel> value, string action)
        {
            string msg = string.Empty;

            if (value.Action == "insert")
            {
                DatabaseActionResultModel Result = _tsIdentifikasi.Create(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "update")
            {
                DatabaseActionResultModel Result = _tsIdentifikasi.Update(value.Value);
                msg = Result.Pesan;
            }
            else if (value.Action == "remove")
            {
                DatabaseActionResultModel Result = _tsIdentifikasi.Remove(Int32.Parse(value.Key.ToString()));
                msg = Result.Pesan;
            }
            return Json(new { data = value.Value, message = msg });
        }
        #endregion Perencanaan Pengadaan

        #region Identifikasi Barang
        [Route("IndexBarang")]
        public ActionResult IndexBarang()
        {
            ViewBag.Title = "Identifikasi Kebutuhan Barang";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 1)";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiBarangTemplate")]
        public IActionResult PartialTemplateIdentifikasiBarang([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            var valTemplate = _tsIdentifikasi.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Identifikasi Kebutuhan Barang " + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "1";
            value.Value.opd = _httpContextAccessor.HttpContext.Session.GetString("OpdName");
            value.Value.pejabat = _httpContextAccessor.HttpContext.Session.GetString("Nama");
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";

            return PartialView("_tsIdentifikasiBarangTemplate", value.Value);
        }
        #endregion Identifikasi Barang

        #region Identifikasi Pekerjaan
        [Route("IndexPekerjaan")]
        public ActionResult IndexPekerjaan()
        {
            ViewBag.Title = "Identifikasi Kebutuhan Pekerjaan Kontruksi";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 2)";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiPekerjaanTemplate")]
        public IActionResult PartialTemplateIdentifikasiPekerjaan([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            var valTemplate = _tsIdentifikasi.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Identifikasi Kebutuhan Pekerjaan Kontruksi" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "2";
            value.Value.opd = _httpContextAccessor.HttpContext.Session.GetString("OpdName");
            value.Value.pejabat = _httpContextAccessor.HttpContext.Session.GetString("Nama");
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";

            return PartialView("_tsIdentifikasiBarangTemplate", value.Value);
        }
        #endregion Identifikasi Pekerjaan

        #region Identifikasi Jasa Konsultasi
        [Route("IndexKonsultasi")]
        public ActionResult IndexKonsultasi()
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Konsultasi";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 3)";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiKonsultasiTemplate")]
        public IActionResult PartialTemplateIdentifikasiKonsultasi([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            var valTemplate = _tsIdentifikasi.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Konsultasi" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "3";
            value.Value.opd = _httpContextAccessor.HttpContext.Session.GetString("OpdName");
            value.Value.pejabat = _httpContextAccessor.HttpContext.Session.GetString("Nama");
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";

            return PartialView("_tsIdentifikasiBarangTemplate", value.Value);
        }
        #endregion Identifikasi Jasa Konsultasi

        #region Identifikasi Jasa LAinnya
        [Route("IndexLainnya")]
        public ActionResult IndexLainnya()
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Lainnya";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 4)";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiLainnyaTemplate")]
        public IActionResult PartialTemplateIdentifikasiLainnya([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            var valTemplate = _tsIdentifikasi.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Lainnya" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "4";
            value.Value.opd = _httpContextAccessor.HttpContext.Session.GetString("OpdName");
            value.Value.pejabat = _httpContextAccessor.HttpContext.Session.GetString("Nama");
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";

            return PartialView("_tsIdentifikasiBarangTemplate", value.Value);
        }
        #endregion Identifikasi Jasa Konsultasi

        #region Surat Penetapan
        [Route("SuratPenetapan")]
        public IActionResult SuratPenetapan()
        {
            //var valTemplate = _tsIdentifikasi.GetAll(1);
            //ViewBag.datasource = valTemplate;
            ViewBag.Title = "Surat Penetapan";
            ViewBag.opd = _httpContextAccessor.HttpContext.Session.GetString("OpdName");
            ViewBag.pejabat = _httpContextAccessor.HttpContext.Session.GetString("Nama");
            return PartialView("_tsIdentifikasiSuratPenetapan");
        }
        #endregion Surat Penetapan
    }
}