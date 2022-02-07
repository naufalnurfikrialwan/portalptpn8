using Omu.ValueInjecter;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDProject
    {
        public static CRUDProject CRUD = new CRUDProject();

        public List<Project> ListProject
        {
            get
            {
                List<Project> result = (List<Project>)HttpContext.Current.Session["ListProject"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListProject"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Project project)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.Project.Add(project);
                db.SaveChanges();

                ListProject.Add(project);
                HttpContext.Current.Session["ListProject"] = ListProject;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Project> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.Project.ToList();
                foreach (var m in model)
                {
                    m.Bagian = CRUDBagian.CRUD.Get1Record(m.BagianId);
                }

                return model;
            }
            catch
            {
                return new List<Project>();
            }
        }
        public bool Update(Project project)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.Project.FirstOrDefault(o => o.ProjectId == project.ProjectId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(project);
                }
                else
                {
                    model.InjectFrom(project);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListProject.FirstOrDefault(o => o.ProjectId == project.ProjectId);
                    context.InjectFrom(project);
                    HttpContext.Current.Session["ListProject"] = ListProject;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Project project)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.Project.FirstOrDefault(o => o.ProjectId == project.ProjectId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(project);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Project project)
        {
            try
            {
                var context = ListProject.FirstOrDefault(o => o.ProjectId == project.ProjectId);
                ListProject.Remove(context);
                HttpContext.Current.Session["ListProject"] = ListProject;
                CRUDProjectProgress.CRUD.Erase(context.ProjectId);
                CRUDProjectBlok.CRUD.Erase(context.ProjectId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListProject"] = null;
            return true;
        }

        public Project Get1Record(Guid projectId)
        {
            var model = ListProject.FirstOrDefault(o => o.ProjectId == projectId);
            return model;
        }

        public List<Project> GetnRecord(Guid projectId)
        {
            var model = ListProject.Where(o => o.ProjectId == projectId).ToList();
            return model;
        }

        public List<Project> GetAllRecord()
        {
            var model = ListProject;
            return model;
        }

    }
}