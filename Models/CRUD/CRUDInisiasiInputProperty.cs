using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDInisiasiInputProperty
    {
        public static CRUDInisiasiInputProperty CRUD = new CRUDInisiasiInputProperty();

        public List<InisiasiInputProperty> ListInisiasiInputProperty
        {
            get
            {
                List<InisiasiInputProperty> result = (List<InisiasiInputProperty>)HttpContext.Current.Session["ListInisiasiInputProperty"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListInisiasiInputProperty"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(InisiasiInputProperty aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                db.InisiasiInputProperty.Add(aplikasi);
                db.SaveChanges();

                ListInisiasiInputProperty.Add(aplikasi);
                HttpContext.Current.Session["ListInisiasiInputProperty"] = ListInisiasiInputProperty;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<InisiasiInputProperty> Read()
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.InisiasiInputProperty.ToList();
                return model;
            }
            catch
            {
                return new List<InisiasiInputProperty>();
            }
        }

        public bool Update(InisiasiInputProperty aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.InisiasiInputProperty.FirstOrDefault(o => o.InisiasiInputPropertyId == aplikasi.InisiasiInputPropertyId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(aplikasi);
                }
                else
                {
                    model.InjectFrom(aplikasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListInisiasiInputProperty.FirstOrDefault(o => o.InisiasiInputPropertyId == aplikasi.InisiasiInputPropertyId);
                    context.InjectFrom(aplikasi);
                    HttpContext.Current.Session["ListInisiasiInputProperty"] = ListInisiasiInputProperty;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(InisiasiInputProperty aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.InisiasiInputProperty.FirstOrDefault(o => o.InisiasiInputPropertyId == aplikasi.InisiasiInputPropertyId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(aplikasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(InisiasiInputProperty aplikasi)
        {
            try
            {
                var context = ListInisiasiInputProperty.FirstOrDefault(o => o.InisiasiInputPropertyId == aplikasi.InisiasiInputPropertyId);
                ListInisiasiInputProperty.Remove(context);
                HttpContext.Current.Session["ListInisiasiInputProperty"] = ListInisiasiInputProperty;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListInisiasiInputProperty"] = null;
            return true;
        }

        public InisiasiInputProperty Get1Record(Guid menuId)
        {
            var model = ListInisiasiInputProperty.FirstOrDefault(o => o.MenuId == menuId);
            return model;
        }

        public List<InisiasiInputProperty> GetAllRecord()
        {
            var model = ListInisiasiInputProperty;
            return model;
        }
    }
}