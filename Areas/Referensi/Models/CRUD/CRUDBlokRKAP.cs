using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBlokRKAP
    {
        public static CRUDBlokRKAP CRUD = new CRUDBlokRKAP();

        public List<BlokRKAP> ListBlokRKAP
        {
            get
            {
                List<BlokRKAP> result = (List<BlokRKAP>)HttpContext.Current.Session["ListBlokRKAP"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBlokRKAP"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(BlokRKAP blokRKAP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BlokRKAP.Add(blokRKAP);
                db.SaveChanges();

                ListBlokRKAP.Add(blokRKAP);
                HttpContext.Current.Session["ListBlokRKAP"] = ListBlokRKAP;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BlokRKAP> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRKAP.ToList();
                return model;
            }
            catch
            {
                return new List<BlokRKAP>();
            }
        }
        public bool Update(BlokRKAP blokRKAP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRKAP.FirstOrDefault(o => o.BlokRKAPId == blokRKAP.BlokRKAPId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(blokRKAP);
                }
                else
                {
                    model.InjectFrom(blokRKAP);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBlokRKAP.FirstOrDefault(o => o.BlokRKAPId == blokRKAP.BlokRKAPId);
                    context.InjectFrom(blokRKAP);
                    HttpContext.Current.Session["ListBlokRKAP"] = ListBlokRKAP;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BlokRKAP blokRKAP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRKAP.FirstOrDefault(o => o.BlokRKAPId == blokRKAP.BlokRKAPId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(blokRKAP);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BlokRKAP blokRKAP)
        {
            try
            {
                var context = ListBlokRKAP.FirstOrDefault(o => o.BlokRKAPId == blokRKAP.BlokRKAPId);
                ListBlokRKAP.Remove(context);
                HttpContext.Current.Session["ListBlokRKAP"] = ListBlokRKAP;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid blokId)
        {
            try
            {
                var context = ListBlokRKAP.Where(o => o.BlokId == blokId).ToList();
                foreach (var m in context)
                {
                    ListBlokRKAP.Remove(m);
                }
                HttpContext.Current.Session["ListBlokRKAP"] = ListBlokRKAP;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BlokRKAP Get1Record(Guid blokRKAPId)
        {
            var model = ListBlokRKAP.FirstOrDefault(o => o.BlokRKAPId == blokRKAPId);
            return model;
        }

        public List<BlokRKAP> GetnRecord(Guid blokRKAPId)
        {
            var model = ListBlokRKAP.Where(o => o.BlokRKAPId == blokRKAPId).ToList();
            return model;
        }

        public List<BlokRKAP> GetAllRecord()
        {
            var model = ListBlokRKAP;
            return model;
        }
    }
}