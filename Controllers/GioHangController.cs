using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    public class GioHangController : Controller
    {
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        // Laygiohang
        #region
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if(lstGioHang==null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //themgiohang
        public ActionResult ThemGioHang(int iMaSach,string strURL)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSach);
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;

            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if(sanpham==null)
            {
                sanpham = new GioHang(iMaSach);
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
              if(sanpham.iSoLuong<sach.SoLuongTon)
                {
                    sanpham.iSoLuong++;

                }
                return Redirect(strURL);
            }
            
        }
        public ActionResult CapNhatGioHang(int iMaSP,FormCollection f)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSP);
            if(sach==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
            if(sanpham!=null)
            {
                sanpham.iSoLuong = int.Parse( f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang(int iMaSP)
        {
            Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == iMaSP);
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sanpham = lstGioHang.SingleOrDefault(n => n.iMaSach == iMaSP);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == sanpham.iMaSach);
                
            }
            if(lstGioHang.Count==0)
            {
                return RedirectToAction("Index","Home");
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult GioHang()
        {
            if(Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang(); 
            
            return View(lstGioHang);
        }
        private int TongSoLuong()
        {
            int iTongsoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongsoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongsoLuong;
        }

        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.ThanhTien);
            }
            return iTongTien;
        }
        public ActionResult GioHangPartial()
        {
            if(TongSoLuong()==0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }


        public ActionResult GioHangTongTien()
        {
            if (TongTien() <= 0)
            {
                return PartialView();
            }
            ViewBag.TongTien = TongTien();
            return PartialView();
        }

        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            return View(lstGioHang);
        }
        #endregion

        #region

        [HttpPost]
        public ActionResult DatHang()
        {
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            dh.MaKh = kh.MaKH;
            dh.DaThanhToan = Convert.ToInt32(TongTien());
            dh.TinhTrangGiaoHang = 0;
            dh.NgayDat = DateTime.Now;
            dh.NgayGiao = DateTime.Now;
            db.DonHangs.Add(dh);
            //db.SaveChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.MaDonHang = dh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = item.dDonGia.ToString();
                db.ChiTietDonHangs.Add(ctdh);

                Sach sach = db.Saches.SingleOrDefault(n => n.MaSach == item.iMaSach);
                sach.SoLuongTon -= item.iSoLuong;
            }
            db.SaveChanges();
            Session["GioHang"] = null;
            return RedirectToAction("Index", "Home");
        }

        public ActionResult HoaDon()
        {
            if ((Session["TaiKhoan"] == null) || (Session["TaiKhoan"].ToString() == ""))
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.KhachHang = (KhachHang)Session["TaiKhoan"];
            return View(lstGioHang);
        }

        #endregion
    }


}