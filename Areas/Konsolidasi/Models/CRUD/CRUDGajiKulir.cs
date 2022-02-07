using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiKulir
    {
        public static CRUDGajiKulir CRUD = new CRUDGajiKulir();

        public List<GajiKulir> ListGajiKulir
        {
            get
            {
                List<GajiKulir> result = (List<GajiKulir>)HttpContext.Current.Session["ListGajiKulir"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiKulir"] = result = Read();
                }
                return result;
            }
        }
        public List<GajiKulir> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.GajiKulir.ToList();
                return model;
            }
            catch
            {
                return new List<GajiKulir>();
            }
        }

        public List<GajiKulir> GetAllRecord()
        {
            var model = ListGajiKulir;
            return model;
        }

    }
}