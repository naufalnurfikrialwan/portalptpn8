using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKota
    {
        public static CRUDKota CRUD = new CRUDKota();

        public List<Kota> ListKota
        {
            get
            {
                List<Kota> result = (List<Kota>)HttpContext.Current.Session["ListKota"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKota"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kota kota)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kota.Add(kota);
                db.SaveChanges();

                ListKota.Add(kota);
                HttpContext.Current.Session["ListKota"] = ListKota;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kota> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kota.ToList();
                return model;
            }
            catch
            {
                return new List<Kota>();
            }
        }
        public bool Update(Kota kota)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kota.FirstOrDefault(o => o.KotaId == kota.KotaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kota);
                }
                else
                {
                    model.InjectFrom(kota);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKota.FirstOrDefault(o => o.KotaId == kota.KotaId);
                    context.InjectFrom(kota);
                    HttpContext.Current.Session["ListKota"] = ListKota;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kota kota)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kota.FirstOrDefault(o => o.KotaId == kota.KotaId);
                model.InjectFrom(kota);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kota);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kota kota)
        {
            try
            {
                var context = ListKota.FirstOrDefault(o => o.KotaId == kota.KotaId);
                ListKota.Remove(context);
                HttpContext.Current.Session["ListKota"] = ListKota;
                CRUDKecamatan.CRUD.Erase(kota.KotaId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid propinsiId)
        {
            try
            {
                var context = ListKota.Where(o => o.PropinsiId == propinsiId).ToList();
                foreach(var m in context)
                {
                    ListKota.Remove(m);
                    CRUDKecamatan.CRUD.Erase(m.KotaId);
                }
                HttpContext.Current.Session["ListKota"] = ListKota;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Kota Get1Record(Guid kotaId)
        {
            var model = ListKota.FirstOrDefault(o => o.KotaId == kotaId);
            return model;
        }

        public List<Kota> GetnRecord(Guid propinsiId)
        {
            var model = ListKota.Where(o => o.PropinsiId == propinsiId).ToList();
            return model;
        }

        public List<Kota> GetAllRecord()
        {
            var model = ListKota;
            return model;
        }
    }
}