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
    public class BookingController : Controller
    {

        // GET: Admin/Booking
        //public ActionResult Index(int? page, string searchTerm)
        //{
        //    int pageSize = 10;
        //    int pageNum = page ?? 1;

        //    // Lấy danh sách phòng từ nguồn dữ liệu và truyền nó tới view
        //    //List<Room> roomList = GetRoomsFromDataSource();
        //    var bookingList = db.Bookings.ToList();
        //    if (searchTerm != null)
        //    {
        //        var booking = db.Bookings
        //        .Where(r => r.email.Contains(searchTerm))
        //        .ToList();
        //        return View(booking.ToPagedList(pageNum, pageSize));
        //    }
        //    return View(bookingList.ToPagedList(pageNum, pageSize));
        //}
        dbHotel db = new dbHotel();
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var bookings = db.Bookings.OrderByDescending(b => b.bookingdate);
            var statuses = db.Status.ToList();
            var statusList = db.Status.ToList();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                bookings = (IOrderedQueryable<Booking>)bookings.Where(b =>
                    b.email.Contains(searchTerm) ||
                    b.bookingid.ToString().Contains(searchTerm) ||
                    b.phone.Contains(searchTerm));
            }
            ViewBag.StatusList = statusList;
            ViewBag.Status = new SelectList(statuses, "statusid", "statusname");
            var pagedBookings = bookings.ToList().ToPagedList(pageNum, pageSize);

            return View(pagedBookings);
        }
        [HttpPost]
        public ActionResult UpdateBookingStatus(int bookingid, int statusid)
        {
            try
            {
                var booking = db.Bookings.Find(bookingid);
                if (booking != null)
                {
                    booking.statusid = statusid;
                    db.SaveChanges();

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = "Không tìm thấy đặt phòng" });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        //private List<Booking> GetBookingFromDataSource()
        //{
        //    // Thực hiện truy vấn hoặc logic để lấy danh sách tin tức từ nguồn dữ liệu
        //    // Ví dụ: sử dụng Entity Framework để truy vấn từ bảng News

        //    using (var context = new dbHotel())
        //    {
        //        List<Booking> bookingList = context.Bookings.ToList();
        //        return bookingList;
        //    }
        //}
        [HttpGet]
        public ActionResult TotalBookings()
        {
            var totalBookings = db.Bookings.Count();
            return Json(totalBookings, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TotalRevenue()
        {
            var totalRevenue = db.Bookings.Sum(b => b.total);
            return Json(totalRevenue, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult RevenueByDate()
        {
            var revenueByDate = db.Bookings.GroupBy(b => b.bookingdate)
                                           .Select(g => new { Date = g.Key, Revenue = g.Sum(b => b.total) })
                                           .ToList();
            return Json(revenueByDate, JsonRequestBehavior.AllowGet);
        }

        // Các action methods khác tương tự

        //[Authorize(Roles = "Admin")]
        //public ActionResult ChangeBookingStatus(int bookingId, string newStatus)
        //{
        //    var booking = db.Bookings.Find(bookingId);
        //    var statusList = db.Status.ToList();
        //    ViewBag.Status = statusList;

        //    if (booking != null)
        //    {
        //        var selectedStatus = statusList.FirstOrDefault(s => s.statusname == newStatus);
        //        if (selectedStatus != null)
        //        {
        //            booking.Status = selectedStatus; // Chuyển đổi kiểu ở đây
        //            db.SaveChanges();
        //        }
        //        else
        //        {
        //            // Trạng thái không hợp lệ
        //            ModelState.AddModelError("", "Invalid status.");
        //        }
        //    }
        //    else
        //    {
        //        // Đặt phòng không tồn tại
        //        ModelState.AddModelError("", "Booking not found.");
        //    }

        //    return RedirectToAction("Index");
        //}

    }
}