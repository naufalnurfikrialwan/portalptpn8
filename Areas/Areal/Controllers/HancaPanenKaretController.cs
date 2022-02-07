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
    public class HancaPanenKaretController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasKaret
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Hanca Panen";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult ChartHancaPerKebun(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPerTanggal().Where(o => o.KodeBudidaya == "00")
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok} into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                            T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                            T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                            T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                            T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                            T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                            T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                            T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                            T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                            T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                            T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                            T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                            T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                            T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                            T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                            T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                            T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                            T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                            T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                            T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                            T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                            T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                            T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                            T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                            T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                            T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                            T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                            T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                            T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                            T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                            T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                            T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                         }).ToList();

            List<RotasiPanenPerBulan> kebun = new List<RotasiPanenPerBulan>();
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal normaHanca = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                }

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    m.T01LuasJelajah = m.T01JumlahPohon / normaHanca;
                    m.T02LuasJelajah = m.T02JumlahPohon / normaHanca;
                    m.T03LuasJelajah = m.T03JumlahPohon / normaHanca;
                    m.T04LuasJelajah = m.T04JumlahPohon / normaHanca;
                    m.T05LuasJelajah = m.T05JumlahPohon / normaHanca;
                    m.T06LuasJelajah = m.T06JumlahPohon / normaHanca;
                    m.T07LuasJelajah = m.T07JumlahPohon / normaHanca;
                    m.T08LuasJelajah = m.T08JumlahPohon / normaHanca;
                    m.T09LuasJelajah = m.T09JumlahPohon / normaHanca;
                    m.T10LuasJelajah = m.T10JumlahPohon / normaHanca;
                    m.T11LuasJelajah = m.T11JumlahPohon / normaHanca;
                    m.T12LuasJelajah = m.T12JumlahPohon / normaHanca;
                    m.T13LuasJelajah = m.T13JumlahPohon / normaHanca;
                    m.T14LuasJelajah = m.T14JumlahPohon / normaHanca;
                    m.T15LuasJelajah = m.T15JumlahPohon / normaHanca;
                    m.T16LuasJelajah = m.T16JumlahPohon / normaHanca;
                    m.T17LuasJelajah = m.T17JumlahPohon / normaHanca;
                    m.T18LuasJelajah = m.T18JumlahPohon / normaHanca;
                    m.T19LuasJelajah = m.T19JumlahPohon / normaHanca;
                    m.T20LuasJelajah = m.T20JumlahPohon / normaHanca;
                    m.T21LuasJelajah = m.T21JumlahPohon / normaHanca;
                    m.T22LuasJelajah = m.T22JumlahPohon / normaHanca;
                    m.T23LuasJelajah = m.T23JumlahPohon / normaHanca;
                    m.T24LuasJelajah = m.T24JumlahPohon / normaHanca;
                    m.T25LuasJelajah = m.T25JumlahPohon / normaHanca;
                    m.T26LuasJelajah = m.T26JumlahPohon / normaHanca;
                    m.T27LuasJelajah = m.T27JumlahPohon / normaHanca;
                    m.T28LuasJelajah = m.T28JumlahPohon / normaHanca;
                    m.T29LuasJelajah = m.T29JumlahPohon / normaHanca;
                    m.T30LuasJelajah = m.T30JumlahPohon / normaHanca;
                    m.T31LuasJelajah = m.T31JumlahPohon / normaHanca;
                    kebun.Add(m);
                }
            }

            return Json(kebun, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult HancaPerKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPerTanggal().Where(o => o.KodeBudidaya == "00")
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun)-int.Parse(m.TahunTanam) equals n.Umur
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam, n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            LuasAreal= CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                            T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                            T01KgLateks = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgLateks + 0),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                            T02KgLateks = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLateks + 0),
                            T02KgLump = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLump + 0),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                            T03KgLateks = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLateks + 0),
                            T03KgLump = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLump + 0),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                            T04KgLateks = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLateks + 0),
                            T04KgLump = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLump + 0),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                            T05KgLateks = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLateks + 0),
                            T05KgLump = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLump + 0),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                            T06KgLateks = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLateks + 0),
                            T06KgLump = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLump + 0),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                            T07KgLateks = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLateks + 0),
                            T07KgLump = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLump + 0),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                            T08KgLateks = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLateks + 0),
                            T08KgLump = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLump + 0),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                            T09KgLateks = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLateks + 0),
                            T09KgLump = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLump + 0),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                            T10KgLateks = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLateks + 0),
                            T10KgLump = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLump + 0),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                            T11KgLateks = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLateks + 0),
                            T11KgLump = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLump + 0),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                            T12KgLateks = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLateks + 0),
                            T12KgLump = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLump + 0),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                            T13KgLateks = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLateks + 0),
                            T13KgLump = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLump + 0),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                            T14KgLateks = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLateks + 0),
                            T14KgLump = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLump + 0),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                            T15KgLateks = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLateks + 0),
                            T15KgLump = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLump + 0),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                            T16KgLateks = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLateks + 0),
                            T16KgLump = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLump + 0),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                            T17KgLateks = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLateks + 0),
                            T17KgLump = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLump + 0),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                            T18KgLateks = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLateks + 0),
                            T18KgLump = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLump + 0),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                            T19KgLateks = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLateks + 0),
                            T19KgLump = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLump + 0),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                            T20KgLateks = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLateks + 0),
                            T20KgLump = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLump + 0),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                            T21KgLateks = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLateks + 0),
                            T21KgLump = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLump + 0),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                            T22KgLateks = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLateks + 0),
                            T22KgLump = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLump + 0),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                            T23KgLateks = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLateks + 0),
                            T23KgLump = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLump + 0),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                            T24KgLateks = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLateks + 0),
                            T24KgLump = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLump + 0),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                            T25KgLateks = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLateks + 0),
                            T25KgLump = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLump + 0),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                            T26KgLateks = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLateks + 0),
                            T26KgLump = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLump + 0),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                            T27KgLateks = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLateks + 0),
                            T27KgLump = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLump + 0),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                            T28KgLateks = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLateks + 0),
                            T28KgLump = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLump + 0),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                            T29KgLateks = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLateks + 0),
                            T29KgLump = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLump + 0),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                            T30KgLateks = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLateks + 0),
                            T30KgLump = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLump + 0),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                            T31KgLateks = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLateks + 0),
                            T31KgLump = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLump + 0),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                        };

            List<RotasiPanenPerBulan> kebun = new List<RotasiPanenPerBulan>();
            decimal jumlahPohon = 0;
            decimal jumlahHK = 0;
            decimal jumlahHanca = 0;
            decimal normaHanca = 0;
            decimal normaRotasi = 0;

            foreach (var o in model)
            {
                jumlahPohon = o.T01JumlahPohon + o.T02JumlahPohon + o.T03JumlahPohon + o.T04JumlahPohon + o.T05JumlahPohon +
                    o.T06JumlahPohon + o.T07JumlahPohon + o.T08JumlahPohon + o.T09JumlahPohon + o.T10JumlahPohon +
                    o.T11JumlahPohon + o.T12JumlahPohon + o.T13JumlahPohon + o.T14JumlahPohon + o.T15JumlahPohon +
                    o.T16JumlahPohon + o.T17JumlahPohon + o.T18JumlahPohon + o.T19JumlahPohon + o.T20JumlahPohon +
                    o.T21JumlahPohon + o.T22JumlahPohon + o.T23JumlahPohon + o.T24JumlahPohon + o.T25JumlahPohon +
                    o.T26JumlahPohon + o.T27JumlahPohon + o.T28JumlahPohon + o.T29JumlahPohon + o.T30JumlahPohon + o.T31JumlahPohon;

                jumlahHK = o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                    o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                    o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                    o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                    o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                    o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK;


                normaHanca = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ? 
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).PohonDisadapPerHanca: 0) ;

                normaRotasi = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ?
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).Rotasi/12 : 0);

                if (normaHanca > 0) jumlahHanca = jumlahPohon / normaHanca;

                if ((int)jumlahHanca == 0)
                    o.Warna = "black";
                if (jumlahHanca <= jumlahHK*70/100)
                    o.Warna = "red";
                else if (jumlahHanca <= jumlahHK*80/100)
                    o.Warna = "orange";
                else if (jumlahHanca <= jumlahHK*90/100)
                    o.Warna = "yellow";
                else if (jumlahHanca <= jumlahHK)
                    o.Warna = "green";
                else
                    o.Warna = "blue";

                kebun.Add(o);
            }
            var jsonResult = Json(kebun.OrderBy(o => o.NamaLokasi).ToDataSourceResult(request));
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
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

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var model1 = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "00" && o.status == "TM")
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
            foreach(var m in model1)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal jumlahPohon = 0;
                decimal jumlahHK = 0;
                decimal jumlahHanca = 0;
                decimal normaHanca = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;

                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi/12 : 0);
                }

                jumlahPohon = CRUDHasilPanen.CRUD.GetHasilPanenPerKebun()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahPohon);
                jumlahHK = CRUDHasilPanen.CRUD.GetHasilPanenPerKebun()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK);

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                    jumlahHanca = jumlahPohon / normaHanca;
                }
                else
                {
                    normaHanca = 0;
                    normaRotasi = 0;
                    jumlahHanca = 0;
                }

                
                m.Value1 = (double) jumlahHanca;
                m.Value2 = (double) jumlahHK;

                if (warna == "")
                {
                    if((int)jumlahHanca == 0)
                        m.Warna = "black";
                    else if (jumlahHanca <= jumlahHK * 70 / 100)
                        m.Warna = "red";
                    else if (jumlahHanca <= jumlahHK * 80 / 100)
                        m.Warna = "orange";
                    else if (jumlahHanca <= jumlahHK * 90/100)
                        m.Warna = "yellow";
                    else if (jumlahHanca <= jumlahHK)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM"));
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
        public ActionResult ChartHancaPerAfdeling(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPerTanggal().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 2) == Id)
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok} into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "00", "TM"),
                             T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                             T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                             T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                             T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                             T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                             T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                             T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                             T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                             T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                             T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                             T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                             T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                             T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                             T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                             T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                             T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                             T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                             T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                             T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                             T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                             T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                             T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                             T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                             T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                             T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                             T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                             T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                             T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                             T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                             T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                             T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                         }).ToList();

            List<RotasiPanenPerBulan> afdeling = new List<RotasiPanenPerBulan>();
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal normaHanca = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                }

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    m.T01LuasJelajah = m.T01JumlahPohon / normaHanca;
                    m.T02LuasJelajah = m.T02JumlahPohon / normaHanca;
                    m.T03LuasJelajah = m.T03JumlahPohon / normaHanca;
                    m.T04LuasJelajah = m.T04JumlahPohon / normaHanca;
                    m.T05LuasJelajah = m.T05JumlahPohon / normaHanca;
                    m.T06LuasJelajah = m.T06JumlahPohon / normaHanca;
                    m.T07LuasJelajah = m.T07JumlahPohon / normaHanca;
                    m.T08LuasJelajah = m.T08JumlahPohon / normaHanca;
                    m.T09LuasJelajah = m.T09JumlahPohon / normaHanca;
                    m.T10LuasJelajah = m.T10JumlahPohon / normaHanca;
                    m.T11LuasJelajah = m.T11JumlahPohon / normaHanca;
                    m.T12LuasJelajah = m.T12JumlahPohon / normaHanca;
                    m.T13LuasJelajah = m.T13JumlahPohon / normaHanca;
                    m.T14LuasJelajah = m.T14JumlahPohon / normaHanca;
                    m.T15LuasJelajah = m.T15JumlahPohon / normaHanca;
                    m.T16LuasJelajah = m.T16JumlahPohon / normaHanca;
                    m.T17LuasJelajah = m.T17JumlahPohon / normaHanca;
                    m.T18LuasJelajah = m.T18JumlahPohon / normaHanca;
                    m.T19LuasJelajah = m.T19JumlahPohon / normaHanca;
                    m.T20LuasJelajah = m.T20JumlahPohon / normaHanca;
                    m.T21LuasJelajah = m.T21JumlahPohon / normaHanca;
                    m.T22LuasJelajah = m.T22JumlahPohon / normaHanca;
                    m.T23LuasJelajah = m.T23JumlahPohon / normaHanca;
                    m.T24LuasJelajah = m.T24JumlahPohon / normaHanca;
                    m.T25LuasJelajah = m.T25JumlahPohon / normaHanca;
                    m.T26LuasJelajah = m.T26JumlahPohon / normaHanca;
                    m.T27LuasJelajah = m.T27JumlahPohon / normaHanca;
                    m.T28LuasJelajah = m.T28JumlahPohon / normaHanca;
                    m.T29LuasJelajah = m.T29JumlahPohon / normaHanca;
                    m.T30LuasJelajah = m.T30JumlahPohon / normaHanca;
                    m.T31LuasJelajah = m.T31JumlahPohon / normaHanca;
                    afdeling.Add(m);
                }
            }

            return Json(afdeling, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult HancaPerAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPerTanggal().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0,2)==Id)
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun) - int.Parse(m.TahunTanam) equals n.Umur
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam,n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM",g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                            T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                            T01KgLateks = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgLateks + 0),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                            T02KgLateks = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLateks + 0),
                            T02KgLump = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLump + 0),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                            T03KgLateks = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLateks + 0),
                            T03KgLump = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLump + 0),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                            T04KgLateks = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLateks + 0),
                            T04KgLump = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLump + 0),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                            T05KgLateks = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLateks + 0),
                            T05KgLump = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLump + 0),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                            T06KgLateks = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLateks + 0),
                            T06KgLump = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLump + 0),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                            T07KgLateks = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLateks + 0),
                            T07KgLump = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLump + 0),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                            T08KgLateks = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLateks + 0),
                            T08KgLump = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLump + 0),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                            T09KgLateks = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLateks + 0),
                            T09KgLump = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLump + 0),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                            T10KgLateks = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLateks + 0),
                            T10KgLump = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLump + 0),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                            T11KgLateks = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLateks + 0),
                            T11KgLump = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLump + 0),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                            T12KgLateks = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLateks + 0),
                            T12KgLump = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLump + 0),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                            T13KgLateks = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLateks + 0),
                            T13KgLump = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLump + 0),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                            T14KgLateks = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLateks + 0),
                            T14KgLump = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLump + 0),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                            T15KgLateks = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLateks + 0),
                            T15KgLump = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLump + 0),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                            T16KgLateks = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLateks + 0),
                            T16KgLump = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLump + 0),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                            T17KgLateks = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLateks + 0),
                            T17KgLump = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLump + 0),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                            T18KgLateks = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLateks + 0),
                            T18KgLump = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLump + 0),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                            T19KgLateks = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLateks + 0),
                            T19KgLump = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLump + 0),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                            T20KgLateks = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLateks + 0),
                            T20KgLump = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLump + 0),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                            T21KgLateks = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLateks + 0),
                            T21KgLump = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLump + 0),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                            T22KgLateks = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLateks + 0),
                            T22KgLump = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLump + 0),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                            T23KgLateks = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLateks + 0),
                            T23KgLump = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLump + 0),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                            T24KgLateks = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLateks + 0),
                            T24KgLump = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLump + 0),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                            T25KgLateks = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLateks + 0),
                            T25KgLump = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLump + 0),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                            T26KgLateks = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLateks + 0),
                            T26KgLump = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLump + 0),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                            T27KgLateks = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLateks + 0),
                            T27KgLump = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLump + 0),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                            T28KgLateks = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLateks + 0),
                            T28KgLump = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLump + 0),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                            T29KgLateks = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLateks + 0),
                            T29KgLump = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLump + 0),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                            T30KgLateks = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLateks + 0),
                            T30KgLump = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLump + 0),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                            T31KgLateks = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLateks + 0),
                            T31KgLump = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLump + 0),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                        };

            List<RotasiPanenPerBulan> afdeling = new List<RotasiPanenPerBulan>();
            decimal jumlahPohon = 0;
            decimal jumlahHK = 0;
            decimal jumlahHanca = 0;
            decimal normaHanca = 0;
            decimal normaRotasi = 0;

            foreach (var o in model)
            {
                jumlahPohon = o.T01JumlahPohon + o.T02JumlahPohon + o.T03JumlahPohon + o.T04JumlahPohon + o.T05JumlahPohon +
                    o.T06JumlahPohon + o.T07JumlahPohon + o.T08JumlahPohon + o.T09JumlahPohon + o.T10JumlahPohon +
                    o.T11JumlahPohon + o.T12JumlahPohon + o.T13JumlahPohon + o.T14JumlahPohon + o.T15JumlahPohon +
                    o.T16JumlahPohon + o.T17JumlahPohon + o.T18JumlahPohon + o.T19JumlahPohon + o.T20JumlahPohon +
                    o.T21JumlahPohon + o.T22JumlahPohon + o.T23JumlahPohon + o.T24JumlahPohon + o.T25JumlahPohon +
                    o.T26JumlahPohon + o.T27JumlahPohon + o.T28JumlahPohon + o.T29JumlahPohon + o.T30JumlahPohon + o.T31JumlahPohon;

                jumlahHK = o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                    o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                    o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                    o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                    o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                    o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK;


                normaHanca = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ?
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).PohonDisadapPerHanca : 0);

                normaRotasi = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ?
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).Rotasi / 12 : 0);

                if (normaHanca > 0) jumlahHanca = jumlahPohon / normaHanca;

                if ((int)jumlahHanca == 0)
                    o.Warna = "black";
                if (jumlahHanca <= jumlahHK * 70 / 100)
                    o.Warna = "red";
                else if (jumlahHanca <= jumlahHK * 80 / 100)
                    o.Warna = "orange";
                else if (jumlahHanca <= jumlahHK * 90 / 100)
                    o.Warna = "yellow";
                else if (jumlahHanca <= jumlahHK)
                    o.Warna = "green";
                else
                    o.Warna = "blue";

                afdeling.Add(o);
            }
            return Json(afdeling.ToDataSourceResult(request));
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

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodekebun == Id).Where(o => o.kodebudidaya == "00" && o.status == "TM")
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
                decimal jumlahPohon = 0;
                decimal jumlahHK = 0;
                decimal jumlahHanca = 0;
                decimal normaHanca = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;

                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi / 12 : 0);
                }

                jumlahPohon = CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahPohon);
                jumlahHK = CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK);

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                    jumlahHanca = jumlahPohon / normaHanca;
                }
                else
                {
                    normaHanca = 0;
                    normaRotasi = 0;
                    jumlahHanca = 0;
                }


                m.Value1 = (double)jumlahHanca;
                m.Value2 = (double)jumlahHK;

                if (warna == "")
                {
                    if ((int)jumlahHanca == 0)
                        m.Warna = "black";
                    else if (jumlahHanca <= jumlahHK * 70 / 100)
                        m.Warna = "red";
                    else if (jumlahHanca <= jumlahHK * 80 / 100)
                        m.Warna = "orange";
                    else if (jumlahHanca <= jumlahHK * 90 / 100)
                        m.Warna = "yellow";
                    else if (jumlahHanca <= jumlahHK)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM"));
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
        public ActionResult ChartHancaPerBlok(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 4) == Id)
                         where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                         group m by new { m.KodeBlok} into g
                         select new RotasiPanenPerBulan
                         {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "00", "TM"),
                             T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                             T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                             T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                             T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                             T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                             T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                             T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                             T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                             T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                             T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                             T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                             T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                             T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                             T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                             T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                             T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                             T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                             T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                             T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                             T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                             T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                             T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                             T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                             T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                             T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                             T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                             T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                             T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                             T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                             T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                             T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                         }).ToList();

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal normaHanca = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                }

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    m.T01LuasJelajah = m.T01JumlahPohon / normaHanca;
                    m.T02LuasJelajah = m.T02JumlahPohon / normaHanca;
                    m.T03LuasJelajah = m.T03JumlahPohon / normaHanca;
                    m.T04LuasJelajah = m.T04JumlahPohon / normaHanca;
                    m.T05LuasJelajah = m.T05JumlahPohon / normaHanca;
                    m.T06LuasJelajah = m.T06JumlahPohon / normaHanca;
                    m.T07LuasJelajah = m.T07JumlahPohon / normaHanca;
                    m.T08LuasJelajah = m.T08JumlahPohon / normaHanca;
                    m.T09LuasJelajah = m.T09JumlahPohon / normaHanca;
                    m.T10LuasJelajah = m.T10JumlahPohon / normaHanca;
                    m.T11LuasJelajah = m.T11JumlahPohon / normaHanca;
                    m.T12LuasJelajah = m.T12JumlahPohon / normaHanca;
                    m.T13LuasJelajah = m.T13JumlahPohon / normaHanca;
                    m.T14LuasJelajah = m.T14JumlahPohon / normaHanca;
                    m.T15LuasJelajah = m.T15JumlahPohon / normaHanca;
                    m.T16LuasJelajah = m.T16JumlahPohon / normaHanca;
                    m.T17LuasJelajah = m.T17JumlahPohon / normaHanca;
                    m.T18LuasJelajah = m.T18JumlahPohon / normaHanca;
                    m.T19LuasJelajah = m.T19JumlahPohon / normaHanca;
                    m.T20LuasJelajah = m.T20JumlahPohon / normaHanca;
                    m.T21LuasJelajah = m.T21JumlahPohon / normaHanca;
                    m.T22LuasJelajah = m.T22JumlahPohon / normaHanca;
                    m.T23LuasJelajah = m.T23JumlahPohon / normaHanca;
                    m.T24LuasJelajah = m.T24JumlahPohon / normaHanca;
                    m.T25LuasJelajah = m.T25JumlahPohon / normaHanca;
                    m.T26LuasJelajah = m.T26JumlahPohon / normaHanca;
                    m.T27LuasJelajah = m.T27JumlahPohon / normaHanca;
                    m.T28LuasJelajah = m.T28JumlahPohon / normaHanca;
                    m.T29LuasJelajah = m.T29JumlahPohon / normaHanca;
                    m.T30LuasJelajah = m.T30JumlahPohon / normaHanca;
                    m.T31LuasJelajah = m.T31JumlahPohon / normaHanca;
                    blok.Add(m);
                }
            }

            return Json(blok, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult HancaPerBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 4) == Id)
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun) - int.Parse(m.TahunTanam) equals n.Umur
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam, n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM",g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                            T01JumlahPohon = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon),
                            T01KgLateks = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgLateks + 0),
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02JumlahPohon = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon),
                            T02KgLateks = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLateks + 0),
                            T02KgLump = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgLump + 0),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03JumlahPohon = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon),
                            T03KgLateks = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLateks + 0),
                            T03KgLump = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgLump + 0),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04JumlahPohon = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon),
                            T04KgLateks = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLateks + 0),
                            T04KgLump = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgLump + 0),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05JumlahPohon = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon),
                            T05KgLateks = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLateks + 0),
                            T05KgLump = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgLump + 0),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06JumlahPohon = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon),
                            T06KgLateks = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLateks + 0),
                            T06KgLump = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgLump + 0),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07JumlahPohon = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon),
                            T07KgLateks = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLateks + 0),
                            T07KgLump = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgLump + 0),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08JumlahPohon = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon),
                            T08KgLateks = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLateks + 0),
                            T08KgLump = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgLump + 0),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09JumlahPohon = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon),
                            T09KgLateks = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLateks + 0),
                            T09KgLump = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgLump + 0),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10JumlahPohon = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon),
                            T10KgLateks = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLateks + 0),
                            T10KgLump = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgLump + 0),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11JumlahPohon = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon),
                            T11KgLateks = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLateks + 0),
                            T11KgLump = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgLump + 0),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12JumlahPohon = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon),
                            T12KgLateks = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLateks + 0),
                            T12KgLump = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgLump + 0),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T13JumlahPohon = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon),
                            T13KgLateks = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLateks + 0),
                            T13KgLump = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgLump + 0),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T14JumlahPohon = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon),
                            T14KgLateks = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLateks + 0),
                            T14KgLump = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgLump + 0),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T15JumlahPohon = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon),
                            T15KgLateks = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLateks + 0),
                            T15KgLump = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgLump + 0),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T16JumlahPohon = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon),
                            T16KgLateks = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLateks + 0),
                            T16KgLump = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgLump + 0),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T17JumlahPohon = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon),
                            T17KgLateks = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLateks + 0),
                            T17KgLump = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgLump + 0),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T18JumlahPohon = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon),
                            T18KgLateks = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLateks + 0),
                            T18KgLump = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgLump + 0),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T19JumlahPohon = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon),
                            T19KgLateks = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLateks + 0),
                            T19KgLump = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgLump + 0),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T20JumlahPohon = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon),
                            T20KgLateks = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLateks + 0),
                            T20KgLump = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgLump + 0),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T21JumlahPohon = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon),
                            T21KgLateks = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLateks + 0),
                            T21KgLump = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgLump + 0),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T22JumlahPohon = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon),
                            T22KgLateks = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLateks + 0),
                            T22KgLump = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgLump + 0),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T23JumlahPohon = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon),
                            T23KgLateks = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLateks + 0),
                            T23KgLump = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgLump + 0),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T24JumlahPohon = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon),
                            T24KgLateks = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLateks + 0),
                            T24KgLump = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgLump + 0),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T25JumlahPohon = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon),
                            T25KgLateks = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLateks + 0),
                            T25KgLump = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgLump + 0),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T26JumlahPohon = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon),
                            T26KgLateks = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLateks + 0),
                            T26KgLump = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgLump + 0),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T27JumlahPohon = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon),
                            T27KgLateks = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLateks + 0),
                            T27KgLump = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgLump + 0),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T28JumlahPohon = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon),
                            T28KgLateks = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLateks + 0),
                            T28KgLump = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgLump + 0),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T29JumlahPohon = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon),
                            T29KgLateks = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLateks + 0),
                            T29KgLump = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgLump + 0),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahPohon = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon),
                            T30KgLateks = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLateks + 0),
                            T30KgLump = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgLump + 0),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahPohon = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon),
                            T31KgLateks = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLateks + 0),
                            T31KgLump = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgLump + 0),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            decimal jumlahPohon = 0;
            decimal jumlahHK = 0;
            decimal jumlahHanca = 0;
            decimal normaHanca = 0;
            decimal normaRotasi = 0;

            foreach (var o in model)
            {
                jumlahPohon = o.T01JumlahPohon + o.T02JumlahPohon + o.T03JumlahPohon + o.T04JumlahPohon + o.T05JumlahPohon +
                    o.T06JumlahPohon + o.T07JumlahPohon + o.T08JumlahPohon + o.T09JumlahPohon + o.T10JumlahPohon +
                    o.T11JumlahPohon + o.T12JumlahPohon + o.T13JumlahPohon + o.T14JumlahPohon + o.T15JumlahPohon +
                    o.T16JumlahPohon + o.T17JumlahPohon + o.T18JumlahPohon + o.T19JumlahPohon + o.T20JumlahPohon +
                    o.T21JumlahPohon + o.T22JumlahPohon + o.T23JumlahPohon + o.T24JumlahPohon + o.T25JumlahPohon +
                    o.T26JumlahPohon + o.T27JumlahPohon + o.T28JumlahPohon + o.T29JumlahPohon + o.T30JumlahPohon + o.T31JumlahPohon;

                jumlahHK = o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                    o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                    o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                    o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                    o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                    o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK;


                normaHanca = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ?
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).PohonDisadapPerHanca : 0);

                normaRotasi = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)) != null ?
                    CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(m => m.Umur == DateTime.Now.Year - int.Parse(o.TahunTanam)).Rotasi / 12 : 0);

                if (normaHanca > 0) jumlahHanca = jumlahPohon / normaHanca;

                if ((int)jumlahHanca == 0)
                    o.Warna = "black";
                if (jumlahHanca <= jumlahHK * 70 / 100)
                    o.Warna = "red";
                else if (jumlahHanca <= jumlahHK * 80 / 100)
                    o.Warna = "orange";
                else if (jumlahHanca <= jumlahHK * 90 / 100)
                    o.Warna = "yellow";
                else if (jumlahHanca <= jumlahHK)
                    o.Warna = "green";
                else
                    o.Warna = "blue";

                blok.Add(o);
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

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodekebun + o.kodeafdeling == Id).Where(o => o.kodebudidaya == "00" && o.status == "TM")
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
                decimal jumlahPohon = 0;
                decimal jumlahHK = 0;
                decimal jumlahHanca = 0;
                decimal normaHanca = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;

                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaHanca = normaHanca + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ?
                            CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).Rotasi / 12 : 0);
                }

                jumlahPohon = CRUDHasilPanen.CRUD.GetHasilPanenPerBlok()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahPohon);
                jumlahHK = CRUDHasilPanen.CRUD.GetHasilPanenPerBlok()
                    .Where(o => o.Bulan == int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK);

                if (strTahunTanam.Length > 0)
                {
                    normaHanca = normaHanca / strTahunTanam.Length;
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                    jumlahHanca = jumlahPohon / normaHanca;
                }
                else
                {
                    normaHanca = 0;
                    normaRotasi = 0;
                    jumlahHanca = 0;
                }


                m.Value1 = (double)jumlahHanca;
                m.Value2 = (double)jumlahHK;

                if (warna == "")
                {
                    if ((int)jumlahHanca == 0)
                        m.Warna = "black";
                    else if (jumlahHanca <= jumlahHK * 70 / 100)
                        m.Warna = "red";
                    else if (jumlahHanca <= jumlahHK * 80 / 100)
                        m.Warna = "orange";
                    else if (jumlahHanca <= jumlahHK * 90 / 100)
                        m.Warna = "yellow";
                    else if (jumlahHanca <= jumlahHK)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM"));
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