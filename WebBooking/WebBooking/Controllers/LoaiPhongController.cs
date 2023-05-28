using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Controllers
{
    public class LoaiPhongController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: /Category
        public ActionResult CacLoaiPhong()
        {
            // Lấy danh sách các mã loại phòng
            List<Category> categories = db.Categories.ToList();

            return View(categories);
        }
        public ActionResult Phong(int id)
        {
            // Lấy mã loại phòng
            Category categories = db.Categories.Find(id);

            if (categories == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách các phòng thuộc loại này
            List<Room> rooms = db.Rooms.Where(r => r.categoryid == id).ToList();

            ViewBag.categoryroom = categories.categoryname;

            return View(rooms);
        }
        public ActionResult ChiTietPhong(int id)
        {
            var category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }
    }
}