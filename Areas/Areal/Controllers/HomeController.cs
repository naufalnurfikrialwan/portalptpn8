using Ptpn8.Areas.Referensi.Models.CRUD;
using System.Linq;
using System.Web.Mvc;
using System.Text;
using System.IO;
using System;
using Ptpn8.Areas.Areal.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Areal.Models.CRUD;
using System.Collections.Generic;
using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Models.CRUD;
using Ptpn8.Models;
using System.Data.SqlClient;
using System.Data;


namespace Ptpn8.Areas.Areal.Controllers
{
    public class HomeController : Controller
    {
        // GET: Areal/Home
        [Authorize]
        public ActionResult Index()
        {
            var model = new RiwayatAkses();
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("ArealDanEvaluasiKinerja");
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Home Areal";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            return View();
        }

        [Authorize]
        public ActionResult cariData([DataSourceRequest]DataSourceRequest request, string cariText = "")
        {

            cariText = cariText.ToLower();
            var kebun = (from m in CRUDKebun.CRUD.GetAllRecord()
                        where m.Nama.ToLower().Contains(cariText)
                        select new PetaView
                        {
                            Peta = null,
                            Id = m.KebunId,
                            Nama = m.Nama,
                            Warna = "Kebun",
                            IdInduk = m.WilayahId
                        }).ToList();

            var afdeling = (from m in CRUDAfdeling.CRUD.GetAllRecord()
                           where m.Nama.ToLower().Contains(cariText)
                           select new PetaView
                           {
                               Peta = null,
                               Id = m.AfdelingId,
                               Nama = m.Nama+"-"+m.Kebun.Nama,
                               Warna = "Afdeling",
                               IdInduk = m.KebunId
                           }).ToList();

            var blok = (from m in CRUDBlok.CRUD.GetAllRecord()
                       where m.Nama.ToLower().Contains(cariText)
                       select new PetaView
                       {
                           Peta = null,
                           Id = m.BlokId,
                           Nama = m.Nama+"-"+m.Afdeling.Nama+"-"+m.Afdeling.Kebun.Nama,
                           Warna = "Blok",
                           IdInduk = m.AfdelingId
                       }).ToList();

            var model = kebun.Union(afdeling).Union(blok).Distinct().ToList();
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal="0", string budidaya="", string masaBerlaku="0", string cariText="")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            List<DataViewKebun> model = new List<DataViewKebun> { };
            string scriptSQL = "exec CreateDataViewKebun " + DateTime.Now.Year.ToString();
            var modelx = ExecSQL(scriptSQL);
            DataViewKebun mm;
            for (int i = 0; i < modelx.Count; i++)
            {
                mm = new DataViewKebun();
                mm.Id = Guid.Parse((string)modelx[i][0]);
                mm.Wilayah = (string)modelx[i][1];
                mm.Nama = (string)modelx[i][2];
                mm.DaftarBudidaya = new List<string> { (string)modelx[i][3] };
                mm.DaftarTahunTanam = new List<string> { (string)modelx[i][4] };
                mm.DaftarSKHGU = new List<string> { (string)modelx[i][5] };
                mm.DaftarStatusAreal = new List<string> { (string)modelx[i][6] };
                mm.TanggalBerlaku = new List<string> { (string)modelx[i][7] };
                mm.TanggalBerakhir = new List<string> { (string)modelx[i][8] };
                mm.LuasAreal = Decimal.Parse((string)modelx[i][9]);
                mm.SisaWaktu = new List<int> { };
                mm.TM = Decimal.Parse((string)modelx[i][10]);
                mm.TBM = Decimal.Parse((string)modelx[i][11]);
                mm.TTI = Decimal.Parse((string)modelx[i][12]);
                mm.TTAD = Decimal.Parse((string)modelx[i][13]);
                mm.Pesemaian = Decimal.Parse((string)modelx[i][14]);
                mm.Entrys = Decimal.Parse((string)modelx[i][15]);
                mm.Reboisasi = Decimal.Parse((string)modelx[i][16]);
                mm.Monocrop = Decimal.Parse((string)modelx[i][17]);
                mm.Intercrop = Decimal.Parse((string)modelx[i][18]);
                mm.TPJ = Decimal.Parse((string)modelx[i][19]);
                mm.Emplasemen = Decimal.Parse((string)modelx[i][20]);
                mm.Jalan = Decimal.Parse((string)modelx[i][21]);
                mm.Fasos = Decimal.Parse((string)modelx[i][22]);
                mm.Marginal = Decimal.Parse((string)modelx[i][23]);
                mm.CadanganMurni = Decimal.Parse((string)modelx[i][24]);
                mm.CadanganPokok = Decimal.Parse((string)modelx[i][25]);
                mm.Agrowisata = Decimal.Parse((string)modelx[i][26]);
                mm.KerjasamaBisnis = Decimal.Parse((string)modelx[i][27]);
                mm.PinjamPakai = Decimal.Parse((string)modelx[i][28]);
                mm.Okupasi = Decimal.Parse((string)modelx[i][29]);
                model.Add(mm);
            }

