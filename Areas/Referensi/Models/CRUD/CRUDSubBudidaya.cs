using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDSubBudidaya
    {
        public static CRUDSubBudidaya CRUD = new CRUDSubBudidaya();

        public List<SubBudidaya> ListSubBudidaya
        {
            get
            {
                List<SubBudidaya> result = (List<SubBudidaya>)HttpContext.Current.Session["ListSubBudidaya"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListSubBudidaya"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(SubBudidaya subBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.SubBudidaya.Add(subBudidaya);
                db.SaveChanges();

                ListSubBudidaya.Add(subBudidaya);
                HttpContext.Current.Session["ListSubBudidaya"] = ListSubBudidaya;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<SubBudidaya> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.SubBudidaya.ToList();
                foreach (var m in model)
                {
                    m.FileFoto = m.SubBudidayaId.ToString() + ".jpg";
                    m.Budidaya = CRUDBudidaya.CRUD.Get1Record(m.MainBudidayaId);
                    m.Foto = new byte[0];
                    //m.Grade = new List<Grade>();
                }
                return model;
            }
            catch
            {
                return new List<SubBudidaya>();
            }
        }
        public bool Update(SubBudidaya subBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.SubBudidaya.FirstOrDefault(o => o.SubBudidayaId == subBudidaya.SubBudidayaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(subBudidaya);
                }
                else
                {
                    model.InjectFrom(subBudidaya);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListSubBudidaya.FirstOrDefault(o => o.SubBudidayaId == subBudidaya.SubBudidayaId);
                    context.InjectFrom(subBudidaya);
                    HttpContext.Current.Session["ListSubBudidaya"] = ListSubBudidaya;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(SubBudidaya subBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.SubBudidaya.FirstOrDefault(o => o.SubBudidayaId == subBudidaya.SubBudidayaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(subBudidaya);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(SubBudidaya subBudidaya)
        {
            try
            {
                var context = ListSubBudidaya.FirstOrDefault(o => o.SubBudidayaId == subBudidaya.SubBudidayaId);
                ListSubBudidaya.Remove(context);
                HttpContext.Current.Session["ListSubBudidaya"] = ListSubBudidaya;
                CRUDGrade.CRUD.Erase(subBudidaya.SubBudidayaId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid mainBudidayaId)
        {
            try
            {
                var context = ListSubBudidaya.Where(o => o.MainBudidayaId == mainBudidayaId).ToList();
                foreach(var m in context)
                {
                    ListSubBudidaya.Remove(m);
                    CRUDGrade.CRUD.Erase(m.SubBudidayaId);
                }
                HttpContext.Current.Session["ListSubBudidaya"] = ListSubBudidaya;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public SubBudidaya Get1Record(Guid subBudidayaId)
        {
            var model = ListSubBudidaya.FirstOrDefault(o => o.SubBudidayaId == subBudidayaId);
            return model;
        }

        public SubBudidaya Get1Record(string subBudidayaId)
        {
            var model = ListSubBudidaya.FirstOrDefault(o => o.SubBudidayaId == Guid.Parse(subBudidayaId));
            return model;
        }

        public List<SubBudidaya> GetnRecord(Guid subBudidayaId)
        {
            var model = ListSubBudidaya.Where(o => o.SubBudidayaId == subBudidayaId).ToList();
            return model;
        }

        public List<SubBudidaya> GetAllRecord()
        {
            var model = ListSubBudidaya;
            return model;
        }
    }

    public class SubBudidayaIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var subBudidayaId = (Guid)value;
                var model = CRUDSubBudidaya.CRUD.Get1Record(subBudidayaId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Jenis Tidak Valid");
            }
            else
                return new ValidationResult("Jenis Harus Diisi");
        }
    }

}