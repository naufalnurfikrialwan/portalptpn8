using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBidang
    {
        public static CRUDBidang CRUD = new CRUDBidang();

        public List<Bidang> ListBidang
        {
            get
            {
                List<Bidang> result = (List<Bidang>)HttpContext.Current.Session["ListBidang"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBidang"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Bidang bidang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Bidang.Add(bidang);
                db.SaveChanges();

                ListBidang.Add(bidang);
                HttpContext.Current.Session["ListBidang"] = ListBidang;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Bidang> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bidang.ToList();
                foreach(var m in model)
                {
                    m.Wilayah = CRUDWilayah.CRUD.Get1Record(m.WilayahId);
                }
                return model;
            }
            catch
            {
                return new List<Bidang>();
            }
        }
        public bool Update(Bidang bidang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bidang.FirstOrDefault(o => o.BidangId == bidang.BidangId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(bidang);
                }
                else
                {
                    model.InjectFrom(bidang);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBidang.FirstOrDefault(o => o.BidangId == bidang.BidangId);
                    context.InjectFrom(bidang);
                    HttpContext.Current.Session["ListBidang"] = ListBidang;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Bidang bidang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Bidang.FirstOrDefault(o => o.BidangId == bidang.BidangId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(bidang);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Bidang bidang)
        {
            try
            {
                var context = ListBidang.FirstOrDefault(o => o.BidangId == bidang.BidangId);
                ListBidang.Remove(context);
                HttpContext.Current.Session["ListBidang"] = ListBidang;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid wilayahId)
        {
            try
            {
                var context = ListBidang.Where(o => o.WilayahId== wilayahId).ToList();
                foreach (var m in context)
                {
                    ListBidang.Remove(m);
                }
                HttpContext.Current.Session["ListBidang"] = ListBidang;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Bidang Get1Record(Guid bidangId)
        {
            var model = ListBidang.FirstOrDefault(o => o.BidangId == bidangId);
            return model;
        }

        public List<Bidang> GetnRecord(Guid bidangId)
        {
            var model = ListBidang.Where(o => o.BidangId == bidangId).ToList();
            return model;
        }

        public List<Bidang> GetAllRecord()
        {
            var model = ListBidang;
            return model;
        }
    }
}