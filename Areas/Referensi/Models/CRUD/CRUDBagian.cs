using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBagian
    {
        public static CRUDBagian CRUD = new CRUDBagian();

        public List<Bagian> ListBagian
        {
            get
            {
                List<Bagian> result = (List<Bagian>)HttpContext.Current.Session["ListBagian"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBagian"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Bagian bagian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Bagian.Add(bagian);
                db.SaveChanges();

                ListBagian.Add(bagian);
                HttpContext.Current.Session["ListBagian"] = ListBagian;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Bagian> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bagian.ToList();
                foreach(var m in model)
                {
                    m.Direksi = CRUDDireksi.CRUD.Get1Record(m.DireksiId);
                }
                return model;
            }
            catch
            {
                return new List<Bagian>();
            }
        }
        public bool Update(Bagian bagian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bagian.FirstOrDefault(o => o.BagianId == bagian.BagianId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(bagian);
                }
                else
                {
                    model.InjectFrom(bagian);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBagian.FirstOrDefault(o => o.BagianId == bagian.BagianId);
                    context.InjectFrom(bagian);
                    HttpContext.Current.Session["ListBagian"] = ListBagian;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Bagian bagian)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bagian.FirstOrDefault(o => o.BagianId == bagian.BagianId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(bagian);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Bagian bagian)
        {
            try
            {
                var context = ListBagian.FirstOrDefault(o => o.BagianId == bagian.BagianId);
                ListBagian.Remove(context);
                HttpContext.Current.Session["ListBagian"] = ListBagian;
                CRUDUrusan.CRUD.Erase(bagian.BagianId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid direksiId)
        {
            try
            {
                var context = ListBagian.Where(o => o.DireksiId == direksiId).ToList();
                foreach (var m in context)
                {
                    ListBagian.Remove(m);
                    CRUDUrusan.CRUD.Erase(m.BagianId);
                }
                HttpContext.Current.Session["ListBagian"] = ListBagian;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Bagian Get1Record(Guid bagianId)
        {
            var model = ListBagian.FirstOrDefault(o => o.BagianId == bagianId);
            return model;
        }

        public Bagian Get1Record(string kodebagian)
        {
            var model = ListBagian.FirstOrDefault(o => o.kd_kbn.Substring(0,kodebagian.Length) == kodebagian);
            return model;
        }

        public List<Bagian> GetnRecord(Guid bagianId)
        {
            var model = ListBagian.Where(o => o.BagianId == bagianId).ToList();
            return model;
        }

        public List<Bagian> GetAllRecord()
        {
            var model = ListBagian;
            return model;
        }
    }
}