using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class BudidayaController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = new MainBudidaya();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult BudidayaRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDBudidaya.CRUD.GetAllRecord();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BudidayaCreate([DataSourceRequest]DataSourceRequest request,
            MainBudidaya Budidaya)
        {
            if (Budidaya != null && ModelState.IsValid)
            {
                CRUDBudidaya.CRUD.Create(Budidaya);
            }
            return Json(new[] { Budidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BudidayaUpdate([DataSourceRequest] DataSourceRequest request, MainBudidaya Budidaya)
        {
            if (Budidaya != null && ModelState.IsValid)
            {
                //Foto = CRUDImage.CRUD.GetFromUpload(FileFoto, Foto);
                CRUDBudidaya.CRUD.Update(Budidaya);
            }
            return Json(new[] { Budidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult BudidayaDestroy([DataSourceRequest] DataSourceRequest request, MainBudidaya Budidaya)
        {
            if (Budidaya != null)
            {
                CRUDBudidaya.CRUD.Update(Budidaya);
            }
            return Json(new[] { Budidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubBudidayaRead(Guid BudidayaId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDSubBudidaya.CRUD.GetAllRecord().Where(o=>o.MainBudidayaId== BudidayaId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubBudidayaCreate([DataSourceRequest]DataSourceRequest request,
            SubBudidaya subBudidaya)
        {
            if (subBudidaya != null && ModelState.IsValid)
            {
                CRUDSubBudidaya.CRUD.Create(subBudidaya);
            }
            return Json(new[] { subBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubBudidayaUpdate([DataSourceRequest] DataSourceRequest request, SubBudidaya subBudidaya)
        {
            if (subBudidaya != null && ModelState.IsValid)
            {
                CRUDSubBudidaya.CRUD.Update(subBudidaya);
            }
            return Json(new[] { subBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SubBudidayaDestroy([DataSourceRequest] DataSourceRequest request, SubBudidaya subBudidaya)
        {
            if (subBudidaya != null)
            {
                CRUDSubBudidaya.CRUD.Delete(subBudidaya);
            }
            return Json(new[] { subBudidaya }.ToDataSourceResult(request, ModelState));
        }
    }
}