using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAktivaTransaksi
    {
        public static CRUDAktivaTransaksi CRUD = new CRUDAktivaTransaksi();

        public List<AktivaTransaksi> ListAktivaTransaksi
        {
            get
            {
                List<AktivaTransaksi> result = (List<AktivaTransaksi>)HttpContext.Current.Session["ListAktivaTransaksi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAktivaTransaksi"] = result = Read();
                }
                return result;
            }
        }
        public List<AktivaTransaksi> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AktivaTransaksi.ToList();
                return model;
            }
            catch
            {
                return new List<AktivaTransaksi>();
            }
        }

        public List<AktivaTransaksi> GetAllRecord()
        {
            var model = ListAktivaTransaksi;
            return model;
        }

    }
}