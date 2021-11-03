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
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using TasikmalayaKota.Simpatik.Web.Services.Middleware.Models;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using System.IO;
using Syncfusion.XlsIO;
using System.Security.Cryptography;
using Syncfusion.XlsIO.Implementation;
using Syncfusion.Pdf;
using Syncfusion.XlsIORenderer;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Syncfusion.EJ2.Spreadsheet;

namespace simpat1k.Controllers
{
    //[AutoValidateAntiforgeryToken]
    [Route("Identifikasi")]
    public class tsIdentifikasiController : Controller
    {
        private readonly IWebHostEnvironment HostEnvironment;
        private readonly ItsIdentifikasi _tsIdentifikasi;
        private readonly string OPD;
        private readonly string TA;
        private readonly string _Folder;

        public tsIdentifikasiController(ItsIdentifikasi tsIdentifikasi, IHttpContextAccessor httpContextAccessor, IWebHostEnvironment hostEnvironment)
        {
            HostEnvironment = hostEnvironment;
            _tsIdentifikasi = tsIdentifikasi;
            OPD = httpContextAccessor.HttpContext.Session.GetString("Opd");
            TA = httpContextAccessor.HttpContext.Session.GetString("TahunAktif");
            _Folder = "filesrtpenetapan/";
        }
        public class ExportModel : DataManagerRequest  // Create the model class to Deserialize the GridModel 
        {
            public List<string> columnsName { get; set; }
            public List<string> fields { get; set; }
            public List<string> format { get; set; }
        }

        #region Perencanaan Pengadaan
        [Route("IndexMe")]
        public ActionResult Index()
        {
            ViewBag.Title = "Rekap Perencanaan Pengadaan";
            ViewBag.queryTipePaket = "new ej.data.Query().addParams('tipepaket', 0)";
            return View();
        }

        [Route("IndexLapPenyedia")]
        public ActionResult IndexLapPenyedia()
        {
            ViewBag.Title = "Rekap Perencanaan Pengadaan Penyedia";
            ViewBag.queryTipePaket = "new ej.data.Query().addParams('tipepaket', 1)";
            return View();
        }

        [Route("IndexLapSwakelola")]
        public ActionResult IndexLapSwakelola()
        {
            ViewBag.Title = "Rekap Perencanaan Swakelola";
            ViewBag.queryTipePaket = "new ej.data.Query().addParams('tipepaket', 2)";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiPengadaanAll")]
        public IActionResult UrlDataIdentiifikasi([FromBody] tsIdentifikasiModel dm)
        {
            IEnumerable DataSource = _tsIdentifikasi.GetAll(Int32.Parse(dm.jeniskebutuhan), dm.idpaket);
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
        [Route("IdentifikasiPengadaanPenetapan")]
        public IActionResult UrlDataIdentiifikasiPenetapan([FromBody] tsIdentifikasiModel dm)
        {
            IEnumerable DataSource = _tsIdentifikasi.GetAllLaporan(dm.tipepaket);
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
        [ValidateAntiForgeryToken]
        [Route("IdentifikasiPengadaanCrud")]
        public ActionResult CrudIdentifikasi([FromBody] CRUDModel<tsIdentifikasiModel> value, string action)
        {
            string msg = string.Empty;
            string idpaket = string.Empty;

            foreach (var param in value.Params)
            {
                if ((value.Action == "insert") || (value.Action == "update"))
                {
                    if (param.Key == "jeniskebutuhan")
                    {
                        value.Value.jeniskebutuhan = param.GetType().GetProperty("Value").GetValue(param).ToString();
                    }
                    if (param.Key == "idpaket")
                    {
                        value.Value.idpaket = param.GetType().GetProperty("Value").GetValue(param).ToString();
                        idpaket = value.Value.idpaket;
                    }
                }
                else if ((value.Action == "remove") && param.Key == "idpaket")
                {
                    idpaket = param.Value.ToString();
                }
            }

            if ((HttpContext.Session.GetInt32("Tipe") == 3) && (HttpContext.Session.GetString("Pappk") == "3"))
            {
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
                    DatabaseActionResultModel Result = _tsIdentifikasi.Remove(Int32.Parse(value.Key.ToString()), idpaket);
                    msg = Result.Pesan;
                }
            }
            else
            {
                msg = "GAGAL DISIMPAN";
            }

            return Json(new { data = value.Value, message = msg });
        }
        #endregion Perencanaan Pengadaan

        #region Identifikasi Barang
        //[Route("IndexBarang")]
        [Route("IndexBarang/{id}")]
        public ActionResult IndexBarang(string id)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Barang";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('idpaket', '" + id + "')";
            ViewBag.LinkPaket = "IndexBarang";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiBarangTemplate")]
        public IActionResult PartialTemplateIdentifikasiBarang([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Barang " + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "1";
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.opd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.querySubKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 3)";

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
            }
            else
            {
                ViewBag.truefalse = true;
            }
            #endregion Enable

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.banyakterbatas = new enumDataModel().BanyakTerbatas();
            ViewBag.prioritas = new enumDataModel().Prioritas();
            ViewBag.pengoperasian = new enumDataModel().Pengoperasian();
            ViewBag.rekomendasi = new enumDataModel().Rekomendasi();
            ViewBag.sumberDana = new enumDataModel().SumberDana();
            ViewBag.kelayakan = new enumDataModel().Kelayakan();
            ViewBag.satuan = new enumDataModel().Satuan().Where(s => s.Value != "0");
            #endregion Combobox

            return PartialView("_tsIdentifikasiBarangTemplate", value.Value);
        }
        #endregion Identifikasi Barang

        #region Identifikasi Pekerjaan
        [Route("IndexPekerjaan/{id}")]
        public ActionResult IndexPekerjaan(string id)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Pekerjaan Kontruksi";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 2).addParams('idpaket', '" + id + "')";
            ViewBag.LinkPaket = "IndexPekerjaan";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiPekerjaanTemplate")]
        public IActionResult PartialTemplateIdentifikasiPekerjaan([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Pekerjaan Kontruksi" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "2";
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.opd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.querySubKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 3)";

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
            }
            else
            {
                ViewBag.truefalse = true;
            }
            #endregion Enable

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.banyakterbatas = new enumDataModel().BanyakTerbatas();
            ViewBag.prioritas = new enumDataModel().Prioritas();
            ViewBag.pengoperasian = new enumDataModel().Pengoperasian();
            ViewBag.rekomendasi = new enumDataModel().Rekomendasi();
            ViewBag.sumberDana = new enumDataModel().SumberDana();
            ViewBag.kelayakan = new enumDataModel().Kelayakan();
            ViewBag.sudahbelum = new enumDataModel().SudahBelum();
            ViewBag.komplekssederhana = new enumDataModel().KompleksSederhana();
            ViewBag.barangmaterial = new enumDataModel().BarangMaterial();
            ViewBag.satuan = new enumDataModel().Satuan().Where(s => s.Value != "0");
            #endregion Combobox

            return PartialView("_tsIdentifikasiPekerjaanTemplate", value.Value);
        }
        #endregion Identifikasi Pekerjaan

