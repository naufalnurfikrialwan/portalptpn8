using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDTipeKamar
    {
        public static CRUDTipeKamar CRUD = new CRUDTipeKamar();

        public List<TipeKamar> ListTipeKamar
        {
            get
            {
                List<TipeKamar> result = (List<TipeKamar>)HttpContext.Current.Session["ListTipeKamar"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListTipeKamar"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(TipeKamar TipeKamar)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.TipeKamar.Add(TipeKamar);
                db.SaveChanges();

                ListTipeKamar.Add(TipeKamar);
                HttpContext.Current.Session["ListTipeKamar"] = ListTipeKamar;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<TipeKamar> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TipeKamar.ToList(); //.Include("BankOrganisasiBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<TipeKamar>();
            }
        }
        public bool Update(TipeKamar TipeKamar)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TipeKamar.FirstOrDefault(o => o.TipeKamarId == TipeKamar.TipeKamarId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(TipeKamar);
                }
                else
                {
                    model.InjectFrom(TipeKamar);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListTipeKamar.FirstOrDefault(o => o.TipeKamarId == TipeKamar.TipeKamarId);
                    context.InjectFrom(TipeKamar);
                    HttpContext.Current.Session["ListTipeKamar"] = ListTipeKamar;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(TipeKamar TipeKamar)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TipeKamar.FirstOrDefault(o => o.TipeKamarId == TipeKamar.TipeKamarId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(TipeKamar);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(TipeKamar TipeKamar)
        {
            try
            {
                var context = ListTipeKamar.FirstOrDefault(o => o.TipeKamarId == TipeKamar.TipeKamarId);
                ListTipeKamar.Remove(context);
                HttpContext.Current.Session["ListTipeKamar"] = ListTipeKamar;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListTipeKamar"] = null;
            return true;
        }

        public TipeKamar Get1Record(Guid TipeKamarId)
        {
            var model = ListTipeKamar.FirstOrDefault(o => o.TipeKamarId == TipeKamarId);

            return model;
        }

        public List<TipeKamar> GetnRecord(Guid TipeKamarId)
        {
            var model = ListTipeKamar.Where(o => o.TipeKamarId == TipeKamarId).ToList();
            return model;
        }

        public List<TipeKamar> GetAllRecord()
        {
            var model = ListTipeKamar;
            return model;
        }
    }

    public class TipeKamarAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var TipeKamar = (TipeKamar)value;
                var model = CRUDTipeKamar.CRUD.Get1Record(TipeKamar.TipeKamarId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("TipeKamar Tidak Valid");
            }
            else
                return new ValidationResult("TipeKamar Harus Diisi");
        }
    }

    public class TipeKamarIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var TipeKamarId = (Guid)value;
                var model = CRUDTipeKamar.CRUD.Get1Record(TipeKamarId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("TipeKamar Tidak Valid");
            }
            else
                return new ValidationResult("TipeKamar Harus Diisi");
        }
    }
}