using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Interfaces;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.mKodeRekening.Models;
using System;
using Syncfusion.EJ2.Navigations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace simpat1k.Controllers
{
    [Route("KodeRekeningMaster")]
    public class mKodeRekeningController : Controller
    {
        private readonly ImKodeRekening _mKodeRekening;

        public mKodeRekeningController(ImKodeRekening mKodeRekening)
        {
            _mKodeRekening = mKodeRekening;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Tipe") == 1)
            {
                ViewBag.Title = "Master Kode Rekening";
                ViewBag.headerAkun = new TabHeader { Text = "Akun" };
                ViewBag.headerKelompok = new TabHeader { Text = "Kelompok" };
                ViewBag.headerJenis = new TabHeader { Text = "Jenis" };
                ViewBag.headerObjek = new TabHeader { Text = "Objek" };
                ViewBag.headerRincian = new TabHeader { Text = "Rincian" };
                ViewBag.headerSubRincian = new TabHeader { Text = "Sub Rincian" };
                ViewBag.queryAkun = "new ej.data.Query().addParams('IdPosisi', 1)";
                ViewBag.queryKelompok = "new ej.data.Query().addParams('IdPosisi', 2)";
                ViewBag.queryJenis = "new ej.data.Query().addParams('IdPosisi', 3)";
                ViewBag.queryObjek = "new ej.data.Query().addParams('IdPosisi', 4)";
                ViewBag.queryRincian = "new ej.data.Query().addParams('IdPosisi', 5)";
                ViewBag.querySubRincian = "new ej.data.Query().addParams('IdPosisi', 6)";
                return View();
            }
            else
            {
                return RedirectToAction("HandleError", "ErrorExecute", new { code = 404 });
            }
        }

        [HttpPost]
        [Route("KodeRekeningMasterAll")]
        public IActionResult UrlDatasource([FromBody] mKodeRekeningModel dm)
        {
            IEnumerable DataSource = _mKodeRekening.GetAll(dm.IdPosisi);
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
            int count = DataSource.Cast<mKodeRekeningModel>().Count();
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
        [Route("KodeRekeningMasterSelect")]
        public IActionResult SelectDatasource([FromBody] mKodeRekeningModel dm)
        {
            IEnumerable DataSource = _mKodeRekening.GetAll(0);
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
            int count = DataSource.Cast<mKodeRekeningModel>().Count();
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
        [Route("KodeRekeningMasterCrud")]
        public IActionResult CrudUpdate([FromBody] CRUDModel<mKodeRekeningModel> value, string action)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (HttpContext.Session.GetInt32("Tipe") == 1)
                    {
                        if (value.Action == "insert")
                        {
                            foreach (var param in value.Params)
                            {
                                value.Value.IdPosisi = Int32.Parse(param.GetType().GetProperty("Value").GetValue(param).ToString());
                            }

                            DatabaseActionResultModel Result = _mKodeRekening.Create(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "update")
                        {
                            DatabaseActionResultModel Result = _mKodeRekening.Update(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "remove")
                        {
                            DatabaseActionResultModel Result = _mKodeRekening.Remove(Int32.Parse(value.Key.ToString()));
                            msg = Result.Pesan;
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
        [Route("KodeRekeningMasterTemplateAkun")]
        public IActionResult PartialTemplateAkun([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.Title = "Master Akun " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateAkun", value.Value);
        }

        [HttpPost]
        [Route("KodeRekeningMasterTemplateKelompok")]
        public IActionResult PartialTemplateKelompok([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryAkun = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.Title = "Master Kelompok " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateKelompok", value.Value);
        }

        [HttpPost]
        [Route("KodeRekeningMasterTemplateJenis")]
        public IActionResult PartialTemplateJenis([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryAkun = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKelompok = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.Title = "Master Jenis " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateJenis", value.Value);
        }

        [HttpPost]
        [Route("KodeRekeningMasterTemplateObjek")]
        public IActionResult PartialTemplateObjek([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryAkun = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKelompok = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.queryJenis = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 3)";
            ViewBag.Title = "Master Objek " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateObjek", value.Value);
        }

        [HttpPost]
        [Route("KodeRekeningMasterTemplateRincian")]
        public IActionResult PartialTemplateRincian([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryAkun = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKelompok = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.queryJenis = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 3)";
            ViewBag.queryObjek = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 4)";
            ViewBag.Title = "Master Rincian " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateRincian", value.Value);
        }

        [HttpPost]
        [Route("KodeRekeningMasterTemplateSubRincian")]
        public IActionResult PartialTemplateSubRincian([FromBody] CRUDModel<mKodeRekeningModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryAkun = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKelompok = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.queryJenis = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 3)";
            ViewBag.queryObjek = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 4)";
            ViewBag.queryRincian = "new ej.data.Query().select(['NamaSubRincian', 'IdKodeRekening']).take(10).requiresCount().addParams('IdPosisi', 5)";
            ViewBag.Title = "Master Rincian " + value.Value.NamaSubRincian;

            return PartialView("_mKodeRekeningTemplateSubRincian", value.Value);
        }

    }
}