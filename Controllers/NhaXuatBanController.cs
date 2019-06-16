using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebSiteBanSach.Models;

namespace WebSiteBanSach.Controllers
{
    public class NhaXuatBanController : Controller
    {
        // GET: NhaXuatBan
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public PartialViewResult NhaXuatBanPartial()
        {
            return PartialView(db.NhaXuatBans.Take(10).OrderBy(x => x.TenNXB).ToList());
        }
    }
}