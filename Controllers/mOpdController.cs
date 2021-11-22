using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Models;
using Syncfusion.EJ2.Linq;
using System.Collections.Generic;
using Syncfusion.EJ2.Navigations;
using TasikmalayaKota.Simpatik.Web.Models;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.VisualBasic;

namespace simpat1k.Controllers
{
    [Route("OpdMaster")]
    public class mOpdController : Controller
    {
        private readonly ImOpd _mOpd;

        public mOpdController(ImOpd mOpd)
        {
            _mOpd = mOpd;
        }

        [Route("IndexMe")]
        public ActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Tipe") == 1)
            {
                ViewBag.Title = "Master OPD";
                ViewBag.headerOrganisasi = new TabHeader { Text = "ORGANISASI" };
                ViewBag.headerSubOrganisasi = new TabHeader { Text = "UNIT" };
                ViewBag.queryOpd = "new ej.data.Query().addParams('IdPosisi', 1)";
                ViewBag.querySubOpd = "new ej.data.Query().addParams('IdPosisi', 2)";
                return View();
            }
            else
            {
                return RedirectToAction("HandleError", "ErrorExecute", new { code = 404 });
            }
        }

        [HttpPost]
        [Route("OpdMasterAll")]
        public IActionResult UrlDatasource([FromBody] mOpdModel dm)
        {
            IEnumerable DataSource = _mOpd.GetAll(dm.IdPosisi);
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
            int count = DataSource.Cast<mOpdModel>().Count();
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
        [Route("OpdMasterUrusan")]
        public IActionResult UrusanDatasource([FromBody] mOpdModel dm)
        {
            IEnumerable DataSource = _mOpd.GetAllUrusan(dm.IdPosisi, dm.ListIdUrusanCb);
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
            int count = DataSource.Cast<mOpdModel>().Count();
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
        [Route("OpdMasterSelect")]
        public IActionResult SelectDatasource([FromBody] mOpdModel dm)
        {
            IEnumerable DataSource = _mOpd.GetAll(0);
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
            int count = DataSource.Cast<mOpdModel>().Count();
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
        [Route("OpdMasterCrud")]
        public ActionResult CrudUpdate([FromBody] CRUDModel<mOpdModel> value, string action)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (HttpContext.Session.GetInt32("Tipe") == 1)
                    {
                        if(value.Value.ListIdUrusan != null)
                        {
                            if (value.Value.IdPosisi == 2)
                            {
                                value.Value.ListIdUrusanCb = string.Join(",", value.Value.ListIdUrusanCb);
                            }
                            else
                            {
                                value.Value.ListIdUrusanCb = string.Join(",", value.Value.ListIdUrusan);
                            }
                        }
                        else
                        {
                            value.Value.ListIdUrusanCb = string.Join(",", value.Value.ListIdUrusanCb);
                        }

                        if (value.Action == "insert")
                        {
                            foreach (var param in value.Params)
                            {
                                value.Value.IdPosisi = Int32.Parse(param.GetType().GetProperty("Value").GetValue(param).ToString());
                            }

                            DatabaseActionResultModel Result = _mOpd.Create(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "update")
                        {
                            DatabaseActionResultModel Result = _mOpd.Update(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "remove")
                        {
                            DatabaseActionResultModel Result = _mOpd.Remove(Int32.Parse(value.Key.ToString()));
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
        [Route("OpdMasterTemplateUrusan")]
        public IActionResult PartialTemplateUrusan([FromBody] CRUDModel<mOpdModel> value)
        {
            ViewBag.Title = "Master Opd " + value.Value.NamaOpd;

            return PartialView("_mOpdTemplateUrusan", value.Value);
        }

        [HttpPost]
        [Route("OpdMasterTemplateOpd")]
        public IActionResult PartialTemplateOpd([FromBody] CRUDModel<mOpdModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryUrusan = "new ej.data.Query().select(['NamaSubUrusan', 'IdUrusan']).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.Title = "Master Opd " + value.Value.NamaOpd;

            return PartialView("_mOpdTemplateOpd", value.Value);
        }

        [HttpPost]
        [Route("OpdMasterTemplateSubOpd")]
        public IActionResult PartialTemplateSubOpd([FromBody] CRUDModel<mOpdModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryUrusan = "new ej.data.Query().select(['NamaSubOpd', 'IdOpd']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaSubOpd', 'IdOpd']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.Title = "Master Opd " + value.Value.NamaOpd;

            return PartialView("_mOpdTemplateSubOpd", value.Value);
        }

    }
}