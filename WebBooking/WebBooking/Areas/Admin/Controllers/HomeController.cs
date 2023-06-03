using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, nhanvien")]
    public class HomeController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Admin/Home
        public ActionResult Index()
        {
            // Tổng số lượng đơn đặt phòng
            int totalBookings = db.Bookings.Count();

            // Tổng doanh thu
            decimal totalRevenue = db.Bookings.Sum(b => b.total ?? 0);

            ViewBag.TotalBookings = totalBookings;
            ViewBag.TotalRevenue = totalRevenue;

            return View();
        }
    }
}