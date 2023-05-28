using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBooking.Models.DB;
using PagedList.Mvc;
using PagedList;

namespace WebBooking.Controllers
{
    public class TinTucController : Controller
    {
        dbHotel db = new dbHotel();
        // GET: TinTuc
        public ActionResult TinTuc(int? page, string searchTerm)
        {
            //var news = db.News.ToList();
            //return View(news);

            List<CategoryNew> categories = db.CategoryNews.ToList();
            ViewBag.Categories = categories;

            int pageSize = 2;
            int pageNum = page ?? 1;

            // Lấy danh sách tin tức từ nguồn dữ liệu và truyền nó tới view
            List<New> newsList = GetNewsFromDataSource();
            if(searchTerm!= null)
            {
                var tintuc = db.News
                .Where(r => r.title.Contains(searchTerm))
                .ToList();
                return View(tintuc.ToPagedList(pageNum, pageSize));
            }
            return View(newsList.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ChiTietTinTuc(int id)
        {
            var news = db.News.Find(id);
            if (news == null)
            {
                return HttpNotFound();
            }
            return View(news);
        }

        public ActionResult LoaiTinTuc(int id)
        {
            // Lấy mã loại phòng
            CategoryNew categories = db.CategoryNews.Find(id);

            if (categories == null)
            {
                return HttpNotFound();
            }

            // Lấy danh sách các phòng thuộc loại này
            List<New> news = db.News.Where(r => r.categoryid == id).ToList();

            ViewBag.categorynews = categories.CategoryName;

            return View(news);
        }

        //public ActionResult TimKiemTinTuc(string searchTerm)
        //{
        //    var tintuc = db.News
        //        .Where(r => r.title.Contains(searchTerm))
        //        .ToList();
        //    return View(tintuc);
        //}


        private List<New> GetNewsFromDataSource()
        {
            // Thực hiện truy vấn hoặc logic để lấy danh sách tin tức từ nguồn dữ liệu
            // Ví dụ: sử dụng Entity Framework để truy vấn từ bảng News

            using (var context = new dbHotel())
            {
                List<New> newsList = context.News.ToList();
                return newsList;
            }
        }

    }
}