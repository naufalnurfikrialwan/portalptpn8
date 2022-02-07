using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    public class ReportController : Controller
    {
        private static string _cs;
        public static string cs
        {
            get { return _cs; }
            set { _cs = value; }
        }
        private static string _AplikasiId;
        public static string AplikasiId
        {
            get { return _AplikasiId; }
            set { _AplikasiId = value; }
        }
        private static string _NamaAplikasi;
        public static string NamaAplikasi
        {
            get { return _NamaAplikasi; }
            set { _NamaAplikasi = value; }
        }
        private static string _MenuId;
        public static string MenuId
        {
            get { return _MenuId; }
            set { _MenuId = value; }
        }
        private static string _NamaModul;
        public static string NamaModul
        {
            get { return _NamaModul; }
            set { _NamaModul = value; }
        }
        private static string _NamaView;
        public static string NamaView
        {
            get { return _NamaView; }
            set { _NamaView = value; }
        }
        public static string _FieldTahunBuku;
        public static string FieldTahunBuku
        {
            get { return _FieldTahunBuku; }
            set { _FieldTahunBuku = value; }
        }
        // GET: Report
        [Authorize]
        public ActionResult Report(string aplikasiId = "", string menuId = "")
        {
            if (aplikasiId == "") aplikasiId = Guid.Empty.ToString();
            if (menuId == "") menuId = Guid.Empty.ToString();

            string errStr = "";
            errStr = InisiasiReport(aplikasiId, menuId);
            if (errStr != "") return View("Error", new List<string>() { "Aplikasi Id: " + aplikasiId + "; Menu Id: " + menuId + " ada masalah!" + " Message: " + errStr });

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
            return View(@"\" + NamaAplikasi + @"\" + NamaView);
        }

        public string InisiasiReport(string aplikasiId, string menuId)
        {
            HttpContext.Session["LOKASIKEBUNID"] = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId;
            var menuAplikasi = from m in CRUDMenu.CRUD.GetAllRecord().Where(o => o.AplikasiId == Guid.Parse(aplikasiId)) select m;
            if (menuAplikasi == null) return "Tidak Ditemukan!";

            AplikasiId = aplikasiId;
            MenuId = menuId;
            var menu = (from m in menuAplikasi
                        join n in CRUDAplikasi.CRUD.GetAllRecord() on m.AplikasiId equals n.AplikasiId into g
                        where m.AplikasiId == Guid.Parse(AplikasiId) && m.MenuId == Guid.Parse(MenuId)
                        select new
                        {
                            NamaAplikasi = g.Select(o => o.NamaAplikasi).FirstOrDefault(),
                            NamaModul = m.LinkText,
                            ConnectionString = m.ConnectionString,
                            NamaTabelHeader = m.NamaTabelHeader,
                            NamaTabelDetail = m.NamaTabelDetail,
                            NamaView = m.NamaView,
                            FieldTahunBuku = m.FieldTahunBuku,
                        }).FirstOrDefault();
            if (menu == null) return "Tidak Ditemukan!";

            cs = System.Configuration.ConfigurationManager.ConnectionStrings[menu.ConnectionString].ConnectionString;
            NamaAplikasi = menu.NamaAplikasi.Trim();
            NamaModul = menu.NamaModul.Trim();
            NamaView = menu.NamaView.Trim();
            FieldTahunBuku = menu.FieldTahunBuku;

            var riwayatAkses = new RiwayatAkses
            {
                UserName = HttpContext.User.Identity.Name,
                TanggalAkses = DateTime.Now,
                PageYangDiakses = NamaModul,
                Id = Guid.Parse(MenuId)
            };
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(riwayatAkses);

            ViewBag.Menu = menuAplikasi.OrderBy(o => o.KodeMenu).ToList();
            ViewBag.Title = NamaModul;
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            ViewBag.Message = "";
            return "";
        }

        [Authorize]
        [HttpPost]
        public ActionResult detailRead([DataSourceRequest] DataSourceRequest request, string strClassView, string scriptSQL)
        {
            List<object> Results = new List<object>();
            try
            {
                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    cmd.CommandTimeout = 0;
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
            catch (Exception ex)
            {
            }
            return Json(Results.ToDataSourceResult(request));
        }

        public ActionResult GetDataFromGRID([DataSourceRequest] DataSourceRequest request,
            string strClassView, string scriptSQL, string _menuId)
        {
            List<object> Results = new List<object>();
            try
            {
                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Report" + _menuId];
                //string[] arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;

                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(cs))
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

        [Authorize]
        [HttpPost]
        public ActionResult GetDataFrom(string strClassView, string scriptSQL)
        {
            List<object> Results = new List<object>();
            try
            {
                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(cs))
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
            catch (Exception ex)
            {

            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetKodeKebun()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            Guid kebunId = Ptpn8.Areas.Referensi.Models.CRUD.CRUDLokasiKerja.CRUD.LoginLokasiKerja(userName).PosisiLokasiKerjaId;
            string kodeKebun = Ptpn8.Areas.Referensi.Models.CRUD.CRUDKebun.CRUD.Get1Record(kebunId).kd_kbn;
            string namaKebun = Ptpn8.Areas.Referensi.Models.CRUD.CRUDKebun.CRUD.Get1Record(kebunId).Nama;
            return Json(new string[]{ kodeKebun, namaKebun}, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetKebunId()
        {
            string userName = System.Web.HttpContext.Current.User.Identity.Name;
            Guid kebunId = Ptpn8.Areas.Referensi.Models.CRUD.CRUDLokasiKerja.CRUD.LoginLokasiKerja(userName).PosisiLokasiKerjaId;
            return Json(new Guid[] { kebunId }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GetDataFromImage(string strClassView, string scriptSQL)
        {
            List<object> Results = new List<object>();
            try
            {
                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    DataTable tap = new DataTable();
                    new SqlDataAdapter(scriptSQL, conn).Fill(tap);
                    for (int i = 0; i < tap.Rows.Count; i++)
                    {
                        object[] result = new object[tap.Columns.Count];
                        DataRow dr = tap.Rows[i];
                        for (int j = 0; j < dr.ItemArray.Length; j++)
                        {
                            if (dr[j].GetType().Name == "Byte[]")
                            {
                                CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                                CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                            }
                            else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                            {
                                CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            }
                            result[j] = dr[j].ToString();
                        }
                        Results.Add(result);
                    }
                    conn.Close();
                }
            }
            catch (Exception ex)
            {

            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }
    }
}