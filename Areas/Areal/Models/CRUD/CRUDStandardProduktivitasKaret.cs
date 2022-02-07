using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDStandardProduktivitasKaret
    {
        public static CRUDStandardProduktivitasKaret CRUD = new CRUDStandardProduktivitasKaret();

        public List<StandardProduktivitasKaret> ListStandardProduktivitasKaret
        {
            get
            {
                List<StandardProduktivitasKaret> result = (List<StandardProduktivitasKaret>)HttpContext.Current.Session["ListStandardProduktivitasKaret"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListStandardProduktivitasKaret"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(StandardProduktivitasKaret standardProduktivitasKaret)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.StandardProduktivitasKaret.Add(standardProduktivitasKaret);
                db.SaveChanges();

                ListStandardProduktivitasKaret.Add(standardProduktivitasKaret);
                HttpContext.Current.Session["ListStandardProduktivitasKaret"] = ListStandardProduktivitasKaret;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<StandardProduktivitasKaret> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasKaret.ToList();
                return model;
            }
            catch
            {
                return new List<StandardProduktivitasKaret>();
            }
        }
        public bool Update(StandardProduktivitasKaret standardProduktivitasKaret)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasKaret.FirstOrDefault(o => o.StandardProduktivitasKaretId == standardProduktivitasKaret.StandardProduktivitasKaretId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(standardProduktivitasKaret);
                }
                else
                {
                    model.InjectFrom(standardProduktivitasKaret);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListStandardProduktivitasKaret.FirstOrDefault(o => o.StandardProduktivitasKaretId == standardProduktivitasKaret.StandardProduktivitasKaretId);
                    context.InjectFrom(standardProduktivitasKaret);
                    HttpContext.Current.Session["ListStandardProduktivitasKaret"] = ListStandardProduktivitasKaret;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(StandardProduktivitasKaret standardProduktivitasKaret)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.StandardProduktivitasKaret.FirstOrDefault(o => o.StandardProduktivitasKaretId == standardProduktivitasKaret.StandardProduktivitasKaretId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(standardProduktivitasKaret);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(StandardProduktivitasKaret standardProduktivitasKaret)
        {
            try
            {
                var context = ListStandardProduktivitasKaret.FirstOrDefault(o => o.StandardProduktivitasKaretId == standardProduktivitasKaret.StandardProduktivitasKaretId);
                ListStandardProduktivitasKaret.Remove(context);
                HttpContext.Current.Session["ListStandardProduktivitasKaret"] = ListStandardProduktivitasKaret;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListStandardProduktivitasKaret"] = null;
            return true;
        }

        public StandardProduktivitasKaret Get1Record(Guid standardProduktivitasKaretId)
        {
            var model = ListStandardProduktivitasKaret.FirstOrDefault(o => o.StandardProduktivitasKaretId == standardProduktivitasKaretId);
            return model;
        }
        public StandardProduktivitasKaret Get1Record(int umur)
        {
            var model = ListStandardProduktivitasKaret.FirstOrDefault(o => o.Umur == umur);
            if (model != null) return model; else return new StandardProduktivitasKaret();

        }

        public List<StandardProduktivitasKaret> GetnRecord(Guid standardProduktivitasKaretId)
        {
            var model = ListStandardProduktivitasKaret.Where(o => o.StandardProduktivitasKaretId == standardProduktivitasKaretId).ToList();
            return model;
        }

        public List<StandardProduktivitasKaret> GetAllRecord()
        {
            var model = ListStandardProduktivitasKaret;
            return model;
        }

    }
}