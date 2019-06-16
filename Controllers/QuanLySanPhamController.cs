using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace WebSiteBanSach.Controllers
{
    public class QuanLySanPhamController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // GET: QuanLySanPham
        public ActionResult Index(int? page)
        {
            int pagenumber = (page ?? 1);
            int pagesize = 3;

            return View(db.Saches.ToList().OrderBy(n => n.MaSach).ToPagedList(pagenumber, pagesize));
        }

        [HttpGet]
        public ActionResult ThemMoi()
        {
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.MaChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoi(Sach sach, HttpPostedFileBase fileupload)

        {



            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.MaChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");

            if (fileupload == null)
            {

                ViewBag.ThongBao = "Chọn hình ảnh";
                return View();
            }

            if (ModelState.IsValid)
            {
                var fileName = Path.GetFileName(fileupload.FileName);

                var path = Path.Combine(Server.MapPath("~/HinhAnhSP"), fileName);

                if (System.IO.File.Exists(path))
                {

                    ViewBag.ThongBao = "Hình ảnh đã tồn tại";
                }
                else
                {
                    fileupload.SaveAs(path);
                }
                sach.AnhBia = fileupload.FileName;
                db.Saches.Add(sach);
                db.SaveChanges();
            }
            return View();

        }
        public ActionResult ChinhSua(int MaSach)
        {

            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);

            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.MaChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");
            return View(sach);


        }


        [HttpPost]
        [ValidateInput(false)]

        public ActionResult ChinhSua(Sach sach)
        {

            if (ModelState.IsValid)
            {
                db.Entry(sach).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            ViewBag.MaChuDe = new SelectList(db.ChuDes.ToList().OrderBy(n => n.MaChuDe), "MaChuDe", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NhaXuatBans.ToList().OrderBy(n => n.MaNXB), "MaNXB", "TenNXB");


            return Redirect("Index");

        }
        [HttpGet]
        public ActionResult Xoa(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {

                Response.StatusCode = 404;
                return null;

            }
            return View(sach);

        }
        [HttpPost, ActionName("Xoa")]
        public ActionResult XacNhanXoa(int MaSach)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == MaSach);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;

            }

            db.Saches.Remove(sach);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}