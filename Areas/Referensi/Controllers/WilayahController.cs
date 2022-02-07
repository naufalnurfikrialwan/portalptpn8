using System;
using System.Linq;
using System.Web.Mvc;
using Ptpn8.Areas.Referensi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models.CRUD;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class WilayahController : Controller
    {
        // GET: Wilayah
        [Authorize]
        public ActionResult Index()
        {
            var model = new Wilayah();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult wilayahRead(Guid direksiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDWilayah.CRUD.ListWilayah.Where(o=>o.DireksiId == direksiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult wilayahCreate([DataSourceRequest]DataSourceRequest request,
            Wilayah wilayah)
        {
            if (wilayah != null && ModelState.IsValid)
            {
                CRUDWilayah.CRUD.Create(wilayah);
            }
            return Json(new[] { wilayah }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult wilayahUpdate([DataSourceRequest] DataSourceRequest request, Wilayah wilayah)
        {
            if (wilayah != null && ModelState.IsValid)
            {
                CRUDWilayah.CRUD.Update(wilayah);
            }
            return Json(new[] { wilayah }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult wilayahDestroy([DataSourceRequest] DataSourceRequest request, Wilayah wilayah)
        {
            if (wilayah != null)
            {
                CRUDWilayah.CRUD.Delete(wilayah);
            }
            return Json(new[] { wilayah }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BidangRead(Guid wilayahId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDBidang.CRUD.ListBidang.Where(o => o.WilayahId == wilayahId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BidangCreate([DataSourceRequest]DataSourceRequest request,
            Bidang bidang)
        {
            if (bidang != null && ModelState.IsValid)
            {
                CRUDBidang.CRUD.Create(bidang);
            }
            return Json(new[] { bidang }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BidangUpdate([DataSourceRequest] DataSourceRequest request, Bidang bidang)
        {
            if (bidang != null && ModelState.IsValid)
            {
                CRUDBidang.CRUD.Update(bidang);
            }
            return Json(new[] { bidang }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BidangDestroy([DataSourceRequest] DataSourceRequest request, Bidang bidang)
        {
            if (bidang != null)
            {
                CRUDBidang.CRUD.Delete(bidang);
            }
            return Json(new[] { bidang }.ToDataSourceResult(request, ModelState));
        }

    }
}