using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TasikmalayaKota.Simpatik.Web.Services.mUser.Models;
using TasikmalayaKota.Simpatik.Web.Models;
namespace simpat1k.Controllers
{

   public class mLembagaController : Controller
   {
        private Simpat1kContext _context;

		public mLembagaController(Simpat1kContext Context)
		{
            this._context=Context;
		}

        public ActionResult Index()
        {

            ViewBag.dataSource = _context.mUserModel.ToList();

            return View();
        }

    }
}