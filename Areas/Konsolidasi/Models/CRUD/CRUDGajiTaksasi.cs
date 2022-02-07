using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiTaksasi
    {
        public static CRUDGajiTaksasi CRUD = new CRUDGajiTaksasi();

        public List<GajiTaksasi> ListGajiTaksasi
        {
            get
            {
                List<GajiTaksasi> result = (List<GajiTaksasi>)HttpContext.Current.Session["ListGajiTaksasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiTaksasi"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiTaksasi> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiTaksasi.ToList();
                return model;
            }
            catch
            {
                return new List<GajiTaksasi>();
            }
        }

        public List<GajiTaksasi> GetAllRecord()
        {
            var model = ListGajiTaksasi;
            return model;
        }

    }
}