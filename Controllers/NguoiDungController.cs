using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;
using System.Security.Cryptography;

namespace WebSiteBanSach.Controllers
{
    public class NguoiDungController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();

        // GET: NguoiDung
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangKy()
        {

            return View();
        }
        [HttpPost]

     


        public ActionResult DangKy(KhachHang kh)
        {
           
    
            db.KhachHangs.Add(kh);
            db.SaveChanges();
                return View();
        }
        [HttpGet]
        public ActionResult DangNhap()
        {

            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            string sTaikhoan = f["txtTaiKhoan"].ToString();
            string sMatKhau = f.Get("txtMatKhau").ToString();
       
            KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == sTaikhoan && n.MatKhau == sMatKhau);
            
   
            if (kh!=null&&sTaikhoan=="a")
            {
               
                ViewBag.ThongBao = "Bạn đang đăng nhập dưới quyền admin";
                Session["TaiKhoan"] = kh;


             return   RedirectToAction("Index","Home"); 
                        
           
            }
            if(kh!=null&&sTaikhoan!="a")
            {
                ViewBag.ThongBao = "Chúc Mừng Bạn đăng nhập thành công !";
                Session["TaiKhoan"] = kh;
                return RedirectToAction("Index","Home");
            }
            ViewBag.ThongBao = " Tên Tài Khoản hoặc Mật Khẩu không đúng !";
            return View();
        }
        public ActionResult NguoiDungPartial()
        {
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return PartialView();
            }
            ViewBag.KhachHang = (KhachHang)Session["TaiKhoan"];
            return PartialView();
        }
        public ActionResult DangXuat()
        {
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            Session["TaiKhoan"] = null;
            Session["GioHang"] = null;
            return RedirectToAction("Index", "Home");
        }



    }
   
}