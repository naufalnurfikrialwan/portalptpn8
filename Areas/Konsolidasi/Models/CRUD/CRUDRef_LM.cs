using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_LM
    {
        public static CRUDRef_LM CRUD = new CRUDRef_LM();

        public List<Ref_LM> ListRef_LM
        {
            get
            {
                List<Ref_LM> result = (List<Ref_LM>)HttpContext.Current.Session["ListRef_LM"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_LM"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_LM> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_LM.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_LM>();
            }
        }

        public List<Ref_LM> GetAllRecord()
        {
            var model = ListRef_LM;
            return model;
        }

    }
}