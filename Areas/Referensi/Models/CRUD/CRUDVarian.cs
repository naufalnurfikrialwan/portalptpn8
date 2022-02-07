using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDVarian
    {
        public static CRUDVarian CRUD = new CRUDVarian();

        public List<Varian> ListVarian
        {
            get
            {
                List<Varian> result = (List<Varian>)HttpContext.Current.Session["ListVarian"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListVarian"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Varian varian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Varian.Add(varian);
                db.SaveChanges();

                ListVarian.Add(varian);
                HttpContext.Current.Session["ListVarian"] = ListVarian;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Varian> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Varian.ToList();
                return model;
            }
            catch
            {
                return new List<Varian>();
            }
        }
        public bool Update(Varian varian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Varian.FirstOrDefault(o => o.VarianId == varian.VarianId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(varian);
                }
                else
                {
                    model.InjectFrom(varian);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListVarian.FirstOrDefault(o => o.VarianId == varian.VarianId);
                    context.InjectFrom(varian);
                    HttpContext.Current.Session["ListVarian"] = ListVarian;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Varian varian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Varian.FirstOrDefault(o => o.VarianId == varian.VarianId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Varian varian)
        {
            try
            {
                var context = ListVarian.FirstOrDefault(o => o.VarianId == varian.VarianId);
                ListVarian.Remove(context);
                HttpContext.Current.Session["ListVarian"] = ListVarian;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid gradeId)
        {
            try
            {
                var context = ListVarian.Where(o => o.GradeId == gradeId).ToList();
                foreach (var m in context)
                {
                    ListVarian.Remove(m);
                }
                HttpContext.Current.Session["ListVarian"] = ListVarian;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Varian Get1Record(Guid varianId)
        {
            var model = ListVarian.FirstOrDefault(o => o.VarianId == varianId);
            return model;
        }

        public List<Varian> GetnRecord(Guid varianId)
        {
            var model = ListVarian.Where(o => o.VarianId == varianId).ToList();
            return model;
        }

        public List<Varian> GetAllRecord()
        {
            var model = ListVarian;
            return model;
        }
    }
}