using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDTehRelasi_DaftarProduk
    {
        public static CRUDTehRelasi_DaftarProduk CRUD = new CRUDTehRelasi_DaftarProduk();

        public List<TehRelasi_DaftarProduk> ListTehRelasi_DaftarProduk
        {
            get
            {
                List<TehRelasi_DaftarProduk> result = (List<TehRelasi_DaftarProduk>)HttpContext.Current.Session["ListTehRelasi_DaftarProduk"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListTehRelasi_DaftarProduk"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(TehRelasi_DaftarProduk tehRelasi_DaftarProduk)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.TehRelasi_DaftarProduk.Add(tehRelasi_DaftarProduk);
                db.SaveChanges();

                ListTehRelasi_DaftarProduk.Add(tehRelasi_DaftarProduk);
                HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarProduk;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<TehRelasi_DaftarProduk> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarProduk.ToList(); //.Include("VesselBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<TehRelasi_DaftarProduk>();
            }
        }
        public bool Update(TehRelasi_DaftarProduk tehRelasi_DaftarProduk)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarProduk.FirstOrDefault(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProduk.TehRelasi_DaftarProdukId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(tehRelasi_DaftarProduk);
                }
                else
                {
                    model.InjectFrom(tehRelasi_DaftarProduk);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListTehRelasi_DaftarProduk.FirstOrDefault(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProduk.TehRelasi_DaftarProdukId);
                    context.InjectFrom(tehRelasi_DaftarProduk);
                    HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarProduk;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(TehRelasi_DaftarProduk tehRelasi_DaftarProduk)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TehRelasi_DaftarProduk.FirstOrDefault(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProduk.TehRelasi_DaftarProdukId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(tehRelasi_DaftarProduk);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(TehRelasi_DaftarProduk tehRelasi_DaftarProduk)
        {
            try
            {
                var context = ListTehRelasi_DaftarProduk.FirstOrDefault(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProduk.TehRelasi_DaftarProdukId);
                ListTehRelasi_DaftarProduk.Remove(context);
                HttpContext.Current.Session["ListVessel"] = ListTehRelasi_DaftarProduk;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListTehRelasi_DaftarProduk"] = null;
            return true;
        }

        public TehRelasi_DaftarProduk Get1Record(Guid tehRelasi_DaftarProdukId)
        {
            var model = ListTehRelasi_DaftarProduk.FirstOrDefault(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProdukId);
            return model;
        }

        public List<TehRelasi_DaftarProduk> GetnRecord(Guid tehRelasi_DaftarProdukId)
        {
            var model = ListTehRelasi_DaftarProduk.Where(o => o.TehRelasi_DaftarProdukId == tehRelasi_DaftarProdukId).ToList();
            return model;
        }

        public List<TehRelasi_DaftarProduk> GetAllRecord()
        {
            var model = ListTehRelasi_DaftarProduk;
            return model;
        }
    }

    public class TehRelasi_DaftarProdukAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var tehRelasi_DaftarProduk = (TehRelasi_DaftarProduk)value;
                var model = CRUDTehRelasi_DaftarProduk.CRUD.Get1Record(tehRelasi_DaftarProduk.TehRelasi_DaftarProdukId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Tidak Valid");
            }
            else
                return new ValidationResult("Harus Diisi");
        }
    }

    public class TehRelasi_DaftarProdukIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var tehRelasi_DaftarProdukId = (Guid)value;
                var model = CRUDTehRelasi_DaftarProduk.CRUD.Get1Record(tehRelasi_DaftarProdukId);
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