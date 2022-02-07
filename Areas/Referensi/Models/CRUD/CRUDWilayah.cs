using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDWilayah
    {
        public static CRUDWilayah CRUD = new CRUDWilayah();

        public List<Wilayah> ListWilayah
        {
            get
            {
                List<Wilayah> result = (List<Wilayah>)HttpContext.Current.Session["ListWilayah"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListWilayah"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Wilayah wilayah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Wilayah.Add(wilayah);
                db.SaveChanges();

                ListWilayah.Add(wilayah);
                HttpContext.Current.Session["ListWilayah"] = ListWilayah;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Wilayah> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Wilayah.ToList();
                foreach (var m in model)
                {
                    m.Propinsi = new Propinsi();
                    m.Propinsi = CRUDPropinsi.CRUD.Get1Record(m.PropinsiId);

                    m.Kota = new Kota();
                    m.Kota = CRUDKota.CRUD.Get1Record(m.KotaId);

                    m.Kecamatan = new Kecamatan();
                    m.Kecamatan = CRUDKecamatan.CRUD.Get1Record(m.KecamatanId);

                    m.Kelurahan = new Kelurahan();
                    m.Kelurahan = CRUDKelurahan.CRUD.Get1Record(m.KelurahanId);

                    m.Direksi = CRUDDireksi.CRUD.Get1Record(m.DireksiId);
                }

                return model;
            }
            catch
            {
                return new List<Wilayah>();
            }
        }
        public bool Update(Wilayah wilayah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Wilayah.FirstOrDefault(o => o.WilayahId == wilayah.WilayahId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(wilayah);
                }
                else
                {
                    model.InjectFrom(wilayah);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListWilayah.FirstOrDefault(o => o.WilayahId == wilayah.WilayahId);
                    context.InjectFrom(wilayah);
                    HttpContext.Current.Session["ListWilayah"] = ListWilayah;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Wilayah wilayah)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Wilayah.FirstOrDefault(o => o.WilayahId == wilayah.WilayahId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(wilayah);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Wilayah wilayah)
        {
            try
            {
                var context = ListWilayah.FirstOrDefault(o => o.WilayahId == wilayah.WilayahId);
                ListWilayah.Remove(context);
                HttpContext.Current.Session["ListWilayah"] = ListWilayah;
                CRUDKebun.CRUD.Erase(wilayah.WilayahId);
                CRUDBidang.CRUD.Erase(wilayah.WilayahId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid direksiId)
        {
            try
            {
                var context = ListWilayah.Where(o => o.DireksiId== direksiId).ToList();
                foreach (var m in context)
                {
                    ListWilayah.Remove(m);
                    CRUDKebun.CRUD.Erase(m.WilayahId);
                    CRUDBidang.CRUD.Erase(m.WilayahId);
                }
                HttpContext.Current.Session["ListWilayah"] = ListWilayah;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Wilayah Get1Record(Guid wilayahId)
        {
            var model = ListWilayah.FirstOrDefault(o => o.WilayahId == wilayahId);
            return model;
        }

        public Wilayah Get1Record(string kodeKebun)
        {
            var model = ListWilayah.FirstOrDefault(o => o.kd_kbn == kodeKebun);
            return model;
        }

        public List<Wilayah> GetnRecord(Guid wilayahId)
        {
            var model = ListWilayah.Where(o => o.WilayahId == wilayahId).ToList();
            return model;
        }

        public List<Wilayah> GetAllRecord()
        {
            var model = ListWilayah;
            return model;
        }
    }
}