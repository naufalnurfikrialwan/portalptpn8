using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDPropinsi
    {
        public static CRUDPropinsi CRUD = new CRUDPropinsi();

        public List<Propinsi> ListPropinsi
        {
            get
            {
                List<Propinsi> result = (List<Propinsi>)HttpContext.Current.Session["ListPropinsi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListPropinsi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Propinsi propinsi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Propinsi.Add(propinsi);
                db.SaveChanges();

                ListPropinsi.Add(propinsi);
                HttpContext.Current.Session["ListPropinsi"] = ListPropinsi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Propinsi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Propinsi.ToList();
                return model;
            }
            catch
            {
                return new List<Propinsi>();
            }
        }
        public bool Update(Propinsi propinsi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Propinsi.FirstOrDefault(o => o.PropinsiId == propinsi.PropinsiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(propinsi);
                }
                else
                {
                    model.InjectFrom(propinsi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListPropinsi.FirstOrDefault(o => o.PropinsiId == propinsi.PropinsiId);
                    context.InjectFrom(propinsi);
                    HttpContext.Current.Session["ListPropinsi"] = ListPropinsi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Propinsi propinsi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Propinsi.FirstOrDefault(o => o.PropinsiId == propinsi.PropinsiId);
                model.InjectFrom(propinsi);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(propinsi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Propinsi propinsi)
        {
            try
            {
                var context = ListPropinsi.FirstOrDefault(o => o.PropinsiId == propinsi.PropinsiId);
                ListPropinsi.Remove(context);
                HttpContext.Current.Session["ListPropinsi"] = ListPropinsi;
                CRUDKota.CRUD.Erase(propinsi.PropinsiId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid negaraId)
        {
            try
            {
                var context = ListPropinsi.Where(o => o.NegaraId == negaraId).ToList();
                foreach (var m in context)
                {
                    ListPropinsi.Remove(m);
                    CRUDKota.CRUD.Erase(m.PropinsiId);
                }
                HttpContext.Current.Session["ListPropinsi"] = ListPropinsi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Propinsi Get1Record(Guid propinsiId)
        {
            var model = ListPropinsi.FirstOrDefault(o => o.PropinsiId == propinsiId);
            return model;
        }

        public List<Propinsi> GetnRecord(Guid negaraId)
        {
            var model = ListPropinsi.Where(o => o.NegaraId == negaraId).ToList();
            return model;
        }

        public List<Propinsi> GetAllRecord()
        {
            var model = ListPropinsi;
            return model;
        }
    }
}