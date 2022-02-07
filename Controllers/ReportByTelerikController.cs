using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    public class ReportByTelerikController : Controller
    {
        //private string msgInternalError = "Internal Error, Hubungi Bagian TI";
        [Authorize]
        public ActionResult Report(string aplikasiId = "", string menuId = "")
        {
            if (aplikasiId == "") aplikasiId = Guid.Empty.ToString();
            if (menuId == "") menuId = Guid.Empty.ToString();
            Dictionary<string, object> _app = new Dictionary<string, object>();
            _app = InisiasiReport(aplikasiId, menuId, CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString());

            if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() != "Success")
            {
                return View("Error", new List<string>() { { "Aplikasi Id: " + aplikasiId }, { "Menu Id: " + menuId }, { " ada masalah!" + " Message: " + _app.FirstOrDefault(o => o.Key == "Message").Value.ToString() } });
            }
            else
            {
                var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Get1Record(HttpContext.User.Identity.Name);
                if (current == null)
                {
                    current = new Ptpn8.Models.PenggunaPortalYangAktif();
                }
                current.PenggunaPortalYangAktifId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.User.Identity.Name).Id);
                current.Tanggal = DateTime.Now;
                current.UserName = HttpContext.User.Identity.Name;
                current.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.User.Identity.Name).Nama;
                current.StatusBaru = true;
                current.Aplikasi = _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString();
                current.Menu = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);

                return View(@"\" + _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString() + @"\" + _app.FirstOrDefault(o => o.Key == "NamaView").Value.ToString());
            }
        }

        public Dictionary<string, object> InisiasiReport(string aplikasiId, string menuId, string posisiLokasiKerjaId)
        {
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["ReportByTelerik" + menuId + posisiLokasiKerjaId];
            if (_app == null)
            {
                _app = readApp(aplikasiId, menuId, posisiLokasiKerjaId);
                if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() == "Success")
                {
                    HttpContext.Application["Input" + menuId + posisiLokasiKerjaId] = _app;
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

        public Dictionary<string, object> readApp(string aplikasiId, string menuId, string posisiLokasiKerjaId)
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
                            classHeaderView = m.classHeaderView,
                            classHeaderTable = m.classHeaderTable,
                            ARRclassDetailView = m.classDetailView.ToString().Split(';'),
                            ARRclassDetailTable = m.classDetailTable.ToString().Split(';'),
                            NamaAplikasi = g.Select(o => o.NamaAplikasi).FirstOrDefault(),
                            NamaModul = m.LinkText,
                            ARRConnectionString = m.ConnectionString.ToString().Split(';'),
                            NamaTabelHeader = m.NamaTabelHeader,
                            ARRNamaTabelDetail = m.NamaTabelDetail.ToString().Split(';'),
                            NamaView = m.NamaView,
                            FieldTahunBuku = m.FieldTahunBuku,
                            FieldNomorInput = m.FieldNomorInput,
                            FieldNomorInputView = m.FieldNomorInputView,
                            ARRFieldKeyHeader = m.FieldKeyHeader.ToString().Split(';'),
                            ARRFieldKeyDetail = m.FieldKeyDetail.ToString().Split(';'),
                            bolehBuatNoBaru = m.bolehBuatBaru,
                            ListViewName = menuId + "_" + posisiLokasiKerjaId,
                            PemilikAplikasi = g.First().Pemilik.ToLower()
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
            result.Add("strClassHeaderView", menu.classHeaderView.Trim());
            result.Add("strClassHeaderTable", menu.classHeaderTable.Trim());
            result.Add("ARRClassDetailView", menu.ARRclassDetailView);
            result.Add("ARRClassDetailTable", menu.ARRclassDetailTable);
            result.Add("NamaAplikasi", menu.NamaAplikasi.Trim());
            result.Add("NamaModul", menu.NamaModul.Trim());
            result.Add("NamaTabelHeader", menu.NamaTabelHeader.Trim());
            result.Add("ARRNamaTabelDetail", menu.ARRNamaTabelDetail);
            result.Add("NamaView", menu.NamaView.Trim());
            result.Add("FieldTahunBuku", menu.FieldTahunBuku);
            result.Add("FieldNomorInput", menu.FieldNomorInput);
            result.Add("FieldNomorInputView", menu.FieldNomorInputView);
            result.Add("ARRFieldKeyHeader", menu.ARRFieldKeyHeader);
            result.Add("ARRFieldKeyDetail", menu.ARRFieldKeyDetail);
            result.Add("bolehBuatNoBaru", menu.bolehBuatNoBaru);
            result.Add("listViewName", menu.ListViewName);
            result.Add("PemilikAplikasi", menu.PemilikAplikasi);

            result.Add("Message", "Success");
            return result;
        }

    }
}