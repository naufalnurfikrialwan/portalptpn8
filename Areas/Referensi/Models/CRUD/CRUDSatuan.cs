using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDSatuan
    {
        public static CRUDSatuan CRUD = new CRUDSatuan();

        public List<Satuan> ListSatuan
        {
            get
            {
                List<Satuan> result = (List<Satuan>)HttpContext.Current.Session["ListSatuan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListSatuan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Satuan satuan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Satuan.Add(satuan);
                db.SaveChanges();

                ListSatuan.Add(satuan);
                HttpContext.Current.Session["ListSatuan"] = ListSatuan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Satuan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Satuan.ToList();
                return model;
            }
            catch
            {
                return new List<Satuan>();
            }
        }
        public bool Update(Satuan satuan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Satuan.FirstOrDefault(o => o.SatuanId == satuan.SatuanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(satuan);
                }
                else
                {
                    model.InjectFrom(satuan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListSatuan.FirstOrDefault(o => o.SatuanId == satuan.SatuanId);
                    context.InjectFrom(satuan);
                    HttpContext.Current.Session["ListSatuan"] = ListSatuan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Satuan satuan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Satuan.FirstOrDefault(o => o.SatuanId == satuan.SatuanId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(satuan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Satuan satuan)
        {
            try
            {
                var context = ListSatuan.FirstOrDefault(o => o.SatuanId == satuan.SatuanId);
                ListSatuan.Remove(context);
                HttpContext.Current.Session["ListSatuan"] = ListSatuan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Satuan Get1Record(Guid satuanId)
        {
            var model = ListSatuan.FirstOrDefault(o => o.SatuanId == satuanId);
            return model;
        }

        public Satuan Get1Record(string kd_sat)
        {
            var model = ListSatuan.FirstOrDefault(o => o.kd_sat == kd_sat);
            return model;
        }

        public List<Satuan> GetnRecord(Guid satuanId)
        {
            var model = ListSatuan.Where(o => o.SatuanId == satuanId).ToList();
            return model;
        }

        public List<Satuan> GetAllRecord()
        {
            var model = ListSatuan;
            return model;
        }
    }

    public class SatuanIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var satuanId = (Guid)value;
                var model = CRUDSatuan.CRUD.Get1Record(satuanId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Satuan Tidak Valid");
            }
            else
                return new ValidationResult("Satuan Harus Diisi");
        }
    }
}