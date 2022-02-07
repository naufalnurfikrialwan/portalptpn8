using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDEap
    {
        public static CRUDEap CRUD = new CRUDEap();

        public List<Eap> ListEap
        {
            get
            {
                List<Eap> result = (List<Eap>)HttpContext.Current.Session["ListEap"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListEap"] = result = Read();
                }
                return result;
            }
        }
        public List<Eap> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Eap.ToList();
                return model;
            }
            catch
            {
                return new List<Eap>();
            }
        }

        public List<Eap> GetAllRecord()
        {
            var model = ListEap;
            return model;
        }

    }
}