        #region Identifikasi Jasa Konsultasi
        [Route("IndexKonsultasi/{id}")]
        public ActionResult IndexKonsultasi(string id)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Konsultasi";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 3).addParams('idpaket', '" + id + "')";
            ViewBag.LinkPaket = "IndexKonsultasi";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiKonsultasiTemplate")]
        public IActionResult PartialTemplateIdentifikasiKonsultasi([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Konsultasi" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "3";
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.opd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.querySubKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 3)";

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
            }
            else
            {
                ViewBag.truefalse = true;
            }
            #endregion Enable

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.banyakterbatas = new enumDataModel().BanyakTerbatas();
            ViewBag.prioritas = new enumDataModel().Prioritas();
            ViewBag.pengoperasian = new enumDataModel().Pengoperasian();
            ViewBag.rekomendasi = new enumDataModel().Rekomendasi();
            ViewBag.sumberDana = new enumDataModel().SumberDana();
            ViewBag.kelayakan = new enumDataModel().Kelayakan();
            ViewBag.satuan = new enumDataModel().Satuan().Where(s => s.Value != "0");
            #endregion Combobox

            return PartialView("_tsIdentifikasiKonsultasiTemplate", value.Value);
        }
        #endregion Identifikasi Jasa Konsultasi

        #region Identifikasi Jasa Lainnya
        [Route("IndexLainnya/{id}")]
        public ActionResult IndexLainnya(string id)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Lainnya";
            ViewBag.queryIdentifikasi = "new ej.data.Query().addParams('jeniskebutuhan', 4).addParams('idpaket', '" + id + "')";
            ViewBag.LinkPaket = "IndexLainnya";
            return View();
        }

        [HttpPost]
        [Route("IdentifikasiLainnyaTemplate")]
        public IActionResult PartialTemplateIdentifikasiLainnya([FromBody] CRUDModel<tsIdentifikasiModel> value)
        {
            ViewBag.Title = "Identifikasi Kebutuhan Jasa Lainnya" + value.Value.ididetifikasi;
            value.Value.jeniskebutuhan = "4";
            value.Value.opd = value.Value.opd == null ? HttpContext.Session.GetString("OpdName") : value.Value.opd;
            value.Value.pejabat = value.Value.pejabat == null ? HttpContext.Session.GetString("Nama") : value.Value.pejabat;

            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryKodeRekening = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 6)";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.querySubKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 3)";

            #region Enable
            if ((HttpContext.Session.GetInt32("Tipe") == 2) || (HttpContext.Session.GetInt32("Tipe") == 4))
            {
                ViewBag.truefalse = false;
            }
            else
            {
                ViewBag.truefalse = true;
            }
            #endregion Enable

            #region Combobox
            ViewBag.yatidak = new enumDataModel().YaTidak();
            ViewBag.banyakterbatas = new enumDataModel().BanyakTerbatas();
            ViewBag.prioritas = new enumDataModel().Prioritas();
            ViewBag.pengoperasian = new enumDataModel().Pengoperasian();
            ViewBag.rekomendasi = new enumDataModel().Rekomendasi();
            ViewBag.sumberDana = new enumDataModel().SumberDana();
            ViewBag.kelayakan = new enumDataModel().Kelayakan();
            ViewBag.satuan = new enumDataModel().Satuan().Where(s => s.Value != "0");
            #endregion Combobox

            return PartialView("_tsIdentifikasiLainnyaTemplate", value.Value);
        }
        #endregion Identifikasi Jasa Lainnya

        #region Surat Penetapan
        [Route("SuratPenetapan")]
        public IActionResult SuratPenetapan()
        {
            //ViewBag.Title = "Surat Penetapan";
            //ViewBag.opd = HttpContext.Session.GetString("OpdName");
            //ViewBag.pejabat = HttpContext.Session.GetString("Nama");
            //return PartialView("_tsIdentifikasiSuratPenetapan"); 
            IEnumerable DataSource = _tsIdentifikasi.GetAllDasar();

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];

                //Adding a picture
                FileStream imageStream = new FileStream("wwwroot/images/logo/Logo_Kota_Tasikmalaya.png", FileMode.Open, FileAccess.Read);
                IPictureShape shape = worksheet.Pictures.AddPicture(1, 1, imageStream);

                //Positioning a Picture
                shape.Top = 10;
                shape.Left = 305;
                //shape.Left = 255;

                //Re-sizing a Picture
                shape.Height = 100;
                shape.Width = 100;

                ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();

                IList<SuratPenetapanModel> reports = DataSource.AsQueryable().Cast<SuratPenetapanModel>().ToList();
                //var visualData = reports.Select(xy => new { xy.opd, xy.urutdasar, xy.ketdasar, xy.tipedasar, xy.namappk, xy.nipppk}).ToList();

                int m = 13;
                int a = m;
                int t = 0;

                //Apply style to headers 
                worksheet["A7:G7"].Merge();
                foreach (var opd in reports.GroupBy(c => new { c.opd }).Select(sg => new { sg.Key }))
                {
                    worksheet["A7"].Text = opd.Key.opd.ToUpper();
                }

                worksheet["A10:G10"].Merge();
                worksheet["A10"].Text = "SURAT PENETAPAN";

                worksheet["A11:G11"].Merge();
                worksheet["A11"].Text = "Nomor: ...........................................";

                worksheet["A" + m].Text = "Menimbang";
                worksheet["B" + m].Text = ":";
                foreach (var menimbang in reports.Where(rs => rs.tipedasar.Contains("menimbang")).Select(mb => new { mb.urutdasar, mb.ketdasar }))
                {
                    worksheet["C" + m].Text = menimbang.urutdasar;
                    //worksheet.Range["F" + m].ColumnWidth = 50;
                    worksheet["D" + m + ":" + "G" + m].Merge();
                    worksheet["D" + m].Text = menimbang.ketdasar;
                    worksheet.Range["D" + m].RowHeight = 35;

                    m = m + 1;
                }

                m = m + 1;

                worksheet["A" + m].Text = "Dasar";
                worksheet["B" + m].Text = ":";
                foreach (var dasar in reports.Where(rs => rs.tipedasar.Contains("dasar")).Select(mb => new { mb.urutdasar, mb.ketdasar }))
                {
                    worksheet["C" + m].Text = dasar.urutdasar;
                    //worksheet.Range["F" + m].ColumnWidth = 50;
                    worksheet["D" + m + ":" + "G" + m].Merge();
                    worksheet["D" + m].Text = dasar.ketdasar;
                    worksheet.Range["D" + m].RowHeight = 35;

                    m = m + 1;
                }

                t = m + 1;

                worksheet["A" + t + ":" + "G" + t].Merge();
                worksheet["A" + t].Text = "MENETAPKAN";

                m = t + 2;
                foreach (var tahun in reports.GroupBy(c => new { c.tahun }).Select(sg => new { sg.Key }))
                {
                    worksheet["A" + m].Text = "Kesatu";
                    worksheet["B" + m].Text = ":";
                    worksheet["C" + m + ":" + "G" + m].Merge();
                    worksheet["C" + m].Text = String.Format("Perencanaan Pengadaan Barang/Jasa Pemerintah dalam lingkup (Satuan Kerja / Perangkat Daerah) Tahun Anggaran " + tahun.Key.tahun.ToString());
                    worksheet.Range["C" + m].RowHeight = 35;

                    m = m + 1;

                    worksheet["A" + m].Text = "Kedua";
                    worksheet["B" + m].Text = ":";
                    worksheet["C" + m + ":" + "G" + m].Merge();
                    worksheet["C" + m].Text = String.Format("Hasil Perencanaan Pengadaan dituangkan ke dalam Rencana Umum Pengadaan Barang / Jasa Pemerintah Tahun Anggaran " + tahun.Key.tahun.ToString());
                    worksheet.Range["C" + m].RowHeight = 35;

                    m = m + 1;

                    worksheet["A" + m].Text = "Ketiga";
                    worksheet["B" + m].Text = ":";
                    worksheet["C" + m + ":" + "G" + m].Merge();
                    worksheet["C" + m].Text = "Penetapan ini mulai berlaku pada tanggal ditetapkan. ";
                    worksheet.Range["C" + m].RowHeight = 35;
                }

                m = m + 1;

                //Footer Laporan
                foreach (var footr in reports.GroupBy(c => new { c.namappk, c.nipppk }).Select(sg => new { sg.Key }))
                {
                    //worksheet["F" + (m + 2) + ":" + "H" + (m + 2)].Merge();
                    worksheet["G" + (m + 2)].Text = "TASIKMALAYA, " + formatindonesia();
                    //worksheet["F" + (m + 3) + ":" + "H" + (m + 3)].Merge();
                    worksheet["G" + (m + 3)].Text = "Pengguna Anggaran,";
                    //worksheet["F" + (m + 8) + ":" + "H" + (m + 8)].Merge();
                    worksheet["G" + (m + 8)].Text = footr.Key.namappk.ToUpper();
                    //worksheet["F" + (m + 9) + ":" + "H" + (m + 9)].Merge();
                    worksheet["G" + (m + 9)].Text = "NIP. " + footr.Key.nipppk.ToUpper();
                }


                IStyle menimbangStyle = workbook.Styles.Add("MenimbangStyle");
                menimbangStyle.BeginUpdate();
                menimbangStyle.VerticalAlignment = ExcelVAlign.VAlignTop;
                menimbangStyle.HorizontalAlignment = ExcelHAlign.HAlignJustify;
                menimbangStyle.WrapText = true;
                menimbangStyle.EndUpdate();
                //worksheet.Range["C" + a + ":" + "H" + (t-1)].CellStyle = menimbangStyle;
                worksheet.Range["C" + a + ":" + "G" + m].CellStyle = menimbangStyle;

                IStyle menetapkanDepanStyle = workbook.Styles.Add("MenetapkanDepanStyle");
                menetapkanDepanStyle.BeginUpdate();
                menetapkanDepanStyle.VerticalAlignment = ExcelVAlign.VAlignTop;
                menetapkanDepanStyle.EndUpdate();
                worksheet.Range["A" + a + ":" + "B" + m].CellStyle = menetapkanDepanStyle;

                IStyle headingStyleCenter = workbook.Styles.Add("HeadingStyle");
                headingStyleCenter.BeginUpdate();
                headingStyleCenter.Font.Bold = true;
                headingStyleCenter.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                headingStyleCenter.EndUpdate();
                worksheet.Range["A7:G11"].CellStyle = headingStyleCenter;
                worksheet.Range["A" + t + ":" + "G" + t].CellStyle = headingStyleCenter;

                //IStyle menetapkanStyle = workbook.Styles.Add("MenetapkanStyle");
                //menetapkanStyle.BeginUpdate();
                //menetapkanStyle.VerticalAlignment = ExcelVAlign.VAlignTop;
                //menetapkanStyle.HorizontalAlignment = ExcelHAlign.HAlignJustify;
                //menetapkanStyle.WrapText = true;
                //menetapkanStyle.EndUpdate();
                //worksheet.Range["C" + (t + 1) + ":" + "H" + m].CellStyle = menetapkanStyle;

                IStyle footerStyleCenter = workbook.Styles.Add("FooterStyle");
                footerStyleCenter.BeginUpdate();
                footerStyleCenter.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                footerStyleCenter.EndUpdate();
                worksheet.Range["G" + (m + 2) + ":G" + (m + 10)].CellStyle = footerStyleCenter;

                worksheet.UsedRange.AutofitColumns();
                //worksheet.UsedRange.AutofitRows();
                worksheet.Range["C" + a + ":" + "G" + (t - 1)].AutofitRows();
                //worksheet.Range["C" + (t + 1) + ":" + "H" + m].AutofitRows();

                //worksheet.HPageBreaks.Add(worksheet.Range["H" + (m + 1)]);

                //Saving the workbook as stream
                MemoryStream stream = new MemoryStream();


                //--## Excel Save ##--
                //workbook.SaveAs(stream);
                //stream.Position = 0;

                ////Download the Excel file in the browser
                //FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                //fileStreamResult.FileDownloadName = "Surat_Penetapan.xlsx";
                //return fileStreamResult;
                //--## Excel Save ##--

                //--## Excel To PDF Save ##--
                ////Initialize XlsIO renderer.
                //XlsIORenderer renderer = new XlsIORenderer();

                ////Convert Excel document into PDF document 
                //PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);
                //pdfDocument.Save(stream);
                //stream.Position = 0;

                ////Download the PDF file in the browser
                //FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

                //fileStreamResult.FileDownloadName = "Surat_Penetapan.pdf";
                //return fileStreamResult;
                //--## Excel To PDF Save ##--

                //--## to pdfviewer ##--
                string sFileName = @"Surat_Penetapan_" + System.Guid.NewGuid().ToString() + ".pdf";
                XlsIORenderer renderer = new XlsIORenderer();

                //Convert Excel document into PDF document 
                PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);
                pdfDocument.Save(stream);
                stream.Position = 0;

                string path = Path.Combine(HostEnvironment.WebRootPath, _Folder, sFileName);
                using (FileStream outputFileStream = new FileStream(path, FileMode.Create))
                {
                    stream.CopyTo(outputFileStream);
                }

                ViewBag.Title = sFileName;
                ViewBag.DocumentPath = Path.Combine(HostEnvironment.WebRootPath, _Folder, sFileName);
                return View("~/Views/PdfViewer/Default.cshtml");
                //--## to pdfviewer ##--
            }
        }
        #endregion Surat Penetapan

        #region tglIndonesia
        public string formatindonesia()
        {
            string[] hari = { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
            string[] bulan = { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember" };

            string hariIni = hari[(int)DateTime.Today.DayOfWeek];
            string bulanIni = bulan[DateTime.Today.Month];
            //string TglIndonesia = string.Format("{0}-{1}-{2} ({3})", DateTime.Today.Day, bulanIni, DateTime.Today.Year, hariIni);
            string TglIndonesia = string.Format("{0}-{1}-{2}", DateTime.Today.Day, bulanIni, DateTime.Today.Year);
            return TglIndonesia;
        }
        #endregion tglIndonesia

        #region Laporan Perencanaan Pengadaan
        [Route("ExcelExportIdentifikasi")]
        public ActionResult ExcelExportIdentifikasi(string GridModel)
        {
            ExportModel exportModel = new ExportModel();
            exportModel = (ExportModel)JsonConvert.DeserializeObject(GridModel, typeof(ExportModel));  // Deserialized the GridModel

            IEnumerable DataSource = _tsIdentifikasi.GetAllLaporan(0);
            DataOperations operation = new DataOperations();

            if (exportModel.Sorted != null && exportModel.Sorted.Count > 0) // Perform Sorting
            {
                DataSource = operation.PerformSorting(DataSource, exportModel.Sorted);
            }
            if (exportModel.Where != null && exportModel.Where.Count > 0) //Perform Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, exportModel.Where, exportModel.Where[0].Operator);
            }

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet;
                ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();

                IList<tsIdentifikasiModel> subkegiatan = DataSource.AsQueryable().Cast<tsIdentifikasiModel>().ToList();

                foreach (var subkegiatanName in subkegiatan.GroupBy(c => new { c.opd, c.subopd, c.namappk, c.nipppk, c.namaprogram, c.namakegiatan, c.namasubkegiatan }).Select(sg => new { sg.Key }))
                {
                    worksheet = workbook.Worksheets.Create(subkegiatanName.Key.namasubkegiatan);

                    IList<tsIdentifikasiModel> reports = DataSource.AsQueryable().Cast<tsIdentifikasiModel>().ToList();
                    var visualData = reports.Where(rs => rs.namasubkegiatan.Contains(subkegiatanName.Key.namasubkegiatan)).Select(vd => new { vd.no, vd.namabrgkerj, vd.kriteria, vd.usahakecil, vd.uraian, vd.lokasi, vd.jeniskebutuhan, vd.kbki, vd.tipepaketnama, vd.namapaket, vd.spesifikasi, vd.jumlahbarang, vd.satuan, vd.tipeswakelolapaket, vd.subopd, vd.metodepemilihan, vd.pelaksanaanmulai, vd.pelaksanaanakhir, vd.nilaisumberdana, vd.valuesumberdana }).ToList();
                    int FirstRow = 11;
                    int CountData = visualData.Count;

                    //Apply style to headers 
                    worksheet["A1:T1"].Merge();
                    worksheet["A1"].Text = "RENCANA PENGADAAN " + subkegiatanName.Key.opd.ToUpper();

                    //Header Laporan
                    worksheet["A3:B3"].Merge();
                    worksheet["A3"].Text = "Pemerintah Daerah";
                    worksheet["C3"].Text = ": TASIKMALAYA";

                    worksheet["A4:B4"].Merge();
                    worksheet["A4"].Text = "Perangkat Daerah";
                    worksheet["C4:J4"].Merge();
                    worksheet["C4"].Text = ": " + subkegiatanName.Key.subopd.ToUpper();

                    worksheet["A5:B5"].Merge();
                    worksheet["A5"].Text = "Tahun Anggaran";
                    worksheet["C5:J5"].Merge();
                    worksheet["C5"].Text = ": " + TA;

                    worksheet["A6:B6"].Merge();
                    worksheet["A6"].Text = "Program";
                    worksheet["C6:J6"].Merge();
                    worksheet["C6"].Text = ": " + subkegiatanName.Key.namaprogram;

                    worksheet["A7:B7"].Merge();
                    worksheet["A7"].Text = "Kegiatan";
                    worksheet["C7:J7"].Merge();
                    worksheet["C7"].Text = ": " + subkegiatanName.Key.namakegiatan;

                    worksheet["A8:B8"].Merge();
                    worksheet["A8"].Text = "Sub Kegiatan";
                    worksheet["C8:J8"].Merge();
                    worksheet["C8"].Text = ": " + subkegiatanName.Key.namasubkegiatan;

                    //Footer Laporan
                    worksheet["K" + (FirstRow + (CountData + 2)) + ":N" + (FirstRow + (CountData + 2))].Merge();
                    worksheet["K" + (FirstRow + CountData + 2)].Text = "TASIKMALAYA, " + formatindonesia();
                    worksheet["K" + (FirstRow + (CountData + 3)) + ":N" + (FirstRow + (CountData + 3))].Merge();
                    worksheet["K" + (FirstRow + CountData + 3)].Text = "Disusun oleh,";
                    worksheet["K" + (FirstRow + (CountData + 4)) + ":N" + (FirstRow + (CountData + 4))].Merge();
                    worksheet["K" + (FirstRow + CountData + 4)].Text = "Pejabat Pembuat Komitmen,";
                    worksheet["K" + (FirstRow + (CountData + 9)) + ":N" + (FirstRow + (CountData + 9))].Merge();
                    worksheet["K" + (FirstRow + CountData + 9)].Text = subkegiatanName.Key.namappk.ToUpper();
                    worksheet["K" + (FirstRow + (CountData + 10)) + ":N" + (FirstRow + (CountData + 10))].Merge();
                    worksheet["K" + (FirstRow + CountData + 10)].Text = "NIP. " + subkegiatanName.Key.nipppk.ToUpper();

                    //Judul Header Tabel
                    worksheet["A10"].Text = "No";
                    worksheet["B10"].Text = "Nama Barang/ Jasa";
                    worksheet["C10"].Text = "Kriteria Barang / Jasa";
                    worksheet["D10"].Text = "Kriteria Pelaku Usaha";
                    worksheet["E10"].Text = "Uraian Pekerjaan";
                    worksheet["F10"].Text = "Lokasi Pekerjaan";
                    worksheet["G10"].Text = "Jenis Pengadaan";
                    worksheet["H10"].Text = "Kodefikasi Barang / Jasa";
                    worksheet["I10"].Text = "Cara Pengadaan";
                    worksheet["J10"].Text = "Nama Paket";
                    worksheet["K10"].Text = "Spesifikasi";
                    worksheet["L10"].Text = "Kuantitas";
                    worksheet["M10"].Text = "Satuan";
                    worksheet["N10"].Text = "Tipe Swakelola";
                    worksheet["O10"].Text = "Penyelenggara Swakelola";
                    worksheet["P10"].Text = "Metode Pemilihan";
                    worksheet["Q10"].Text = "Jadwal Pelaksanaan Mulai";
                    worksheet["R10"].Text = "Jadwal Pelaksanaan Akhir";
                    worksheet["S10"].Text = "Anggaran Pengadaan";
                    worksheet["T10"].Text = "Sumber Dana";

                    IStyle headingStyleLeft = workbook.Styles.Add("HeadingStyleHeader" + subkegiatanName.Key.subopd + subkegiatanName.Key.namaprogram + subkegiatanName.Key.namakegiatan + subkegiatanName.Key.namasubkegiatan);
                    headingStyleLeft.BeginUpdate();
                    headingStyleLeft.Font.Bold = true;
                    headingStyleLeft.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                    headingStyleLeft.EndUpdate();
                    worksheet.Range["A3:A8"].CellStyle = headingStyleLeft;

                    IStyle headingStyleCenter = workbook.Styles.Add("HeadingStyle" + subkegiatanName.Key.subopd + subkegiatanName.Key.namaprogram + subkegiatanName.Key.namakegiatan + subkegiatanName.Key.namasubkegiatan);
                    headingStyleCenter.BeginUpdate();
                    headingStyleCenter.Font.Bold = true;
                    headingStyleCenter.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    headingStyleCenter.EndUpdate();
                    worksheet.Range["A1:C1"].CellStyle = headingStyleCenter;
                    worksheet.Range["B" + (FirstRow + (CountData + 2)) + ":B" + (FirstRow + (CountData + 3))].CellStyle = headingStyleCenter;

                    IStyle headingTitleBorder = workbook.Styles.Add("HeadingTitleBorder" + subkegiatanName.Key.subopd + subkegiatanName.Key.namaprogram + subkegiatanName.Key.namakegiatan + subkegiatanName.Key.namasubkegiatan);
                    headingTitleBorder.BeginUpdate();
                    headingTitleBorder.Font.Bold = true;
                    headingTitleBorder.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    headingTitleBorder.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    headingTitleBorder.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    headingTitleBorder.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    headingTitleBorder.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    headingTitleBorder.EndUpdate();
                    worksheet.Range["A10:T10"].CellStyle = headingTitleBorder;

                    IStyle headingBorder = workbook.Styles.Add("HeadingBorder" + subkegiatanName.Key.subopd + subkegiatanName.Key.namaprogram + subkegiatanName.Key.namakegiatan + subkegiatanName.Key.namasubkegiatan);
                    headingBorder.BeginUpdate();
                    headingBorder.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                    headingBorder.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                    headingBorder.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                    headingBorder.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                    headingBorder.EndUpdate();
                    worksheet.Range["A" + FirstRow + ":T" + (FirstRow + (CountData - 1))].CellStyle = headingBorder;

                    IStyle footerStyleCenter = workbook.Styles.Add("FooterStyle" + subkegiatanName.Key.subopd + subkegiatanName.Key.namaprogram + subkegiatanName.Key.namakegiatan + subkegiatanName.Key.namasubkegiatan);
                    footerStyleCenter.BeginUpdate();
                    footerStyleCenter.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                    footerStyleCenter.EndUpdate();
                    worksheet.Range["K" + (FirstRow + (CountData + 2)) + ":N" + (FirstRow + (CountData + 10))].CellStyle = footerStyleCenter;

                    importDataOptions.FirstRow = FirstRow;
                    importDataOptions.IncludeHeader = false;
                    importDataOptions.NestedDataLayoutOptions = ExcelNestedDataLayoutOptions.Default;
                    worksheet.ImportData(visualData, importDataOptions);

                    worksheet.UsedRange.AutofitColumns();
                    worksheet.Protect(",6Gn]a3VX@whh`>z=,]Ps6Jy<j4.x[n aGA:$-C`.,Kmf)93nYXjS2Q~rGP@, CZb8DmQ9tCh = SDY_RI - &{={pN3m`KxL % B9 + k", ExcelSheetProtection.All);
                }

                IWorksheet hideworksheet = workbook.Worksheets[0];
                hideworksheet.Visibility = WorksheetVisibility.Hidden;

                MemoryStream stream = new MemoryStream();
                //--## Excel Save ##--

                //--## Protect ##--
                bool isProtectWindow = true;
                bool isProtectContent = true;

                //Protect Workbook
                //workbook.SetWriteProtectionPassword(",6Gn]a3VX@whh`>z=,]Ps6Jy<j4.x[n aGA:$-C`.,Kmf)93nYXjS2Q~rGP@, CZb8DmQ9tCh = SDY_RI - &{={pN3m`KxL % B9 + k");
                workbook.Protect(isProtectWindow, isProtectContent, ",6Gn]a3VX@whh`>z=,]Ps6Jy<j4.x[n aGA:$-C`.,Kmf)93nYXjS2Q~rGP@, CZb8DmQ9tCh = SDY_RI - &{={pN3m`KxL % B9 + k");

                workbook.SaveAs(stream);
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                fileStreamResult.FileDownloadName = "Laporan_Identifikasi.xlsx";
                //--## Excel Save ##--

                //--## Excel To PDF Save ##--
                ////Initialize XlsIO renderer.
                //XlsIORenderer renderer = new XlsIORenderer();

                ////Convert Excel document into PDF document 
                //PdfDocument pdfDocument = renderer.ConvertToPDF(workbook);
                //pdfDocument.Save(stream);
                //stream.Position = 0;

                ////Download the PDF file in the browser
                //FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/pdf");

                //fileStreamResult.FileDownloadName = "Laporan_Identifikasi.pdf";
                //--## Excel To PDF Save ##--

                return fileStreamResult;
            }
        }
        #endregion Laporan Perencanaan Pengadaan

        #region Laporan Identifikasi Swakelola
        [Route("ExcelExportIdentifikasiSwakelola")]
        public ActionResult ExcelExportIdentifikasiSwakelola(string GridModel)
        {
            ExportModel exportModel = new ExportModel();
            exportModel = (ExportModel)JsonConvert.DeserializeObject(GridModel, typeof(ExportModel));  // Deserialized the GridModel

            IEnumerable DataSource = _tsIdentifikasi.GetAllLaporan(2);
            DataOperations operation = new DataOperations();

            if (exportModel.Sorted != null && exportModel.Sorted.Count > 0) // Perform Sorting
            {
                DataSource = operation.PerformSorting(DataSource, exportModel.Sorted);
            }
            if (exportModel.Where != null && exportModel.Where.Count > 0) //Perform Filtering
            {
                DataSource = operation.PerformFiltering(DataSource, exportModel.Where, exportModel.Where[0].Operator);
            }

            using (ExcelEngine excelEngine = new ExcelEngine())
            {
                IApplication application = excelEngine.Excel;
                application.DefaultVersion = ExcelVersion.Excel2013;
                IWorkbook workbook = application.Workbooks.Create(1);
                IWorksheet worksheet = workbook.Worksheets[0];
                ExcelImportDataOptions importDataOptions = new ExcelImportDataOptions();

                IList<tsIdentifikasiModel> reports = DataSource.AsQueryable().Cast<tsIdentifikasiModel>().ToList();
                var visualData = reports.Select(xy => new { xy.jeniskebutuhan, xy.namabrgkerj, xy.tipeswakelolapaket }).ToList();
                int FirstRow = 11;
                int CountData = visualData.Count;

                //Apply style to headers 
                worksheet["A1:C1"].Merge();
                worksheet["A1"].Text = "Rencana Umum Pengadaan Swakelola";

                //Header Laporan
                worksheet["A3"].Text = "Pemerintah Daerah";
                worksheet["B3"].Text = ": Tasikmalaya";

                worksheet["A4"].Text = "Perangkat Daerah";
                worksheet["B4"].Text = ": " + OPD;

                worksheet["A5"].Text = "Tahun Anggaran";
                worksheet["B5"].Text = ": " + TA;

                worksheet["A6"].Text = "Program";
                worksheet["B6"].Text = ":";

                worksheet["A7"].Text = "Kegiatan";
                worksheet["B7"].Text = ":";

                worksheet["A8"].Text = "Sub Kegiatan";
                worksheet["B8"].Text = ":";

                //Footer Laporan
                worksheet["B" + (FirstRow + CountData + 2)].Text = "Disusun oleh,";
                worksheet["B" + (FirstRow + CountData + 3)].Text = "Pejabat Pembuat Komitmen,";

                //Judul Header Tabel
                worksheet["A10"].Text = "Jenis Pengadaan";
                worksheet["B10"].Text = "Nama Barang/ Jasa";
                worksheet["C10"].Text = "Tipe Swakelola";

                IStyle headingStyleLeft = workbook.Styles.Add("HeadingStyleHeader");
                headingStyleLeft.BeginUpdate();
                headingStyleLeft.Font.Bold = true;
                headingStyleLeft.HorizontalAlignment = ExcelHAlign.HAlignLeft;
                headingStyleLeft.EndUpdate();
                worksheet.Range["A3:A8"].CellStyle = headingStyleLeft;

                IStyle headingStyleCenter = workbook.Styles.Add("HeadingStyle");
                headingStyleCenter.BeginUpdate();
                headingStyleCenter.Font.Bold = true;
                headingStyleCenter.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                headingStyleCenter.EndUpdate();
                worksheet.Range["A1:C1"].CellStyle = headingStyleCenter;
                worksheet.Range["B" + (FirstRow + (CountData + 2)) + ":B" + (FirstRow + (CountData + 3))].CellStyle = headingStyleCenter;

                IStyle headingTitleBorder = workbook.Styles.Add("HeadingTitleBorder");
                headingTitleBorder.BeginUpdate();
                headingTitleBorder.Font.Bold = true;
                headingTitleBorder.HorizontalAlignment = ExcelHAlign.HAlignCenter;
                headingTitleBorder.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                headingTitleBorder.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                headingTitleBorder.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                headingTitleBorder.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                headingTitleBorder.EndUpdate();
                worksheet.Range["A10:C10"].CellStyle = headingTitleBorder;

                IStyle headingBorder = workbook.Styles.Add("HeadingBorder");
                headingBorder.BeginUpdate();
                headingBorder.Borders[ExcelBordersIndex.EdgeLeft].LineStyle = ExcelLineStyle.Thin;
                headingBorder.Borders[ExcelBordersIndex.EdgeRight].LineStyle = ExcelLineStyle.Thin;
                headingBorder.Borders[ExcelBordersIndex.EdgeTop].LineStyle = ExcelLineStyle.Thin;
                headingBorder.Borders[ExcelBordersIndex.EdgeBottom].LineStyle = ExcelLineStyle.Thin;
                headingBorder.EndUpdate();
                worksheet.Range["A" + FirstRow + ":C" + (FirstRow + (CountData - 1))].CellStyle = headingBorder;

                importDataOptions.FirstRow = FirstRow;
                importDataOptions.IncludeHeader = false;
                importDataOptions.NestedDataLayoutOptions = ExcelNestedDataLayoutOptions.Default;
                worksheet.ImportData(visualData, importDataOptions);

                worksheet.UsedRange.AutofitColumns();

                MemoryStream stream = new MemoryStream();
                workbook.SaveAs(stream);
                stream.Position = 0;

                //Download the Excel file in the browser
                FileStreamResult fileStreamResult = new FileStreamResult(stream, "application/excel");

                fileStreamResult.FileDownloadName = "Laporan_Identifikasi_Swakelola.xlsx";

                return fileStreamResult;
            }
        }
        #endregion Laporan Identifikasi Swakelola
    }
}