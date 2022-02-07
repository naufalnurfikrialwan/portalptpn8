using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDCustomer_Agrowisata
    {
        public static CRUDCustomer_Agrowisata CRUD = new CRUDCustomer_Agrowisata();

        public List<Customer_Agrowisata> ListCustomer_Agrowisata
        {
            get
            {
                List<Customer_Agrowisata> result = (List<Customer_Agrowisata>)HttpContext.Current.Session["ListCustomer_Agrowisata"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListCustomer_Agrowisata"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Customer_Agrowisata Customer_Agrowisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Customer_Agrowisata.Add(Customer_Agrowisata);
                db.SaveChanges();

                ListCustomer_Agrowisata.Add(Customer_Agrowisata);
                HttpContext.Current.Session["ListCustomer_Agrowisata"] = ListCustomer_Agrowisata;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Customer_Agrowisata> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Customer_Agrowisata.ToList(); //.Include("BankOrganisasiBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<Customer_Agrowisata>();
            }
        }
        public bool Update(Customer_Agrowisata Customer_Agrowisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Customer_Agrowisata.FirstOrDefault(o => o.Customer_AgrowisataId == Customer_Agrowisata.Customer_AgrowisataId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(Customer_Agrowisata);
                }
                else
                {
                    model.InjectFrom(Customer_Agrowisata);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListCustomer_Agrowisata.FirstOrDefault(o => o.Customer_AgrowisataId == Customer_Agrowisata.Customer_AgrowisataId);
                    context.InjectFrom(Customer_Agrowisata);
                    HttpContext.Current.Session["ListTipeKamar"] = ListCustomer_Agrowisata;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Customer_Agrowisata Customer_Agrowisata)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Customer_Agrowisata.FirstOrDefault(o => o.Customer_AgrowisataId == Customer_Agrowisata.Customer_AgrowisataId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(Customer_Agrowisata);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Customer_Agrowisata Customer_Agrowisata)
        {
            try
            {
                var context = ListCustomer_Agrowisata.FirstOrDefault(o => o.Customer_AgrowisataId == Customer_Agrowisata.Customer_AgrowisataId);
                ListCustomer_Agrowisata.Remove(context);
                HttpContext.Current.Session["ListCustomer_Agrowisata"] = ListCustomer_Agrowisata;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListCustomer_Agrowisata"] = null;
            return true;
        }

        public Customer_Agrowisata Get1Record(Guid Customer_AgrowisataId)
        {
            var model = ListCustomer_Agrowisata.FirstOrDefault(o => o.Customer_AgrowisataId == Customer_AgrowisataId);

            return model;
        }

        public List<Customer_Agrowisata> GetnRecord(Guid Customer_AgrowisataId)
        {
            var model = ListCustomer_Agrowisata.Where(o => o.Customer_AgrowisataId == Customer_AgrowisataId).ToList();
            return model;
        }

        public List<Customer_Agrowisata> GetAllRecord()
        {
            var model = ListCustomer_Agrowisata;
            return model;
        }
    }

    public class Customer_AgrowisataAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var Customer_Agrowisata = (Customer_Agrowisata)value;
                var model = CRUDCustomer_Agrowisata.CRUD.Get1Record(Customer_Agrowisata.Customer_AgrowisataId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Customer_Agrowisata Tidak Valid");
            }
            else
                return new ValidationResult("Customer_Agrowisata Harus Diisi");
        }
    }

    public class Customer_AgrowisataIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var Customer_AgrowisataId = (Guid)value;
                var model = CRUDCustomer_Agrowisata.CRUD.Get1Record(Customer_AgrowisataId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Customer_Agrowisata Tidak Valid");
            }
            else
                return new ValidationResult("Customer_Agrowisata Harus Diisi");
        }
    }
}