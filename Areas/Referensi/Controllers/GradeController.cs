using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class GradeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            var model = new Grade();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult GradeRead(String subBudidayaId, [DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDGrade.CRUD.ListGrade.Where(o=>o.SubBudidayaId==Guid.Parse(subBudidayaId));
            foreach(var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileFoto, m.Foto);
            }
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GradeCreate([DataSourceRequest]DataSourceRequest request,
            Grade grade)
        {
            if (grade != null && ModelState.IsValid)
            {
                CRUDGrade.CRUD.Create(grade);
            }
            return Json(new[] { grade }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GradeUpdate([DataSourceRequest] DataSourceRequest request, Grade grade)
        {
            if (grade != null && ModelState.IsValid)
            {
                grade.Foto = CRUDImage.CRUD.GetFromUpload(grade.FileFoto, grade.Foto);
                CRUDGrade.CRUD.Update(grade);
            }
            return Json(new[] { grade }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult GradeDestroy([DataSourceRequest] DataSourceRequest request, Grade grade)
        {
            if (grade != null)
            {
                CRUDGrade.CRUD.Delete(grade);
            }
            return Json(new[] { grade }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult VarianRead(Guid gradeId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDVarian.CRUD.GetAllRecord().Where(o=>o.GradeId==gradeId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult VarianCreate([DataSourceRequest]DataSourceRequest request,
            Varian varian)
        {
            if (varian != null && ModelState.IsValid)
            {
                CRUDVarian.CRUD.Create(varian);
            }
            return Json(new[] { varian }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult VarianUpdate([DataSourceRequest] DataSourceRequest request, Varian varian)
        {
            if (varian != null && ModelState.IsValid)
            {
                CRUDVarian.CRUD.Update(varian);
            }
            return Json(new[] { varian }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult VarianDestroy([DataSourceRequest] DataSourceRequest request, Varian varian)
        {
            if (varian != null)
            {
                CRUDVarian.CRUD.Delete(varian);
            }
            return Json(new[] { varian }.ToDataSourceResult(request, ModelState));
        }
    }
}