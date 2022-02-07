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
    public class TenagaKerjaPanenSawitController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasSawit
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

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01")
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                                Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                                Tahun = int.Parse(tahun),
                                KodeLokasi = g.Key.KodeBlok,
                                NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                                NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                                KodeBudidaya = "01",
                                NamaBudidaya = "Sawit",
                                TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                                LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "01", "TM"),
                                JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "01", "TM"),
                                T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) : 0,
                                T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) : 0,
                                T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) : 0,
                                T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) : 0,
                                T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) : 0,
                                T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) : 0,
                                T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) : 0,
                                T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) : 0,
                                T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) : 0,
                                T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) : 0,
                                T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) : 0,
                                T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) : 0,
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
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
            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01")
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "01", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah),
                            T01KgTBS = g.Where(o => o.Bulan == 1).Sum(o => o.KgTBS),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah),
                            T02KgTBS = g.Where(o => o.Bulan == 2).Sum(o => o.KgTBS),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah),
                            T03KgTBS = g.Where(o => o.Bulan == 3).Sum(o => o.KgTBS),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah),
                            T04KgTBS = g.Where(o => o.Bulan == 4).Sum(o => o.KgTBS),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah),
                            T05KgTBS = g.Where(o => o.Bulan == 5).Sum(o => o.KgTBS),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah),
                            T06KgTBS = g.Where(o => o.Bulan == 6).Sum(o => o.KgTBS),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah),
                            T07KgTBS = g.Where(o => o.Bulan == 7).Sum(o => o.KgTBS),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah),
                            T08KgTBS = g.Where(o => o.Bulan == 8).Sum(o => o.KgTBS),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah),
                            T09KgTBS = g.Where(o => o.Bulan == 9).Sum(o => o.KgTBS),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah),
                            T10KgTBS = g.Where(o => o.Bulan == 10).Sum(o => o.KgTBS),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah),
                            T11KgTBS = g.Where(o => o.Bulan == 11).Sum(o => o.KgTBS),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah),
                            T12KgTBS = g.Where(o => o.Bulan == 12).Sum(o => o.KgTBS),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                        };

            List<RotasiPanenPerTahun> kebun = new List<RotasiPanenPerTahun>();
            decimal RealisasiHaPerHK = 0;
            decimal NormaHKPerHa = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                {
                    RealisasiHaPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah +
                    m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) / (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                }
                else
                    RealisasiHaPerHK = 0;

                NormaHKPerHa = (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ? 
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).HKperHa: 0);

                if(RealisasiHaPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 70 / 100))
                    m.Warna = "red";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 80 / 100))
                    m.Warna = "orange";
                else if (RealisasiHaPerHK <= ( NormaHKPerHa * 90 / 100))
                    m.Warna = "yellow";
                else if (RealisasiHaPerHK <= (NormaHKPerHa))
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
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal avg = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).HKperHa : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                if (CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK) > 0)
                {
                    m.Value1 = (double) ((CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah)) /
                        (CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK)));
                }
                else
                    m.Value1 = 0;

                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "01", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= (m.Value2 * 70 / 100))
                        m.Warna = "red";
                    else if (m.Value1 <= (m.Value2 * 80 / 100))
                        m.Warna = "orange";
                    else if (m.Value1 <= (m.Value2 * 90 / 100))
                        m.Warna = "yellow";
                    else if (m.Value1 <= (m.Value2 * 100 / 100))
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

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 2) == Id)
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "01", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "01", "TM"),
                             T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) : 0,
                             T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) : 0,
                             T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) : 0,
                             T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) : 0,
                             T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) : 0,
                             T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) : 0,
                             T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) : 0,
                             T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) : 0,
                             T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) : 0,
                             T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) : 0,
                             T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) : 0,
                             T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) : 0,
                         }).ToList();

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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 2) == Id)
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "01", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah),
                            T01KgTBS = g.Where(o => o.Bulan == 1).Sum(o => o.KgTBS),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah),
                            T02KgTBS = g.Where(o => o.Bulan == 2).Sum(o => o.KgTBS),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah),
                            T03KgTBS = g.Where(o => o.Bulan == 3).Sum(o => o.KgTBS),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah),
                            T04KgTBS = g.Where(o => o.Bulan == 4).Sum(o => o.KgTBS),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah),
                            T05KgTBS = g.Where(o => o.Bulan == 5).Sum(o => o.KgTBS),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah),
                            T06KgTBS = g.Where(o => o.Bulan == 6).Sum(o => o.KgTBS),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah),
                            T07KgTBS = g.Where(o => o.Bulan == 7).Sum(o => o.KgTBS),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah),
                            T08KgTBS = g.Where(o => o.Bulan == 8).Sum(o => o.KgTBS),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah),
                            T09KgTBS = g.Where(o => o.Bulan == 9).Sum(o => o.KgTBS),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah),
                            T10KgTBS = g.Where(o => o.Bulan == 10).Sum(o => o.KgTBS),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah),
                            T11KgTBS = g.Where(o => o.Bulan == 11).Sum(o => o.KgTBS),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah),
                            T12KgTBS = g.Where(o => o.Bulan == 12).Sum(o => o.KgTBS),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                        };

            List<RotasiPanenPerTahun> afdeling = new List<RotasiPanenPerTahun>();
            decimal RealisasiHaPerHK = 0;
            decimal NormaHKPerHa = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                {
                    RealisasiHaPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah +
                    m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) / (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                }
                else
                    RealisasiHaPerHK = 0;

                NormaHKPerHa = (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ?
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).HKperHa : 0);

                if (RealisasiHaPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 70 / 100))
                    m.Warna = "red";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 80 / 100))
                    m.Warna = "orange";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 90 / 100))
                    m.Warna = "yellow";
                else if (RealisasiHaPerHK <= (NormaHKPerHa))
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
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).HKperHa : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                if (CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK) > 0)
                {
                    m.Value1 = (double)((CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah)) /
                        (CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK)));
                }
                else
                    m.Value1 = 0;

                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun, "01", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= (m.Value2 * 70 / 100))
                        m.Warna = "red";
                    else if (m.Value1 <= (m.Value2 * 80 / 100))
                        m.Warna = "orange";
                    else if (m.Value1 <= (m.Value2 * 90 / 100))
                        m.Warna = "yellow";
                    else if (m.Value1 <= (m.Value2 * 100 / 100))
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

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 4) == Id)
                         where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                         group m by new { m.KodeBlok } into g
                         select new RotasiPanenPerTahun
                         {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "01", "TM"),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "01", "TM"),
                             T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK) : 0,
                             T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK) : 0,
                             T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK) : 0,
                             T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK) : 0,
                             T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK) : 0,
                             T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK) : 0,
                             T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK) : 0,
                             T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK) : 0,
                             T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK) : 0,
                             T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK) : 0,
                             T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK) : 0,
                             T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) > 0 ? g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah) / g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK) : 0,
                         }).ToList();

            return Json(model, JsonRequestBehavior.AllowGet);
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.KodeBlok.Substring(0, 4) == Id)
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerTahun
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.KodeBlok,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM") == null ? "" : CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "01", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            KodeBudidaya = "01",
                            NamaBudidaya = "Sawit",
                            TahunTanam = g.Key.TahunTanam,
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "01", "TM", g.Key.TahunTanam),
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.KodeBlok, "01", "TM"),
                            T01LuasJelajah = g.Where(o => o.Bulan == 1).Sum(o => o.HaJelajah),
                            T01KgTBS = g.Where(o => o.Bulan == 1).Sum(o => o.KgTBS),
                            T01JumlahHK = g.Where(o => o.Bulan == 1).Sum(o => o.JumlahHK),
                            T01JumlahUpah = g.Where(o => o.Bulan == 1).Sum(o => o.RpGaji),
                            T02LuasJelajah = g.Where(o => o.Bulan == 2).Sum(o => o.HaJelajah),
                            T02KgTBS = g.Where(o => o.Bulan == 2).Sum(o => o.KgTBS),
                            T02JumlahHK = g.Where(o => o.Bulan == 2).Sum(o => o.JumlahHK),
                            T02JumlahUpah = g.Where(o => o.Bulan == 2).Sum(o => o.RpGaji),
                            T03LuasJelajah = g.Where(o => o.Bulan == 3).Sum(o => o.HaJelajah),
                            T03KgTBS = g.Where(o => o.Bulan == 3).Sum(o => o.KgTBS),
                            T03JumlahHK = g.Where(o => o.Bulan == 3).Sum(o => o.JumlahHK),
                            T03JumlahUpah = g.Where(o => o.Bulan == 3).Sum(o => o.RpGaji),
                            T04LuasJelajah = g.Where(o => o.Bulan == 4).Sum(o => o.HaJelajah),
                            T04KgTBS = g.Where(o => o.Bulan == 4).Sum(o => o.KgTBS),
                            T04JumlahHK = g.Where(o => o.Bulan == 4).Sum(o => o.JumlahHK),
                            T04JumlahUpah = g.Where(o => o.Bulan == 4).Sum(o => o.RpGaji),
                            T05LuasJelajah = g.Where(o => o.Bulan == 5).Sum(o => o.HaJelajah),
                            T05KgTBS = g.Where(o => o.Bulan == 5).Sum(o => o.KgTBS),
                            T05JumlahHK = g.Where(o => o.Bulan == 5).Sum(o => o.JumlahHK),
                            T05JumlahUpah = g.Where(o => o.Bulan == 5).Sum(o => o.RpGaji),
                            T06LuasJelajah = g.Where(o => o.Bulan == 6).Sum(o => o.HaJelajah),
                            T06KgTBS = g.Where(o => o.Bulan == 6).Sum(o => o.KgTBS),
                            T06JumlahHK = g.Where(o => o.Bulan == 6).Sum(o => o.JumlahHK),
                            T06JumlahUpah = g.Where(o => o.Bulan == 6).Sum(o => o.RpGaji),
                            T07LuasJelajah = g.Where(o => o.Bulan == 7).Sum(o => o.HaJelajah),
                            T07KgTBS = g.Where(o => o.Bulan == 7).Sum(o => o.KgTBS),
                            T07JumlahHK = g.Where(o => o.Bulan == 7).Sum(o => o.JumlahHK),
                            T07JumlahUpah = g.Where(o => o.Bulan == 7).Sum(o => o.RpGaji),
                            T08LuasJelajah = g.Where(o => o.Bulan == 8).Sum(o => o.HaJelajah),
                            T08KgTBS = g.Where(o => o.Bulan == 8).Sum(o => o.KgTBS),
                            T08JumlahHK = g.Where(o => o.Bulan == 8).Sum(o => o.JumlahHK),
                            T08JumlahUpah = g.Where(o => o.Bulan == 8).Sum(o => o.RpGaji),
                            T09LuasJelajah = g.Where(o => o.Bulan == 9).Sum(o => o.HaJelajah),
                            T09KgTBS = g.Where(o => o.Bulan == 9).Sum(o => o.KgTBS),
                            T09JumlahHK = g.Where(o => o.Bulan == 9).Sum(o => o.JumlahHK),
                            T09JumlahUpah = g.Where(o => o.Bulan == 9).Sum(o => o.RpGaji),
                            T10LuasJelajah = g.Where(o => o.Bulan == 10).Sum(o => o.HaJelajah),
                            T10KgTBS = g.Where(o => o.Bulan == 10).Sum(o => o.KgTBS),
                            T10JumlahHK = g.Where(o => o.Bulan == 10).Sum(o => o.JumlahHK),
                            T10JumlahUpah = g.Where(o => o.Bulan == 10).Sum(o => o.RpGaji),
                            T11LuasJelajah = g.Where(o => o.Bulan == 11).Sum(o => o.HaJelajah),
                            T11KgTBS = g.Where(o => o.Bulan == 11).Sum(o => o.KgTBS),
                            T11JumlahHK = g.Where(o => o.Bulan == 11).Sum(o => o.JumlahHK),
                            T11JumlahUpah = g.Where(o => o.Bulan == 11).Sum(o => o.RpGaji),
                            T12LuasJelajah = g.Where(o => o.Bulan == 12).Sum(o => o.HaJelajah),
                            T12KgTBS = g.Where(o => o.Bulan == 12).Sum(o => o.KgTBS),
                            T12JumlahHK = g.Where(o => o.Bulan == 12).Sum(o => o.JumlahHK),
                            T12JumlahUpah = g.Where(o => o.Bulan == 12).Sum(o => o.RpGaji),
                        };

            List<RotasiPanenPerTahun> blok = new List<RotasiPanenPerTahun>();
            decimal RealisasiHaPerHK = 0;
            decimal NormaHKPerHa = 0;
            foreach (var m in model)
            {
                if (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK > 0)
                {
                    RealisasiHaPerHK = (m.T01LuasJelajah + m.T02LuasJelajah + m.T03LuasJelajah + m.T04LuasJelajah + m.T05LuasJelajah + m.T06LuasJelajah +
                    m.T07LuasJelajah + m.T08LuasJelajah + m.T09LuasJelajah + m.T10LuasJelajah + m.T11LuasJelajah + m.T12LuasJelajah) / (m.T01JumlahHK + m.T02JumlahHK + m.T03JumlahHK + m.T04JumlahHK + m.T05JumlahHK + m.T06JumlahHK +
                    m.T07JumlahHK + m.T08JumlahHK + m.T09JumlahHK + m.T10JumlahHK + m.T11JumlahHK + m.T12JumlahHK);
                }
                else
                    RealisasiHaPerHK = 0;

                NormaHKPerHa = (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)) != null ?
                    CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(m.TahunTanam)).HKperHa : 0);

                if (RealisasiHaPerHK == 0)
                    m.Warna = "black";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 70 / 100))
                    m.Warna = "red";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 80 / 100))
                    m.Warna = "orange";
                else if (RealisasiHaPerHK <= (NormaHKPerHa * 90 / 100))
                    m.Warna = "yellow";
                else if (RealisasiHaPerHK <= (NormaHKPerHa))
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
                    avg = avg + (CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])) != null ? CRUDStandardProduktivitasSawit.CRUD.GetAllRecord().FirstOrDefault(o => o.Umur == DateTime.Now.Year - int.Parse(strTahunTanam[i])).HKperHa : 0);
                }
                if (avg > 0) m.Value2 = (double)(avg / strTahunTanam.Length); else m.Value2 = 0;

                if (CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK) > 0)
                {
                    m.Value1 = (double)((CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.HaJelajah)) /
                        (CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "01" && o.Bulan <= int.Parse(bulan) && o.Tahun == int.Parse(tahun) && o.KodeBlok == m.KodeKebun).Sum(o => o.JumlahHK)));
                }
                else
                    m.Value1 = 0;

                m.Value3 = (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(m.KodeKebun, "01", "TM");
                if (warna == "")
                {
                    if (m.Value1 == 0)
                        m.Warna = "black";
                    else if (m.Value1 <= (m.Value2 * 70 / 100))
                        m.Warna = "red";
                    else if (m.Value1 <= (m.Value2 * 80 / 100))
                        m.Warna = "orange";
                    else if (m.Value1 <= (m.Value2 * 90 / 100))
                        m.Warna = "yellow";
                    else if (m.Value1 <= (m.Value2 * 100 / 100))
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