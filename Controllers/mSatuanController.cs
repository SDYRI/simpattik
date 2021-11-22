using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TasikmalayaKota.Simpatik.Web.Models;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Models;
using TasikmalayaKota.Simpatik.Web.Services.mSatuan.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mSatuan.Models;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    [Route("SatuanMaster")]
    public class mSatuanController : Controller
    {
        private readonly ImSatuan _mSatuan;

        public mSatuanController(ImSatuan mSatuan)
        {
            _mSatuan = mSatuan;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("SatuanMasterAll")]
        public IActionResult UrlDatasource([FromBody] mSatuanModel dm)
        {
            IEnumerable DataSource = _mSatuan.GetAll();
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
            int count = DataSource.Cast<mSatuanModel>().Count();
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
        [Route("SatuanMasterCrud")]
        public ActionResult CrudUpdate([FromBody] CRUDModel<mSatuanModel> value, string action)
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
                            DatabaseActionResultModel Result = _mSatuan.Create(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "update")
                        {
                            DatabaseActionResultModel Result = _mSatuan.Update(value.Value);
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
        [Route("SatuanMasterTemplate")]
        public IActionResult PartialTemplateSatuan([FromBody] CRUDModel<mSatuanModel> value)
        {
            ViewBag.Title = "Master Satuan " + value.Value.NamaSatuan;

            return PartialView("_mSatuanTemplate", value.Value);
        }
    }
}
