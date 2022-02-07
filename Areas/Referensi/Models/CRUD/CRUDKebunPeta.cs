using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKebunPeta
    {
        public static CRUDKebunPeta CRUD = new CRUDKebunPeta();

        public List<KebunPeta> ListKebunPeta
        {
            get
            {
                List<KebunPeta> result = (List<KebunPeta>)HttpContext.Current.Session["ListKebunPeta"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKebunPeta"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(KebunPeta kebunPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.KebunPeta.Add(kebunPeta);
                db.SaveChanges();

                ListKebunPeta.Add(kebunPeta);
                HttpContext.Current.Session["ListKebunPeta"] = ListKebunPeta;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<KebunPeta> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunPeta.ToList();
                foreach(var m in model)
                {
                    m.Kebun = CRUDKebun.CRUD.Get1Record(m.KebunId);
                }
                return model;
            }
            catch
            {
                return new List<KebunPeta>();
            }
        }
        public bool Update(KebunPeta kebunPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunPeta.FirstOrDefault(o => o.KebunPetaId == kebunPeta.KebunPetaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kebunPeta);
                }
                else
                {
                    model.InjectFrom(kebunPeta);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKebunPeta.FirstOrDefault(o => o.KebunPetaId == kebunPeta.KebunPetaId);
                    context.InjectFrom(kebunPeta);
                    HttpContext.Current.Session["ListKebunPeta"] = ListKebunPeta;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(KebunPeta kebunPeta)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunPeta.FirstOrDefault(o => o.KebunPetaId == kebunPeta.KebunPetaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kebunPeta);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(KebunPeta kebunPeta)
        {
            try
            {
                var context = ListKebunPeta.FirstOrDefault(o => o.KebunPetaId == kebunPeta.KebunPetaId);
                ListKebunPeta.Remove(context);
                HttpContext.Current.Session["ListKebunPeta"] = ListKebunPeta;
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
                var context = ListKebunPeta.Where(o => o.KebunId == kebunId).ToList();
                foreach (var m in context)
                {
                    ListKebunPeta.Remove(m);
                }
                HttpContext.Current.Session["ListKebunPeta"] = ListKebunPeta;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public KebunPeta Get1Record(Guid kebunPetaId)
        {
            var model = ListKebunPeta.FirstOrDefault(o => o.KebunPetaId == kebunPetaId);
            return model;
        }

        public KebunPeta Get1Record(string kodeKebun)
        {
            var model = ListKebunPeta.FirstOrDefault(o => o.Kebun.kd_kbn == kodeKebun);
            return model;
        }

        public List<KebunPeta> GetnRecord(Guid kebunPetaId)
        {
            var model = ListKebunPeta.Where(o => o.KebunPetaId == kebunPetaId).ToList();
            return model;
        }

        public List<KebunPeta> GetAllRecord()
        {
            var model = ListKebunPeta;
            return model;
        }
    }
}