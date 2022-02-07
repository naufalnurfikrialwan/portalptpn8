using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDDireksi
    {
        public static CRUDDireksi CRUD = new CRUDDireksi();

        public List<Direksi> ListDireksi
        {
            get
            {
                List<Direksi> result = (List<Direksi>)HttpContext.Current.Session["ListDireksi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListDireksi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Direksi direksi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Direksi.Add(direksi);
                db.SaveChanges();

                ListDireksi.Add(direksi);
                HttpContext.Current.Session["ListDireksi"] = ListDireksi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Direksi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Direksi.ToList();
                foreach(var m in model)
                {
                    m.Organisasi = CRUDOrganisasi.CRUD.Get1Record(m.OrganisasiId);
                }
                return model;
            }
            catch
            {
                return new List<Direksi>();
            }
        }
        public bool Update(Direksi direksi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Direksi.FirstOrDefault(o => o.DireksiId == direksi.DireksiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(direksi);
                }
                else
                {
                    model.InjectFrom(direksi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListDireksi.FirstOrDefault(o => o.DireksiId == direksi.DireksiId);
                    context.InjectFrom(direksi);
                    HttpContext.Current.Session["ListDireksi"] = ListDireksi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Direksi direksi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Direksi.FirstOrDefault(o => o.DireksiId == direksi.DireksiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(direksi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Direksi direksi)
        {
            try
            {
                var context = ListDireksi.FirstOrDefault(o => o.DireksiId == direksi.DireksiId);
                ListDireksi.Remove(context);
                HttpContext.Current.Session["ListDireksi"] = ListDireksi;
                CRUDBagian.CRUD.Erase(direksi.DireksiId);
                CRUDWilayah.CRUD.Erase(direksi.DireksiId);
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
                var context = ListDireksi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListDireksi.Remove(m);
                    CRUDBagian.CRUD.Erase(m.DireksiId);
                    CRUDWilayah.CRUD.Erase(m.DireksiId);
                }
                HttpContext.Current.Session["ListDireksi"] = ListDireksi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Direksi Get1Record(Guid direksiId)
        {
            var model = ListDireksi.FirstOrDefault(o => o.DireksiId == direksiId);
            return model;
        }

        public List<Direksi> GetnRecord(Guid direksiId)
        {
            var model = ListDireksi.Where(o => o.DireksiId == direksiId).ToList();
            return model;
        }

        public List<Direksi> GetAllRecord()
        {
            var model = ListDireksi;
            return model;
        }
    }
}