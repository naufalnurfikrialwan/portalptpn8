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
    public class TenagaKerjaPanenKaretController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasKaret
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Tenaga Kerja Panen";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult ChartTenagaKerjaPerKebun(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00")
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                                Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                                Tahun = int.Parse(tahun),
                                KodeLokasi = g.Key.KodeBlok,
                                NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                                NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                                KodeBudidaya = "00",
                                NamaBudidaya = "Karet",
                                TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                                LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                                JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                                 T01JumlahPohon = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon),
                                 T02JumlahPohon = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon),
                                 T03JumlahPohon = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon),
                                 T04JumlahPohon = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon),
                                 T05JumlahPohon = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon),
                                 T06JumlahPohon = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon),
                                 T07JumlahPohon = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon),
                                 T08JumlahPohon = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon),
                                 T09JumlahPohon = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon),
                                 T10JumlahPohon = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon),
                                 T11JumlahPohon = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon),
                                 T12JumlahPohon = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon),
                                 T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                                 T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                                 T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                                 T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                                 T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                                 T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                                 T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                                 T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                                 T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                                 T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                                 T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                                 T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                         }).ToList();

            List<RotasiPanenPerTahun> kebun = new List<RotasiPanenPerTahun>();
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
                    m.T01LuasJelajah = m.T01JumlahHK > 0 ? (m.T01JumlahPohon / normaHanca) / m.T01JumlahHK : 0;
                    m.T02LuasJelajah = m.T02JumlahHK > 0 ? (m.T02JumlahPohon / normaHanca) / m.T02JumlahHK : 0;
                    m.T03LuasJelajah = m.T03JumlahHK > 0 ? (m.T03JumlahPohon / normaHanca) / m.T03JumlahHK : 0;
                    m.T04LuasJelajah = m.T04JumlahHK > 0 ? (m.T04JumlahPohon / normaHanca) / m.T04JumlahHK : 0;
                    m.T05LuasJelajah = m.T05JumlahHK > 0 ? (m.T05JumlahPohon / normaHanca) / m.T05JumlahHK : 0;
                    m.T06LuasJelajah = m.T06JumlahHK > 0 ? (m.T06JumlahPohon / normaHanca) / m.T06JumlahHK : 0;
                    m.T07LuasJelajah = m.T07JumlahHK > 0 ? (m.T07JumlahPohon / normaHanca) / m.T07JumlahHK : 0;
                    m.T08LuasJelajah = m.T08JumlahHK > 0 ? (m.T08JumlahPohon / normaHanca) / m.T08JumlahHK : 0;
                    m.T09LuasJelajah = m.T09JumlahHK > 0 ? (m.T09JumlahPohon / normaHanca) / m.T09JumlahHK : 0;
                    m.T10LuasJelajah = m.T10JumlahHK > 0 ? (m.T10JumlahPohon / normaHanca) / m.T10JumlahHK : 0;
                    m.T11LuasJelajah = m.T11JumlahHK > 0 ? (m.T11JumlahPohon / normaHanca) / m.T11JumlahHK : 0;
                    m.T12LuasJelajah = m.T12JumlahHK > 0 ? (m.T12JumlahPohon / normaHanca) / m.T12JumlahHK : 0;
                    kebun.Add(m);
                }
            }

            return Json(kebun, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TenagaKerjaPerKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);
            StatusLokasiKerja userStatus = new StatusLokasiKerja();
            userStatus = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00")
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun) - int.Parse(m.TahunTanam) equals n.Umur
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam, n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "00", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T01KgLateks = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks),
                            T01KgLump = g.Where(o => o.Bulan == 1).Sum(o => o.KgLump),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T01JumlahTandan = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks+o.KgLump),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02KgLateks = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks),
                            T02KgLump = g.Where(o => o.Bulan == 2).Sum(o => o.KgLump),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T02JumlahTandan = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks + o.KgLump),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03KgLateks = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks),
                            T03KgLump = g.Where(o => o.Bulan == 3).Sum(o => o.KgLump),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T03JumlahTandan = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks + o.KgLump),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04KgLateks = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks),
                            T04KgLump = g.Where(o => o.Bulan == 4).Sum(o => o.KgLump),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T04JumlahTandan = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks + o.KgLump),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05KgLateks = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks),
                            T05KgLump = g.Where(o => o.Bulan == 5).Sum(o => o.KgLump),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T05JumlahTandan = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks + o.KgLump),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06KgLateks = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks),
                            T06KgLump = g.Where(o => o.Bulan == 6).Sum(o => o.KgLump),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T06JumlahTandan = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks + o.KgLump),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07KgLateks = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks),
                            T07KgLump = g.Where(o => o.Bulan == 7).Sum(o => o.KgLump),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T07JumlahTandan = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks + o.KgLump),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08KgLateks = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks),
                            T08KgLump = g.Where(o => o.Bulan == 8).Sum(o => o.KgLump),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T08JumlahTandan = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks + o.KgLump),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09KgLateks = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks),
                            T09KgLump = g.Where(o => o.Bulan == 9).Sum(o => o.KgLump),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T09JumlahTandan = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks + o.KgLump),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10KgLateks = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks),
                            T10KgLump = g.Where(o => o.Bulan == 10).Sum(o => o.KgLump),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T10JumlahTandan = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks + o.KgLump),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11KgLateks = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks),
                            T11KgLump = g.Where(o => o.Bulan == 11).Sum(o => o.KgLump),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T11JumlahTandan = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks + o.KgLump),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12KgLateks = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks),
                            T12KgLump = g.Where(o => o.Bulan == 12).Sum(o => o.KgLump),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                            T12JumlahTandan = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks + o.KgLump),
                        };

            List<RotasiPanenPerTahun> kebun = new List<RotasiPanenPerTahun>();
            decimal RealisasiHcPerHK = 0;
            decimal NormaHcPerHK = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                    RealisasiHcPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) /
                                    (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                else
                    RealisasiHcPerHK = 0;

                NormaHcPerHK = 1;

                if (RealisasiHcPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 70 / 100)
                    m.Warna = "red";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 80 / 100)
                    m.Warna = "orange";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 90 / 100)
                    m.Warna = "yellow";
                else if (RealisasiHcPerHK <= NormaHcPerHK)
                    m.Warna = "green";
                else
                    m.Warna = "blue";
                kebun.Add(m);
            }

            return Json(kebun.OrderBy(o => o.NamaLokasi).ToDataSourceResult(request));
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

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "00" && o.status == "TM")
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
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal avg = 0;
                decimal jumlahHanca = 0;
                decimal jumlahHK = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    if (avg > 0) jumlahHanca = jumlahHanca + (CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahPohon)) / avg; 
                    jumlahHK = jumlahHK + (CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahHK));
                }

                if (jumlahHK > 0) m.Value1 = (double)(jumlahHanca / jumlahHK); else m.Value1 = 0;
                m.Value2 = 1;
                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= m.Value2*70/100)
                        m.Warna = "red";
                    else if (m.Value1 <= m.Value2 * 80 / 100)
                        m.Warna = "orange";
                    else if (m.Value1 <= m.Value2 * 90 / 100)
                        m.Warna = "yellow";
                    else if (m.Value1 <= m.Value2)
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
        public ActionResult ChartTenagaKerjaPerAfdeling(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 2) == Id)
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "00", "TM"),
                             T01JumlahPohon = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon),
                             T02JumlahPohon = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon),
                             T03JumlahPohon = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon),
                             T04JumlahPohon = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon),
                             T05JumlahPohon = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon),
                             T06JumlahPohon = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon),
                             T07JumlahPohon = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon),
                             T08JumlahPohon = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon),
                             T09JumlahPohon = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon),
                             T10JumlahPohon = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon),
                             T11JumlahPohon = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon),
                             T12JumlahPohon = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon),
                             T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                             T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                             T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                             T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                             T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                             T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                             T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                             T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                             T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                             T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                             T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                             T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                         }).ToList();

            List<RotasiPanenPerTahun> afdeling = new List<RotasiPanenPerTahun>();
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
                    m.T01LuasJelajah = m.T01JumlahHK > 0 ? (m.T01JumlahPohon / normaHanca) / m.T01JumlahHK : 0;
                    m.T02LuasJelajah = m.T02JumlahHK > 0 ? (m.T02JumlahPohon / normaHanca) / m.T02JumlahHK : 0;
                    m.T03LuasJelajah = m.T03JumlahHK > 0 ? (m.T03JumlahPohon / normaHanca) / m.T03JumlahHK : 0;
                    m.T04LuasJelajah = m.T04JumlahHK > 0 ? (m.T04JumlahPohon / normaHanca) / m.T04JumlahHK : 0;
                    m.T05LuasJelajah = m.T05JumlahHK > 0 ? (m.T05JumlahPohon / normaHanca) / m.T05JumlahHK : 0;
                    m.T06LuasJelajah = m.T06JumlahHK > 0 ? (m.T06JumlahPohon / normaHanca) / m.T06JumlahHK : 0;
                    m.T07LuasJelajah = m.T07JumlahHK > 0 ? (m.T07JumlahPohon / normaHanca) / m.T07JumlahHK : 0;
                    m.T08LuasJelajah = m.T08JumlahHK > 0 ? (m.T08JumlahPohon / normaHanca) / m.T08JumlahHK : 0;
                    m.T09LuasJelajah = m.T09JumlahHK > 0 ? (m.T09JumlahPohon / normaHanca) / m.T09JumlahHK : 0;
                    m.T10LuasJelajah = m.T10JumlahHK > 0 ? (m.T10JumlahPohon / normaHanca) / m.T10JumlahHK : 0;
                    m.T11LuasJelajah = m.T11JumlahHK > 0 ? (m.T11JumlahPohon / normaHanca) / m.T11JumlahHK : 0;
                    m.T12LuasJelajah = m.T12JumlahHK > 0 ? (m.T12JumlahPohon / normaHanca) / m.T12JumlahHK : 0;
                    afdeling.Add(m);
                }
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult TenagaKerjaPerAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 2) == Id)
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun) - int.Parse(m.TahunTanam) equals n.Umur
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam, n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "00", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T01KgLateks = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks),
                            T01KgLump = g.Where(o => o.Bulan == 1).Sum(o => o.KgLump),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T01JumlahTandan = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks + o.KgLump),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02KgLateks = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks),
                            T02KgLump = g.Where(o => o.Bulan == 2).Sum(o => o.KgLump),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T02JumlahTandan = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks + o.KgLump),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03KgLateks = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks),
                            T03KgLump = g.Where(o => o.Bulan == 3).Sum(o => o.KgLump),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T03JumlahTandan = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks + o.KgLump),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04KgLateks = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks),
                            T04KgLump = g.Where(o => o.Bulan == 4).Sum(o => o.KgLump),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T04JumlahTandan = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks + o.KgLump),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05KgLateks = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks),
                            T05KgLump = g.Where(o => o.Bulan == 5).Sum(o => o.KgLump),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T05JumlahTandan = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks + o.KgLump),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06KgLateks = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks),
                            T06KgLump = g.Where(o => o.Bulan == 6).Sum(o => o.KgLump),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T06JumlahTandan = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks + o.KgLump),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07KgLateks = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks),
                            T07KgLump = g.Where(o => o.Bulan == 7).Sum(o => o.KgLump),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T07JumlahTandan = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks + o.KgLump),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08KgLateks = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks),
                            T08KgLump = g.Where(o => o.Bulan == 8).Sum(o => o.KgLump),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T08JumlahTandan = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks + o.KgLump),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09KgLateks = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks),
                            T09KgLump = g.Where(o => o.Bulan == 9).Sum(o => o.KgLump),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T09JumlahTandan = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks + o.KgLump),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10KgLateks = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks),
                            T10KgLump = g.Where(o => o.Bulan == 10).Sum(o => o.KgLump),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T10JumlahTandan = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks + o.KgLump),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11KgLateks = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks),
                            T11KgLump = g.Where(o => o.Bulan == 11).Sum(o => o.KgLump),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T11JumlahTandan = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks + o.KgLump),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12KgLateks = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks),
                            T12KgLump = g.Where(o => o.Bulan == 12).Sum(o => o.KgLump),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                            T12JumlahTandan = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks + o.KgLump),
                        };

            List<RotasiPanenPerTahun> afdeling = new List<RotasiPanenPerTahun>();
            decimal RealisasiHcPerHK = 0;
            decimal NormaHcPerHK = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                    RealisasiHcPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) /
                                    (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                else
                    RealisasiHcPerHK = 0;

                NormaHcPerHK = 1;

                if (RealisasiHcPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 70 / 100)
                    m.Warna = "red";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 80 / 100)
                    m.Warna = "orange";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 90 / 100)
                    m.Warna = "yellow";
                else if (RealisasiHcPerHK <= NormaHcPerHK)
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
                decimal avg = 0;
                decimal jumlahHanca = 0;
                decimal jumlahHK = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    if (avg > 0) jumlahHanca = jumlahHanca + (CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahPohon)) / avg;
                    jumlahHK = jumlahHK + (CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahHK));
                }

                if (jumlahHK > 0) m.Value1 = (double)(jumlahHanca / jumlahHK); else m.Value1 = 0;
                m.Value2 = 1;
                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= m.Value2 * 70 / 100)
                        m.Warna = "red";
                    else if (m.Value1 <= m.Value2 * 80 / 100)
                        m.Warna = "orange";
                    else if (m.Value1 <= m.Value2 * 90 / 100)
                        m.Warna = "yellow";
                    else if (m.Value1 <= m.Value2)
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
        public ActionResult ChartTenagaKerjaPerBlok(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 4) == Id)
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "00", "TM"),
                             T01JumlahPohon = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon),
                             T02JumlahPohon = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon),
                             T03JumlahPohon = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon),
                             T04JumlahPohon = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon),
                             T05JumlahPohon = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon),
                             T06JumlahPohon = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon),
                             T07JumlahPohon = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon),
                             T08JumlahPohon = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon),
                             T09JumlahPohon = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon),
                             T10JumlahPohon = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon),
                             T11JumlahPohon = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon),
                             T12JumlahPohon = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon),
                             T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                             T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                             T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                             T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                             T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                             T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                             T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                             T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                             T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                             T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                             T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                             T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                         }).ToList();

            List<RotasiPanenPerTahun> blok = new List<RotasiPanenPerTahun>();
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
                    m.T01LuasJelajah = m.T01JumlahHK > 0 ? (m.T01JumlahPohon / normaHanca) / m.T01JumlahHK : 0;
                    m.T02LuasJelajah = m.T02JumlahHK > 0 ? (m.T02JumlahPohon / normaHanca) / m.T02JumlahHK : 0;
                    m.T03LuasJelajah = m.T03JumlahHK > 0 ? (m.T03JumlahPohon / normaHanca) / m.T03JumlahHK : 0;
                    m.T04LuasJelajah = m.T04JumlahHK > 0 ? (m.T04JumlahPohon / normaHanca) / m.T04JumlahHK : 0;
                    m.T05LuasJelajah = m.T05JumlahHK > 0 ? (m.T05JumlahPohon / normaHanca) / m.T05JumlahHK : 0;
                    m.T06LuasJelajah = m.T06JumlahHK > 0 ? (m.T06JumlahPohon / normaHanca) / m.T06JumlahHK : 0;
                    m.T07LuasJelajah = m.T07JumlahHK > 0 ? (m.T07JumlahPohon / normaHanca) / m.T07JumlahHK : 0;
                    m.T08LuasJelajah = m.T08JumlahHK > 0 ? (m.T08JumlahPohon / normaHanca) / m.T08JumlahHK : 0;
                    m.T09LuasJelajah = m.T09JumlahHK > 0 ? (m.T09JumlahPohon / normaHanca) / m.T09JumlahHK : 0;
                    m.T10LuasJelajah = m.T10JumlahHK > 0 ? (m.T10JumlahPohon / normaHanca) / m.T10JumlahHK : 0;
                    m.T11LuasJelajah = m.T11JumlahHK > 0 ? (m.T11JumlahPohon / normaHanca) / m.T11JumlahHK : 0;
                    m.T12LuasJelajah = m.T12JumlahHK > 0 ? (m.T12JumlahPohon / normaHanca) / m.T12JumlahHK : 0;
                    blok.Add(m);
                }
            }

            return Json(blok, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        public ActionResult TenagaKerjaPerBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 4) == Id)
                        join n in CRUDStandardProduktivitasKaret.CRUD.GetAllRecord() on int.Parse(tahun) - int.Parse(m.TahunTanam) equals n.Umur
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam, n.PohonDisadapPerHanca } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "00",
                            NamaBudidaya = "Karet",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "00", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T01KgLateks = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks),
                            T01KgLump = g.Where(o => o.Bulan == 1).Sum(o => o.KgLump),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T01JumlahTandan = g.Where(o => o.Bulan == 1).Sum(o => o.KgLateks + o.KgLump),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T02KgLateks = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks),
                            T02KgLump = g.Where(o => o.Bulan == 2).Sum(o => o.KgLump),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T02JumlahTandan = g.Where(o => o.Bulan == 2).Sum(o => o.KgLateks + o.KgLump),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T03KgLateks = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks),
                            T03KgLump = g.Where(o => o.Bulan == 3).Sum(o => o.KgLump),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T03JumlahTandan = g.Where(o => o.Bulan == 3).Sum(o => o.KgLateks + o.KgLump),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T04KgLateks = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks),
                            T04KgLump = g.Where(o => o.Bulan == 4).Sum(o => o.KgLump),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T04JumlahTandan = g.Where(o => o.Bulan == 4).Sum(o => o.KgLateks + o.KgLump),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T05KgLateks = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks),
                            T05KgLump = g.Where(o => o.Bulan == 5).Sum(o => o.KgLump),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T05JumlahTandan = g.Where(o => o.Bulan == 5).Sum(o => o.KgLateks + o.KgLump),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T06KgLateks = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks),
                            T06KgLump = g.Where(o => o.Bulan == 6).Sum(o => o.KgLump),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T06JumlahTandan = g.Where(o => o.Bulan == 6).Sum(o => o.KgLateks + o.KgLump),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T07KgLateks = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks),
                            T07KgLump = g.Where(o => o.Bulan == 7).Sum(o => o.KgLump),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T07JumlahTandan = g.Where(o => o.Bulan == 7).Sum(o => o.KgLateks + o.KgLump),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T08KgLateks = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks),
                            T08KgLump = g.Where(o => o.Bulan == 8).Sum(o => o.KgLump),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T08JumlahTandan = g.Where(o => o.Bulan == 8).Sum(o => o.KgLateks + o.KgLump),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T09KgLateks = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks),
                            T09KgLump = g.Where(o => o.Bulan == 9).Sum(o => o.KgLump),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T09JumlahTandan = g.Where(o => o.Bulan == 9).Sum(o => o.KgLateks + o.KgLump),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T10KgLateks = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks),
                            T10KgLump = g.Where(o => o.Bulan == 10).Sum(o => o.KgLump),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T10JumlahTandan = g.Where(o => o.Bulan == 10).Sum(o => o.KgLateks + o.KgLump),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T11KgLateks = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks),
                            T11KgLump = g.Where(o => o.Bulan == 11).Sum(o => o.KgLump),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T11JumlahTandan = g.Where(o => o.Bulan == 11).Sum(o => o.KgLateks + o.KgLump),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahPohon) / g.Key.PohonDisadapPerHanca,
                            T12KgLateks = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks),
                            T12KgLump = g.Where(o => o.Bulan == 12).Sum(o => o.KgLump),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                            T12JumlahTandan = g.Where(o => o.Bulan == 12).Sum(o => o.KgLateks + o.KgLump),
                        };

            List<RotasiPanenPerTahun> blok = new List<RotasiPanenPerTahun>();
            decimal RealisasiHcPerHK = 0;
            decimal NormaHcPerHK = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                    RealisasiHcPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah + m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) /
                                    (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK + m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                else
                    RealisasiHcPerHK = 0;

                NormaHcPerHK = 1;

                if (RealisasiHcPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 70 / 100)
                    m.Warna = "red";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 80 / 100)
                    m.Warna = "orange";
                else if (RealisasiHcPerHK <= NormaHcPerHK * 90 / 100)
                    m.Warna = "yellow";
                else if (RealisasiHcPerHK <= NormaHcPerHK)
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
                decimal avg = 0;
                decimal jumlahHanca = 0;
                decimal jumlahHK = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = (CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasKaret.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).PohonDisadapPerHanca : 0);
                    if (avg > 0) jumlahHanca = jumlahHanca + (CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahPohon)) / avg;
                    jumlahHK = jumlahHK + (CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun && o.TahunTanam == strTahunTanam[i]).Sum(o => o.JumlahHK));
                }

                if (jumlahHK > 0) m.Value1 = (double)(jumlahHanca / jumlahHK); else m.Value1 = 0;
                m.Value2 = 1;
                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= m.Value2 * 70 / 100)
                        m.Warna = "red";
                    else if (m.Value1 <= m.Value2 * 80 / 100)
                        m.Warna = "orange";
                    else if (m.Value1 <= m.Value2 * 90 / 100)
                        m.Warna = "yellow";
                    else if (m.Value1 <= m.Value2)
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