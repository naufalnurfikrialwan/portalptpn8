using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiPremiPanen
    {
        public static CRUDGajiPremiPanen CRUD = new CRUDGajiPremiPanen();

        public List<GajiPremiPanen> ListGajiPremiPanen
        {
            get
            {
                List<GajiPremiPanen> result = (List<GajiPremiPanen>)HttpContext.Current.Session["ListGajiPremiPanen"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiPremiPanen"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiPremiPanen> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiPremiPanen.ToList();
                return model;
            }
            catch
            {
                return new List<GajiPremiPanen>();
            }
        }

        public List<GajiPremiPanen> GetAllRecord()
        {
            var model = ListGajiPremiPanen;
            return model;
        }

    }
}