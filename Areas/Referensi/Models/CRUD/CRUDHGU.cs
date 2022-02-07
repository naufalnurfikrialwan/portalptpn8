using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDHGU
    {
        public static CRUDHGU CRUD = new CRUDHGU();

        public List<HGU> ListHGU
        {
            get
            {
                List<HGU> result = (List<HGU>)HttpContext.Current.Session["ListHGU"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListHGU"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(HGU hgu)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.HGU.Add(hgu);
                db.SaveChanges();

                ListHGU.Add(hgu);
                HttpContext.Current.Session["ListHGU"] = ListHGU;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<HGU> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.HGU.ToList();
                foreach (var m in model)
                {
                    if (m.TanggalBerakhir.Year == 1900 || m.TanggalBerakhir < DateTime.Now)
                        m.SisaWaktu = 0;
                    else
                        m.SisaWaktu = (m.TanggalBerakhir - DateTime.Now).Duration().Days / 365;
                }
                return model;
            }
            catch
            {
                return new List<HGU>();
            }
        }
        public bool Update(HGU hgu)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.HGU.FirstOrDefault(o => o.HGUId == hgu.HGUId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(hgu);
                }
                else
                {
                    model.InjectFrom(hgu);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListHGU.FirstOrDefault(o => o.HGUId == hgu.HGUId);
                    context.InjectFrom(hgu);
                    HttpContext.Current.Session["ListHGU"] = ListHGU;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(HGU hgu)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.HGU.FirstOrDefault(o => o.HGUId == hgu.HGUId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(hgu);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(HGU hgu)
        {
            try
            {
                var context = ListHGU.FirstOrDefault(o => o.HGUId == hgu.HGUId);
                ListHGU.Remove(context);
                HttpContext.Current.Session["ListHGU"] = ListHGU;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public HGU Get1Record(Guid hguId)
        {
            var model = ListHGU.FirstOrDefault(o => o.HGUId == hguId);
            return model;
        }

        public List<HGU> GetnRecord(Guid hguId)
        {
            var model = ListHGU.Where(o => o.HGUId == hguId).ToList();
            return model;
        }

        public List<HGU> GetAllRecord()
        {
            var model = ListHGU;
            return model;
        }
    }
}