using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCCalculator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "The MVC Calculator description page.";

            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Calculator()
        {
            return new CalculatorController().Calculator();

        }

        [Authorize]
        [HttpPost]
        public ActionResult Calculator(string submit)
        {
            return new CalculatorController().Calculator(submit, this.ControllerContext);

        }


        public ActionResult Contact()
        {
            ViewBag.Message = "I suspect none of these contacts actually work. You can try them if you wish.";

            return View();
        }
    }
}