using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBuyer
    {
        public static CRUDBuyer CRUD = new CRUDBuyer();

        public List<Buyer> ListBuyer
        {
            get
            {
                List<Buyer> result = (List<Buyer>)HttpContext.Current.Session["ListBuyer"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBuyer"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Buyer buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Buyers.Add(buyer);
                db.SaveChanges();

                ListBuyer.Add(buyer);
                HttpContext.Current.Session["ListBuyer"] = ListBuyer;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Buyer> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Buyers.ToList(); //.Include("BuyerBudidaya").ToList();
                foreach (var m in model)
                {
                    m.FileFoto = m.BuyerId.ToString() + ".jpg";
                }

                return model;
            }
            catch
            {
                return new List<Buyer>();
            }
        }
        public bool Update(Buyer buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Buyers.FirstOrDefault(o => o.BuyerId == buyer.BuyerId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(buyer);
                }
                else
                {
                    model.InjectFrom(buyer);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBuyer.FirstOrDefault(o => o.BuyerId == buyer.BuyerId);
                    context.InjectFrom(buyer);
                    HttpContext.Current.Session["ListBuyer"] = ListBuyer;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Buyer buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Buyers.FirstOrDefault(o => o.BuyerId == buyer.BuyerId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(buyer);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Buyer buyer)
        {
            try
            {
                var context = ListBuyer.FirstOrDefault(o => o.BuyerId == buyer.BuyerId);
                ListBuyer.Remove(context);
                HttpContext.Current.Session["ListBuyer"] = ListBuyer;
                CRUDBuyerBudidaya.CRUD.ErasebyBuyer(buyer.BuyerId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListBuyer"] = null;
            return true;
        }

        public Buyer Get1Record(Guid buyerId)
        {
            var model = ListBuyer.FirstOrDefault(o => o.BuyerId == buyerId);

            return model;
        }

        public List<Buyer> GetnRecord(Guid buyerId)
        {
            var model = ListBuyer.Where(o=>o.BuyerId== buyerId).ToList();
            return model;
        }

        public List<Buyer> GetAllRecord()
        {
            var model = ListBuyer;
            return model;
        }
    }

    public class BuyerAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var buyer = (Buyer)value;
                var model = CRUDBuyer.CRUD.Get1Record(buyer.BuyerId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Buyer Tidak Valid");
            }
            else
                return new ValidationResult("Buyer Harus Diisi");
        }
    }

    public class BuyerIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var buyerId = (Guid)value;
                var model = CRUDBuyer.CRUD.Get1Record(buyerId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Buyer Tidak Valid");
            }
            else
                return new ValidationResult("Buyer Harus Diisi");
        }
    }
}