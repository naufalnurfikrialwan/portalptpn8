using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDDataViewBlok
    {
        public static CRUDDataViewBlok CRUD = new CRUDDataViewBlok();

        public List<DataViewBlok> ListDataViewBlok
        {
            get
            {
                List<DataViewBlok> result = (List<DataViewBlok>)HttpContext.Current.Session["ListDataViewBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListDataViewBlok"] = result = Read();
                }
                return result;
            }
        }
        public List<DataViewBlok> Read()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            try
            {
                var model = (from blk in CRUDBlok.CRUD.GetAllRecord()
                             join blkR in CRUDBlokRealisasi.CRUD.GetAllRecord() on blk.BlokId equals blkR.BlokId
                             join hgu in CRUDHGU.CRUD.GetAllRecord() on blkR.HGUId equals hgu.HGUId
                             group blkR by new
                             {
                                 a = blk.BlokId,
                                 b = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).Nama,
                                 c = CRUDStatusAreal.CRUD.Get1Record(blkR.StatusArealId).Nama,
                                 d = CRUDStatusAreal.CRUD.Get1Record(blkR.StatusArealId).Deskripsi,
                                 e = blk.kd_blok,
                                 f = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).kd_bud
                             } into g
                             select new DataViewBlok
                             {
                                 Id = g.Key.a,
                                 Afdeling = g.Select(o => o.Blok.Afdeling.Nama).FirstOrDefault(),
                                 AfdelingId = g.Select(o => o.Blok.Afdeling.AfdelingId).FirstOrDefault(),
                                 Nama = g.Key.c == "1.1" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e,g.Key.f,"TM")!=null? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TM").namablok : g.Select(o => o.Blok.Nama).FirstOrDefault()) :
                                     g.Key.c == "1.2" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM")!=null? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM").namablok : g.Select(o => o.Blok.Nama).FirstOrDefault()) :
                                     g.Key.c == "1.3" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI")!=null? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI").namablok : g.Select(o => o.Blok.Nama).FirstOrDefault()) :
                                     g.Key.c == "1.5" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN")!=null? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN").namablok : g.Select(o => o.Blok.Nama).FirstOrDefault()) :
                                     g.Select(o => o.Blok.Nama).FirstOrDefault(),
                                 DaftarBudidaya = g.Key.b ,
                                 DaftarTahunTanam = g.Key.c == "1.1" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TM") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TM").tahuntanam : g.Select(o => o.Blok.TahunTanam).FirstOrDefault()) :
                                     g.Key.c == "1.2" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM").tahuntanam : g.Select(o => o.Blok.TahunTanam).FirstOrDefault()) :
                                     g.Key.c == "1.3" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI").tahuntanam : g.Select(o => o.Blok.TahunTanam).FirstOrDefault()) :
                                     g.Key.c == "1.5" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN").tahuntanam : g.Select(o => o.Blok.TahunTanam).FirstOrDefault()) :
                                     g.Select(o => o.Blok.TahunTanam).FirstOrDefault(),
                                 DaftarSKHGU = g.Where(o => o.HGU.No_SK.Trim() != "").Select(o => o.HGU.No_SK).FirstOrDefault(),

                                 DaftarStatusAreal = g.Key.c == "1.1" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TM") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TM").status : g.Key.d) :
                                     g.Key.c == "1.2" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TBM").status : g.Key.d) :
                                     g.Key.c == "1.3" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "TTI").status : g.Key.d) :
                                     g.Key.c == "1.5" ? (CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN") != null ? CRUDRef_Areal.CRUD.Get1Record(g.Key.e, g.Key.f, "PESEMAIAN").status : g.Key.d) :
                                     g.Key.d,

                                 TanggalBerlaku = g.Where(o => o.HGU.TanggalTerbit.Year != 1900).Select(o => o.HGU.TanggalTerbit.ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 TanggalBerakhir = g.Where(o => o.HGU.TanggalBerakhir.Year != 1900).Select(o => o.HGU.TanggalBerakhir.ToString("dd/MM/yyyy")).FirstOrDefault(),
                                 SisaWaktu = g.Select(o => o.HGU.SisaWaktu).FirstOrDefault(), //new List<int> { g.Key.d },
                                 LuasAreal = g.Key.c == "1.1" ? CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TM") :
                                     g.Key.c == "1.2" ? CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.e, g.Key.f, "TBM") :
                                     g.Key.c == "1.3" ? CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.e, g.Key.f, "TTI") :
                                     g.Key.c == "1.5" ? CRUDRef_Areal.CRUD.HitungLuasArealBlok(g.Key.e, g.Key.f, "PESEMAIAN") :
                                     g.Sum(o => o.LuasAreal)
                             }).ToList();

                return model;
            }
            catch
            {
                return new List<DataViewBlok>();
            }
        }

        public List<DataViewBlok> GetAllRecord()
        {
            var model = ListDataViewBlok;
            return model;
        }

    }
}