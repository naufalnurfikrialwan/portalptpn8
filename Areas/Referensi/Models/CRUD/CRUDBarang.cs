using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBarang
    {
        public static CRUDBarang CRUD = new CRUDBarang();

        public List<Barang> ListBarang
        {
            get
            {
                List<Barang> result = (List<Barang>)HttpContext.Current.Session["ListBarang"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBarang"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Barang barang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Barang.Add(barang);
                db.SaveChanges();

                ListBarang.Add(barang);
                HttpContext.Current.Session["ListBarang"] = ListBarang;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Barang> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Barang.ToList();
                return model;
            }
            catch
            {
                return new List<Barang>();
            }
        }
        public bool Update(Barang barang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Barang.FirstOrDefault(o => o.BarangId == barang.BarangId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(barang);
                }
                else
                {
                    model.InjectFrom(barang);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBarang.FirstOrDefault(o => o.BarangId == barang.BarangId);
                    context.InjectFrom(barang);
                    HttpContext.Current.Session["ListBarang"] = ListBarang;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Barang barang)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Barang.FirstOrDefault(o => o.BarangId == barang.BarangId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(barang);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Barang barang)
        {
            try
            {
                var context = ListBarang.FirstOrDefault(o => o.BarangId == barang.BarangId);
                ListBarang.Remove(context);
                HttpContext.Current.Session["ListBarang"] = ListBarang;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Barang Get1Record(Guid barangId)
        {
            var model = ListBarang.FirstOrDefault(o => o.BarangId == barangId);
            return model;
        }

        public Barang Get1Record(string kodeBarang)
        {
            var model = ListBarang.FirstOrDefault(o => o.Norek == kodeBarang);
            return model;
        }

        public List<Barang> GetnRecord(String cFilter)
        {
            var model = ListBarang.Where(o => o.NamaBarang.ToLower().Contains(cFilter.ToLower())).ToList();
            return model;
        }

        public List<Barang> GetmRecord(String cFilter)
        {
            var model = ListBarang.Where(o => o.Norek.Substring(0, cFilter.Length).ToLower().Contains(cFilter.ToLower())).ToList();
            return model;
        }

        public List<Barang> GetAllRecord()
        {
            var model = ListBarang;
            return model;
        }
    }

    public class KodeBarangAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var rekg = (Barang)value;
                var model = CRUDBarang.CRUD.Get1Record(rekg.BarangId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Barang Tidak Valid");
            }
            else
                return new ValidationResult("Barang Harus Diisi");
        }
    }
}