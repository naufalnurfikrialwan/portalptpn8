using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDStandardProduktivitasSawit
    {
        public static CRUDStandardProduktivitasSawit CRUD = new CRUDStandardProduktivitasSawit();

        public List<StandardProduktivitasSawit> ListStandardProduktivitasSawit
        {
            get
            {
                List<StandardProduktivitasSawit> result = (List<StandardProduktivitasSawit>)HttpContext.Current.Session["ListStandardProduktivitasSawit"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListStandardProduktivitasSawit"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(StandardProduktivitasSawit standardProduktivitasSawit)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.StandardProduktivitasSawit.Add(standardProduktivitasSawit);
                db.SaveChanges();

                ListStandardProduktivitasSawit.Add(standardProduktivitasSawit);
                HttpContext.Current.Session["ListStandardProduktivitasSawit"] = ListStandardProduktivitasSawit;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<StandardProduktivitasSawit> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasSawit.ToList();
                return model;
            }
            catch
            {
                return new List<StandardProduktivitasSawit>();
            }
        }
        public bool Update(StandardProduktivitasSawit standardProduktivitasSawit)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasSawit.FirstOrDefault(o => o.StandardProduktivitasSawitId == standardProduktivitasSawit.StandardProduktivitasSawitId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(standardProduktivitasSawit);
                }
                else
                {
                    model.InjectFrom(standardProduktivitasSawit);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListStandardProduktivitasSawit.FirstOrDefault(o => o.StandardProduktivitasSawitId == standardProduktivitasSawit.StandardProduktivitasSawitId);
                    context.InjectFrom(standardProduktivitasSawit);
                    HttpContext.Current.Session["ListStandardProduktivitasSawit"] = ListStandardProduktivitasSawit;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(StandardProduktivitasSawit standardProduktivitasSawit)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasSawit.FirstOrDefault(o => o.StandardProduktivitasSawitId == standardProduktivitasSawit.StandardProduktivitasSawitId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(standardProduktivitasSawit);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(StandardProduktivitasSawit standardProduktivitasSawit)
        {
            try
            {
                var context = ListStandardProduktivitasSawit.FirstOrDefault(o => o.StandardProduktivitasSawitId == standardProduktivitasSawit.StandardProduktivitasSawitId);
                ListStandardProduktivitasSawit.Remove(context);
                HttpContext.Current.Session["ListStandardProduktivitasSawit"] = ListStandardProduktivitasSawit;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListStandardProduktivitasSawit"] = null;
            return true;
        }

        public StandardProduktivitasSawit Get1Record(Guid standardProduktivitasSawitId)
        {
            var model = ListStandardProduktivitasSawit.FirstOrDefault(o => o.StandardProduktivitasSawitId == standardProduktivitasSawitId);
            return model;
        }
        public StandardProduktivitasSawit Get1Record(int umur)
        {
            var model = ListStandardProduktivitasSawit.FirstOrDefault(o => o.Umur == umur);
            if (model != null) return model; else return new StandardProduktivitasSawit();

        }

        public List<StandardProduktivitasSawit> GetnRecord(Guid standardProduktivitasSawitId)
        {
            var model = ListStandardProduktivitasSawit.Where(o => o.StandardProduktivitasSawitId == standardProduktivitasSawitId).ToList();
            return model;
        }

        public List<StandardProduktivitasSawit> GetAllRecord()
        {
            var model = ListStandardProduktivitasSawit;
            return model;
        }

    }
}