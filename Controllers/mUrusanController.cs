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
using TasikmalayaKota.Simpatik.Web.Services.mProgram.Models;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Interfaces;
using TasikmalayaKota.Simpatik.Web.Services.mUrusan.Models;

namespace simpat1k.Controllers
{
    [Route("UrusanMaster")]
    public class mUrusanController : Controller
    {
        private readonly ImUrusan _mUrusan;

        public mUrusanController(ImUrusan mUrusan)
        {
            _mUrusan = mUrusan;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            if (HttpContext.Session.GetInt32("Tipe") == 1)
            {
                ViewBag.Title = "Master Urusan, Sub Urusan";
                ViewBag.headerUrusan = new TabHeader { Text = "URUSAN PEMERINTAHAN" };
                ViewBag.headerSubUrusan = new TabHeader { Text = "BIDANG URUSAN" };
                ViewBag.queryUrusan = "new ej.data.Query().addParams('IdPosisi', 1)";
                ViewBag.querySubUrusan = "new ej.data.Query().addParams('IdPosisi', 2)";
                return View();
            }
            else
            {
                return RedirectToAction("HandleError", "ErrorExecute", new { code = 404 });
            }
        }

        [HttpPost]
        [Route("UrusanMasterAll")]
        public IActionResult UrlDatasource([FromBody] mUrusanModel dm)
        {
            IEnumerable DataSource = _mUrusan.GetAll(dm.IdPosisi);
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
            int count = DataSource.Cast<mUrusanModel>().Count();
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
        [Route("UrusanMasterCrud")]
        public IActionResult CrudUpdate([FromBody] CRUDModel<mUrusanModel> value, string action)
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

                            DatabaseActionResultModel Result = _mUrusan.Create(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "update")
                        {
                            DatabaseActionResultModel Result = _mUrusan.Update(value.Value);
                            msg = Result.Pesan;
                        }
                        else if (value.Action == "remove")
                        {
                            DatabaseActionResultModel Result = _mUrusan.Remove(Int32.Parse(value.Key.ToString()));
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
        [Route("UrusanMasterTemplateUrusan")]
        public IActionResult PartialTemplateProgram([FromBody] CRUDModel<mUrusanModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.Title = "Master Urusan " + value.Value.NamaSubUrusan;

            return PartialView("_mUrusanTemplateUrusan", value.Value);
        }

        [HttpPost]
        [Route("UrusanMasterTemplateSubUrusan")]
        public IActionResult PartialTemplateSubUrusan([FromBody] CRUDModel<mUrusanModel> value)
        {
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryUrusan = "new ej.data.Query().select(['NamaSubUrusan', 'IdUrusan']).take(10).requiresCount().addParams('IdPosisi', 1)";
            ViewBag.Title = "Master Sub Urusan " + value.Value.NamaSubUrusan;

            return PartialView("_mUrusanTemplateSubUrusan", value.Value);
        }
    }
}
