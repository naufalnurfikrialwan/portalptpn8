using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDKartu
    {
        public static CRUDKartu CRUD = new CRUDKartu();

        private int _TahunBuku;
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set {

                if(value!=_TahunBuku)
                {
                    HttpContext.Current.Session["ListKartu"] = null;
                    HttpContext.Current.Session["ListKartuperKebun"] = null;
                    HttpContext.Current.Session["ListKartuperAfdeling"] = null;
                    HttpContext.Current.Session["ListKartuperBlok"] = null;
                }
                this._TahunBuku = value;
            }
        }

        public List<Kartu> ListKartu
        {
            get
            {
                List<Kartu> result = Read();//(List<Kartu>)HttpContext.Current.Session["ListKartu"];
                //if (result == null)
                //{
                //    HttpContext.Current.Session["ListKartu"] = result = Read();
                //}
                return result;
            }
        }

        public List<KartuperKebun> ListKartuperKebun
        {
            get
            {
                List<KartuperKebun> result = ReadKartuperKebun(); //(List<KartuperKebun>)HttpContext.Current.Session["ListKartuperKebun"];
                //if (result == null)
                //{
                //    HttpContext.Current.Session["ListKartuperKebun"] = result = ReadKartuperKebun();
                //}
                return result;
            }
        }

        public List<KartuperAfdeling> ListKartuperAfdeling
        {
            get
            {
                List<KartuperAfdeling> result = ReadKartuperAfdeling(); //(List<KartuperAfdeling>)HttpContext.Current.Session["ListKartuperAfdeling"];
                //if (result == null)
                //{
                //    HttpContext.Current.Session["ListKartuperAfdeling"] = result = ReadKartuperAfdeling();
                //}
                return result;
            }
        }

        public List<KartuperBlok> ListKartuperBlok
        {
            get
            {
                List<KartuperBlok> result = ReadKartuperBlok(); //(List<KartuperBlok>)HttpContext.Current.Session["ListKartuperBlok"];
                //if (result == null)
                //{
                //    HttpContext.Current.Session["ListKartuperBlok"] = result = ReadKartuperBlok();
                //}
                return result;
            }
        }

        public List<Kartu> Read()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;
            try
            {
                //var model = db.Kartu.Where(o=>o.Tanggal.Value.Year == TahunBuku).ToList();
                //return model;

                List<Kartu> model = new List<Kartu>();
                //string strQry = "SELECT * FROM [SPDK_KBN_KONSOL].[dbo].[kartu] WHERE year(tanggal)=" + TahunBuku.ToString();
                //using (var conn = new SqlConnection(cs))
                //using (var cmd = conn.CreateCommand())
                //{
                //    conn.Open();
                //    cmd.CommandText = strQry;
                //    using (var reader = cmd.ExecuteReader())
                //    {
                //        while (reader.Read())
                //        {
                //            model.Add(new Kartu
                //            {
                //                KartuID = reader.GetGuid(reader.GetOrdinal("KartuID")),
                //                KodeUnit = reader.GetString(reader.GetOrdinal("kodeunit")),
                //                Tanggal = reader.GetDateTime(reader.GetOrdinal("tanggal")),
                //                NoBukti = reader.GetString(reader.GetOrdinal("NoBukti")),
                //                NoInput = reader.GetString(reader.GetOrdinal("NoInput")),
                //                KodeBudidaya = reader.GetString(reader.GetOrdinal("KodeBudidaya")),
                //                KodeAfdeling = reader.GetString(reader.GetOrdinal("KodeAfdeling")),
                //                Norek = reader.GetString(reader.GetOrdinal("Norek")),
                //                KodeBarang = reader.GetString(reader.GetOrdinal("KodeBarang")),
                //                Netral = reader.GetString(reader.GetOrdinal("Netral")),
                //                kdblok = reader.GetString(reader.GetOrdinal("kdblok")),
                //                KodeUnsur = reader.GetString(reader.GetOrdinal("KodeUnsur")),
                //                Uraian = reader.GetString(reader.GetOrdinal("Uraian")),
                //                Jml_fisik = reader.GetDouble(reader.GetOrdinal("Jml_fisik")),
                //                Debet = reader.GetDouble(reader.GetOrdinal("Debet")),
                //                Kredit = reader.GetDouble(reader.GetOrdinal("Kredit")),
                //                Aplikasi = reader.GetString(reader.GetOrdinal("Aplikasi"))
                //            });
                //        };
                //    }
                //    conn.Close();
                //}
                return model;
            }
            catch
            {
                return new List<Kartu>();
            }
        }

        public List<KartuperKebun> ReadKartuperKebun()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;

            List<KartuperKebun> model = new List<KartuperKebun>();
            string strQry = "SELECT KodeUnit as KodeKebun,month(tanggal) as Bulan,year(tanggal) as Tahun, KodeBudidaya, KodeUnsur, Norek as KodeRekening, sum(Jml_fisik) as JumlahFisik, sum(Debet-Kredit) as Nilai FROM [SPDK_KBN_KONSOL].[dbo].[kartu] WHERE year(tanggal) = " +
                TahunBuku.ToString() + " group by KodeUnit, month(tanggal), year(tanggal), KodeBudidaya, KodeUnsur, Norek";
            using (var conn = new SqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = strQry;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.Add(new KartuperKebun
                        {
                            Tahun = reader.GetInt32(reader.GetOrdinal("Tahun")),
                            Bulan = reader.GetInt32(reader.GetOrdinal("Bulan")),
                            KodeKebun = reader.GetString(reader.GetOrdinal("KodeKebun")),
                            KodeBudidaya = reader.GetString(reader.GetOrdinal("KodeBudidaya")),
                            KodeUnsur = reader.GetString(reader.GetOrdinal("KodeUnsur")),
                            KodeRekening = reader.GetString(reader.GetOrdinal("KodeRekening")),
                            JumlahFisik = reader.GetDouble(reader.GetOrdinal("JumlahFisik")),
                            Nilai = reader.GetDouble(reader.GetOrdinal("Nilai"))
                        });
                    };
                }
                conn.Close();
            }
            return model.ToList();
        }

        public List<KartuperAfdeling> ReadKartuperAfdeling()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;

            List<KartuperAfdeling> model = new List<KartuperAfdeling>();
            string strQry = "SELECT KodeUnit+KodeAfdeling as KodeAfdeling,month(tanggal) as Bulan,year(tanggal) as Tahun, KodeBudidaya, KodeUnsur, Norek as KodeRekening, sum(Jml_fisik) as JumlahFisik, sum(Debet-Kredit) as Nilai FROM [SPDK_KBN_KONSOL].[dbo].[kartu] WHERE year(tanggal) = " +
                TahunBuku.ToString() + " group by KodeUnit, KodeAfdeling, month(tanggal), year(tanggal), KodeBudidaya, KodeUnsur, Norek";
            using (var conn = new SqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = strQry;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.Add(new KartuperAfdeling
                        {
                            Tahun = reader.GetInt32(reader.GetOrdinal("Tahun")),
                            Bulan = reader.GetInt32(reader.GetOrdinal("Bulan")),
                            KodeAfdeling = reader.GetString(reader.GetOrdinal("KodeAfdeling")),
                            KodeBudidaya = reader.GetString(reader.GetOrdinal("KodeBudidaya")),
                            KodeUnsur = reader.GetString(reader.GetOrdinal("KodeUnsur")),
                            KodeRekening = reader.GetString(reader.GetOrdinal("KodeRekening")),
                            JumlahFisik = reader.GetDouble(reader.GetOrdinal("JumlahFisik")),
                            Nilai = reader.GetDouble(reader.GetOrdinal("Nilai"))
                        });
                    };
                }
                conn.Close();
            }
            return model.ToList();


            //try
            //{
            //    var model = from m in ListKartu
            //                group m by new
            //                {
            //                    g1 = m.Tanggal.Value.Year,
            //                    g2 = m.Tanggal.Value.Month,
            //                    g3 = m.KodeUnit + m.KodeAfdeling,
            //                    g4 = m.KodeBudidaya,
            //                    g5 = m.KodeUnsur,
            //                    g6 = m.Norek
            //                } into g
            //                select new KartuperAfdeling
            //                {
            //                    Tahun = g.Key.g1,
            //                    Bulan = g.Key.g2,
            //                    KodeAfdeling = g.Key.g3,
            //                    KodeBudidaya = g.Key.g4,
            //                    KodeUnsur = g.Key.g5,
            //                    KodeRekening = g.Key.g6,
            //                    JumlahFisik = g.Sum(o => o.Jml_fisik),
            //                    Nilai = g.Sum(o => o.Debet - o.Kredit),
            //                };
            //    return model.ToList();
            //}
            //catch (Exception e)
            //{
            //    return new List<KartuperAfdeling>();
            //}
        }

        public List<KartuperBlok> ReadKartuperBlok()
        {
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;

            List<KartuperBlok> model = new List<KartuperBlok>();
            string strQry = "SELECT KodeUnit+KodeAfdeling+kdblok as KodeAfdeling,month(tanggal) as Bulan,year(tanggal) as Tahun, KodeBudidaya, KodeUnsur, Norek as KodeRekening, sum(Jml_fisik) as JumlahFisik, sum(Debet-Kredit) as Nilai FROM [SPDK_KBN_KONSOL].[dbo].[kartu] WHERE len(kdblok)=4 and year(tanggal) = " +
                TahunBuku.ToString() + " group by KodeUnit, KodeAfdeling, kdblok, month(tanggal), year(tanggal), KodeBudidaya, KodeUnsur, Norek";
            using (var conn = new SqlConnection(cs))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = strQry;
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model.Add(new KartuperBlok
                        {
                            Tahun = reader.GetInt32(reader.GetOrdinal("Tahun")),
                            Bulan = reader.GetInt32(reader.GetOrdinal("Bulan")),
                            KodeBlok = reader.GetString(reader.GetOrdinal("KodeBlok")),
                            KodeBudidaya = reader.GetString(reader.GetOrdinal("KodeBudidaya")),
                            KodeUnsur = reader.GetString(reader.GetOrdinal("KodeUnsur")),
                            KodeRekening = reader.GetString(reader.GetOrdinal("KodeRekening")),
                            JumlahFisik = reader.GetDouble(reader.GetOrdinal("JumlahFisik")),
                            Nilai = reader.GetDouble(reader.GetOrdinal("Nilai"))
                        });
                    };
                }
                conn.Close();
            }
            return model.ToList();

            //try
            //{
            //    var model = from m in ListKartu.Where(o=>o.kdblok.Trim().Length==4)
            //                group m by new
            //                {
            //                    g1 = m.Tanggal.Value.Year,
            //                    g2 = m.Tanggal.Value.Month,
            //                    g3 = m.KodeUnit + m.KodeAfdeling + m.kdblok,
            //                    g4 = m.KodeBudidaya,
            //                    g5 = m.KodeUnsur,
            //                    g6 = m.Norek    
            //                } into g
            //                select new KartuperBlok
            //                {
            //                    Tahun = g.Key.g1,
            //                    Bulan = g.Key.g2,
            //                    KodeBlok = g.Key.g3,
            //                    KodeBudidaya = g.Key.g4,
            //                    KodeUnsur = g.Key.g5,
            //                    KodeRekening = g.Key.g6,
            //                    JumlahFisik = g.Sum(o => o.Jml_fisik),
            //                    Nilai = g.Sum(o => o.Debet - o.Kredit),
            //                };
            //    return model.ToList();
            //}
            //catch (Exception e)
            //{
            //    return new List<KartuperBlok>();
            //}
        }
        public List<Kartu> GetAllRecord()
        {
            var model = ListKartu;
            return model;
        }

        public List<KartuperKebun> GetKartuPerKebun()
        {
            var model = ListKartuperKebun;
            return model;
        }

        public List<KartuperAfdeling> GetKartuPerAfdeling()
        {
            var model = ListKartuperAfdeling;
            return model;
        }

        public List<KartuperBlok> GetKartuPerBlok()
        {
            var model = ListKartuperBlok;
            return model;
        }
    }
}