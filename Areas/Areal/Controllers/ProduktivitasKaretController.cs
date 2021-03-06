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
    public class ProduktivitasKaretController : Controller
    {
        
        [Authorize]
        // GET: Areal/ProduktivitasKaret
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Produktivitas";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
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

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00")
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok} into g
                        select new ProduktivitasPerKebun
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            Wilayah = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            Budidaya = "Karet",
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),
                            ProtasKebun_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM"),

                            ProtasPTPN8_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan == int.Parse(bulan) ).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("00", "TM"),
                            ProtasPTPN8_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("00", "TM"),

                        }).ToList();

            for (int j = 0; j < model.Count(); j++)
            {
                string[] strTahunTanam = model[j].TahunTanam.Split(',').Distinct().ToArray();
                double avgProtas = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                }
                model[j].ProtasKebun_BalaiBI = (avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                model[j].ProtasKebun_BalaiSBI = ((avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100) * int.Parse(bulan);
            }

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult ProtasPerKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
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
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun) 
                            && (userStatus.NamaLokasiKerja=="Afdeling" ? m.KodeBlok.Substring(0,2)==CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn : 
                            userStatus.NamaLokasiKerja=="Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new ProduktivitasPerKebun
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).KebunId,
                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            TahunTanam = g.Key.TahunTanam,
                            Wilayah = CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.KodeBlok).Wilayah.Nama,
                            Budidaya = "Karet",
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("00", "TM", g.Key.TahunTanam),
                            ProtasKebun_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) ,
                            ProtasKebun_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) ,
                            ProtasKebun_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) ,
                            ProtasKebun_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) ,
                            ProtasKebun_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) ,
                            ProtasKebun_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) ,

                            ProtasPTPN8_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan == int.Parse(bulan) && o.TahunTanam == g.Key.TahunTanam).Sum(o => o.KgLateks+o.KgLump)) ,
                            ProtasPTPN8_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.TahunTanam == g.Key.TahunTanam).Sum(o => o.KgLateks+o.KgLump)),

                            ProtasKebun_BalaiBI = (double)CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12,
                            ProtasKebun_BalaiSBI = (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12) * int.Parse(bulan),

                        };
            List<ProduktivitasPerKebun> kebun = new List<ProduktivitasPerKebun>();
            foreach(var m in model)
            {
                if(m.ProtasKebun_BI<0.9*m.ProtasPTPN8_BI)
                {
                    m.Warna_Sem1 = "red";
                }
                else if (m.ProtasKebun_BI >= m.ProtasPTPN8_BI)
                {
                    m.Warna_Sem1 = "green";
                }
                else
                {
                    m.Warna_Sem1 = "yellow";
                }
                kebun.Add(m);
            }

            return Json(kebun.OrderBy(o => o.Kebun).Where(o => o.ProtasKebun_SBI + o.ProtasKebun_RSBI > 0).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult KebunGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna="", string perbandingan = "", string pctBalai = "100")
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

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord()
                         where m.kodebudidaya == "00" && m.status == "TM"
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.kodekebun == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.kodekebun).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                         group m by new { g1 = m.kodekebun} into g
                         select new PetaView
                         {
                             KodeKebun = g.Key.g1,
                             Id = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                             Peta = CRUDKebunPeta.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebunPeta.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDKebunPeta.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                             Nama = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                             TahunTanam = string.Join(",",g.Select(o=>o.tahuntanam).ToArray())
                         }).ToList();

            List<PetaView> kebun = new List<PetaView>();
            double? RealisasiProduksi;
            double? RKAPProduksi;
            double? LuasAreal;
            
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                LuasAreal = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "00", "TM");
                RealisasiProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.KgLateks+o.KgLump) / LuasAreal;
                if (perbandingan == "Induk")
                {
                    double sumProd = 0;
                    double sumAreal = 0;

                    //for (int i = 0; i < strTahunTanam.Length; i++)
                    //{
                    //    sumProd += (double)CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00" && o.TahunTanam == strTahunTanam[i]).Sum(o => o.KgLateks+o.KgLump); 
                    //    sumAreal += (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("00", "TM",strTahunTanam[i]);
                    //}

                    sumProd = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.KgLateks + o.KgLump);
                    sumAreal = (double)CRUDRef_Areal.CRUD.HitungLuasArealPTPN8("00", "TM");

                    if (sumAreal > 0) RKAPProduksi = (sumProd / sumAreal); else RKAPProduksi = 0;
                }
                else if (perbandingan == "RKAP")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / LuasAreal;
                }
                else if (perbandingan == "PKB")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.PKBKgLateks+o.PKBKgLump) / LuasAreal;
                }
                else
                {
                    double avgProtas = 0;
                    for (int i=0; i<strTahunTanam.Length; i++)
                    {
                        avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                    }
                    RKAPProduksi = (avgProtas/strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                }

                if (LuasAreal > 0)
                {
                    m.Value3 = LuasAreal;
                    m.Value1 = RealisasiProduksi;
                    m.Value2 = RKAPProduksi;

                    if (RKAPProduksi > 0)
                    {
                        if (warna == "")
                        {
                            if (RealisasiProduksi / RKAPProduksi < 0.9) m.Warna = "red";
                            else if (RealisasiProduksi / RKAPProduksi >= 1) m.Warna = "green";
                            else m.Warna = "yellow";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        kebun.Add(m);

                    }
                    else
                    {
                        if (warna == "")
                        {
                            m.Warna = "black";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        kebun.Add(m);
                    }
                }
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
        public ActionResult ChartProtasPerAfdeling(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
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
                         select new ProduktivitasPerAfdeling
                         {
                             Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                             Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                             Kebun = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                             Budidaya = "Karet",
                             LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),
                             ProtasAfdeling_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM"),

                             ProtasKebun_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan == int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2) ).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "00", "TM"),
                             ProtasKebun_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2) ).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "00", "TM"),

                         }).ToList();

            for (int j = 0; j < model.Count(); j++)
            {
                string[] strTahunTanam = model[j].TahunTanam.Split(',').Distinct().ToArray();
                double avgProtas = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                }
                model[j].ProtasAfdeling_BalaiBI = (avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                model[j].ProtasAfdeling_BalaiSBI = ((avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100) * int.Parse(bulan);
            }

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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0,2)==Id)
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam} into g
                        select new ProduktivitasPerAfdeling
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).AfdelingId,
                            Afdeling= CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Nama,
                            TahunTanam = g.Key.TahunTanam,
                            Kebun = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok).Kebun.Nama,
                            Budidaya = "Karet",
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.KodeBlok.Substring(0, 2), "00", "TM", g.Key.TahunTanam),
                            ProtasAfdeling_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump),
                            ProtasAfdeling_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump),
                            ProtasAfdeling_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump),
                            ProtasAfdeling_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump),
                            ProtasAfdeling_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump),
                            ProtasAfdeling_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump),

                            ProtasKebun_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan == int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2) && o.TahunTanam == g.Key.TahunTanam).Sum(o => o.KgLateks+o.KgLump)),
                            ProtasKebun_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 2) && o.TahunTanam == g.Key.TahunTanam).Sum(o => o.KgLateks+o.KgLump)),

                            ProtasAfdeling_BalaiBI = (double)CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12,
                            ProtasAfdeling_BalaiSBI = (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12) * int.Parse(bulan),

                        };

            List<ProduktivitasPerAfdeling> afdeling = new List<ProduktivitasPerAfdeling>();
            foreach (var m in model)
            {
                if (m.ProtasAfdeling_BI < 0.9 * m.ProtasKebun_BI)
                {
                    m.Warna_Sem1 = "red";
                }
                else if (m.ProtasAfdeling_BI >= m.ProtasKebun_BI)
                {
                    m.Warna_Sem1 = "green";
                }
                else
                {
                    m.Warna_Sem1 = "yellow";
                }
                afdeling.Add(m);
            }

            return Json(afdeling.OrderBy(o => o.Afdeling).Where(o => o.ProtasAfdeling_SBI + o.ProtasAfdeling_RSBI > 0).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult AfdelingGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna="", string perbandingan = "", string pctBalai = "100")
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
                         group m by new { g1 = m.kodekebun + m.kodeafdeling} into g
                         select new PetaView
                         {
                             KodeKebun = g.Key.g1,
                             Id = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).AfdelingId,
                             Peta = CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDAfdelingPeta.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                             Nama = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                             TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                         }).ToList();

            List<PetaView> afdeling = new List<PetaView>();
            double? RealisasiProduksi;
            double? RKAPProduksi;
            double? LuasAreal;
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                LuasAreal = (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun, "00", "TM");
                RealisasiProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00" ).Sum(o => o.KgLateks+o.KgLump) / LuasAreal;
                if (perbandingan == "Induk")
                {
                    double sumProd = 0;
                    double sumAreal = 0;
                    //for (int i = 0; i < strTahunTanam.Length; i++)
                    //{
                    //    sumProd += (double)CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBlok == m.KodeKebun.Substring(0, 2) && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00" && o.TahunTanam == strTahunTanam[i]).Sum(o => o.KgLateks+o.KgLump);
                    //    sumAreal += (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun.Substring(0, 2), "00", "TM",strTahunTanam[i]);
                    //}
                    sumProd = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerKebun().Where(o => o.KodeBlok == m.KodeKebun.Substring(0, 2) && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.KgLateks + o.KgLump);
                    sumAreal = (double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun.Substring(0, 2), "00", "TM");
                    if (sumAreal > 0) RKAPProduksi = (sumProd / sumAreal); else RKAPProduksi = 0;

                }
                else if (perbandingan == "RKAP")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / LuasAreal;
                }
                else if (perbandingan == "PKB")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.PKBKgLateks+o.PKBKgLump) / LuasAreal;
                }
                else
                {
                    double avgProtas = 0;
                    for (int i = 0; i < strTahunTanam.Length; i++)
                    {
                        avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                    }
                    RKAPProduksi = (avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                }

                if (LuasAreal > 0)
                {

                    m.Value3 = LuasAreal;
                    m.Value1 = RealisasiProduksi;
                    m.Value2 = RKAPProduksi;

                    if (RKAPProduksi > 0)
                    {
                        if (warna == "")
                        {
                            if (RealisasiProduksi / RKAPProduksi < 0.9) m.Warna = "red";
                            else if (RealisasiProduksi / RKAPProduksi >= 1) m.Warna = "green";
                            else m.Warna = "yellow";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        afdeling.Add(m);

                    }
                    else
                    {
                        if (warna == "")
                        {
                            m.Warna = "black";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        afdeling.Add(m);
                    }
                }
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
        public ActionResult ChartProtasPerBlok(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string perbandingan = "", string pctBalai = "100")
        {
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDHasilPanen.CRUD.TahunBuku = int.Parse(tahun);
            CRUDHasilPanen.CRUD.BulanBuku = int.Parse(bulan);

            var model = (from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0, 4) == Id)
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun)
                        group m by new { m.KodeBlok} into g
                        select new ProduktivitasPerBlok
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Blok = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            TahunTanam = string.Join(",", g.Select(o => o.TahunTanam).ToArray()),
                            Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)).Nama,
                            Budidaya = "Karet",
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),
                            ProtasBlok_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump) / (double)CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM"),

                            ProtasAfdeling_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.Bulan == int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "00", "TM"),
                            ProtasAfdeling_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4)).Sum(o => o.KgLateks+o.KgLump)) / (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "00", "TM"),
                        }).ToList();

            for (int j = 0; j < model.Count(); j++)
            {
                string[] strTahunTanam = model[j].TahunTanam.Split(',').Distinct().ToArray();
                double avgProtas = 0;
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                }
                model[j].ProtasBlok_BalaiBI = (avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                model[j].ProtasBlok_BalaiSBI = ((avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100) * int.Parse(bulan);
            }

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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBudidaya == "00" && o.KodeBlok.Substring(0,4)==Id)
                        where m.Bulan <= int.Parse(bulan) && m.Tahun == int.Parse(tahun) 
                        group m by new { m.KodeBlok, m.TahunTanam} into g
                        select new ProduktivitasPerBlok
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.KodeBlok).BlokId,
                            Blok = CRUDRef_Areal.CRUD.Get1Record(g.Key.KodeBlok, "00", "TM").namablok,
                            TahunTanam = g.Key.TahunTanam,
                            Afdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.KodeBlok.Substring(0, 4)).Nama,
                            Budidaya = "Karet",
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.KodeBlok, "00", "TM", g.Key.TahunTanam),
                            LuasArealInduk = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.KodeBlok.Substring(0, 4), "00", "TM", g.Key.TahunTanam),
                            ProtasBlok_BI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o=>o.KgLateks+o.KgLump),
                            ProtasBlok_RBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump),
                            ProtasBlok_PBI = (double)g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump),
                            ProtasBlok_SBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.KgLateks+o.KgLump),
                            ProtasBlok_RSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPKgLateks+o.RKAPKgLump),
                            ProtasBlok_PSBI = (double)g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBKgLateks+o.PKBKgLump),

                            ProtasAfdeling_BI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o=>o.KodeBudidaya=="00" && o.Bulan == int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0,4) && o.TahunTanam == g.Key.TahunTanam).Sum(o=>o.KgLateks+o.KgLump) ),
                            ProtasAfdeling_SBI = (double)(CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBudidaya == "00" && o.Bulan <= int.Parse(bulan) && o.KodeBlok == g.Key.KodeBlok.Substring(0, 4) && o.TahunTanam == g.Key.TahunTanam).Sum(o => o.KgLateks+o.KgLump) ),

                            ProtasBlok_BalaiBI = (double)CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12,
                            ProtasBlok_BalaiSBI = (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(g.Key.TahunTanam)).TonPerHa / 12) * int.Parse(bulan),

                        };

            List<ProduktivitasPerBlok> blok = new List<ProduktivitasPerBlok>();
            foreach (var m in model)
            {
                if (m.ProtasBlok_BI < 0.9 * m.ProtasAfdeling_BI)
                {
                    m.Warna_Sem1 = "red";
                }
                else if (m.ProtasBlok_BI >= m.ProtasAfdeling_BI)
                {
                    m.Warna_Sem1 = "green";
                }
                else
                {
                    m.Warna_Sem1 = "yellow";
                }
                blok.Add(m);
            }

            return Json(blok.OrderBy(o => o.Blok).Where(o=>o.ProtasBlok_SBI+o.ProtasBlok_RSBI > 0).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult BlokGeoJSon(string Id = "", string budidaya = "", string tahun = "", string bulan = "", string warna = "", string perbandingan="", string pctBalai="")
        {
            if (perbandingan == "") return View();
            if (Id == "") return View(); else Id = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;
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

            var model = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodekebun + o.kodeafdeling == Id).Where(o=>o.kodebudidaya=="00" && o.status=="TM")
                        group m by new { g1 = m.kodekebun + m.kodeafdeling + m.kodeblok} into g
                        select new PetaView
                        {
                            KodeKebun = g.Key.g1,
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.g1)==null? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                            Peta = CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1)==null? "" : CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta !=null ? CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                            Nama = g.Select(o=>o.namablok).FirstOrDefault(),
                            TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                        }).ToList();

            List<PetaView> blok = new List<PetaView>();
            double? RealisasiProduksi;
            double? RKAPProduksi;
            double? LuasAreal;
            foreach (var m in model)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                LuasAreal = (double) CRUDRef_Areal.CRUD.HitungLuasArealBlok(m.KodeKebun, "00", "TM");
                RealisasiProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.KgLateks+o.KgLump) / LuasAreal;
                if (perbandingan=="Induk")
                {
                    double sumProd = 0;
                    double sumAreal = 0;
                    //for (int i = 0; i < strTahunTanam.Length; i++)
                    //{
                    //    sumProd += (double)CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBlok == m.KodeKebun.Substring(0, 4) && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00" && o.TahunTanam == strTahunTanam[i]).Sum(o => o.KgLateks+o.KgLump);
                    //    sumAreal += (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun.Substring(0, 4), "00", "TM",strTahunTanam[i]);
                    //}
                    sumProd = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerAfdeling().Where(o => o.KodeBlok == m.KodeKebun.Substring(0, 4) && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.KgLateks + o.KgLump);
                    sumAreal = (double)CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(m.KodeKebun.Substring(0, 4), "00", "TM");

                    if (sumAreal > 0) RKAPProduksi = (sumProd / sumAreal); else RKAPProduksi = 0;

                }
                else if (perbandingan == "RKAP")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.RKAPKgLateks+o.RKAPKgLump) / LuasAreal;
                }
                else if (perbandingan == "PKB")
                {
                    RKAPProduksi = (double)CRUDHasilPanen.CRUD.GetHasilPanenPerBlok().Where(o => o.KodeBlok == m.KodeKebun && o.Bulan == int.Parse(bulan) && o.KodeBudidaya == "00").Sum(o => o.PKBKgLateks+o.PKBKgLump) / LuasAreal;
                }
                else
                {
                    double avgProtas = 0;
                    for (int i = 0; i < strTahunTanam.Length; i++)
                    {
                        avgProtas += (double)(CRUDStandardProduktivitasKaret.CRUD.Get1Record(int.Parse(tahun) - int.Parse(strTahunTanam[i].ToString())).TonPerHa / 12);
                    }
                    RKAPProduksi = (avgProtas / strTahunTanam.Length) * int.Parse(pctBalai) / 100;
                }

                if (LuasAreal>0)
                {
                    
                    m.Value3 = LuasAreal;
                    m.Value1 = RealisasiProduksi;
                    m.Value2 = RKAPProduksi;

                    if (RKAPProduksi>0)
                    {
                        if (warna == "")
                        {
                            if (RealisasiProduksi / RKAPProduksi < 0.9) m.Warna = "red";
                            else if (RealisasiProduksi / RKAPProduksi >= 1) m.Warna = "green";
                            else m.Warna = "yellow";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        blok.Add(m);

                    }
                    else
                    {
                        if (warna == "")
                        {
                            m.Warna = "black";
                        }
                        else
                        {
                            m.Warna = warna;
                        }
                        blok.Add(m);
                    }
                }
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