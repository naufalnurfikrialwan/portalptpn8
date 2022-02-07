using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDNPWPOrganisasi
    {
        public static CRUDNPWPOrganisasi CRUD = new CRUDNPWPOrganisasi();

        public List<NPWPOrganisasi> ListNPWPOrganisasi
        {
            get
            {
                List<NPWPOrganisasi> result = (List<NPWPOrganisasi>)HttpContext.Current.Session["ListNPWPOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListNPWPOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(NPWPOrganisasi npwpOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.NPWPOrganisasi.Add(npwpOrganisasi);
                db.SaveChanges();

                ListNPWPOrganisasi.Add(npwpOrganisasi);
                HttpContext.Current.Session["ListNPWPOrganisasi"] = ListNPWPOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<NPWPOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.NPWPOrganisasi.ToList();
                return model;
            }
            catch
            {
                return new List<NPWPOrganisasi>();
            }
        }
        public bool Update(NPWPOrganisasi npwpOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.NPWPOrganisasi.FirstOrDefault(o => o.NPWPOrganisasiId == npwpOrganisasi.NPWPOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(npwpOrganisasi);
                }
                else
                {
                    model.InjectFrom(npwpOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListNPWPOrganisasi.FirstOrDefault(o => o.NPWPOrganisasiId == npwpOrganisasi.NPWPOrganisasiId);
                    context.InjectFrom(npwpOrganisasi);
                    HttpContext.Current.Session["ListNPWPOrganisasi"] = ListNPWPOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(NPWPOrganisasi npwpOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.NPWPOrganisasi.FirstOrDefault(o => o.NPWPOrganisasiId == npwpOrganisasi.NPWPOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(npwpOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(NPWPOrganisasi npwpOrganisasi)
        {
            try
            {
                var context = ListNPWPOrganisasi.FirstOrDefault(o => o.NPWPOrganisasiId == npwpOrganisasi.NPWPOrganisasiId);
                ListNPWPOrganisasi.Remove(context);
                HttpContext.Current.Session["ListNPWPOrganisasi"] = ListNPWPOrganisasi;
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
                var context = ListNPWPOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListNPWPOrganisasi.Remove(m);
                }
                HttpContext.Current.Session["ListNPWPOrganisasi"] = ListNPWPOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }


        public NPWPOrganisasi Get1Record(Guid npwpOrganisasiId)
        {
            var model = ListNPWPOrganisasi.FirstOrDefault(o => o.NPWPOrganisasiId == npwpOrganisasiId);
            return model;
        }

        public List<NPWPOrganisasi> GetnRecord(Guid npwpOrganisasiId)
        {
            var model = ListNPWPOrganisasi.Where(o => o.NPWPOrganisasiId == npwpOrganisasiId).ToList();
            return model;
        }

        public List<NPWPOrganisasi> GetAllRecord()
        {
            var model = ListNPWPOrganisasi;
            return model;
        }
    }
}