using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
namespace WebSiteBanSach.Controllers
{
    public class SachController : Controller
    {
        // GET: Sach
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult SachMoiPartial()
        {

            var lstSachMoi = db.Saches.Take(3).ToList();
            return PartialView(lstSachMoi);
        }
        public ViewResult XemChiTiet(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;

            }
            ViewBag.TenChuDe = db.ChuDes.Single(n => n.MaChuDe == sach.MaChuDe).TenChuDe;
            ViewBag.TenNhaXuatBan = db.NhaXuatBans.Single(n => n.MaNXB == sach.MaNXB).TenNXB;
            return View(sach);

        }
    }
}