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
        public ActionResult CreateFact(int aid)
        {
            aid = db.Authors.Find(aid).AuthorId;
            ViewBag.AuthorId = aid;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateFact(Fact fact, int AuthorId)
        {
            fact.AuthorId = AuthorId;
            if (ModelState.IsValid)
            {
                db.Facts.Add(fact);                
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsAuthor", new { id = AuthorId });
            }
            return (CreateFact(fact.AuthorId));
        }
        public ActionResult DeleteFact(int id)
        {
            var fact = db.Facts.Find(id);
            if (fact == null)
                return HttpNotFound();
            return View(fact);
        }
        [HttpPost, ActionName("DeleteFact")]
        public async Task<ActionResult> DeleteFactConfimed(int id)
        {
            var fact = db.Facts.Find(id);
            if (fact == null)
                return HttpNotFound();
            db.Facts.Remove(fact);
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsAuthor", new { id = fact.AuthorId });
        }
        public ActionResult EditFact(int id)
        {
            var fact = db.Facts.Find(id);
            if (fact == null)
                return HttpNotFound();
            return View(fact);
        }
        [HttpPost]
        public async Task<ActionResult> EditFact(Fact fact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fact).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsAuthor", new { id = fact.AuthorId });
            }
            return EditFact(fact.FactId);
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
        public ActionResult GetAudio()
        {
            return View(db.Audios.Include(a => a.Author).ToList());
        }
        public ActionResult DetailsAudio(int id)
        {
            var audio = db.Audios.Include(a => a.Author).SingleOrDefault(i => i.AudioId == id);
            if (audio == null)
                return HttpNotFound();
            return View(audio);
        }
        public ActionResult CreateAudio()
        {
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");            
            ViewBag.Author = author;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateAudio(Audio audio)
        {
            if (ModelState.IsValid)
            {
                db.Audios.Add(audio);
                await db.SaveChangesAsync();
                return RedirectToAction("GetAudio");
            }
            return (CreateAudio());
        }
        public ActionResult DeleteAudio(int id)
        {
            var audio = db.Audios.Find(id);
            if (audio == null)
                return HttpNotFound();
            return View(audio);
        }
        [HttpPost, ActionName("DeleteAudio")]
        public async Task<ActionResult> DeleteAudioConfimed(int id)
        {
            var audio = db.Audios.Find(id);
            if (audio == null)
                return HttpNotFound();
            db.Audios.Remove(audio);
            await db.SaveChangesAsync();
            return RedirectToAction("GetAudio");
        }
        public ActionResult EditAudio(int id)
        {
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");
            ViewBag.Author = author;
            var audio = db.Audios.Find(id);
            if (audio == null)
                return HttpNotFound();
            return View(audio);
        }
        [HttpPost]
        public async Task<ActionResult> EditAudio(Audio audio)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audio).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetAudio");
            }
            return EditAudio(audio.AudioId);
        }
        public ActionResult GetVideo()
        {
            return View(db.Videos.Include(a => a.Author).ToList());
        }
        public ActionResult DetailsVideo(int id)
        {
            var video = db.Videos.Include(a => a.Author).SingleOrDefault(i => i.VideoId == id);
            if (video == null)
                return HttpNotFound();
            return View(video);
        }
        public ActionResult CreateVideo()
        {
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");
            ViewBag.Author = author;
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateVideo(Video video)
        {
            if (ModelState.IsValid)
            {
                db.Videos.Add(video);
                await db.SaveChangesAsync();
                return RedirectToAction("GetVideo");
            }
            return (CreateAudio());
        }
        public ActionResult DeleteVideo(int id)
        {
            var video = db.Videos.Find(id);
            if (video == null)
                return HttpNotFound();
            return View(video);
        }
        [HttpPost, ActionName("DeleteVideo")]
        public async Task<ActionResult> DeleteVideoConfimed(int id)
        {
            var video = db.Videos.Find(id);
            if (video == null)
                return HttpNotFound();
            db.Videos.Remove(video);
            await db.SaveChangesAsync();
            return RedirectToAction("GetVideo");
        }
        public ActionResult EditVideo(int id)
        {
            SelectList author = new SelectList(db.Authors, "AuthorId", "AuthorFullName");
            ViewBag.Author = author;
            var video = db.Videos.Find(id);
            if (video == null)
                return HttpNotFound();
            return View(video);
        }
        [HttpPost]
        public async Task<ActionResult> EditVideo(Video video)
        {
            if (ModelState.IsValid)
            {
                db.Entry(video).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetVideo");
            }
            return EditAudio(video.VideoId);
        }
        //--------------------------------------------------------------------------------------------------------------------------------------------
        public ActionResult CreateQuiz()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuiz(Quiz quiz, List<string> QuestionText, List<string> QuestionDescription, List<string> QuestionAnswer, List<HttpPostedFileBase> uploadQuestionImage)
        {
            quiz.QuizIsPrivate = true;
            db.Quizzes.Add(quiz);
            if (QuestionText != null && QuestionAnswer != null)
            {
                for (int i = 0; i < QuestionText.Count; i++)
                {
                    if (String.IsNullOrEmpty(QuestionText[i]) || String.IsNullOrEmpty(QuestionAnswer[i]))
                    {
                        ModelState.AddModelError("Questions", "Не заполнен текст вопроса или ответ на вопрос");
                    }
                    Question question = new Question
                    {
                        QuestionText = QuestionText[i],
                        QuestionDescription = QuestionDescription[i],
                        QuestionAnswer = QuestionAnswer[i].ToLower(),
                        QuizId = quiz.QuizId
                    };
                    if (uploadQuestionImage[i] != null)
                    {
                        byte[] imageData = null;
                        using (var binaryReader = new BinaryReader(uploadQuestionImage[i].InputStream))
                        {
                            imageData = binaryReader.ReadBytes(uploadQuestionImage[i].ContentLength);
                        }
                        question.QuestionImage = imageData;
                    }
                    db.Questions.Add(question);
                }
            }
            if (ModelState.IsValid)
            {
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return CreateQuiz();
        }
        public ActionResult GetQuiz()
        {
            return View(db.Quizzes.ToList());
        }
        public ActionResult EditQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        [HttpPost]
        public async Task<ActionResult> EditQuiz(Quiz quiz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(quiz).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("GetQuiz");
            }
            return EditQuiz(quiz.QuizId);
        }
        public ActionResult DeleteQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        [HttpPost, ActionName("DeleteQuiz")]
        public async Task<ActionResult> DeleteQuizConfirmed(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            db.Quizzes.Remove(quiz);
            await db.SaveChangesAsync();
            return RedirectToAction("GetQuiz");
        }
        public ActionResult DetailsQuiz(int id)
        {
            Quiz quiz = db.Quizzes.Find(id);
            if (quiz == null)
            {
                return HttpNotFound();
            }
            return View(quiz);
        }
        public ActionResult CreateQuestion(int qid)
        {
            Question question = new Question
            {
                QuizId = qid
            };
            return View(question);
        }
        [HttpPost]
        public async Task<ActionResult> CreateQuestion(Question question, HttpPostedFileBase uploadQuestionImage)
        {
            question.QuestionAnswer = question.QuestionAnswer.ToLower();
            if (uploadQuestionImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadQuestionImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadQuestionImage.ContentLength);
                }
                question.QuestionImage = imageData;
            }
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuiz", new { id = question.QuizId });
            }
            return (CreateQuestion(question.QuizId));
        }
        public ActionResult DetailsQuestion(int id)
        {
            var questions = db.Questions.Include(q => q.Choices).Include(q => q.Quiz).ToList();
            var question = questions.Find(i => i.QuestionId == id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        public ActionResult DeleteQuestion(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        [HttpPost, ActionName("DeleteQuestion")]
        public async Task<ActionResult> DeleteQuestionConfimed(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            db.Questions.Remove(question);
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsQuiz", new { id = question.QuizId });
        }
        public ActionResult EditQuestion(int id)
        {
            var question = db.Questions.Find(id);
            if (question == null)
                return HttpNotFound();
            return View(question);
        }
        [HttpPost]
        public async Task<ActionResult> EditQuestion(Question question, HttpPostedFileBase uploadQuestionImage, bool? deleteQuestionImage)
        {
            question.QuestionAnswer = question.QuestionAnswer.ToLower();
            if (uploadQuestionImage != null)
            {
                byte[] imageData = null;
                using (var binaryReader = new BinaryReader(uploadQuestionImage.InputStream))
                {
                    imageData = binaryReader.ReadBytes(uploadQuestionImage.ContentLength);
                }
                question.QuestionImage = imageData;
            }
            if (deleteQuestionImage == true)
            {
                Array.Clear(question.QuestionImage, 0, question.QuestionImage.Length);
            }
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = question.QuestionId });
            }
            return EditQuestion(question.QuestionId);
        }
        public ActionResult CreateChoice(int qid)
        {
            Choice choice = new Choice
            {
                QuestionId = qid,
            };
            return View(choice);
        }
        [HttpPost]
        public async Task<ActionResult> CreateChoice(Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Choices.Add(choice);
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
            }
            return CreateChoice(choice.ChoiceId);
        }
        public ActionResult EditChoice(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            return View(choice);
        }
        [HttpPost]
        public async Task<ActionResult> EditChoice(Choice choice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choice).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
            }
            return EditChoice(choice.ChoiceId);
        }
        public ActionResult DeleteChoice(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            return View(choice);
        }
        [HttpPost, ActionName("DeleteChoice")]
        public async Task<ActionResult> DeleteChoiceConfirmed(int id)
        {
            var choice = db.Choices.Find(id);
            if (choice == null)
                return HttpNotFound();
            db.Choices.Remove(choice);
            await db.SaveChangesAsync();
            return RedirectToAction("DetailsQuestion", new { id = choice.QuestionId });
        }
        public ActionResult PublishQuiz(int id)
        {
            List<ApplicationUser> users = db.Users.ToList();
            ViewBag.Users = users;
            var quiz = db.Quizzes.Find(id);
            return View(quiz);
        }
        [HttpPost]        
        public ActionResult ResultQuiz(int id)
        {
            ViewBag.Name = db.Quizzes.Find(id).QuizName;
            ViewBag.Id = id;
            var results = db.Results.Include(u => u.User).Where(q => q.QuizId == id).ToList();
            return View(results);
        }
        public ActionResult Help()
        {
            return View();
        }
    }
}