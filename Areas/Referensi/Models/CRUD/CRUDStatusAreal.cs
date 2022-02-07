using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDStatusAreal
    {
        public static CRUDStatusAreal CRUD = new CRUDStatusAreal();

        public List<StatusAreal> ListStatusAreal
        {
            get
            {
                List<StatusAreal> result = (List<StatusAreal>)HttpContext.Current.Session["ListStatusAreal"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListStatusAreal"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(StatusAreal statusAreal)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.StatusAreal.Add(statusAreal);
                db.SaveChanges();

                ListStatusAreal.Add(statusAreal);
                HttpContext.Current.Session["ListStatusAreal"] = ListStatusAreal;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<StatusAreal> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.StatusAreal.ToList();
                return model;
            }
            catch
            {
                return new List<StatusAreal>();
            }
        }
        public bool Update(StatusAreal statusAreal)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.StatusAreal.FirstOrDefault(o => o.StatusArealId == statusAreal.StatusArealId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(statusAreal);
                }
                else
                {
                    model.InjectFrom(statusAreal);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListStatusAreal.FirstOrDefault(o => o.StatusArealId == statusAreal.StatusArealId);
                    context.InjectFrom(statusAreal);
                    HttpContext.Current.Session["ListStatusAreal"] = ListStatusAreal;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(StatusAreal statusAreal)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.StatusAreal.FirstOrDefault(o => o.StatusArealId == statusAreal.StatusArealId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(statusAreal);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(StatusAreal statusAreal)
        {
            try
            {
                var context = ListStatusAreal.FirstOrDefault(o => o.StatusArealId == statusAreal.StatusArealId);
                ListStatusAreal.Remove(context);
                HttpContext.Current.Session["ListStatusAreal"] = ListStatusAreal;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public StatusAreal Get1Record(Guid statusArealId)
        {
            var model = ListStatusAreal.FirstOrDefault(o => o.StatusArealId == statusArealId);
            return model;
        }

        public StatusAreal Get1Record(string nama)
        {
            var model = ListStatusAreal.FirstOrDefault(o => o.Nama.ToLower().TrimEnd() == nama.ToLower().TrimEnd());
            return model;
        }

        public List<StatusAreal> GetnRecord(Guid statusArealId)
        {
            var model = ListStatusAreal.Where(o => o.StatusArealId == statusArealId).ToList();
            return model;
        }

        public List<StatusAreal> GetAllRecord()
        {
            var model = ListStatusAreal;
            return model;
        }
    }
}