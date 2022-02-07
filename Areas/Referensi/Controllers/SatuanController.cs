using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class SatuanController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = new Satuan();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SatuanRead([DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDSatuan.CRUD.GetAllRecord().ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SatuanCreate([DataSourceRequest]DataSourceRequest request,
            Satuan Satuan)
        {
            if (Satuan != null && ModelState.IsValid)
            {
                CRUDSatuan.CRUD.Create(Satuan);
            }
            return Json(new[] { Satuan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SatuanUpdate([DataSourceRequest] DataSourceRequest request, Satuan Satuan)
        {
            if (Satuan != null && ModelState.IsValid)
            {
                CRUDSatuan.CRUD.Update(Satuan);
            }
            return Json(new[] { Satuan }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult SatuanDestroy([DataSourceRequest] DataSourceRequest request, Satuan Satuan)
        {
            if (Satuan != null)
            {
                CRUDSatuan.CRUD.Delete(Satuan);
            }
            return Json(new[] { Satuan }.ToDataSourceResult(request, ModelState));
        }

    }
}