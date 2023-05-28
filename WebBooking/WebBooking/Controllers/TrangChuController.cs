using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Controllers
{
    public class TrangChuController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Room
        public ActionResult TrangChu()
        {
            // Lấy danh sách các mã loại phòng
            List<Category> categories = db.Categories.ToList();

            return View(categories);
        }

        public ActionResult ThongTin()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult LienHe()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}