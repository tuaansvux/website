using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
using PagedList;
using PagedList.Mvc;

namespace WebSiteBanSach.Controllers
{
    
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanSachEntities db =  new QuanLyBanSachEntities();
        [HttpPost]
        public ActionResult KetQuaTimKiem(FormCollection f,int ? page)
        {
            string sTuKhoa = f["txtTimKiem"].ToString();
            List<Sach> lsKQTK = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            ViewBag.TuKhoa = sTuKhoa;

            int pagenumber = (page ?? 1);
            int pagesize = 3;
            
            if (lsKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Khong tim thay sach can tim !";
                return View(db.Saches.OrderBy(n=>n.TenSach).ToPagedList(pagenumber,pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + lsKQTK.Count + "kết quả";
            
            return View(lsKQTK.OrderBy(n=>n.TenSach).ToPagedList(pagenumber,pagesize));
        }

        [HttpGet]
        public ActionResult KetQuaTimKiem(string sTuKhoa, int? page)
        {
            ViewBag.TuKhoa = sTuKhoa;
            List<Sach> lsKQTK = db.Saches.Where(n => n.TenSach.Contains(sTuKhoa)).ToList();
            ViewBag.TuKhoa = sTuKhoa;

            int pagenumber = (page ?? 1);
            int pagesize = 3;

            if (lsKQTK.Count == 0)
            {
                ViewBag.ThongBao = "Khong tim thay sach can tim !";
                return View(db.Saches.OrderBy(n => n.TenSach).ToPagedList(pagenumber, pagesize));
            }
            ViewBag.ThongBao = "Đã tìm thấy" + lsKQTK.Count + "kết quả";

            return View(lsKQTK.OrderBy(n => n.TenSach).ToPagedList(pagenumber, pagesize));
        }
    }
}