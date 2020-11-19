using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Interfaces;
using Syncfusion.EJ2.Base;
using System.Collections;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using System.Dynamic;
using TasikmalayaKota.Simpatik.Web.Services.mOpd.Interfaces;
using TasikmalayaKota.Simpatik.Web.Models;
using System;

namespace simpat1k.Controllers
{
   [Route("UserMaster")]
   public class mUserController : Controller
   {
        private readonly ImUser _mUser;
        private readonly ImOpd _mOpd;

        public mUserController(ImUser mUser, ImOpd mOpd)
		{
            _mUser = mUser;
            _mOpd = mOpd;
        }

        [Route("IndexMe")]
        public IActionResult Index()
        {
            ViewBag.Title = "Master User";
            return View();
        }

        [HttpPost]
        [Route("UserMasterAll")]
        public IActionResult UrlDatasource([FromBody] mUserModel dm)
        {
            IEnumerable DataSource = _mUser.GetAll();
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
            int count = DataSource.Cast<mUserModel>().Count();
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
        [Route("UserMasterCrud")]
        public IActionResult CrudUpdate([FromBody] CRUDModel<mUserModel> value, string action)
        {
            string msg = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {
                    if (value.Action == "insert")
                    {
                        DatabaseActionResultModel Result = _mUser.Create(value.Value);
                        msg = Result.Pesan;
                    }
                    else if (value.Action == "update")
                    {
                        DatabaseActionResultModel Result = _mUser.Update(value.Value);
                        value.Value.ListOpdUser = Result.Data.ToString();
                        msg = Result.Pesan;
                    }
                    else if (value.Action == "remove")
                    {
                        DatabaseActionResultModel Result = _mUser.Remove(value.Key.ToString());
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
        [Route("UserMasterTemplate")]
        public IActionResult PartialTemplateUser([FromBody] CRUDModel<mUserModel> value)
        {
            var valTemplate = _mUser.GetAll();
            ViewBag.datasource = valTemplate;
            //ViewBag.dataOPD = _mOpd.GetAll().ToList();
            ViewBag.sortDropdown = "Ascending";
            ViewBag.queryOPD = "new ej.data.Query().select(['NamaOpd', 'IdOpd']).take(10).requiresCount()";
            ViewBag.Title = "Master User " + value.Value.NamaUser;
            ViewBag.id = 2; //value.Value.ListIdOpdUser;

            return PartialView("_mUserTemplate", value.Value);
        }
    }
}