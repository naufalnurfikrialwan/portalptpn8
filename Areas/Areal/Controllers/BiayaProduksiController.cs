using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Areal.Models;
using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ptpn8.Areas.Areal.Controllers
{
    public class BiayaProduksiController : Controller
    {
        [Authorize]
        public ActionResult IndexBiayaProduksiKaret()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Biaya Produksi Karet";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult IndexBiayaProduksiSawit()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Biaya Produksi Sawit";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;

            return View();
        }
        [Authorize]
        public ActionResult IndexBiayaProduksiTeh()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Biaya Produksi Teh";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }
        [Authorize]
        public ActionResult IndexBiayaProduksiLainnya()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Biaya Produksi Lainnya";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;

            return View();
        }

        [HttpPost]
        [Authorize]
        public ActionResult biayaProduksi([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string cariText = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunMemorial.CRUD.TahunBuku = int.Parse(tahun);

            var model = new List<BiayaProduksiperBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                     where (budidaya == "" ? (m.KodeBudidaya!="00" && m.KodeBudidaya!="01" && m.KodeBudidaya!="02") : m.KodeBudidaya == budidaya) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602" 
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605" 
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" || m.KodeRekening.Substring(0, 3).ToString() == "608" 
                                || m.KodeRekening.Substring(0, 3).ToString() == "609" || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                             (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                             (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                     group m by new
                     {
                         g1 = m.KodeKebun,
                         g2 = m.KodeBudidaya
                     }
                    into g
                     select new BiayaProduksiperBudidaya
                     {
                         KodeKebun = g.Key.g1,
                         KodeBudidaya = g.Key.g2,

                         ProduksiSendiriMurni_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                         ProduksiPembelianAntarKebun_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRealisasi),
                         ProduksiPenjualanAntarKebun_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRealisasi),
                         ProduksiLancuranTBM_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRealisasi),
                         ProduksiPihak3_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRealisasi),
                         JumlahProduksi_BI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI").Sum(o => o.JumlahRealisasi),

                         BiayaStaf_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "600").Sum(o => o.Nilai),
                         BiayaPemeliharaanTM_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "601").Sum(o => o.Nilai),
                         BiayaPanen_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "602" && o.KodeRekening.Substring(0, 5) != "60214").Sum(o => o.Nilai),
                         BiayaPengangkutan_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 5) == "60214").Sum(o => o.Nilai),
                         BiayaPenyusutanTanaman_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900001").Sum(o => o.Nilai),
                         BiayaPenyusutanNonTanaman_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900002").Sum(o => o.Nilai),
                         BiayaPengolahan_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && (o.KodeRekening.Substring(0, 3) == "603")|| (o.KodeRekening.Substring(0, 3) == "604")|| (o.KodeRekening.Substring(0, 3) == "605")|| (o.KodeRekening.Substring(0, 3) == "606")|| (o.KodeRekening.Substring(0, 3) == "607")).Sum(o => o.Nilai),
                         BiayaPembelianHasilTanaman_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "608").Sum(o => o.Nilai),
                         BiayaPenjualanHasilTanaman_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "609").Sum(o => o.Nilai),
                         JumlahBiayaProduksi_BI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2).Sum(o => o.Nilai),

                         ProduksiSendiriMurni_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi),
                         ProduksiPembelianAntarKebun_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRealisasi),
                         ProduksiPenjualanAntarKebun_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRealisasi),
                         ProduksiLancuranTBM_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRealisasi),
                         ProduksiPihak3_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRealisasi),
                         JumlahProduksi_SBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI").Sum(o => o.JumlahRealisasi),

                         BiayaStaf_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "600").Sum(o => o.Nilai),
                         BiayaPemeliharaanTM_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "601").Sum(o => o.Nilai),
                         BiayaPanen_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "602" && o.KodeRekening.Substring(0, 5) != "60214").Sum(o => o.Nilai),
                         BiayaPengangkutan_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 5) == "60214").Sum(o => o.Nilai),
                         BiayaPenyusutanTanaman_SBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900001").Sum(o => o.Nilai),
                         BiayaPenyusutanNonTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900002").Sum(o => o.Nilai),
                         BiayaPengolahan_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && (o.KodeRekening.Substring(0, 3) == "603") || (o.KodeRekening.Substring(0, 3) == "604") || (o.KodeRekening.Substring(0, 3) == "605") || (o.KodeRekening.Substring(0, 3) == "606") || (o.KodeRekening.Substring(0, 3) == "607")).Sum(o => o.Nilai),
                         BiayaPembelianHasilTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "608").Sum(o => o.Nilai),
                         BiayaPenjualanHasilTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "609").Sum(o => o.Nilai),
                         JumlahBiayaProduksi_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2).Sum(o => o.Nilai),

                         ProduksiSendiriMurni_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                         ProduksiPembelianAntarKebun_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRKAP),
                         ProduksiPenjualanAntarKebun_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRKAP),
                         ProduksiLancuranTBM_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRKAP),
                         ProduksiPihak3_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRKAP),
                         JumlahProduksi_RBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI").Sum(o => o.JumlahRKAP),

                         ProduksiSendiriMurni_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP),
                         ProduksiPembelianAntarKebun_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080400").Sum(o => o.JumlahRKAP),
                         ProduksiPenjualanAntarKebun_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6090400").Sum(o => o.JumlahRKAP),
                         ProduksiLancuranTBM_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080500").Sum(o => o.JumlahRKAP),
                         ProduksiPihak3_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI" && o.KodeRekening == "6080402").Sum(o => o.JumlahRKAP),
                         JumlahProduksi_RSBI = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI") == null ? 0 : CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.Variable == "JADI").Sum(o => o.JumlahRKAP),

                     }).ToList();


            //model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
            //                where (budidaya == "" ? (m.KodeBudidaya != "00" && m.KodeBudidaya != "01" && m.KodeBudidaya != "02") : m.KodeBudidaya == budidaya) &&
            //                      (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" || m.KodeRekening.Substring(0, 3).ToString() == "608"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "609" || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
            //                        (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
            //                        (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
            //                group m by new
            //                {
            //                    g1 = m.KodeKebun,
            //                    g2 = m.KodeBudidaya
            //                }
            //        into g
            //                select new BiayaProduksiperBudidaya
            //                {
            //                    KodeKebun = g.Key.g1,
            //                    KodeBudidaya = g.Key.g2,

            //                    BiayaStaf_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "600").Sum(o => o.Nilai),
            //                    BiayaPemeliharaanTM_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "601").Sum(o => o.Nilai),
            //                    BiayaPanen_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "602" && o.KodeRekening.Substring(0, 5) != "60214").Sum(o => o.Nilai),
            //                    BiayaPengangkutan_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 5) == "60214").Sum(o => o.Nilai),
            //                    BiayaPenyusutanTanaman_SBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900001").Sum(o => o.Nilai),
            //                    BiayaPenyusutanNonTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900002").Sum(o => o.Nilai),
            //                    BiayaPengolahan_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && (o.KodeRekening.Substring(0, 3) == "603") || (o.KodeRekening.Substring(0, 3) == "604") || (o.KodeRekening.Substring(0, 3) == "605") || (o.KodeRekening.Substring(0, 3) == "606") || (o.KodeRekening.Substring(0, 3) == "607")).Sum(o => o.Nilai),
            //                    BiayaPembelianHasilTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "608").Sum(o => o.Nilai),
            //                    BiayaPenjualanHasilTanaman_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "609").Sum(o => o.Nilai),
            //                    JumlahBiayaProduksi_SBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2).Sum(o => o.Nilai),

            //                }).ToList());

            //model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
            //                where (budidaya == "" ? (m.KodeBudidaya != "00" && m.KodeBudidaya != "01" && m.KodeBudidaya != "02") : m.KodeBudidaya == budidaya) &&
            //                      (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" || m.KodeRekening.Substring(0, 3).ToString() == "608"
            //                        || m.KodeRekening.Substring(0, 3).ToString() == "609" || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
            //                        (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
            //                        (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
            //                group m by new
            //                {
            //                    g1 = m.KodeKebun,
            //                    g2 = m.KodeBudidaya
            //                }
            //        into g
            //                select new BiayaProduksiperBudidaya
            //                {
            //                    KodeKebun = g.Key.g1,
            //                    KodeBudidaya = g.Key.g2,

            //                    BiayaStaf_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "600").Sum(o => o.NilaiRKAP),
            //                    BiayaPemeliharaanTM_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "601").Sum(o => o.NilaiRKAP),
            //                    BiayaPanen_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "602" && o.KodeRekening.Substring(0, 5) != "60214").Sum(o => o.NilaiRKAP),
            //                    BiayaPengangkutan_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 5) == "60214").Sum(o => o.NilaiRKAP),
            //                    BiayaPenyusutanTanaman_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900001").Sum(o => o.NilaiRKAP),
            //                    BiayaPenyusutanNonTanaman_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900002").Sum(o => o.NilaiRKAP),
            //                    BiayaPengolahan_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && (o.KodeRekening.Substring(0, 3) == "603") || (o.KodeRekening.Substring(0, 3) == "604") || (o.KodeRekening.Substring(0, 3) == "605") || (o.KodeRekening.Substring(0, 3) == "606") || (o.KodeRekening.Substring(0, 3) == "607")).Sum(o => o.NilaiRKAP),
            //                    BiayaPembelianHasilTanaman_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "608").Sum(o => o.NilaiRKAP),
            //                    BiayaPenjualanHasilTanaman_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "609").Sum(o => o.NilaiRKAP),
            //                    JumlahBiayaProduksi_RBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2).Sum(o => o.NilaiRKAP),

            //                    BiayaStaf_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "600").Sum(o => o.NilaiRKAP),
            //                    BiayaPemeliharaanTM_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "601").Sum(o => o.NilaiRKAP),
            //                    BiayaPanen_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "602" && o.KodeRekening.Substring(0, 5) != "60214").Sum(o => o.NilaiRKAP),
            //                    BiayaPengangkutan_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 5) == "60214").Sum(o => o.NilaiRKAP),
            //                    BiayaPenyusutanTanaman_RSBI = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900001").Sum(o => o.NilaiRKAP),
            //                    BiayaPenyusutanNonTanaman_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 7) == "4900002").Sum(o => o.NilaiRKAP),
            //                    BiayaPengolahan_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && (o.KodeRekening.Substring(0, 3) == "603") || (o.KodeRekening.Substring(0, 3) == "604") || (o.KodeRekening.Substring(0, 3) == "605") || (o.KodeRekening.Substring(0, 3) == "606") || (o.KodeRekening.Substring(0, 3) == "607")).Sum(o => o.NilaiRKAP),
            //                    BiayaPembelianHasilTanaman_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "608").Sum(o => o.NilaiRKAP),
            //                    BiayaPenjualanHasilTanaman_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2 && o.KodeRekening.Substring(0, 3) == "609").Sum(o => o.NilaiRKAP),
            //                    JumlahBiayaProduksi_RSBI = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == g.Key.g1 && o.KodeBudidaya == g.Key.g2).Sum(o => o.NilaiRKAP),

            //                }).ToList());

            var result = from m in model
                         group m by new
                         {
                             g1 = m.KodeKebun,
                             g2 = m.KodeBudidaya
                         }
                         into g
                         select new BiayaProduksiperBudidaya {
                             KodeKebun = g.Key.g1,
                             KodeBudidaya = g.Key.g2,
                             NamaKebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                             NamaWilayah = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                             KebunId = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                             NamaBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g2) == null ? "" : CRUDBudidaya.CRUD.Get1Record(g.Key.g2).Nama,
                             LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1,g.Key.g2,"TM"), // CRUDBlokRealisasi.CRUD.getLuasArealperKebun(g.Key.g1, g.Key.g2, "1.1"),

                             ProduksiSendiriMurni_BI = g.Sum(o => o.ProduksiSendiriMurni_BI),
                             ProduksiPembelianAntarKebun_BI = g.Sum(o => o.ProduksiPembelianAntarKebun_BI),
                             ProduksiPenjualanAntarKebun_BI = g.Sum(o => o.ProduksiPenjualanAntarKebun_BI),
                             ProduksiLancuranTBM_BI = g.Sum(o => o.ProduksiLancuranTBM_BI),
                             ProduksiPihak3_BI = g.Sum(o => o.ProduksiPihak3_BI),
                             JumlahProduksi_BI = g.Sum(o => o.JumlahProduksi_BI),

                             BiayaStaf_BI = g.Sum(o => o.BiayaStaf_BI),
                             BiayaPemeliharaanTM_BI = g.Sum(o => o.BiayaPemeliharaanTM_BI),
                             BiayaPanen_BI = g.Sum(o => o.BiayaPanen_BI),
                             BiayaPengangkutan_BI = g.Sum(o => o.BiayaPengangkutan_BI),
                             BiayaPenyusutanTanaman_BI = g.Sum(o => o.BiayaPenyusutanTanaman_BI),
                             BiayaPenyusutanNonTanaman_BI = g.Sum(o => o.BiayaPenyusutanNonTanaman_BI),
                             BiayaPengolahan_BI = g.Sum(o => o.BiayaPengolahan_BI),
                             BiayaPembelianHasilTanaman_BI = g.Sum(o => o.BiayaPembelianHasilTanaman_BI),
                             BiayaPenjualanHasilTanaman_BI = g.Sum(o => o.BiayaPenjualanHasilTanaman_BI),
                             JumlahBiayaProduksi_BI = g.Sum(o => o.JumlahBiayaProduksi_BI),

                             ProduksiSendiriMurni_SBI = g.Sum(o => o.ProduksiSendiriMurni_SBI),
                             ProduksiPembelianAntarKebun_SBI = g.Sum(o => o.ProduksiPembelianAntarKebun_SBI),
                             ProduksiPenjualanAntarKebun_SBI = g.Sum(o => o.ProduksiPenjualanAntarKebun_SBI),
                             ProduksiLancuranTBM_SBI = g.Sum(o => o.ProduksiLancuranTBM_SBI),
                             ProduksiPihak3_SBI = g.Sum(o => o.ProduksiPihak3_SBI),
                             JumlahProduksi_SBI = g.Sum(o => o.JumlahProduksi_SBI),

                             BiayaStaf_SBI = g.Sum(o => o.BiayaStaf_SBI),
                             BiayaPemeliharaanTM_SBI = g.Sum(o => o.BiayaPemeliharaanTM_SBI),
                             BiayaPanen_SBI = g.Sum(o => o.BiayaPanen_SBI),
                             BiayaPengangkutan_SBI = g.Sum(o => o.BiayaPengangkutan_SBI),
                             BiayaPenyusutanTanaman_SBI = g.Sum(o => o.BiayaPenyusutanTanaman_SBI),
                             BiayaPenyusutanNonTanaman_SBI = g.Sum(o => o.BiayaPenyusutanNonTanaman_SBI),
                             BiayaPengolahan_SBI = g.Sum(o => o.BiayaPengolahan_SBI),
                             BiayaPembelianHasilTanaman_SBI = g.Sum(o => o.BiayaPembelianHasilTanaman_SBI),
                             BiayaPenjualanHasilTanaman_SBI = g.Sum(o => o.BiayaPenjualanHasilTanaman_SBI),
                             JumlahBiayaProduksi_SBI = g.Sum(o => o.JumlahBiayaProduksi_SBI),

                             ProduksiSendiriMurni_RBI = g.Sum(o => o.ProduksiSendiriMurni_RBI),
                             ProduksiPembelianAntarKebun_RBI = g.Sum(o => o.ProduksiPembelianAntarKebun_RBI),
                             ProduksiPenjualanAntarKebun_RBI = g.Sum(o => o.ProduksiPenjualanAntarKebun_RBI),
                             ProduksiLancuranTBM_RBI = g.Sum(o => o.ProduksiLancuranTBM_RBI),
                             ProduksiPihak3_RBI = g.Sum(o => o.ProduksiPihak3_RBI),
                             JumlahProduksi_RBI = g.Sum(o => o.JumlahProduksi_RBI),

                             BiayaStaf_RBI = g.Sum(o => o.BiayaStaf_RBI),
                             BiayaPemeliharaanTM_RBI = g.Sum(o => o.BiayaPemeliharaanTM_RBI),
                             BiayaPanen_RBI = g.Sum(o => o.BiayaPanen_RBI),
                             BiayaPengangkutan_RBI = g.Sum(o => o.BiayaPengangkutan_RBI),
                             BiayaPenyusutanTanaman_RBI = g.Sum(o => o.BiayaPenyusutanTanaman_RBI),
                             BiayaPenyusutanNonTanaman_RBI = g.Sum(o => o.BiayaPenyusutanNonTanaman_RBI),
                             BiayaPengolahan_RBI = g.Sum(o => o.BiayaPengolahan_RBI),
                             BiayaPembelianHasilTanaman_RBI = g.Sum(o => o.BiayaPembelianHasilTanaman_RBI),
                             BiayaPenjualanHasilTanaman_RBI = g.Sum(o => o.BiayaPenjualanHasilTanaman_RBI),
                             JumlahBiayaProduksi_RBI = g.Sum(o => o.JumlahBiayaProduksi_RBI),

                             ProduksiSendiriMurni_RSBI = g.Sum(o=>o.ProduksiSendiriMurni_RSBI),
                             ProduksiPembelianAntarKebun_RSBI = g.Sum(o => o.ProduksiPembelianAntarKebun_RSBI),
                             ProduksiPenjualanAntarKebun_RSBI = g.Sum(o => o.ProduksiPenjualanAntarKebun_RSBI),
                             ProduksiLancuranTBM_RSBI = g.Sum(o => o.ProduksiLancuranTBM_RSBI),
                             ProduksiPihak3_RSBI = g.Sum(o => o.ProduksiPihak3_RSBI),
                             JumlahProduksi_RSBI = g.Sum(o => o.JumlahProduksi_RSBI),

                             BiayaStaf_RSBI = g.Sum(o => o.BiayaStaf_RSBI),
                             BiayaPemeliharaanTM_RSBI = g.Sum(o => o.BiayaPemeliharaanTM_RSBI),
                             BiayaPanen_RSBI = g.Sum(o => o.BiayaPanen_RSBI),
                             BiayaPengangkutan_RSBI = g.Sum(o => o.BiayaPengangkutan_RSBI),
                             BiayaPenyusutanTanaman_RSBI = g.Sum(o => o.BiayaPenyusutanTanaman_RSBI),
                             BiayaPenyusutanNonTanaman_RSBI = g.Sum(o => o.BiayaPenyusutanNonTanaman_RSBI),
                             BiayaPengolahan_RSBI = g.Sum(o => o.BiayaPengolahan_RSBI),
                             BiayaPembelianHasilTanaman_RSBI = g.Sum(o => o.BiayaPembelianHasilTanaman_RSBI),
                             BiayaPenjualanHasilTanaman_RSBI = g.Sum(o => o.BiayaPenjualanHasilTanaman_RSBI),
                             JumlahBiayaProduksi_RSBI = g.Sum(o => o.JumlahBiayaProduksi_RSBI),
                         };

            return Json(result.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult KebunGeoJSon(string Id= "", string budidaya="", string tahun="", string bulan="")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh" || budidaya == "") budidaya = ""; else budidaya = CRUDBudidaya.CRUD.GetAllRecord().Where(o => o.Nama == budidaya).Select(o => o.kd_bud).FirstOrDefault();
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var model = from m in CRUDKebun.CRUD.GetAllRecord()
                           join n in CRUDKebunPeta.CRUD.GetAllRecord().Where(o => o.Peta != null) on m.KebunId equals n.KebunId
                           select new PetaView
                           {
                               Peta = n.Peta.ToString(),
                               Id = m.KebunId,
                               Nama = m.Nama,
                               KodeKebun = m.kd_kbn
                           };

            List<PetaView> kebun = new List<PetaView>();
            double? RealisasiProduksi;
            double? RKAPProduksi;
            foreach (var m in model)
            {
                
                RealisasiProduksi = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == m.KodeKebun && (budidaya == "" ? true : o.KodeBudidaya == budidaya) && o.Variable == "JADI" && (o.KodeRekening == "X" || o.KodeRekening == "6080500")) == null ? 0 : 
                                    CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == m.KodeKebun && (budidaya == "" ? true : o.KodeBudidaya == budidaya) && o.Variable == "JADI" && (o.KodeRekening == "X" || o.KodeRekening == "6080500")).Sum(o => o.JumlahRealisasi);
                RKAPProduksi = CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == m.KodeKebun && (budidaya == "" ? true : o.KodeBudidaya == budidaya) && o.Variable == "JADI" && (o.KodeRekening == "X" || o.KodeRekening == "6080500")) == null ? 0 :
                                    CRUDAkunProduksiHP.CRUD.GetProduksiperBudidaya().Where(o => o.Bulan <= int.Parse(bulan) && o.KodeKebun == m.KodeKebun && (budidaya == "" ? true : o.KodeBudidaya == budidaya) && o.Variable == "JADI" && (o.KodeRekening == "X" || o.KodeRekening == "6080500")).Sum(o => o.JumlahRKAP);

                if (RealisasiProduksi + RKAPProduksi > 0)
                {
                    m.Value1 = RealisasiProduksi;
                    m.Value2 = RKAPProduksi;
                    if (RealisasiProduksi / RKAPProduksi < 0.9) m.Warna = "red";//"#ea171b";
                    else if (RealisasiProduksi / RKAPProduksi >= 1) m.Warna = "green";//"#10ac28";
                    else m.Warna = "yellow";//"#e0e307";
                    kebun.Add(m);
                }
                
            }

            foreach (var k in kebun)
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
                featureCollection.Add(feature);
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }

        [HttpPost]
        public ActionResult Export_Save(string img)
        {
            string senderp = "62811238499";
            string password = "";
            WhatsAppApi.WhatsApp wa = new WhatsAppApi.WhatsApp(senderp, password, "bdhendra", true);
            wa.OnConnectSuccess += () =>
            {
                Console.WriteLine("Connected");
                wa.OnLoginSuccess += (phoneNumber, data) =>
                {
                    wa.SendMessage("6282216462061", "Test...");
                };
                wa.OnLoginFailed += (data) =>
                {
                };
                wa.Login();
            };
            wa.OnConnectFailed += (ex) =>
            {
            };
            wa.Connect();

            //var fileContents = Convert.FromBase64String(base64);
            return Content("true", "application/json");
        }
    }
}



