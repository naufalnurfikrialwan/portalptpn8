using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiPremi
    {
        public static CRUDGajiPremi CRUD = new CRUDGajiPremi();

        public List<GajiPremi> ListGajiPremi
        {
            get
            {
                List<GajiPremi> result = (List<GajiPremi>)HttpContext.Current.Session["ListGajiPremi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiPremi"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiPremi> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiPremi.ToList();
                return model;
            }
            catch
            {
                return new List<GajiPremi>();
            }
        }

        public List<GajiPremi> GetAllRecord()
        {
            var model = ListGajiPremi;
            return model;
        }

    }
}