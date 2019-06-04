using BelarusLib.Models;
using System;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Web.Mvc;

namespace BelarusLib.Controllers
{
    public class LibraryController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Library
        public ActionResult Index()
        {
            var genres = db.Genres.ToList();
            return View(genres);
        }
        public ActionResult Authors()
        {
            var authors = db.Authors.OrderBy(f => f.AuthorFullName).ToList();
            return View(authors);
        }
        public ActionResult Compositions(int? GenreId, string SearchResult = null, int page = 1)
        {
            int pagesize = 10;
            ViewBag.SearchResult = SearchResult;
            ViewBag.GenreId = GenreId;
            var query = db.Compositions.Include(a => a.Author).Include(g => g.Genres);
            if (GenreId != null && GenreId != 0)
            {
                ViewBag.Genre = db.Genres.Find(GenreId).GenreName;
                query = query.Where(g => g.Genres.Any(i => i.GenreId == GenreId));
            }
            if (!String.IsNullOrEmpty(SearchResult))
            {
                ViewBag.SearchResult = SearchResult;
                query = query.Where(n => n.CompositionName.Contains(SearchResult));
            }
            var compostions = query.ToList();
            CompositionsListViewModel model = new CompositionsListViewModel()
            {
                Compositions = compostions.Skip((page - 1) * pagesize).Take(pagesize),
                PagingInfo = new PagingInfo
                {
                    CurrentPage = page,
                    ItemsPerPage = pagesize,
                    TotalItems = compostions.Count(),
                },
            };
            return View(model);
        }
        public ActionResult SearchResultAuthors(string SearchResult)
        {
            ViewBag.SearchResult = SearchResult;
            var searchresult = db.Authors.Where(c => c.AuthorFullName.Contains(SearchResult));
            return View(searchresult);
        }
        public ActionResult Author(int id)
        {
            var author = db.Authors.Include(c => c.Compositions).Include(f => f.Facts).Include(a => a.Audios).Include(v => v.Videos).SingleOrDefault(i => i.AuthorId == id);
            if (author == null)
                return HttpNotFound();
            return View(author);
        }
        public ActionResult Composition(int id)
        {
            var composition = db.Compositions.Include(a => a.Author).Include(g => g.Genres).SingleOrDefault(i => i.CompositionId == id);
            if (composition == null)
                return HttpNotFound();
            return View(composition);
        }
    }
}