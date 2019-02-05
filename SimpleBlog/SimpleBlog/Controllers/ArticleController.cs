using SimpleBlog.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace SimpleBlog.Controllers
{
    public class ArticleController : Controller
    {
        public EntityState EntryState { get; private set; }

        // GET: Article
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult List()
        {
            using (var database = new BlogDbContext())
            {
                var articles = database.Articles
                    .Include(a => a.Author)
                    .ToList();

                return View(articles);
            }
        }

        // GET: Article
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database  = new BlogDbContext())
            {

                var article = database
                    .Articles
                    .Where(a => a.Id == id)
                    .Include(a => a.Author)
                    .FirstOrDefault();

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        // GET: Article/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Create(Article article)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var authorId = database.Users
                        .Where(u => u.UserName == this.User.Identity.Name)
                        .FirstOrDefault()
                        .Id;

                    article.AuthourId = authorId;

                    database.Articles.Add(article);
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }
            }

            return View(article);
        }

        // GET: Article/Delete
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles.Where(a => a.Id == id).Include(a => a.Author).FirstOrDefault();

                if (!isUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }

                return View(article);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {
                var article = database.Articles.Where(a => a.Id == id).Include(a => a.Author).First();

                if (article == null)
                {
                    return HttpNotFound();
                }

                database.Articles.Remove(article);
                database.SaveChanges();

                return RedirectToAction("Index");
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            using (var database = new BlogDbContext())
            {

                var article = database.Articles.Where(a => a.Id == id).Include(a => a.Author).First();

                if (!isUserAuthorizedToEdit(article))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                }

                if (article == null)
                {
                    return HttpNotFound();
                }

                var model = new ArticleViewModel();
                model.Id = article.Id;
                model.Title = article.Title;
                model.Content = article.Content;

                return View(model);
            }
        }

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(ArticleViewModel model)
        {
            if (ModelState.IsValid)
            {
                using (var database = new BlogDbContext())
                {
                    var article = database.Articles.FirstOrDefault(a => a.Id == model.Id);

                    article.Title = model.Title;
                    article.Content = model.Content;

                    database.Entry(article).State = EntityState.Modified;
                    database.SaveChanges();

                    return RedirectToAction("Index");
                }

            }

            return View(model);
        }

        private bool isUserAuthorizedToEdit(Article article)
        {
            bool isAdmin = this.User.IsInRole("Admin");
            bool isAuthor = article.isAuthor(this.User.Identity.Name);

            return isAuthor || isAdmin;
        }

    }
}