using BelarusLib.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace BelarusLib.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            return View();
        }
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }
        public ActionResult GetRole()
        {
            return View(RoleManager.Roles);
        }
        public ActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateRole(CreateRoleModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityResult result = await RoleManager.CreateAsync(new ApplicationRole
                {
                    Name = model.RoleName,
                });
                if (result.Succeeded)
                {
                    return RedirectToAction("GetRole");
                }
                else
                {
                    ModelState.AddModelError("", "Что-то пошло не так");
                }
            }
            return View(model);
        }
        public async Task<ActionResult> EditRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role != null)
            {
                return View(new EditRoleModel { Id = role.Id, RoleName = role.Name });
            }
            return RedirectToAction("GetRole");
        }
        [HttpPost]
        public async Task<ActionResult> EditRole(EditRoleModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = await RoleManager.FindByIdAsync(model.Id);
                if (role != null)
                {
                    role.Name = model.RoleName;
                    IdentityResult result = await RoleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("GetRole");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Что-то пошло не так");
                    }
                }
            }
            return View(model);
        }
        public async Task<ActionResult> DeleteRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            return View(role);
        }
        [HttpPost, ActionName("DeleteRole")]
        public async Task<ActionResult> DeleteRoleConfirmed(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            if (role == null)
            {
                return HttpNotFound();
            }
            IdentityResult result = await RoleManager.DeleteAsync(role);
            return RedirectToAction("GetRole");


        }
        public async Task<ActionResult> GiveRole(string id)
        {
            ApplicationRole role = await RoleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();

            IEnumerable<ApplicationUser> members = UserManager.Users.Where(x => memberIDs.Any(y => String.Equals(y, x.Id)));

            IEnumerable<ApplicationUser> nonMembers = UserManager.Users.Except(members);

            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> GiveRole(RoleModificationModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                foreach (string userId in model.IdsToAdd ?? new string[] { })
                {
                    result = await UserManager.AddToRoleAsync(userId, model.RoleName);

                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (string userId in model.IdsToDelete ?? new string[] { })
                {
                    result = await UserManager.RemoveFromRoleAsync(userId, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("GetRole");
            }
            return View("Error", new string[] { "Роль не найдена" });
        }
        public ActionResult GetUser()
        {
            return View(UserManager.Users);
        }
        public ActionResult DetailsUser(string id)
        {
            var user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        public async Task<ActionResult> DeleteUser(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }
        [HttpPost, ActionName("DeleteUser")]
        public async Task<ActionResult> DeleteUserConfirmed(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            await db.SaveChangesAsync();
            IdentityResult result = await UserManager.DeleteAsync(user);
            return RedirectToAction("GetUser");
        }
        public ActionResult GetAuthor()
        {
            return View(db.Authors.ToList());
        }
        public ActionResult CreateAuthor(Author author)
        {            
            return View(author);
        }
        [HttpPost]
        public async Task<ActionResult> CreateAuthor(Author author, HttpPostedFileBase uploadImage)
        {            
            if (uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                author.AuthorPhoto = imageData;
            }
            if (author.AuthorPhoto == null)
            {
                ModelState.AddModelError("Photo", "Для автора не выбрана фотография.");
            }
            if (ModelState.IsValid)
            {
                db.Authors.Add(author);
                await db.SaveChangesAsync();
                return RedirectToAction("GetAuthor");
            }
            return (CreateAuthor(author));
        }
        public ActionResult DetailsAuthor(int id)
        {            
            var author = db.Authors.Find(id);
            if (author == null)
                return HttpNotFound();
            return View(author);
        }
        public ActionResult DeleteAuthor(int id)
        {
            var author = db.Authors.Find(id);
            if (author == null)
                return HttpNotFound();
            return View(author);
        }
        [HttpPost, ActionName("DeleteAuthor")]
        public async Task<ActionResult> DeleteAuthorConfimed(int id)
        {
            var author = db.Authors.Find(id);
            if (author == null)
                return HttpNotFound();
            db.Authors.Remove(author);
            await db.SaveChangesAsync();
            return RedirectToAction("GetAuthor");
        }
        public ActionResult EditAuthor(int id)
        {
            var author = db.Authors.Find(id);
            if (author == null)
                return HttpNotFound();
            return View(author);
        }
        [HttpPost]
        public async Task<ActionResult> EditAuthor(Author author, HttpPostedFileBase uploadImage)
        {            
            if (uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                author.AuthorPhoto = imageData;
            }            
            if (ModelState.IsValid)
            {
                db.Entry(author).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetAuthor");
            }
            return EditAuthor(author.AuthorId);
        }
        public ActionResult GetComposition()
        {
            return View(db.Compositions.Include(a => a.Author).Include(g => g.Genres).ToList());
        }
        public ActionResult CreateComposition(Composition composition)
        {
            List<Genre> genres = db.Genres.ToList();            
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");
            ViewBag.Genres = genres;            
            ViewBag.Author = author;
            return View(composition);
        }
        [HttpPost]
        public async Task<ActionResult> CreateComposition(Composition composition, int[] selectedGenres, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                composition.CompositionCover = imageData;
            }
            if (composition.CompositionCover == null)
            {
                ModelState.AddModelError("Photo", "Для произведения не выбрана обложка.");
            }
            if (selectedGenres != null)
            {
                foreach (var g in db.Genres.Where(g => selectedGenres.Contains(g.GenreId)))
                {
                    composition.Genres.Add(g);
                }
            }
            else
            {
                ModelState.AddModelError("Genres", "Для произведения не выбран жанр.");
            }
            if (ModelState.IsValid)
            {
                db.Compositions.Add(composition);
                await db.SaveChangesAsync();
                return RedirectToAction("GetComposition");
            }
            return (CreateComposition(composition));
        }
        public ActionResult DetailsComposition(int id)
        {
            var composition = db.Compositions.Include(a => a.Author).Include(g => g.Genres).SingleOrDefault(i => i.CompositionId == id);
            if (composition == null)
                return HttpNotFound();
            return View(composition);
        }
        public ActionResult DeleteComposition(int id)
        {
            var composition = db.Compositions.Find(id);
            if (composition == null)
                return HttpNotFound();
            return View(composition);
        }
        [HttpPost, ActionName("DeleteComposition")]
        public async Task<ActionResult> DeleteCompositionConfimed(int id)
        {
            var composition = db.Compositions.Find(id);
            if (composition == null)
                return HttpNotFound();
            db.Compositions.Remove(composition);
            await db.SaveChangesAsync();
            return RedirectToAction("GetComposition");
        }
        public ActionResult EditComposition(int id)
        {
            List<Genre> genres = db.Genres.ToList();            
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");
            ViewBag.Genres = genres;            
            ViewBag.Author = author;
            var composition = db.Compositions.Find(id);
            if (composition == null)
                return HttpNotFound();
            return View(composition);
        }
        [HttpPost]
        public async Task<ActionResult> EditComposition(Composition composition, int[] selectedGenres, HttpPostedFileBase uploadImage)
        {
            if (uploadImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadImage.ContentLength);
                }
                composition.CompositionCover = imageData;
            }
            composition.Genres.Clear();
            if (selectedGenres != null)
            {
                foreach (var g in db.Genres.Where(g => selectedGenres.Contains(g.GenreId)))
                {
                    composition.Genres.Add(g);
                }
            }
            else
            {
                ModelState.AddModelError("Genres", "Для произведения не выбран жанр.");
            }
            if (ModelState.IsValid)
            {
                db.Entry(composition).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetComposition");
            }
            return EditAuthor(composition.CompositionId);
        }
        public ActionResult GetGenre()
        {
            return View(db.Genres.ToList());
        }        
        public ActionResult CreateGenre()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Genres.Add(genre);
                await db.SaveChangesAsync();
                return RedirectToAction("GetGenre");
            }
            return (CreateGenre());
        }
        public ActionResult DeleteGenre(int id)
        {
            var genre = db.Genres.Find(id);
            if (genre == null)
                return HttpNotFound();
            return View(genre);
        }
        [HttpPost, ActionName("DeleteGenre")]
        public async Task<ActionResult> DeleteGenreConfimed(int id)
        {
            var genre = db.Genres.Find(id);
            if (genre == null)
                return HttpNotFound();
            db.Genres.Remove(genre);
            await db.SaveChangesAsync();
            return RedirectToAction("GetGenre");
        }
        public ActionResult EditGenre(int id)
        {
            var genre = db.Genres.Find(id);
            if (genre == null)
                return HttpNotFound();
            return View(genre);
        }
        [HttpPost]
        public async Task<ActionResult> EditGenre(Genre genre)
        {
            if (ModelState.IsValid)
            {
                db.Entry(genre).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetGenre");
            }
            return EditGenre(genre.GenreId);
        }        
    }
}