using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_DikKLM
    {
        public static CRUDRef_DikKLM CRUD = new CRUDRef_DikKLM();

        public List<Ref_DikKLM> ListRef_DikKLM
        {
            get
            {
                List<Ref_DikKLM> result = (List<Ref_DikKLM>)HttpContext.Current.Session["ListRef_DikKLM"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_DikKLM"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_DikKLM> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_DikKLM.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_DikKLM>();
            }
        }

        public List<Ref_DikKLM> GetAllRecord()
        {
            var model = ListRef_DikKLM;
            return model;
        }

    }
}