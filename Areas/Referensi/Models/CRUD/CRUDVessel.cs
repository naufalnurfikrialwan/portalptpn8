using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDVessel
    {
        public static CRUDVessel CRUD = new CRUDVessel();

        public List<Vessel> ListVessel
        {
            get
            {
                List<Vessel> result = (List<Vessel>)HttpContext.Current.Session["ListVessel"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListVessel"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Vessel vessel)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Vessel.Add(vessel);
                db.SaveChanges();

                ListVessel.Add(vessel);
                HttpContext.Current.Session["ListVessel"] = ListVessel;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Vessel> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Vessel.ToList(); //.Include("VesselBudidaya").ToList();
                return model;
            }
            catch
            {
                return new List<Vessel>();
            }
        }
        public bool Update(Vessel vessel)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Vessel.FirstOrDefault(o => o.VesselId == vessel.VesselId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(vessel);
                }
                else
                {
                    model.InjectFrom(vessel);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListVessel.FirstOrDefault(o => o.VesselId == vessel.VesselId);
                    context.InjectFrom(vessel);
                    HttpContext.Current.Session["ListVessel"] = ListVessel;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Vessel vessel)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Vessel.FirstOrDefault(o => o.VesselId == vessel.VesselId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(vessel);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Vessel vessel)
        {
            try
            {
                var context = ListVessel.FirstOrDefault(o => o.VesselId == vessel.VesselId);
                ListVessel.Remove(context);
                HttpContext.Current.Session["ListVessel"] = ListVessel;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListVessel"] = null;
            return true;
        }

        public Vessel Get1Record(Guid vesselId)
        {
            var model = ListVessel.FirstOrDefault(o => o.VesselId == vesselId);
            return model;
        }

        public List<Vessel> GetnRecord(Guid vesselId)
        {
            var model = ListVessel.Where(o => o.VesselId == vesselId).ToList();
            return model;
        }

        public List<Vessel> GetAllRecord()
        {
            var model = ListVessel;
            return model;
        }
    }

    public class VesselAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var vessel = (Vessel)value;
                var model = CRUDVessel.CRUD.Get1Record(vessel.VesselId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Vessel Tidak Valid");
            }
            else
                return new ValidationResult("Vessel Harus Diisi");
        }
    }

    public class VesselIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var vesselId = (Guid)value;
                var model = CRUDVessel.CRUD.Get1Record(vesselId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Vessel Tidak Valid");
            }
            else
                return new ValidationResult("Vessel Harus Diisi");
        }
    }
}