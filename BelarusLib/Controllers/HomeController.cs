using BelarusLib.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;

namespace BelarusLib.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            ViewBag.Authors = db.Authors.ToList();
            ViewBag.Genres = db.Genres.ToList();
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }
        public ActionResult Author(int id)
        {
            var author = db.Authors.Include(c => c.Compositions).SingleOrDefault(i => i.AuthorId == id);            
            if (author == null)
                return HttpNotFound();
            return View(author);
        }
        public ActionResult Compositions(int? id)
        {            
            var composition = db.Compositions.Include(a => a.Author).Include(g => g.Genres);
            if (id != null && id != 0)
            {
                ViewBag.Genre = db.Genres.Find(id).GenreName;
                composition = composition.Where(g => g.Genres.Any(i => i.GenreId == id));
            }
            List<Composition> compositions = composition.ToList();            
            return View(composition);
        }
        public ActionResult Composition(int id)
        {
            var composition = db.Compositions.Include(a => a.Author).Include(g => g.Genres).SingleOrDefault(i => i.AuthorId == id);
            if (composition == null)
                return HttpNotFound();
            return View(composition);
        }
        public ActionResult SearchResult(string search)
        {
            ViewBag.SearchResult = search;
            var searchresult = db.Compositions.Where(c => c.CompositionName.Contains(search)).Include(a => a.Author).Include(g => g.Genres).ToList();
            return View(searchresult);
        }
    }
}