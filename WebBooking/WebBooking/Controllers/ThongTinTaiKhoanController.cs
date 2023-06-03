using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebBooking.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using WebBooking.Models.DB;

namespace WebBooking.Controllers
{
    public class ThongTinTaiKhoanController : Controller
    {
        //private UserManager<ApplicationUser> _userManager;
        //public ThongTinTaiKhoanController()
        //{
        //}
        //public ThongTinTaiKhoanController(UserManager<ApplicationUser> userManager)
        //{
        //    _userManager = userManager;
        //}
        dbHotel db = new dbHotel();
        public ActionResult Index()
        {
            // Lấy thông tin người dùng hiện tại
            var userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);

            // Truyền thông tin người dùng tới view
            return View(user);
        }
        public ActionResult Edit()
        {
            var userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(userId);

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(ApplicationUser model, String userId)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(model.Id);
                if (user != null)
                {
                    // Cập nhật các thuộc tính tài khoản
                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    user.PhoneNumber = model.PhoneNumber;
                    user.Birthday = model.Birthday;

                    var result = userManager.Update(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đã xảy ra lỗi khi cập nhật thông tin tài khoản.");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Người dùng không được tìm thấy.");
                }
            }

            return View(model);
        }
        public ActionResult LichSuDatPhong()
        {
            // Lấy User ID của người dùng hiện tại
            string userId = User.Identity.GetUserId();

            // Truy vấn danh sách đơn đặt phòng của User ID đó từ cơ sở dữ liệu
            var bookings = db.Bookings.Where(b => b.userid == userId).ToList();

            // Truyền danh sách đơn đặt phòng vào View để hiển thị
            return View(bookings);
        }
        public ActionResult HuyDatPhong(int id)
        {
            // Tìm đơn đặt phòng dựa trên ID
            var booking = db.Bookings.Find(id);

            if (booking == null)
            {
                return HttpNotFound();
            }

            // Kiểm tra trạng thái hiện tại của đơn đặt phòng
            if (booking.statusid != 1) // Kiểm tra trạng thái đã được xác nhận
            {
                TempData["ErrorMessage"] = "Không thể hủy đặt phòng. Đơn đặt phòng không ở trạng thái chờ xác nhận.";
                //ViewBag.Script = "<script>alert('Đã hủy đặt phòng thành công.');</script>";
                /*return RedirectToAction("Error");*/ // Chuyển hướng đến trang lỗi và hiển thị thông báo
            }

            // Cập nhật trạng thái thành 5 (hủy đặt phòng)
            booking.statusid = 5;

            // Lưu thay đổi vào cơ sở dữ liệu
            db.SaveChanges();

            TempData["SuccessMessage"] = "Đã hủy đặt phòng thành công.";
            return RedirectToAction("LichSuDatPhong"); // Chuyển hướng đến trang thông báo hủy đặt phòng thành công
        }

        public ActionResult HuyDatPhongSuccess()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"] as string;
            return View();
        }



    }
}