using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_SatuanHasil
    {
        public static CRUDRef_SatuanHasil CRUD = new CRUDRef_SatuanHasil();

        public List<Ref_SatuanHasil> ListRef_SatuanHasil
        {
            get
            {
                List<Ref_SatuanHasil> result = (List<Ref_SatuanHasil>)HttpContext.Current.Session["ListRef_SatuanHasil"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_SatuanHasil"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_SatuanHasil> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_SatuanHasil.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_SatuanHasil>();
            }
        }

        public List<Ref_SatuanHasil> GetAllRecord()
        {
            var model = ListRef_SatuanHasil;
            return model;
        }

    }
}