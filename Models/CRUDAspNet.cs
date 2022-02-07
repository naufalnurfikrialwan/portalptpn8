using Omu.ValueInjecter;
using Ptpn8.UserManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models
{
    public class CRUDApplicationUser
    {
        public static CRUDApplicationUser CRUD = new CRUDApplicationUser();

        public List<ApplicationUser> ListApplicationUser
        {
            get
            {
                List<ApplicationUser> result = (List<ApplicationUser>)HttpContext.Current.Application["AspNetUsers"];
                if (result == null)
                {
                    HttpContext.Current.Application["AspNetUsers"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(ApplicationUser applicationUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();

                ListApplicationUser.Add(applicationUser);
                HttpContext.Current.Application["AspNetUsers"] = ListApplicationUser;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<ApplicationUser> Read()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var model = db.Users.ToList(); //.Include("ApplicationUserBudidaya").ToList();
                foreach(var m in model)
                {
                    m.FileFoto = m.UserName + ".jpg";
                }
                return model;
            }
            catch
            {
                return new List<ApplicationUser>();
            }
        }
        public bool Update(ApplicationUser applicationUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var model = db.Users.FirstOrDefault(o => o.UserName == applicationUser.UserName);
                if (model == null)
                {
                    // harus create record baru
                    Create(applicationUser);
                }
                else
                {
                    model.InjectFrom(applicationUser);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListApplicationUser.FirstOrDefault(o => o.UserName == applicationUser.UserName);
                    context.InjectFrom(applicationUser);
                    HttpContext.Current.Application["AspNetUsers"] = ListApplicationUser;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(ApplicationUser applicationUser)
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var model = db.Users.FirstOrDefault(o => o.UserName == applicationUser.UserName);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(applicationUser);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(ApplicationUser applicationUser)
        {
            try
            {
                var context = ListApplicationUser.FirstOrDefault(o => o.UserName == applicationUser.UserName);
                ListApplicationUser.Remove(context);
                HttpContext.Current.Application["AspNetUsers"] = ListApplicationUser;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Application["AspNetUsers"] = null;
            return true;
        }

        public ApplicationUser Get1Record(string userName)
        {
            var model = ListApplicationUser.FirstOrDefault(o => o.UserName == userName);
            return model;
        }

        public List<ApplicationUser> GetAllRecord()
        {
            var model = ListApplicationUser;
            return model;
        }

        internal object Get1Record(object name)
        {
            throw new NotImplementedException();
        }
    }
    public class CRUDApplicationRole
    {
        public static CRUDApplicationRole CRUD = new CRUDApplicationRole();

        public List<ApplicationRole> ListApplicationRole
        {
            get
            {
                List<ApplicationRole> result = (List<ApplicationRole>)HttpContext.Current.Application["AspNetRoles"];
                if (result == null)
                {
                    HttpContext.Current.Application["AspNetRoles"] = result = Read();
                }
                return result;
            }
        }

        public List<ApplicationRole> Read()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            try
            {
                var model = db.ApplicationRole.ToList(); 
                return model;
            }
            catch
            {
                return new List<ApplicationRole>();
            }
        }

        public ApplicationRole Get1Record(string roleId)
        {
            var model = ListApplicationRole.FirstOrDefault(o => o.Id == roleId);
            return model;
        }

        public List<ApplicationRole> GetAllRecord()
        {
            var model = ListApplicationRole;
            return model;
        }


    }
}