using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data;
using System.Data.SqlClient;
using Kendo.Mvc.UI;
using System.Reflection;
using Kendo.Mvc.Extensions;

namespace Ptpn8.Controllers
{
    public class EvaluasiKinerjaHarianController : Controller
    {
        [Authorize]
        public ActionResult EvaluasiKinerjaHarian(string aplikasiId = "", string menuId = "")
        {
            if (aplikasiId == "") aplikasiId = Guid.Empty.ToString();
            if (menuId == "") menuId = Guid.Empty.ToString();
            Dictionary<string, object> _app = new Dictionary<string, object>();
            _app = InisiasiParameter(aplikasiId, menuId);
            if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() != "Success")
            {
                return View("Error", new List<string>() { { "Aplikasi Id: " + aplikasiId }, { "Menu Id: " + menuId }, { " ada masalah!" + " Message: " + _app.FirstOrDefault(o => o.Key == "Message").Value.ToString() } });
            }

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
                current.StatusBaru = true;
                current.Aplikasi = _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString();
                current.Menu = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();

                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);
                ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(userName).PosisiLokasiKerja;
            }
            return View(@"\" + _app.FirstOrDefault(o => o.Key == "NamaView").Value.ToString());
        }
        public Dictionary<string, object> InisiasiParameter(string aplikasiId, string menuId)
        {
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["EvaluasiKinerjaHarian" + menuId];
            if (_app == null)
            {
                _app = readApp(aplikasiId, menuId);
                if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() == "Success")
                {
                    HttpContext.Application["EvaluasiKinerjaHarian" + menuId] = _app;
                }
            }

            var riwayatAkses = new RiwayatAkses
            {
                UserName = HttpContext.User.Identity.Name,
                TanggalAkses = DateTime.Now,
                PageYangDiakses = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString(),
                Id = Guid.Parse(_app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString())
            };
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(riwayatAkses);

            ViewBag.Menu = (from m in CRUDMenu.CRUD.GetAllRecord().Where(o => o.AplikasiId == Guid.Parse(aplikasiId)) select m).OrderBy(o => o.KodeMenu).ToList();
            ViewBag.Title = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            ViewBag.Message = "";
            ViewBag.MenuId = _app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString();
            return _app;
        }
        public Dictionary<string, object> readApp(string aplikasiId, string menuId)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            HttpContext.Session["LOKASIKEBUNID"] = CRUDApplicationUser.CRUD.Get1Record(User.Identity.Name).PosisiLokasiKerjaId; // CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId;

            var menuAplikasi = from m in CRUDMenu.CRUD.GetAllRecord().Where(o => o.AplikasiId == Guid.Parse(aplikasiId)) select m;
            if (menuAplikasi == null)
            {
                result.Add("Message", "Tidak Ditemukan!");
                return result;
            }

            var menu = (from m in menuAplikasi
                        join n in CRUDAplikasi.CRUD.GetAllRecord() on m.AplikasiId equals n.AplikasiId into g
                        where m.AplikasiId == Guid.Parse(aplikasiId) && m.MenuId == Guid.Parse(menuId)
                        select new
                        {
                            NamaAplikasi = g.Select(o => o.NamaAplikasi).FirstOrDefault(),
                            NamaModul = m.LinkText,
                            ARRConnectionString = m.ConnectionString.ToString().Split(';'),
                            NamaView = m.NamaView
                        }).FirstOrDefault();

            if (menu == null)
            {
                result.Add("Message", "Tidak Ditemukan!");
                return result;
            }

            string[] arrcs = new string[menu.ARRConnectionString.Length];
            for (int j = 0; j < menu.ARRConnectionString.Length; j++)
            {
                arrcs[j] = System.Configuration.ConfigurationManager.ConnectionStrings[menu.ARRConnectionString[j]].ConnectionString;
            }

            result.Add("AplikasiId", aplikasiId);
            result.Add("MenuId", menuId);
            result.Add("cs", arrcs);
            result.Add("NamaAplikasi", menu.NamaAplikasi.Trim());
            result.Add("NamaModul", menu.NamaModul.Trim());
            result.Add("NamaView", menu.NamaView.Trim());
            result.Add("Message", "Success");
            return result;
        }

        public ActionResult GetDataFromGRID([DataSourceRequest] DataSourceRequest request,
            string strClassView, string scriptSQL, string _menuId)
        {
            List<object> Results = new List<object>();
            try
            {
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["EvaluasiKinerjaHarian" + _menuId];
                string[] arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;

                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(arrCS[0]))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Result = Activator.CreateInstance(T);
                            for (int i = 0; i < modelProperties.Count; i++)
                                try
                                {
                                    modelProperties[i].SetValue(Result, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                }
                                catch
                                {
                                    continue;
                                }

                            Results.Add(Result);
                        };
                    }
                    conn.Close();
                }
            }
            catch 
            {
            }
            return Json(Results.ToDataSourceResult(request));
        }

        public ActionResult GetDataFromModel(string strClassView, string scriptSQL, string _menuId)
        {
            List<object> Results = new List<object>();
            try
            {
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["EvaluasiKinerjaHarian" + _menuId];
                string[] arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;

                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(arrCS[0]))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Result = Activator.CreateInstance(T);
                            for (int i = 0; i < modelProperties.Count; i++)
                                try
                                {
                                    modelProperties[i].SetValue(Result, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                }
                                catch
                                {
                                    continue;
                                }

                            Results.Add(Result);
                        };
                    }
                    conn.Close();
                }
            }
            catch 
            {
            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

       

    }
}