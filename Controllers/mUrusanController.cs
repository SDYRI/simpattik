using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Base;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            return View();
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
    }
}
