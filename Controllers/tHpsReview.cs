using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    public class tHpsReview : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexHps()
        {
            return View();
        }

        public IActionResult IndexReviewHps()
        {
            return View();
        }
    }
}
