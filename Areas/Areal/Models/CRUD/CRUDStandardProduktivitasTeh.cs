using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDStandardProduktivitasTeh
    {
        public static CRUDStandardProduktivitasTeh CRUD = new CRUDStandardProduktivitasTeh();

        public List<StandardProduktivitasTeh> ListStandardProduktivitasTeh
        {
            get
            {
                List<StandardProduktivitasTeh> result = (List<StandardProduktivitasTeh>)HttpContext.Current.Session["ListStandardProduktivitasTeh"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListStandardProduktivitasTeh"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(StandardProduktivitasTeh standardProduktivitasTeh)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.StandardProduktivitasTeh.Add(standardProduktivitasTeh);
                db.SaveChanges();

                ListStandardProduktivitasTeh.Add(standardProduktivitasTeh);
                HttpContext.Current.Session["ListStandardProduktivitasTeh"] = ListStandardProduktivitasTeh;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<StandardProduktivitasTeh> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasTeh.ToList();
                return model;
            }
            catch
            {
                return new List<StandardProduktivitasTeh>();
            }
        }
        public bool Update(StandardProduktivitasTeh standardProduktivitasTeh)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasTeh.FirstOrDefault(o => o.StandardProduktivitasTehId == standardProduktivitasTeh.StandardProduktivitasTehId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(standardProduktivitasTeh);
                }
                else
                {
                    model.InjectFrom(standardProduktivitasTeh);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListStandardProduktivitasTeh.FirstOrDefault(o => o.StandardProduktivitasTehId == standardProduktivitasTeh.StandardProduktivitasTehId);
                    context.InjectFrom(standardProduktivitasTeh);
                    HttpContext.Current.Session["ListStandardProduktivitasTeh"] = ListStandardProduktivitasTeh;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(StandardProduktivitasTeh standardProduktivitasTeh)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasTeh.FirstOrDefault(o => o.StandardProduktivitasTehId == standardProduktivitasTeh.StandardProduktivitasTehId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(standardProduktivitasTeh);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(StandardProduktivitasTeh standardProduktivitasTeh)
        {
            try
            {
                var context = ListStandardProduktivitasTeh.FirstOrDefault(o => o.StandardProduktivitasTehId == standardProduktivitasTeh.StandardProduktivitasTehId);
                ListStandardProduktivitasTeh.Remove(context);
                HttpContext.Current.Session["ListStandardProduktivitasTeh"] = ListStandardProduktivitasTeh;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListStandardProduktivitasTeh"] = null;
            return true;
        }

        public StandardProduktivitasTeh Get1Record(Guid standardProduktivitasTehId)
        {
            var model = ListStandardProduktivitasTeh.FirstOrDefault(o => o.StandardProduktivitasTehId == standardProduktivitasTehId);
            return model;
        }
        public StandardProduktivitasTeh Get1Record(string dataran)
        {
            var model = ListStandardProduktivitasTeh.FirstOrDefault(o => o.Dataran== dataran);
            if (model != null) return model; else return new StandardProduktivitasTeh();

        }

        public List<StandardProduktivitasTeh> GetnRecord(Guid standardProduktivitasTehId)
        {
            var model = ListStandardProduktivitasTeh.Where(o => o.StandardProduktivitasTehId == standardProduktivitasTehId).ToList();
            return model;
        }

        public List<StandardProduktivitasTeh> GetAllRecord()
        {
            var model = ListStandardProduktivitasTeh;
            return model;
        }

    }
}