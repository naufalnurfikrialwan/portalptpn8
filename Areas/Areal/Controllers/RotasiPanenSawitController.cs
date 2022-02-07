using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Areal.Models;
using Ptpn8.Areas.Areal.Models.CRUD;
using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Areas.Areal.Controllers
{
    public class RotasiPanenSawitController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasSawit
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Rotasi Panen";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult ChartRotasiPerKebun(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPerTanggal().Where(o => o.KodeBudidaya == "01")
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "01", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RotasiPerKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPerTanggal().Where(o => o.KodeBudidaya == "01")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            LuasAreal= CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "01", "TM", g.Key.TahunTanam),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T01JumlahTandan = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahTandan),
                            T01KgTBS = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgTBS + o.KgBrondol),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T02JumlahTandan = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahTandan),
                            T02KgTBS = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgTBS + o.KgBrondol),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T03JumlahTandan = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahTandan),
                            T03KgTBS = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgTBS + o.KgBrondol),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T04JumlahTandan = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahTandan),
                            T04KgTBS = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgTBS + o.KgBrondol),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T05JumlahTandan = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahTandan),
                            T05KgTBS = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgTBS + o.KgBrondol),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T06JumlahTandan = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahTandan),
                            T06KgTBS = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgTBS + o.KgBrondol),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T07JumlahTandan = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahTandan),
                            T07KgTBS = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgTBS + o.KgBrondol),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T08JumlahTandan = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahTandan),
                            T08KgTBS = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgTBS + o.KgBrondol),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T09JumlahTandan = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahTandan),
                            T09KgTBS = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgTBS + o.KgBrondol),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T10JumlahTandan = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahTandan),
                            T10KgTBS = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgTBS + o.KgBrondol),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T11JumlahTandan = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahTandan),
                            T11KgTBS = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgTBS + o.KgBrondol),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T12JumlahTandan = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahTandan),
                            T12KgTBS = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgTBS + o.KgBrondol),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T13JumlahTandan = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahTandan),
                            T13KgTBS = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgTBS + o.KgBrondol),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T14JumlahTandan = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahTandan),
                            T14KgTBS = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgTBS + o.KgBrondol),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T15JumlahTandan = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahTandan),
                            T15KgTBS = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgTBS + o.KgBrondol),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T16JumlahTandan = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahTandan),
                            T16KgTBS = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgTBS + o.KgBrondol),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T17JumlahTandan = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahTandan),
                            T17KgTBS = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgTBS + o.KgBrondol),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T18JumlahTandan = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahTandan),
                            T18KgTBS = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgTBS + o.KgBrondol),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T19JumlahTandan = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahTandan),
                            T19KgTBS = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgTBS + o.KgBrondol),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T20JumlahTandan = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahTandan),
                            T20KgTBS = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgTBS + o.KgBrondol),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T21JumlahTandan = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahTandan),
                            T21KgTBS = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgTBS + o.KgBrondol),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T22JumlahTandan = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahTandan),
                            T22KgTBS = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgTBS + o.KgBrondol),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T23JumlahTandan = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahTandan),
                            T23KgTBS = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgTBS + o.KgBrondol),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T24JumlahTandan = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahTandan),
                            T24KgTBS = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgTBS + o.KgBrondol),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T25JumlahTandan = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahTandan),
                            T25KgTBS = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgTBS + o.KgBrondol),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T26JumlahTandan = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahTandan),
                            T26KgTBS = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgTBS + o.KgBrondol),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T27JumlahTandan = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahTandan),
                            T27KgTBS = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgTBS + o.KgBrondol),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T28JumlahTandan = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahTandan),
                            T28KgTBS = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgTBS + o.KgBrondol),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T29JumlahTandan = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahTandan),
                            T29KgTBS = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgTBS + o.KgBrondol),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T30JumlahTandan = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahTandan),
                            T30KgTBS = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgTBS + o.KgBrondol),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T31JumlahTandan = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahTandan),
                            T31KgTBS = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgTBS + o.KgBrondol),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> kebun = new List<RotasiPanenPerBulan>();
            double RealisasiJelajah = 0;
            double NormaJelajah = 0;
            foreach (var m in model)
            {
                RealisasiJelajah = (double)(m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah 
                    + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah + m.T13LuasJelajah
                    + m.T14LuasJelajah + m.T15LuasJelajah + m.T16LuasJelajah + m.T17LuasJelajah + m.T18LuasJelajah + m.T19LuasJelajah + m.T20LuasJelajah
                    + m.T21LuasJelajah + m.T22LuasJelajah + m.T23LuasJelajah + m.T24LuasJelajah + m.T25LuasJelajah + m.T26LuasJelajah + m.T27LuasJelajah
                    + m.T28LuasJelajah + m.T29LuasJelajah + m.T30LuasJelajah + m.T31LuasJelajah) / (double)m.LuasAreal;
                NormaJelajah = (double)(CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ? 
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).Rotasi / 12 : 0) ;

                if ((int)RealisasiJelajah == 0)
                    m.Warna = "black";
                else if ((int)RealisasiJelajah == 1)
                    m.Warna = "red";
                else if ((int)RealisasiJelajah == 2)
                    m.Warna = "orange";
                else if ((int)RealisasiJelajah == 3)
                    m.Warna = "yellow";
                else if ((int)RealisasiJelajah == 4)
                    m.Warna = "green";
                else
                    m.Warna = "blue";

                kebun.Add(m);
            }

            return Json(kebun.OrderBy(o=>o.NamaLokasi).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult KebunGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            if (pctBalai == "") pctBalai = "100";
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "01" && o.status == "TM")
                         where (userStatus.NamaLokasiKerja == "Afdeling" ? m.kodekebun == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                         userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.kodekebun).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                         group m by new { g1 = m.kodekebun } into g
                         select new PetaView
                         {
                             KodeKebun = g.Key.g1,
                             Id = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                             Peta = CRUDKebunPeta.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebunPeta.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDKebunPeta.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                             Nama = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                         }).ToList();

            List<PetaView> kebun = new List<PetaView>();
            foreach(var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal avg = 0;
                for(int i=0;i<strTahunTanam.Length;i++)
                {
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year-int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi/12 : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                m.Value1 = (int) ((double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01" && o.Bulan == int.Parse(bulan) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah) / CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "01", "TM")));
                m.Value3 =  (int) ((double) CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "01", "TM"));
                if (warna == "")
                {
                    if ((int)m.Value1 == 0)
                        m.Warna = "black";
                    else if ((int)m.Value1 == 1)
                        m.Warna = "red";
                    else if ((int)m.Value1 == 2)
                        m.Warna = "orange";
                    else if ((int)m.Value1 == 3)
                        m.Warna = "yellow";
                    else if ((int)m.Value1 == 4)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                kebun.Add(m);
            }

            foreach (var k in kebun)
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta);
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    feature.Attributes.AddAttribute("NAMA_KEBUN", k.Nama);
                    feature.Attributes.AddAttribute("TOPLEFT_X", feature.Geometry.Coordinates.Min(o => o.X));
                    feature.Attributes.AddAttribute("TOPLEFT_Y", feature.Geometry.Coordinates.Min(o => o.Y));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_X", feature.Geometry.Coordinates.Max(o => o.X));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_Y", feature.Geometry.Coordinates.Max(o => o.Y));
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.Id);
                    feature.Attributes.AddAttribute("VALUE1", k.Value1);
                    feature.Attributes.AddAttribute("VALUE2", k.Value2);
                    feature.Attributes.AddAttribute("VALUE3", k.Value3);
                    featureCollection.Add(feature);
                }
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }

        [Authorize]
        public ActionResult ChartRotasiPerAfdeling(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPerTanggal().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 2) == Id)
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "01", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult RotasiPerAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPerTanggal().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0,2)==Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "01", "TM",g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T01JumlahTandan = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahTandan),
                            T01KgTBS = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgTBS + o.KgBrondol),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T02JumlahTandan = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahTandan),
                            T02KgTBS = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgTBS + o.KgBrondol),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T03JumlahTandan = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahTandan),
                            T03KgTBS = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgTBS + o.KgBrondol),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T04JumlahTandan = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahTandan),
                            T04KgTBS = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgTBS + o.KgBrondol),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T05JumlahTandan = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahTandan),
                            T05KgTBS = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgTBS + o.KgBrondol),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T06JumlahTandan = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahTandan),
                            T06KgTBS = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgTBS + o.KgBrondol),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T07JumlahTandan = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahTandan),
                            T07KgTBS = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgTBS + o.KgBrondol),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T08JumlahTandan = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahTandan),
                            T08KgTBS = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgTBS + o.KgBrondol),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T09JumlahTandan = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahTandan),
                            T09KgTBS = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgTBS + o.KgBrondol),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T10JumlahTandan = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahTandan),
                            T10KgTBS = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgTBS + o.KgBrondol),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T11JumlahTandan = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahTandan),
                            T11KgTBS = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgTBS + o.KgBrondol),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T12JumlahTandan = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahTandan),
                            T12KgTBS = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgTBS + o.KgBrondol),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T13JumlahTandan = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahTandan),
                            T13KgTBS = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgTBS + o.KgBrondol),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T14JumlahTandan = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahTandan),
                            T14KgTBS = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgTBS + o.KgBrondol),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T15JumlahTandan = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahTandan),
                            T15KgTBS = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgTBS + o.KgBrondol),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T16JumlahTandan = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahTandan),
                            T16KgTBS = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgTBS + o.KgBrondol),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T17JumlahTandan = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahTandan),
                            T17KgTBS = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgTBS + o.KgBrondol),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T18JumlahTandan = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahTandan),
                            T18KgTBS = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgTBS + o.KgBrondol),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T19JumlahTandan = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahTandan),
                            T19KgTBS = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgTBS + o.KgBrondol),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T20JumlahTandan = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahTandan),
                            T20KgTBS = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgTBS + o.KgBrondol),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T21JumlahTandan = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahTandan),
                            T21KgTBS = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgTBS + o.KgBrondol),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T22JumlahTandan = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahTandan),
                            T22KgTBS = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgTBS + o.KgBrondol),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T23JumlahTandan = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahTandan),
                            T23KgTBS = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgTBS + o.KgBrondol),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T24JumlahTandan = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahTandan),
                            T24KgTBS = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgTBS + o.KgBrondol),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T25JumlahTandan = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahTandan),
                            T25KgTBS = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgTBS + o.KgBrondol),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T26JumlahTandan = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahTandan),
                            T26KgTBS = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgTBS + o.KgBrondol),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T27JumlahTandan = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahTandan),
                            T27KgTBS = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgTBS + o.KgBrondol),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T28JumlahTandan = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahTandan),
                            T28KgTBS = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgTBS + o.KgBrondol),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T29JumlahTandan = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahTandan),
                            T29KgTBS = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgTBS + o.KgBrondol),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T30JumlahTandan = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahTandan),
                            T30KgTBS = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgTBS + o.KgBrondol),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T31JumlahTandan = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahTandan),
                            T31KgTBS = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgTBS + o.KgBrondol),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> afdeling = new List<RotasiPanenPerBulan>();
            double RealisasiJelajah = 0;
            double NormaJelajah = 0;
            foreach (var m in model)
            {
                RealisasiJelajah = (double)(m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah
                    + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah + m.T13LuasJelajah
                    + m.T14LuasJelajah + m.T15LuasJelajah + m.T16LuasJelajah + m.T17LuasJelajah + m.T18LuasJelajah + m.T19LuasJelajah + m.T20LuasJelajah
                    + m.T21LuasJelajah + m.T22LuasJelajah + m.T23LuasJelajah + m.T24LuasJelajah + m.T25LuasJelajah + m.T26LuasJelajah + m.T27LuasJelajah
                    + m.T28LuasJelajah + m.T29LuasJelajah + m.T30LuasJelajah + m.T31LuasJelajah) / (double)m.LuasAreal;
                NormaJelajah = (double)(CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ? 
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).Rotasi / 12 : 0);

                if ((int)RealisasiJelajah == 0)
                    m.Warna = "black";
                else if ((int)RealisasiJelajah == 1)
                    m.Warna = "red";
                else if ((int)RealisasiJelajah == 2)
                    m.Warna = "orange";
                else if ((int)RealisasiJelajah == 3)
                    m.Warna = "yellow";
                else if ((int)RealisasiJelajah == 4)
                    m.Warna = "green";
                else
                    m.Warna = "blue";

                afdeling.Add(m);
            }
            return Json(afdeling.OrderBy(o => o.NamaLokasi).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult AfdelingGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            if (pctBalai == "") pctBalai = "100";

            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodekebun == Id).Where(o => o.kodebudidaya == "01" && o.status == "TM")
                         group m by new { g1 = m.kodekebun + m.kodeafdeling } into g
                         select new PetaView
                         {
                             KodeKebun = g.Key.g1,
                             Id = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).AfdelingId,
                             Peta = CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                             Nama = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                         }).ToList();

            List<PetaView> afdeling = new List<PetaView>();
            foreach (var m in model)
            {

                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal avg = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi/12 : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                m.Value1 = (int) ((double) (CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.Bulan == int.Parse(bulan) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah) / CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun, "01", "TM")));
                m.Value3 = (int) ((double) CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun, "01", "TM"));
                if (warna == "")
                {
                    if ((int)m.Value1 == 0)
                        m.Warna = "black";
                    else if ((int)m.Value1 == 1)
                        m.Warna = "red";
                    else if ((int)m.Value1 == 2)
                        m.Warna = "orange";
                    else if ((int)m.Value1 == 3)
                        m.Warna = "yellow";
                    else if ((int)m.Value1 == 4)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";

                }
                else
                    m.Warna = warna;

                afdeling.Add(m);
            }

            foreach (var k in afdeling)
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta.ToString());
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    feature.Attributes.AddAttribute("NAMA_KEBUN", k.Nama);
                    feature.Attributes.AddAttribute("TOPLEFT_X", feature.Geometry.Coordinates.Min(o => o.X));
                    feature.Attributes.AddAttribute("TOPLEFT_Y", feature.Geometry.Coordinates.Min(o => o.Y));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_X", feature.Geometry.Coordinates.Max(o => o.X));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_Y", feature.Geometry.Coordinates.Max(o => o.Y));
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.Id);
                    feature.Attributes.AddAttribute("VALUE1", k.Value1);
                    feature.Attributes.AddAttribute("VALUE2", k.Value2);
                    feature.Attributes.AddAttribute("VALUE3", k.Value3);
                    featureCollection.Add(feature);
                }
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }

        [Authorize]
        public ActionResult ChartRotasiPerBlok(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 4) == Id)
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "01", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult RotasiPerBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 4) == Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "01", "TM",g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T01JumlahTandan = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahTandan),
                            T01KgTBS = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgTBS + o.KgBrondol),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T02JumlahTandan = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahTandan),
                            T02KgTBS = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgTBS + o.KgBrondol),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T03JumlahTandan = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahTandan),
                            T03KgTBS = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgTBS + o.KgBrondol),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T04JumlahTandan = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahTandan),
                            T04KgTBS = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgTBS + o.KgBrondol),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T05JumlahTandan = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahTandan),
                            T05KgTBS = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgTBS + o.KgBrondol),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T06JumlahTandan = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahTandan),
                            T06KgTBS = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgTBS + o.KgBrondol),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T07JumlahTandan = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahTandan),
                            T07KgTBS = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgTBS + o.KgBrondol),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T08JumlahTandan = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahTandan),
                            T08KgTBS = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgTBS + o.KgBrondol),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T09JumlahTandan = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahTandan),
                            T09KgTBS = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgTBS + o.KgBrondol),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T10JumlahTandan = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahTandan),
                            T10KgTBS = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgTBS + o.KgBrondol),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T11JumlahTandan = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahTandan),
                            T11KgTBS = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgTBS + o.KgBrondol),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T12JumlahTandan = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahTandan),
                            T12KgTBS = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgTBS + o.KgBrondol),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T13JumlahTandan = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahTandan),
                            T13KgTBS = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgTBS + o.KgBrondol),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T14JumlahTandan = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahTandan),
                            T14KgTBS = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgTBS + o.KgBrondol),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T15JumlahTandan = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahTandan),
                            T15KgTBS = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgTBS + o.KgBrondol),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T16JumlahTandan = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahTandan),
                            T16KgTBS = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgTBS + o.KgBrondol),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T17JumlahTandan = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahTandan),
                            T17KgTBS = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgTBS + o.KgBrondol),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T18JumlahTandan = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahTandan),
                            T18KgTBS = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgTBS + o.KgBrondol),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T19JumlahTandan = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahTandan),
                            T19KgTBS = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgTBS + o.KgBrondol),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T20JumlahTandan = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahTandan),
                            T20KgTBS = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgTBS + o.KgBrondol),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T21JumlahTandan = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahTandan),
                            T21KgTBS = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgTBS + o.KgBrondol),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T22JumlahTandan = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahTandan),
                            T22KgTBS = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgTBS + o.KgBrondol),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T23JumlahTandan = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahTandan),
                            T23KgTBS = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgTBS + o.KgBrondol),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T24JumlahTandan = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahTandan),
                            T24KgTBS = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgTBS + o.KgBrondol),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T25JumlahTandan = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahTandan),
                            T25KgTBS = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgTBS + o.KgBrondol),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T26JumlahTandan = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahTandan),
                            T26KgTBS = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgTBS + o.KgBrondol),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T27JumlahTandan = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahTandan),
                            T27KgTBS = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgTBS + o.KgBrondol),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T28JumlahTandan = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahTandan),
                            T28KgTBS = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgTBS + o.KgBrondol),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T29JumlahTandan = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahTandan),
                            T29KgTBS = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgTBS + o.KgBrondol),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T30JumlahTandan = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahTandan),
                            T30KgTBS = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgTBS + o.KgBrondol),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T31JumlahTandan = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahTandan),
                            T31KgTBS = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgTBS + o.KgBrondol),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            double RealisasiJelajah = 0;
            double NormaJelajah = 0;
            foreach (var m in model)
            {
                RealisasiJelajah = (double)(m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah
                    + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah + m.T13LuasJelajah
                    + m.T14LuasJelajah + m.T15LuasJelajah + m.T16LuasJelajah + m.T17LuasJelajah + m.T18LuasJelajah + m.T19LuasJelajah + m.T20LuasJelajah
                    + m.T21LuasJelajah + m.T22LuasJelajah + m.T23LuasJelajah + m.T24LuasJelajah + m.T25LuasJelajah + m.T26LuasJelajah + m.T27LuasJelajah
                    + m.T28LuasJelajah + m.T29LuasJelajah + m.T30LuasJelajah + m.T31LuasJelajah) / (double)m.LuasAreal;
                NormaJelajah = (double)(CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ? 
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).Rotasi / 12 : 0);

                if ((int)RealisasiJelajah == 0)
                    m.Warna = "black";
                else if ((int)RealisasiJelajah == 1)
                    m.Warna = "red";
                else if ((int)RealisasiJelajah == 2)
                    m.Warna = "orange";
                else if ((int)RealisasiJelajah == 3)
                    m.Warna = "yellow";
                else if ((int)RealisasiJelajah == 4)
                    m.Warna = "green";
                else
                    m.Warna = "blue";

                blok.Add(m);
            }
            return Json(blok.OrderBy(o => o.NamaLokasi).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult BlokGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodekebun + o.kodeafdeling == Id).Where(o => o.kodebudidaya == "01" && o.status == "TM")
                         group m by new { g1 = m.kodekebun + m.kodeafdeling + m.kodeblok } into g
                         select new PetaView
                         {
                             KodeKebun = g.Key.g1,
                             Id = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                             Peta = CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                             Nama = g.Select(o => o.namablok).FirstOrDefault(),
                             TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                         }).ToList();

            List<PetaView> blok = new List<PetaView>();
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal avg = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi/12 : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                m.Value1 = (int)((double) (CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.Bulan == int.Parse(bulan) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah) / CRUDRef_Areal.CRUD.HitungLuasArealBlok(m.KodeKebun, "01", "TM")));
                m.Value3 = (int)((double) CRUDRef_Areal.CRUD.HitungLuasArealBlok(m.KodeKebun, "01", "TM"));

                if (warna == "")
                {
                    if ((int)m.Value1 == 0)
                        m.Warna = "black";
                    else if ((int)m.Value1 == 1)
                        m.Warna = "red";
                    else if ((int)m.Value1 == 2)
                        m.Warna = "orange";
                    else if ((int)m.Value1 == 3)
                        m.Warna = "yellow";
                    else if ((int)m.Value1 == 4)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";

                }
                else
                    m.Warna = warna;

                blok.Add(m);
            }

            foreach (var k in blok)
            {
                if (k.Peta != "")
                {
                    var wktReader = new NetTopologySuite.IO.WKTReader();
                    var geom = wktReader.Read(k.Peta.ToString());
                    var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                    feature.Attributes.AddAttribute("NAMA_KEBUN", k.Nama);
                    feature.Attributes.AddAttribute("TOPLEFT_X", feature.Geometry.Coordinates.Min(o => o.X));
                    feature.Attributes.AddAttribute("TOPLEFT_Y", feature.Geometry.Coordinates.Min(o => o.Y));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_X", feature.Geometry.Coordinates.Max(o => o.X));
                    feature.Attributes.AddAttribute("BOTTOMRIGHT_Y", feature.Geometry.Coordinates.Max(o => o.Y));
                    feature.Attributes.AddAttribute("COLOR", k.Warna);
                    feature.Attributes.AddAttribute("ID", k.Id);
                    feature.Attributes.AddAttribute("VALUE1", k.Value1);
                    feature.Attributes.AddAttribute("VALUE2", k.Value2);
                    feature.Attributes.AddAttribute("VALUE3", k.Value3);
                    featureCollection.Add(feature);
                }
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }
    }
}