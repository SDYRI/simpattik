using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    public class tsMcpReview : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult IndexPerencanaan()
        {
            return View();
        }

        public IActionResult IndexPemaketan()
        {
            return View();
        }

        public IActionResult IndexPaketStartegis()
        {
            return View();
        }

        public IActionResult IndexKesesuaianSirup()
        {
            return View();
        }

        public IActionResult IndexUploadHPS()
        {
            return View();
        }

        public IActionResult IndexDatabaseVendor()
        {
            return View();
        }
    }
}
