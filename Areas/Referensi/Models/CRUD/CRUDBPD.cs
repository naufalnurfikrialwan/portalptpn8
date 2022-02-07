using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBPD
    {
        public static CRUDBPD CRUD = new CRUDBPD();

        public List<BPD> ListBPD
        {
            get
            {
                List<BPD> result = (List<BPD>)HttpContext.Current.Session["ListBPD"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBPD"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(BPD bpd)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BPD.Add(bpd);
                db.SaveChanges();

                ListBPD.Add(bpd);
                HttpContext.Current.Session["ListBPD"] = ListBPD;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BPD> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BPD.ToList();
                return model;
            }
            catch
            {
                return new List<BPD>();
            }
        }
        public bool Update(BPD bpd)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BPD.FirstOrDefault(o => o.BPDId == bpd.BPDId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(bpd);
                }
                else
                {
                    model.InjectFrom(bpd);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBPD.FirstOrDefault(o => o.BPDId == bpd.BPDId);
                    context.InjectFrom(bpd);
                    HttpContext.Current.Session["ListBPD"] = ListBPD;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BPD bpd)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BPD.FirstOrDefault(o => o.BPDId == bpd.BPDId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(bpd);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BPD bpd)
        {
            try
            {
                var context = ListBPD.FirstOrDefault(o => o.BPDId == bpd.BPDId);
                ListBPD.Remove(context);
                HttpContext.Current.Session["ListBPD"] = ListBPD;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public BPD Get1Record(Guid bpdId)
        {
            var model = ListBPD.FirstOrDefault(o => o.BPDId == bpdId);
            return model;
        }

        public List<BPD> GetnRecord(Guid bpdId)
        {
            var model = ListBPD.Where(o => o.BPDId == bpdId).ToList();
            return model;
        }

        public List<BPD> GetAllRecord()
        {
            var model = ListBPD;
            return model;
        }
    }
}