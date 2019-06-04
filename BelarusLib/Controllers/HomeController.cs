using BelarusLib.Models;
using System.Web.Mvc;

namespace BelarusLib.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Good luck.";
            return View();
        }
        public ActionResult Help()
        {
            return View();
        }       
    }
}