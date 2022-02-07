using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_Barang
    {
        public static CRUDRef_Barang CRUD = new CRUDRef_Barang();

        public List<Ref_Barang> ListRef_Barang
        {
            get
            {
                List<Ref_Barang> result = (List<Ref_Barang>)HttpContext.Current.Session["ListRef_Barang"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_Barang"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_Barang> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_Barang.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_Barang>();
            }
        }

        public List<Ref_Barang> GetAllRecord()
        {
            var model = ListRef_Barang;
            return model;
        }

    }
}