using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace TasikmalayaKota.Simpatik.Web.Controllers
{
   public class RevisiController : Controller
   {
        
        public IActionResult Index()
        {
           
              ViewBag.items = new[] { "Bold", "Italic", "Underline", "StrikeThrough",
                            "FontName", "FontSize", "FontColor", "BackgroundColor",
                            "LowerCase", "UpperCase", "|",
                            "Formats", "Alignments", "OrderedList", "UnorderedList",
                            "Outdent", "Indent", "|",
                            "CreateTable", "CreateLink", "Image", "|", "ClearFormat", "Print",
                            "SourceCode", "FullScreen", "|", "Undo", "Redo" };
              return View();
         }

    }
}

       