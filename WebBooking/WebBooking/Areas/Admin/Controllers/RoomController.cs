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
    public class RoomController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: Admin/Room
        //public ActionResult Index()
        //{
        //    var rooms = db.Rooms.ToList();
        //    return View(rooms);
        //}
        public ActionResult Index(int? page, string searchTerm)
        {
            int pageSize = 10;
            int pageNum = page ?? 1;

            var roomsList = db.Rooms.ToList();
            if (searchTerm != null)
            {
                var room = db.Rooms
                .Where(r => r.roomname.Contains(searchTerm))
                .ToList();
                return View(room.ToPagedList(pageNum, pageSize));
            }
            return View(roomsList.ToPagedList(pageNum, pageSize));
        }
        public ActionResult Create()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index", "Room");
            }

            // Lấy danh sách các danh mục và đưa vào ViewBag để hiển thị trong dropdown (nếu validation không thành công)
            ViewBag.Categories = db.Categories.ToList();

            return View(room);
        }

        public ActionResult Edit(int id)
        {
            ViewBag.Categories = db.Categories.ToList();
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            return View(room);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Room");
            }

            ViewBag.Categories = db.Categories.ToList();

            return View(room);
        }
        public ActionResult Details(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            return View(room);
        }
        public ActionResult Delete(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            return View(room);
        }
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            db.Rooms.Remove(room);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
    }
}