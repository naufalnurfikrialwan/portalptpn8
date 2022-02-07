using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKomisaris
    {
        public static CRUDKomisaris CRUD = new CRUDKomisaris();

        public List<Komisaris> ListKomisaris
        {
            get
            {
                List<Komisaris> result = (List<Komisaris>)HttpContext.Current.Session["ListKomisaris"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKomisaris"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Komisaris komisaris)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Komisaris.Add(komisaris);
                db.SaveChanges();

                ListKomisaris.Add(komisaris);
                HttpContext.Current.Session["ListKomisaris"] = ListKomisaris;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Komisaris> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Komisaris.ToList();
                return model;
            }
            catch
            {
                return new List<Komisaris>();
            }
        }
        public bool Update(Komisaris komisaris)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Komisaris.FirstOrDefault(o => o.KomisarisId == komisaris.KomisarisId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(komisaris);
                }
                else
                {
                    model.InjectFrom(komisaris);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKomisaris.FirstOrDefault(o => o.KomisarisId == komisaris.KomisarisId);
                    context.InjectFrom(komisaris);
                    HttpContext.Current.Session["ListKomisaris"] = ListKomisaris;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Komisaris komisaris)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Komisaris.FirstOrDefault(o => o.KomisarisId == komisaris.KomisarisId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(komisaris);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Komisaris komisaris)
        {
            try
            {
                var context = ListKomisaris.FirstOrDefault(o => o.KomisarisId == komisaris.KomisarisId);
                ListKomisaris.Remove(context);
                HttpContext.Current.Session["ListKomisaris"] = ListKomisaris;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid organisasiId)
        {
            try
            {
                var context = ListKomisaris.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach(var m in context)
                {
                    ListKomisaris.Remove(m);
                }
                HttpContext.Current.Session["ListKomisaris"] = ListKomisaris;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Komisaris Get1Record(Guid komisarisId)
        {
            var model = ListKomisaris.FirstOrDefault(o => o.KomisarisId == komisarisId);
            return model;
        }

        public List<Komisaris> GetnRecord(Guid komisarisId)
        {
            var model = ListKomisaris.Where(o => o.KomisarisId == komisarisId).ToList();
            return model;
        }

        public List<Komisaris> GetAllRecord()
        {
            var model = ListKomisaris;
            return model;
        }
    }
}