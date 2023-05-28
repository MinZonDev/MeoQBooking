using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StatisticsController : Controller
    {
        private dbHotel db = new dbHotel();
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult GetStatistical(string fromDate, string toDate)
        //{
        //    var query = from b in db.Bookings
        //                where b.bookingdate.HasValue
        //                join r in db.Rooms on b.roomid equals r.roomid
        //                select new
        //                {
        //                    CreatedDate = b.bookingdate.Value,
        //                    Total = b.total,
        //                    RoomPrice = r.price
        //                };

        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        DateTime startDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy", null);
        //        query = query.Where(x => x.CreatedDate >= startDate);
        //    }

        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        DateTime endDate = DateTime.ParseExact(toDate, "dd/MM/yyyy", null);
        //        query = query.Where(x => x.CreatedDate <= endDate);
        //    }

        //    var result = query.GroupBy(x => x.CreatedDate.Date).Select(x => new
        //    {
        //        Date = x.Key,
        //        TotalRevenue = x.Sum(y => y.Total),
        //        TotalProfit = x.Sum(y => y.Total - (y.RoomPrice ?? 0))
        //    }).OrderBy(x => x.Date);

        //    return Json(new { Data = result }, JsonRequestBehavior.AllowGet);
        //}



        // GET: Admin/Statistics
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

        // GET: Admin/Statistics/RevenueByDate
        public ActionResult RevenueByDate(DateTime date)
        {
            // Thống kê doanh thu theo ngày
            decimal revenue = db.Bookings
                .Where(b => b.checkout.HasValue && b.checkout.Value.Date == date.Date)
                .Sum(b => b.total ?? 0);

            ViewBag.Date = date;
            ViewBag.Revenue = revenue;

            return View();
        }

        // GET: Admin/Statistics/RevenueByMonth
        public ActionResult RevenueByMonth(int year, int month)
        {
            // Thống kê doanh thu theo tháng
            decimal revenue = db.Bookings
                .Where(b => b.checkout.HasValue && b.checkout.Value.Year == year && b.checkout.Value.Month == month)
                .Sum(b => b.total ?? 0);

            ViewBag.Year = year;
            ViewBag.Month = month;
            ViewBag.Revenue = revenue;

            return View();
        }

        // GET: Admin/Statistics/RevenueByYear
        public ActionResult RevenueByYear(int year)
        {
            // Thống kê doanh thu theo năm
            decimal revenue = db.Bookings
                .Where(b => b.checkout.HasValue && b.checkout.Value.Year == year)
                .Sum(b => b.total ?? 0);

            ViewBag.Year = year;
            ViewBag.Revenue = revenue;

            return View();
        }
    }

}