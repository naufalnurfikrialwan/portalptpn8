using Kendo.Mvc.UI;
using Omu.ValueInjecter;
using Ptpn8.Areas.Areal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDMenu
    {
        public static CRUDMenu CRUD = new CRUDMenu();

        public List<Menu> ListMenu
        {
            get
            {
                List<Menu> result = (List<Menu>)HttpContext.Current.Session["ListMenu"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListMenu"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Menu menu)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                db.Menu.Add(menu);
                db.SaveChanges();

                ListMenu.Add(menu);
                HttpContext.Current.Session["ListMenu"] = ListMenu;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Menu> Read()
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Menu.ToList();
                return model;
            }
            catch
            {
                return new List<Menu>();
            }
        }

        

        public bool Update(Menu menu)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Menu.FirstOrDefault(o => o.MenuId == menu.MenuId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(menu);
                }
                else
                {
                    model.InjectFrom(menu);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListMenu.FirstOrDefault(o => o.MenuId == menu.MenuId);
                    context.InjectFrom(menu);
                    HttpContext.Current.Session["ListMenu"] = ListMenu;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Menu menu)
        {
            Ptpn8dbContext db = new Ptpn8dbContext();
            try
            {
                var model = db.Menu.FirstOrDefault(o => o.MenuId == menu.MenuId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(menu);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Menu menu)
        {
            try
            {
                var context = ListMenu.FirstOrDefault(o => o.MenuId == menu.MenuId);
                ListMenu.Remove(context);
                HttpContext.Current.Session["ListMenu"] = ListMenu;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListMenu"] = null;
            return true;
        }

        public Menu Get1Record(Guid menuId)
        {
            var model = ListMenu.FirstOrDefault(o => o.MenuId == menuId);
            return model;
        }

        public List<Menu> GetnRecord(Guid menuId)
        {
            var model = ListMenu.Where(o => o.MenuId == menuId).ToList();
            return model;
        }

        public List<Menu> GetAllRecord()
        {
            var model = ListMenu;
            return model;
        }

        public List<Menu> BuildMenu(string namaAplikasi)
        {
            List<Menu> result = new List<Menu>();

            result = (from m in CRUDMenu.CRUD.GetAllRecord()
                      join n in CRUDAplikasi.CRUD.GetAllRecord() on m.AplikasiId equals n.AplikasiId
                      where n.NamaAplikasi == namaAplikasi
                      select m).OrderBy(o => o.KodeMenu).ToList();
            return result;
        }

        public List<Menu> BuildMenu(Guid menuId)
        {
            List<Menu> result = new List<Menu>();
            result = (from m in CRUDMenu.CRUD.GetAllRecord()
                      join n in CRUDAplikasi.CRUD.GetAllRecord() on m.AplikasiId equals n.AplikasiId
                      where n.AplikasiId == menuId
                      select m).OrderBy(o => o.KodeMenu).ToList();
            return result;
        }

    }
}