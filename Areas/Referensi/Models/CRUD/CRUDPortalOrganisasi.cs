using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDPortalOrganisasi
    {
        public static CRUDPortalOrganisasi CRUD = new CRUDPortalOrganisasi();

        public List<PortalOrganisasi> ListPortalOrganisasi
        {
            get
            {
                List<PortalOrganisasi> result = (List<PortalOrganisasi>)HttpContext.Current.Session["ListPortalOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListPortalOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(PortalOrganisasi portalOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.PortalOrganisasi.Add(portalOrganisasi);
                db.SaveChanges();

                ListPortalOrganisasi.Add(portalOrganisasi);
                HttpContext.Current.Session["ListPortalOrganisasi"] = ListPortalOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<PortalOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.PortalOrganisasi.ToList();
                return model;
            }
            catch
            {
                return new List<PortalOrganisasi>();
            }
        }
        public bool Update(PortalOrganisasi portalOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.PortalOrganisasi.FirstOrDefault(o => o.PortalOrganisasiId == portalOrganisasi.PortalOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(portalOrganisasi);
                }
                else
                {
                    model.InjectFrom(portalOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListPortalOrganisasi.FirstOrDefault(o => o.PortalOrganisasiId == portalOrganisasi.PortalOrganisasiId);
                    context.InjectFrom(portalOrganisasi);
                    HttpContext.Current.Session["ListPortalOrganisasi"] = ListPortalOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(PortalOrganisasi portalOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.PortalOrganisasi.FirstOrDefault(o => o.PortalOrganisasiId == portalOrganisasi.PortalOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(portalOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(PortalOrganisasi portalOrganisasi)
        {
            try
            {
                var context = ListPortalOrganisasi.FirstOrDefault(o => o.PortalOrganisasiId == portalOrganisasi.PortalOrganisasiId);
                ListPortalOrganisasi.Remove(context);
                HttpContext.Current.Session["ListPortalOrganisasi"] = ListPortalOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid organisasiId)
        {
            try
            {
                var context = ListPortalOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListPortalOrganisasi.Remove(m);
                }
                HttpContext.Current.Session["ListPortalOrganisasi"] = ListPortalOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public PortalOrganisasi Get1Record(Guid portalOrganisasiId)
        {
            var model = ListPortalOrganisasi.FirstOrDefault(o => o.PortalOrganisasiId == portalOrganisasiId);
            return model;
        }

        public List<PortalOrganisasi> GetnRecord(Guid portalOrganisasiId)
        {
            var model = ListPortalOrganisasi.Where(o => o.PortalOrganisasiId == portalOrganisasiId).ToList();
            return model;
        }

        public List<PortalOrganisasi> GetAllRecord()
        {
            var model = ListPortalOrganisasi;
            return model;
        }
    }
}