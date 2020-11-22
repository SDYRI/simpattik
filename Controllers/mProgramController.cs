using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using TasikmalayaKota.Simpatik.Web.Models;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Models;
using System;
using Syncfusion.EJ2.Navigations;

namespace simpat1k.Controllers
{
    [Route("ProgramMaster")]
    public class mProgramController : Controller
    {
        private readonly ImProgram _mProgram;

        public mProgramController(ImProgram mProgram)
        {
            _mProgram = mProgram;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            ViewBag.Title = "Master Program, Kegiatan, Sub Kegiatan";
            ViewBag.headerProgram = new TabHeader { Text = "Program" };
            ViewBag.headerKegiatan = new TabHeader { Text = "Kegiatan" };
            ViewBag.headerSubKegiatan = new TabHeader { Text = "Sub Kegiatan" };
            ViewBag.queryProgram = "new ej.data.Query().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().addParams('IdPosisi', 2)";
            ViewBag.querySubKegiatan = "new ej.data.Query().addParams('IdPosisi', 3)";
            return View();
        }

        [HttpPost]
        [Route("ProgramMasterAll")]
        public IActionResult UrlDatasource([FromBody] mProgramModel dm)
        {
            IEnumerable DataSource = _mProgram.GetAll(dm.IdPosisi);
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
            int count = DataSource.Cast<mProgramModel>().Count();
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
        [Route("ProgramMasterSelect")]
        public IActionResult SelectDatasource([FromBody] mProgramModel dm)
        {
            IEnumerable DataSource = _mProgram.GetAll();
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
            int count = DataSource.Cast<mProgramModel>().Count();
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
        [Route("ProgramMasterCrud")]
        public IActionResult CrudUpdate([FromBody] CRUDModel<mProgramModel> value, string action)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (value.Action == "insert")
                    {
                        foreach (var param in value.Params)
                        {
                            value.Value.IdPosisi = Int32.Parse(param.GetType().GetProperty("Value").GetValue(param).ToString());
                        }

                        DatabaseActionResultModel Result = _mProgram.Create(value.Value);
                        msg = Result.Pesan;
                    }
                    else if (value.Action == "update")
                    {
                        DatabaseActionResultModel Result = _mProgram.Update(value.Value);
                        msg = Result.Pesan;
                    }
                    else if (value.Action == "remove")
                    {
                        DatabaseActionResultModel Result = _mProgram.Remove(Int32.Parse(value.Key.ToString()));
                        msg = Result.Pesan;
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
        [Route("ProgramMasterTemplateProgram")]
        public IActionResult PartialTemplateProgram([FromBody] CRUDModel<mProgramModel> value)
        {
            var valTemplate = _mProgram.GetAll(1);
            ViewBag.datasource = valTemplate;
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.Title = "Master Program " + value.Value.NamaProgram;

            return PartialView("_mProgramTemplateProgram", value.Value);
        }

        [HttpPost]
        [Route("ProgramMasterTemplateKegiatan")]
        public IActionResult PartialTemplateKegiatan([FromBody] CRUDModel<mProgramModel> value)
        {
            var valTemplate = _mProgram.GetAll(2);
            ViewBag.datasource = valTemplate;
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.Title = "Master Kegiatan " + value.Value.NamaProgram;

            return PartialView("_mProgramTemplateKegiatan", value.Value);
        }

        [HttpPost]
        [Route("ProgramMasterTemplateSubKegiatan")]
        public IActionResult PartialTemplateSubKegiatan([FromBody] CRUDModel<mProgramModel> value)
        {
            var valTemplate = _mProgram.GetAll(3);
            ViewBag.datasource = valTemplate;
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.queryProgram = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.queryKegiatan = "new ej.data.Query().select(['NamaSubkegiatan', 'IdProgram']).take(10).requiresCount().addParams('IdPosisi', 2)";
            ViewBag.Title = "Master Sub Kegiatan " + value.Value.NamaProgram;

            return PartialView("_mProgramTemplateSubKegiatan", value.Value);
        }

    }
}