using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBlokRJPP
    {
        public static CRUDBlokRJPP CRUD = new CRUDBlokRJPP();

        public List<BlokRJPP> ListBlokRJPP
        {
            get
            {
                List<BlokRJPP> result = (List<BlokRJPP>)HttpContext.Current.Session["ListBlokRJPP"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBlokRJPP"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(BlokRJPP blokRJPP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BlokRJPP.Add(blokRJPP);
                db.SaveChanges();

                ListBlokRJPP.Add(blokRJPP);
                HttpContext.Current.Session["ListBlokRJPP"] = ListBlokRJPP;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BlokRJPP> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRJPP.ToList();
                return model;
            }
            catch
            {
                return new List<BlokRJPP>();
            }
        }
        public bool Update(BlokRJPP blokRJPP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRJPP.FirstOrDefault(o => o.BlokRJPPId == blokRJPP.BlokRJPPId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(blokRJPP);
                }
                else
                {
                    model.InjectFrom(blokRJPP);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBlokRJPP.FirstOrDefault(o => o.BlokRJPPId == blokRJPP.BlokRJPPId);
                    context.InjectFrom(blokRJPP);
                    HttpContext.Current.Session["ListBlokRJPP"] = ListBlokRJPP;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BlokRJPP blokRJPP)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRJPP.FirstOrDefault(o => o.BlokRJPPId == blokRJPP.BlokRJPPId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(blokRJPP);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BlokRJPP blokRJPP)
        {
            try
            {
                var context = ListBlokRJPP.FirstOrDefault(o => o.BlokRJPPId == blokRJPP.BlokRJPPId);
                ListBlokRJPP.Remove(context);
                HttpContext.Current.Session["ListBlokRJPP"] = ListBlokRJPP;
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
                var context = ListBlokRJPP.Where(o => o.BlokId == blokId).ToList();
                foreach (var m in context)
                {
                    ListBlokRJPP.Remove(m);
                }
                HttpContext.Current.Session["ListBlokRJPP"] = ListBlokRJPP;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BlokRJPP Get1Record(Guid blokRJPPId)
        {
            var model = ListBlokRJPP.FirstOrDefault(o => o.BlokRJPPId == blokRJPPId);
            return model;
        }

        public List<BlokRJPP> GetnRecord(Guid blokRJPPId)
        {
            var model = ListBlokRJPP.Where(o => o.BlokRJPPId == blokRJPPId).ToList();
            return model;
        }

        public List<BlokRJPP> GetAllRecord()
        {
            var model = ListBlokRJPP;
            return model;
        }
    }
}