using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Admin/Category
        //public ActionResult Index()
        //{
        //    var categories = db.Categories.ToList();
        //    return View(categories);
        //}
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var categoriesList = db.Categories.ToList();
            if (searchTerm != null)
            {
                var category = db.Categories
                .Where(r => r.categoryname.Contains(searchTerm))
                .ToList();
                return View(category.ToPagedList(pageNum, pageSize));
            }
            return View(categoriesList.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        public ActionResult Edit(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category category)
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
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        public ActionResult Delete(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



    }
}