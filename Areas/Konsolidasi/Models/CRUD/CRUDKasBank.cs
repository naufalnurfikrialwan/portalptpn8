using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDKasBank
    {
        public static CRUDKasBank CRUD = new CRUDKasBank();

        public List<KasBank> ListKasBank
        {
            get
            {
                List<KasBank> result = (List<KasBank>)HttpContext.Current.Session["ListKasBank"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListKasBank"] = result = Read();
                }
                return result;
            }
        }
        public List<KasBank> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.KasBank.ToList();
                return model;
            }
            catch
            {
                return new List<KasBank>();
            }
        }

        public List<KasBank> GetAllRecord()
        {
            var model = ListKasBank;
            return model;
        }

    }
}