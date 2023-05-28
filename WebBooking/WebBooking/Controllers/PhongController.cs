using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;

namespace WebBooking.Controllers
{
    public class PhongController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: TinTuc
        public ActionResult CacPhong(int? page, string searchTerm)
        {

            List<Category> categories = db.Categories.ToList();
            ViewBag.Categories = categories;

            int pageSize = 6;
            int pageNum = page ?? 1;

            // Lấy danh sách phòng từ nguồn dữ liệu và truyền nó tới view
            List<Room> roomList = GetRoomsFromDataSource();
            if (searchTerm != null)
            {
                var room = db.Rooms
                .Where(r => r.roomname.Contains(searchTerm))
                .ToList();
                return View(room.ToPagedList(pageNum, pageSize));
            }
            return View(roomList.ToPagedList(pageNum, pageSize));
        }
        private List<Room> GetRoomsFromDataSource()
        {
            // Thực hiện truy vấn hoặc logic để lấy danh sách tin tức từ nguồn dữ liệu
            // Ví dụ: sử dụng Entity Framework để truy vấn từ bảng News

            using (var context = new dbHotel())
            {
                List<Room> roomList = context.Rooms.ToList();
                return roomList;
            }
        }
        public ActionResult ChiTietPhong(int id)
        {
            var room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            return View(room);
        }
        // Action tìm kiếm phòng
        public ActionResult TimKiem(string searchTerm)
        {
            var rooms = db.Rooms
                .Where(r => r.roomname.Contains(searchTerm) || r.Category.categoryname.Contains(searchTerm))
                .ToList();
            return View(rooms);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}