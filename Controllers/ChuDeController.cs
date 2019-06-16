using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
namespace WebSiteBanSach.Controllers
{
    public class ChuDeController : Controller
    {
        // GET: ChuDe
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChuDePartial()
        {
            return PartialView(db.ChuDes.Take(5).ToList());
        }
        public ViewResult SachTheoChuDe (int MaChuDe)
        {
            ChuDe cd = db.ChuDes.SingleOrDefault(n => n.MaChuDe == MaChuDe);
            if(cd==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<Sach> lstsach = db.Saches.Where(n => n.MaChuDe == MaChuDe).OrderBy(n => n.GiaBan).ToList();
            if(lstsach.Count==0)
            {
                ViewBag.Sach = "Không có sách nào thuộc chủ đề này";
            }
            return View(lstsach);
        }
        public ViewResult DanhMucChuDe()
        {
            return View(db.ChuDes.ToList());
        }

    }
}