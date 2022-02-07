using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_Basic
    {
        public static CRUDRef_Basic CRUD = new CRUDRef_Basic();

        public List<Ref_Basic> ListRef_Basic
        {
            get
            {
                List<Ref_Basic> result = (List<Ref_Basic>)HttpContext.Current.Session["ListRef_Basic"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_Basic"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_Basic> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_Basic.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_Basic>();
            }
        }

        public List<Ref_Basic> GetAllRecord()
        {
            var model = ListRef_Basic;
            return model;
        }

    }
}