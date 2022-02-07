using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAktivaSaldo
    {
        public static CRUDAktivaSaldo CRUD = new CRUDAktivaSaldo();

        public List<AktivaSaldo> ListAktivaSaldo
        {
            get
            {
                List<AktivaSaldo> result = (List<AktivaSaldo>)HttpContext.Current.Session["ListAktivaSaldo"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAktivaSaldo"] = result = Read();
                }
                return result;
            }
        }
        public List<AktivaSaldo> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AktivaSaldo.ToList();
                return model;
            }
            catch
            {
                return new List<AktivaSaldo>();
            }
        }

        public List<AktivaSaldo> GetAllRecord()
        {
            var model = ListAktivaSaldo;
            return model;
        }

    }
}