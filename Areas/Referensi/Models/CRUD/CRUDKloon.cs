using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKloon
    {
        public static CRUDKloon CRUD = new CRUDKloon();

        public List<Kloon> ListKloon
        {
            get
            {
                List<Kloon> result = (List<Kloon>)HttpContext.Current.Session["ListKloon"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKloon"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kloon kloon)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kloon.Add(kloon);
                db.SaveChanges();

                ListKloon.Add(kloon);
                HttpContext.Current.Session["ListKloon"] = ListKloon;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kloon> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kloon.ToList();
                return model;
            }
            catch
            {
                return new List<Kloon>();
            }
        }
        public bool Update(Kloon kloon)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kloon.FirstOrDefault(o => o.KloonId == kloon.KloonId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kloon);
                }
                else
                {
                    model.InjectFrom(kloon);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKloon.FirstOrDefault(o => o.KloonId == kloon.KloonId);
                    context.InjectFrom(kloon);
                    HttpContext.Current.Session["ListKloon"] = ListKloon;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kloon kloon)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kloon.FirstOrDefault(o => o.KloonId == kloon.KloonId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kloon);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kloon kloon)
        {
            try
            {
                var context = ListKloon.FirstOrDefault(o => o.KloonId == kloon.KloonId);
                ListKloon.Remove(context);
                HttpContext.Current.Session["ListKloon"] = ListKloon;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public Kloon Get1Record(Guid kloonId)
        {
            var model = ListKloon.FirstOrDefault(o => o.KloonId == kloonId);
            return model;
        }

        public List<Kloon> GetnRecord(Guid kloonId)
        {
            var model = ListKloon.Where(o => o.KloonId == kloonId).ToList();
            return model;
        }

        public List<Kloon> GetAllRecord()
        {
            var model = ListKloon;
            return model;
        }
    }
}