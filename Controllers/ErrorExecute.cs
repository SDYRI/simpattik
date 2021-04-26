using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
    [Route("Error")]
    public class ErrorExecute : Controller
    {
        [Route("/Error/{code:int}")]
        public IActionResult HandleError(int code)
        {
            ViewData["ErrorMessage"] = $"Error occurred. The ErrorCode is: {code}";
            //return View("~/Views/Shared/HandleError.cshtml");
            //if (code == 404)
            //    return View("Error404");
            //else if (code == 405)
            //    return View("Error405");

            return View();
        }
    }
}
