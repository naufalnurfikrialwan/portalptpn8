using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDOrganisasi
    {
        public static CRUDOrganisasi CRUD = new CRUDOrganisasi();

        public List<Organisasi> ListOrganisasi
        {
            get {
                List<Organisasi> result = (List<Organisasi>) HttpContext.Current.Session["ListOrganisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListOrganisasi"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Organisasi organisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.Organisasi.Add(organisasi);
                db.SaveChanges();

                ListOrganisasi.Add(organisasi);
                HttpContext.Current.Session["ListOrganisasi"] = ListOrganisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<Organisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Organisasi.ToList();
                foreach (var m in model)
                {
                    m.FileLogo = m.OrganisasiId.ToString() + ".jpg";
                    m.Negara = CRUDNegara.CRUD.Get1Record(m.NegaraId);
                    m.Propinsi = CRUDPropinsi.CRUD.Get1Record(m.PropinsiId);
                    m.Kota = CRUDKota.CRUD.Get1Record(m.KotaId);
                    m.Kecamatan = CRUDKecamatan.CRUD.Get1Record(m.KecamatanId);
                    m.Kelurahan = CRUDKelurahan.CRUD.Get1Record(m.KelurahanId);
                }

                return model;
            }
            catch
            {
                return new List<Organisasi>();
            }
        }
        public bool Update(Organisasi organisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Organisasi.FirstOrDefault(o => o.OrganisasiId == organisasi.OrganisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(organisasi);
                }
                else
                {
                    model.InjectFrom(organisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListOrganisasi.FirstOrDefault(o => o.OrganisasiId == organisasi.OrganisasiId);
                    context.InjectFrom(organisasi);
                    HttpContext.Current.Session["ListOrganisasi"] = ListOrganisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(Organisasi organisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.Organisasi.FirstOrDefault(o => o.OrganisasiId == organisasi.OrganisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(organisasi);
                CRUDKomisaris.CRUD.Erase(organisasi.OrganisasiId);
                CRUDDireksi.CRUD.Erase(organisasi.OrganisasiId);
                CRUDEmailOrganisasi.CRUD.Erase(organisasi.OrganisasiId);
                CRUDFaksimiliOrganisasi.CRUD.Erase(organisasi.OrganisasiId);
                CRUDNPWPOrganisasi.CRUD.Erase(organisasi.OrganisasiId);
                CRUDPortalOrganisasi.CRUD.Erase(organisasi.OrganisasiId);
                CRUDTeleponOrganisasi.CRUD.Erase(organisasi.OrganisasiId);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(Organisasi organisasi)
        {
            try
            {
                var context = ListOrganisasi.FirstOrDefault(o => o.OrganisasiId == organisasi.OrganisasiId);
                ListOrganisasi.Remove(context);
                HttpContext.Current.Session["ListOrganisasi"] = ListOrganisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Organisasi Get1Record(Guid organisasiId)
        {
            var model = ListOrganisasi.FirstOrDefault(o => o.OrganisasiId == organisasiId);
            return model;
        }

        public List<Organisasi> GetnRecord(Guid organisasiId)
        {
            var model = ListOrganisasi.Where(o => o.OrganisasiId == organisasiId).ToList();
            return model;
        }

        public List<Organisasi> GetAllRecord()
        {
            var model = ListOrganisasi;
            return model;
        }
    }
}