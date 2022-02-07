using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDTehRelasi_DaftarPenerima
    {
        public static CRUDTehRelasi_DaftarPenerima CRUD = new CRUDTehRelasi_DaftarPenerima();

        public List<TehRelasi_DaftarPenerima> ListTehRelasi_DaftarPenerima
        {
            get
            {
                List<TehRelasi_DaftarPenerima> result = (List<TehRelasi_DaftarPenerima>)HttpContext.Current.Session["ListTehRelasi_DaftarPenerima"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListTehRelasi_DaftarPenerima"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(TehRelasi_DaftarPenerima tehRelasi_DaftarPenerima)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.TehRelasi_DaftarPenerima.Add(tehRelasi_DaftarPenerima);
                db.SaveChanges();

                ListTehRelasi_DaftarPenerima.Add(tehRelasi_DaftarPenerima);
                HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarPenerima;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<TehRelasi_DaftarPenerima> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarPenerima.ToList(); //.Include("VesselBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<TehRelasi_DaftarPenerima>();
            }
        }
        public bool Update(TehRelasi_DaftarPenerima tehRelasi_DaftarPenerima)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarPenerima.FirstOrDefault(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerima.TehRelasi_DaftarPenerimaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(tehRelasi_DaftarPenerima);
                }
                else
                {
                    model.InjectFrom(tehRelasi_DaftarPenerima);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListTehRelasi_DaftarPenerima.FirstOrDefault(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerima.TehRelasi_DaftarPenerimaId);
                    context.InjectFrom(tehRelasi_DaftarPenerima);
                    HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarPenerima;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(TehRelasi_DaftarPenerima tehRelasi_DaftarPenerima)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarPenerima.FirstOrDefault(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerima.TehRelasi_DaftarPenerimaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(tehRelasi_DaftarPenerima);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(TehRelasi_DaftarPenerima tehRelasi_DaftarPenerima)
        {
            try
            {
                var context = ListTehRelasi_DaftarPenerima.FirstOrDefault(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerima.TehRelasi_DaftarPenerimaId);
                ListTehRelasi_DaftarPenerima.Remove(context);
                HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarPenerima;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListTehRelasi_DaftarPenerima"] = null;
            return true;
        }

        public TehRelasi_DaftarPenerima Get1Record(Guid tehRelasi_DaftarPenerimaId)
        {
            var model = ListTehRelasi_DaftarPenerima.FirstOrDefault(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerimaId);
            return model;
        }

        public List<TehRelasi_DaftarPenerima> GetnRecord(Guid tehRelasi_DaftarPenerimaId)
        {
            var model = ListTehRelasi_DaftarPenerima.Where(o => o.TehRelasi_DaftarPenerimaId == tehRelasi_DaftarPenerimaId).ToList();
            return model;
        }

        public List<TehRelasi_DaftarPenerima> GetAllRecord()
        {
            var model = ListTehRelasi_DaftarPenerima;
            return model;
        }
    }

    public class TehRelasi_DaftarPenerimaAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var tehRelasi_DaftarPenerima = (TehRelasi_DaftarPenerima)value;
                var model = CRUDTehRelasi_DaftarPenerima.CRUD.Get1Record(tehRelasi_DaftarPenerima.TehRelasi_DaftarPenerimaId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Tidak Valid");
            }
            else
                return new ValidationResult("Harus Diisi");
        }
    }

    public class TehRelasi_DaftarPenerimaIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var tehRelasi_DaftarPenerimaId = (Guid)value;
                var model = CRUDTehRelasi_DaftarPenerima.CRUD.Get1Record(tehRelasi_DaftarPenerimaId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Tidak Valid");
            }
            else
                return new ValidationResult("Harus Diisi");
        }
    }
}