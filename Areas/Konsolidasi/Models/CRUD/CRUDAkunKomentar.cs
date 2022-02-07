using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAkunKomentar
    {
        public static CRUDAkunKomentar CRUD = new CRUDAkunKomentar();

        public List<AkunKomentar> ListAkunKomentar
        {
            get
            {
                List<AkunKomentar> result = (List<AkunKomentar>)HttpContext.Current.Session["ListAkunKomentar"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAkunKomentar"] = result = Read();
                }
                return result;
            }
        }
        public List<AkunKomentar> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AkunKomentar.ToList();
                return model;
            }
            catch
            {
                return new List<AkunKomentar>();
            }
        }

        public List<AkunKomentar> GetAllRecord()
        {
            var model = ListAkunKomentar;
            return model;
        }

    }
}