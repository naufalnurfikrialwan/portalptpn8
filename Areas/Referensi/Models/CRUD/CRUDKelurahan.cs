using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKelurahan
    {
        public static CRUDKelurahan CRUD = new CRUDKelurahan();

        public List<Kelurahan> ListKelurahan
        {
            get
            {
                List<Kelurahan> result = (List<Kelurahan>)HttpContext.Current.Session["ListKelurahan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKelurahan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kelurahan kelurahan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kelurahan.Add(kelurahan);
                db.SaveChanges();

                ListKelurahan.Add(kelurahan);
                HttpContext.Current.Session["ListKelurahan"] = ListKelurahan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kelurahan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kelurahan.ToList();
                return model;
            }
            catch
            {
                return new List<Kelurahan>();
            }
        }
        public bool Update(Kelurahan kelurahan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kelurahan.FirstOrDefault(o => o.KelurahanId == kelurahan.KelurahanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kelurahan);
                }
                else
                {
                    model.InjectFrom(kelurahan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKelurahan.FirstOrDefault(o => o.KelurahanId == kelurahan.KelurahanId);
                    context.InjectFrom(kelurahan);
                    HttpContext.Current.Session["ListKelurahan"] = ListKelurahan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kelurahan kelurahan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kelurahan.FirstOrDefault(o => o.KelurahanId == kelurahan.KelurahanId);
                model.InjectFrom(kelurahan);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kelurahan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kelurahan kelurahan)
        {
            try
            {
                var context = ListKelurahan.FirstOrDefault(o => o.KelurahanId == kelurahan.KelurahanId);
                ListKelurahan.Remove(context);
                HttpContext.Current.Session["ListKelurahan"] = ListKelurahan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid kecamatanId)
        {
            try
            {
                var context = ListKelurahan.Where(o => o.KecamatanId == kecamatanId).ToList();
                foreach (var m in context)
                {
                    ListKelurahan.Remove(m);
                }
                HttpContext.Current.Session["ListKelurahan"] = ListKelurahan;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Kelurahan Get1Record(Guid kelurahanId)
        {
            var model = ListKelurahan.FirstOrDefault(o => o.KelurahanId == kelurahanId);
            return model;
        }

        public List<Kelurahan> GetnRecord(Guid kecamatanId)
        {
            var model = ListKelurahan.Where(o => o.KecamatanId == kecamatanId).ToList();
            return model;
        }

        public List<Kelurahan> GetAllRecord()
        {
            var model = ListKelurahan;
            return model;
        }
    }
}