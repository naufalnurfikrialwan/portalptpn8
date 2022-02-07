using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKemasan
    {
        public static CRUDKemasan CRUD = new CRUDKemasan();

        public List<Kemasan> ListKemasan
        {
            get
            {
                List<Kemasan> result = (List<Kemasan>)HttpContext.Current.Session["ListKemasan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKemasan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kemasan kemasan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kemasan.Add(kemasan);
                db.SaveChanges();

                ListKemasan.Add(kemasan);
                HttpContext.Current.Session["ListKemasan"] = ListKemasan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kemasan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kemasan.ToList();
                return model;
            }
            catch
            {
                return new List<Kemasan>();
            }
        }
        public bool Update(Kemasan kemasan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kemasan.FirstOrDefault(o => o.KemasanId == kemasan.KemasanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kemasan);
                }
                else
                {
                    model.InjectFrom(kemasan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKemasan.FirstOrDefault(o => o.KemasanId == kemasan.KemasanId);
                    context.InjectFrom(kemasan);
                    HttpContext.Current.Session["ListKemasan"] = ListKemasan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kemasan kemasan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kemasan.FirstOrDefault(o => o.KemasanId == kemasan.KemasanId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kemasan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kemasan kemasan)
        {
            try
            {
                var context = ListKemasan.FirstOrDefault(o => o.KemasanId == kemasan.KemasanId);
                ListKemasan.Remove(context);
                HttpContext.Current.Session["ListKemasan"] = ListKemasan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Kemasan Get1Record(Guid kemasanId)
        {
            var model = ListKemasan.FirstOrDefault(o => o.KemasanId == kemasanId);
            return model;
        }

        public List<Kemasan> GetnRecord(Guid kemasanId)
        {
            var model = ListKemasan.Where(o => o.KemasanId == kemasanId).ToList();
            return model;
        }

        public List<Kemasan> GetAllRecord()
        {
            var model = ListKemasan;
            return model;
        }
    }
}