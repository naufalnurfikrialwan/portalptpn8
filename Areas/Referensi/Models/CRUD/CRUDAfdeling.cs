using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDAfdeling
    {
        public static CRUDAfdeling CRUD = new CRUDAfdeling();

        public List<Afdeling> ListAfdeling
        {
            get
            {
                List<Afdeling> result = (List<Afdeling>)HttpContext.Current.Session["ListAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAfdeling"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Afdeling afdeling)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Afdeling.Add(afdeling);
                db.SaveChanges();

                ListAfdeling.Add(afdeling);
                HttpContext.Current.Session["ListAfdeling"] = ListAfdeling;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Afdeling> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Afdeling.ToList();
                foreach(var m in model)
                {
                    m.Kebun = new Kebun();
                    m.Kebun = CRUDKebun.CRUD.Get1Record(m.KebunId);
                }
                return model;
            }
            catch
            {
                return new List<Afdeling>();
            }
        }
        public bool Update(Afdeling afdeling)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Afdeling.FirstOrDefault(o => o.AfdelingId == afdeling.AfdelingId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(afdeling);
                }
                else
                {
                    model.InjectFrom(afdeling);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListAfdeling.FirstOrDefault(o => o.AfdelingId == afdeling.AfdelingId);
                    context.InjectFrom(afdeling);
                    HttpContext.Current.Session["ListAfdeling"] = ListAfdeling;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Afdeling afdeling)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Afdeling.FirstOrDefault(o => o.AfdelingId == afdeling.AfdelingId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(afdeling);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Afdeling afdeling)
        {
            try
            {
                var context = ListAfdeling.FirstOrDefault(o => o.AfdelingId == afdeling.AfdelingId);
                ListAfdeling.Remove(context);
                HttpContext.Current.Session["ListAfdeling"] = ListAfdeling;
                CRUDAfdelingPeta.CRUD.Erase(afdeling.AfdelingId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid kebunId)
        {
            try
            {
                var context = ListAfdeling.Where(o => o.KebunId== kebunId).ToList();
                foreach (var m in context)
                {
                    CRUDAfdelingPeta.CRUD.Erase(m.AfdelingId);
                    ListAfdeling.Remove(m);
                }
                HttpContext.Current.Session["ListAfdeling"] = ListAfdeling;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Afdeling Get1Record(Guid afdelingId)
        {
            var model = ListAfdeling.FirstOrDefault(o => o.AfdelingId == afdelingId);
            return model;
        }

        public Afdeling Get1Record(string kodeAfdeling)
        {
            var model = ListAfdeling.FirstOrDefault(o => o.kd_afd == kodeAfdeling);
            return model;
        }

        public List<Afdeling> GetnRecord(Guid afdelingId)
        {
            var model = ListAfdeling.Where(o => o.AfdelingId == afdelingId).ToList();
            return model;
        }

        public List<Afdeling> GetAllRecord()
        {
            var model = ListAfdeling;
            return model;
        }
    }
}