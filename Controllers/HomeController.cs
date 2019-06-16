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
    public class HomeController : Controller
    {
        // GET: Home
        QuanLyBanSachEntities db = new QuanLyBanSachEntities();
        public ActionResult Index(int ? page)
        {
            int pageSize = 6;

            int pageNumber = (page ?? 1) ;

            return View(db.Saches.ToList().OrderBy(n=>n.GiaBan).ToPagedList(pageNumber,pageSize));
            

        }
        
       
    
    }

}