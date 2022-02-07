using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDProjectProgress
    {
        public static CRUDProjectProgress CRUD = new CRUDProjectProgress();

        public List<ProjectProgress> ListProjectProgress
        {
            get
            {
                List<ProjectProgress> result = (List<ProjectProgress>)HttpContext.Current.Session["ListProjectProgress"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListProjectProgress"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(ProjectProgress projectProgress)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                db.ProjectProgress.Add(projectProgress);
                db.SaveChanges();

                ListProjectProgress.Add(projectProgress);
                HttpContext.Current.Session["ListProjectProgress"] = ListProjectProgress;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<ProjectProgress> Read()
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectProgress.ToList();
                return model;
            }
            catch
            {
                return new List<ProjectProgress>();
            }
        }
        public bool Update(ProjectProgress projectProgress)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectProgress.FirstOrDefault(o => o.ProjectProgressId == projectProgress.ProjectProgressId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(projectProgress);
                }
                else
                {
                    model.InjectFrom(projectProgress);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListProjectProgress.FirstOrDefault(o => o.ProjectProgressId == projectProgress.ProjectProgressId);
                    context.InjectFrom(projectProgress);
                    HttpContext.Current.Session["ListProjectProgress"] = ListProjectProgress;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(ProjectProgress projectProgress)
        {
            ArealdbContext db = new ArealdbContext();
            try
            {
                var model = db.ProjectProgress.FirstOrDefault(o => o.ProjectProgressId == projectProgress.ProjectProgressId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(projectProgress);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(ProjectProgress projectProgress)
        {
            try
            {
                var context = ListProjectProgress.FirstOrDefault(o => o.ProjectProgressId == projectProgress.ProjectProgressId);
                ListProjectProgress.Remove(context);
                HttpContext.Current.Session["ListProjectProgress"] = ListProjectProgress;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid projectProgressId)
        {
            try
            {
                var context = ListProjectProgress.Where(o => o.ProjectProgressId == projectProgressId);
                foreach(var m in context)
                {
                    ListProjectProgress.Remove(m);
                    HttpContext.Current.Session["ListProjectProgress"] = ListProjectProgress;
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
            HttpContext.Current.Session["ListProjectProgress"] = null;
            return true;
        }

        public ProjectProgress Get1Record(Guid projectProgressId)
        {
            var model = ListProjectProgress.FirstOrDefault(o => o.ProjectProgressId == projectProgressId);
            return model;
        }

        public List<ProjectProgress> GetnRecord(Guid projectProgressId)
        {
            var model = ListProjectProgress.Where(o => o.ProjectProgressId == projectProgressId).ToList();
            return model;
        }

        public List<ProjectProgress> GetAllRecord()
        {
            var model = ListProjectProgress;
            return model;
        }

    }
}