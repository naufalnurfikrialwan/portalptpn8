using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDRekening
    {
        public static CRUDRekening CRUD = new CRUDRekening();

        public List<Rekening> ListRekening
        {
            get
            {
                List<Rekening> result = (List<Rekening>)HttpContext.Current.Session["ListRekening"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRekening"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Rekening rekening)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Rekening.Add(rekening);
                db.SaveChanges();

                ListRekening.Add(rekening);
                HttpContext.Current.Session["ListRekening"] = ListRekening;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Rekening> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Rekening.ToList();
                foreach(var m in model)
                {
                    m.Text = m.Norek + "-" + m.NamaRekening + " (" + m.KelompokRekening + ";" + m.SaldoNormal + ")";
                }
                return model;
            }
            catch
            {
                return new List<Rekening>();
            }
        }
        public bool Update(Rekening rekening)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Rekening.FirstOrDefault(o => o.RekeningId == rekening.RekeningId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(rekening);
                }
                else
                {
                    model.InjectFrom(rekening);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListRekening.FirstOrDefault(o => o.RekeningId == rekening.RekeningId);
                    context.InjectFrom(rekening);
                    HttpContext.Current.Session["ListRekening"] = ListRekening;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Rekening rekening)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Rekening.FirstOrDefault(o => o.RekeningId == rekening.RekeningId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(rekening);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Rekening rekening)
        {
            try
            {
                var context = ListRekening.FirstOrDefault(o => o.RekeningId == rekening.RekeningId);
                ListRekening.Remove(context);
                HttpContext.Current.Session["ListRekening"] = ListRekening;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Rekening Get1Record(Guid rekeningId)
        {
            var model = ListRekening.FirstOrDefault(o => o.RekeningId == rekeningId);
            return model;
        }

        public Rekening Get1Record(string kodeRekening)
        {
            var model = ListRekening.FirstOrDefault(o => o.Norek == kodeRekening);
            return model;
        }

        public List<Rekening> GetnRecord(String cFilter)
        {
            var model = ListRekening.Where(o => o.NamaRekening.ToLower().Contains(cFilter.ToLower())).ToList();
            return model;
        }

        public List<Rekening> GetmRecord(String cFilter)
        {
            var model = ListRekening.Where(o => o.Norek.Substring(0,cFilter.Length).ToLower().Contains(cFilter.ToLower())).ToList();
            return model;
        }

        public List<Rekening> GetAllRecord()
        {
            var model = ListRekening;
            return model;
        }
    }

    public class KodeRekeningAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value!=null)
            {
                var rekg = (Rekening)value;
                var model = CRUDRekening.CRUD.Get1Record(rekg.RekeningId);
                if(model!=null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Rekening Tidak Valid");
            }
            else
                return new ValidationResult("Rekening Harus Diisi");
        }
    }
}