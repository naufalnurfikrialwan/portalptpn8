using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_Dik
    {
        public static CRUDRef_Dik CRUD = new CRUDRef_Dik();

        public List<Ref_Dik> ListRef_Dik
        {
            get
            {
                List<Ref_Dik> result = (List<Ref_Dik>)HttpContext.Current.Session["ListRef_Dik"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_Dik"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_Dik> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_Dik.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_Dik>();
            }
        }

        public List<Ref_Dik> GetAllRecord()
        {
            var model = ListRef_Dik;
            return model;
        }

    }
}