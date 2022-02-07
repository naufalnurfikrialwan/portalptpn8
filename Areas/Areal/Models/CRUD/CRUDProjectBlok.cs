using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDProjectBlok
    {
        public static CRUDProjectBlok CRUD = new CRUDProjectBlok();

        public List<ProjectBlok> ListProjectBlok
        {
            get
            {
                List<ProjectBlok> result = (List<ProjectBlok>)HttpContext.Current.Session["ListProjectBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListProjectBlok"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(ProjectBlok projectBlok)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.ProjectBlok.Add(projectBlok);
                db.SaveChanges();

                ListProjectBlok.Add(projectBlok);
                HttpContext.Current.Session["ListProjectBlok"] = ListProjectBlok;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<ProjectBlok> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectBlok.ToList();
                return model;
            }
            catch
            {
                return new List<ProjectBlok>();
            }
        }
        public bool Update(ProjectBlok projectBlok)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectBlok.FirstOrDefault(o => o.ProjectBlokId == projectBlok.ProjectBlokId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(projectBlok);
                }
                else
                {
                    model.InjectFrom(projectBlok);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListProjectBlok.FirstOrDefault(o => o.ProjectBlokId == projectBlok.ProjectBlokId);
                    context.InjectFrom(projectBlok);
                    HttpContext.Current.Session["ListProjectBlok"] = ListProjectBlok;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(ProjectBlok projectBlok)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectBlok.FirstOrDefault(o => o.ProjectBlokId == projectBlok.ProjectBlokId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(projectBlok);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(ProjectBlok projectBlok)
        {
            try
            {
                var context = ListProjectBlok.FirstOrDefault(o => o.ProjectBlokId == projectBlok.ProjectBlokId);
                ListProjectBlok.Remove(context);
                HttpContext.Current.Session["ListProjectBlok"] = ListProjectBlok;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid projectBlokId)
        {
            try
            {
                var context = ListProjectBlok.Where(o => o.ProjectBlokId == projectBlokId);
                foreach (var m in context)
                {
                    ListProjectBlok.Remove(m);
                    HttpContext.Current.Session["ListProjectBlok"] = ListProjectBlok;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public bool EraseAll()
        {
            HttpContext.Current.Session["ListProjectBlok"] = null;
            return true;
        }

        public ProjectBlok Get1Record(Guid projectBlokId)
        {
            var model = ListProjectBlok.FirstOrDefault(o => o.ProjectBlokId == projectBlokId);
            return model;
        }

        public List<ProjectBlok> GetnRecord(Guid projectBlokId)
        {
            var model = ListProjectBlok.Where(o => o.ProjectBlokId == projectBlokId).ToList();
            return model;
        }

        public List<ProjectBlok> GetAllRecord()
        {
            var model = ListProjectBlok;
            return model;
        }

    }
}