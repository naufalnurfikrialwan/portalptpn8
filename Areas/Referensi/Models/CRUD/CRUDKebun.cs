using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDKebun
    {
        public static CRUDKebun CRUD = new CRUDKebun();

        public List<Kebun> ListKebun
        {
            get
            {
                List<Kebun> result = (List<Kebun>)HttpContext.Current.Session["ListKebun"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKebun"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Kebun kebun)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Kebun.Add(kebun);
                db.SaveChanges();

                ListKebun.Add(kebun);
                HttpContext.Current.Session["ListKebun"] = ListKebun;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Kebun> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kebun.ToList();
                foreach (var m in model)
                {
                    m.Wilayah = new Wilayah();
                    m.Wilayah = CRUDWilayah.CRUD.Get1Record(m.WilayahId);

                    m.Propinsi = new Propinsi();
                    m.Propinsi = CRUDPropinsi.CRUD.Get1Record(m.PropinsiId);

                    m.Kota = new Kota();
                    m.Kota = CRUDKota.CRUD.Get1Record(m.KotaId);

                    m.Kecamatan = new Kecamatan();
                    m.Kecamatan = CRUDKecamatan.CRUD.Get1Record(m.KecamatanId);

                    m.Kelurahan = new Kelurahan();
                    m.Kelurahan = CRUDKelurahan.CRUD.Get1Record(m.KelurahanId);

                }
                return model;
            }
            catch
            {
                return new List<Kebun>();
            }
        }
        public bool Update(Kebun kebun)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kebun.FirstOrDefault(o => o.KebunId == kebun.KebunId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kebun);
                }
                else
                {
                    model.InjectFrom(kebun);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKebun.FirstOrDefault(o => o.KebunId == kebun.KebunId);
                    context.InjectFrom(kebun);
                    HttpContext.Current.Session["ListKebun"] = ListKebun;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Kebun kebun)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Kebun.FirstOrDefault(o => o.KebunId == kebun.KebunId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kebun);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Kebun kebun)
        {
            try
            {
                var context = ListKebun.FirstOrDefault(o => o.KebunId == kebun.KebunId);
                ListKebun.Remove(context);
                HttpContext.Current.Session["ListKebun"] = ListKebun;
                CRUDAfdeling.CRUD.Erase(kebun.KebunId);
                CRUDKebunBudidaya.CRUD.Erase(kebun.KebunId);
                CRUDKebunPeta.CRUD.Erase(kebun.KebunId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid wilayahId)
        {
            try
            {
                var context = ListKebun.Where(o => o.WilayahId == wilayahId).ToList();
                foreach (var m in context)
                {
                    ListKebun.Remove(m);
                    CRUDAfdeling.CRUD.Erase(m.KebunId);
                    CRUDKebunBudidaya.CRUD.Erase(m.KebunId);
                    CRUDKebunPeta.CRUD.Erase(m.KebunId);
                }
                HttpContext.Current.Session["ListKebun"] = ListKebun;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Kebun Get1Record(Guid kebunId)
        {
            var model = ListKebun.FirstOrDefault(o => o.KebunId == kebunId);
            return model;
        }

        public Kebun Get1Record(string kodeKebun)
        {
            var model = ListKebun.FirstOrDefault(o => o.kd_kbn == kodeKebun);
            return model;
        }

        public List<Kebun> GetnRecord(Guid kebunId)
        {
            var model = ListKebun.Where(o => o.KebunId == kebunId).ToList();
            return model;
        }

        public List<Kebun> GetAllRecord()
        {
            var model = ListKebun;
            return model;
        }
    }

    public class KebunIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var kebunId = (Guid)value;
                var model = CRUDKebun.CRUD.Get1Record(kebunId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Kebun Tidak Valid");
            }
            else
                return new ValidationResult("Kebun Harus Diisi");
        }
    }
}