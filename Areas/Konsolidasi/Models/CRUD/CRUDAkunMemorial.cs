using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAkunMemorial
    {
        public static CRUDAkunMemorial CRUD = new CRUDAkunMemorial();

        private int _TahunBuku;
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set {
                if(value!=_TahunBuku)
                {
                    HttpContext.Current.Session["ListAkunMemorial"]=null;
                    HttpContext.Current.Session["ListSaldoperKebun"]=null;
                    HttpContext.Current.Session["ListSaldoperAfdeling"]=null;
                    HttpContext.Current.Session["ListSaldoperBlok"]=null;
                }
                this._TahunBuku = value;
            }
        }

        public List<AkunMemorial> ListAkunMemorial
        {
            get
            {
                List<AkunMemorial> result = (List<AkunMemorial>)HttpContext.Current.Session["ListAkunMemorial"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAkunMemorial"] = result = Read();
                }
                return result;
            }
        }
        public List<SaldoperKebun> ListSaldoperKebun
        {
            get
            {
                List<SaldoperKebun> result = (List<SaldoperKebun>)HttpContext.Current.Session["ListSaldoperKebun"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListSaldoperKebun"] = result = ReadSaldoperKebun();
                }
                return result;
            }
        }

        public List<SaldoperAfdeling> ListSaldoperAfdeling
        {
            get
            {
                List<SaldoperAfdeling> result = (List<SaldoperAfdeling>)HttpContext.Current.Session["ListSaldoperAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListSaldoperAfdeling"] = result = ReadSaldoperAfdeling();
                }
                return result;
            }
        }

        public List<SaldoperBlok> ListSaldoperBlok
        {
            get
            {
                List<SaldoperBlok> result = (List<SaldoperBlok>)HttpContext.Current.Session["ListSaldoperBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListSaldoperBlok"] = result = ReadSaldoperBlok();
                }
                return result;
            }
        }
        public List<AkunMemorial> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AkunMemorial.ToList();
                return model;
            }
            catch
            {
                return new List<AkunMemorial>();
            }
        }

        public List<SaldoperKebun> ReadSaldoperKebun()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = (from m in db.AkunMemorial.Where(o => o.tanggal.Value.Year == TahunBuku).Where(o => o.NoInput.Trim() == "SALDO AWAL").Where(o=>o.kodebud.Trim().Length==2)
                            group m by new
                            {
                                g1 = m.tanggal.Value.Month==1 ? m.tanggal.Value.Year-1: m.tanggal.Value.Year,
                                g2 = m.tanggal.Value.Month==1 ? 12 : m.tanggal.Value.Month - 1,
                                g3 = m.KodeUnit,
                                g4 = m.kodebud,
                                g5 = m.norek
                            }
                            into g select new SaldoperKebun {
                                Tahun = g.Key.g1,
                                Bulan = g.Key.g2,
                                KodeKebun = g.Key.g3,
                                KodeBudidaya = g.Key.g4,
                                KodeRekening = g.Key.g5,
                                KodeUnsur = g.Select(o=>o.unsur).FirstOrDefault(),
                                JumlahFisik = g.Sum(o=>o.JmlFisik),
                                Nilai = g.Sum(o=>o.DK=="D"?o.nilai:o.nilai*-1)
                            }).ToList();
                
                return model;
            }
            catch
            {
                return new List<SaldoperKebun>();
            }
        }

        public List<SaldoperAfdeling> ReadSaldoperAfdeling()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = (from m in db.AkunMemorial.Where(o => o.tanggal.Value.Year == TahunBuku).Where(o => o.NoInput.Trim() == "SALDO AWAL").Where(o => o.kodebud.Trim().Length == 2)
                             .Where(o=>o.KodeUnit.Trim().Length==2).Where(o => o.kodeafd.Trim().Length == 2)
                             group m by new
                             {
                                 g1 = m.tanggal.Value.Month == 1 ? m.tanggal.Value.Year - 1 : m.tanggal.Value.Year,
                                 g2 = m.tanggal.Value.Month == 1 ? 12 : m.tanggal.Value.Month - 1,
                                 g3 = m.KodeUnit+m.kodeafd,
                                 g4 = m.kodebud,
                                 g5 = m.norek
                             }
                            into g
                             select new SaldoperAfdeling
                             {
                                 Tahun = g.Key.g1,
                                 Bulan = g.Key.g2,
                                 KodeAfdeling = g.Key.g3,
                                 KodeBudidaya = g.Key.g4,
                                 KodeRekening = g.Key.g5,
                                 KodeUnsur = g.Select(o => o.unsur).FirstOrDefault(),
                                 JumlahFisik = g.Sum(o => o.JmlFisik),
                                 Nilai = g.Sum(o => o.DK == "D" ? o.nilai : o.nilai * -1)
                             }).ToList();

                return model;
            }
            catch
            {
                return new List<SaldoperAfdeling>();
            }
        }

        public List<SaldoperBlok> ReadSaldoperBlok()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = (from m in db.AkunMemorial.Where(o => o.tanggal.Value.Year == TahunBuku).Where(o => o.NoInput.Trim() == "SALDO AWAL").Where(o => o.kodebud.Trim().Length == 2)
                             .Where(o => o.KodeUnit.Trim().Length == 2).Where(o => o.kodeafd.Trim().Length == 2).Where(o => o.kdblok.Trim().Length == 4)
                             group m by new
                             {
                                 g1 = m.tanggal.Value.Month == 1 ? m.tanggal.Value.Year - 1 : m.tanggal.Value.Year,
                                 g2 = m.tanggal.Value.Month == 1 ? 12 : m.tanggal.Value.Month - 1,
                                 g3 = m.KodeUnit + m.kodeafd + m.kdblok,
                                 g4 = m.kodebud,
                                 g5 = m.norek
                             }
                            into g
                             select new SaldoperBlok
                             {
                                 Tahun = g.Key.g1,
                                 Bulan = g.Key.g2,
                                 KodeBlok = g.Key.g3,
                                 KodeBudidaya = g.Key.g4,
                                 KodeRekening = g.Key.g5,
                                 KodeUnsur = g.Select(o => o.unsur).FirstOrDefault(),
                                 JumlahFisik = g.Sum(o => o.JmlFisik),
                                 Nilai = g.Sum(o => o.DK == "D" ? o.nilai : o.nilai * -1)
                             }).ToList();

                return model;
            }
            catch
            {
                return new List<SaldoperBlok>();
            }
        }

        public List<SaldoperKebun> GetSaldoPerKebun()
        {
            var model = ListSaldoperKebun;
            return model;
        }

        public List<SaldoperAfdeling> GetSaldoPerAfdeling()
        {
            var model = ListSaldoperAfdeling;
            return model;
        }
        public List<SaldoperBlok> GetSaldoPerBlok()
        {
            var model = ListSaldoperBlok;
            return model;
        }
        public List<AkunMemorial> GetAllRecord()
        {
            var model = ListAkunMemorial;
            return model;
        }

    }
}