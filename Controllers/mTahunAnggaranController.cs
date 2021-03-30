using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Models;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mTahunAnggaran.Interfaces;
using Syncfusion.EJ2.Base;
using System.Collections;
using System;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace simpat1k.Controllers
{
    [Route("TahunAnggaranMaster")]
    public class mTahunAnggaranController : Controller
   {
        private readonly ImTahunAnggaran _mTahunAnggaran;

        public mTahunAnggaranController(ImTahunAnggaran mTahunAnggaran)
        {
            _mTahunAnggaran = mTahunAnggaran;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            ViewBag.Title = "Master Tahun Anggaran ";
            return View();
        }

        [HttpPost]
        [Route("TahunAnggaranMasterAll")]
        public IActionResult UrlDatasource([FromBody] mTahunAnggaranModel dm)
        {
            IEnumerable DataSource = _mTahunAnggaran.GetAll();
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
            int count = DataSource.Cast<mTahunAnggaranModel>().Count();
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
        [Route("TahunAnggaranMasterCrud")]
        public ActionResult CrudUpdate([FromBody] CRUDModel<mTahunAnggaranModel> value, string action)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (value.Action == "insert")
                    {
                        DatabaseActionResultModel Result = _mTahunAnggaran.Create(value.Value);
                        msg = Result.Pesan;
                        //if (Result.Success)
                        //{
                        //    //model.IdKota = Convert.ToInt32(Result.Data);
                        //    //SaveLogSimelon(model, "129");
                        //}
                    }
                    else if (value.Action == "update")
                    {
                        DatabaseActionResultModel Result = _mTahunAnggaran.Update(value.Value);
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
        [Route("TahunAnggaranMasterTemplate")]
        public IActionResult PartialTemplateUser([FromBody] CRUDModel<mTahunAnggaranModel> value)
        {
            var valTemplate = _mTahunAnggaran.GetAll();
            ViewBag.datasource = valTemplate;
            ViewBag.Title = "Master Tahun Anggaran " + value.Value.TahunAnggaran;

            return PartialView("_mTahunAnggaranTemplate", value.Value);
        }

    }
}