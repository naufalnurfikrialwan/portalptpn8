using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDAfdelingPeta
    {
        public static CRUDAfdelingPeta CRUD = new CRUDAfdelingPeta();

        public List<AfdelingPeta> ListAfdelingPeta
        {
            get
            {
                List<AfdelingPeta> result = (List<AfdelingPeta>)HttpContext.Current.Session["ListAfdelingPeta"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAfdelingPeta"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(AfdelingPeta afdelingPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.AfdelingPeta.Add(afdelingPeta);
                db.SaveChanges();

                ListAfdelingPeta.Add(afdelingPeta);
                HttpContext.Current.Session["ListAfdelingPeta"] = ListAfdelingPeta;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<AfdelingPeta> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AfdelingPeta.ToList();
                foreach(var m in model)
                {
                    m.Afdeling = CRUDAfdeling.CRUD.Get1Record(m.AfdelingId);
                }
                return model;
            }
            catch
            {
                return new List<AfdelingPeta>();
            }
        }
        public bool Update(AfdelingPeta afdelingPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AfdelingPeta.FirstOrDefault(o => o.AfdelingPetaId == afdelingPeta.AfdelingPetaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(afdelingPeta);
                }
                else
                {
                    model.InjectFrom(afdelingPeta);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListAfdelingPeta.FirstOrDefault(o => o.AfdelingPetaId == afdelingPeta.AfdelingPetaId);
                    context.InjectFrom(afdelingPeta);
                    HttpContext.Current.Session["ListAfdelingPeta"] = ListAfdelingPeta;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(AfdelingPeta afdelingPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AfdelingPeta.FirstOrDefault(o => o.AfdelingPetaId == afdelingPeta.AfdelingPetaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(afdelingPeta);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(AfdelingPeta afdelingPeta)
        {
            try
            {
                var context = ListAfdelingPeta.FirstOrDefault(o => o.AfdelingPetaId == afdelingPeta.AfdelingPetaId);
                ListAfdelingPeta.Remove(context);
                HttpContext.Current.Session["ListAfdelingPeta"] = ListAfdelingPeta;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid afdelingId)
        {
            try
            {
                var context = ListAfdelingPeta.Where(o => o.AfdelingId == afdelingId).ToList();
                foreach (var m in context)
                {
                    ListAfdelingPeta.Remove(m);
                }
                HttpContext.Current.Session["ListAfdelingPeta"] = ListAfdelingPeta;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public AfdelingPeta Get1Record(Guid afdelingPetaId)
        {
            var model = ListAfdelingPeta.FirstOrDefault(o => o.AfdelingPetaId == afdelingPetaId);
            return model;
        }

        public AfdelingPeta Get1Record(string kodeAfdeling)
        {
            var model = ListAfdelingPeta.FirstOrDefault(o => o.Afdeling.kd_afd == kodeAfdeling);
            return model;
        }

        public List<AfdelingPeta> GetnRecord(Guid afdelingPetaId)
        {
            var model = ListAfdelingPeta.Where(o => o.AfdelingPetaId == afdelingPetaId).ToList();
            return model;
        }

        public List<AfdelingPeta> GetAllRecord()
        {
            var model = ListAfdelingPeta;
            return model;
        }
    }
}