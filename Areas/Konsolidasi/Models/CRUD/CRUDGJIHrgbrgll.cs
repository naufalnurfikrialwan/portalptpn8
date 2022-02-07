using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGJIHrgbrgll
    {
        public static CRUDGJIHrgbrgll CRUD = new CRUDGJIHrgbrgll();

        public List<GJIHrgbrgll> ListGJIHrgbrgll
        {
            get
            {
                List<GJIHrgbrgll> result = (List<GJIHrgbrgll>)HttpContext.Current.Session["ListGJIHrgbrgll"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGJIHrgbrgll"] = result = Read();
                }
                return result;
            }
        }
        public List<GJIHrgbrgll> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GJIHrgbrgll.ToList();
                return model;
            }
            catch
            {
                return new List<GJIHrgbrgll>();
            }
        }

        public List<GJIHrgbrgll> GetAllRecord()
        {
            var model = ListGJIHrgbrgll;
            return model;
        }

    }
}