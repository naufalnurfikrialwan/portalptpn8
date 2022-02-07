using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDAlokasi
    {
        public static CRUDAlokasi CRUD = new CRUDAlokasi();

        public List<Alokasi> ListAlokasi
        {
            get
            {
                List<Alokasi> result = (List<Alokasi>)HttpContext.Current.Session["ListAlokasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAlokasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Alokasi alokasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Alokasi.Add(alokasi);
                db.SaveChanges();

                ListAlokasi.Add(alokasi);
                HttpContext.Current.Session["ListAlokasi"] = ListAlokasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Alokasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Alokasi.ToList(); //.Include("AlokasiBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<Alokasi>();
            }
        }
        public bool Update(Alokasi alokasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Alokasi.FirstOrDefault(o => o.AlokasiId == alokasi.AlokasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(alokasi);
                }
                else
                {
                    model.InjectFrom(alokasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListAlokasi.FirstOrDefault(o => o.AlokasiId == alokasi.AlokasiId);
                    context.InjectFrom(alokasi);
                    HttpContext.Current.Session["ListAlokasi"] = ListAlokasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Alokasi alokasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Alokasi.FirstOrDefault(o => o.AlokasiId == alokasi.AlokasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(alokasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Alokasi alokasi)
        {
            try
            {
                var context = ListAlokasi.FirstOrDefault(o => o.AlokasiId == alokasi.AlokasiId);
                ListAlokasi.Remove(context);
                HttpContext.Current.Session["ListAlokasi"] = ListAlokasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListAlokasi"] = null;
            return true;
        }

        public Alokasi Get1Record(Guid alokasiId)
        {
            var model = ListAlokasi.FirstOrDefault(o => o.AlokasiId == alokasiId);

            return model;
        }

        public List<Alokasi> GetnRecord(Guid alokasiId)
        {
            var model = ListAlokasi.Where(o => o.AlokasiId == alokasiId).ToList();
            return model;
        }

        public List<Alokasi> GetAllRecord()
        {
            var model = ListAlokasi;
            return model;
        }
    }

    public class AlokasiAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var alokasi = (Alokasi)value;
                var model = CRUDAlokasi.CRUD.Get1Record(alokasi.AlokasiId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Alokasi Tidak Valid");
            }
            else
                return new ValidationResult("Alokasi Harus Diisi");
        }
    }

    public class AlokasiIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var alokasiId = (Guid)value;
                var model = CRUDAlokasi.CRUD.Get1Record(alokasiId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Alokasi Tidak Valid");
            }
            else
                return new ValidationResult("Alokasi Harus Diisi");
        }
    }
}