using Ptpn8.Areas.Konsolidasi.Models;
using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDHasilPanen
    {
        public static CRUDHasilPanen CRUD = new CRUDHasilPanen();
        private int _TahunBuku;
        private int _BulanBuku;
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set { this._TahunBuku = value; }
        }
        public int BulanBuku
        {
            get { return _BulanBuku; }
            set { this._BulanBuku = value; }
        }


        public List<HasilPanen> ListHasilPanenPerBlok
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerBlok"];
                if (result == null || result.Count==0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerBlok"] = result = ReadHasilPanenPerBlok();
                }
                return result;
            }
        }

        public List<HasilPanen> ListHasilPanenPerAfdeling
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerAfdeling"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerAfdeling"] = result = ReadHasilPanenPerAfdeling();
                }
                return result;
            }
        }

        public List<HasilPanen> ListHasilPanenPerKebun
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerKebun"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerKebun"] = result = ReadHasilPanenPerKebun();
                }
                return result;
            }
        }

        public List<HasilPanen> ListHasilPanenPerBlokPer6Bulan
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerBlokPer6Bulan"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerBlokPer6Bulan"] = result = ReadHasilPanenPerBlokPer6Bulan(BulanBuku,TahunBuku);
                }
                return result;
            }
        }

        public List<HasilPanen> ListHasilPanenPerAfdelingPer6Bulan
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerAfdelingPer6Bulan"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerAfdelingPer6Bulan"] = result = ReadHasilPanenPerAfdelingPer6Bulan();
                }
                return result;
            }
        }

        public List<HasilPanen> ListHasilPanenPerKebunPer6Bulan
        {
            get
            {
                List<HasilPanen> result = (List<HasilPanen>)HttpContext.Current.Session["ListHasilPanenPerKebunPer6Bulan"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerKebunPer6Bulan"] = result = ReadHasilPanenPerKebunPer6Bulan();
                }
                return result;
            }
        }

        public List<HasilPanenPerTanggal> ListHasilPanenPerBlokPerTanggal
        {
            get
            {
                List<HasilPanenPerTanggal> result = (List<HasilPanenPerTanggal>)HttpContext.Current.Session["ListHasilPanenPerBlokPerTanggal"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerBlokPerTanggal"] = result = ReadHasilPanenPerBlokPerTanggal();
                }
                return result;
            }
        }

        private List<HasilPanenPerTanggal> ListHasilPanenPerAfdelingPerTanggal
        {
            get
            {
                List<HasilPanenPerTanggal> result = (List<HasilPanenPerTanggal>)HttpContext.Current.Session["ListHasilPanenPerAfdelingPerTanggal"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerAfdelingPerTanggal"] = result = ReadHasilPanenPerAfdelingPerTanggal();
                }
                return result;
            }
        }

        private List<HasilPanenPerTanggal> ListHasilPanenPerKebunPerTanggal
        {
            get
            {
                List<HasilPanenPerTanggal> result = (List<HasilPanenPerTanggal>)HttpContext.Current.Session["ListHasilPanenPerKebunPerTanggal"];
                if (result == null || result.Count == 0)
                {
                    HttpContext.Current.Session["ListHasilPanenPerKebunPerTanggal"] = result = ReadHasilPanenPerKebunPerTanggal();
                }
                return result;
            }
        }

        private List<HasilPanenPerTanggal> ReadHasilPanenPerBlokPerTanggal()
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            try
            {
                List<HasilPanenPerTanggal> model = new List<HasilPanenPerTanggal>();
                string strQry =
                    "SELECT A.kodeunit,A.kodebud1,A.kodeafd1,A.kdblok,A.tanggal,A.kdpanen,A.thntnm,A.regmdr,A.grup, " +
                    "Count(*) as RealisasiJumlahHK, SUM(A.jmlupah) as jmlupah, SUM(A.hslpanen) as hslpanen, SUM(A.hslpanenlump) as hslpanenlump, " +
                    "SUM(A.hslpanenTBS) as hslpanenTBS, sum(A.jelajahHA) as Jelajah, sum(A.nbrd) as JumlahPohon, avg(A.jelajahHA) as JelajahMesin  " +
                    "FROM[SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] A " +
                    "inner join[SPDK_KBN_KONSOL].[dbo].[ref_areal] B on B.kodekebun + B.kodeafdeling + B.kodeblok = A.kodeunit + A.kodeafd1 + A.kdblok and A.thntnm = B.tahuntanam " +
                    "where year(A.tanggal) = " + TahunBuku.ToString() + " and LEN(A.kdblok) = 4 and len(rtrim(A.norek)) = 9 and substring(A.norek, 1, 5) = '60203' and A.kodeabs = '1' and B.status = 'TM' and len(A.thntnm) = 4 " +
                    "group by A.kodeunit, A.kodebud1, A.kodeafd1, A.kdblok, A.tanggal, A.kdpanen, A.thntnm, A.regmdr, A.grup";

                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new HasilPanenPerTanggal
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd1")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud1")),
                                Tanggal = reader.GetDateTime(reader.GetOrdinal("tanggal")),
                                KgLateks = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "00" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                KgLump = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "00" ? reader.GetDouble(reader.GetOrdinal("hslpanenlump")) : 0),
                                KgTBS = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "01" && reader.GetString(reader.GetOrdinal("kdpanen")) == "1" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                KgBrondol = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "01" && reader.GetString(reader.GetOrdinal("kdpanen")) == "2" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                JumlahTandan = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "01" ? reader.GetDouble(reader.GetOrdinal("hslpanenTBS")) : 0),
                                JumlahPohon = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "02" ? reader.GetDouble(reader.GetOrdinal("Jelajah")) : reader.GetDouble(reader.GetOrdinal("JumlahPohon"))),
                                KgPucuk = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "02" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                PanenLainnya = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) != "00" && reader.GetString(reader.GetOrdinal("kodebud1")) != "01" && reader.GetString(reader.GetOrdinal("kodebud1")) != "02" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                JumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                HaJelajah = (decimal)(reader.GetString(reader.GetOrdinal("kodebud1")) == "02" ? reader.GetDouble(reader.GetOrdinal("JelajahMesin")) : reader.GetDouble(reader.GetOrdinal("Jelajah"))),
                                RpGaji = (decimal)reader.GetDouble(reader.GetOrdinal("jmlupah")),
                                GroupMesin = reader.GetString(reader.GetOrdinal("grup")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT A.kodeunit,A.kodebud,A.kodeafd,A.kdblok,A.tanggal,A.thntnm," +
                            "sum(A.nilai) as nilai, Count(*) as RealisasiJumlahHK, SUM(A.jmlhasil) as jmlhasil FROM [SPDK_KBN_KONSOL].[dbo].[GajiKulir] A " +
                            "inner join[SPDK_KBN_KONSOL].[dbo].[ref_areal] B on B.kodekebun + B.kodeafdeling + B.kodeblok = A.kodeunit + A.kodeafd + A.kdblok and A.thntnm = B.tahuntanam " +
                            "where year(A.tanggal)=" + TahunBuku.ToString() + " and LEN(A.kdblok)=4 and len(rtrim(A.norek))=9 and substring(A.norek,1,5)='60203' and len(A.thntnm)=4 and B.status = 'TM' " +
                            "group by A.kodeunit, A.kodebud, A.kodeafd, A.kdblok, A.tanggal, A.thntnm";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new HasilPanenPerTanggal
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Tanggal = reader.GetDateTime(reader.GetOrdinal("tanggal")),
                                KgLateks = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "00" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                KgTBS = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "01" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                KgPucuk = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "02" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                PanenLainnya = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) != "00" && reader.GetString(reader.GetOrdinal("kodebud")) != "01" && reader.GetString(reader.GetOrdinal("kodebud")) != "02" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                JumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RpGaji = (decimal)reader.GetDouble(reader.GetOrdinal("nilai"))
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT A.kodeunit, A.kodebud, A.kodeafd, A.kdblok, A.tanggal, A.thntnm," +
                        "sum(A.jmlupah) as jmlupah, sum(A.jmlhadir) as RealisasiJumlahHK, SUM(A.jmlhasil) as jmlhasil " +
                        "FROM [SPDK_KBN_KONSOL].[dbo].[GajiSPKHonor] A " +
                        "inner join [SPDK_KBN_KONSOL].[dbo].[ref_areal] B on B.kodekebun + B.kodeafdeling + B.kodeblok = A.kodeunit + A.kodeafd + A.kdblok and A.thntnm = B.tahuntanam " +
                        "where year(A.tanggal)=" + TahunBuku.ToString() + " and LEN(A.kdblok)=4 and len(rtrim(A.norek))=9 and substring(A.norek,1,5)='60203' and len(A.thntnm)=4  and B.status='TM' " +
                        "group by A.kodeunit, A.kodebud, A.kodeafd, A.kdblok, A.tanggal, A.thntnm";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new HasilPanenPerTanggal
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Tanggal = reader.GetDateTime(reader.GetOrdinal("tanggal")),
                                KgLateks = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "00" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                KgTBS = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "01" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                KgPucuk = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "02" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                PanenLainnya = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) != "00" && reader.GetString(reader.GetOrdinal("kodebud")) != "01" && reader.GetString(reader.GetOrdinal("kodebud")) != "02" ? reader.GetDouble(reader.GetOrdinal("jmlhasil")) : 0),
                                JumlahHK = (int)reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")),
                                RpGaji = (decimal)reader.GetDouble(reader.GetOrdinal("jmlupah"))
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT A.kodeunit, A.kodebud, A.kodeafd, A.kodeblok, A.thntnm, month(A.tanggal) as tanggal, " +
                        "sum(A.nilai_RKAP) as RKAPjmlupah, sum(A.nilai_PMK) as PMKjmlupah, " +
                        "sum(A.jmlHK_RKAP) as RKAPJumlahHK, sum(A.jmlHK_PMK) as PMKJumlahHK, " +
                        "SUM(A.jmlHsl_RKAP) as RKAPjmlhasil, SUM(A.jmlHsl_PMK) as PMKjmlhasil " +
                        "FROM [SPDK_KBN_KONSOL].[dbo].[AkunRKAP] A " +
                        "inner join [SPDK_KBN_KONSOL].[dbo].[ref_areal] B on B.kodekebun + B.kodeafdeling + B.kodeblok = A.kodeunit + A.kodeafd + A.kodeblok and A.thntnm = B.tahuntanam " +
                        "where year(A.tanggal)=" + TahunBuku.ToString() + " and LEN(A.kodeblok)=4 and len(rtrim(A.norek))=9 and substring(A.norek,1,5)='60203' and len(A.thntnm)=4  and B.status='TM' " +
                        "group by A.kodeunit, A.kodebud, A.kodeafd, A.kodeblok, A.thntnm, month(A.tanggal)";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                var mdl = new HasilPanenPerTanggal
                                {
                                    KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")) + reader.GetString(reader.GetOrdinal("kodeblok")),
                                    TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                    KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                    Tanggal = DateTime.Parse("01/" + reader.GetInt32(reader.GetOrdinal("tanggal")).ToString() + "/" + TahunBuku.ToString()),
                                    RKAPKgLateks = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "00" ? reader.GetDouble(reader.GetOrdinal("RKAPjmlhasil")) : 0),
                                    RKAPKgTBS = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "01" ? reader.GetDouble(reader.GetOrdinal("RKAPjmlhasil")) : 0),
                                    RKAPKgPucuk = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "02" ? reader.GetDouble(reader.GetOrdinal("RKAPjmlhasil")) : 0),
                                    RKAPPanenLainnya = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) != "00" && reader.GetString(reader.GetOrdinal("kodebud")) != "01" && reader.GetString(reader.GetOrdinal("kodebud")) != "02" ? reader.GetDouble(reader.GetOrdinal("RKAPjmlhasil")) : 0),
                                    RKAPJumlahHK = (int)reader.GetDouble(reader.GetOrdinal("RKAPJumlahHK")),
                                    RKAPRpGaji = (decimal)reader.GetDouble(reader.GetOrdinal("RKAPjmlupah")),
                                    PKBKgLateks = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "00" ? reader.GetDouble(reader.GetOrdinal("PMKjmlhasil")) : 0),
                                    PKBKgTBS = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "01" ? reader.GetDouble(reader.GetOrdinal("PMKjmlhasil")) : 0),
                                    PKBKgPucuk = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) == "02" ? reader.GetDouble(reader.GetOrdinal("PMKjmlhasil")) : 0),
                                    PKBPanenLainnya = (decimal)(reader.GetString(reader.GetOrdinal("kodebud")) != "00" && reader.GetString(reader.GetOrdinal("kodebud")) != "01" && reader.GetString(reader.GetOrdinal("kodebud")) != "02" ? reader.GetDouble(reader.GetOrdinal("PMKjmlhasil")) : 0),
                                    PKBJumlahHK = (int)reader.GetDouble(reader.GetOrdinal("PMKJumlahHK")),
                                    PKBRpGaji = (decimal)reader.GetDouble(reader.GetOrdinal("PMKjmlupah"))
                                };

                                model.Add(mdl);
                            }
                            catch (Exception ex)
                            {
                                var x = ex;
                            }

                        };
                    }
                    conn.Close();
                }

                return model;
            }
            catch 
            {
                return new List<HasilPanenPerTanggal>();
            }
        }

        private List<HasilPanen> ReadHasilPanenPerBlok()
        {
            try
            {
                var model = (from m in ListHasilPanenPerBlokPerTanggal
                             group m by new
                             {
                                 g1 = m.KodeBlok,
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Tanggal.Month,
                                 g5 = m.Tanggal.Year,
                                 g6 = m.GroupMesin
                             } into g
                             select new HasilPanen
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Bulan = g.Key.g4,
                                 Tahun = g.Key.g5,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g6,
                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanen>();
            }
        }

        private List<HasilPanen> ReadHasilPanenPerBlokPer6Bulan(int bulan, int tahun)
        {

            bulan = int.Parse(HttpContext.Current.Session["BulanView"].ToString());
            tahun = int.Parse(HttpContext.Current.Session["TahunView"].ToString());

            int bulanAwal;
            int tahunAwal;

            if(bulan-5>0)
            {
                bulanAwal = bulan - 5;
                tahunAwal = tahun;
            }
            else
            {
                bulanAwal = 12 + (bulan - 5);
                tahunAwal = tahun - 1;
            }

            List<HasilPanenPerTanggal> modelPerTanggal = new List<HasilPanenPerTanggal>();
            if (tahunAwal!=tahun)
            {
                this.TahunBuku = tahunAwal;
                modelPerTanggal.AddRange(ReadHasilPanenPerBlokPerTanggal());
                this.TahunBuku = tahun;
            }
            modelPerTanggal.AddRange(ListHasilPanenPerBlokPerTanggal);
            var model = (from m in modelPerTanggal where (m.Tanggal.Month>=bulanAwal && m.Tanggal.Year>=tahunAwal) && (m.Tanggal.Month<=bulan && m.Tanggal.Year<=tahun) 
                         group m by new
                         {
                             g1 = m.KodeBlok,
                             g2 = m.KodeBudidaya,
                             g3 = m.TahunTanam,
                             g4 = m.Tanggal.Month,
                             g5 = m.Tanggal.Year,
                             g6 = m.GroupMesin
                         } into g
                         select new HasilPanen
                         {
                             KodeBlok = g.Key.g1,
                             TahunTanam = g.Key.g3,
                             KodeBudidaya = g.Key.g2,
                             LuasAreal = 0,
                             Bulan = g.Key.g4,
                             Tahun = g.Key.g5,
                             KgLateks = g.Sum(o => o.KgLateks),
                             KgLump = g.Sum(o => o.KgLump),
                             KgTBS = g.Sum(o => o.KgTBS),
                             KgBrondol = g.Sum(o => o.KgBrondol),
                             JumlahTandan = g.Sum(o => o.JumlahTandan),
                             JumlahPohon = g.Sum(o => o.JumlahPohon),
                             KgPucuk = g.Sum(o => o.KgPucuk),
                             PanenLainnya = g.Sum(o => o.PanenLainnya),
                             JumlahHK = g.Sum(o => o.JumlahHK),
                             HaJelajah = g.Sum(o => o.HaJelajah),
                             RpGaji = g.Sum(o => o.RpGaji),

                             RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                             RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                             RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                             RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                             RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                             RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                             RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                             RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                             RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                             RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                             PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                             PKBKgLump = g.Sum(o => o.PKBKgLump),
                             PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                             PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                             PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                             PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                             PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                             PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                             PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                             PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             GroupMesin = g.Key.g6
                         }).ToList();
            return model;
        }
        
        private List<HasilPanen> ReadHasilPanenPerAfdeling()
        {
            try
            {
                var model = (from m in ListHasilPanenPerBlok
                             group m by new {
                                g1 = m.KodeBlok.Substring(0,4),
                                g2 = m.KodeBudidaya,
                                g3 = m.TahunTanam,
                                g4 = m.Bulan,
                                g5 = m.Tahun,
                                g6 = m.GroupMesin} into g
                            select new HasilPanen {
                                KodeBlok = g.Key.g1,
                                TahunTanam = g.Key.g3,
                                KodeBudidaya = g.Key.g2,
                                LuasAreal = 0,
                                Bulan = g.Key.g4,
                                Tahun = g.Key.g5,
                                KgLateks = g.Sum(o=>o.KgLateks),
                                KgLump = g.Sum(o => o.KgLump),
                                KgTBS = g.Sum(o => o.KgTBS),
                                KgBrondol = g.Sum(o => o.KgBrondol),
                                JumlahTandan = g.Sum(o => o.JumlahTandan),
                                JumlahPohon = g.Sum(o => o.JumlahPohon),
                                KgPucuk = g.Sum(o => o.KgPucuk),
                                PanenLainnya = g.Sum(o => o.PanenLainnya),
                                JumlahHK = g.Sum(o => o.JumlahHK),
                                HaJelajah = g.Sum(o => o.HaJelajah),
                                RpGaji = g.Sum(o => o.RpGaji),
                                GroupMesin = g.Key.g6,

                                RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                PKBKgLump = g.Sum(o => o.PKBKgLump),
                                PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                PKBRpGaji = g.Sum(o => o.PKBRpGaji),


                            }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanen>();
            }
        }

        private List<HasilPanen> ReadHasilPanenPerAfdelingPer6Bulan()
        {
            try
            {
                var model = (from m in ListHasilPanenPerBlokPer6Bulan
                             group m by new
                             {
                                 g1 = m.KodeBlok.Substring(0, 4),
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Bulan,
                                 g5 = m.Tahun,
                                 g6 = m.GroupMesin
                             } into g
                             select new HasilPanen
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Bulan = g.Key.g4,
                                 Tahun = g.Key.g5,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g6,

                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanen>();
            }
        }

        private List<HasilPanenPerTanggal> ReadHasilPanenPerAfdelingPerTanggal()
        {
            try
            {
                var model = (from m in ListHasilPanenPerBlokPerTanggal
                             group m by new
                             {
                                 g1 = m.KodeBlok.Substring(0, 4),
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Tanggal,
                                 g5 = m.GroupMesin
                             } into g
                             select new HasilPanenPerTanggal
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Tanggal = g.Key.g4,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g5,

                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanenPerTanggal>();
            }
        }

        private List<HasilPanen> ReadHasilPanenPerKebun()
        {
            try
            {
                var model = (from m in ListHasilPanenPerAfdeling
                             group m by new
                             {
                                 g1 = m.KodeBlok.Substring(0, 2),
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Bulan,
                                 g5 = m.Tahun,
                                 g6 = m.GroupMesin
                             } into g
                             select new HasilPanen
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Bulan = g.Key.g4,
                                 Tahun = g.Key.g5,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g6,

                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanen>();
            }
        }

        private List<HasilPanen> ReadHasilPanenPerKebunPer6Bulan()
        {
            try
            {
                var model = (from m in ListHasilPanenPerAfdelingPer6Bulan
                             group m by new
                             {
                                 g1 = m.KodeBlok.Substring(0, 2),
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Bulan,
                                 g5 = m.Tahun,
                                 g6 = m.GroupMesin
                             } into g
                             select new HasilPanen
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Bulan = g.Key.g4,
                                 Tahun = g.Key.g5,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g6,

                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),

                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanen>();
            }
        }

        private List<HasilPanenPerTanggal> ReadHasilPanenPerKebunPerTanggal()
        {
            try
            {
                var model = (from m in ListHasilPanenPerAfdelingPerTanggal
                             group m by new
                             {
                                 g1 = m.KodeBlok.Substring(0, 2),
                                 g2 = m.KodeBudidaya,
                                 g3 = m.TahunTanam,
                                 g4 = m.Tanggal,
                                 g5 = m.GroupMesin
                             } into g
                             select new HasilPanenPerTanggal
                             {
                                 KodeBlok = g.Key.g1,
                                 TahunTanam = g.Key.g3,
                                 KodeBudidaya = g.Key.g2,
                                 LuasAreal = 0,
                                 Tanggal = g.Key.g4,
                                 KgLateks = g.Sum(o => o.KgLateks),
                                 KgLump = g.Sum(o => o.KgLump),
                                 KgTBS = g.Sum(o => o.KgTBS),
                                 KgBrondol = g.Sum(o => o.KgBrondol),
                                 JumlahTandan = g.Sum(o => o.JumlahTandan),
                                 JumlahPohon = g.Sum(o => o.JumlahPohon),
                                 KgPucuk = g.Sum(o => o.KgPucuk),
                                 PanenLainnya = g.Sum(o => o.PanenLainnya),
                                 JumlahHK = g.Sum(o => o.JumlahHK),
                                 HaJelajah = g.Sum(o => o.HaJelajah),
                                 RpGaji = g.Sum(o => o.RpGaji),
                                 GroupMesin = g.Key.g5,
                                 RKAPKgLateks = g.Sum(o => o.RKAPKgLateks),
                                 RKAPKgLump = g.Sum(o => o.RKAPKgLump),
                                 RKAPKgTBS = g.Sum(o => o.RKAPKgTBS),
                                 RKAPKgBrondol = g.Sum(o => o.RKAPKgBrondol),
                                 RKAPJumlahTandan = g.Sum(o => o.RKAPJumlahTandan),
                                 RKAPKgPucuk = g.Sum(o => o.RKAPKgPucuk),
                                 RKAPPanenLainnya = g.Sum(o => o.RKAPPanenLainnya),
                                 RKAPJumlahHK = g.Sum(o => o.RKAPJumlahHK),
                                 RKAPHaJelajah = g.Sum(o => o.RKAPHaJelajah),
                                 RKAPRpGaji = g.Sum(o => o.RKAPRpGaji),

                                 PKBKgLateks = g.Sum(o => o.PKBKgLateks),
                                 PKBKgLump = g.Sum(o => o.PKBKgLump),
                                 PKBKgTBS = g.Sum(o => o.PKBKgTBS),
                                 PKBKgBrondol = g.Sum(o => o.PKBKgBrondol),
                                 PKBJumlahTandan = g.Sum(o => o.PKBJumlahTandan),
                                 PKBKgPucuk = g.Sum(o => o.PKBKgPucuk),
                                 PKBPanenLainnya = g.Sum(o => o.PKBPanenLainnya),
                                 PKBJumlahHK = g.Sum(o => o.PKBJumlahHK),
                                 PKBHaJelajah = g.Sum(o => o.PKBHaJelajah),
                                 PKBRpGaji = g.Sum(o => o.PKBRpGaji),


                             }).ToList();
                return model;
            }
            catch
            {
                return new List<HasilPanenPerTanggal>();
            }
        }

        public List<HasilPanen> GetHasilPanenPerBlok()
        {
            var model = ListHasilPanenPerBlok;
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerBlokPer6Bulan()
        {
            var model = ListHasilPanenPerBlokPer6Bulan;
            return model;
        }

        public List<HasilPanenPerTanggal> GetHasilPanenPerBlokPerTanggal()
        {
            var model = ListHasilPanenPerBlokPerTanggal;
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerAfdeling()
        {
            var model = ListHasilPanenPerAfdeling;
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerAfdelingPer6Bulan()
        {
            var model = ListHasilPanenPerAfdelingPer6Bulan;
            return model;
        }

        public List<HasilPanenPerTanggal> GetHasilPanenPerAfdelingPerTanggal()
        {
            var model = ListHasilPanenPerAfdelingPerTanggal;
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerKebun()
        {
            var model = ListHasilPanenPerKebun;
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerKebun(string dataran)
        {
            if (dataran == "") dataran = "Sedang";
            var model = (from m in ListHasilPanenPerKebun join n in CRUDKebun.CRUD.GetAllRecord() on m.KodeBlok equals n.kd_kbn
                        where n.Dataran==dataran select m).ToList();
            return model;
        }

        public List<HasilPanen> GetHasilPanenPerKebunPer6Bulan()
        {
            var model = ListHasilPanenPerKebunPer6Bulan;
            return model;
        }

        public List<HasilPanenPerTanggal> GetHasilPanenPerKebunPerTanggal()
        {
            var model = ListHasilPanenPerKebunPerTanggal;
            return model;
        }

    }
}