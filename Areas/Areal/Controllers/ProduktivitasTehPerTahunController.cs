using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Areal.Models;
using Ptpn8.Areas.Areal.Models.CRUD;
using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Omu.ValueInjecter;
using Ptpn8.Models.CRUD;
using Ptpn8.Models;

namespace Ptpn8.Areas.Areal.Controllers
{
    public class ProduktivitasTehPerTahunController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasTeh
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Produktivitas";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            if (HttpContext.Session["BulanView"] == null)
            {
                SetSessionValue(DateTime.Now.Month.ToString(),
                    DateTime.Now.Year.ToString(), "Kebun", "", "");
            }
            HttpContext.Session["ListHasilPanenPerBlokPer6Bulan"] = null;
            HttpContext.Session["ListHasilPanenPerAfdelingPer6Bulan"] = null;
            HttpContext.Session["ListHasilPanenPerKebunPer6Bulan"] = null;
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult ChartProtasPerKebun(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);


            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPer6Bulan().Where(o => o.KodeBudidaya == "02")
                         group m by new { m.KodeBlok } into g
                         select new ProduktivitasPerKebun
                         {
                             Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                             Kebun = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                             Wilayah = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                             Budidaya = "Teh",
                             LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),

                             Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),
                             Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "02", "TM"),

                             Januari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 1).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Februari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 2).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Maret_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 3).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             April_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 4).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Mei_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 5).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Juni_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 6).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Juli_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 7).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Agustus_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 8).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             September_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 9).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Oktober_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 10).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             November_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 11).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                             Desember_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun(CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran).Where(o => o.KodeBudidaya == "02" && o.Bulan == 12).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Dataran),
                         }).ToList();



            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ProtasPerKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPer6Bulan().Where(o => o.KodeBudidaya == "02")
                          where (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                          userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                          group m by new { g1 = m.KodeBlok, g2 = m.TahunTanam } into g
                          select new ProduktivitasPerKebun
                          {
                              Id = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                              Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                              TahunTanam = g.Key.g2,
                              Wilayah = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                              Budidaya = "Teh",
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1, "02", "TM", g.Key.g2),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("02", "TM", g.Key.g2, CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Dataran),
                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),
                          }).ToList();

            List<ProduktivitasPerKebun> model2 = new List<ProduktivitasPerKebun>();
            model2.AddRange(model1);

            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;

            List<ProduktivitasPerKebun> kebun = new List<ProduktivitasPerKebun>();
            foreach (var m in model1)
            {
                var dataran = CRUDKebun.CRUD.Get1Record(m.Id).Dataran;
                var listDataran = (from x in model2.Where(o => o.TahunTanam == m.TahunTanam)
                                   join y in CRUDKebun.CRUD.GetAllRecord() on x.Id equals y.KebunId
                                   where y.Dataran == dataran select x);
                m.Januari_PTPN8 = listDataran.Sum(o => o.Januari_Real);
                m.Februari_PTPN8 = listDataran.Sum(o => o.Februari_Real);
                m.Maret_PTPN8 = listDataran.Sum(o => o.Maret_Real);
                m.April_PTPN8 = listDataran.Sum(o => o.April_Real);
                m.Mei_PTPN8 = listDataran.Sum(o => o.Mei_Real);
                m.Juni_PTPN8 = listDataran.Sum(o => o.Juni_Real);
                m.Juli_PTPN8 = listDataran.Sum(o => o.Juli_Real);
                m.Agustus_PTPN8 = listDataran.Sum(o => o.Agustus_Real);
                m.September_PTPN8 = listDataran.Sum(o => o.September_Real);
                m.Oktober_PTPN8 = listDataran.Sum(o => o.Oktober_Real);
                m.November_PTPN8 = listDataran.Sum(o => o.November_Real);
                m.Desember_PTPN8 = listDataran.Sum(o => o.Desember_Real);

                m.MATime_Sem1 = (m.Januari_Real / (double)m.LuasAreal - m.Januari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Februari_Real / (double)m.LuasAreal - m.Februari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Maret_Real / (double)m.LuasAreal - m.Maret_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.April_Real / (double)m.LuasAreal - m.April_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Mei_Real / (double)m.LuasAreal - m.Mei_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juni_Real / (double)m.LuasAreal - m.Juni_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juli_Real / (double)m.LuasAreal - m.Juli_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Agustus_Real / (double)m.LuasAreal - m.Agustus_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.September_Real / (double)m.LuasAreal - m.September_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Oktober_Real / (double)m.LuasAreal - m.Oktober_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.November_Real / (double)m.LuasAreal - m.November_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Desember_Real / (double)m.LuasAreal - m.Desember_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Januari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Februari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Maret_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.April_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Mei_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juni_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juli_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Agustus_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.September_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Oktober_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.November_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Desember_PTPN8 / (double)m.LuasArealInduk) : 0);


                if (m.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        m.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        m.MABelow_Sem1 = 0;
                }
                else
                {
                    m.MABelow_Sem1 = 0;
                }

                m.Warna_Sem1 = SetWarna(m.MABelow_Sem1, m.MATime_Sem1);
                kebun.Add(m);
            }
            return Json(kebun.OrderBy(o => o.Kebun).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult KebunGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebunPer6Bulan().Where(o => o.KodeBudidaya == "02")
                          where (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                          userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                          group m by new { g1 = m.KodeBlok } into g
                          select new ProduktivitasPerKebun
                          {
                              Id = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                              Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                              Wilayah = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                              Budidaya = "Teh",
                              TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1, "02", "TM"),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealPTPN8PerDataran("02", "TM", CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Dataran),

                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),

                          }).ToList();

            List<ProduktivitasPerKebun> model2 = new List<ProduktivitasPerKebun>();
            model2.AddRange(model1);

            List<PetaView> kebun = new List<PetaView>();
            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;
            foreach (var mdl in model1)
            {
                var dataran = CRUDKebun.CRUD.Get1Record(mdl.Id).Dataran;
                var listDataran = (from x in model2 join y in CRUDKebun.CRUD.GetAllRecord() on x.Id equals y.KebunId
                                   where y.Dataran == dataran select x);

                mdl.Januari_PTPN8 = listDataran.Sum(o => o.Januari_Real);
                mdl.Februari_PTPN8 = listDataran.Sum(o => o.Februari_Real);
                mdl.Maret_PTPN8 = listDataran.Sum(o => o.Maret_Real);
                mdl.April_PTPN8 = listDataran.Sum(o => o.April_Real);
                mdl.Mei_PTPN8 = listDataran.Sum(o => o.Mei_Real);
                mdl.Juni_PTPN8 = listDataran.Sum(o => o.Juni_Real);
                mdl.Juli_PTPN8 = listDataran.Sum(o => o.Juli_Real);
                mdl.Agustus_PTPN8 = listDataran.Sum(o => o.Agustus_Real);
                mdl.September_PTPN8 = listDataran.Sum(o => o.September_Real);
                mdl.Oktober_PTPN8 = listDataran.Sum(o => o.Oktober_Real);
                mdl.November_PTPN8 = listDataran.Sum(o => o.November_Real);
                mdl.Desember_PTPN8 = listDataran.Sum(o => o.Desember_Real);

                mdl.MATime_Sem1 = (mdl.Januari_Real / (double)mdl.LuasAreal - mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Februari_Real / (double)mdl.LuasAreal - mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Maret_Real / (double)mdl.LuasAreal - mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.April_Real / (double)mdl.LuasAreal - mdl.April_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Mei_Real / (double)mdl.LuasAreal - mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juni_Real / (double)mdl.LuasAreal - mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juli_Real / (double)mdl.LuasAreal - mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Agustus_Real / (double)mdl.LuasAreal - mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.September_Real / (double)mdl.LuasAreal - mdl.September_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Oktober_Real / (double)mdl.LuasAreal - mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.November_Real / (double)mdl.LuasAreal - mdl.November_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Desember_Real / (double)mdl.LuasAreal - mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) : 0);


                if (mdl.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        mdl.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        mdl.MABelow_Sem1 = 0;
                }
                else
                {
                    mdl.MABelow_Sem1 = 0;
                }

                var kbn = new PetaView();
                var kd_kbn = CRUDKebun.CRUD.Get1Record(mdl.Id) == null ? "" : CRUDKebun.CRUD.Get1Record(mdl.Id).kd_kbn;
                kbn.Id = mdl.Id;
                kbn.KodeKebun = kd_kbn;
                kbn.Peta = CRUDKebunPeta.CRUD.Get1Record(kd_kbn) == null ? "" : CRUDKebunPeta.CRUD.Get1Record(kd_kbn).Peta != null ? CRUDKebunPeta.CRUD.Get1Record(kd_kbn).Peta.ToString() : "";
                kbn.Nama = mdl.Kebun;
                kbn.TahunTanam = mdl.TahunTanam;
                kbn.Value1 = mdl.MATime_Sem1;
                kbn.Value2 = mdl.MABelow_Sem1;
                kbn.Value3 = 0;
                kbn.Warna = SetWarna(mdl.MABelow_Sem1, mdl.MATime_Sem1);
                kebun.Add(kbn);
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
                    if (warna == "") feature.Attributes.AddAttribute("COLOR", k.Warna); else feature.Attributes.AddAttribute("COLOR", warna);
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
        public ActionResult ChartProtasPerAfdeling(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 2) == Id)
                         group m by new { m.KodeBlok } into g
                         select new ProduktivitasPerAfdeling
                         {
                             Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                             Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                             Kebun = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                             Budidaya = "Teh",
                             LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),

                             Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),
                             Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "02", "TM"),

                             Januari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 1 && o.KodeBlok == g.Key.KodeBlok.Substring(0,2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Februari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 2 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Maret_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 3 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             April_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 4 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Mei_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 5 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Juni_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 6 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni(g.Key.KodeBlok.Substring(0, 2), "02", 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Juli_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 7 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni(g.Key.KodeBlok.Substring(0, 2), "02", 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Agustus_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 8 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             September_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 9 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Oktober_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 10 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             November_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 11 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM"),
                             Desember_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "02" && o.Bulan == 12 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "02", "TM")
                         }).ToList();


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ProtasPerAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 2) == Id)
                          group m by new { g1 = m.KodeBlok, g2 = m.TahunTanam } into g
                          select new ProduktivitasPerAfdeling
                          {
                              Id = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).AfdelingId,
                              Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                              Kebun = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Kebun.Nama,
                              TahunTanam = g.Key.g2,
                              Budidaya = "Teh",
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.g1, "02", "TM", g.Key.g2),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1.Substring(0, 2), "02", "TM", g.Key.g2),

                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),
                          }).ToList();

            List<ProduktivitasPerAfdeling> model2 = new List<ProduktivitasPerAfdeling>();
            model2.AddRange(model1);

            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;
            List<ProduktivitasPerAfdeling> afdeling = new List<ProduktivitasPerAfdeling>();
            foreach (var m in model1)
            {

                m.Januari_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Januari_Real);
                m.Februari_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Februari_Real);
                m.Maret_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Maret_Real);
                m.April_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.April_Real);
                m.Mei_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Mei_Real);
                m.Juni_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Juni_Real);
                m.Juli_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Juli_Real);
                m.Agustus_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Agustus_Real);
                m.September_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.September_Real);
                m.Oktober_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Oktober_Real);
                m.November_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.November_Real);
                m.Desember_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Desember_Real);

                m.MATime_Sem1 = (m.Januari_Real / (double)m.LuasAreal - m.Januari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Februari_Real / (double)m.LuasAreal - m.Februari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Maret_Real / (double)m.LuasAreal - m.Maret_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.April_Real / (double)m.LuasAreal - m.April_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Mei_Real / (double)m.LuasAreal - m.Mei_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juni_Real / (double)m.LuasAreal - m.Juni_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juli_Real / (double)m.LuasAreal - m.Juli_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Agustus_Real / (double)m.LuasAreal - m.Agustus_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.September_Real / (double)m.LuasAreal - m.September_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Oktober_Real / (double)m.LuasAreal - m.Oktober_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.November_Real / (double)m.LuasAreal - m.November_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Desember_Real / (double)m.LuasAreal - m.Desember_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Januari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Februari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Maret_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.April_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Mei_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juni_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juli_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Agustus_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.September_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Oktober_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.November_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Desember_PTPN8 / (double)m.LuasArealInduk) : 0);


                if (m.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        m.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        m.MABelow_Sem1 = 0;
                }
                else
                {
                    m.MABelow_Sem1 = 0;
                }

                m.Warna_Sem1 = SetWarna(m.MABelow_Sem1, m.MATime_Sem1);
                afdeling.Add(m);
            }
            return Json(afdeling.OrderBy(o => o.Afdeling).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult AfdelingGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdelingPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 2) == Id)
                          group m by new { g1 = m.KodeBlok } into g
                          select new ProduktivitasPerAfdeling
                          {
                              Id = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).AfdelingId,
                              Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                              Kebun = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Kebun.Nama,
                              Budidaya = "Teh",
                              TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.g1, "02", "TM"),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1.Substring(0, 2), "02", "TM"),

                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),
                          }).ToList();

            List<ProduktivitasPerAfdeling> model2 = new List<ProduktivitasPerAfdeling>();
            model2.AddRange(model1);

            List<PetaView> afdeling = new List<PetaView>();
            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;
            foreach (var mdl in model1)
            {
                mdl.Januari_PTPN8 = model2.Sum(o => o.Januari_Real);
                mdl.Februari_PTPN8 = model2.Sum(o => o.Februari_Real);
                mdl.Maret_PTPN8 = model2.Sum(o => o.Maret_Real);
                mdl.April_PTPN8 = model2.Sum(o => o.April_Real);
                mdl.Mei_PTPN8 = model2.Sum(o => o.Mei_Real);
                mdl.Juni_PTPN8 = model2.Sum(o => o.Juni_Real);
                mdl.Juli_PTPN8 = model2.Sum(o => o.Juli_Real);
                mdl.Agustus_PTPN8 = model2.Sum(o => o.Agustus_Real);
                mdl.September_PTPN8 = model2.Sum(o => o.September_Real);
                mdl.Oktober_PTPN8 = model2.Sum(o => o.Oktober_Real);
                mdl.November_PTPN8 = model2.Sum(o => o.November_Real);
                mdl.Desember_PTPN8 = model2.Sum(o => o.Desember_Real);

                mdl.MATime_Sem1 = (mdl.Januari_Real / (double)mdl.LuasAreal - mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Februari_Real / (double)mdl.LuasAreal - mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Maret_Real / (double)mdl.LuasAreal - mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.April_Real / (double)mdl.LuasAreal - mdl.April_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Mei_Real / (double)mdl.LuasAreal - mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juni_Real / (double)mdl.LuasAreal - mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juli_Real / (double)mdl.LuasAreal - mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Agustus_Real / (double)mdl.LuasAreal - mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.September_Real / (double)mdl.LuasAreal - mdl.September_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Oktober_Real / (double)mdl.LuasAreal - mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.November_Real / (double)mdl.LuasAreal - mdl.November_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Desember_Real / (double)mdl.LuasAreal - mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) : 0);


                if (mdl.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        mdl.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        mdl.MABelow_Sem1 = 0;
                }
                else
                {
                    mdl.MABelow_Sem1 = 0;
                }

                var afd = new PetaView();
                var kd_afd = CRUDAfdeling.CRUD.Get1Record(mdl.Id) == null ? "" : CRUDAfdeling.CRUD.Get1Record(mdl.Id).kd_afd;
                afd.Id = mdl.Id;
                afd.KodeKebun = kd_afd;
                afd.Peta = CRUDAfdelingPeta.CRUD.Get1Record(kd_afd) == null ? "" : CRUDAfdelingPeta.CRUD.Get1Record(kd_afd).Peta != null ? CRUDAfdelingPeta.CRUD.Get1Record(kd_afd).Peta.ToString() : "";
                afd.Nama = mdl.Afdeling;
                afd.TahunTanam = mdl.TahunTanam;
                afd.Value1 = mdl.MATime_Sem1;
                afd.Value2 = mdl.MABelow_Sem1;
                afd.Value3 = 0;
                afd.Warna = SetWarna(mdl.MABelow_Sem1, mdl.MATime_Sem1);
                afdeling.Add(afd);
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
                    if (warna == "") feature.Attributes.AddAttribute("COLOR", k.Warna); else feature.Attributes.AddAttribute("COLOR", warna);
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
        public ActionResult ChartProtasPerBlok(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 4) == Id)
                         group m by new { m.KodeBlok } into g
                         select new ProduktivitasPerBlok
                         {
                             Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                             Blok = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "02", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "02", "TM").namablok,
                             Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)).Nama,

                             TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                             Budidaya = "Teh",
                             LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),

                             Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),
                             Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.KodeBlok, 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "02", "TM"),

                             Januari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 1 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 1, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Februari_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 2 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 2, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Maret_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 3 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 3, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             April_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 4 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 4, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Mei_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 5 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 5, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Juni_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 6 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni(g.Key.KodeBlok.Substring(0, 2), "02", 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 6, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Juli_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 7 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni(g.Key.KodeBlok.Substring(0, 2), "02", 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 7, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Agustus_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 8 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 8, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             September_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 9 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 9, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Oktober_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 10 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 10, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             November_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 11 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 11, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM"),
                             Desember_PTPN8 = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "02" && o.Bulan == 12 && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgPucuk)) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", 12, int.Parse(tahun) - 1))
                                / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "02", "TM")
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult ProtasPerBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 4) == Id)
                          group m by new { g1 = m.KodeBlok, g2 = m.TahunTanam } into g
                          select new ProduktivitasPerBlok
                          {
                              Id = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                              Blok = CRUDRef_Areal.CRUD.Get1Record(g.Key.g1, "02", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.g1, "02", "TM").namablok,
                              Afdeling = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlok.CRUD.Get1Record(g.Key.g1).Afdeling.Nama,
                              TahunTanam = g.Key.g2,
                              Budidaya = "Teh",
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.g1, "02", "TM", g.Key.g2),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.g1.Substring(0, 4), "02", "TM", g.Key.g2),

                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),
                          }).ToList();

            List<ProduktivitasPerBlok> model2 = new List<ProduktivitasPerBlok>();
            model2.AddRange(model1);

            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;
            List<ProduktivitasPerBlok> blok = new List<ProduktivitasPerBlok>();
            foreach (var m in model1)
            {
                m.Januari_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Januari_Real);
                m.Februari_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Februari_Real);
                m.Maret_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Maret_Real);
                m.April_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.April_Real);
                m.Mei_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Mei_Real);
                m.Juni_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Juni_Real);
                m.Juli_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Juli_Real);
                m.Agustus_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Agustus_Real);
                m.September_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.September_Real);
                m.Oktober_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Oktober_Real);
                m.November_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.November_Real);
                m.Desember_PTPN8 = model2.Where(o => o.TahunTanam == m.TahunTanam).Sum(o => o.Desember_Real);

                m.MATime_Sem1 = (m.Januari_Real / (double)m.LuasAreal - m.Januari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Februari_Real / (double)m.LuasAreal - m.Februari_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Maret_Real / (double)m.LuasAreal - m.Maret_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.April_Real / (double)m.LuasAreal - m.April_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Mei_Real / (double)m.LuasAreal - m.Mei_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juni_Real / (double)m.LuasAreal - m.Juni_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Juli_Real / (double)m.LuasAreal - m.Juli_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Agustus_Real / (double)m.LuasAreal - m.Agustus_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.September_Real / (double)m.LuasAreal - m.September_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Oktober_Real / (double)m.LuasAreal - m.Oktober_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.November_Real / (double)m.LuasAreal - m.November_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0) +
                    (m.Desember_Real / (double)m.LuasAreal - m.Desember_PTPN8 / (double)m.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((m.Januari_Real / (double)m.LuasAreal) - (m.Januari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Januari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Februari_Real / (double)m.LuasAreal) - (m.Februari_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Februari_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Maret_Real / (double)m.LuasAreal) - (m.Maret_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Maret_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.April_Real / (double)m.LuasAreal) - (m.April_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.April_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Mei_Real / (double)m.LuasAreal) - (m.Mei_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Mei_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juni_Real / (double)m.LuasAreal) - (m.Juni_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juni_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Juli_Real / (double)m.LuasAreal) - (m.Juli_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Juli_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Agustus_Real / (double)m.LuasAreal) - (m.Agustus_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Agustus_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.September_Real / (double)m.LuasAreal) - (m.September_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.September_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Oktober_Real / (double)m.LuasAreal) - (m.Oktober_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Oktober_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.November_Real / (double)m.LuasAreal) - (m.November_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.November_PTPN8 / (double)m.LuasArealInduk) : 0) +
                    ((m.Desember_Real / (double)m.LuasAreal) - (m.Desember_PTPN8 / (double)m.LuasArealInduk) < 0 ? (m.Desember_PTPN8 / (double)m.LuasArealInduk) : 0);


                if (m.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        m.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        m.MABelow_Sem1 = 0;
                }
                else
                {
                    m.MABelow_Sem1 = 0;
                }

                m.Warna_Sem1 = SetWarna(m.MABelow_Sem1, m.MATime_Sem1);
                blok.Add(m);
            }
            return Json(blok.OrderBy(o => o.Blok).ToDataSourceResult(request));
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

            var model1 = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPer6Bulan().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 4) == Id)
                          group m by new { g1 = m.KodeBlok } into g
                          select new ProduktivitasPerBlok
                          {
                              Id = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                              Blok = CRUDRef_Areal.CRUD.Get1Record(g.Key.g1, "02", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.g1, "02", "TM").namablok,
                              Afdeling = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlok.CRUD.Get1Record(g.Key.g1).Afdeling.Nama,
                              Budidaya = "Teh",
                              TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                              LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.g1, "02", "TM"),
                              LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.g1.Substring(0, 4), "02", "TM"),

                              Januari_Real = (double)g.Where(o => o.Bulan == 1).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 1, int.Parse(tahun) - 1)),
                              Februari_Real = (double)g.Where(o => o.Bulan == 2).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 2, int.Parse(tahun) - 1)),
                              Maret_Real = (double)g.Where(o => o.Bulan == 3).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 3, int.Parse(tahun) - 1)),
                              April_Real = (double)g.Where(o => o.Bulan == 4).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 4, int.Parse(tahun) - 1)),
                              Mei_Real = (double)g.Where(o => o.Bulan == 5).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 5, int.Parse(tahun) - 1)),
                              Juni_Real = (double)g.Where(o => o.Bulan == 6).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 6, int.Parse(tahun) - 1)),
                              Juli_Real = (double)g.Where(o => o.Bulan == 7).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 7, int.Parse(tahun) - 1)),
                              Agustus_Real = (double)g.Where(o => o.Bulan == 8).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 8, int.Parse(tahun) - 1)),
                              September_Real = (double)g.Where(o => o.Bulan == 9).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 9, int.Parse(tahun) - 1)),
                              Oktober_Real = (double)g.Where(o => o.Bulan == 10).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 10, int.Parse(tahun) - 1)),
                              November_Real = (double)g.Where(o => o.Bulan == 11).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 11, int.Parse(tahun) - 1)),
                              Desember_Real = (double)g.Where(o => o.Bulan == 12).Sum(o => o.KgPucuk) *
                                (CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) > 0 ? CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun)) : CRUDAkunProduksiHP.CRUD.RealisasiRasioBasahKeringBulanIni("02", g.Key.g1, 12, int.Parse(tahun) - 1)),
                          }).ToList();

            List<ProduktivitasPerBlok> model2 = new List<ProduktivitasPerBlok>();
            model2.AddRange(model1);

            List<PetaView> blok = new List<PetaView>();
            double? jmlSelisih1 = 0;
            double? jmlSelisih2 = 0;
            foreach (var mdl in model1)
            {
                mdl.Januari_PTPN8 = model2.Sum(o => o.Januari_Real);
                mdl.Februari_PTPN8 = model2.Sum(o => o.Februari_Real);
                mdl.Maret_PTPN8 = model2.Sum(o => o.Maret_Real);
                mdl.April_PTPN8 = model2.Sum(o => o.April_Real);
                mdl.Mei_PTPN8 = model2.Sum(o => o.Mei_Real);
                mdl.Juni_PTPN8 = model2.Sum(o => o.Juni_Real);
                mdl.Juli_PTPN8 = model2.Sum(o => o.Juli_Real);
                mdl.Agustus_PTPN8 = model2.Sum(o => o.Agustus_Real);
                mdl.September_PTPN8 = model2.Sum(o => o.September_Real);
                mdl.Oktober_PTPN8 = model2.Sum(o => o.Oktober_Real);
                mdl.November_PTPN8 = model2.Sum(o => o.November_Real);
                mdl.Desember_PTPN8 = model2.Sum(o => o.Desember_Real);

                mdl.MATime_Sem1 = (mdl.Januari_Real / (double)mdl.LuasAreal - mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Februari_Real / (double)mdl.LuasAreal - mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Maret_Real / (double)mdl.LuasAreal - mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.April_Real / (double)mdl.LuasAreal - mdl.April_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Mei_Real / (double)mdl.LuasAreal - mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juni_Real / (double)mdl.LuasAreal - mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Juli_Real / (double)mdl.LuasAreal - mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Agustus_Real / (double)mdl.LuasAreal - mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.September_Real / (double)mdl.LuasAreal - mdl.September_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Oktober_Real / (double)mdl.LuasAreal - mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.November_Real / (double)mdl.LuasAreal - mdl.November_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0) +
                    (mdl.Desember_Real / (double)mdl.LuasAreal - mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk < 0 ? 1 : 0);

                jmlSelisih1 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk)) * -1 : 0);

                jmlSelisih2 = ((mdl.Januari_Real / (double)mdl.LuasAreal) - (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Januari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Februari_Real / (double)mdl.LuasAreal) - (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Februari_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Maret_Real / (double)mdl.LuasAreal) - (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Maret_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.April_Real / (double)mdl.LuasAreal) - (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.April_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Mei_Real / (double)mdl.LuasAreal) - (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Mei_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juni_Real / (double)mdl.LuasAreal) - (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juni_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Juli_Real / (double)mdl.LuasAreal) - (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Juli_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Agustus_Real / (double)mdl.LuasAreal) - (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Agustus_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.September_Real / (double)mdl.LuasAreal) - (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.September_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Oktober_Real / (double)mdl.LuasAreal) - (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Oktober_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.November_Real / (double)mdl.LuasAreal) - (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.November_PTPN8 / (double)mdl.LuasArealInduk) : 0) +
                    ((mdl.Desember_Real / (double)mdl.LuasAreal) - (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) < 0 ? (mdl.Desember_PTPN8 / (double)mdl.LuasArealInduk) : 0);


                if (mdl.MATime_Sem1 > 0)
                {
                    if (jmlSelisih2 > 0)
                        mdl.MABelow_Sem1 = (jmlSelisih1 / jmlSelisih2);
                    else
                        mdl.MABelow_Sem1 = 0;
                }
                else
                {
                    mdl.MABelow_Sem1 = 0;
                }

                var blk = new PetaView();
                var kd_blok = CRUDBlok.CRUD.Get1Record(mdl.Id) == null ? "" : CRUDBlok.CRUD.Get1Record(mdl.Id).kd_blok;
                blk.Id = mdl.Id;
                blk.KodeKebun = kd_blok;
                blk.Peta = CRUDBlokRealisasi.CRUD.Get1Record(kd_blok) == null ? "" : CRUDBlokRealisasi.CRUD.Get1Record(kd_blok).Peta != null ? CRUDBlokRealisasi.CRUD.Get1Record(kd_blok).Peta.ToString() : "";
                blk.Nama = mdl.Blok;
                blk.TahunTanam = mdl.TahunTanam;
                blk.Value1 = mdl.MATime_Sem1;
                blk.Value2 = mdl.MABelow_Sem1;
                blk.Value3 = 0;
                blk.Warna = SetWarna(mdl.MABelow_Sem1, mdl.MATime_Sem1);
                blok.Add(blk);
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
                    if (warna == "") feature.Attributes.AddAttribute("COLOR", k.Warna); else feature.Attributes.AddAttribute("COLOR", warna);
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

        public string SetWarna(double? maBelow, double? maTime)
        {
            string result = "";
            switch ((int)maTime)
            {
                case 0:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "green";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "yellow";
                    }
                    else
                    {
                        result = "orange";
                    }
                    break;

                case 1:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "green";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "yellow";
                    }
                    else
                    {
                        result = "orange";
                    }
                    break;

                case 2:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "green";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "yellow";
                    }
                    else
                    {
                        result = "orange";
                    }
                    break;

                case 3:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "green";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "yellow";
                    }
                    else
                    {
                        result = "orange";
                    }
                    break;

                case 4:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "yellow";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "orange";
                    }
                    else
                    {
                        result = "red";
                    }

                    break;
                case 5:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "yellow";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "orange";
                    }
                    else
                    {
                        result = "red";
                    }

                    break;
                case 6:
                    if (maBelow >= 0 && maBelow < 0.1)
                    {
                        result = "yellow";
                    }
                    else if (maBelow >= 0.1 && maBelow < 0.3)
                    {
                        result = "orange";
                    }
                    else
                    {
                        result = "red";
                    }

                    break;
            }
            return result;

        }

        [HttpPost]
        public bool SetSessionValue(string bulan = "", string tahun = "", string layer = "", string lokasiKebun = "", string lokasiAfdeling = "")

        {
            if (bulan.Length > 0)
            {
                HttpContext.Session["BulanView"] = bulan;
            }

            if (tahun.Length > 0)
            {
                HttpContext.Session["TahunView"] = tahun;
            }

            if (layer.Length > 0)
            {
                HttpContext.Session["ActiveLayer"] = layer;
            }

            if (lokasiKebun.Length > 0)
            {
                HttpContext.Session["KebunId"] = lokasiKebun;
            }

            if (lokasiAfdeling.Length > 0)
            {
                HttpContext.Session["AfdelingId"] = lokasiAfdeling;
            }

            HttpContext.Session["ListHasilPanenPerKebunPer6Bulan"] = null;
            HttpContext.Session["ListHasilPanenPerAfdelingPer6Bulan"] = null;
            HttpContext.Session["ListHasilPanenPerBlokPer6Bulan"] = null;

            return true; // Return the session value
        }
    }
}