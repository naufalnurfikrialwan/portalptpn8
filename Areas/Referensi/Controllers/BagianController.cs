using System;
using System.Linq;
using System.Web.Mvc;
using Ptpn8.Areas.Referensi.Models;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models.CRUD;


namespace Ptpn8.Areas.Referensi.Controllers
{
    public class BagianController : Controller
    {
        // GET: Bagian
        [Authorize]
        public ActionResult Index()
        {
            var model = new Bagian();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult bagianRead(Guid direksiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDBagian.CRUD.ListBagian.Where(o => o.DireksiId == direksiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult bagianCreate([DataSourceRequest]DataSourceRequest request,
            Bagian bagian)
        {
            if (bagian != null && ModelState.IsValid)
            {
                CRUDBagian.CRUD.Create(bagian);
            }
            return Json(new[] { bagian }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult bagianUpdate([DataSourceRequest] DataSourceRequest request, Bagian bagian)
        {
            if (bagian != null && ModelState.IsValid)
            {
                CRUDBagian.CRUD.Update(bagian);
            }
            return Json(new[] { bagian }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult bagianDestroy([DataSourceRequest] DataSourceRequest request, Bagian bagian)
        {
            if (bagian != null)
            {
                CRUDBagian.CRUD.Delete(bagian);
            }
            return Json(new[] { bagian }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult UrusanRead(Guid bagianId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDUrusan.CRUD.ListUrusan.Where(o => o.BagianId == bagianId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult UrusanCreate([DataSourceRequest]DataSourceRequest request,
            Urusan urusan)
        {
            if (urusan != null && ModelState.IsValid)
            {
                CRUDUrusan.CRUD.Create(urusan);
            }
            return Json(new[] { urusan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult UrusanUpdate([DataSourceRequest] DataSourceRequest request, Urusan urusan)
        {
            if (urusan != null && ModelState.IsValid)
            {
                CRUDUrusan.CRUD.Update(urusan);
            }
            return Json(new[] { urusan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult UrusanDestroy([DataSourceRequest] DataSourceRequest request, Urusan urusan)
        {
            if (urusan != null)
            {
                CRUDUrusan.CRUD.Delete(urusan);
            }
            return Json(new[] { urusan }.ToDataSourceResult(request, ModelState));
        }
    }
}