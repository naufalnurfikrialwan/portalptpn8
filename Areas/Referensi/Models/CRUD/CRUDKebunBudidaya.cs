using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKebunBudidaya
    {
        public static CRUDKebunBudidaya CRUD = new CRUDKebunBudidaya();

        public List<KebunBudidaya> ListKebunBudidaya
        {
            get
            {
                List<KebunBudidaya> result = (List<KebunBudidaya>)HttpContext.Current.Session["ListKebunBudidaya"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKebunBudidaya"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(KebunBudidaya kebunBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.KebunBudidaya.Add(kebunBudidaya);
                db.SaveChanges();
                ListKebunBudidaya.Add(kebunBudidaya);
                HttpContext.Current.Session["ListKebunBudidaya"] = ListKebunBudidaya;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<KebunBudidaya> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunBudidaya.ToList();
                foreach (var m in model)
                {
                    m.Budidaya = CRUDBudidaya.CRUD.Get1Record(m.MainBudidayaId);
                    m.Kebun = CRUDKebun.CRUD.Get1Record(m.KebunId);
                }
                return model;
            }
            catch
            {
                return new List<KebunBudidaya>();
            }
        }
        public bool Update(KebunBudidaya kebunBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunBudidaya.FirstOrDefault(o => o.KebunBudidayaId == kebunBudidaya.KebunBudidayaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kebunBudidaya);
                }
                else
                {
                    model.InjectFrom(kebunBudidaya);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKebunBudidaya.FirstOrDefault(o => o.KebunBudidayaId == kebunBudidaya.KebunBudidayaId);
                    context.InjectFrom(kebunBudidaya);
                    HttpContext.Current.Session["ListKebunBudidaya"] = ListKebunBudidaya;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(KebunBudidaya kebunBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KebunBudidaya.FirstOrDefault(o => o.KebunBudidayaId == kebunBudidaya.KebunBudidayaId);
                model.InjectFrom(kebunBudidaya);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kebunBudidaya);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(KebunBudidaya kebunBudidaya)
        {
            try
            {
                var context = ListKebunBudidaya.FirstOrDefault(o => o.KebunBudidayaId == kebunBudidaya.KebunBudidayaId);
                ListKebunBudidaya.Remove(context);
                HttpContext.Current.Session["ListKebunBudidaya"] = ListKebunBudidaya;
                CRUDPropinsi.CRUD.Erase(kebunBudidaya.KebunBudidayaId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid  kebunId)
        {
            try
            {
                var context = ListKebunBudidaya.Where(o => o.KebunId == kebunId);
                foreach (var m in context)
                {
                    ListKebunBudidaya.Remove(m);
                }
                HttpContext.Current.Session["ListKebunBudidaya"] = ListKebunBudidaya;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public KebunBudidaya Get1Record(Guid kebunBudidayaId)
        {
            var model = ListKebunBudidaya.FirstOrDefault(o => o.KebunBudidayaId == kebunBudidayaId);
            return model;
        }
        public KebunBudidaya Get1Record(string kodeMerk)
        {
            var model = ListKebunBudidaya.FirstOrDefault(o => o.kd_merk == kodeMerk);
            return model;
        }

        public List<KebunBudidaya> GetnRecord(Guid kebunBudidayaId)
        {
            var model = ListKebunBudidaya.Where(o => o.KebunBudidayaId == kebunBudidayaId).ToList();
            return model;
        }

        public List<KebunBudidaya> GetAllRecord()
        {
            var model = ListKebunBudidaya;
            return model;
        }
    }

    public class MerkAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var kebunBudidaya = (KebunBudidaya)value;
                var model = CRUDKebunBudidaya.CRUD.Get1Record(kebunBudidaya.kd_merk);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Merk Tidak Valid");
            }
            else
                return new ValidationResult("Merk Harus Diisi");
        }
    }

    public class MerkIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var merkId = (Guid)value;
                var model = CRUDKebunBudidaya.CRUD.Get1Record(merkId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Merk Tidak Valid");
            }
            else
                return new ValidationResult("Merk Harus Diisi");
        }
    }
}