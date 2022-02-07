using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDDataViewAfdeling
    {
        public static CRUDDataViewAfdeling CRUD = new CRUDDataViewAfdeling();

        public List<DataViewAfdeling> ListDataViewAfdeling
        {
            get
            {
                List<DataViewAfdeling> result = (List<DataViewAfdeling>)HttpContext.Current.Session["ListDataViewAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListDataViewAfdeling"] = result = Read();
                }
                return result;
            }
        }
        public List<DataViewAfdeling> Read()
        {
            CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
            try
            {
                var model = (from afd in CRUDAfdeling.CRUD.GetAllRecord()
                             join blkR in CRUDBlokRealisasi.CRUD.GetAllRecord() on afd.AfdelingId equals blkR.Blok.AfdelingId
                             join hgu in CRUDHGU.CRUD.GetAllRecord() on blkR.HGUId equals hgu.HGUId
                             group blkR by new
                             {
                                 a = afd.AfdelingId,
                                 b = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).Nama,
                                 c = CRUDStatusAreal.CRUD.Get1Record(blkR.StatusArealId).Nama,
                                 //d = hgu.SisaWaktu,
                                 e = afd.kd_afd,
                                 f = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).kd_bud
                             } into g
                             select new DataViewAfdeling
                             {
                                 Id = g.Key.a,
                                 Kebun = g.Select(o => o.Blok.Afdeling.Kebun.Nama).FirstOrDefault(),
                                 KebunId = g.Select(o => o.Blok.Afdeling.Kebun.KebunId).FirstOrDefault(),
                                 Nama = g.Select(o => o.Blok.Afdeling.Nama).FirstOrDefault(),
                                 DaftarBudidaya = new List<string> { g.Key.b },
                                 DaftarTahunTanam = g.Where(o => o.Blok.TahunTanam.Trim() != "").Select(o => o.Blok.TahunTanam).ToList(),
                                 DaftarSKHGU = g.Where(o => o.HGU.No_SK.Trim() != "").Select(o => o.HGU.No_SK).ToList(),
                                 DaftarStatusAreal = new List<string> { g.Key.c },
                                 TanggalBerlaku = g.Where(o => o.HGU.TanggalTerbit.Year != 1900).Select(o => o.HGU.TanggalTerbit.ToString("dd/MM/yyyy")).ToList(),
                                 TanggalBerakhir = g.Where(o => o.HGU.TanggalBerakhir.Year != 1900).Select(o => o.HGU.TanggalBerakhir.ToString("dd/MM/yyyy")).ToList(),
                                 SisaWaktu = g.Select(o => o.HGU.SisaWaktu).ToList(), //new List<int> { g.Key.d },
                                 //TM = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e,g.Key.f,"TM"),
                                 ////g.Where(o => o.StatusAreal == "1.1").Sum(o => o.LuasAreal),
                                 //TBM = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "TBM"),
                                 ////g.Where(o => o.StatusAreal == "1.2").Sum(o => o.LuasAreal),
                                 //TTI = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "TTI"),
                                 ////g.Where(o => o.StatusAreal == "1.3").Sum(o => o.LuasAreal),

                                 //TTAD = g.Where(o => o.StatusAreal == "1.4").Sum(o => o.LuasAreal),
                                 //Pesemaian = CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "Pesemaian"),
                                 ////g.Where(o => o.StatusAreal == "1.5").Sum(o => o.LuasAreal),
                                 //Entrys = g.Where(o => o.StatusAreal == "1.6").Sum(o => o.LuasAreal),
                                 //Reboisasi = g.Where(o => o.StatusAreal == "1.7").Sum(o => o.LuasAreal),
                                 //Monocrop = g.Where(o => o.StatusAreal == "1.8").Sum(o => o.LuasAreal),
                                 //Intercrop = g.Where(o => o.StatusAreal == "1.9").Sum(o => o.LuasAreal),
                                 //TPJ = g.Where(o => o.StatusAreal == "1.10").Sum(o => o.LuasAreal),
                                 //Emplasemen = g.Where(o => o.StatusAreal == "2.1").Sum(o => o.LuasAreal),
                                 //Jalan = g.Where(o => o.StatusAreal == "2.2").Sum(o => o.LuasAreal),
                                 //Fasos = g.Where(o => o.StatusAreal == "2.3").Sum(o => o.LuasAreal),
                                 //Marginal = g.Where(o => o.StatusAreal == "2.4").Sum(o => o.LuasAreal),
                                 //CadanganMurni = g.Where(o => o.StatusAreal == "2.5.1").Sum(o => o.LuasAreal),
                                 //CadanganPokok = g.Where(o => o.StatusAreal == "2.5.2").Sum(o => o.LuasAreal),
                                 //Agrowisata = g.Where(o => o.StatusAreal == "2.6").Sum(o => o.LuasAreal),
                                 //KerjasamaBisnis = g.Where(o => o.StatusAreal == "3.1").Sum(o => o.LuasAreal),
                                 //PinjamPakai = g.Where(o => o.StatusAreal == "3.2").Sum(o => o.LuasAreal),
                                 //Okupasi = g.Where(o => o.StatusAreal == "4.1").Sum(o => o.LuasAreal),
                                 LuasAreal = g.Key.c == "1.1" ? CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "TM") :
                                     g.Key.c == "1.2" ? CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "TBM") :
                                     g.Key.c == "1.3" ? CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "TTI") :
                                     g.Key.c == "1.5" ? CRUDRef_Areal.CRUD.HitungLuasArealAfdeling(g.Key.e, g.Key.f, "PESEMAIAN") :
                                     g.Sum(o => o.LuasAreal)
                             }).ToList();

                foreach (var m in model)
                {
                    //m.DaftarBudidaya = m.DaftarDistinct().OrderBy(x => x).ToList();
                    m.DaftarSKHGU = m.DaftarSKHGU.Distinct().OrderBy(x => x).ToList();
                    //m.DaftarStatusAreal = m.DaftarStatusAreal.Distinct().OrderBy(x => x).ToList();
                    m.DaftarTahunTanam = m.DaftarTahunTanam.Distinct().OrderBy(x => x).ToList();
                    m.SisaWaktu = m.SisaWaktu.Distinct().OrderBy(x => x).ToList();
                    m.TanggalBerakhir = m.TanggalBerakhir.Distinct().OrderBy(x => x).ToList();
                    m.TanggalBerlaku = m.TanggalBerlaku.Distinct().OrderBy(x => x).ToList();
                }
                return model;
            }
            catch
            {
                return new List<DataViewAfdeling>();
            }
        }

        public List<DataViewAfdeling> GetAllRecord()
        {
            var model = ListDataViewAfdeling;
            return model;
        }

    }
}