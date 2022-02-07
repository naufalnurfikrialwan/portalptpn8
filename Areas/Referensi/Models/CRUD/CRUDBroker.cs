using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBroker
    {
        public static CRUDBroker CRUD = new CRUDBroker();

        public List<Broker> ListBroker
        {
            get
            {
                List<Broker> result = (List<Broker>)HttpContext.Current.Session["ListBroker"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBroker"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Broker buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Broker.Add(buyer);
                db.SaveChanges();

                ListBroker.Add(buyer);
                HttpContext.Current.Session["ListBroker"] = ListBroker;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Broker> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Broker.ToList(); //.Include("BrokerBudidaya").ToList();
                foreach (var m in model)
                {
                    m.FileFoto = m.BrokerId.ToString() + ".jpg";
                }

                return model;
            }
            catch
            {
                return new List<Broker>();
            }
        }
        public bool Update(Broker buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Broker.FirstOrDefault(o => o.BrokerId == buyer.BrokerId);
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

                    var context = ListBroker.FirstOrDefault(o => o.BrokerId == buyer.BrokerId);
                    context.InjectFrom(buyer);
                    HttpContext.Current.Session["ListBroker"] = ListBroker;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Broker buyer)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Broker.FirstOrDefault(o => o.BrokerId == buyer.BrokerId);
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

        public bool Erase(Broker buyer)
        {
            try
            {
                var context = ListBroker.FirstOrDefault(o => o.BrokerId == buyer.BrokerId);
                ListBroker.Remove(context);
                HttpContext.Current.Session["ListBroker"] = ListBroker;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListBroker"] = null;
            return true;
        }

        public Broker Get1Record(Guid buyerId)
        {
            var model = ListBroker.FirstOrDefault(o => o.BrokerId == buyerId);
            return model;
        }

        public List<Broker> GetnRecord(Guid buyerId)
        {
            var model = ListBroker.Where(o=>o.BrokerId== buyerId).ToList();
            return model;
        }

        public List<Broker> GetAllRecord()
        {
            var model = ListBroker;
            return model;
        }
    }

    public class BrokerAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var buyer = (Broker)value;
                var model = CRUDBroker.CRUD.Get1Record(buyer.BrokerId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Broker Tidak Valid");
            }
            else
                return new ValidationResult("Broker Harus Diisi");
        }
    }

    public class BrokerIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var brokerId = (Guid)value;
                var model = CRUDBroker.CRUD.Get1Record(brokerId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Broker Tidak Valid");
            }
            else
                return new ValidationResult("Broker Harus Diisi");
        }
    }
}