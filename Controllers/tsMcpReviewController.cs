using Microsoft.AspNetCore.Mvc;
using Syncfusion.EJ2.Navigations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    [Route("Mcp")]
    public class tsMcpReviewController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Route("IndexPerencanaanMe")]
        public IActionResult IndexPerencanaan()
        {
            ViewBag.Title = "Review Perencanaan";
            ViewBag.identifikasi = "/Mcp/IndexPerencanaanDetailMe/";
            return View();
        }

        [Route("IndexPerencanaanDetailMe/{id}")]
        public IActionResult IndexPerencanaanDetail(string id)
        {
            ViewBag.Title = "Review Perencanaan Detail";
            ViewBag.headerPenyedia = new TabHeader { Text = "Penyedia" };
            ViewBag.headerSwakelola = new TabHeader { Text = "Swakelola" };
            ViewBag.queryPenyedia = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 1)";
            ViewBag.querySwakelola = "new ej.data.Query().addParams('jeniskebutuhan', 1).addParams('tipePaket', 2)";
            return View();
        }

        [Route("IndexPemaketanMe")]
        public IActionResult IndexPemaketan()
        {
            return View();
        }

        [Route("IndexPaketStrategisMe")]
        public IActionResult IndexPaketStartegis()
        {
            return View();
        }

        [Route("IndexKesesuaianSirupMe")]
        public IActionResult IndexKesesuaianSirup()
        {
            return View();
        }

        [Route("IndexUploadHpsMe")]
        public IActionResult IndexUploadHPS()
        {
            return View();
        }

        [Route("IndexDatabaseVendorMe")]
        public IActionResult IndexDatabaseVendor()
        {
            return View();
        }
    }
}
