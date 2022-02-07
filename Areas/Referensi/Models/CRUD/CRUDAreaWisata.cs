using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDAreaWisata
    {
        public static CRUDAreaWisata CRUD = new CRUDAreaWisata();

        public List<AreaWisata> ListAreaWisata
        {
            get
            {
                List<AreaWisata> result = (List<AreaWisata>)HttpContext.Current.Session["ListAreaWisata"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAreaWisata"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(AreaWisata AreaWisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.AreaWisata.Add(AreaWisata);
                db.SaveChanges();

                ListAreaWisata.Add(AreaWisata);
                HttpContext.Current.Session["ListAreaWisata"] = ListAreaWisata;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<AreaWisata> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AreaWisata.ToList(); //.Include("BankOrganisasiBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<AreaWisata>();
            }
        }
        public bool Update(AreaWisata AreaWisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AreaWisata.FirstOrDefault(o => o.AreaWisataId == AreaWisata.AreaWisataId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(AreaWisata);
                }
                else
                {
                    model.InjectFrom(AreaWisata);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListAreaWisata.FirstOrDefault(o => o.AreaWisataId == AreaWisata.AreaWisataId);
                    context.InjectFrom(AreaWisata);
                    HttpContext.Current.Session["ListAreaWisata"] = ListAreaWisata;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(AreaWisata AreaWisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.AreaWisata.FirstOrDefault(o => o.AreaWisataId == AreaWisata.AreaWisataId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(AreaWisata);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(AreaWisata AreaWisata)
        {
            try
            {
                var context = ListAreaWisata.FirstOrDefault(o => o.AreaWisataId == AreaWisata.AreaWisataId);
                ListAreaWisata.Remove(context);
                HttpContext.Current.Session["ListAreaWisata"] = ListAreaWisata;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListAreaWisata"] = null;
            return true;
        }

        public AreaWisata Get1Record(Guid AreaWisataId)
        {
            var model = ListAreaWisata.FirstOrDefault(o => o.AreaWisataId == AreaWisataId);

            return model;
        }

        public List<AreaWisata> GetnRecord(Guid AreaWisataId)
        {
            var model = ListAreaWisata.Where(o => o.AreaWisataId == AreaWisataId).ToList();
            return model;
        }

        public List<AreaWisata> GetAllRecord()
        {
            var model = ListAreaWisata;
            return model;
        }
    }

    public class AreaWisataAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var AreaWisata = (AreaWisata)value;
                var model = CRUDAreaWisata.CRUD.Get1Record(AreaWisata.AreaWisataId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("AreaWisata Tidak Valid");
            }
            else
                return new ValidationResult("AreaWisata Harus Diisi");
        }
    }

    public class AreaWisataIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var AreaWisataId = (Guid)value;
                var model = CRUDAreaWisata.CRUD.Get1Record(AreaWisataId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("AreaWisata Tidak Valid");
            }
            else
                return new ValidationResult("AreaWisata Harus Diisi");
        }
    }
}