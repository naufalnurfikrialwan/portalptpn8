using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDJenisTanah
    {
        public static CRUDJenisTanah CRUD = new CRUDJenisTanah();

        public List<JenisTanah> ListJenisTanah
        {
            get
            {
                List<JenisTanah> result = (List<JenisTanah>)HttpContext.Current.Session["ListJenisTanah"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListJenisTanah"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(JenisTanah jenisTanah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.JenisTanah.Add(jenisTanah);
                db.SaveChanges();

                ListJenisTanah.Add(jenisTanah);
                HttpContext.Current.Session["ListJenisTanah"] = ListJenisTanah;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<JenisTanah> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.JenisTanah.ToList();
                return model;
            }
            catch
            {
                return new List<JenisTanah>();
            }
        }
        public bool Update(JenisTanah jenisTanah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.JenisTanah.FirstOrDefault(o => o.JenisTanahId == jenisTanah.JenisTanahId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(jenisTanah);
                }
                else
                {
                    model.InjectFrom(jenisTanah);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListJenisTanah.FirstOrDefault(o => o.JenisTanahId == jenisTanah.JenisTanahId);
                    context.InjectFrom(jenisTanah);
                    HttpContext.Current.Session["ListJenisTanah"] = ListJenisTanah;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(JenisTanah jenisTanah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.JenisTanah.FirstOrDefault(o => o.JenisTanahId == jenisTanah.JenisTanahId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(jenisTanah);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(JenisTanah jenisTanah)
        {
            try
            {
                var context = ListJenisTanah.FirstOrDefault(o => o.JenisTanahId == jenisTanah.JenisTanahId);
                ListJenisTanah.Remove(context);
                HttpContext.Current.Session["ListJenisTanah"] = ListJenisTanah;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public JenisTanah Get1Record(Guid jenisTanahId)
        {
            var model = ListJenisTanah.FirstOrDefault(o => o.JenisTanahId == jenisTanahId);
            return model;
        }

        public List<JenisTanah> GetnRecord(Guid jenisTanahId)
        {
            var model = ListJenisTanah.Where(o => o.JenisTanahId == jenisTanahId).ToList();
            return model;
        }

        public List<JenisTanah> GetAllRecord()
        {
            var model = ListJenisTanah;
            return model;
        }
    }
}