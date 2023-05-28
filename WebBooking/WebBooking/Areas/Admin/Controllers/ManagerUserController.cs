using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebBooking.Areas.Admin.Controllers
{
    public class ManagerUserController : Controller
    {
        // GET: Admin/ManagerUser
        public ActionResult Index()
        {
            return View();
        }
    }
}