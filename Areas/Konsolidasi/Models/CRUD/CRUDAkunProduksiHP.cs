using Ptpn8.Areas.Referensi.Models.CRUD;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDAkunProduksiHP
    {
        public static CRUDAkunProduksiHP CRUD = new CRUDAkunProduksiHP();

        public List<AkunProduksiHP> ListAkunProduksiHP
        {
            get
            {
                List<AkunProduksiHP> result = (List<AkunProduksiHP>)HttpContext.Current.Session["ListAkunProduksiHP"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListAkunProduksiHP"] = result = Read();
                }
                return result;
            }
        }

        public List<ProduksiperBudidaya> ListProduksiperBudidaya
        {
            get
            {
                List<ProduksiperBudidaya> result = (List<ProduksiperBudidaya>)HttpContext.Current.Session["ListProduksiperBudidaya"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListProduksiperBudidaya"] = result = ReadProduksiperBudidaya();
                }
                return result;
            }
        }

        public List<ProduksiperJenis> ListProduksiperJenis
        {
            get
            {
                List<ProduksiperJenis> result = (List<ProduksiperJenis>)HttpContext.Current.Session["ListProduksiperJenis"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListProduksiperJenis"] = result = ReadProduksiperJenis();
                }
                return result;
            }
        }

        public List<AkunProduksiHP> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = db.AkunProduksiHP.ToList();
                return model;
            }
            catch
            {
                return new List<AkunProduksiHP>();
            }
        }

        public List<ProduksiperBudidaya> ReadProduksiperBudidaya()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = from m in db.AkunProduksiHP group m by new {
                    g1 = m.tanggal.Value.Year,
                    g2 = m.tanggal.Value.Month,
                    g3 = m.kodeunit,
                    g4 = m.kodebud,
                    g5 = m.kodevar,
                    g6 = m.norek
                } into g select new ProduksiperBudidaya {
                    Tahun = g.Key.g1,
                    Bulan = g.Key.g2,
                    KodeKebun = g.Key.g3,
                    KodeBudidaya = g.Key.g4,
                    Variable = g.Key.g5,
                    KodeRekening = g.Key.g6,
                    JumlahRealisasi = g.Sum(o=>o.bi),
                    JumlahRKAP = g.Sum(o=>o.rbi),
                    JumlahPKB = g.Sum(o=>o.pbi)
                };

                return model.ToList();
            }
            catch
            {
                return new List<ProduksiperBudidaya>();
            }
        }

        public List<ProduksiperJenis> ReadProduksiperJenis()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                var model = from m in db.AkunProduksiHP
                            group m by new
                            {
                                g1 = m.tanggal.Value.Year,
                                g2 = m.tanggal.Value.Month,
                                g3 = m.kodeunit,
                                g4 = m.kodeJenisOlah,
                                g5 = m.kodevar,
                                g6 = m.norek
                            } into g
                            select new ProduksiperJenis
                            {
                                Tahun = g.Key.g1,
                                Bulan = g.Key.g2,
                                KodeKebun = g.Key.g3,
                                KodeJenisBudidaya = g.Key.g4,
                                Variable = g.Key.g5,
                                KodeRekening = g.Key.g6,
                                JumlahRealisasi = g.Sum(o => o.bi),
                                JumlahRKAP = g.Sum(o => o.rbi),
                                JumlahPKB = g.Sum(o => o.pbi)
                            };

                return model.ToList();
            }
            catch
            {
                return new List<ProduksiperJenis>();
            }
        }


        public List<AkunProduksiHP> GetAllRecord()
        {
            var model = ListAkunProduksiHP;
            return model;
        }

        public List<ProduksiperBudidaya> GetProduksiperBudidaya()
        {
            var model = ListProduksiperBudidaya;
            return model;
        }

        public List<ProduksiperJenis> GetProduksiperJenis()
        {
            var model = ListProduksiperJenis;
            return model;
        }

        public double RealisasiRasioBasahKeringBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double) ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RealisasiRasioBasahKeringsdBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RKAPRasioBasahKeringBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RKAPRasioBasahKeringsdBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }
        public double PKBRasioBasahKeringBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double PKBRasioBasahKeringsdBulanIni(string kodeBudidaya, string kodeKebun, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            kodeKebun = kodeKebun.Substring(0, 2);
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.KodeKebun == kodeKebun && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RealisasiRasioBasahKeringBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RealisasiRasioBasahKeringsdBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRealisasi);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RealisasiRasioBasahKeringBulanIni(string kodeBudidaya, int bulan, int tahun, string dataran)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            if (dataran == "") dataran = "Sedang";
            produksiBasah = (double) (from m in ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X")
                            join n in CRUDKebun.CRUD.GetAllRecord() on m.KodeKebun equals n.kd_kbn where n.Dataran == dataran select m).Sum(o=>o.JumlahRealisasi);
            produksiKering = (double) (from m in ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X")
                            join n in CRUDKebun.CRUD.GetAllRecord() on m.KodeKebun equals n.kd_kbn where n.Dataran == dataran select m).Sum(o => o.JumlahRealisasi);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RealisasiRasioBasahKeringsdBulanIni(string kodeBudidaya, int bulan, int tahun, string dataran)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            if (dataran == "") dataran = "Sedang";
            produksiBasah = (double)(from m in ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X")
                                     join n in CRUDKebun.CRUD.GetAllRecord() on m.KodeKebun equals n.kd_kbn
                                     where n.Dataran == dataran
                                     select m).Sum(o => o.JumlahRealisasi);
            produksiKering = (double)(from m in ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X")
                                      join n in CRUDKebun.CRUD.GetAllRecord() on m.KodeKebun equals n.kd_kbn
                                      where n.Dataran == dataran
                                      select m).Sum(o => o.JumlahRealisasi);

            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RKAPRasioBasahKeringBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double RKAPRasioBasahKeringsdBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahRKAP);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }
        public double PKBRasioBasahKeringBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan == bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

        public double PKBRasioBasahKeringsdBulanIni(string kodeBudidaya, int bulan, int tahun)
        {
            double result = 0;
            double produksiBasah = 0;
            double produksiKering = 0;
            produksiBasah = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "BASAH" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            produksiKering = (double)ListProduksiperBudidaya.Where(o => o.KodeBudidaya == kodeBudidaya && o.Bulan <= bulan && o.Tahun == tahun && o.Variable == "JADI" && o.KodeRekening == "X").Sum(o => o.JumlahPKB);
            if (produksiBasah > 0)
                result = produksiKering / produksiBasah;
            else
                result = 0;

            return result;
        }

    }
}