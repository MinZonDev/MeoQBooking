using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, nhanvien")]
    public class StatisticsController : Controller
    {
        private dbHotel db = new dbHotel();
        // GET: Admin/Statistics
        public ActionResult Index()
        {
            //// Tổng số lượng đơn đặt phòng
            int totalBooked = db.Bookings.Count();

            //// Tổng doanh thu
            ///
            //decimal totalRevenue = db.Bookings.Sum(b => b.total ?? 0);
            // Tổng số lượng đơn đặt phòng có statusid bằng 4
            int totalBookings = db.Bookings.Count(b => b.statusid == 4);

            // Tổng doanh thu của các đơn đặt phòng có statusid bằng 4
            decimal totalRevenue = db.Bookings
                .Where(b => b.statusid == 4)
                .Sum(b => b.total ?? 0);

            // Lấy tháng và năm hiện tại
            int currentDay = DateTime.Now.Day;
            int currentMonth = DateTime.Now.Month;
            int currentYear = DateTime.Now.Year;

            // Tổng số lượng đơn đặt phòng trong hôm nay
            int totalBookingToday = db.Bookings.Count(b => b.bookingdate.HasValue && b.bookingdate.Value.Day == currentDay && b.bookingdate.Value.Month == currentMonth && b.bookingdate.Value.Year == currentYear);

            //// Tổng doanh thu trong hôm nay
            //decimal totalRevenueToday = db.Bookings
            //.Where(b => b.statusid == 4 && b.checkout.HasValue && b.checkout.Value.Day == currentDay && b.checkout.Value.Month == currentMonth && b.checkout.Value.Year == currentYear)
            //.Sum(b => b.total ?? 0);
            decimal? totalRevenueToday = db.Bookings
            .Where(b => b.statusid == 4 && b.checkout.HasValue && b.checkout.Value.Day == currentDay && b.checkout.Value.Month == currentMonth && b.checkout.Value.Year == currentYear)
            .Sum(b => b.total) ?? 0m;



            // Tổng số lượng đơn đặt phòng trong tháng hiện tại
            int totalBookingThisMonth = db.Bookings.Count(b => b.bookingdate.HasValue && b.bookingdate.Value.Month == currentMonth && b.bookingdate.Value.Year == currentYear);

            // Tổng doanh thu của các đơn đặt phòng có statusid bằng 4 trong tháng hiện tại
            decimal totalRevenueThisMonth = db.Bookings
            .Where(b => b.statusid == 4 && b.checkout.HasValue && b.checkout.Value.Month == currentMonth && b.checkout.Value.Year == currentYear)
            .Sum(b => b.total ?? 0);
            // Tổng số lượng đơn đặt phòng có statusid bằng 4 trong năm hiện tại
            int totalBookingThisYear = db.Bookings.Count(b => b.bookingdate.HasValue && b.bookingdate.Value.Year == currentYear);

            // Tổng doanh thu của các đơn đặt phòng có statusid bằng 4 trong năm hiện tại
            decimal totalRevenueThisYear = db.Bookings
            .Where(b => b.statusid == 4 && b.checkout.HasValue && b.checkout.Value.Year == currentYear)
            .Sum(b => b.total ?? 0);

            var monthlyRevenue = db.Bookings
                .Where(b => b.statusid == 4 && b.checkin.HasValue)
                .GroupBy(b => new { b.checkin.Value.Month, b.checkin.Value.Year })
                .Select(g => new { Month = g.Key.Month, Year = g.Key.Year, Revenue = g.Sum(b => b.total ?? 0) })
                .OrderBy(g => g.Year)
                .ThenBy(g => g.Month)
                .ToList();
            ViewBag.MonthlyRevenue = monthlyRevenue;
            ViewBag.TotalBooked = totalBooked;
            ViewBag.TotalBookings = totalBookings;
            ViewBag.TotalRevenue = totalRevenue;
            ViewBag.TotalBookingToday = totalBookingToday;
            ViewBag.TotalRevenueToday = totalRevenueThisMonth;
            ViewBag.TotalBookingThisMonth = totalBookingThisMonth;
            ViewBag.TotalRevenueThisMonth = totalRevenueThisMonth;
            ViewBag.TotalBookingThisYear = totalBookingThisYear;
            ViewBag.TotalRevenueThisYear = totalRevenueThisYear;
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