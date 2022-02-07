using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDEmailOrganisasi
    {
        public static CRUDEmailOrganisasi CRUD = new CRUDEmailOrganisasi();

        public List<EmailOrganisasi> ListEmailOrganisasi
        {
            get
            {
                List<EmailOrganisasi> result = (List<EmailOrganisasi>)HttpContext.Current.Session["ListEmailOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListEmailOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(EmailOrganisasi emailOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.EmailOrganisasi.Add(emailOrganisasi);
                db.SaveChanges();

                ListEmailOrganisasi.Add(emailOrganisasi);
                HttpContext.Current.Session["ListEmailOrganisasi"] = ListEmailOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<EmailOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.EmailOrganisasi.ToList();
                return model;
            }
            catch
            {
                return new List<EmailOrganisasi>();
            }
        }
        public bool Update(EmailOrganisasi emailOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.EmailOrganisasi.FirstOrDefault(o => o.EmailOrganisasiId == emailOrganisasi.EmailOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(emailOrganisasi);
                }
                else
                {
                    model.InjectFrom(emailOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListEmailOrganisasi.FirstOrDefault(o => o.EmailOrganisasiId == emailOrganisasi.EmailOrganisasiId);
                    context.InjectFrom(emailOrganisasi);
                    HttpContext.Current.Session["ListEmailOrganisasi"] = ListEmailOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(EmailOrganisasi emailOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.EmailOrganisasi.FirstOrDefault(o => o.EmailOrganisasiId == emailOrganisasi.EmailOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(emailOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(EmailOrganisasi emailOrganisasi)
        {
            try
            {
                var context = ListEmailOrganisasi.FirstOrDefault(o => o.EmailOrganisasiId == emailOrganisasi.EmailOrganisasiId);
                ListEmailOrganisasi.Remove(context);
                HttpContext.Current.Session["ListEmailOrganisasi"] = ListEmailOrganisasi;
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
                var context = ListEmailOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListEmailOrganisasi.Remove(m);
                }
                HttpContext.Current.Session["ListEmailOrganisasi"] = ListEmailOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public EmailOrganisasi Get1Record(Guid emailOrganisasiId)
        {
            var model = ListEmailOrganisasi.FirstOrDefault(o => o.EmailOrganisasiId == emailOrganisasiId);
            return model;
        }

        public List<EmailOrganisasi> GetnRecord(Guid emailOrganisasiId)
        {
            var model = ListEmailOrganisasi.Where(o => o.EmailOrganisasiId == emailOrganisasiId).ToList();
            return model;
        }

        public List<EmailOrganisasi> GetAllRecord()
        {
            var model = ListEmailOrganisasi;
            return model;
        }
    }
}