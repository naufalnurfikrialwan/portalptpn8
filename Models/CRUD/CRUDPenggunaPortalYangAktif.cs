using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDPenggunaPortalYangAktif
    {
        public static CRUDPenggunaPortalYangAktif CRUD = new CRUDPenggunaPortalYangAktif();

        public List<PenggunaPortalYangAktif> ListPenggunaPortalYangAktif
        {
            get
            {
                List<PenggunaPortalYangAktif> result = (List<PenggunaPortalYangAktif>)HttpContext.Current.Application["PenggunaPortalYangAktif"];
                if (result == null)
                {
                    HttpContext.Current.Application["PenggunaPortalYangAktif"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(PenggunaPortalYangAktif penggunaPortalYangAktif)
        {
            ListPenggunaPortalYangAktif.Add(penggunaPortalYangAktif);
            HttpContext.Current.Application["PenggunaPortalYangAktif"] = ListPenggunaPortalYangAktif;
            return true;
        }
        public List<PenggunaPortalYangAktif> Read()
        {
            return new List<PenggunaPortalYangAktif>();
        }

        public bool Update(PenggunaPortalYangAktif penggunaPortalYangAktif)
        {
            var model = ListPenggunaPortalYangAktif.FirstOrDefault(o => o.PenggunaPortalYangAktifId == penggunaPortalYangAktif.PenggunaPortalYangAktifId);
            if (model == null)
            {
                // harus create record baru
                return Create(penggunaPortalYangAktif);
            }
            else
            {
                var context = ListPenggunaPortalYangAktif.FirstOrDefault(o => o.PenggunaPortalYangAktifId == penggunaPortalYangAktif.PenggunaPortalYangAktifId);
                context.InjectFrom(penggunaPortalYangAktif);
                HttpContext.Current.Application["PenggunaPortalYangAktif"] = ListPenggunaPortalYangAktif;
            }
            return true;
        }

        public bool Delete(List<PenggunaPortalYangAktif> listPenggunaPortalYangAktif)
        {
            foreach(var model in listPenggunaPortalYangAktif)
            {
                Erase(model);
            }
            return true;
        }

        public bool Erase(PenggunaPortalYangAktif penggunaPortalYangAktif)
        {
            var context = ListPenggunaPortalYangAktif.FirstOrDefault(o => o.PenggunaPortalYangAktifId == penggunaPortalYangAktif.PenggunaPortalYangAktifId);
            ListPenggunaPortalYangAktif.Remove(context);
            HttpContext.Current.Application["PenggunaPortalYangAktif"] = ListPenggunaPortalYangAktif;
            return true;
        }

        public bool EraseAll()
        {
            HttpContext.Current.Application["PenggunaPortalYangAktif"] = null;
            return true;
        }

        public PenggunaPortalYangAktif Get1Record(Guid penggunaPortalYangAktifId)
        {
            var model = ListPenggunaPortalYangAktif.FirstOrDefault(o => o.PenggunaPortalYangAktifId == penggunaPortalYangAktifId);
            return model;
        }

        public PenggunaPortalYangAktif Get1Record(string userName)
        {
            var model = ListPenggunaPortalYangAktif.FirstOrDefault(o => o.UserName== userName);
            return model;
        }

        public List<PenggunaPortalYangAktif> GetnRecord(Guid penggunaPortalYangAktifId)
        {
            var model = ListPenggunaPortalYangAktif.Where(o => o.PenggunaPortalYangAktifId == penggunaPortalYangAktifId).ToList();
            return model;
        }

        public List<PenggunaPortalYangAktif> GetAllRecord()
        {
            var model = ListPenggunaPortalYangAktif;
            return model;
        }
    }
}