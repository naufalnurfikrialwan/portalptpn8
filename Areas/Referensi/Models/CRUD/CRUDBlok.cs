using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBlok
    {
        public static CRUDBlok CRUD = new CRUDBlok();

        public List<Blok> ListBlok
        {
            get
            {
                List<Blok> result = (List<Blok>)HttpContext.Current.Session["ListBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBlok"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Blok blok)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Blok.Add(blok);
                db.SaveChanges();

                ListBlok.Add(blok);
                HttpContext.Current.Session["ListBlok"] = ListBlok;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Blok> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Blok.ToList();
                foreach(var m in model)
                {
                    m.Afdeling = CRUDAfdeling.CRUD.Get1Record(m.AfdelingId);
                }
                return model;
            }
            catch
            {
                return new List<Blok>();
            }
        }
        public bool Update(Blok blok)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Blok.FirstOrDefault(o => o.BlokId == blok.BlokId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(blok);
                }
                else
                {
                    model.InjectFrom(blok);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBlok.FirstOrDefault(o => o.BlokId == blok.BlokId);
                    context.InjectFrom(blok);
                    HttpContext.Current.Session["ListBlok"] = ListBlok;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Blok blok)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Blok.FirstOrDefault(o => o.BlokId == blok.BlokId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(blok);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Blok blok)
        {
            try
            {
                var context = ListBlok.FirstOrDefault(o => o.BlokId == blok.BlokId);
                ListBlok.Remove(context);
                HttpContext.Current.Session["ListBlok"] = ListBlok;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid afdelingId)
        {
            try
            {
                var context = ListBlok.Where(o => o.AfdelingId == afdelingId).ToList();
                foreach (var m in context)
                {
                    ListBlok.Remove(m);
                }
                HttpContext.Current.Session["ListBlok"] = ListBlok;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Blok Get1Record(Guid blokId)
        {
            var model = ListBlok.FirstOrDefault(o => o.BlokId == blokId);
            return model;
        }
        public Blok Get1Record(string kodeBlok)
        {
            var model = ListBlok.FirstOrDefault(o => o.kd_blok == kodeBlok);
            return model;
        }

        public List<Blok> GetnRecord(Guid blokId)
        {
            var model = ListBlok.Where(o => o.BlokId == blokId).ToList();
            return model;
        }

        public List<Blok> GetAllRecord()
        {
            var model = ListBlok;
            return model;
        }
    }
}