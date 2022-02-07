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
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Ptpn8.Areas.Areal.Controllers
{
    public class RotasiPanenTehGuntingController : Controller
    {

        [Authorize]
        // GET: Areal/ProduktivitasTeh
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
            
            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin=="")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var kebun = from m in blok
                        group m by new { g1 = m.KodeLokasi.Substring(0, 2) } into g
                        select new RotasiPanenPerBulan
                        {
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.g1,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                            KodeBudidaya = "02",
                            NamaBudidaya = "Teh",
                            JumlahRotasi = g.Average(o => o.JumlahRotasi),
                        };
            return Json(kebun, JsonRequestBehavior.AllowGet);
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin == "")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                            && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0, 2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                            userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T01KgPucuk = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgPucuk),
                            T02KgPucuk = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgPucuk),
                            T03KgPucuk = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgPucuk),
                            T04KgPucuk = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgPucuk),
                            T05KgPucuk = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgPucuk),
                            T06KgPucuk = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgPucuk),
                            T07KgPucuk = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgPucuk),
                            T08KgPucuk = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgPucuk),
                            T09KgPucuk = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgPucuk),
                            T10KgPucuk = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgPucuk),
                            T11KgPucuk = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgPucuk),
                            T12KgPucuk = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgPucuk),
                            T13KgPucuk = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgPucuk),
                            T14KgPucuk = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgPucuk),
                            T15KgPucuk = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgPucuk),
                            T16KgPucuk = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgPucuk),
                            T17KgPucuk = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgPucuk),
                            T18KgPucuk = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgPucuk),
                            T19KgPucuk = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgPucuk),
                            T20KgPucuk = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgPucuk),
                            T21KgPucuk = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgPucuk),
                            T22KgPucuk = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgPucuk),
                            T23KgPucuk = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgPucuk),
                            T24KgPucuk = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgPucuk),
                            T25KgPucuk = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgPucuk),
                            T26KgPucuk = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgPucuk),
                            T27KgPucuk = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgPucuk),
                            T28KgPucuk = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgPucuk),
                            T29KgPucuk = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgPucuk),
                            T30KgPucuk = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgPucuk),
                            T31KgPucuk = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgPucuk),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;

                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);

                o.JumlahRotasi = jumlahRotasi;

                blok.Add(o);
            }

            var kebun = from m in blok group m by new { g1 = m.KodeLokasi.Substring(0, 2), g2 = m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g1).KebunId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.g1, "02", "TM", g.Key.g2),
                            KodeLokasi = g.Key.g1,
                            NamaLokasi = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Nama,
                            NamaInduk = CRUDKebun.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g1).Wilayah.Nama,
                            KodeBudidaya = "02",
                            NamaBudidaya = "Teh",
                            TahunTanam = g.Key.g2,
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonKebun(g.Key.g1, "02", "TM"),
                            JumlahRotasi = g.Average(o => o.JumlahRotasi),
                            TotalJumlahHK = g.Sum(o => o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                                o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                                o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                                o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                                o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                                o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK),
                            TotalLuasJelajah = g.Sum(o => o.T01LuasJelajah + o.T02LuasJelajah + o.T03LuasJelajah + o.T04LuasJelajah + o.T05LuasJelajah +
                                o.T06LuasJelajah + o.T07LuasJelajah + o.T08LuasJelajah + o.T09LuasJelajah + o.T10LuasJelajah +
                                o.T11LuasJelajah + o.T12LuasJelajah + o.T13LuasJelajah + o.T14LuasJelajah + o.T15LuasJelajah +
                                o.T16LuasJelajah + o.T17LuasJelajah + o.T18LuasJelajah + o.T19LuasJelajah + o.T20LuasJelajah +
                                o.T21LuasJelajah + o.T22LuasJelajah + o.T23LuasJelajah + o.T24LuasJelajah + o.T25LuasJelajah +
                                o.T26LuasJelajah + o.T27LuasJelajah + o.T28LuasJelajah + o.T29LuasJelajah + o.T30LuasJelajah + o.T31LuasJelajah),
                            TotalKgPucuk = g.Sum(o => o.T01KgPucuk + o.T02KgPucuk + o.T03KgPucuk + o.T04KgPucuk + o.T05KgPucuk +
                                o.T06KgPucuk + o.T07KgPucuk + o.T08KgPucuk + o.T09KgPucuk + o.T10KgPucuk +
                                o.T11KgPucuk + o.T12KgPucuk + o.T13KgPucuk + o.T14KgPucuk + o.T15KgPucuk +
                                o.T16KgPucuk + o.T17KgPucuk + o.T18KgPucuk + o.T19KgPucuk + o.T20KgPucuk +
                                o.T21KgPucuk + o.T22KgPucuk + o.T23KgPucuk + o.T24KgPucuk + o.T25KgPucuk +
                                o.T26KgPucuk + o.T27KgPucuk + o.T28KgPucuk + o.T29KgPucuk + o.T30KgPucuk + o.T31KgPucuk),
                        };

            List<RotasiPanenPerBulan> mdl = new List<RotasiPanenPerBulan>();
            foreach(var m in kebun)
            {
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2))!=null ? CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2)).Dataran : "Sedang";
                var normaRotasi = CRUDStandardProduktivitasTeh.CRUD.Get1Record(dataran).RotasiGunting / 12;
                if ((int)m.JumlahRotasi == 0)
                    m.Warna = "red";
                else if ((int)m.JumlahRotasi <= normaRotasi-1)
                    m.Warna = "yellow";
                else if ((int)m.JumlahRotasi <= normaRotasi)
                    m.Warna = "green";
                else
                    m.Warna = "blue";

                var arrTglPanen = m.TanggalPanen.Split(',').Distinct();
                m.TanggalPanen = string.Join(",", arrTglPanen.OrderBy(o=>o));
                mdl.Add(m);
            }

            return Json(mdl.OrderBy(o=>o.NamaLokasi).ToDataSourceResult(request));
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
            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin == "")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        && (userStatus.NamaLokasiKerja == "Afdeling" ? m.KodeBlok.Substring(0,2) == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).kd_kbn :
                        userStatus.NamaLokasiKerja == "Kebun" || userStatus.NamaLokasiKerja == "Bidang" ? CRUDKebun.CRUD.Get1Record(m.KodeBlok.Substring(0, 2)).WilayahId == CRUDKebun.CRUD.Get1Record(userStatus.PosisiLokasiKerjaId).WilayahId : true)
                        group m by new { m.KodeBlok} into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            //TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var model1 = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "02" && o.status == "TM")
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
            foreach (var m in model1)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal jumlahRotasi = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeKebun) != null ? CRUDKebun.CRUD.Get1Record(m.KodeKebun).Dataran : "Sedang";
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran==dataran) != null ?
                            CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran == dataran).RotasiGunting / 12 : 0);
                }

                jumlahRotasi = blok.Where(o => o.KodeLokasi.Substring(0, 2) == m.KodeKebun).Count() > 0 ? blok.Where(o => o.KodeLokasi.Substring(0, 2) == m.KodeKebun).Average(o => o.JumlahRotasi) : 0;
                if (strTahunTanam.Length > 0)
                {
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                }
                else
                {
                    normaRotasi = 0;
                }

                m.Value1 = (double)(int)jumlahRotasi;
                m.Value2 = (double)normaRotasi;

                if (warna == "")
                {
                    if ((int)jumlahRotasi == 0)
                        m.Warna = "red";
                    else if ((int)jumlahRotasi <= normaRotasi-1)
                        m.Warna = "yellow";
                    else if ((int)jumlahRotasi <= normaRotasi)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "02", "TM"));
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin=="" && o.KodeBlok.Substring(0, 2) == Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var afdeling = from m in blok
                           group m by new { g1 = m.KodeLokasi.Substring(0, 4) } into g
                           select new RotasiPanenPerBulan
                           {
                               Bulan = int.Parse(bulan),
                               Tahun = int.Parse(tahun),
                               KodeLokasi = g.Key.g1,
                               NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                               NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Kebun.Nama,
                               KodeBudidaya = "02",
                               NamaBudidaya = "Teh",
                               JumlahRotasi = g.Average(o => o.JumlahRotasi),
                           };
            return Json(afdeling, JsonRequestBehavior.AllowGet);
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 2) == Id && o.GroupMesin == "")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam} into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T01KgPucuk = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgPucuk),
                            T02KgPucuk = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgPucuk),
                            T03KgPucuk = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgPucuk),
                            T04KgPucuk = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgPucuk),
                            T05KgPucuk = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgPucuk),
                            T06KgPucuk = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgPucuk),
                            T07KgPucuk = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgPucuk),
                            T08KgPucuk = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgPucuk),
                            T09KgPucuk = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgPucuk),
                            T10KgPucuk = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgPucuk),
                            T11KgPucuk = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgPucuk),
                            T12KgPucuk = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgPucuk),
                            T13KgPucuk = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgPucuk),
                            T14KgPucuk = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgPucuk),
                            T15KgPucuk = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgPucuk),
                            T16KgPucuk = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgPucuk),
                            T17KgPucuk = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgPucuk),
                            T18KgPucuk = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgPucuk),
                            T19KgPucuk = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgPucuk),
                            T20KgPucuk = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgPucuk),
                            T21KgPucuk = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgPucuk),
                            T22KgPucuk = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgPucuk),
                            T23KgPucuk = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgPucuk),
                            T24KgPucuk = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgPucuk),
                            T25KgPucuk = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgPucuk),
                            T26KgPucuk = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgPucuk),
                            T27KgPucuk = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgPucuk),
                            T28KgPucuk = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgPucuk),
                            T29KgPucuk = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgPucuk),
                            T30KgPucuk = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgPucuk),
                            T31KgPucuk = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgPucuk),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var afdeling = from m in blok group m by new { g1 = m.KodeLokasi.Substring(0, 4), g2 = m.TahunTanam} into g
                           select new RotasiPanenPerBulan
                           {
                               Id = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).AfdelingId,
                               Bulan = int.Parse(bulan),
                               Tahun = int.Parse(tahun),
                               LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.g1, "02", "TM", g.Key.g2),
                               KodeLokasi = g.Key.g1,
                               NamaLokasi = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Nama,
                               NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g1).Kebun.Nama,
                               KodeBudidaya = "02",
                               NamaBudidaya = "Teh",
                               TahunTanam = g.Key.g2,
                               JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonAfdeling(g.Key.g1, "02", "TM"),
                               JumlahRotasi = g.Average(o => o.JumlahRotasi),
                               TotalJumlahHK = g.Sum(o => o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                                   o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                                   o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                                   o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                                   o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                                   o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK),
                               TotalLuasJelajah = g.Sum(o => o.T01LuasJelajah + o.T02LuasJelajah + o.T03LuasJelajah + o.T04LuasJelajah + o.T05LuasJelajah +
                                   o.T06LuasJelajah + o.T07LuasJelajah + o.T08LuasJelajah + o.T09LuasJelajah + o.T10LuasJelajah +
                                   o.T11LuasJelajah + o.T12LuasJelajah + o.T13LuasJelajah + o.T14LuasJelajah + o.T15LuasJelajah +
                                   o.T16LuasJelajah + o.T17LuasJelajah + o.T18LuasJelajah + o.T19LuasJelajah + o.T20LuasJelajah +
                                   o.T21LuasJelajah + o.T22LuasJelajah + o.T23LuasJelajah + o.T24LuasJelajah + o.T25LuasJelajah +
                                   o.T26LuasJelajah + o.T27LuasJelajah + o.T28LuasJelajah + o.T29LuasJelajah + o.T30LuasJelajah + o.T31LuasJelajah),
                               TotalKgPucuk = g.Sum(o => o.T01KgPucuk + o.T02KgPucuk + o.T03KgPucuk + o.T04KgPucuk + o.T05KgPucuk +
                                   o.T06KgPucuk + o.T07KgPucuk + o.T08KgPucuk + o.T09KgPucuk + o.T10KgPucuk +
                                   o.T11KgPucuk + o.T12KgPucuk + o.T13KgPucuk + o.T14KgPucuk + o.T15KgPucuk +
                                   o.T16KgPucuk + o.T17KgPucuk + o.T18KgPucuk + o.T19KgPucuk + o.T20KgPucuk +
                                   o.T21KgPucuk + o.T22KgPucuk + o.T23KgPucuk + o.T24KgPucuk + o.T25KgPucuk +
                                   o.T26KgPucuk + o.T27KgPucuk + o.T28KgPucuk + o.T29KgPucuk + o.T30KgPucuk + o.T31KgPucuk),
                           };

            List<RotasiPanenPerBulan> mdl = new List<RotasiPanenPerBulan>();
            foreach (var m in afdeling)
            {
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2)) != null ? CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2)).Dataran : "Sedang";
                var normaRotasi = CRUDStandardProduktivitasTeh.CRUD.Get1Record(dataran).RotasiGunting / 12;
                if ((int)m.JumlahRotasi == 0)
                    m.Warna = "red";
                else if ((int)m.JumlahRotasi <= normaRotasi-1)
                    m.Warna = "yellow";
                else if ((int)m.JumlahRotasi <= normaRotasi)
                    m.Warna = "green";
                else
                    m.Warna = "blue";
                mdl.Add(m);
            }

            return Json(mdl.OrderBy(o=>o.NamaLokasi).ToDataSourceResult(request));
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
            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin=="" && o.KodeBlok.Substring(0, 2) == Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok} into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            //TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var model1 = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "02" && o.status == "TM" && o.kodekebun == Id)
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
            foreach (var m in model1)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal jumlahRotasi = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeKebun.Substring(0, 2))!=null? CRUDKebun.CRUD.Get1Record(m.KodeKebun.Substring(0, 2)).Dataran : "Sedang";
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran == dataran) != null ?
                            CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran == dataran).RotasiGunting / 12 : 0);
                }

                jumlahRotasi = blok.Where(o => o.KodeLokasi.Substring(0, 4) == m.KodeKebun).Count() > 0 ? blok.Where(o => o.KodeLokasi.Substring(0, 4) == m.KodeKebun).Average(o => o.JumlahRotasi) : 0;
                if (strTahunTanam.Length > 0)
                {
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                }
                else
                {
                    normaRotasi = 0;
                }

                m.Value1 = (double)(int)jumlahRotasi;
                m.Value2 = (double)normaRotasi;

                if (warna == "")
                {
                    if ((int)jumlahRotasi == 0)
                        m.Warna = "red";
                    else if ((int)jumlahRotasi <= normaRotasi - 1)
                        m.Warna = "yellow";
                    else if ((int)jumlahRotasi <= normaRotasi)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "02", "TM"));
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin=="" && o.KodeBlok.Substring(0, 4) == Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var blok1 = from m in blok
                        group m by new { g1 = m.KodeLokasi } into g
                        select new RotasiPanenPerBulan
                        {
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            KodeLokasi = g.Key.g1,
                            NamaLokasi = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlok.CRUD.Get1Record(g.Key.g1).Nama,
                            NamaInduk = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlok.CRUD.Get1Record(g.Key.g1).Afdeling.Nama,
                            KodeBudidaya = "02",
                            NamaBudidaya = "Teh",
                            JumlahRotasi = g.Average(o => o.JumlahRotasi),
                        };
            return Json(blok1, JsonRequestBehavior.AllowGet);
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

            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.KodeBlok.Substring(0, 4) == Id && o.GroupMesin == "")
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok, m.TahunTanam} into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T01LuasJelajah = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.HaJelajah),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T02LuasJelajah = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.HaJelajah),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T03LuasJelajah = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.HaJelajah),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T04LuasJelajah = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.HaJelajah),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T05LuasJelajah = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.HaJelajah),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T06LuasJelajah = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.HaJelajah),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T07LuasJelajah = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.HaJelajah),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T08LuasJelajah = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.HaJelajah),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T09LuasJelajah = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.HaJelajah),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T10LuasJelajah = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.HaJelajah),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T11LuasJelajah = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.HaJelajah),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T12LuasJelajah = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.HaJelajah),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T13LuasJelajah = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.HaJelajah),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T14LuasJelajah = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.HaJelajah),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T15LuasJelajah = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.HaJelajah),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T16LuasJelajah = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.HaJelajah),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T17LuasJelajah = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.HaJelajah),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T18LuasJelajah = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.HaJelajah),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T19LuasJelajah = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.HaJelajah),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T20LuasJelajah = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.HaJelajah),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T21LuasJelajah = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.HaJelajah),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T22LuasJelajah = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.HaJelajah),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T23LuasJelajah = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.HaJelajah),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T24LuasJelajah = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.HaJelajah),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T25LuasJelajah = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.HaJelajah),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T26LuasJelajah = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.HaJelajah),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T27LuasJelajah = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.HaJelajah),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T28LuasJelajah = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.HaJelajah),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T29LuasJelajah = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.HaJelajah),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T30LuasJelajah = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.HaJelajah),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                            T31LuasJelajah = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.HaJelajah),
                            T01KgPucuk = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.KgPucuk),
                            T02KgPucuk = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.KgPucuk),
                            T03KgPucuk = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.KgPucuk),
                            T04KgPucuk = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.KgPucuk),
                            T05KgPucuk = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.KgPucuk),
                            T06KgPucuk = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.KgPucuk),
                            T07KgPucuk = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.KgPucuk),
                            T08KgPucuk = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.KgPucuk),
                            T09KgPucuk = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.KgPucuk),
                            T10KgPucuk = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.KgPucuk),
                            T11KgPucuk = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.KgPucuk),
                            T12KgPucuk = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.KgPucuk),
                            T13KgPucuk = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.KgPucuk),
                            T14KgPucuk = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.KgPucuk),
                            T15KgPucuk = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.KgPucuk),
                            T16KgPucuk = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.KgPucuk),
                            T17KgPucuk = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.KgPucuk),
                            T18KgPucuk = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.KgPucuk),
                            T19KgPucuk = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.KgPucuk),
                            T20KgPucuk = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.KgPucuk),
                            T21KgPucuk = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.KgPucuk),
                            T22KgPucuk = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.KgPucuk),
                            T23KgPucuk = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.KgPucuk),
                            T24KgPucuk = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.KgPucuk),
                            T25KgPucuk = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.KgPucuk),
                            T26KgPucuk = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.KgPucuk),
                            T27KgPucuk = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.KgPucuk),
                            T28KgPucuk = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.KgPucuk),
                            T29KgPucuk = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.KgPucuk),
                            T30KgPucuk = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.KgPucuk),
                            T31KgPucuk = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.KgPucuk),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                string tanggalPanen = "";
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                tanggalPanen = tanggalPanen + (o.T01JumlahHK > 0 ? "1," : "");
                tanggalPanen = tanggalPanen + (o.T02JumlahHK > 0 ? "2," : "");
                tanggalPanen = tanggalPanen + (o.T03JumlahHK > 0 ? "3," : "");
                tanggalPanen = tanggalPanen + (o.T04JumlahHK > 0 ? "4," : "");
                tanggalPanen = tanggalPanen + (o.T05JumlahHK > 0 ? "5," : "");
                tanggalPanen = tanggalPanen + (o.T06JumlahHK > 0 ? "6," : "");
                tanggalPanen = tanggalPanen + (o.T07JumlahHK > 0 ? "7," : "");
                tanggalPanen = tanggalPanen + (o.T08JumlahHK > 0 ? "8," : "");
                tanggalPanen = tanggalPanen + (o.T09JumlahHK > 0 ? "9," : "");
                tanggalPanen = tanggalPanen + (o.T10JumlahHK > 0 ? "10," : "");
                tanggalPanen = tanggalPanen + (o.T11JumlahHK > 0 ? "11," : "");
                tanggalPanen = tanggalPanen + (o.T12JumlahHK > 0 ? "12," : "");
                tanggalPanen = tanggalPanen + (o.T13JumlahHK > 0 ? "13," : "");
                tanggalPanen = tanggalPanen + (o.T14JumlahHK > 0 ? "14," : "");
                tanggalPanen = tanggalPanen + (o.T15JumlahHK > 0 ? "15," : "");
                tanggalPanen = tanggalPanen + (o.T16JumlahHK > 0 ? "16," : "");
                tanggalPanen = tanggalPanen + (o.T17JumlahHK > 0 ? "17," : "");
                tanggalPanen = tanggalPanen + (o.T18JumlahHK > 0 ? "18," : "");
                tanggalPanen = tanggalPanen + (o.T19JumlahHK > 0 ? "19," : "");
                tanggalPanen = tanggalPanen + (o.T20JumlahHK > 0 ? "20," : "");
                tanggalPanen = tanggalPanen + (o.T21JumlahHK > 0 ? "21," : "");
                tanggalPanen = tanggalPanen + (o.T22JumlahHK > 0 ? "22," : "");
                tanggalPanen = tanggalPanen + (o.T23JumlahHK > 0 ? "23," : "");
                tanggalPanen = tanggalPanen + (o.T24JumlahHK > 0 ? "24," : "");
                tanggalPanen = tanggalPanen + (o.T25JumlahHK > 0 ? "25," : "");
                tanggalPanen = tanggalPanen + (o.T26JumlahHK > 0 ? "26," : "");
                tanggalPanen = tanggalPanen + (o.T27JumlahHK > 0 ? "27," : "");
                tanggalPanen = tanggalPanen + (o.T28JumlahHK > 0 ? "28," : "");
                tanggalPanen = tanggalPanen + (o.T29JumlahHK > 0 ? "29," : "");
                tanggalPanen = tanggalPanen + (o.T30JumlahHK > 0 ? "30," : "");
                tanggalPanen = tanggalPanen + (o.T31JumlahHK > 0 ? "31," : "");

                if (tanggalPanen.Length > 0)
                    o.TanggalPanen = tanggalPanen.Substring(0, tanggalPanen.LastIndexOf(","));
                else
                    o.TanggalPanen = "";

                blok.Add(o);
            }

            var blok1 = from m in blok group m by new { g1 = m.KodeLokasi, g2 = m.TahunTanam } into g
                        select new RotasiPanenPerBulan
                        {
                            Id = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                            Bulan = int.Parse(bulan),
                            Tahun = int.Parse(tahun),
                            LuasAreal = CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.g1, "02", "TM", g.Key.g2),
                            KodeLokasi = g.Key.g1,
                            NamaLokasi = CRUDRef_Areal.CRUD.Get1Record(g.Key.g1, "02", "TM").namablok,
                            NamaInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.g1.Substring(0,4)).Nama,
                            KodeBudidaya = "02",
                            NamaBudidaya = "Teh",
                            TahunTanam = g.Key.g2,
                            JumlahPohon = CRUDRef_Areal.CRUD.HitungJumlahPohonBlok(g.Key.g1, "02", "TM"),
                            JumlahRotasi = g.Average(o => o.JumlahRotasi),
                            TanggalPanen = string.Join(",", g.Select(o => o.TanggalPanen).ToArray()),
                            TotalJumlahHK = g.Sum(o => o.T01JumlahHK + o.T02JumlahHK + o.T03JumlahHK + o.T04JumlahHK + o.T05JumlahHK +
                                o.T06JumlahHK + o.T07JumlahHK + o.T08JumlahHK + o.T09JumlahHK + o.T10JumlahHK +
                                o.T11JumlahHK + o.T12JumlahHK + o.T13JumlahHK + o.T14JumlahHK + o.T15JumlahHK +
                                o.T16JumlahHK + o.T17JumlahHK + o.T18JumlahHK + o.T19JumlahHK + o.T20JumlahHK +
                                o.T21JumlahHK + o.T22JumlahHK + o.T23JumlahHK + o.T24JumlahHK + o.T25JumlahHK +
                                o.T26JumlahHK + o.T27JumlahHK + o.T28JumlahHK + o.T29JumlahHK + o.T30JumlahHK + o.T31JumlahHK),
                            TotalLuasJelajah = g.Sum(o => o.T01LuasJelajah + o.T02LuasJelajah + o.T03LuasJelajah + o.T04LuasJelajah + o.T05LuasJelajah +
                                o.T06LuasJelajah + o.T07LuasJelajah + o.T08LuasJelajah + o.T09LuasJelajah + o.T10LuasJelajah +
                                o.T11LuasJelajah + o.T12LuasJelajah + o.T13LuasJelajah + o.T14LuasJelajah + o.T15LuasJelajah +
                                o.T16LuasJelajah + o.T17LuasJelajah + o.T18LuasJelajah + o.T19LuasJelajah + o.T20LuasJelajah +
                                o.T21LuasJelajah + o.T22LuasJelajah + o.T23LuasJelajah + o.T24LuasJelajah + o.T25LuasJelajah +
                                o.T26LuasJelajah + o.T27LuasJelajah + o.T28LuasJelajah + o.T29LuasJelajah + o.T30LuasJelajah + o.T31LuasJelajah),
                            TotalKgPucuk = g.Sum(o => o.T01KgPucuk + o.T02KgPucuk + o.T03KgPucuk + o.T04KgPucuk + o.T05KgPucuk +
                                o.T06KgPucuk + o.T07KgPucuk + o.T08KgPucuk + o.T09KgPucuk + o.T10KgPucuk +
                                o.T11KgPucuk + o.T12KgPucuk + o.T13KgPucuk + o.T14KgPucuk + o.T15KgPucuk +
                                o.T16KgPucuk + o.T17KgPucuk + o.T18KgPucuk + o.T19KgPucuk + o.T20KgPucuk +
                                o.T21KgPucuk + o.T22KgPucuk + o.T23KgPucuk + o.T24KgPucuk + o.T25KgPucuk +
                                o.T26KgPucuk + o.T27KgPucuk + o.T28KgPucuk + o.T29KgPucuk + o.T30KgPucuk + o.T31KgPucuk),
                        };
            List<RotasiPanenPerBulan> mdl = new List<RotasiPanenPerBulan>();
            foreach (var m in blok1)
            {
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2))!=null ? CRUDKebun.CRUD.Get1Record(m.KodeLokasi.Substring(0, 2)).Dataran : "Sedang";
                var normaRotasi = CRUDStandardProduktivitasTeh.CRUD.Get1Record(dataran).RotasiGunting / 12;
                if ((int)m.JumlahRotasi == 0)
                    m.Warna = "red";
                else if ((int)m.JumlahRotasi <= normaRotasi-1)
                    m.Warna = "yellow";
                else if ((int)m.JumlahRotasi <= normaRotasi)
                    m.Warna = "green";
                else
                    m.Warna = "blue";
                mdl.Add(m);
            }

            return Json(mdl.OrderBy(o => o.NamaLokasi).ToDataSourceResult(request));
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
            var model = from m in CRUDHasilPanen.CRUD.GetHasilPanenPerBlokPerTanggal().Where(o => o.KodeBudidaya == "02" && o.GroupMesin=="" && o.KodeBlok.Substring(0, 4) == Id)
                        where m.Tanggal.Month == int.Parse(bulan) && m.Tanggal.Year == int.Parse(tahun)
                        group m by new { m.KodeBlok} into g
                        select new RotasiPanenPerBulan
                        {
                            KodeLokasi = g.Key.KodeBlok,
                            //TahunTanam = g.Key.TahunTanam,
                            T01JumlahHK = g.Where(o => o.Tanggal.Day == 1).Sum(o => o.JumlahHK),
                            T02JumlahHK = g.Where(o => o.Tanggal.Day == 2).Sum(o => o.JumlahHK),
                            T03JumlahHK = g.Where(o => o.Tanggal.Day == 3).Sum(o => o.JumlahHK),
                            T04JumlahHK = g.Where(o => o.Tanggal.Day == 4).Sum(o => o.JumlahHK),
                            T05JumlahHK = g.Where(o => o.Tanggal.Day == 5).Sum(o => o.JumlahHK),
                            T06JumlahHK = g.Where(o => o.Tanggal.Day == 6).Sum(o => o.JumlahHK),
                            T07JumlahHK = g.Where(o => o.Tanggal.Day == 7).Sum(o => o.JumlahHK),
                            T08JumlahHK = g.Where(o => o.Tanggal.Day == 8).Sum(o => o.JumlahHK),
                            T09JumlahHK = g.Where(o => o.Tanggal.Day == 9).Sum(o => o.JumlahHK),
                            T10JumlahHK = g.Where(o => o.Tanggal.Day == 10).Sum(o => o.JumlahHK),
                            T11JumlahHK = g.Where(o => o.Tanggal.Day == 11).Sum(o => o.JumlahHK),
                            T12JumlahHK = g.Where(o => o.Tanggal.Day == 12).Sum(o => o.JumlahHK),
                            T13JumlahHK = g.Where(o => o.Tanggal.Day == 13).Sum(o => o.JumlahHK),
                            T14JumlahHK = g.Where(o => o.Tanggal.Day == 14).Sum(o => o.JumlahHK),
                            T15JumlahHK = g.Where(o => o.Tanggal.Day == 15).Sum(o => o.JumlahHK),
                            T16JumlahHK = g.Where(o => o.Tanggal.Day == 16).Sum(o => o.JumlahHK),
                            T17JumlahHK = g.Where(o => o.Tanggal.Day == 17).Sum(o => o.JumlahHK),
                            T18JumlahHK = g.Where(o => o.Tanggal.Day == 18).Sum(o => o.JumlahHK),
                            T19JumlahHK = g.Where(o => o.Tanggal.Day == 19).Sum(o => o.JumlahHK),
                            T20JumlahHK = g.Where(o => o.Tanggal.Day == 20).Sum(o => o.JumlahHK),
                            T21JumlahHK = g.Where(o => o.Tanggal.Day == 21).Sum(o => o.JumlahHK),
                            T22JumlahHK = g.Where(o => o.Tanggal.Day == 22).Sum(o => o.JumlahHK),
                            T23JumlahHK = g.Where(o => o.Tanggal.Day == 23).Sum(o => o.JumlahHK),
                            T24JumlahHK = g.Where(o => o.Tanggal.Day == 24).Sum(o => o.JumlahHK),
                            T25JumlahHK = g.Where(o => o.Tanggal.Day == 25).Sum(o => o.JumlahHK),
                            T26JumlahHK = g.Where(o => o.Tanggal.Day == 26).Sum(o => o.JumlahHK),
                            T27JumlahHK = g.Where(o => o.Tanggal.Day == 27).Sum(o => o.JumlahHK),
                            T28JumlahHK = g.Where(o => o.Tanggal.Day == 28).Sum(o => o.JumlahHK),
                            T29JumlahHK = g.Where(o => o.Tanggal.Day == 29).Sum(o => o.JumlahHK),
                            T30JumlahHK = g.Where(o => o.Tanggal.Day == 30).Sum(o => o.JumlahHK),
                            T31JumlahHK = g.Where(o => o.Tanggal.Day == 31).Sum(o => o.JumlahHK),
                        };

            List<RotasiPanenPerBulan> blok = new List<RotasiPanenPerBulan>();
            foreach (var o in model)
            {
                int jumlahRotasi = 0;
                jumlahRotasi = jumlahRotasi + (o.T01JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T02JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T03JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T04JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T05JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T06JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T07JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T08JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T09JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T10JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T11JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T12JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T13JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T14JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T15JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T16JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T17JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T18JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T19JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T20JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T21JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T22JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T23JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T24JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T25JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T26JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T27JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T28JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T29JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T30JumlahHK > 0 ? 1 : 0);
                jumlahRotasi = jumlahRotasi + (o.T31JumlahHK > 0 ? 1 : 0);
                o.JumlahRotasi = jumlahRotasi;
                blok.Add(o);
            }

            var model1 = (from m in CRUDRef_Areal.CRUD.GetAllRecord().Where(o => o.kodebudidaya == "02" && o.status == "TM" && o.kodekebun + o.kodeafdeling == Id)
                          group m by new { g1 = m.kodekebun + m.kodeafdeling + m.kodeblok } into g
                          select new PetaView
                          {
                              KodeKebun = g.Key.g1,
                              Id = CRUDBlok.CRUD.Get1Record(g.Key.g1) == null ? Guid.Empty : CRUDBlok.CRUD.Get1Record(g.Key.g1).BlokId,
                              Peta = CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1) == null ? "" : CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta != null ? CRUDBlokRealisasi.CRUD.Get1Record(g.Key.g1).Peta.ToString() : "",
                              Nama = g.Select(o => o.namablok).FirstOrDefault(),
                              TahunTanam = string.Join(",", g.Select(o => o.tahuntanam).ToArray())
                          }).ToList();

            List<PetaView> blok1 = new List<PetaView>();
            foreach (var m in model1)
            {
                string[] strTahunTanam = m.TahunTanam.Split(',').Distinct().ToArray();
                decimal jumlahRotasi = 0;
                decimal normaRotasi = 0;
                m.Value2 = 0;
                var dataran = CRUDKebun.CRUD.Get1Record(m.KodeKebun.Substring(0, 2)) != null ? CRUDKebun.CRUD.Get1Record(m.KodeKebun.Substring(0, 2)).Dataran : "Sedang";
                for (int i = 0; i < strTahunTanam.Length; i++)
                {
                    normaRotasi = normaRotasi + (CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran == dataran) != null ?
                            CRUDStandardProduktivitasTeh.CRUD.GetAllRecord().FirstOrDefault(o => o.Dataran == dataran).RotasiGunting / 12 : 0);
                }

                jumlahRotasi = blok.Where(o => o.KodeLokasi == m.KodeKebun).Count() > 0 ? blok.Where(o => o.KodeLokasi == m.KodeKebun).Average(o => o.JumlahRotasi) : 0;
                if (strTahunTanam.Length > 0)
                {
                    normaRotasi = normaRotasi / strTahunTanam.Length;
                }
                else
                {
                    normaRotasi = 0;
                }

                m.Value1 = (double)(int)jumlahRotasi;
                m.Value2 = (double)normaRotasi;

                if (warna == "")
                {
                    if ((int)jumlahRotasi == 0)
                        m.Warna = "red";
                    else if ((int)jumlahRotasi <= normaRotasi - 1)
                        m.Warna = "yellow";
                    else if ((int)jumlahRotasi <= normaRotasi)
                        m.Warna = "green";
                    else
                        m.Warna = "blue";
                }
                else
                    m.Warna = warna;

                m.Value3 = 0;// (int)((double)CRUDRef_Areal.CRUD.HitungLuasArealKebun(m.KodeKebun, "02", "TM"));
                blok1.Add(m);
            }

            foreach (var k in blok1)
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