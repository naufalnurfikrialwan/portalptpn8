using Ptpn8.Areas.Areal.Models;
using Ptpn8.Areas.Areal.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Areas.NDNK.Controllers
{
    public class HomeController : Controller
    {
        // GET: NDNK/Home
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("NDNK");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Home NDNK";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            return View();
        }
    }
}