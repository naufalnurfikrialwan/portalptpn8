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
    public class DashBiayaProduksiController : Controller
    {
        // GET: Areal/DashBiayaProduksi
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Dashboard Biaya Produksi";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            return View();
        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                        where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) &&
                              (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") 
                        group m by new
                        {
                            g1 = m.KodeBudidaya,
                        }
                       into g
                        select new ChartProduksiPerBudidaya
                        {
                            Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                            RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                            RKAPJumlahProduksi = 0
                        }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeBudidaya,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeBudidaya,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Budidaya into g
                         select new ChartProduksiPerBudidaya
                         {
                             Budidaya = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi/1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi/1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o=>o.KodeBudidaya.Length == 2)
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeBudidaya,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1)!=null? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeBudidaya,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9)
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeBudidaya,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Budidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.g1) != null ? CRUDBudidaya.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Budidaya into g
                         select new ChartProduksiPerBudidaya
                         {
                             Budidaya = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi/1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi/1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiKaretBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o=>o.KodeBudidaya=="00")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "00")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "00")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun= g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiKaretSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya.Length == 2).Where(o => o.KodeBudidaya == "00")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "00")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "00")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                                || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                                || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                                || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiSawitBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "01")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "01")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "01")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiSawitSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya.Length == 2).Where(o => o.KodeBudidaya == "01")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "01")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "01")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiTehBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "02")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan == int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "02")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "02")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan == int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

        [Authorize]
        public ActionResult ChartTotalBiayaProduksiTehSBI(string Id = "", string budidaya = "", string tahun = "", string bulan = "")
        {
            CRUDKartu.CRUD.TahunBuku = int.Parse(tahun);
            CRUDAkunRKAP.CRUD.TahunBuku = int.Parse(tahun);
            var model = new List<ChartProduksiPerBudidaya>();
            model = (from m in CRUDKartu.CRUD.GetKartuPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya.Length == 2).Where(o => o.KodeBudidaya == "02")
                     where (tahun == "" ? true : m.Tahun == int.Parse(tahun)) && (bulan == "" ? true : m.Bulan <= int.Parse(bulan)) &&
                           (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002")
                     group m by new
                     {
                         g1 = m.KodeKebun,
                     }
                       into g
                     select new ChartProduksiPerBudidaya
                     {
                         Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                         RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                         RKAPJumlahProduksi = 0
                     }).ToList();

            model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "02")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RealisasiBiayaProduksi = (float)g.Sum(o => o.Nilai),
                                RKAPJumlahProduksi = 0
                            }).ToList());

            model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o => o.KodeRekening.Trim().Length == 9).Where(o => o.KodeBudidaya == "02")
                            where (m.KodeRekening.Substring(0, 3).ToString() == "600" || m.KodeRekening.Substring(0, 3).ToString() == "601" || m.KodeRekening.Substring(0, 3).ToString() == "602"
                             || m.KodeRekening.Substring(0, 3).ToString() == "603" || m.KodeRekening.Substring(0, 3).ToString() == "604" || m.KodeRekening.Substring(0, 3).ToString() == "605"
                             || m.KodeRekening.Substring(0, 3).ToString() == "606" || m.KodeRekening.Substring(0, 3).ToString() == "607" //|| m.KodeRekening.Substring(0, 3).ToString() == "608" || m.KodeRekening.Substring(0, 3).ToString() == "609" 
                             || m.KodeRekening.Substring(0, 7).ToString() == "4900001" || m.KodeRekening.Substring(0, 7).ToString() == "4900002") &&
                                    (tahun == "" ? true : m.Tahun == int.Parse(tahun)) &&
                                    (bulan == "" ? true : m.Bulan <= int.Parse(bulan))
                            group m by new
                            {
                                g1 = m.KodeKebun,
                            }
                            into g
                            select new ChartProduksiPerBudidaya
                            {
                                Kebun = CRUDKebun.CRUD.Get1Record(g.Key.g1) != null ? CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama : "",
                                RKAPBiayaProduksi = (float)g.Sum(o => o.NilaiRKAP),
                            }).ToList());

            var result = from m in model
                         group m by m.Kebun into g
                         select new ChartProduksiPerBudidaya
                         {
                             Kebun = g.Key,
                             RealisasiBiayaProduksi = g.Sum(o => o.RealisasiBiayaProduksi / 1000000000),
                             RKAPBiayaProduksi = g.Sum(o => o.RKAPBiayaProduksi / 1000000000)
                         };
            return Json(result.Where(o => o.RealisasiBiayaProduksi + o.RKAPBiayaProduksi > 0).ToList());

        }

    }
}