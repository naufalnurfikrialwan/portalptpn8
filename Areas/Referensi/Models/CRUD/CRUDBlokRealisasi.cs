using Omu.ValueInjecter;
using Ptpn8.Areas.Areal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBlokRealisasi
    {
        public static CRUDBlokRealisasi CRUD = new CRUDBlokRealisasi();

        private int _TahunBuku;
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set {
                if(value!=_TahunBuku)
                {
                    HttpContext.Current.Session["ListBlokRealisasi"] = null;
                    HttpContext.Current.Session["ListLuasArealperKebun"]=null;
                    HttpContext.Current.Session["ListLuasArealperAfdeling"]=null;
                }
                this._TahunBuku = value;
            }
        }

        public class LuasArealperKebun
        {
            public Guid KebunId { get; set; }
            public string KodeKebun { get; set; }
            public Guid MainBudidayaId { get; set; }
            public string KodeBudidaya { get; set; }
            public string StatusAreal { get; set; }
            public decimal LuasAreal { get; set; }
        }

        public class LuasArealperAfdeling
        {
            public Guid AfdelingId { get; set; }
            public string KodeAfdeling { get; set; }
            public Guid KebunId { get; set; }
            public Guid MainBudidayaId { get; set; }
            public string KodeBudidaya { get; set; }
            public string StatusAreal { get; set; }
            public decimal LuasAreal { get; set; }
        }


        public List<BlokRealisasi> ListBlokRealisasi
        {
            get
            {
                List<BlokRealisasi> result = (List<BlokRealisasi>)HttpContext.Current.Session["ListBlokRealisasi"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBlokRealisasi"] = result = Read();
                }
                return result;
            }
        }

        public List<LuasArealperKebun> ListLuasArealperKebun
        {
            get
            {
                List<LuasArealperKebun> result = (List<LuasArealperKebun>)HttpContext.Current.Session["ListLuasArealperKebun"];
                if(result == null)
                {
                    HttpContext.Current.Session["ListLuasArealperKebun"] = result = HitungLuasArealperKebun();
                }
                return result;
            }
        }

        public List<LuasArealperAfdeling> ListLuasArealperAfdeling
        {
            get
            {
                List<LuasArealperAfdeling> result = (List<LuasArealperAfdeling>)HttpContext.Current.Session["ListLuasArealperAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListLuasArealperAfdeling"] = result = HitungLuasArealperAfdeling();
                }
                return result;
            }
        }

        public bool Create(BlokRealisasi blokRealisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BlokRealisasi.Add(blokRealisasi);
                db.SaveChanges();

                ListBlokRealisasi.Add(blokRealisasi);
                HttpContext.Current.Session["ListBlokRealisasi"] = ListBlokRealisasi;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BlokRealisasi> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRealisasi.Where(o=>o.TahunBuku==TahunBuku.ToString())
                        //.Where(o=>o.MainBudidayaId.ToString().ToUpper()== "3D662350-146F-48B0-A181-5A840330C451" || o.MainBudidayaId.ToString().ToUpper() == "01EB6E4D-31D2-4ECF-A143-D109A71A3EC8" || o.MainBudidayaId.ToString().ToUpper() == "55C355B8-E57A-48B5-A2B0-1E71B4CCF5AB")
                        .ToList();
                foreach(var m in model)
                {
                    m.Blok = CRUDBlok.CRUD.Get1Record(m.BlokId);
                    m.StatusAreal = CRUDStatusAreal.CRUD.Get1Record(m.StatusArealId).Nama;
                    m.HGU = CRUDHGU.CRUD.Get1Record(m.HGUId);
                }
                return model;
            }
            catch
            {
                return new List<BlokRealisasi>();
            }
        }
        public bool Update(BlokRealisasi blokRealisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRealisasi.FirstOrDefault(o => o.BlokRealisasiId == blokRealisasi.BlokRealisasiId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(blokRealisasi);
                }
                else
                {
                    model.InjectFrom(blokRealisasi);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBlokRealisasi.FirstOrDefault(o => o.BlokRealisasiId == blokRealisasi.BlokRealisasiId);
                    context.InjectFrom(blokRealisasi);
                    HttpContext.Current.Session["ListBlokRealisasi"] = ListBlokRealisasi;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BlokRealisasi blokRealisasi)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BlokRealisasi.FirstOrDefault(o => o.BlokRealisasiId == blokRealisasi.BlokRealisasiId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(blokRealisasi);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BlokRealisasi blokRealisasi)
        {
            try
            {
                var context = ListBlokRealisasi.FirstOrDefault(o => o.BlokRealisasiId == blokRealisasi.BlokRealisasiId);
                ListBlokRealisasi.Remove(context);
                HttpContext.Current.Session["ListBlokRealisasi"] = ListBlokRealisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid blokId)
        {
            try
            {
                var context = ListBlokRealisasi.Where(o => o.BlokId == blokId).ToList();
                foreach (var m in context)
                {
                    ListBlokRealisasi.Remove(m);
                }
                HttpContext.Current.Session["ListBlokRealisasi"] = ListBlokRealisasi;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public BlokRealisasi Get1Record(Guid blokRealisasiId)
        {
            var model = ListBlokRealisasi.FirstOrDefault(o => o.BlokRealisasiId == blokRealisasiId);
            return model;
        }

        public BlokRealisasi Get1Record(string kodeBlok)
        {
            var model = ListBlokRealisasi.FirstOrDefault(o => o.Blok.kd_blok == kodeBlok);
            return model;
        }
        public BlokRealisasi Get1Record(string kodeBlok, string statusAreal)
        {
            var model = ListBlokRealisasi.FirstOrDefault(o => o.Blok.kd_blok == kodeBlok && o.StatusAreal==statusAreal);
            return model;
        }

        public List<BlokRealisasi> GetnRecord(Guid blokRealisasiId)
        {
            var model = ListBlokRealisasi.Where(o => o.BlokRealisasiId == blokRealisasiId).ToList();
            return model;
        }

        public List<LuasArealperKebun> HitungLuasArealperKebun()
        {
            var model = from m in ListBlokRealisasi group m by new { m.Blok.Afdeling.KebunId, m.MainBudidayaId, m.StatusAreal } into g select new LuasArealperKebun {
                KebunId = g.Key.KebunId,
                KodeKebun = g.Select(o => o.Blok.Afdeling.Kebun.kd_kbn).FirstOrDefault(),
                MainBudidayaId = g.Key.MainBudidayaId,
                KodeBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.MainBudidayaId).kd_bud,
                StatusAreal = g.Key.StatusAreal,
                LuasAreal = g.Sum(o=>o.LuasAreal)
            } ;
            return model.ToList();
        }

        public List<LuasArealperAfdeling> HitungLuasArealperAfdeling()
        {
            var model = from m in ListBlokRealisasi
                        group m by new { m.Blok.AfdelingId, m.Blok.Afdeling.KebunId, m.MainBudidayaId, m.StatusAreal } into g
                        select new LuasArealperAfdeling
                        {
                            AfdelingId = g.Key.AfdelingId,
                            KodeAfdeling = g.Select(o => o.Blok.Afdeling.kd_afd).FirstOrDefault(),
                            KebunId = g.Key.KebunId,
                            MainBudidayaId = g.Key.MainBudidayaId,
                            KodeBudidaya = CRUDBudidaya.CRUD.Get1Record(g.Key.MainBudidayaId).kd_bud,
                            StatusAreal = g.Key.StatusAreal,
                            LuasAreal = g.Sum(o => o.LuasAreal)
                        };
            return model.ToList();
        }


        public decimal getLuasArealperKebun(Guid kebunId, Guid budidayaId, string statusAreal)
        {
            var model = ListLuasArealperKebun.Where(o => o.KebunId== kebunId && o.MainBudidayaId == budidayaId && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }
        public decimal getLuasArealperKebun(string kodeKebun, string kodeBudidaya, string statusAreal)
        {
            var model = ListLuasArealperKebun.Where(o => o.KodeKebun == kodeKebun && o.KodeBudidaya == kodeBudidaya && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }


        public decimal getLuasArealperAfdeling(Guid afdelingId, Guid mainBudidayaId, string statusAreal)
        {
            var model = ListLuasArealperAfdeling.Where(o => o.AfdelingId == afdelingId && o.MainBudidayaId == mainBudidayaId && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }
        public decimal getLuasArealperAfdeling(string kodeAfdeling, string kodeBudidaya, string statusAreal)
        {
            var model = ListLuasArealperAfdeling.Where(o => o.KodeAfdeling == kodeAfdeling && o.KodeBudidaya == kodeBudidaya && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }

        public decimal getLuasArealperBlok(Guid blokId, Guid mainBudidayaId, string statusAreal)
        {
            var model = ListBlokRealisasi.Where(o => o.BlokId == blokId && o.MainBudidayaId == mainBudidayaId && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }

        public decimal getLuasArealperBlok(string kodeBlok, string kodeBudidaya, string statusAreal)
        {
            var model = ListBlokRealisasi.Where(o => o.Blok.kd_blok == kodeBlok && CRUDBudidaya.CRUD.Get1Record(o.MainBudidayaId).kd_bud== kodeBudidaya && o.StatusAreal == statusAreal);
            if (model != null) return model.Sum(o => o.LuasAreal); else return 0;
        }
        public List<ChartStatusAreal> getLuasArealperStatusAreal()
        {
            var model = from m in ListBlokRealisasi
                        join n in CRUDStatusAreal.CRUD.GetAllRecord() on m.StatusAreal equals n.Nama
                        group m by n.Deskripsi into g
                        select new ChartStatusAreal {
                            StatusAreal = g.Key,
                            LuasAreal = g.Sum(o=>o.LuasAreal)
                        };
            return model.ToList();
        }

        public List<ChartStatusArealPerBudidaya> getLuasArealperBudidaya(string kodeBudidaya)
        {
            if (kodeBudidaya != "")
            {
                var model = from m in ListBlokRealisasi.Where(o => o.MainBudidayaId == CRUDBudidaya.CRUD.Get1Record(kodeBudidaya).MainBudidayaId)
                            join n in CRUDStatusAreal.CRUD.GetAllRecord() on m.StatusAreal equals n.Nama
                            group m by n.Deskripsi into g
                            select new ChartStatusArealPerBudidaya
                            {
                                StatusAreal = g.Key,
                                LuasAreal = g.Sum(o => o.LuasAreal)
                            };
                return model.ToList();
            }
            else
            {
                var model = from m in ListBlokRealisasi.Where(o => o.MainBudidayaId != CRUDBudidaya.CRUD.Get1Record("00").MainBudidayaId)
                            .Where(o => o.MainBudidayaId != CRUDBudidaya.CRUD.Get1Record("01").MainBudidayaId)
                            .Where(o => o.MainBudidayaId != CRUDBudidaya.CRUD.Get1Record("02").MainBudidayaId)
                            .Where(o => o.MainBudidayaId != CRUDBudidaya.CRUD.Get1Record("03").MainBudidayaId)
                            join n in CRUDStatusAreal.CRUD.GetAllRecord() on m.StatusAreal equals n.Nama
                            group m by n.Deskripsi into g
                            select new ChartStatusArealPerBudidaya
                            {
                                StatusAreal = g.Key,
                                LuasAreal = g.Sum(o => o.LuasAreal)
                            };
                return model.ToList();
            }
        }

        public List<BlokRealisasi> GetAllRecord()
        {
            var model = ListBlokRealisasi;
            return model;
        }
    }
}