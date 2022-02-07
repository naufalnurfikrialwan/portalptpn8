using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDKondisiCRUD
    {
        public static CRUDKondisiCRUD CRUD = new CRUDKondisiCRUD();

        public List<KondisiCRUD> ListKondisiCRUD
        {
            get
            {
                List<KondisiCRUD> result = (List<KondisiCRUD>)HttpContext.Current.Session["ListKondisiCRUD"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKondisiCRUD"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(KondisiCRUD kondisiCRUD)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                db.KondisiCRUD.Add(kondisiCRUD);
                db.SaveChanges();

                ListKondisiCRUD.Add(kondisiCRUD);
                HttpContext.Current.Session["ListKondisiCRUD"] = ListKondisiCRUD;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<KondisiCRUD> Read()
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.KondisiCRUD.ToList();
                return model;
            }
            catch
            {
                return new List<KondisiCRUD>();
            }
        }

        public bool Update(KondisiCRUD kondisiCRUD)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.KondisiCRUD.FirstOrDefault(o => o.KondisiCRUDId == kondisiCRUD.KondisiCRUDId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(kondisiCRUD);
                }
                else
                {
                    model.InjectFrom(kondisiCRUD);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListKondisiCRUD.FirstOrDefault(o => o.KondisiCRUDId == kondisiCRUD.KondisiCRUDId);
                    context.InjectFrom(kondisiCRUD);
                    HttpContext.Current.Session["ListKondisiCRUD"] = ListKondisiCRUD;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(KondisiCRUD kondisiCRUD)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.KondisiCRUD.FirstOrDefault(o => o.KondisiCRUDId == kondisiCRUD.KondisiCRUDId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(kondisiCRUD);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(KondisiCRUD kondisiCRUD)
        {
            try
            {
                var context = ListKondisiCRUD.FirstOrDefault(o => o.KondisiCRUDId == kondisiCRUD.KondisiCRUDId);
                ListKondisiCRUD.Remove(context);
                HttpContext.Current.Session["ListKondisiCRUD"] = ListKondisiCRUD;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListKondisiCRUD"] = null;
            return true;
        }

        public KondisiCRUD Get1Record(Guid menuId)
        {
            var model = ListKondisiCRUD.FirstOrDefault(o => o.MenuId == menuId);
            return model;
        }

        public List<KondisiCRUD> GetAllRecord()
        {
            var model = ListKondisiCRUD;
            return model;
        }
    }
}