using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBudidaya
    {
        public static CRUDBudidaya CRUD = new CRUDBudidaya();

        public List<MainBudidaya> ListBudidaya
        {
            get
            {
                List<MainBudidaya> result = (List<MainBudidaya>)HttpContext.Current.Session["ListBudidaya"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBudidaya"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(MainBudidaya budidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.MainBudidaya.Add(budidaya);
                db.SaveChanges();

                ListBudidaya.Add(budidaya);
                HttpContext.Current.Session["ListBudidaya"] = ListBudidaya;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<MainBudidaya> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.MainBudidaya.ToList();
                foreach(var m in model)
                {
                    m.FileFoto = m.MainBudidayaId.ToString() + ".jpg";
                }
                return model;
            }
            catch
            {
                return new List<MainBudidaya>();
            }
        }
        public bool Update(MainBudidaya budidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.MainBudidaya.FirstOrDefault(o => o.MainBudidayaId == budidaya.MainBudidayaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(budidaya);
                }
                else
                {
                    model.InjectFrom(budidaya);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBudidaya.FirstOrDefault(o => o.MainBudidayaId == budidaya.MainBudidayaId);
                    context.InjectFrom(budidaya);
                    HttpContext.Current.Session["ListBudidaya"] = ListBudidaya;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(MainBudidaya budidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.MainBudidaya.FirstOrDefault(o => o.MainBudidayaId == budidaya.MainBudidayaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(budidaya);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(MainBudidaya budidaya)
        {
            try
            {
                var context = ListBudidaya.FirstOrDefault(o => o.MainBudidayaId == budidaya.MainBudidayaId);
                ListBudidaya.Remove(context);
                HttpContext.Current.Session["ListBudidaya"] = ListBudidaya;
                CRUDSubBudidaya.CRUD.Erase(budidaya.MainBudidayaId);
                CRUDBuyerBudidaya.CRUD.Erase(budidaya.MainBudidayaId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public MainBudidaya Get1Record(Guid budidayaId)
        {
            var model = ListBudidaya.FirstOrDefault(o => o.MainBudidayaId == budidayaId);
            return model;
        }

        public MainBudidaya Get1Record(string kodeBudidaya)
        {
            var model = ListBudidaya.FirstOrDefault(o => o.kd_bud == kodeBudidaya) != null ? ListBudidaya.FirstOrDefault(o => o.kd_bud == kodeBudidaya) : ListBudidaya.FirstOrDefault(o => o.Nama.ToLower().TrimEnd() == kodeBudidaya.ToLower().TrimEnd());
            return model;
        }

        public List<MainBudidaya> GetnRecord(Guid budidayaId)
        {
            var model = ListBudidaya.Where(o => o.MainBudidayaId == budidayaId).ToList();
            return model;
        }

        public List<MainBudidaya> GetAllRecord()
        {
            var model = ListBudidaya;
            return model;
        }
    }
}