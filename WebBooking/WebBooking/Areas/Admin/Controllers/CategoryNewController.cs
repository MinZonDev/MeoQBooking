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
    public class CategoryNewController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Admin/CategoryNews
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var categorynewsList = db.CategoryNews.ToList();
            if (searchTerm != null)
            {
                var categorynew = db.CategoryNews
                .Where(r => r.CategoryName.Contains(searchTerm))
                .ToList();
                return View(categorynew.ToPagedList(pageNum, pageSize));
            }
            return View(categorynewsList.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(CategoryNew category)
        {
            if (ModelState.IsValid)
            {
                db.CategoryNews.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            var category = db.CategoryNews.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(CategoryNew category)
        {
            if (ModelState.IsValid)
            {
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }
        public ActionResult Details(int id)
        {
            var category = db.CategoryNews.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        public ActionResult Delete()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {
            var category = db.CategoryNews.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            db.CategoryNews.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}