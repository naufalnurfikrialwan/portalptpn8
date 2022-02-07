using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKecamatan
    {
        public static CRUDKecamatan CRUD = new CRUDKecamatan();

        public List<Kecamatan> ListKecamatan
        {
            get
            {
                List<Kecamatan> result = (List<Kecamatan>)HttpContext.Current.Session["ListKecamatan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKecamatan"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kecamatan kecamatan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kecamatan.Add(kecamatan);
                db.SaveChanges();

                ListKecamatan.Add(kecamatan);
                HttpContext.Current.Session["ListKecamatan"] = ListKecamatan;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kecamatan> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kecamatan.ToList();
                return model;
            }
            catch
            {
                return new List<Kecamatan>();
            }
        }
        public bool Update(Kecamatan kecamatan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kecamatan.FirstOrDefault(o => o.KecamatanId == kecamatan.KecamatanId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kecamatan);
                }
                else
                {
                    model.InjectFrom(kecamatan);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKecamatan.FirstOrDefault(o => o.KecamatanId == kecamatan.KecamatanId);
                    context.InjectFrom(kecamatan);
                    HttpContext.Current.Session["ListKecamatan"] = ListKecamatan;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kecamatan kecamatan)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kecamatan.FirstOrDefault(o => o.KecamatanId == kecamatan.KecamatanId);
                model.InjectFrom(kecamatan);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kecamatan);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kecamatan kecamatan)
        {
            try
            {
                var context = ListKecamatan.FirstOrDefault(o => o.KecamatanId == kecamatan.KecamatanId);
                ListKecamatan.Remove(context);
                HttpContext.Current.Session["ListKecamatan"] = ListKecamatan;
                CRUDKelurahan.CRUD.Erase(kecamatan.KecamatanId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid kotaId)
        {
            try
            {
                var context = ListKecamatan.Where(o => o.KotaId == kotaId).ToList();
                foreach(var m in context)
                {
                    ListKecamatan.Remove(m);
                    CRUDKelurahan.CRUD.Erase(m.KecamatanId);
                }
                HttpContext.Current.Session["ListKecamatan"] = ListKecamatan;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Kecamatan Get1Record(Guid kecamatanId)
        {
            var model = ListKecamatan.FirstOrDefault(o => o.KecamatanId == kecamatanId);
            return model;
        }

        public List<Kecamatan> GetnRecord(Guid kotaId)
        {
            var model = ListKecamatan.Where(o => o.KotaId == kotaId).ToList();
            return model;
        }

        public List<Kecamatan> GetAllRecord()
        {
            var model = ListKecamatan;
            return model;
        }
    }
}