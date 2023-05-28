using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using WebBooking.Models;
using Microsoft.AspNet.Identity.EntityFramework;

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

    }
}