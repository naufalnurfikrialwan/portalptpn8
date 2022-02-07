using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAktivaMaster
    {
        public static CRUDAktivaMaster CRUD = new CRUDAktivaMaster();

        public List<AktivaMaster> ListAktivaMaster
        {
            get
            {
                List<AktivaMaster> result = (List<AktivaMaster>)HttpContext.Current.Session["ListAktivaMaster"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAktivaMaster"] = result = Read();
                }
                return result;
            }
        }
        public List<AktivaMaster> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AktivaMaster.ToList();   
                return model;
            }
            catch
            {
                return new List<AktivaMaster>();
            }
        }

        public List<AktivaMaster> GetAllRecord()
        {
            var model = ListAktivaMaster;
            return model;
        }

    }
}