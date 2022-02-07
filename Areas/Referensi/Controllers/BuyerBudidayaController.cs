using System;
using System.Linq;
using System.Web.Mvc;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.IO;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class BuyerBudidayaController : Controller
    {

        // GET: BuyerBudidaya
        [Authorize]
        public ActionResult Index()
        {
            var model = new BuyerBudidaya();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerBudidayaRead(Guid mainBudidayaId, [DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDBuyerBudidaya.CRUD.GetAllRecord().Where(o => o.MainBudidayaId == mainBudidayaId);
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerBudidayaCreate([DataSourceRequest]DataSourceRequest request,
            BuyerBudidaya buyerBudidaya)
        {
            if (buyerBudidaya != null && ModelState.IsValid)
            {
                CRUDBuyerBudidaya.CRUD.Create(buyerBudidaya);
                //CRUDBuyer.CRUD.EraseAll();
            }
            return Json(new[] { buyerBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerBudidayaUpdate([DataSourceRequest] DataSourceRequest request, BuyerBudidaya buyerBudidaya)
        {
            if (buyerBudidaya != null && ModelState.IsValid)
            {
                CRUDBuyerBudidaya.CRUD.Update(buyerBudidaya);
                //CRUDBuyer.CRUD.EraseAll();
            }
            return Json(new[] { buyerBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerBudidayaDestroy([DataSourceRequest] DataSourceRequest request, BuyerBudidaya buyerBudidaya)
        {
            if (buyerBudidaya != null)
            {
                CRUDBuyerBudidaya.CRUD.Delete(buyerBudidaya);
                //CRUDBuyer.CRUD.EraseAll();
            }
            return Json(new[] { buyerBudidaya }.ToDataSourceResult(request, ModelState));
        }
    }
}