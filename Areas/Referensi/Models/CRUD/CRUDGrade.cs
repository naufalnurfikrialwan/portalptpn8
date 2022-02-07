using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDGrade
    {

        public static CRUDGrade CRUD = new CRUDGrade();

        public List<Grade> ListGrade
        {
            get
            {
                List<Grade> result = (List<Grade>)HttpContext.Current.Session["ListGrade"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGrade"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Grade grade)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Grade.Add(grade);
                db.SaveChanges();

                ListGrade.Add(grade);
                HttpContext.Current.Session["ListGrade"] = ListGrade;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Grade> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Grade.ToList();
                foreach(var m in model)
                {
                    m.FileFoto = m.GradeId.ToString() + ".jpg";
                    //m.Satuan = CRUDSatuan.CRUD.Get1Record(m.SatuanId);
                    //m.Kemasan = CRUDKemasan.CRUD.Get1Record(m.KemasanId);
                    //m.SubBudidaya = CRUDSubBudidaya.CRUD.Get1Record(m.SubBudidayaId);
                    //m.SubBudidaya = CRUDBudidaya.CRUD.Get1Record(m.SubMainBudidayaId);
                    //m.Text = m.Nama + " [ " + m.SubNama + ", " + m.SubNama + " ]"; // -[ " +
                             //m.JumlahSatuan.ToString().Trim() +" "+ m.Satuan.Nama + "/" + m.Kemasan.Nama+" ]";
                }
                return model;
            }
            catch
            {
                return new List<Grade>();
            }
        }

        

        public bool Update(Grade grade)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Grade.FirstOrDefault(o => o.GradeId == grade.GradeId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(grade);
                }
                else
                {
                    model.InjectFrom(grade);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListGrade.FirstOrDefault(o => o.GradeId == grade.GradeId);
                    context.InjectFrom(grade);
                    HttpContext.Current.Session["ListGrade"] = ListGrade;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Grade grade)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Grade.FirstOrDefault(o => o.GradeId == grade.GradeId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(grade);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Grade grade)
        {
            try
            {
                var context = ListGrade.FirstOrDefault(o => o.GradeId == grade.GradeId);
                ListGrade.Remove(context);
                HttpContext.Current.Session["ListGrade"] = ListGrade;
                CRUDVarian.CRUD.Erase(grade.GradeId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid subBudidayaId)
        {
            try
            {
                var context = ListGrade.Where(o => o.SubBudidayaId== subBudidayaId).ToList();
                foreach (var m in context)
                {
                    ListGrade.Remove(m);
                    CRUDVarian.CRUD.Erase(m.GradeId);
                }
                HttpContext.Current.Session["ListGrade"] = ListGrade;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Grade Get1Record(Guid gradeId)
        {
            var model = ListGrade.FirstOrDefault(o => o.GradeId == gradeId);
            return model;
        }

        public Grade Get1Record(string kd_grade)
        {
            var model = ListGrade.FirstOrDefault(o => o.kd_grade == kd_grade);
            return model;
        }

        public List<Grade> GetnRecord(Guid gradeId)
        {
            var model = ListGrade.Where(o => o.GradeId == gradeId).ToList();
            return model;
        }

        public List<Grade> GetAllRecord()
        {
            var model = ListGrade;
            return model;
        }
    }

    public class GradeIdAda : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var gradeId = (Guid)value;
                var model = CRUDGrade.CRUD.Get1Record(gradeId);
                if (model != null)
                    return ValidationResult.Success;
                else
                    return new ValidationResult("Grade Tidak Valid");
            }
            else
                return new ValidationResult("Grade Harus Diisi");
        }
    }
}