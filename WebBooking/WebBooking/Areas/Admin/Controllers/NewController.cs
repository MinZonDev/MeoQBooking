using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NewController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Admin/News
        //public ActionResult Index()
        //{
        //    var news = db.News.ToList();
        //    return View(news);
        //}
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var newsList = db.News.ToList();
            if (searchTerm != null)
            {
                var news = db.News
                .Where(r => r.title.Contains(searchTerm))
                .ToList();
                return View(news.ToPagedList(pageNum, pageSize));
            }
            return View(newsList.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            ViewBag.CategoryNews = db.CategoryNews.ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(New news)
        {
            if (ModelState.IsValid)
            {
                news.createdtime = DateTime.UtcNow;
                db.News.Add(news);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            // Lấy danh sách các danh mục và đưa vào ViewBag để hiển thị trong dropdown (nếu validation không thành công)
            ViewBag.CategoryNews = db.CategoryNews.ToList();

            return View(news);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.CategoryNews = db.CategoryNews.ToList();
            var news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(New news)
        {
            if (ModelState.IsValid)
            {
                db.Entry(news).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "News");
            }

            ViewBag.Categories = db.Categories.ToList();

            return View(news);
        }
        public ActionResult Details(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            return View(news);
        }

        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }

            db.News.Remove(news);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}