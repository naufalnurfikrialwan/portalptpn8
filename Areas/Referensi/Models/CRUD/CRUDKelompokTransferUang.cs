using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKelompokTransferUang
    {
        public static CRUDKelompokTransferUang CRUD = new CRUDKelompokTransferUang();

        public List<KelompokTransferUang> ListKelompokTransferUang
        {
            get
            {
                List<KelompokTransferUang> result = (List<KelompokTransferUang>)HttpContext.Current.Session["ListKelompokTransferUang"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKelompokTransferUang"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(KelompokTransferUang KelompokTransferUang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.KelompokTransferUang.Add(KelompokTransferUang);
                db.SaveChanges();

                ListKelompokTransferUang.Add(KelompokTransferUang);
                HttpContext.Current.Session["ListKelompokTransferUang"] = ListKelompokTransferUang;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<KelompokTransferUang> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KelompokTransferUang.ToList();
                return model;
            }
            catch
            {
                return new List<KelompokTransferUang>();
            }
        }
        public bool Update(KelompokTransferUang KelompokTransferUang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KelompokTransferUang.FirstOrDefault(o => o.KelompokTransferUangId == KelompokTransferUang.KelompokTransferUangId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(KelompokTransferUang);
                }
                else
                {
                    model.InjectFrom(KelompokTransferUang);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKelompokTransferUang.FirstOrDefault(o => o.KelompokTransferUangId == KelompokTransferUang.KelompokTransferUangId);
                    context.InjectFrom(KelompokTransferUang);
                    HttpContext.Current.Session["ListKelompokTransferUang"] = ListKelompokTransferUang;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(KelompokTransferUang KelompokTransferUang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.KelompokTransferUang.FirstOrDefault(o => o.KelompokTransferUangId == KelompokTransferUang.KelompokTransferUangId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(KelompokTransferUang);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(KelompokTransferUang KelompokTransferUang)
        {
            try
            {
                var context = ListKelompokTransferUang.FirstOrDefault(o => o.KelompokTransferUangId == KelompokTransferUang.KelompokTransferUangId);
                ListKelompokTransferUang.Remove(context);
                HttpContext.Current.Session["ListKelompokTransferUang"] = ListKelompokTransferUang;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public KelompokTransferUang Get1Record(Guid KelompokTransferUangId)
        {
            var model = ListKelompokTransferUang.FirstOrDefault(o => o.KelompokTransferUangId == KelompokTransferUangId);
            return model;
        }

        public List<KelompokTransferUang> GetnRecord(Guid KelompokTransferUangId)
        {
            var model = ListKelompokTransferUang.Where(o => o.KelompokTransferUangId == KelompokTransferUangId).ToList();
            return model;
        }

        public List<KelompokTransferUang> GetAllRecord()
        {
            var model = ListKelompokTransferUang;
            return model;
        }
    }
}