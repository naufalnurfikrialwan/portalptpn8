using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDUnit
    {
        public static CRUDUnit CRUD = new CRUDUnit();

        public List<Unit> ListUnit
        {
            get
            {
                List<Unit> result = (List<Unit>)HttpContext.Current.Session["ListUnit"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListUnit"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Unit unit)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Unit.Add(unit);
                db.SaveChanges();

                ListUnit.Add(unit);
                HttpContext.Current.Session["ListUnit"] = ListUnit;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Unit> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Unit.ToList();
                return model;
            }
            catch
            {
                return new List<Unit>();
            }
        }
        public bool Update(Unit unit)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Unit.FirstOrDefault(o => o.UnitId == unit.UnitId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(unit);
                }
                else
                {
                    model.InjectFrom(unit);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListUnit.FirstOrDefault(o => o.UnitId == unit.UnitId);
                    context.InjectFrom(unit);
                    HttpContext.Current.Session["ListUnit"] = ListUnit;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Unit unit)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Unit.FirstOrDefault(o => o.UnitId == unit.UnitId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(unit);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Unit unit)
        {
            try
            {
                var context = ListUnit.FirstOrDefault(o => o.UnitId == unit.UnitId);
                ListUnit.Remove(context);
                HttpContext.Current.Session["ListUnit"] = ListUnit;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Unit Get1Record(Guid unitId)
        {
            var model = ListUnit.FirstOrDefault(o => o.OrgId == unitId);
            return model;
        }

        public List<Unit> GetnRecord(Guid unitId)
        {
            var model = ListUnit.Where(o => o.UnitId == unitId).ToList();
            return model;
        }

        public List<Unit> GetAllRecord()
        {
            var model = ListUnit;
            return model;
        }
    }

    public class UnitIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var unitId = (Guid)value;
                var model = CRUDUnit.CRUD.Get1Record(unitId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Unit Tidak Valid");
            }
            else
                return new ValidationResult("Unit Harus Diisi");
        }
    }
}