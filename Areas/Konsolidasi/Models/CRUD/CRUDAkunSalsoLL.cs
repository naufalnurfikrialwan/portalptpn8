using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAkunSaldoLL
    {
        public static CRUDAkunSaldoLL CRUD = new CRUDAkunSaldoLL();

        

        public List<AkunSaldoLL> ListAkunSaldoLL
        {
            get
            {
                List<AkunSaldoLL> result = (List<AkunSaldoLL>)HttpContext.Current.Session["ListAkunSaldoLL"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAkunSaldoLL"] = result = Read();
                }
                return result;
            }
        }

        
        public List<AkunSaldoLL> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AkunSaldoLL.ToList();
                return model;
            }
            catch
            {
                return new List<AkunSaldoLL>();
            }
        }

        public List<AkunSaldoLL> GetAllRecord()
        {
            var model = ListAkunSaldoLL;
            return model;
        }

    }
}