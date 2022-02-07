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
    public class OrganisasiController : Controller
    {

        // GET: Organisasi
        [Authorize]
        public ActionResult Index()
        {
            var model = new Organisasi();
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public ActionResult organisasiRead([DataSourceRequest]DataSourceRequest request)
        {
            var model = CRUDOrganisasi.CRUD.GetAllRecord();
            foreach (var m in model)
            {
                CRUDImage.CRUD.ReadToView(m.FileLogo, m.Logo);
            }
            return Json(model.ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult organisasiCreate([DataSourceRequest]DataSourceRequest request, 
            Organisasi organisasi)
        {
            if(organisasi!=null && ModelState.IsValid)
            {
                CRUDOrganisasi.CRUD.Create(organisasi);
            }
            return Json(new[] { organisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult organisasiUpdate([DataSourceRequest] DataSourceRequest request, Organisasi organisasi)
        {
            if (organisasi != null && ModelState.IsValid)
            {
                organisasi.Logo = CRUDImage.CRUD.GetFromUpload(organisasi.FileLogo, organisasi.Logo);
                CRUDOrganisasi.CRUD.Update(organisasi);
            }
            return Json(new[] { organisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult organisasiDestroy([DataSourceRequest] DataSourceRequest request, Organisasi organisasi)
        {
            if (organisasi != null)
            {
                CRUDOrganisasi.CRUD.Delete(organisasi);
            }
            return Json(new[] { organisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult TeleponOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDTeleponOrganisasi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult TeleponOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            TeleponOrganisasi teleponOrganisasi)
        {
            if (teleponOrganisasi != null && ModelState.IsValid)
            {
                CRUDTeleponOrganisasi.CRUD.Create(teleponOrganisasi);
            }
            return Json(new[] { teleponOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult TeleponOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, TeleponOrganisasi teleponOrganisasi)
        {
            if (teleponOrganisasi != null && ModelState.IsValid)
            {
                CRUDTeleponOrganisasi.CRUD.Update(teleponOrganisasi);
            }
            return Json(new[] { teleponOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult TeleponOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, TeleponOrganisasi teleponOrganisasi)
        {
            if (teleponOrganisasi != null)
            {
                CRUDTeleponOrganisasi.CRUD.Delete(teleponOrganisasi);
            }
            return Json(new[] { teleponOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult FaksimiliOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDFaksimiliOrganisasi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult FaksimiliOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            FaksimiliOrganisasi faksimiliOrganisasi)
        {
            if (faksimiliOrganisasi != null && ModelState.IsValid)
            {
                CRUDFaksimiliOrganisasi.CRUD.Create(faksimiliOrganisasi);
            }
            return Json(new[] { faksimiliOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult FaksimiliOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, FaksimiliOrganisasi faksimiliOrganisasi)
        {
            if (faksimiliOrganisasi != null && ModelState.IsValid)
            {
                CRUDFaksimiliOrganisasi.CRUD.Update(faksimiliOrganisasi);
            }
            return Json(new[] { faksimiliOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult FaksimiliOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, FaksimiliOrganisasi faksimiliOrganisasi)
        {
            if (faksimiliOrganisasi != null)
            {
                CRUDFaksimiliOrganisasi.CRUD.Delete(faksimiliOrganisasi);
            }
            return Json(new[] { faksimiliOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDEmailOrganisasi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            EmailOrganisasi emailOrganisasi)
        {
            if (emailOrganisasi != null && ModelState.IsValid)
            {
                CRUDEmailOrganisasi.CRUD.Create(emailOrganisasi);
            }
            return Json(new[] { emailOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, EmailOrganisasi emailOrganisasi)
        {
            if (emailOrganisasi != null && ModelState.IsValid)
            {
                CRUDEmailOrganisasi.CRUD.Update(emailOrganisasi);
            }
            return Json(new[] { emailOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult EmailOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, EmailOrganisasi emailOrganisasi)
        {
            if (emailOrganisasi != null)
            {
                CRUDEmailOrganisasi.CRUD.Delete(emailOrganisasi);
            }
            return Json(new[] { emailOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult PortalOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDPortalOrganisasi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult PortalOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            PortalOrganisasi portalOrganisasi)
        {
            if (portalOrganisasi != null && ModelState.IsValid)
            {
                CRUDPortalOrganisasi.CRUD.Create(portalOrganisasi);
            }
            return Json(new[] { portalOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult PortalOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, PortalOrganisasi portalOrganisasi)
        {
            if (portalOrganisasi != null && ModelState.IsValid)
            {
                CRUDPortalOrganisasi.CRUD.Update(portalOrganisasi);
            }
            return Json(new[] { portalOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult PortalOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, PortalOrganisasi portalOrganisasi)
        {
            if (portalOrganisasi != null)
            {
                CRUDPortalOrganisasi.CRUD.Delete(portalOrganisasi);
            }
            return Json(new[] { portalOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult NPWPOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDNPWPOrganisasi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult NPWPOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            NPWPOrganisasi npwpOrganisasi)
        {
            if (npwpOrganisasi != null && ModelState.IsValid)
            {
                CRUDNPWPOrganisasi.CRUD.Create(npwpOrganisasi);
            }
            return Json(new[] { npwpOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult NPWPOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, NPWPOrganisasi npwpOrganisasi)
        {
            if (npwpOrganisasi != null && ModelState.IsValid)
            {
                CRUDNPWPOrganisasi.CRUD.Update(npwpOrganisasi);
            }
            return Json(new[] { npwpOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult NPWPOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, NPWPOrganisasi npwpOrganisasi)
        {
            if (npwpOrganisasi != null)
            {
                CRUDNPWPOrganisasi.CRUD.Delete(npwpOrganisasi);
            }
            return Json(new[] { npwpOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KomisarisOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDKomisaris.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KomisarisOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            Komisaris komisarisOrganisasi)
        {
            if (komisarisOrganisasi != null && ModelState.IsValid)
            {
                CRUDKomisaris.CRUD.Create(komisarisOrganisasi);
            }
            return Json(new[] { komisarisOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KomisarisOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, Komisaris komisarisOrganisasi)
        {
            if (komisarisOrganisasi != null && ModelState.IsValid)
            {
                CRUDKomisaris.CRUD.Update(komisarisOrganisasi);
            }
            return Json(new[] { komisarisOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult KomisarisOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, Komisaris komisarisOrganisasi)
        {
            if (komisarisOrganisasi != null)
            {
                CRUDKomisaris.CRUD.Delete(komisarisOrganisasi);
            }
            return Json(new[] { komisarisOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DireksiOrganisasiRead(Guid organisasiId, [DataSourceRequest]DataSourceRequest request)
        {
            return Json(CRUDDireksi.CRUD.GetAllRecord().Where(o=>o.OrganisasiId==organisasiId).ToDataSourceResult(request));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DireksiOrganisasiCreate([DataSourceRequest]DataSourceRequest request,
            Direksi direksiOrganisasi)
        {
            if (direksiOrganisasi != null && ModelState.IsValid)
            {
                CRUDDireksi.CRUD.Create(direksiOrganisasi);
            }
            return Json(new[] { direksiOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DireksiOrganisasiUpdate([DataSourceRequest] DataSourceRequest request, Direksi direksiOrganisasi)
        {
            if (direksiOrganisasi != null && ModelState.IsValid)
            {
                CRUDDireksi.CRUD.Update(direksiOrganisasi);
            }
            return Json(new[] { direksiOrganisasi }.ToDataSourceResult(request, ModelState));
        }

        [Authorize]
        [HttpPost]
        public ActionResult DireksiOrganisasiDestroy([DataSourceRequest] DataSourceRequest request, Direksi direksiOrganisasi)
        {
            if (direksiOrganisasi != null)
            {
                CRUDDireksi.CRUD.Delete(direksiOrganisasi);
            }
            return Json(new[] { direksiOrganisasi }.ToDataSourceResult(request, ModelState));
        }
        
    }
}