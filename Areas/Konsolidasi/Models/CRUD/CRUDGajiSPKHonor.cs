using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiSPKHonor
    {
        public static CRUDGajiSPKHonor CRUD = new CRUDGajiSPKHonor();

        public List<GajiSPKHonor> ListGajiSPKHonor
        {
            get
            {
                List<GajiSPKHonor> result = (List<GajiSPKHonor>)HttpContext.Current.Session["ListGajiSPKHonor"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiSPKHonor"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiSPKHonor> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiSPKHonor.ToList();
                return model;
            }
            catch
            {
                return new List<GajiSPKHonor>();
            }
        }

        public List<GajiSPKHonor> GetAllRecord()
        {
            var model = ListGajiSPKHonor;
            return model;
        }

    }
}