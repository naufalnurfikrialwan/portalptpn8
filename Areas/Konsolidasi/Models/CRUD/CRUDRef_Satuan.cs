using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_Satuan
    {
        public static CRUDRef_Satuan CRUD = new CRUDRef_Satuan();

        public List<Ref_Satuan> ListRef_Satuan
        {
            get
            {
                List<Ref_Satuan> result = (List<Ref_Satuan>)HttpContext.Current.Session["ListRef_Satuan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_Satuan"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_Satuan> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_Satuan.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_Satuan>();
            }
        }

        public List<Ref_Satuan> GetAllRecord()
        {
            var model = ListRef_Satuan;
            return model;
        }

    }
}