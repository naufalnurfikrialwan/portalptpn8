using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiJurnal
    {
        public static CRUDGajiJurnal CRUD = new CRUDGajiJurnal();

        public List<GajiJurnal> ListGajiJurnal
        {
            get
            {
                List<GajiJurnal> result = (List<GajiJurnal>)HttpContext.Current.Session["ListGajiJurnal"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiJurnal"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiJurnal> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiJurnal.ToList();
                return model;
            }
            catch
            {
                return new List<GajiJurnal>();
            }
        }

        public List<GajiJurnal> GetAllRecord()
        {
            var model = ListGajiJurnal;
            return model;
        }

    }
}