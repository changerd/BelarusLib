using BelarusLib.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BelarusLib.Controllers
{
    [Authorize]
    public class TestController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        // GET: Test
        public ActionResult Index()
        {
            var quizzes = context.Quizzes.Where(p => p.QuizIsPrivate == false);
            return View(quizzes);
        }
        public ActionResult Start(int id)
        {
            var quiz = context.Quizzes.Find(id);
            long secinterval = (quiz.QuizDuration.Hours * 3600 * 1000) + (quiz.QuizDuration.Minutes * 60 * 1000) + (quiz.QuizDuration.Seconds * 1000);
            ViewBag.Interval = secinterval;
            return View(quiz);
        }
        [HttpPost]
        public async Task<ActionResult> Start(Quiz quiz, List<int> queid, List<string> choices)
        {
            int count = 0;
            var userid = User.Identity.GetUserId();
            for (int i = 0; i < queid.Count; i++)
            {
                var question = context.Questions.Find(queid[i]);
                if (!String.IsNullOrEmpty(choices[i]))
                    choices[i] = choices[i].ToLower();
                if (String.Equals(question.QuestionAnswer, choices[i]))
                    count++;
            }
            int mark = 10 * count / queid.Count;
            Result result = new Result
            {
                QuizId = quiz.QuizId,
                UserId = userid,
                ResultDate = DateTime.Now,
                ResultScore = mark,

            };
            context.Results.Add(result);
            await context.SaveChangesAsync();
            return RedirectToAction("ShowResult", new { id = result.ResultId, count });
        }
        public ActionResult ShowResult(int id, int count)
        {
            ViewBag.Count = count;
            var results = context.Results.Include(q => q.Quiz).ToList();
            Result result = results.Find(i => i.ResultId == id);
            return View(result);
        }
    }
}