using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Areas.Memo.Controllers
{
    public class HomeController : Controller
    {
        // GET: Memo/Home
        public ActionResult Index()
        {
            return View();
        }
    }
}