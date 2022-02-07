using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBankOrganisasi
    {
        public static CRUDBankOrganisasi CRUD = new CRUDBankOrganisasi();

        public List<BankOrganisasi> ListBankOrganisasi
        {
            get
            {
                List<BankOrganisasi> result = (List<BankOrganisasi>)HttpContext.Current.Session["ListBankOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBankOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(BankOrganisasi BankOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BankOrganisasi.Add(BankOrganisasi);
                db.SaveChanges();

                ListBankOrganisasi.Add(BankOrganisasi);
                HttpContext.Current.Session["ListBankOrganisasi"] = ListBankOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BankOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BankOrganisasi.ToList(); //.Include("BankOrganisasiBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<BankOrganisasi>();
            }
        }
        public bool Update(BankOrganisasi BankOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BankOrganisasi.FirstOrDefault(o => o.BankOrganisasiId == BankOrganisasi.BankOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(BankOrganisasi);
                }
                else
                {
                    model.InjectFrom(BankOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBankOrganisasi.FirstOrDefault(o => o.BankOrganisasiId == BankOrganisasi.BankOrganisasiId);
                    context.InjectFrom(BankOrganisasi);
                    HttpContext.Current.Session["ListBankOrganisasi"] = ListBankOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BankOrganisasi BankOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BankOrganisasi.FirstOrDefault(o => o.BankOrganisasiId == BankOrganisasi.BankOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(BankOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BankOrganisasi BankOrganisasi)
        {
            try
            {
                var context = ListBankOrganisasi.FirstOrDefault(o => o.BankOrganisasiId == BankOrganisasi.BankOrganisasiId);
                ListBankOrganisasi.Remove(context);
                HttpContext.Current.Session["ListBankOrganisasi"] = ListBankOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListBankOrganisasi"] = null;
            return true;
        }

        public BankOrganisasi Get1Record(Guid BankOrganisasiId)
        {
            var model = ListBankOrganisasi.FirstOrDefault(o => o.BankOrganisasiId == BankOrganisasiId);

            return model;
        }

        public List<BankOrganisasi> GetnRecord(Guid BankOrganisasiId)
        {
            var model = ListBankOrganisasi.Where(o => o.BankOrganisasiId == BankOrganisasiId).ToList();
            return model;
        }

        public List<BankOrganisasi> GetAllRecord()
        {
            var model = ListBankOrganisasi;
            return model;
        }
    }

    public class BankOrganisasiAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var BankOrganisasi = (BankOrganisasi)value;
                var model = CRUDBankOrganisasi.CRUD.Get1Record(BankOrganisasi.BankOrganisasiId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("BankOrganisasi Tidak Valid");
            }
            else
                return new ValidationResult("BankOrganisasi Harus Diisi");
        }
    }

    public class BankOrganisasiIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var BankOrganisasiId = (Guid)value;
                var model = CRUDBankOrganisasi.CRUD.Get1Record(BankOrganisasiId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("BankOrganisasi Tidak Valid");
            }
            else
                return new ValidationResult("BankOrganisasi Harus Diisi");
        }
    }
}