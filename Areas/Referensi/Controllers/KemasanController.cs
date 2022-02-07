using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class KemasanController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = new Kemasan();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult KemasanRead([DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDKemasan.CRUD.GetAllRecord().ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KemasanCreate([DataSourceRequest]DataSourceRequest request,
            Kemasan Kemasan)
        {
            if (Kemasan != null && ModelState.IsValid)
            {
                CRUDKemasan.CRUD.Create(Kemasan);
            }
            return Json(new[] { Kemasan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KemasanUpdate([DataSourceRequest] DataSourceRequest request, Kemasan Kemasan)
        {
            if (Kemasan != null && ModelState.IsValid)
            {
                CRUDKemasan.CRUD.Update(Kemasan);
            }
            return Json(new[] { Kemasan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KemasanDestroy([DataSourceRequest] DataSourceRequest request, Kemasan Kemasan)
        {
            if (Kemasan != null)
            {
                CRUDKemasan.CRUD.Delete(Kemasan);
            }
            return Json(new[] { Kemasan }.ToDataSourceResult(request, ModelState));
        }
    }
}