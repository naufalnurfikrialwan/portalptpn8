using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDUrusan
    {
        public static CRUDUrusan CRUD = new CRUDUrusan();

        public List<Urusan> ListUrusan
        {
            get
            {
                List<Urusan> result = (List<Urusan>)HttpContext.Current.Session["ListUrusan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListUrusan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Urusan urusan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Urusan.Add(urusan);
                db.SaveChanges();

                ListUrusan.Add(urusan);
                HttpContext.Current.Session["ListUrusan"] = ListUrusan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Urusan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Urusan.ToList();
                foreach(var m in model)
                {
                    m.Bagian = CRUDBagian.CRUD.Get1Record(m.BagianId);
                }
                return model;
            }
            catch
            {
                return new List<Urusan>();
            }
        }
        public bool Update(Urusan urusan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Urusan.FirstOrDefault(o => o.UrusanId == urusan.UrusanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(urusan);
                }
                else
                {
                    model.InjectFrom(urusan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListUrusan.FirstOrDefault(o => o.UrusanId == urusan.UrusanId);
                    context.InjectFrom(urusan);
                    HttpContext.Current.Session["ListUrusan"] = ListUrusan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Urusan urusan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Urusan.FirstOrDefault(o => o.UrusanId == urusan.UrusanId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(urusan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Urusan urusan)
        {
            try
            {
                var context = ListUrusan.FirstOrDefault(o => o.UrusanId == urusan.UrusanId);
                ListUrusan.Remove(context);
                HttpContext.Current.Session["ListUrusan"] = ListUrusan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid bagianId)
        {
            try
            {
                var context = ListUrusan.Where(o => o.BagianId == bagianId).ToList();
                foreach (var m in context)
                {
                    ListUrusan.Remove(m);
                }
                HttpContext.Current.Session["ListUrusan"] = ListUrusan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Urusan Get1Record(Guid urusanId)
        {
            var model = ListUrusan.FirstOrDefault(o => o.UrusanId == urusanId);
            return model;
        }

        public List<Urusan> GetnRecord(Guid urusanId)
        {
            var model = ListUrusan.Where(o => o.UrusanId == urusanId).ToList();
            return model;
        }

        public List<Urusan> GetAllRecord()
        {
            var model = ListUrusan;
            return model;
        }
    }
}