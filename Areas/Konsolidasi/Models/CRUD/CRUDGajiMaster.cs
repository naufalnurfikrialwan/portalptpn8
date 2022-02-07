using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiMaster
    {
        public static CRUDGajiMaster CRUD = new CRUDGajiMaster();

        public List<GajiMaster> ListGajiMaster
        {
            get
            {
                List<GajiMaster> result = (List<GajiMaster>)HttpContext.Current.Session["ListGajiMaster"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiMaster"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiMaster> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiMaster.ToList();
                return model;
            }
            catch
            {
                return new List<GajiMaster>();
            }
        }

        public List<GajiMaster> GetAllRecord()
        {
            var model = ListGajiMaster;
            return model;
        }

    }
}