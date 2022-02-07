using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class KebunController : Controller
    {
        // GET: Kebun
        [Authorize]
        public ActionResult Index()
        {
            var model = new Kebun();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunRead(Guid wilayahId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDKebun.CRUD.ListKebun.Where(o => o.WilayahId == wilayahId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunCreate([DataSourceRequest]DataSourceRequest request,
            Kebun kebun)
        {
            if (kebun != null && ModelState.IsValid)
            {
                CRUDKebun.CRUD.Create(kebun);
            }
            return Json(new[] { kebun }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunUpdate([DataSourceRequest] DataSourceRequest request, Kebun kebun)
        {
            if (kebun != null && ModelState.IsValid)
            {
                CRUDKebun.CRUD.Update(kebun);
            }
            return Json(new[] { kebun }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunDestroy([DataSourceRequest] DataSourceRequest request, Kebun kebun)
        {
            if (kebun != null)
            {
                CRUDKebun.CRUD.Delete(kebun);
            }
            return Json(new[] { kebun }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AfdelingRead(Guid kebunId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDAfdeling.CRUD.ListAfdeling.Where(o => o.KebunId == kebunId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AfdelingCreate([DataSourceRequest]DataSourceRequest request,
            Afdeling afdeling)
        {
            if (afdeling != null && ModelState.IsValid)
            {
                CRUDAfdeling.CRUD.Create(afdeling);
            }
            return Json(new[] { afdeling }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AfdelingUpdate([DataSourceRequest] DataSourceRequest request, Afdeling afdeling)
        {
            if (afdeling != null && ModelState.IsValid)
            {
                CRUDAfdeling.CRUD.Update(afdeling);
            }
            return Json(new[] { afdeling }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult AfdelingDestroy([DataSourceRequest] DataSourceRequest request, Afdeling afdeling)
        {
            if (afdeling != null)
            {
                CRUDAfdeling.CRUD.Delete(afdeling);
            }
            return Json(new[] { afdeling }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunBudidayaRead(Guid kebunId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDKebunBudidaya.CRUD.ListKebunBudidaya.Where(o => o.KebunId == kebunId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunBudidayaCreate([DataSourceRequest]DataSourceRequest request,
            KebunBudidaya kebunBudidaya)
        {
            if (kebunBudidaya != null && ModelState.IsValid)
            {
                CRUDKebunBudidaya.CRUD.Create(kebunBudidaya);
            }
            return Json(new[] { kebunBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunBudidayaUpdate([DataSourceRequest] DataSourceRequest request, KebunBudidaya kebunBudidaya)
        {
            if (ModelState.IsValid)
            {
                CRUDKebunBudidaya.CRUD.Update(kebunBudidaya);
            }
            return Json(new[] { kebunBudidaya }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KebunBudidayaDestroy([DataSourceRequest] DataSourceRequest request, KebunBudidaya kebunBudidaya)
        {
            if (kebunBudidaya != null)
            {
                CRUDKebunBudidaya.CRUD.Delete(kebunBudidaya);
            }
            return Json(new[] { kebunBudidaya }.ToDataSourceResult(request, ModelState));
        }
    }
}