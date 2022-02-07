using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDNegara
    {
        public static CRUDNegara CRUD = new CRUDNegara();

        public List<Negara> ListNegara
        {
            get
            {
                List<Negara> result = (List<Negara>)HttpContext.Current.Session["ListNegara"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListNegara"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Negara negara)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Negara.Add(negara);
                db.SaveChanges();
                ListNegara.Add(negara);
                HttpContext.Current.Session["ListNegara"] = ListNegara;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Negara> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Negara.ToList();
                foreach (var m in model)
                {
                    m.FileFoto = m.NegaraId.ToString() + ".jpg";
                }
                return model;
            }
            catch
            {
                return new List<Negara>();
            }
        }
        public bool Update(Negara negara)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Negara.FirstOrDefault(o => o.NegaraId == negara.NegaraId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(negara);
                }
                else
                {
                    model.InjectFrom(negara);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListNegara.FirstOrDefault(o => o.NegaraId == negara.NegaraId);
                    context.InjectFrom(negara);
                    HttpContext.Current.Session["ListNegara"] = ListNegara;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Negara negara)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Negara.FirstOrDefault(o => o.NegaraId == negara.NegaraId);
                model.InjectFrom(negara);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(negara);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Negara negara)
        {
            try
            {
                var context = ListNegara.FirstOrDefault(o => o.NegaraId == negara.NegaraId);
                ListNegara.Remove(context);
                HttpContext.Current.Session["ListNegara"] = ListNegara;
                CRUDPropinsi.CRUD.Erase(negara.NegaraId);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public Negara Get1Record(Guid negaraId)
        {
            var model = ListNegara.FirstOrDefault(o => o.NegaraId == negaraId);
            return model;
        }

        public List<Negara> GetnRecord(Guid negaraId)
        {
            var model = ListNegara.Where(o => o.NegaraId == negaraId).ToList();
            return model;
        }

        public List<Negara> GetAllRecord()
        {
            var model = ListNegara;
            return model;
        }
    }
}