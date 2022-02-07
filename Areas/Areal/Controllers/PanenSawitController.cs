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
    public class PanenSawitController : Controller
    {
        // GET: Areal/PanenSawit
        [Authorize]
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Panen";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult getKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string cariText = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            //if (budidaya == "Seluruh") budidaya = "";
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDGajiAbsensi.CRUD.TahunBuku = int.Parse(tahun);
            CRUDGajiAbsensi.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);

            var model = from m in CRUDGajiAbsensi.CRUD.GetRecordPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                        where 
                        //(cariText == "" ? true : m.NamaKebun.ToLower().Contains(cariText.ToLower())) &&
                            (m.KodeBudidaya == "01") &&
                            (m.KodeRekening.Substring(0, 3).ToString() == "602") &&
                            (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                            (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                        group m by new
                        {
                            group1 = m.KodeKebun,
                            group2 = m.KodeBudidaya,
                        }
                        into g
                        select new DataKegiatanKebun
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.group1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.group1).KebunId : Guid.Empty,
                            NamaKebun = CRUDKebun.CRUD.Get1Record(g.Key.group1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.group1).Nama : "",
                            NamaWilayah = CRUDKebun.CRUD.Get1Record(g.Key.group1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.group1).Wilayah.Nama : "",
                            NamaBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.group2) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.group2).Nama : "",
                            LuasAreal = CRUDBlokRealisasi.CRUD.getLuasArealperKebun(g.Key.group1, g.Key.group2, "1.1"),

                            RealisasiJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),

                            RealisasiJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8") && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),

                            RealisasiHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja),
                            RealisasiHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja),

                            RealisasiHasilKerjaLumpKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerjaLump),
                            RealisasiHasilKerjaLumpKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerjaLump),

                            RKAPJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPJumlahKaryawan),
                            RKAPJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.RKAPJumlahKaryawan),

                            RKAPJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8")).Sum(o => o.RKAPJumlahHK),

                            RKAPHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPHasilKerja),
                            RKAPHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.RKAPHasilKerja),

                            RKAPJumlahUpahKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahUpah),
                            RKAPJumlahUpahKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahUpah),

                            RKAPJumlahUpahSosial = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1").Sum(o => o.RKAPJumlahUpah),
                            RKAPJumlahUpahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPJumlahUpah),

                            PKBJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBJumlahKaryawan),
                            PKBJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.PKBJumlahKaryawan),

                            PKBJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8")).Sum(o => o.PKBJumlahHK),

                            PKBHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBHasilKerja),
                            PKBHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.PKBHasilKerja),

                            PKBJumlahUpahKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahUpah),
                            PKBJumlahUpahKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahUpah),

                            PKBJumlahUpahSosial = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1").Sum(o => o.PKBJumlahUpah),
                            PKBJumlahUpahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBJumlahUpah),

                            RealisasiJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja + o.RealisasiHasilKerjaLump),
                            RealisasiJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiBiayaLain = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiTotalBiaya = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                            RKAPJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPJumlahKaryawan),
                            RKAPJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPHasilKerja),
                            RKAPJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPJumlahUpah),

                            PKBJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBJumlahKaryawan),
                            PKBJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBHasilKerja),
                            PKBJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBJumlahUpah),

                            RealisasiSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / (25 * int.Parse(bulan))),
                            RealisasiSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja+o.RealisasiHasilKerjaLump),
                            RealisasiSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiSBIBiayaLain = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiSBITotalBiaya = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                            RKAPSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPJumlahKaryawan),
                            RKAPSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPHasilKerja),
                            RKAPSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPJumlahUpah),

                            PKBSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBJumlahKaryawan),
                            PKBSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBHasilKerja),
                            PKBSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBJumlahUpah),
                        };

            return Json(model.OrderBy(o => o.NamaKebun).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string cariText = "")
        {
            if (Id == "") return View();
            //if (budidaya == "Seluruh") budidaya = "";
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDGajiAbsensi.CRUD.TahunBuku = int.Parse(tahun);
            CRUDGajiAbsensi.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);
            string kodeKebun = CRUDKebun.CRUD.Get1Record(Guid.Parse(Id)).kd_kbn;

            var model = from m in CRUDGajiAbsensi.CRUD.GetRecordPerAfdeling(kodeKebun).Where(o => o.KodeRekening.Trim().Length == 9)
                        where (cariText == "" ? true : m.NamaKebun.ToLower().Contains(cariText.ToLower())) &&
                            (m.KodeBudidaya == "01") &&
                            (m.KodeRekening.Substring(0, 3).ToString() == "602") &&
                            (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                            (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                        group m by new
                        {
                            group1 = m.KodeAfdeling,
                            group2 = m.KodeBudidaya,
                        }
                        into g
                        select new DataKegiatanAfdeling
                        {
                            Id = CRUDAfdeling.CRUD.Get1Record(g.Key.group1) != null ? CRUDAfdeling.CRUD.Get1Record(g.Key.group1).AfdelingId : Guid.Empty,
                            NamaAfdeling = CRUDAfdeling.CRUD.Get1Record(g.Key.group1) != null ? CRUDAfdeling.CRUD.Get1Record(g.Key.group1).Nama : "",
                            NamaKebun = CRUDAfdeling.CRUD.Get1Record(g.Key.group1) != null ? CRUDAfdeling.CRUD.Get1Record(g.Key.group1).Kebun.Nama : "",
                            NamaBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.group2) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.group2).Nama : "",
                            LuasAreal = CRUDBlokRealisasi.CRUD.getLuasArealperAfdeling(g.Key.group1, g.Key.group2, "1.1"),

                            RealisasiJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),

                            RealisasiJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8") && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),

                            RealisasiHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerja),
                            RealisasiHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerja),

                            RealisasiHasilKerjaLumpKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerjaLump),
                            RealisasiHasilKerjaLumpKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerjaLump),

                            RKAPJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPJumlahKaryawan),
                            RKAPJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.RKAPJumlahKaryawan),

                            RKAPJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8")).Sum(o => o.RKAPJumlahHK),

                            RKAPHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPHasilKerja),
                            RKAPHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.RKAPHasilKerja),

                            RKAPJumlahUpahKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahUpah),
                            RKAPJumlahUpahKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.RKAPJumlahUpah),

                            RKAPJumlahUpahSosial = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1").Sum(o => o.RKAPJumlahUpah),
                            RKAPJumlahUpahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.RKAPJumlahUpah),

                            PKBJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBJumlahKaryawan),
                            PKBJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.PKBJumlahKaryawan),

                            PKBJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8")).Sum(o => o.PKBJumlahHK),

                            PKBHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBHasilKerja),
                            PKBHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL").Sum(o => o.PKBHasilKerja),

                            PKBJumlahUpahKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahUpah),
                            PKBJumlahUpahKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1").Sum(o => o.PKBJumlahUpah),

                            PKBJumlahUpahSosial = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1").Sum(o => o.PKBJumlahUpah),
                            PKBJumlahUpahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB").Sum(o => o.PKBJumlahUpah),

                            RealisasiJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja + o.RealisasiHasilKerjaLump),
                            RealisasiJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiBiayaLain = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiTotalBiaya = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                            RKAPJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPJumlahKaryawan),
                            RKAPJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPHasilKerja),
                            RKAPJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RKAPJumlahUpah),

                            PKBJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBJumlahKaryawan),
                            PKBJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBHasilKerja),
                            PKBJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.PKBJumlahUpah),

                            RealisasiSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / (25 * int.Parse(bulan))),
                            RealisasiSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja + o.RealisasiHasilKerjaLump),
                            RealisasiSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiSBIBiayaLain = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiSBITotalBiaya = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                            RKAPSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPJumlahKaryawan),
                            RKAPSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RKAPJumlahHK),
                            RKAPSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPHasilKerja),
                            RKAPSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RKAPJumlahUpah),

                            PKBSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBJumlahKaryawan),
                            PKBSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.PKBJumlahHK),
                            PKBSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBHasilKerja),
                            PKBSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.PKBJumlahUpah),
                        };

            return Json(model.OrderBy(o => o.NamaAfdeling).ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string budidaya = "", string tahun = "", string bulan = "", string cariText = "")
        {
            if (Id == "") return View();
            //if (budidaya == "Seluruh") budidaya = "";
            if (tahun == "") tahun = DateTime.Now.Year.ToString();
            if (bulan == "") bulan = DateTime.Now.Month.ToString();
            CRUDGajiAbsensi.CRUD.TahunBuku = int.Parse(tahun);
            CRUDGajiAbsensi.CRUD.BulanBuku = int.Parse(bulan);
            CRUDBlokRealisasi.CRUD.TahunBuku = int.Parse(tahun);
            string kodeAfdeling = CRUDAfdeling.CRUD.Get1Record(Guid.Parse(Id)).kd_afd;

            var model = from m in CRUDGajiAbsensi.CRUD.GetRecordPerBlok(kodeAfdeling).Where(o => o.KodeRekening.Trim().Length == 9)
                        where (cariText == "" ? true : m.NamaBlok.ToLower().Contains(cariText.ToLower())) &&
                            (m.KodeBudidaya == "01") &&
                            (m.KodeRekening.Substring(0, 3).ToString() == "602") &&
                            (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                            (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                        group m by new
                        {
                            group1 = m.KodeBlok,
                            group2 = m.KodeBudidaya,
                        }
                        into g
                        select new DataKegiatanBlok
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.group1) != null ? CRUDBlok.CRUD.Get1Record(g.Key.group1).BlokId : Guid.Empty,
                            NamaAfdeling = CRUDBlok.CRUD.Get1Record(g.Key.group1) != null ? CRUDBlok.CRUD.Get1Record(g.Key.group1).Afdeling.Nama : "",
                            NamaBlok = CRUDBlok.CRUD.Get1Record(g.Key.group1) != null ? CRUDBlok.CRUD.Get1Record(g.Key.group1).Nama : "",
                            NamaBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.group2) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.group2).Nama : "",
                            TahunTanam = string.Join(", ", g.Select(o => o.TahunTanam).ToArray().Distinct()),
                            LuasAreal = CRUDBlokRealisasi.CRUD.getLuasArealperBlok(g.Key.group1, g.Key.group2, "1.1"),

                            RealisasiJumlahKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),

                            RealisasiJumlahHKKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeAbsen == "1" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKTidakKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeAbsen != "1" && o.KodeAbsen != "7" && o.KodeAbsen != "8" && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),
                            RealisasiJumlahHKMangkir = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && (o.KodeAbsen == "7" || o.KodeAbsen == "8") && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK),

                            RealisasiHasilKerjaKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerja),
                            RealisasiHasilKerjaKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerja),

                            RealisasiHasilKerjaLumpKaryawanTetap = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KB" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerjaLump),
                            RealisasiHasilKerjaLumpKaryawanLepas = g.Where(o => o.Bulan == int.Parse(bulan)).Where(o => o.StatusKaryawan == "KL" && o.KodeRekening.Substring(0, 5) == "60203" ).Sum(o => o.RealisasiHasilKerjaLump),

                            RealisasiJumlahKaryawan = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / 25),
                            RealisasiJumlahHKKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiHasilKerja = g.Where(o => o.Bulan == int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja+o.RealisasiHasilKerjaLump),
                            RealisasiJumlahUpahKerja = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiBiayaLain = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiTotalBiaya = g.Where(o => o.Bulan == int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                            RealisasiSBIJumlahKaryawan = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiJumlahHK / (25 * int.Parse(bulan))),
                            RealisasiSBIJumlahHKKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Where(o => o.KodeAbsen != "7" && o.KodeAbsen != "8").Sum(o => o.RealisasiJumlahHK),
                            RealisasiSBIHasilKerja = g.Where(o => o.Bulan <= int.Parse(bulan) && o.KodeRekening.Substring(0, 5) == "60203").Sum(o => o.RealisasiHasilKerja+o.RealisasiHasilKerjaLump),
                            RealisasiSBIJumlahUpahKerja = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah),
                            RealisasiSBIBiayaLain = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiBiayaLain),
                            RealisasiSBITotalBiaya = g.Where(o => o.Bulan <= int.Parse(bulan)).Sum(o => o.RealisasiJumlahUpah + o.RealisasiBiayaLain),

                        };

            return Json(model.ToDataSourceResult(request));
        }

    }
}