using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKepTan
    {
        public static CRUDKepTan CRUD = new CRUDKepTan();

        public List<KepTan> ListKepTan
        {
            get
            {
                List<KepTan> result = (List<KepTan>)HttpContext.Current.Session["ListKepTan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKepTan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(KepTan kepTan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.KepTan.Add(kepTan);
                db.SaveChanges();

                ListKepTan.Add(kepTan);
                HttpContext.Current.Session["ListKepTan"] = ListKepTan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<KepTan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KepTan.ToList();
                foreach (var m in model)
                {
                }
                return model;
            }
            catch
            {
                return new List<KepTan>();
            }
        }
        public bool Update(KepTan kepTan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KepTan.FirstOrDefault(o => o.KepTanId == kepTan.KepTanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kepTan);
                }
                else
                {
                    model.InjectFrom(kepTan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKepTan.FirstOrDefault(o => o.KepTanId == kepTan.KepTanId);
                    context.InjectFrom(kepTan);
                    HttpContext.Current.Session["ListKepTan"] = ListKepTan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(KepTan kepTan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KepTan.FirstOrDefault(o => o.KepTanId == kepTan.KepTanId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kepTan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(KepTan kepTan)
        {
            try
            {
                var context = ListKepTan.FirstOrDefault(o => o.KepTanId == kepTan.KepTanId);
                ListKepTan.Remove(context);
                HttpContext.Current.Session["ListKepTan"] = ListKepTan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public KepTan Get1Record(Guid kepTanId)
        {
            var model = ListKepTan.FirstOrDefault(o => o.KepTanId == kepTanId);
            return model;
        }

        public List<KepTan> GetAllRecord()
        {
            var model = ListKepTan;
            return model;
        }
    }

    public class KepTanIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var kepTanId = (Guid)value;
                var model = CRUDKepTan.CRUD.Get1Record(kepTanId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("KepTan Tidak Valid");
            }
            else
                return new ValidationResult("KepTan Harus Diisi");
        }
    }
}