using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAkunRKAP
    {
        public static CRUDAkunRKAP CRUD = new CRUDAkunRKAP();

        private int _TahunBuku;
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set {
                if(value!=_TahunBuku)
                {
                    HttpContext.Current.Session["ListAkunRKAP"]=null;
                    HttpContext.Current.Session["ListRKAPperKebun"]=null;
                    HttpContext.Current.Session["ListRKAPperAfdeling"] = null;
                    HttpContext.Current.Session["ListRKAPperBlok"]=null;
                }
                this._TahunBuku = value;
            }
        }

        public List<AkunRKAP> ListAkunRKAP
        {
            get
            {
                List<AkunRKAP> result = (List<AkunRKAP>)HttpContext.Current.Session["ListAkunRKAP"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAkunRKAP"] = result = Read();
                }
                return result;
            }
        }

        public List<RKAPperKebun> ListRKAPperKebun
        {
            get
            {
                List<RKAPperKebun> result = (List<RKAPperKebun>)HttpContext.Current.Session["ListRKAPperKebun"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRKAPperKebun"] = result = ReadRKAPperKebun();
                }
                return result;
            }
        }

        public List<RKAPperAfdeling> ListRKAPperAfdeling
        {
            get
            {
                List<RKAPperAfdeling> result = (List<RKAPperAfdeling>)HttpContext.Current.Session["ListRKAPperAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRKAPperAfdeling"] = result = ReadRKAPperAfdeling();
                }
                return result;
            }
        }

        public List<RKAPperBlok> ListRKAPperBlok
        {
            get
            {
                List<RKAPperBlok> result = (List<RKAPperBlok>)HttpContext.Current.Session["ListRKAPperBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRKAPperBlok"] = result = ReadRKAPperBlok();
                }
                return result;
            }
        }

        public List<AkunRKAP> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AkunRKAP.ToList();
                return model;
            }
            catch
            {
                return new List<AkunRKAP>();
            }
        }

        public List<RKAPperKebun> ReadRKAPperKebun()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = from m in db.AkunRKAP.Where(o => o.tanggal.Value.Year == TahunBuku)
                            group m by new
                            {
                                g1 = m.tanggal.Value.Year,
                                g2 = m.tanggal.Value.Month,
                                g3 = m.kodeunit,
                                g4 = m.kodebud,
                                g5 = m.unsur,
                                g6 = m.klsKary,
                                g7 = m.norek
                            } into g
                            select new RKAPperKebun
                            {
                                Tahun = g.Key.g1,
                                Bulan = g.Key.g2,
                                KodeKebun = g.Key.g3,
                                KodeBudidaya = g.Key.g4,
                                KodeUnsur = g.Key.g5 == "GU" ? "U" : g.Key.g5 == "BB" ? "B" : "L",
                                StatusKaryawan = g.Key.g6 == "A" ? "KB" : g.Key.g6 == "B" ? "KB" : g.Key.g6 == "C" ? "KB" : "KL",
                                KodeRekening = g.Key.g7,
                                JumlahKaryawanRKAP = g.Sum(o=>o.jmlKary_RKAP),
                                JumlahHKRKAP = g.Sum(o=>o.jmlHK_RKAP),
                                JumlahHasilKerjaRKAP =g.Sum(o=>o.jmlHsl_RKAP),
                                NilaiRKAP = g.Sum(o=>o.nilai_RKAP),
                                JumlahKaryawanPKB = g.Sum(o=>o.jmlKary_PMK),
                                JumlahHKPKB = g.Sum(o=>o.jmlHK_PMK),
                                JumlahHasilKerjaPKB = g.Sum(o=>o.jmlHsl_PMK),
                                NilaiPKB = g.Sum(o=>o.nilai_PMK),
                                //KebunId = (CRUDKebun.CRUD.Get1Record(g.Key.g3) == null ? Guid.Empty : CRUDKebun.CRUD.Get1Record(g.Key.g3).KebunId),
                                //NamaKebun = (CRUDKebun.CRUD.Get1Record(g.Key.g3) == null ? "" : CRUDKebun.CRUD.Get1Record(g.Key.g3).Nama),
                                //MainBudidayaId = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? Guid.Empty : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).MainBudidayaId),
                                //NamaBudidaya = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? "" : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).Nama)
                            };
                return model.ToList();
            }
            catch
            {
                return new List<RKAPperKebun>();
            }
        }
        public List<RKAPperAfdeling> ReadRKAPperAfdeling()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = from m in db.AkunRKAP.Where(o => o.tanggal.Value.Year == TahunBuku)
                            group m by new
                            {
                                g1 = m.tanggal.Value.Year,
                                g2 = m.tanggal.Value.Month,
                                g3 = m.kodeunit + m.kodeafd,
                                g4 = m.kodebud,
                                g5 = m.unsur,
                                g6 = m.klsKary,
                                g7 = m.norek
                            } into g
                            select new RKAPperAfdeling
                            {
                                Tahun = g.Key.g1,
                                Bulan = g.Key.g2,
                                KodeAfdeling = g.Key.g3,
                                KodeBudidaya = g.Key.g4,
                                KodeUnsur = g.Key.g5 == "GU" ? "U" : g.Key.g5 == "BB" ? "B" : "L",
                                StatusKaryawan = g.Key.g6 == "A" ? "KB" : g.Key.g6 == "B" ? "KB" : g.Key.g6 == "C" ? "KB" : "KL",
                                KodeRekening = g.Key.g7,
                                JumlahKaryawanRKAP = g.Sum(o => o.jmlKary_RKAP),
                                JumlahHKRKAP = g.Sum(o => o.jmlHK_RKAP),
                                JumlahHasilKerjaRKAP = g.Sum(o => o.jmlHsl_RKAP),
                                NilaiRKAP = g.Sum(o => o.nilai_RKAP),
                                JumlahKaryawanPKB = g.Sum(o => o.jmlKary_PMK),
                                JumlahHKPKB = g.Sum(o => o.jmlHK_PMK),
                                JumlahHasilKerjaPKB = g.Sum(o => o.jmlHsl_PMK),
                                NilaiPKB = g.Sum(o => o.nilai_PMK),
                                //AfdelingId = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).AfdelingId),
                                //KebunId = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).KebunId),
                                //NamaAfdeling = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).Nama),
                                //MainBudidayaId = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? Guid.Empty : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).MainBudidayaId),
                                //NamaBudidaya = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? "" : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).Nama)
                            };
                return model.ToList();
            }
            catch 
            {
                return new List<RKAPperAfdeling>();
            }
        }

        public List<RKAPperBlok> ReadRKAPperBlok()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = from m in db.AkunRKAP.Where(o => o.tanggal.Value.Year == TahunBuku)
                            group m by new
                            {
                                g1 = m.tanggal.Value.Year,
                                g2 = m.tanggal.Value.Month,
                                g3 = m.kodeunit + m.kodeafd + m.kodeblok,
                                g4 = m.kodebud,
                                g5 = m.unsur,
                                g6 = m.klsKary,
                                g7 = m.norek,
                                g8 = m.thntnm
                            } into g
                            select new RKAPperBlok
                            {
                                Tahun = g.Key.g1,
                                Bulan = g.Key.g2,
                                KodeBlok = g.Key.g3,
                                KodeAfdeling = g.Key.g3.Substring(0,4),
                                KodeBudidaya = g.Key.g4,
                                KodeUnsur = g.Key.g5 == "GU" ? "U" : g.Key.g5 == "BB" ? "B" : "L",
                                StatusKaryawan = g.Key.g6 == "A" ? "KB" : g.Key.g6 == "B" ? "KB" : g.Key.g6 == "C" ? "KB" : "KL",
                                KodeRekening = g.Key.g7,
                                TahunTanam = g.Key.g8,
                                JumlahKaryawanRKAP = g.Sum(o => o.jmlKary_RKAP),
                                JumlahHKRKAP = g.Sum(o => o.jmlHK_RKAP),
                                JumlahHasilKerjaRKAP = g.Sum(o => o.jmlHsl_RKAP),
                                NilaiRKAP = g.Sum(o => o.nilai_RKAP),
                                JumlahKaryawanPKB = g.Sum(o => o.jmlKary_PMK),
                                JumlahHKPKB = g.Sum(o => o.jmlHK_PMK),
                                JumlahHasilKerjaPKB = g.Sum(o => o.jmlHsl_PMK),
                                NilaiPKB = g.Sum(o => o.nilai_PMK),
                                //AfdelingId = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).AfdelingId),
                                //KebunId = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? Guid.Empty : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).KebunId),
                                //NamaAfdeling = (CRUDAfdeling.CRUD.Get1Record(g.Key.g3) == null ? "" : CRUDAfdeling.CRUD.Get1Record(g.Key.g3).Nama),
                                //MainBudidayaId = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? Guid.Empty : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).MainBudidayaId),
                                //NamaBudidaya = (CRUDBudidaya.CRUD.Get1Record(g.Key.g4) == null ? "" : CRUDBudidaya.CRUD.Get1Record(g.Key.g4).Nama)
                            };
                return model.ToList();
            }
            catch 
            {
                return new List<RKAPperBlok>();
            }
        }


        public List<AkunRKAP> GetAllRecord()
        {
            var model = ListAkunRKAP;
            return model;
        }

        public List<RKAPperKebun> GetRKAPPerKebun()
        {
            var model = ListRKAPperKebun;
            return model;
        }

        public List<RKAPperAfdeling> GetRKAPPerAfdeling()
        {
            var model = ListRKAPperAfdeling;
            return model;
        }

        public List<RKAPperBlok> GetRKAPPerBlok()
        {
            var model = ListRKAPperBlok;
            return model;
        }

    }
}