using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiPotongan
    {
        public static CRUDGajiPotongan CRUD = new CRUDGajiPotongan();

        public List<GajiPotongan> ListGajiPotongan
        {
            get
            {
                List<GajiPotongan> result = (List<GajiPotongan>)HttpContext.Current.Session["ListGajiPotongan"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiPotongan"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiPotongan> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiPotongan.ToList();
                return model;
            }
            catch
            {
                return new List<GajiPotongan>();
            }
        }

        public List<GajiPotongan> GetAllRecord()
        {
            var model = ListGajiPotongan;
            return model;
        }

    }
}