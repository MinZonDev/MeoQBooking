using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using PagedList;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models;
using WebBooking.Models.DB;

namespace WebBooking.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {


        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext data = new ApplicationDbContext();

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationDbContext dbContext;
        dbHotel db = new dbHotel();
        public AccountController()
        {
            dbContext = new ApplicationDbContext();
        }
        //public ActionResult Index()
        //{
        //    var customers = db.AspNetUsers.ToList();
        //    return View(customers);
        //}
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var usersList = db.AspNetUsers.ToList();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                var users = db.AspNetUsers
                    .Where(u => u.UserName.Contains(searchTerm) ||
                                u.PhoneNumber.Contains(searchTerm) ||
                                u.Email.Contains(searchTerm))
                    .ToList();

                return View(users.ToPagedList(pageNum, pageSize));
            }

            return View(usersList.ToPagedList(pageNum, pageSize));
        }

        public ActionResult AddRoleToUser()
        {
            // Lấy danh sách tài khoản người dùng
            var users = dbContext.Users.ToList();
            ViewBag.Users = users;

            // Lấy danh sách quyền
            var roles = dbContext.Roles.ToList();
            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        public ActionResult AddRoleToUser(string userId, string roleName)
        {
            var user = dbContext.Users.Find(userId);
            var role = dbContext.Roles.FirstOrDefault(r => r.Name == roleName);

            if (user != null && role != null)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(dbContext));
                userManager.AddToRole(user.Id, role.Name);
            }

            return RedirectToAction("Index", "Account"); // Chuyển hướng đến trang chủ hoặc trang khác
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", " Tên hoặc mật khẩu của bạn không đúng");
                    return View(model);
            }
        }

        // thoát tk
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }
        // Tạo 1 đăng nhập mói
        [AllowAnonymous]
        public ActionResult Create()
        {
            // tạo ra list role để người dùng chọn
            ViewBag.Role = new SelectList(data.Roles.ToList(), "Name", "Name");
            return View();
        }

        //
        // POST: /Account/Register

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dbContext.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
