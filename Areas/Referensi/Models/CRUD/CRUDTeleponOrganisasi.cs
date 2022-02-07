using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDTeleponOrganisasi
    {
        public static CRUDTeleponOrganisasi CRUD = new CRUDTeleponOrganisasi();

        public List<TeleponOrganisasi> ListTeleponOrganisasi
        {
            get
            {
                List<TeleponOrganisasi> result = (List<TeleponOrganisasi>)HttpContext.Current.Session["ListTeleponOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListTeleponOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(TeleponOrganisasi teleponOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.TeleponOrganisasi.Add(teleponOrganisasi);
                db.SaveChanges();

                ListTeleponOrganisasi.Add(teleponOrganisasi);
                HttpContext.Current.Session["ListTeleponOrganisasi"] = ListTeleponOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<TeleponOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TeleponOrganisasi.ToList();
                return model;
            }
            catch
            {
                return new List<TeleponOrganisasi>();
            }
        }
        public bool Update(TeleponOrganisasi teleponOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TeleponOrganisasi.FirstOrDefault(o => o.TeleponOrganisasiId == teleponOrganisasi.TeleponOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(teleponOrganisasi);
                }
                else
                {
                    model.InjectFrom(teleponOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListTeleponOrganisasi.FirstOrDefault(o => o.TeleponOrganisasiId == teleponOrganisasi.TeleponOrganisasiId);
                    context.InjectFrom(teleponOrganisasi);
                    HttpContext.Current.Session["ListTeleponOrganisasi"] = ListTeleponOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(TeleponOrganisasi teleponOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.TeleponOrganisasi.FirstOrDefault(o => o.TeleponOrganisasiId == teleponOrganisasi.TeleponOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(teleponOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(TeleponOrganisasi teleponOrganisasi)
        {
            try
            {
                var context = ListTeleponOrganisasi.FirstOrDefault(o => o.TeleponOrganisasiId == teleponOrganisasi.TeleponOrganisasiId);
                ListTeleponOrganisasi.Remove(context);
                HttpContext.Current.Session["ListTeleponOrganisasi"] = ListTeleponOrganisasi;
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
                var context = ListTeleponOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListTeleponOrganisasi.Remove(m);
                }
                HttpContext.Current.Session["ListTeleponOrganisasi"] = ListTeleponOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public TeleponOrganisasi Get1Record(Guid teleponOrganisasiId)
        {
            var model = ListTeleponOrganisasi.FirstOrDefault(o => o.TeleponOrganisasiId == teleponOrganisasiId);
            return model;
        }

        public List<TeleponOrganisasi> GetnRecord(Guid teleponOrganisasiId)
        {
            var model = ListTeleponOrganisasi.Where(o => o.TeleponOrganisasiId == teleponOrganisasiId).ToList();
            return model;
        }

        public List<TeleponOrganisasi> GetAllRecord()
        {
            var model = ListTeleponOrganisasi;
            return model;
        }
    }
}