            //var model = from m in CRUDDataViewKebun.CRUD.GetAllRecord()
            //            where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
            //                (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
            //                (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
            //                 masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
            //                (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
            //            group m by new
            //            {
            //                m.Id,
            //                group1 = (budidaya=="" ? m.Id.ToString() : m.DaftarBudidaya[0]),
            //                group2 = (masaBerlaku == "0" ? m.Id.ToString() : m.SisaWaktu[0].ToString()),
            //                group3 = (statusAreal == "0" ? m.Id.ToString() : m.DaftarStatusAreal[0])
            //            } into g
            //            select new DataViewKebun
            //            {
            //                Id = g.Key.Id,
            //                Wilayah = g.Select(o => o.Wilayah).FirstOrDefault(),
            //                Nama = g.Select(o => o.Nama).FirstOrDefault(),
            //                DaftarBudidaya = g.Select(o => o.DaftarBudidaya[0]).Distinct().ToList(),
            //                DaftarTahunTanam = g.Select(o => string.Join(", ", o.DaftarTahunTanam)).Distinct().ToList(),
            //                DaftarSKHGU = g.Select(o => string.Join(", ", o.DaftarSKHGU)).Distinct().ToList(),
            //                DaftarStatusAreal = g.Select(o => string.Join(", ", o.DaftarStatusAreal)).Distinct().ToList(),
            //                TanggalBerlaku = g.Select(o => string.Join(", ",o.TanggalBerlaku)).Distinct().ToList(),
            //                TanggalBerakhir = g.Select(o => string.Join(", ",o.TanggalBerakhir)).Distinct().ToList(),
            //                SisaWaktu = g.Select(o => o.SisaWaktu[0]).Distinct().ToList(),
            //                TM = g.Where(o => o.DaftarStatusAreal[0] == "1.1").Sum(o => o.LuasAreal),
            //                TBM = g.Where(o => o.DaftarStatusAreal[0] == "1.2").Sum(o => o.LuasAreal),
            //                TTI = g.Where(o => o.DaftarStatusAreal[0] == "1.3").Sum(o => o.LuasAreal),
            //                TTAD = g.Where(o => o.DaftarStatusAreal[0] == "1.4").Sum(o => o.LuasAreal),
            //                Pesemaian = g.Where(o => o.DaftarStatusAreal[0] == "1.5").Sum(o => o.LuasAreal),
            //                Entrys = g.Where(o => o.DaftarStatusAreal[0] == "1.6").Sum(o => o.LuasAreal),
            //                Reboisasi = g.Where(o => o.DaftarStatusAreal[0] == "1.7").Sum(o => o.LuasAreal),
            //                Monocrop = g.Where(o => o.DaftarStatusAreal[0] == "1.8").Sum(o => o.LuasAreal),
            //                Intercrop = g.Where(o => o.DaftarStatusAreal[0] == "1.9").Sum(o => o.LuasAreal),
            //                TPJ = g.Where(o => o.DaftarStatusAreal[0] == "1.10").Sum(o => o.LuasAreal),
            //                Emplasemen = g.Where(o => o.DaftarStatusAreal[0] == "2.1").Sum(o => o.LuasAreal),
            //                Jalan = g.Where(o => o.DaftarStatusAreal[0] == "2.2").Sum(o => o.LuasAreal),
            //                Fasos = g.Where(o => o.DaftarStatusAreal[0] == "2.3").Sum(o => o.LuasAreal),
            //                Marginal = g.Where(o => o.DaftarStatusAreal[0] == "2.4").Sum(o => o.LuasAreal),
            //                CadanganMurni = g.Where(o => o.DaftarStatusAreal[0] == "2.5.1").Sum(o => o.LuasAreal),
            //                CadanganPokok = g.Where(o => o.DaftarStatusAreal[0] == "2.5.2").Sum(o => o.LuasAreal),
            //                Agrowisata = g.Where(o => o.DaftarStatusAreal[0] == "2.6").Sum(o => o.LuasAreal),
            //                KerjasamaBisnis = g.Where(o => o.DaftarStatusAreal[0] == "3.1").Sum(o => o.LuasAreal),
            //                PinjamPakai = g.Where(o => o.DaftarStatusAreal[0] == "3.2").Sum(o => o.LuasAreal),
            //                Okupasi = g.Where(o => o.DaftarStatusAreal[0] == "4.1").Sum(o => o.LuasAreal),
            //                LuasAreal = g.Sum(o => o.LuasAreal)
            //            };
            string[] ss = { };
            string s = "";
            var kebun = new List<DataViewKebun>();
            foreach (var m in model)
            {

                s = "";
                ss = null;
                for (int i = 0; i < m.DaftarTahunTanam.Count(); i++)
                {
                    s += m.DaftarTahunTanam[i].Replace(" ", "").Replace(".", ",").Replace("/", ",").Replace("'", ",") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.DaftarTahunTanam = null;
                m.DaftarTahunTanam = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.DaftarSKHGU.Count(); i++)
                {
                    s += m.DaftarSKHGU[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.DaftarSKHGU = null;
                m.DaftarSKHGU = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.TanggalBerlaku.Count(); i++)
                {
                    s += m.TanggalBerlaku[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.TanggalBerlaku = null;
                m.TanggalBerlaku = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.TanggalBerakhir.Count(); i++)
                {
                    s += m.TanggalBerakhir[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.TanggalBerakhir = null;
                m.TanggalBerakhir = ss.Distinct().OrderBy(x => x).ToList();

                m.sDaftarBudidaya = string.Join(",", m.DaftarBudidaya);
                m.DaftarBudidaya = null;
                m.sDaftarSKHGU = string.Join(",", m.DaftarSKHGU);
                m.DaftarSKHGU = null;
                m.sDaftarStatusAreal = string.Join(",", m.DaftarStatusAreal);
                m.DaftarStatusAreal = null;
                m.sDaftarTahunTanam = string.Join(",", m.DaftarTahunTanam);
                m.DaftarTahunTanam = null;
                m.sSisaWaktu = string.Join(",", m.SisaWaktu);
                m.SisaWaktu = null;
                m.sTanggalBerakhir = string.Join(",", m.TanggalBerakhir);
                m.TanggalBerakhir = null;
                m.sTanggalBerlaku = string.Join(",", m.TanggalBerlaku);
                m.TanggalBerlaku = null;

                kebun.Add(m);
            }

            return Json(kebun.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "",string statusAreal="0", string budidaya = "", string masaBerlaku = "0", string cariText="")
        {
            if (Id == "") return View();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            var model = from m in CRUDDataViewAfdeling.CRUD.GetAllRecord().Where(o=>o.KebunId==Guid.Parse(Id))
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
                             masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new
                        {
                            m.Id,
                            group1 = (budidaya == "" ? m.Id.ToString() : m.DaftarBudidaya[0]),
                            group2 = (masaBerlaku == "0" ? m.Id.ToString() : m.SisaWaktu[0].ToString()),
                            group3 = (statusAreal == "0" ? m.Id.ToString() : m.DaftarStatusAreal[0])
                        } into g
                        select new DataViewAfdeling
                        {
                            Id = g.Key.Id,
                            Kebun = g.Select(o => o.Kebun).FirstOrDefault(),
                            Nama = g.Select(o => o.Nama).FirstOrDefault(),
                            DaftarBudidaya = g.Select(o => o.DaftarBudidaya[0]).Distinct().ToList(),
                            DaftarTahunTanam = g.Select(o => string.Join(", ", o.DaftarTahunTanam)).Distinct().ToList(),
                            DaftarSKHGU = g.Select(o => string.Join(", ", o.DaftarSKHGU)).Distinct().ToList(),
                            DaftarStatusAreal = g.Select(o => string.Join(", ", o.DaftarStatusAreal)).Distinct().ToList(),
                            TanggalBerlaku = g.Select(o => string.Join(", ", o.TanggalBerlaku)).Distinct().ToList(),
                            TanggalBerakhir = g.Select(o => string.Join(", ", o.TanggalBerakhir)).Distinct().ToList(),
                            SisaWaktu = g.Select(o => o.SisaWaktu[0]).Distinct().ToList(),
                            TM = g.Where(o => o.DaftarStatusAreal[0] == "1.1").Sum(o => o.LuasAreal),
                            TBM = g.Where(o => o.DaftarStatusAreal[0] == "1.2").Sum(o => o.LuasAreal),
                            TTI = g.Where(o => o.DaftarStatusAreal[0] == "1.3").Sum(o => o.LuasAreal),
                            TTAD = g.Where(o => o.DaftarStatusAreal[0] == "1.4").Sum(o => o.LuasAreal),
                            Pesemaian = g.Where(o => o.DaftarStatusAreal[0] == "1.5").Sum(o => o.LuasAreal),
                            Entrys = g.Where(o => o.DaftarStatusAreal[0] == "1.6").Sum(o => o.LuasAreal),
                            Reboisasi = g.Where(o => o.DaftarStatusAreal[0] == "1.7").Sum(o => o.LuasAreal),
                            Monocrop = g.Where(o => o.DaftarStatusAreal[0] == "1.8").Sum(o => o.LuasAreal),
                            Intercrop = g.Where(o => o.DaftarStatusAreal[0] == "1.9").Sum(o => o.LuasAreal),
                            TPJ = g.Where(o => o.DaftarStatusAreal[0] == "1.10").Sum(o => o.LuasAreal),
                            Emplasemen = g.Where(o => o.DaftarStatusAreal[0] == "2.1").Sum(o => o.LuasAreal),
                            Jalan = g.Where(o => o.DaftarStatusAreal[0] == "2.2").Sum(o => o.LuasAreal),
                            Fasos = g.Where(o => o.DaftarStatusAreal[0] == "2.3").Sum(o => o.LuasAreal),
                            Marginal = g.Where(o => o.DaftarStatusAreal[0] == "2.4").Sum(o => o.LuasAreal),
                            CadanganMurni = g.Where(o => o.DaftarStatusAreal[0] == "2.5.1").Sum(o => o.LuasAreal),
                            CadanganPokok = g.Where(o => o.DaftarStatusAreal[0] == "2.5.2").Sum(o => o.LuasAreal),
                            Agrowisata = g.Where(o => o.DaftarStatusAreal[0] == "2.6").Sum(o => o.LuasAreal),
                            KerjasamaBisnis = g.Where(o => o.DaftarStatusAreal[0] == "3.1").Sum(o => o.LuasAreal),
                            PinjamPakai = g.Where(o => o.DaftarStatusAreal[0] == "3.2").Sum(o => o.LuasAreal),
                            Okupasi = g.Where(o => o.DaftarStatusAreal[0] == "4.1").Sum(o => o.LuasAreal),
                            LuasAreal = g.Sum(o => o.LuasAreal)
                        };

            string[] ss = { };
            string s = "";
            var afdeling = new List<DataViewAfdeling>();
            foreach (var m in model)
            {

                s = "";
                ss = null;
                for (int i = 0; i < m.DaftarTahunTanam.Count(); i++)
                {
                    s += m.DaftarTahunTanam[i].Replace(" ", "").Replace(".", ",").Replace("/", ",").Replace("'", ",") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.DaftarTahunTanam = null;
                m.DaftarTahunTanam = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.DaftarSKHGU.Count(); i++)
                {
                    s += m.DaftarSKHGU[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.DaftarSKHGU = null;
                m.DaftarSKHGU = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.TanggalBerlaku.Count(); i++)
                {
                    s += m.TanggalBerlaku[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.TanggalBerlaku = null;
                m.TanggalBerlaku = ss.Distinct().OrderBy(x => x).ToList();

                s = "";
                ss = null;
                for (int i = 0; i < m.TanggalBerakhir.Count(); i++)
                {
                    s += m.TanggalBerakhir[i].Replace(" ", "") + ",";
                }
                ss = s.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                m.TanggalBerakhir = null;
                m.TanggalBerakhir = ss.Distinct().OrderBy(x => x).ToList();

                m.sDaftarBudidaya = string.Join(",", m.DaftarBudidaya);
                m.DaftarBudidaya = null;
                m.sDaftarSKHGU = string.Join(",", m.DaftarSKHGU);
                m.DaftarSKHGU = null;
                m.sDaftarStatusAreal = string.Join(",", m.DaftarStatusAreal);
                m.DaftarStatusAreal = null;
                m.sDaftarTahunTanam = string.Join(",", m.DaftarTahunTanam);
                m.DaftarTahunTanam = null;
                m.sSisaWaktu = string.Join(",", m.SisaWaktu);
                m.SisaWaktu = null;
                m.sTanggalBerakhir = string.Join(",", m.TanggalBerakhir);
                m.TanggalBerakhir = null;
                m.sTanggalBerlaku = string.Join(",", m.TanggalBerlaku);
                m.TanggalBerlaku = null;

                afdeling.Add(m);
            }

            return Json(afdeling.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal="0", string budidaya = "", string masaBerlaku = "0", string cariText="")
        {
            if (Id == "") return View();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            var model = from m in CRUDDataViewBlok.CRUD.GetAllRecord().Where(o => o.AfdelingId == Guid.Parse(Id))
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu < 5 :
                             masaBerlaku == "3" ? m.SisaWaktu >= 10 : m.SisaWaktu >= 5 && m.SisaWaktu < 10) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new
                        {
                            m.Id,
                            group1 = (budidaya == "" ? m.Id.ToString() : m.DaftarBudidaya),
                            group2 = (masaBerlaku == "0" ? m.Id.ToString() : m.SisaWaktu.ToString()),
                            group3 = (statusAreal == "0" ? m.Id.ToString() : m.DaftarStatusAreal)
                        } into g
                        select new DataViewBlok
                        {
                            Id = g.Key.Id,
                            Afdeling = g.Select(o => o.Afdeling).FirstOrDefault(),
                            Nama = g.Select(o => o.Nama).FirstOrDefault(),
                            DaftarBudidaya = g.Select(o => o.DaftarBudidaya).FirstOrDefault(),
                            DaftarTahunTanam = g.Select(o => o.DaftarTahunTanam).FirstOrDefault(),
                            DaftarSKHGU = g.Select(o => o.DaftarSKHGU).FirstOrDefault(),
                            DaftarStatusAreal = g.Select(o => o.DaftarStatusAreal).FirstOrDefault(),
                            TanggalBerlaku = g.Select(o => o.TanggalBerlaku).FirstOrDefault(),
                            TanggalBerakhir = g.Select(o => o.TanggalBerakhir).FirstOrDefault(),
                            SisaWaktu = g.Select(o => o.SisaWaktu).FirstOrDefault(),
                            LuasAreal = g.Sum(o => o.LuasAreal)
                        };
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult getLegend([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal="0", string budidaya = "", string masaBerlaku = "0", string cariText="")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            if (Id == Guid.Empty.ToString())
            {
                var kebun = from m in CRUDKebun.CRUD.GetAllRecord()
                            select new PetaView
                            {
                                Peta = null,
                                Id = m.KebunId,
                                Nama = m.Nama,
                                Warna = m.Warna,
                                IdInduk = m.WilayahId
                            };
                return Json(kebun.ToDataSourceResult(request));
            }
            else
            {
                var afdeling = from m in CRUDAfdeling.CRUD.GetAllRecord().Where(o => o.KebunId == Guid.Parse(Id))
                               select new PetaView
                               {
                                   Peta = null,
                                   Id = m.AfdelingId,
                                   Nama = m.Nama,
                                   Warna = m.Warna,
                                   IdInduk = m.KebunId
                               };
                if (afdeling.ToList().Count > 0)
                {
                    return Json(afdeling.ToDataSourceResult(request));
                }
                else
                {
                    var blok = from m in CRUDBlok.CRUD.GetAllRecord().Where(o=>o.AfdelingId == Guid.Parse(Id))
                               join n in CRUDBlokRealisasi.CRUD.GetAllRecord().Where(o=>statusAreal=="0"? true :o.StatusAreal==statusAreal).Where(o => (Guid.Parse(budidaya) == Guid.Empty ? true : o.MainBudidayaId == Guid.Parse(budidaya))) on m.BlokId equals n.BlokId
                               join hgu in CRUDHGU.CRUD.GetAllRecord().Where(o => (masaBerlaku == "0" ? true : masaBerlaku == "1" ? o.SisaWaktu < 5 : masaBerlaku == "3" ? o.SisaWaktu >= 10 : o.SisaWaktu >= 5 && o.SisaWaktu < 10)) on n.HGUId equals hgu.HGUId
                               group m by new { m.BlokId, m.AfdelingId } into g
                               select new PetaView
                               {
                                   Peta = null,
                                   Id = g.Key.BlokId,
                                   Nama = g.FirstOrDefault().Nama,
                                   Warna = g.FirstOrDefault().Warna,
                                   IdInduk = g.Key.AfdelingId
                               };
                    return Json(blok.ToDataSourceResult(request));
                }
            }
        }

        [Authorize]
        public ActionResult getLegendKebun([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal = "0", string budidaya = "", string masaBerlaku = "0", string cariText = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            if (Id == Guid.Empty.ToString())
            {

                var kebun = from m in CRUDDataViewKebun.CRUD.GetAllRecord()
                            where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                                (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                                (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
                                 masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
                                (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                            group m by new { m.Id, m.Nama } into g
                            select new PetaView
                            {
                                Peta = null,
                                Id = g.Key.Id,
                                Nama = g.Key.Nama,
                                Warna = CRUDKebun.CRUD.Get1Record(g.Key.Id).Warna,
                                IdInduk = CRUDKebun.CRUD.Get1Record(g.Key.Id).WilayahId
                            };

                return Json(kebun.OrderBy(o=>o.Nama).ToDataSourceResult(request));
            }
            else return View(); 
        }

        [Authorize]
        public ActionResult getLegendAfdeling([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal = "0", string budidaya = "", string masaBerlaku = "0", string cariText = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            var afdeling = from m in CRUDDataViewAfdeling.CRUD.GetAllRecord().Where(o => o.KebunId == Guid.Parse(Id))
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
                             masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new { m.Id, m.Nama } into g
                        select new PetaView
                            {
                                Peta = null,
                                Id = g.Key.Id,
                                Nama = g.Key.Nama,
                                Warna = CRUDAfdeling.CRUD.Get1Record(g.Key.Id).Warna,
                                IdInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.Id).KebunId
                            };

            if (afdeling.ToList().Count > 0)
            {
                return Json(afdeling.OrderBy(o=>o.Nama).ToDataSourceResult(request));
            }
            
            else return View();
        }

        [Authorize]
        public ActionResult getLegendBlok([DataSourceRequest]DataSourceRequest request, string Id = "", string statusAreal = "0", string budidaya = "", string masaBerlaku = "0", string cariText = "")
        {
            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;

            var model = from m in CRUDDataViewBlok.CRUD.GetAllRecord().Where(o => o.AfdelingId == Guid.Parse(Id))
                       where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                           (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) && 
                           (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu < 5 :
                            masaBerlaku == "3" ? m.SisaWaktu >= 10 : m.SisaWaktu >= 5 && m.SisaWaktu < 10) &&
                           (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                       group m by new { m.Id, m.Nama, m.DaftarBudidaya} into g
                       select new PetaView
                       {
                           Id = g.Key.Id,
                           Nama = CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal.Substring(0, 1) != "1" ? 
                                    CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Deskripsi :
                                    CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Deskripsi + "["+g.Key.DaftarBudidaya+"]" ,

                           Warna = CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal.Substring(0, 1) != "1" ?
                                    CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Warna :
                                    GabungWarna(CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Warna,
                                        CRUDBudidaya.CRUD.Get1Record(g.Key.DaftarBudidaya).Warna),
                       };

            List<PetaView> blok = new List<PetaView>();
            blok.AddRange(from m in model
                          group m by new { m.Nama} into g
                          select new PetaView
                          {
                              Nama = g.Key.Nama,
                              Warna = g.Select(o => o.Warna).FirstOrDefault()
                          });


            if (blok.ToList().Count > 0)
                return Json(blok.OrderBy(o=>o.Nama).ToDataSourceResult(request));
            else
                return View();
        }

        private string GabungWarna(string nilai1, string nilai2)
        {
            System.Drawing.Color Color1 = System.Drawing.Color.FromArgb(Convert.ToInt32(nilai1.Substring(1, 6), 16));
            System.Drawing.Color Color2 = System.Drawing.Color.FromArgb(Convert.ToInt32(nilai2.Substring(1, 6), 16));
            System.Drawing.Color ColorMix = new System.Drawing.Color();
            byte r = byte.Parse(((int) ((Color1.R * 0.6) + (Color2.R * 0.4))).ToString());
            byte g = byte.Parse(((int)((Color1.G * 0.6) + (Color2.G * 0.4))).ToString());
            byte b = byte.Parse(((int)((Color1.B * 0.6) + (Color2.B * 0.4))).ToString());
            ColorMix = System.Drawing.Color.FromArgb(r, g, b);
            return "#" + ColorMix.R.ToString("X2") + ColorMix.G.ToString("X2") + ColorMix.B.ToString("X2");
        }

        [Authorize]
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [Authorize]
        public ActionResult KebunGeoJSon(string Id = "", string statusAreal = "0", string budidaya = "", string masaBerlaku = "0", string cariText = "", string warna = "")
        {

            if (Id == "") Id = Guid.Empty.ToString();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var kebun = from m in CRUDDataViewKebun.CRUD.GetAllRecord()
                        join n in CRUDKebunPeta.CRUD.GetAllRecord().Where(o=>o.Peta!=null) on m.Id equals n.KebunId
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) && 
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
                             masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new { m.Id, m.Nama, n.Peta} into g
                        select new PetaView
                        {
                            Peta = g.Key.Peta.ToString(),
                            Id = g.Key.Id,
                            Nama = g.Key.Nama,
                            Warna = warna == "" ? CRUDKebun.CRUD.Get1Record(g.Key.Id).Warna : warna,
                            IdInduk = CRUDKebun.CRUD.Get1Record(g.Key.Id).WilayahId
                        };

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
                featureCollection.Add(feature);
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }

        [Authorize]
        public ActionResult AfdelingGeoJSon(string Id = "", string statusAreal="0", string budidaya = "", string masaBerlaku = "0", string cariText="", string warna = "")
        {
            if (Id == "") return View();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";

            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var afdeling = from m in CRUDDataViewAfdeling.CRUD.GetAllRecord().Where(o => o.KebunId == Guid.Parse(Id))
                           join n in CRUDAfdelingPeta.CRUD.GetAllRecord().Where(o=>o.Peta!=null) on m.Id equals n.AfdelingId
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) &&
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu.All(x => x < 5) :
                             masaBerlaku == "3" ? m.SisaWaktu.All(x => x >= 10) : m.SisaWaktu.All(x => x >= 5) && m.SisaWaktu.All(x => x < 10)) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new { m.Id, m.Nama, n.Peta } into g
                        select new PetaView
                        {
                            Peta = g.Key.Peta.ToString(),
                            Id = g.Key.Id,
                            Nama = g.Key.Nama,
                            Warna = warna == "" ? CRUDAfdeling.CRUD.Get1Record(g.Key.Id).Warna : warna,
                            IdInduk = CRUDAfdeling.CRUD.Get1Record(g.Key.Id).KebunId
                        };

            foreach (var k in afdeling)
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
                featureCollection.Add(feature);
            }

            serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
            using (var sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, featureCollection);
            }
            return Content(sb.ToString(), "application/json");
        }

        [Authorize]
        public ActionResult BlokGeoJSon(string Id = "", string statusAreal="0", string budidaya = "", string masaBerlaku = "0", string cariText="", string warna = "")
        {
            if (Id == "") return View();
            if (budidaya == "Seluruh") budidaya = "";
            if (statusAreal == "Seluruh") statusAreal = "0";
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            var featureCollection = new NetTopologySuite.Features.FeatureCollection();
            var sb = new StringBuilder();
            var serializer = new NetTopologySuite.IO.GeoJsonSerializer();

            var blok = from m in CRUDDataViewBlok.CRUD.GetAllRecord().Where(o => o.AfdelingId == Guid.Parse(Id))
                       join n in CRUDBlokRealisasi.CRUD.GetAllRecord().Where(o=>o.Peta!=null) on m.Id equals n.BlokId
                        where (cariText == "" ? true : m.Nama.ToLower().Contains(cariText.ToLower())) && 
                            (budidaya == "" ? true : m.DaftarBudidaya.Contains(budidaya)) &&
                            (masaBerlaku == "0" ? true : masaBerlaku == "1" ? m.SisaWaktu < 5 :
                             masaBerlaku == "3" ? m.SisaWaktu >= 10 : m.SisaWaktu >= 5 && m.SisaWaktu < 10) &&
                            (statusAreal == "0" ? true : m.DaftarStatusAreal.Contains(statusAreal))
                        group m by new { m.Id, m.Nama, n.StatusAreal, n.Peta, m.DaftarBudidaya } into g
                       select new PetaView
                       {
                           Peta = g.Key.Peta.ToString(),
                           Id = g.Key.Id,
                           Nama = g.Key.Nama,
                           Warna = warna != "" ? warna : CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal.Substring(0, 1) != "1" ?
                                    CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Warna :
                                    GabungWarna(CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Warna,
                                        CRUDBudidaya.CRUD.Get1Record(g.Key.DaftarBudidaya).Warna),
                           IdInduk = CRUDBlok.CRUD.Get1Record(g.Key.Id).AfdelingId,
                           KodeBudidaya = g.Key.DaftarBudidaya,
                           TahunTanam = CRUDStatusAreal.CRUD.Get1Record(CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).StatusAreal).Deskripsi,
                           Value1 = (double)CRUDBlokRealisasi.CRUD.Get1Record(CRUDBlok.CRUD.Get1Record(g.Key.Id).kd_blok).LuasAreal
                       };

            foreach (var k in blok)
            {
                var wktReader = new NetTopologySuite.IO.WKTReader();
                var geom = wktReader.Read(k.Peta.ToString());
                var feature = new NetTopologySuite.Features.Feature(geom, new NetTopologySuite.Features.AttributesTable());
                
                feature.Attributes.AddAttribute("NAMA_KEBUN", k.Nama+"["+k.TahunTanam+"]"+"["+k.KodeBudidaya+"]"+"["+k.Value1.ToString()+" Ha]");
                feature.Attributes.AddAttribute("TOPLEFT_X", feature.Geometry.Coordinates.Min(o => o.X));
                feature.Attributes.AddAttribute("TOPLEFT_Y", feature.Geometry.Coordinates.Min(o => o.Y));
                feature.Attributes.AddAttribute("BOTTOMRIGHT_X", feature.Geometry.Coordinates.Max(o => o.X));
                feature.Attributes.AddAttribute("BOTTOMRIGHT_Y", feature.Geometry.Coordinates.Max(o => o.Y));
                feature.Attributes.AddAttribute("ID", k.Id);
                feature.Attributes.AddAttribute("COLOR", k.Warna);
                featureCollection.Add(feature);
            }

            if (blok.ToList().Count > 0)
            {
                serializer.Formatting = Newtonsoft.Json.Formatting.Indented;
                using (var sw = new StringWriter(sb))
                {
                    serializer.Serialize(sw, featureCollection);
                }
                return Content(sb.ToString(), "application/json");
            }
            else
                return View();
        }

        [Authorize]
        public ActionResult areaKebun(string id)
        {
            object[] result = { "", "", "", "", "" };
            var wktReader = new NetTopologySuite.IO.WKTReader();
            var kebun = (from m in CRUDKebunPeta.CRUD.GetAllRecord().Where(o=>o.KebunPetaId==Guid.Parse(id))
                         select new
                         {
                             Id = m.KebunId,
                             feature = new NetTopologySuite.Features.Feature(wktReader.Read(m.Peta.ToString()), new NetTopologySuite.Features.AttributesTable())
                         }).ToList();

            if (kebun.Count > 0)
            {
                result = new object[] { kebun.FirstOrDefault().Id.ToString(),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.Y),
                    kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.Y)};
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult pointIsInKebun(string x, string y)
        {
            object[] result = {"","","","",""};
            var wktReader = new NetTopologySuite.IO.WKTReader();
            double Lat = double.Parse(y.Replace(".",","));
            double Lon = double.Parse(x.Replace(".", ","));
            var coord = new NetTopologySuite.Geometries.Point(Lat, Lon);

            var kebun = (from m in CRUDKebunPeta.CRUD.GetAllRecord().Where(o=>coord.Within(wktReader.Read(o.Peta.ToString())))
                        join n in CRUDKebun.CRUD.GetAllRecord()
                        on m.KebunId equals n.KebunId
                        select new 
                        {
                            Id = m.KebunId,
                            Nama = n.Nama,
                            Warna = n.Warna,
                            feature = new NetTopologySuite.Features.Feature(wktReader.Read(m.Peta.ToString()), new NetTopologySuite.Features.AttributesTable())
                        }).ToList();

            if (kebun.Count > 0)
            {
                result = new object[] { kebun.FirstOrDefault().Id.ToString(),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.Y),
                    kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.Y)};
            }
            return Json(result,JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult cariLokasi(string id, string key)
        {
            var wktReader = new NetTopologySuite.IO.WKTReader();
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            if (key == "Kebun")
            {
                var kebun = (from n in CRUDKebun.CRUD.GetAllRecord().Where(o => o.KebunId == Guid.Parse(id))
                             join m in CRUDKebunPeta.CRUD.GetAllRecord() on n.KebunId equals m.KebunId
                             select new
                             {
                                 Id = m.KebunId,
                                 Nama = n.Nama,
                                 Warna = n.Warna,
                                 feature = new NetTopologySuite.Features.Feature(wktReader.Read(m.Peta.ToString()), new NetTopologySuite.Features.AttributesTable())
                             }).ToList();
                if (kebun.Count > 0)
                {
                    var result = new object[] { kebun.FirstOrDefault().Id.ToString(),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.Y),
                        kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.X),kebun.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.Y)};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);
                }
             
            } else if (key == "Afdeling")
            {
                var afdeling = (from m in CRUDAfdeling.CRUD.GetAllRecord().Where(o => o.AfdelingId == Guid.Parse(id))
                                join n in CRUDAfdelingPeta.CRUD.GetAllRecord() on m.AfdelingId equals n.AfdelingId
                                select new
                                {
                                    Id = m.AfdelingId,
                                    Nama = m.Nama,
                                    Warna = m.Warna,
                                    feature = new NetTopologySuite.Features.Feature(wktReader.Read(n.Peta.ToString()), new NetTopologySuite.Features.AttributesTable())
                                }).ToList();

                if (afdeling.Count > 0)
                {
                    var result = new object[] { afdeling.FirstOrDefault().Id.ToString(),afdeling.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.X),afdeling.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.Y),
                        afdeling.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.X),afdeling.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.Y)};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);
                }
            } else if (key=="Blok")
            {
                var blok = (from m in CRUDBlok.CRUD.GetAllRecord().Where(o => o.BlokId == Guid.Parse(id))
                            join n in CRUDBlokRealisasi.CRUD.GetAllRecord() on m.BlokId equals n.BlokId
                            select new
                            {
                                Id = m.BlokId,
                                Nama = m.Nama,
                                Warna = m.Warna,
                                feature = new NetTopologySuite.Features.Feature(wktReader.Read(n.Peta==null?"":n.Peta.ToString()), new NetTopologySuite.Features.AttributesTable())
                            }).ToList();
                if (blok.Count > 0)
                {
                    var result = new object[] { blok.FirstOrDefault().Id.ToString(),blok.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.X),blok.FirstOrDefault().feature.Geometry.Coordinates.Min(o => o.Y),
                            blok.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.X),blok.FirstOrDefault().feature.Geometry.Coordinates.Max(o => o.Y)};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                    return Json(new object[] { }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new object[] { }, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult getStatusAreal()
        {
            List<StatusAreal> model = new List<StatusAreal> ();
            var newRow = new StatusAreal();
            newRow.Nama = "0";
            newRow.Deskripsi = "Seluruh";
            model.Add(newRow);
            model.AddRange(CRUDStatusAreal.CRUD.GetAllRecord().OrderBy(x => x.Nama));
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getBudidaya()
        {
            List<MainBudidaya> model = new List<MainBudidaya>();
            var newRow = new MainBudidaya();
            newRow.MainBudidayaId = Guid.Empty;
            newRow.Nama = "Seluruh";
            model.Add(newRow);
            model.AddRange(CRUDBudidaya.CRUD.GetAllRecord().OrderBy(x => x.Nama));
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult getBagian()
        {
            var bagian = CRUDBagian.CRUD.GetAllRecord();
            return Json(bagian, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Excel_Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);
            return File(fileContents, contentType, fileName);
        }

        [HttpPost]
        [Authorize]
        public ActionResult Export_Save(string contentType, string base64, string fileName)
        {
            var fileContents = Convert.FromBase64String(base64);

            return File(fileContents, contentType, fileName);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperStatusAreal();
            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealKaretRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperBudidaya("00");
            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealSawitRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperBudidaya("01");
            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealTehRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperBudidaya("02");
            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealKinaRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperBudidaya("03");
            return Json(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChartStatusArealLainnyaRead()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = 2016;
            var model = CRUDBlokRealisasi.CRUD.getLuasArealperBudidaya("");
            return Json(model);
        }

        public List<object[]> ExecSQL(string scriptSQL)
        {
            List<object[]> Results = new List<object[]>();
            string arrCS = System.Configuration.ConfigurationManager.ConnectionStrings["csReferensi"].ConnectionString;
            SqlConnection con = new SqlConnection(arrCS);
            try
            {
                con.Open();
                DataTable tap = new DataTable();
                new SqlDataAdapter(scriptSQL, con).Fill(tap);
                for (int i = 0; i < tap.Rows.Count; i++)
                {
                    object[] result = new object[tap.Columns.Count];
                    DataRow dr = tap.Rows[i];
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (dr[j].GetType().Name == "Byte[]")
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                        }
                        else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                        }
                        result[j] = dr[j].ToString();
                    }
                    Results.Add(result);
                }
            }
            catch (Exception ex)
            {
                //Exception   
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return Results;
        }

    }
}