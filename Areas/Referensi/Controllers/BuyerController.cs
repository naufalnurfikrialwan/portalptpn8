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
    public class BuyerController : Controller
    {

        // GET: Buyer
        [Authorize]
        public ActionResult Index()
        {
            var model = new Buyer();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDBuyer.CRUD.GetAllRecord();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        public ActionResult GetBudidaya()
        {
            var model = CRUDBuyer.CRUD.GetAllRecord().GroupBy(o => o.DaftarBudidaya).ToList();
            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [HttpPost]
        public ActionResult buyerCreate([DataSourceRequest]DataSourceRequest request,
            Buyer buyer)
        {
            if (buyer != null && ModelState.IsValid)
            {
                CRUDBuyer.CRUD.Create(buyer);
            }
            return Json(new[] { buyer }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerUpdate([DataSourceRequest] DataSourceRequest request, Buyer buyer)
        {
            if (buyer != null && ModelState.IsValid)
            {
                buyer.Foto = CRUDImage.CRUD.GetFromUpload(buyer.FileFoto, buyer.Foto);
                CRUDBuyer.CRUD.Update(buyer);
            }
            return Json(new[] { buyer }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult buyerDestroy([DataSourceRequest] DataSourceRequest request, Buyer buyer)
        {
            if (buyer != null)
            {
                CRUDBuyer.CRUD.Delete(buyer);
            }
            return Json(new[] { buyer }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        public ActionResult buyerBudidayaRead(Guid buyerId, [DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDBuyerBudidaya.CRUD.GetAllRecord().Where(o => o.BuyerId == buyerId);
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