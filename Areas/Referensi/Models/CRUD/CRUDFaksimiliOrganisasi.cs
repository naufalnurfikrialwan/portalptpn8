using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDFaksimiliOrganisasi
    {
        public static CRUDFaksimiliOrganisasi CRUD = new CRUDFaksimiliOrganisasi();

        public List<FaksimiliOrganisasi> ListFaksimiliOrganisasi
        {
            get
            {
                List<FaksimiliOrganisasi> result = (List<FaksimiliOrganisasi>)HttpContext.Current.Session["ListFaksimiliOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListFaksimiliOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(FaksimiliOrganisasi faksimiliOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.FaksimiliOrganisasi.Add(faksimiliOrganisasi);
                db.SaveChanges();

                ListFaksimiliOrganisasi.Add(faksimiliOrganisasi);
                HttpContext.Current.Session["ListFaksimiliOrganisasi"] = ListFaksimiliOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<FaksimiliOrganisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.FaksimiliOrganisasi.ToList();
                return model;
            }
            catch
            {
                return new List<FaksimiliOrganisasi>();
            }
        }
        public bool Update(FaksimiliOrganisasi faksimiliOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.FaksimiliOrganisasi.FirstOrDefault(o => o.FaksimiliOrganisasiId == faksimiliOrganisasi.FaksimiliOrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(faksimiliOrganisasi);
                }
                else
                {
                    model.InjectFrom(faksimiliOrganisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListFaksimiliOrganisasi.FirstOrDefault(o => o.FaksimiliOrganisasiId == faksimiliOrganisasi.FaksimiliOrganisasiId);
                    context.InjectFrom(faksimiliOrganisasi);
                    HttpContext.Current.Session["ListFaksimiliOrganisasi"] = ListFaksimiliOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(FaksimiliOrganisasi faksimiliOrganisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.FaksimiliOrganisasi.FirstOrDefault(o => o.FaksimiliOrganisasiId == faksimiliOrganisasi.FaksimiliOrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(faksimiliOrganisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(FaksimiliOrganisasi faksimiliOrganisasi)
        {
            try
            {
                var context = ListFaksimiliOrganisasi.FirstOrDefault(o => o.FaksimiliOrganisasiId == faksimiliOrganisasi.FaksimiliOrganisasiId);
                ListFaksimiliOrganisasi.Remove(context);
                HttpContext.Current.Session["ListFaksimiliOrganisasi"] = ListFaksimiliOrganisasi;
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
                var context = ListFaksimiliOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
                foreach (var m in context)
                {
                    ListFaksimiliOrganisasi.Remove(m);
                }
                HttpContext.Current.Session["ListFaksimiliOrganisasi"] = ListFaksimiliOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public FaksimiliOrganisasi Get1Record(Guid faksimiliOrganisasiId)
        {
            var model = ListFaksimiliOrganisasi.FirstOrDefault(o => o.FaksimiliOrganisasiId == faksimiliOrganisasiId);
            return model;
        }

        public List<FaksimiliOrganisasi> GetnRecord(Guid faksimiliOrganisasiId)
        {
            var model = ListFaksimiliOrganisasi.Where(o => o.FaksimiliOrganisasiId == faksimiliOrganisasiId).ToList();
            return model;
        }

        public List<FaksimiliOrganisasi> GetAllRecord()
        {
            var model = ListFaksimiliOrganisasi;
            return model;
        }
    }
}