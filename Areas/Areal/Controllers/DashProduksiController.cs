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
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ptpn8.Areas.Areal.Controllers
{
    
    public class DashProduksiController : Controller
    {
        // GET: Areal/DashProduksi
        [Authorize]
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Dashboard Produksi";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult qtyProduksi([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (budidaya == "" ? true : m.KodeBudidaya == budidaya) &&
                           (tahun == "" ? true : m.Tahun== int.Parse(tahun)) &&
                           (bulan == "" ? true : m.Bulan<= int.Parse(bulan))
                     group m by new
                     {
                         g1 = m.KodeKebun,
                         g2 = m.KodeBudidaya,
                     }
                    into g
                     select new BiayaProduksiperBudidaya
                     {
                         KodeKebun = g.Key.g1,
                         NamaKebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                         NamaWilayah = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                         KebunId = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                         KodeBudidaya = g.Key.g2,
                         NamaBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g2) == null ? "" : CRUDBudidaya.CRUD.Get1Record(g.Key.g2).Nama,
                         LuasAreal = CRUDBlokRealisasi.CRUD.getLuasArealperKebun(g.Key.g1, g.Key.g2, "1.1"),

                         ProduksiSendiriMurni_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                         ProduksiPembelianAntarKebun_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRealisasi),
                         ProduksiPenjualanAntarKebun_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRealisasi),
                         ProduksiLancuranTBM_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRealisasi),
                         ProduksiPihak3_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRealisasi),
                         JumlahProduksi_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI").Sum(o => o.JumlahRealisasi),

                         ProduksiSendiriMurni_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                         ProduksiPembelianAntarKebun_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRealisasi),
                         ProduksiPenjualanAntarKebun_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRealisasi),
                         ProduksiLancuranTBM_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRealisasi),
                         ProduksiPihak3_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRealisasi),
                         JumlahProduksi_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI").Sum(o => o.JumlahRealisasi),

                         ProduksiSendiriMurni_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                         ProduksiPembelianAntarKebun_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRKAP),
                         ProduksiPenjualanAntarKebun_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRKAP),
                         ProduksiLancuranTBM_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRKAP),
                         ProduksiPihak3_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRKAP),
                         JumlahProduksi_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI").Sum(o => o.JumlahRKAP),

                         ProduksiSendiriMurni_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                         ProduksiPembelianAntarKebun_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRKAP),
                         ProduksiPenjualanAntarKebun_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRKAP),
                         ProduksiLancuranTBM_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRKAP),
                         ProduksiPihak3_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRKAP),
                         JumlahProduksi_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.Variable == "JADI").Sum(o => o.JumlahRKAP),
                     };

            return Json(model.OrderBy(o => o.NamaKebun).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult ChartTotalProduksiBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                        group m by new
                        {
                            //g1 = m.Bulan,
                            //g2 = m.Tahun,
                            g3 = m.KodeBudidaya,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {
                            //Bulan = g.Key.g1,
                            //Tahun = g.Key.g2,
                            Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI").Sum(o => o.JumlahRKAP),

                            //Sawit = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya=="01").Sum(o => o.JumlahRealisasi),
                            //Gutta_Perca = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "08").Sum(o => o.JumlahRealisasi),
                            //Alpukat = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "34").Sum(o => o.JumlahRealisasi),
                            //Jambu_Air = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "35").Sum(o => o.JumlahRealisasi),
                            //Sirsak = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "40").Sum(o => o.JumlahRealisasi),
                            //KKE = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "29").Sum(o => o.JumlahRealisasi),
                            //Teh = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "02").Sum(o => o.JumlahRealisasi),
                            //Durian = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "42").Sum(o => o.JumlahRealisasi),
                            //JambuBatu = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "39").Sum(o => o.JumlahRealisasi),
                            //Lainlain = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "99").Sum(o => o.JumlahRealisasi),
                            //Kelapa = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "07").Sum(o => o.JumlahRealisasi),
                            //Kakao = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "04").Sum(o => o.JumlahRealisasi),
                            //BuahNaga = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "41").Sum(o => o.JumlahRealisasi),
                            //Kina = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "03").Sum(o => o.JumlahRealisasi),
                            //Kopi = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "36").Sum(o => o.JumlahRealisasi),
                            //Agrowisata = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "12").Sum(o => o.JumlahRealisasi),
                            //Jeruk = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "37").Sum(o => o.JumlahRealisasi),
                            //Pisang = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "31").Sum(o => o.JumlahRealisasi),
                            //Nanas = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "32").Sum(o => o.JumlahRealisasi),
                            //Salak = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "33").Sum(o => o.JumlahRealisasi),
                            //Karet = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "00").Sum(o => o.JumlahRealisasi),
                            //Belimbing = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "38").Sum(o => o.JumlahRealisasi),
                            //Manggis = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "43").Sum(o => o.JumlahRealisasi),
                            //Pepaya = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "30").Sum(o => o.JumlahRealisasi),
                            //Lengkeng = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "44").Sum(o => o.JumlahRealisasi),


                            //Sawit_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "01").Sum(o => o.JumlahRKAP),
                            //Gutta_Perca_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "08").Sum(o => o.JumlahRKAP),
                            //Alpukat_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "34").Sum(o => o.JumlahRKAP),
                            //Jambu_Air_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "35").Sum(o => o.JumlahRKAP),
                            //Sirsak_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "40").Sum(o => o.JumlahRKAP),
                            //KKE_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "29").Sum(o => o.JumlahRKAP),
                            //Teh_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "02").Sum(o => o.JumlahRKAP),
                            //Durian_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "42").Sum(o => o.JumlahRKAP),
                            //JambuBatu_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "39").Sum(o => o.JumlahRKAP),
                            //Lainlain_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "99").Sum(o => o.JumlahRKAP),
                            //Kelapa_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "07").Sum(o => o.JumlahRKAP),
                            //Kakao_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "04").Sum(o => o.JumlahRKAP),
                            //BuahNaga_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "41").Sum(o => o.JumlahRKAP),
                            //Kina_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "03").Sum(o => o.JumlahRKAP),
                            //Kopi_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "36").Sum(o => o.JumlahRKAP),
                            //Agrowisata_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "12").Sum(o => o.JumlahRKAP),
                            //Jeruk_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "37").Sum(o => o.JumlahRKAP),
                            //Pisang_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "31").Sum(o => o.JumlahRKAP),
                            //Nanas_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "32").Sum(o => o.JumlahRKAP),
                            //Salak_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "33").Sum(o => o.JumlahRKAP),
                            //Karet_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "00").Sum(o => o.JumlahRKAP),
                            //Belimbing_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "38").Sum(o => o.JumlahRKAP),
                            //Manggis_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "43").Sum(o => o.JumlahRKAP),
                            //Pepaya_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "30").Sum(o => o.JumlahRKAP),
                            //Lengkeng_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "44").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o=>o.RealisasiJumlahProduksi+o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                        group m by new
                        {
                            //g1 = m.Bulan,
                            //g2 = m.Tahun,
                            g3 = m.KodeBudidaya,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {
                            //Bulan = g.Key.g1,
                            //Tahun = g.Key.g2,
                            Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI").Sum(o => o.JumlahRKAP),

                            //Sawit = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya=="01").Sum(o => o.JumlahRealisasi),
                            //Gutta_Perca = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "08").Sum(o => o.JumlahRealisasi),
                            //Alpukat = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "34").Sum(o => o.JumlahRealisasi),
                            //Jambu_Air = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "35").Sum(o => o.JumlahRealisasi),
                            //Sirsak = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "40").Sum(o => o.JumlahRealisasi),
                            //KKE = (int) g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "29").Sum(o => o.JumlahRealisasi),
                            //Teh = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "02").Sum(o => o.JumlahRealisasi),
                            //Durian = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "42").Sum(o => o.JumlahRealisasi),
                            //JambuBatu = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "39").Sum(o => o.JumlahRealisasi),
                            //Lainlain = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "99").Sum(o => o.JumlahRealisasi),
                            //Kelapa = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "07").Sum(o => o.JumlahRealisasi),
                            //Kakao = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "04").Sum(o => o.JumlahRealisasi),
                            //BuahNaga = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "41").Sum(o => o.JumlahRealisasi),
                            //Kina = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "03").Sum(o => o.JumlahRealisasi),
                            //Kopi = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "36").Sum(o => o.JumlahRealisasi),
                            //Agrowisata = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "12").Sum(o => o.JumlahRealisasi),
                            //Jeruk = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "37").Sum(o => o.JumlahRealisasi),
                            //Pisang = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "31").Sum(o => o.JumlahRealisasi),
                            //Nanas = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "32").Sum(o => o.JumlahRealisasi),
                            //Salak = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "33").Sum(o => o.JumlahRealisasi),
                            //Karet = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "00").Sum(o => o.JumlahRealisasi),
                            //Belimbing = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "38").Sum(o => o.JumlahRealisasi),
                            //Manggis = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "43").Sum(o => o.JumlahRealisasi),
                            //Pepaya = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "30").Sum(o => o.JumlahRealisasi),
                            //Lengkeng = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "44").Sum(o => o.JumlahRealisasi),


                            //Sawit_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "01").Sum(o => o.JumlahRKAP),
                            //Gutta_Perca_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "08").Sum(o => o.JumlahRKAP),
                            //Alpukat_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "34").Sum(o => o.JumlahRKAP),
                            //Jambu_Air_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "35").Sum(o => o.JumlahRKAP),
                            //Sirsak_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "40").Sum(o => o.JumlahRKAP),
                            //KKE_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "29").Sum(o => o.JumlahRKAP),
                            //Teh_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "02").Sum(o => o.JumlahRKAP),
                            //Durian_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "42").Sum(o => o.JumlahRKAP),
                            //JambuBatu_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "39").Sum(o => o.JumlahRKAP),
                            //Lainlain_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "99").Sum(o => o.JumlahRKAP),
                            //Kelapa_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "07").Sum(o => o.JumlahRKAP),
                            //Kakao_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "04").Sum(o => o.JumlahRKAP),
                            //BuahNaga_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "41").Sum(o => o.JumlahRKAP),
                            //Kina_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "03").Sum(o => o.JumlahRKAP),
                            //Kopi_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "36").Sum(o => o.JumlahRKAP),
                            //Agrowisata_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "12").Sum(o => o.JumlahRKAP),
                            //Jeruk_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "37").Sum(o => o.JumlahRKAP),
                            //Pisang_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "31").Sum(o => o.JumlahRKAP),
                            //Nanas_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "32").Sum(o => o.JumlahRKAP),
                            //Salak_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "33").Sum(o => o.JumlahRKAP),
                            //Karet_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "00").Sum(o => o.JumlahRKAP),
                            //Belimbing_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "38").Sum(o => o.JumlahRKAP),
                            //Manggis_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "43").Sum(o => o.JumlahRKAP),
                            //Pepaya_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "30").Sum(o => o.JumlahRKAP),
                            //Lengkeng_RKAP = (int)g.Where(o => o.Bulan == int.Parse(bulan) && o.Variable == "JADI" && o.KodeBudidaya == "44").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiKaretBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) && m.KodeBudidaya=="00"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {
                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiKaretSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) && m.KodeBudidaya=="00"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {

                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiSawitBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) && m.KodeBudidaya == "01"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {
                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiSawitSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) && m.KodeBudidaya == "01"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {

                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening=="X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiTehBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) && m.KodeBudidaya == "02"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {
                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }

        [Authorize]
        public ActionResult ChartTotalProduksiTehSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            var model = from m in CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya()
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) && m.KodeBudidaya == "02"
                        group m by new
                        {
                            g3 = m.KodeKebun,
                        }
                        into g
                        select new ChartProduksiPerBudidaya
                        {

                            Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama,
                            RealisasiJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                            RKAPJumlahProduksi = (int)g.Where(o => o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                        };
            return Json(model.Where(o => o.RealisasiJumlahProduksi + o.RKAPJumlahProduksi > 0).ToList());
        }


    }
}