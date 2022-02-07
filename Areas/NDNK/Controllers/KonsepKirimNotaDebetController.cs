using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Areal.Models;
using Ptpn8.Areas.Areal.Models.CRUD;
using Ptpn8.Areas.NDNK.Models;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Areas.NDNK.Controllers
{
    [Authorize]
    public class KonsepKirimNotaDebetController : Controller
    {
        // GET: NDNK/KonsepKirimNota
        public ActionResult Index()
        {
            HttpContext.Session["LOKASIKEBUNID"] = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId;
            HttpContext.Session["TAHUNBUKU"] = StatusTahunBuku.CRUDStatusTahunBuku.CRUD.TahunBuku(CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId);

            var model = new RiwayatAkses();
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("NDNK");
            ViewBag.Title = "Pembuatan Debet Nota";
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            model.UserName = HttpContext.User.Identity.Name;
            model.TanggalAkses = DateTime.Now;
            model.PageYangDiakses = "Konsep NDNK";
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(model);
            var mdl = new HDRKonsepKirimNota {
                StatusDK = "DN",
                KebunPengirimId = (Guid)HttpContext.Session["LOKASIKEBUNID"],
                KebunPengirim = CRUDKebun.CRUD.Get1Record((Guid)HttpContext.Session["LOKASIKEBUNID"]),
                Status = "Konsep",
                Pembuat = HttpContext.User.Identity.Name,
                TahunBuku = (int)HttpContext.Session["TAHUNBUKU"]
            };
            return View(mdl);
        }

        [Authorize]
        public ActionResult getNomorInput()
        {
            var currentData = HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.GetAllRecord()
                .Where(o => o.StatusDK == "DN" && o.KebunPengirimId== (Guid)HttpContext.Session["LOKASIKEBUNID"] && o.TahunBuku== (int)HttpContext.Session["TAHUNBUKU"]).OrderByDescending(o => o.Nomor).ToList();
            int NoBaru = 1;
            if (currentData.Count > 0)
            {
                NoBaru = currentData.First().Nomor + 1;
            }
            var newData = new HDRKonsepKirimNota
            {
                Nomor = NoBaru,
                NomorNota = "DN-" + NoBaru.ToString("D5") + " - Data Baru",
                StatusDK = "DN",
                KebunPengirimId = (Guid)HttpContext.Session["LOKASIKEBUNID"],
                KebunPengirim = CRUDKebun.CRUD.Get1Record((Guid)HttpContext.Session["LOKASIKEBUNID"]),
                KebunPenerima = new Kebun(),
                Status = "Konsep",
                Pembuat = HttpContext.User.Identity.Name,
                TahunBuku = (int)HttpContext.Session["TAHUNBUKU"]
            };
            currentData.Add(newData);
            var model = (from m in currentData orderby m.Nomor descending select m).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult HDRKonsepKirimNotaRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.GetAllRecord()
                .Where(o => o.StatusDK == "DN" && o.KebunPengirimId == (Guid)HttpContext.Session["LOKASIKEBUNID"] && o.TahunBuku== (int)HttpContext.Session["TAHUNBUKU"]).OrderByDescending(o => o.Nomor).ToList();
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult HDRKonsepKirimNotaCreate([DataSourceRequest]DataSourceRequest request,
            HDRKonsepKirimNota hDRKonsepKirimNota)
        {
            if (ModelState.IsValid)
            {
                if (HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Create(hDRKonsepKirimNota))
                    return Json(new[] { hDRKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(hDRKonsepKirimNota);
            }
            return View(hDRKonsepKirimNota);

        }

        [Authorize]
        [HttpPost]
        public ActionResult HDRKonsepKirimNotaUpdate([DataSourceRequest] DataSourceRequest request, HDRKonsepKirimNota hDRKonsepKirimNota)
        {
            if (ModelState.IsValid)
            {
                if (HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Update(hDRKonsepKirimNota))
                    return Json(new[] { hDRKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(hDRKonsepKirimNota);
            }
            return View(hDRKonsepKirimNota);
        }

        [Authorize]
        [HttpPost]
        public ActionResult HDRKonsepKirimNotaDestroy([DataSourceRequest] DataSourceRequest request, HDRKonsepKirimNota hDRKonsepKirimNota)
        {
            if (hDRKonsepKirimNota != null)
            {
                if (HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Delete(hDRKonsepKirimNota))
                    return Json(new[] { hDRKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(hDRKonsepKirimNota);
            }
            return View(hDRKonsepKirimNota);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DTLKonsepKirimNotaRead([DataSourceRequest]DataSourceRequest request, string hDRKonsepKirimNotaId)
        {
            var model = DTLKonsepKirimNota.CRUDDTLKonsepKirimNota.CRUD.GetAllRecord().
                Where(o => o.HDRKonsepKirimNotaId == Guid.Parse(hDRKonsepKirimNotaId));
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DTLKonsepKirimNotaCreate([DataSourceRequest]DataSourceRequest request,
            DTLKonsepKirimNota dTLKonsepKirimNota)
        {
            if (ModelState.IsValid)
            {
                if (DTLKonsepKirimNota.CRUDDTLKonsepKirimNota.CRUD.Create(dTLKonsepKirimNota))
                    return Json(new[] { dTLKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(dTLKonsepKirimNota);
            }
            return View(dTLKonsepKirimNota);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DTLKonsepKirimNotaUpdate([DataSourceRequest] DataSourceRequest request, DTLKonsepKirimNota dTLKonsepKirimNota)
        {
            if (dTLKonsepKirimNota != null && ModelState.IsValid)
            {
                if (DTLKonsepKirimNota.CRUDDTLKonsepKirimNota.CRUD.Update(dTLKonsepKirimNota))
                    return Json(new[] { dTLKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(dTLKonsepKirimNota);
            }
            return View(dTLKonsepKirimNota);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DTLKonsepKirimNotaDestroy([DataSourceRequest] DataSourceRequest request, DTLKonsepKirimNota dTLKonsepKirimNota)
        {
            if (dTLKonsepKirimNota != null)
            {
                if (DTLKonsepKirimNota.CRUDDTLKonsepKirimNota.CRUD.Delete(dTLKonsepKirimNota))
                    return Json(new[] { dTLKonsepKirimNota }.ToDataSourceResult(request, ModelState));
                else
                    return View(dTLKonsepKirimNota);
            }
            return View(dTLKonsepKirimNota);
        }

        [Authorize]
        public ActionResult deleteHeader(string hDRKonsepKirimNotaId)
        {
            var model = HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Get1Record(Guid.Parse(hDRKonsepKirimNotaId));
            if (HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Delete(model))
                return Json(true, JsonRequestBehavior.AllowGet);
            else
                return Json(false, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        [HttpPost]
        public ActionResult simpanHeader(string hDRKonsepKirimNotaId, string kebunPengirimId, string kebunPenerimaId,
            string nomor, string tanggal, string pembuat, string status, string keterangan, string statusDK)
        {
            try
            {

                var model = new HDRKonsepKirimNota {
                    HDRKonsepKirimNotaId=Guid.Parse(hDRKonsepKirimNotaId),
                    KebunPengirimId= Guid.Parse(kebunPengirimId),
                    KebunPenerimaId= Guid.Parse(kebunPenerimaId),
                    Nomor=int.Parse(nomor),
                    Tanggal=DateTime.Parse(tanggal),
                    Pembuat=pembuat,
                    Status=status,
                    Keterangan=keterangan,
                    StatusDK=statusDK,
                    NomorNota = "DN-" + int.Parse(nomor).ToString("D5") + " - [" + DateTime.Parse(tanggal).ToString() + "]",
                    KebunPenerima = CRUDKebun.CRUD.Get1Record(Guid.Parse(kebunPenerimaId)),
                    KebunPengirim = CRUDKebun.CRUD.Get1Record(Guid.Parse(kebunPengirimId)),
                    TahunBuku = (int)HttpContext.Session["TAHUNBUKU"]
                };

                if (HDRKonsepKirimNota.CRUDHDRKonsepKirimNota.CRUD.Update(model))
                    return Json(true, JsonRequestBehavior.AllowGet);
                else
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}