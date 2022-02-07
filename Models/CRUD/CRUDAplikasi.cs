using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDAplikasi
    {
        public static CRUDAplikasi CRUD = new CRUDAplikasi();

        public List<Aplikasi> ListAplikasi
        {
            get
            {
                List<Aplikasi> result = (List<Aplikasi>)HttpContext.Current.Session["ListAplikasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAplikasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Aplikasi aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                db.Aplikasi.Add(aplikasi);
                db.SaveChanges();

                ListAplikasi.Add(aplikasi);
                HttpContext.Current.Session["ListAplikasi"] = ListAplikasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Aplikasi> Read()
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Aplikasi.ToList();
                foreach (var m in model)
                {
                    m.Menu = CRUDMenu.CRUD.GetnRecord(m.AplikasiId);
                }

                return model;
            }
            catch
            {
                return new List<Aplikasi>();
            }
        }
        public bool Update(Aplikasi aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Aplikasi.FirstOrDefault(o => o.AplikasiId == aplikasi.AplikasiId);
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

                    var context = ListAplikasi.FirstOrDefault(o => o.AplikasiId == aplikasi.AplikasiId);
                    context.InjectFrom(aplikasi);
                    HttpContext.Current.Session["ListAplikasi"] = ListAplikasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Aplikasi aplikasi)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Aplikasi.FirstOrDefault(o => o.AplikasiId == aplikasi.AplikasiId);
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

        public bool Erase(Aplikasi aplikasi)
        {
            try
            {
                var context = ListAplikasi.FirstOrDefault(o => o.AplikasiId == aplikasi.AplikasiId);
                ListAplikasi.Remove(context);
                HttpContext.Current.Session["ListAplikasi"] = ListAplikasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListAplikasi"] = null;
            return true;
        }

        public Aplikasi Get1Record(Guid aplikasiId)
        {
            var model = ListAplikasi.FirstOrDefault(o => o.AplikasiId == aplikasiId);
            return model;
        }

        public List<Aplikasi> GetnRecord(Guid aplikasiId)
        {
            var model = ListAplikasi.Where(o => o.AplikasiId == aplikasiId).ToList();
            return model;
        }

        public List<Aplikasi> GetAllRecord()
        {
            var model = ListAplikasi;
            return model;
        }
    }
}