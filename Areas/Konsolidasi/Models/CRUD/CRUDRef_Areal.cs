using Ptpn8.Areas.Referensi.Models.CRUD;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDRef_Areal
    {
        public static CRUDRef_Areal CRUD = new CRUDRef_Areal();

        public List<Ref_Areal> ListRef_Areal
        {
            get
            {
                List<Ref_Areal> result = (List<Ref_Areal>)HttpContext.Current.Session["ListRef_Areal"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListRef_Areal"] = result = Read();
                }
                return result;
            }
        }
        public List<Ref_Areal> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.Ref_Areal.ToList();
                return model;
            }
            catch
            {
                return new List<Ref_Areal>();
            }
        }

        public List<Ref_Areal> GetAllRecord()
        {
            var model = ListRef_Areal;
            return model;
        }

        public Ref_Areal Get1Record(string kodeLokasi, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling + o.kodeblok == kodeLokasi).Where(o => o.kodebudidaya == kodeBudidaya)
                .Where(o=>o.status == statusAreal).FirstOrDefault();
            return model;
        }

        public decimal HitungLuasArealPTPN8(string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }
        public decimal HitungLuasArealPTPN8(string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungLuasArealPTPN8(string kodeBudidaya, string statusAreal, string tahunTanam, string dataran)
        {
            if (dataran == "") dataran = "Sedang";

            if (tahunTanam=="")
            {
                var model = (from m in ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya && o.status == statusAreal)
                             join n in CRUDKebun.CRUD.GetAllRecord() on m.kodekebun equals n.kd_kbn
                             where n.Dataran == dataran
                             select m).Sum(o => o.luas);
                if (model != null) return (decimal)model; else return 0;
            }
            else
            {
                var model = (from m in ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya && o.status == statusAreal && o.tahuntanam == tahunTanam)
                             join n in CRUDKebun.CRUD.GetAllRecord() on m.kodekebun equals n.kd_kbn
                             where n.Dataran == dataran
                             select m).Sum(o => o.luas);
                if (model != null) return (decimal)model; else return 0;
            }
        }

        public decimal HitungLuasArealPTPN8PerDataran(string kodeBudidaya, string statusAreal, string dataran)
        {
            if (dataran == "") dataran = "Sedang";
            var model = (from m in ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya && o.status == statusAreal)
                        join n in CRUDKebun.CRUD.GetAllRecord() on m.kodekebun equals n.kd_kbn
                        where n.Dataran==dataran select m).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }
        public decimal HitungLuasArealKebun(string kodeKebun, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun == kodeKebun).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o=>o.luas);
            if (model != null) return (decimal)model; else return 0;
        }
        public decimal HitungLuasArealKebun(string kodeKebun, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun == kodeKebun).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungLuasArealAfdeling(string kodeAfdeling, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun+o.kodeafdeling == kodeAfdeling).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungLuasArealAfdeling(string kodeAfdeling, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling== kodeAfdeling).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam==tahunTanam).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungLuasArealBlok(string kodeBlok, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling + o.kodeblok == kodeBlok).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungLuasArealBlok(string kodeBlok, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling + o.kodeblok== kodeBlok).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam==tahunTanam).Sum(o => o.luas);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonPTPN8(string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }
        public decimal HitungJumlahPohonPTPN8(string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonKebun(string kodeKebun, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun == kodeKebun).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }
        public decimal HitungJumlahPohonKebun(string kodeKebun, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun == kodeKebun).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonAfdeling(string kodeAfdeling, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling == kodeAfdeling).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonAfdeling(string kodeAfdeling, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling == kodeAfdeling).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonBlok(string kodeBlok, string kodeBudidaya, string statusAreal)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling + o.kodeblok == kodeBlok).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

        public decimal HitungJumlahPohonBlok(string kodeBlok, string kodeBudidaya, string statusAreal, string tahunTanam)
        {
            var model = ListRef_Areal.Where(o => o.kodekebun + o.kodeafdeling + o.kodeblok == kodeBlok).Where(o => o.kodebudidaya == kodeBudidaya).Where(o => o.status == statusAreal && o.tahuntanam == tahunTanam).Sum(o => o.jmlpohon);
            if (model != null) return (decimal)model; else return 0;
        }

    }
}