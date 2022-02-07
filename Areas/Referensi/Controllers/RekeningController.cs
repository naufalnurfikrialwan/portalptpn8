using System.Web.Mvc;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class RekeningController : Controller
    {

        // GET: Rekening
        [Authorize]
        public ActionResult Index()
        {
            var model = new Rekening();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult rekeningRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDRekening.CRUD.GetAllRecord();
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult rekeningCreate([DataSourceRequest]DataSourceRequest request,
            Rekening rekening)
        {
            if (rekening != null && ModelState.IsValid)
            {
                CRUDRekening.CRUD.Create(rekening);
            }
            return Json(new[] { rekening }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult rekeningUpdate([DataSourceRequest] DataSourceRequest request, Rekening rekening)
        {
            if (ModelState.IsValid)
            {
                CRUDRekening.CRUD.Update(rekening);
            }
            return Json(new[] { rekening }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult rekeningDestroy([DataSourceRequest] DataSourceRequest request, Rekening rekening)
        {
            if (rekening != null)
            {
                CRUDRekening.CRUD.Delete(rekening);
            }
            return Json(new[] { rekening }.ToDataSourceResult(request, ModelState));
        }

    }
}