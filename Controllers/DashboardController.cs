using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        [Authorize]
        public ActionResult Dashboard(string aplikasiId = "", string menuId = "")
        {
            string userName = User.Identity.Name;
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Get1Record(userName);
            if (current != null)
            {
                string listViewName = current.MenuId + "_" + current.PosisiLokasiKerjaId;
                List<object[]> lstResult = (List<object[]>)HttpContext.Application[listViewName];
                if (lstResult != null)
                {
                    object[] objResult = new object[3];
                    foreach (var listView in lstResult)
                    {
                        if ((int)listView[1] == current.TahunBuku)
                        {
                            objResult = listView;
                            break;
                        }
                    }
                    if ((string)objResult[0] == userName)
                    {
                        objResult[0] = null;
                        objResult[2] = null;
                    }
                }
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Erase(current);
            }

            if (userName != "" || (userName != "" && current == null))
            {
                current = new Ptpn8.Models.PenggunaPortalYangAktif();
                current.PenggunaPortalYangAktifId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Id);
                current.UserName = userName;
                current.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Nama;
                current.LokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).LokasiKerjaId;
                current.PosisiLokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).PosisiLokasiKerjaId;
                current.Tanggal = DateTime.Now;
                current.AplikasiId = Guid.Parse(aplikasiId);
                current.MenuId = Guid.Parse(menuId);
                current.Aplikasi = Ptpn8.Models.CRUD.CRUDAplikasi.CRUD.Get1Record(Guid.Parse(aplikasiId)).NamaAplikasi;
                current.Menu = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(menuId)).LinkText;

                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);
                ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(userName).PosisiLokasiKerja;
            }
            return View();
        }
    }
}