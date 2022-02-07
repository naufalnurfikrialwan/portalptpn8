using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_AsalProd
    {
        public static CRUDRef_AsalProd CRUD = new CRUDRef_AsalProd();

        public List<Ref_AsalProd> ListRef_AsalProd
        {
            get
            {
                List<Ref_AsalProd> result = (List<Ref_AsalProd>)HttpContext.Current.Session["ListRef_AsalProd"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_AsalProd"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_AsalProd> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_AsalProd.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_AsalProd>();
            }
        }

        public List<Ref_AsalProd> GetAllRecord()
        {
            var model = ListRef_AsalProd;
            return model;
        }

    }
}