using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Konsolidasi.Models.CRUD
{
    public class CRUDGajiAbsensi
    {
        public static CRUDGajiAbsensi CRUD = new CRUDGajiAbsensi();
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

        public List<GajiAbsensiPerKebun> ListGajiAbsensiPerKebun
        {
            get
            {
                List<GajiAbsensiPerKebun> result = (List<GajiAbsensiPerKebun>)HttpContext.Current.Session["ListGajiAbsensiPerKebun"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiAbsensiPerKebun"] = result = ReadPerKebun();
                }
                else
                {
                    var m = result.Where(o => o.Bulan == BulanBuku);
                    if(m == null)
                    {
                        HttpContext.Current.Session["ListGajiAbsensiPerKebun"] = result = ReadPerKebun();
                    }
                }
                return result;
            }
        }

        public List<GajiAbsensiPerAfdeling> ListGajiAbsensiPerAfdeling
        {
            get
            {
                List<GajiAbsensiPerAfdeling> result = (List<GajiAbsensiPerAfdeling>)HttpContext.Current.Session["ListGajiAbsensiPerAfdeling"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiAbsensiPerAfdeling"] = result = ReadPerAfdeling();
                }
                else
                {
                    var m = result.Where(o => o.Bulan == BulanBuku);
                    if (m == null)
                    {
                        HttpContext.Current.Session["ListGajiAbsensiPerAfdeling"] = result = ReadPerAfdeling();
                    }
                }
                return result;
            }
        }

        public List<GajiAbsensiPerBlok> ListGajiAbsensiPerBlok
        {
            get
            {
                List<GajiAbsensiPerBlok> result = (List<GajiAbsensiPerBlok>)HttpContext.Current.Session["ListGajiAbsensiPerBlok"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListGajiAbsensiPerBlok"] = result = ReadPerBlok();
                }
                else
                {
                    var m = result.Where(o => o.Bulan == BulanBuku);
                    if (m == null)
                    {
                        HttpContext.Current.Session["ListGajiAbsensiPerBlok"] = result = ReadPerBlok();
                    }
                }
                return result;
            }
        }


        public List<GajiAbsensiPerKebun> ReadPerKebun()
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            CRUDKartu.CRUD.TahunBuku = TahunBuku;
            CRUDAkunRKAP.CRUD.TahunBuku = TahunBuku;
            CRUDAkunMemorial.CRUD.TahunBuku = TahunBuku;

            try
            {
                List<GajiAbsensiPerKebun> model = new List<GajiAbsensiPerKebun>();
                string strQry = "SELECT kodeunit,kodebud1,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts,kodeabs,kdpanen, grup, " +
                        "Count(*) as RealisasiJumlahHK, SUM(hslpanen) as hslpanen, sum(hslpemel) as hslpemel, sum(hsllain2) as hsllain2, SUM(hslpanenlump) as hslpanenlump, " +
                        "SUM(hslpanenTBS) as hslpanenTBS, SUM(jelajahHA) as jelajah FROM[SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] " +
                        "where month(tanggal)<="+BulanBuku.ToString()+" and year(tanggal)="+TahunBuku.ToString()+ " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud1,MONTH(tanggal),YEAR(tanggal),norek,sts,kodeabs,kdpanen,grup";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerKebun {
                                KodeKebun = reader.GetString(reader.GetOrdinal("kodeunit")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud1")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                KodeAbsen = reader.GetString(reader.GetOrdinal("kodeabs")),
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("hslpemel")) + reader.GetDouble(reader.GetOrdinal("hsllain2")) +
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "1" || reader.GetString(reader.GetOrdinal("kdpanen")) == "" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaLump = reader.GetDouble(reader.GetOrdinal("hslpanenlump")) + 
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "2" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaTandan = reader.GetDouble(reader.GetOrdinal("hslpanenTBS")),
                                RealisasiJelajah = reader.GetDouble(reader.GetOrdinal("jelajah")),
                                GrupPanen = reader.GetString(reader.GetOrdinal("grup")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebud,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts," +
                        "Count(*) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiKulir] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud,MONTH(tanggal),YEAR(tanggal),norek,sts";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerKebun
                            {
                                KodeKebun = reader.GetString(reader.GetOrdinal("kodeunit")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                KodeAbsen = "1",
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebud,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,NoSPK," +
                        "sum(jmlhadir) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiSPKHonor] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud,MONTH(tanggal),YEAR(tanggal),norek,NoSPK";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerKebun
                            {
                                KodeKebun = reader.GetString(reader.GetOrdinal("kodeunit")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = "KL",
                                KodeAbsen = "1",
                                RealisasiJumlahHK = (Int32) (reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK"))!=reader.GetDouble(reader.GetOrdinal("jmlhasil")) ? reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")) : 0),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebudidaya,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,kodeunsur," +
                        "sum(debet-kredit) as nilai FROM[SPDK_KBN_KONSOL].[dbo].[Kartu] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebudidaya,MONTH(tanggal),YEAR(tanggal),norek,kodeunsur";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerKebun
                            {
                                KodeKebun = reader.GetString(reader.GetOrdinal("kodeunit")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebudidaya")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = "KB",
                                KodeAbsen = "1",
                                RealisasiJumlahUpah = (reader.GetString(reader.GetOrdinal("kodeunsur")) == "G" || reader.GetString(reader.GetOrdinal("kodeunsur")) == "U" ? reader.GetDouble(reader.GetOrdinal("nilai")) : 0),
                                RealisasiBiayaLain = (reader.GetString(reader.GetOrdinal("kodeunsur")) != "G" && reader.GetString(reader.GetOrdinal("kodeunsur")) != "U" ? reader.GetDouble(reader.GetOrdinal("nilai")) : 0),
                            });
                        };
                    }
                    conn.Close();
                }

                model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerKebun().Where(o => o.Bulan <= BulanBuku)
                                group m by new
                         {
                             g1 = m.KodeKebun,
                             g2 = m.Tahun,
                             g3 = m.Bulan,
                             g4 = m.KodeBudidaya,
                             g5 = m.KodeRekening
                         }
                            into g
                         select new GajiAbsensiPerKebun
                         {
                             KodeKebun = g.Key.g1,
                             Tahun = g.Key.g2,
                             Bulan = g.Key.g3,
                             KodeBudidaya = g.Key.g4,
                             KodeRekening = g.Key.g5,
                             StatusKaryawan = "KB",
                             KodeAbsen = "1",
                             RealisasiJumlahUpah = g.Where(o => o.KodeUnsur == "G").Sum(o => o.Nilai),
                             RealisasiBiayaLain = g.Where(o => o.KodeUnsur != "G").Sum(o => o.Nilai),
                         }).ToList());
                
                model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerKebun().Where(o=>o.Bulan <= BulanBuku)
                                group m by new
                           {
                               g1 = m.KodeKebun,
                               g2 = m.Tahun,
                               g3 = m.Bulan,
                               g4 = m.KodeBudidaya,
                               g5 = m.KodeRekening,
                               g6 = m.StatusKaryawan
                           }
                           into g
                           select new GajiAbsensiPerKebun
                           {
                               KodeKebun = g.Key.g1,
                               Tahun = g.Key.g2,
                               Bulan = g.Key.g3,
                               KodeBudidaya = g.Key.g4,
                               KodeRekening = g.Key.g5,
                               StatusKaryawan = g.Key.g6,
                               KodeAbsen = "1",
                               RKAPJumlahKaryawan = g.Sum(o => o.JumlahKaryawanRKAP),
                               RKAPJumlahHK = g.Sum(o => o.JumlahHKRKAP),
                               RKAPHasilKerja = g.Sum(o => o.JumlahHasilKerjaRKAP),
                               RKAPJumlahUpah = g.Sum(o => o.NilaiRKAP),
                               PKBJumlahKaryawan = g.Sum(o => o.JumlahKaryawanPKB),
                               PKBJumlahHK = g.Sum(o => o.JumlahHKPKB),
                               PKBHasilKerja = g.Sum(o => o.JumlahHasilKerjaPKB),
                               PKBJumlahUpah = g.Sum(o => o.NilaiPKB),
                           }).ToList());
                return model;
            }
            catch
            {
                return new List<GajiAbsensiPerKebun>();
            }
        }

        public List<GajiAbsensiPerAfdeling> ReadPerAfdeling()
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            CRUDKartu.CRUD.TahunBuku = TahunBuku;
            CRUDAkunRKAP.CRUD.TahunBuku = TahunBuku;
            CRUDAkunMemorial.CRUD.TahunBuku = TahunBuku;
            try
            {

                List<GajiAbsensiPerAfdeling> model = new List<GajiAbsensiPerAfdeling>();
                string strQry = "SELECT kodeunit,kodebud1,kodeafd1,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts,kodeabs,kdpanen,grup, " +
                        "Count(*) as RealisasiJumlahHK, SUM(hslpanen) as hslpanen, sum(hslpemel) as hslpemel, sum(hsllain2) as hsllain2, SUM(hslpanenlump) as hslpanenlump, " +
                        "SUM(hslpanenTBS) as hslpanenTBS, SUM(jelajahHA) as jelajah  FROM[SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud1,kodeafd1,MONTH(tanggal),YEAR(tanggal),norek,sts,kodeabs,kdpanen,grup";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerAfdeling
                            {
                                KodeAfdeling = reader.GetString(reader.GetOrdinal("kodeunit"))+ reader.GetString(reader.GetOrdinal("kodeafd1")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud1")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                KodeAbsen = reader.GetString(reader.GetOrdinal("kodeabs")),
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("hslpemel")) + reader.GetDouble(reader.GetOrdinal("hsllain2")) +
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "1" || reader.GetString(reader.GetOrdinal("kdpanen")) == "" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaLump = reader.GetDouble(reader.GetOrdinal("hslpanenlump")) +
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "2" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaTandan = reader.GetDouble(reader.GetOrdinal("hslpanenTBS")),
                                RealisasiJelajah = reader.GetDouble(reader.GetOrdinal("jelajah")),
                                GrupPanen = reader.GetString(reader.GetOrdinal("grup")),
                            });
                        };
                    }
                    conn.Close();
                }
                strQry = "SELECT kodeunit,kodebud,kodeafd,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts," +
                        "Count(*) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiKulir] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud,kodeafd,MONTH(tanggal),YEAR(tanggal),norek,sts";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerAfdeling
                            {
                                KodeAfdeling = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                KodeAbsen = "1",
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebud,kodeafd,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,NoSPK," +
                        "sum(jmlhadir) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiSPKHonor] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud,kodeafd,MONTH(tanggal),YEAR(tanggal),norek,NoSPK";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerAfdeling
                            {
                                KodeAfdeling = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = "KL",
                                KodeAbsen = "1",
                                RealisasiJumlahHK = (Int32)(reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")) != reader.GetDouble(reader.GetOrdinal("jmlhasil")) ? reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")) : 0),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebudidaya,kodeafdeling,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,kodeunsur," +
                        "sum(debet-kredit) as nilai FROM[SPDK_KBN_KONSOL].[dbo].[Kartu] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebudidaya,kodeafdeling,MONTH(tanggal),YEAR(tanggal),norek,kodeunsur";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerAfdeling
                            {
                                KodeAfdeling = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafdeling")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebudidaya")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = "KB",
                                KodeAbsen = "1",
                                RealisasiJumlahUpah = (reader.GetString(reader.GetOrdinal("kodeunsur")) == "G" || reader.GetString(reader.GetOrdinal("kodeunsur")) == "U" ? reader.GetDouble(reader.GetOrdinal("nilai")) : 0),
                                RealisasiBiayaLain = (reader.GetString(reader.GetOrdinal("kodeunsur")) != "G" && reader.GetString(reader.GetOrdinal("kodeunsur")) != "U" ? reader.GetDouble(reader.GetOrdinal("nilai")) : 0),
                            });
                        };
                    }
                    conn.Close();
                }
                
                model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerAfdeling().Where(o => o.Bulan <= BulanBuku)
                                group m by new
                         {
                             g1 = m.KodeAfdeling,
                             g2 = m.Tahun,
                             g3 = m.Bulan,
                             g4 = m.KodeBudidaya,
                             g5 = m.KodeRekening
                         }
                            into g
                         select new GajiAbsensiPerAfdeling
                         {
                             KodeAfdeling = g.Key.g1,
                             Tahun = g.Key.g2,
                             Bulan = g.Key.g3,
                             KodeBudidaya = g.Key.g4,
                             KodeRekening = g.Key.g5,
                             StatusKaryawan = "KB",
                             KodeAbsen = "1",
                             RealisasiJumlahUpah = g.Where(o => o.KodeUnsur == "G").Sum(o => o.Nilai),
                             RealisasiBiayaLain = g.Where(o => o.KodeUnsur != "G").Sum(o => o.Nilai),
                         }).ToList());

                model.AddRange((from m in CRUDAkunRKAP.CRUD.GetRKAPPerAfdeling().Where(o => o.Bulan <= BulanBuku)
                                group m by new
                           {
                               g1 = m.KodeAfdeling,
                               g2 = m.Tahun,
                               g3 = m.Bulan,
                               g4 = m.KodeBudidaya,
                               g5 = m.KodeRekening,
                               g6 = m.StatusKaryawan
                           }
                           into g
                           select new GajiAbsensiPerAfdeling
                           {
                               KodeAfdeling = g.Key.g1,
                               Tahun = g.Key.g2,
                               Bulan = g.Key.g3,
                               KodeBudidaya = g.Key.g4,
                               KodeRekening = g.Key.g5,
                               StatusKaryawan = g.Key.g6,
                               KodeAbsen = "1",
                               //KodePanen = "1",

                               RKAPJumlahKaryawan = g.Sum(o => o.JumlahKaryawanRKAP),
                               RKAPJumlahHK = g.Sum(o => o.JumlahHKRKAP),
                               RKAPHasilKerja = g.Sum(o => o.JumlahHasilKerjaRKAP),
                               RKAPJumlahUpah = g.Sum(o => o.NilaiRKAP),

                               PKBJumlahKaryawan = g.Sum(o => o.JumlahKaryawanPKB),
                               PKBJumlahHK = g.Sum(o => o.JumlahHKPKB),
                               PKBHasilKerja = g.Sum(o => o.JumlahHasilKerjaPKB),
                               PKBJumlahUpah = g.Sum(o => o.NilaiPKB),
                           }).ToList());

                return model; 
            }
            catch 
            {
                return new List<GajiAbsensiPerAfdeling>();
            }
        }

        public List<GajiAbsensiPerBlok> ReadPerBlok()
        {
            string cs = System.Configuration.ConfigurationManager.ConnectionStrings["csKonsol"].ConnectionString;
            KonsolidasiDbContext db = new KonsolidasiDbContext();
            CRUDKartu.CRUD.TahunBuku = TahunBuku;
            CRUDAkunRKAP.CRUD.TahunBuku = TahunBuku;
            CRUDAkunMemorial.CRUD.TahunBuku = TahunBuku;
            try
            {
                List<GajiAbsensiPerBlok> model = new List<GajiAbsensiPerBlok>();
                string strQry = "SELECT kodeunit,kodebud1,kodeafd1,kdblok,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts,kodeabs,kdpanen,thntnm, grup, " +
                        "Count(*) as RealisasiJumlahHK, SUM(jmlupah) as jmlupah, SUM(hslpanen) as hslpanen, sum(hslpemel) as hslpemel, sum(hsllain2) as hsllain2, SUM(hslpanenlump) as hslpanenlump, " +
                        "SUM(hslpanenTBS) as hslpanenTBS, sum(jelajahHA) as Jelajah FROM[SPDK_KBN_KONSOL].[dbo].[GajiAbsensi] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and LEN(kdblok)=4 and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud1,kodeafd1,kdblok,MONTH(tanggal),YEAR(tanggal),norek,sts,kodeabs,kdpanen,thntnm,grup";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerBlok
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd1")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud1")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                KodeAbsen = reader.GetString(reader.GetOrdinal("kodeabs")),
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("hslpemel")) + reader.GetDouble(reader.GetOrdinal("hsllain2")) +
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "1" || reader.GetString(reader.GetOrdinal("kdpanen")) == "" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaLump = reader.GetDouble(reader.GetOrdinal("hslpanenlump")) +
                                    (reader.GetString(reader.GetOrdinal("kdpanen")) == "2" ? reader.GetDouble(reader.GetOrdinal("hslpanen")) : 0),
                                RealisasiHasilKerjaTandan = reader.GetDouble(reader.GetOrdinal("hslpanenTBS")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                RealisasiJumlahUpah = reader.GetDouble(reader.GetOrdinal("jmlupah")),
                                RealisasiJelajah = reader.GetDouble(reader.GetOrdinal("jelajah")),
                                GrupPanen = reader.GetString(reader.GetOrdinal("grup")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebud,kodeafd,kdblok,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,sts,thntnm," +
                         "sum(nilai) as nilai, Count(*) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiKulir] " +
                         "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(kdblok)=4 and len(rtrim(norek))=9 " +
                         "group by kodeunit,kodebud,kodeafd,kdblok,MONTH(tanggal),YEAR(tanggal),norek,sts,thntnm";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerBlok
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = reader.GetString(reader.GetOrdinal("sts")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                KodeAbsen = "1",
                                RealisasiJumlahUpah = reader.GetDouble(reader.GetOrdinal("nilai")),
                                RealisasiJumlahHK = reader.GetInt32(reader.GetOrdinal("RealisasiJumlahHK")),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebud,kodeafd,kdblok,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,thntnm," +
                        "sum(jmlupah) as jmlupah,sum(jmlhadir) as RealisasiJumlahHK, SUM(jmlhasil) as jmlhasil FROM[SPDK_KBN_KONSOL].[dbo].[GajiSPKHonor] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(kdblok)=4 and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebud,kodeafd,kdblok,MONTH(tanggal),YEAR(tanggal),norek,thntnm";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerBlok
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafd")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebud")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                TahunTanam = reader.GetString(reader.GetOrdinal("thntnm")),
                                StatusKaryawan = "KL",
                                KodeAbsen = "1",
                                RealisasiJumlahUpah = reader.GetDouble(reader.GetOrdinal("jmlupah")),
                                RealisasiJumlahHK = (Int32)(reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")) != reader.GetDouble(reader.GetOrdinal("jmlhasil")) ? reader.GetDouble(reader.GetOrdinal("RealisasiJumlahHK")) : 0),
                                RealisasiHasilKerja = reader.GetDouble(reader.GetOrdinal("jmlhasil")),
                            });
                        };
                    }
                    conn.Close();
                }

                strQry = "SELECT kodeunit,kodebudidaya,kodeafdeling,kdblok,month(tanggal) as Bulan,YEAR(tanggal) as Tahun,norek,kodeunsur," +
                        "sum(debet-kredit) as nilai FROM[SPDK_KBN_KONSOL].[dbo].[Kartu] " +
                        "where month(tanggal)<=" + BulanBuku.ToString() + " and year(tanggal)=" + TahunBuku.ToString() + " and len(kdblok)=4 and len(rtrim(norek))=9 " +
                        "group by kodeunit,kodebudidaya,kodeafdeling,kdblok,MONTH(tanggal),YEAR(tanggal),norek,kodeunsur";
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = strQry;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Add(new GajiAbsensiPerBlok
                            {
                                KodeBlok = reader.GetString(reader.GetOrdinal("kodeunit")) + reader.GetString(reader.GetOrdinal("kodeafdeling")) + reader.GetString(reader.GetOrdinal("kdblok")),
                                KodeBudidaya = reader.GetString(reader.GetOrdinal("kodebudidaya")),
                                Bulan = reader.GetInt32(reader.GetOrdinal("bulan")),
                                Tahun = reader.GetInt32(reader.GetOrdinal("tahun")),
                                KodeRekening = reader.GetString(reader.GetOrdinal("norek")),
                                StatusKaryawan = "KB",
                                KodeAbsen = "1",
                                RealisasiBiayaLain = (reader.GetString(reader.GetOrdinal("kodeunsur")) != "G" && reader.GetString(reader.GetOrdinal("kodeunsur")) != "U" ? reader.GetDouble(reader.GetOrdinal("nilai")) : 0),
                            });
                        };
                    }
                    conn.Close();
                }

                model.AddRange((from m in CRUDAkunMemorial.CRUD.GetSaldoPerBlok().Where(o => o.Bulan <= BulanBuku)
                                group m by new
                         {
                             g1 = m.KodeBlok,
                             g2 = m.Tahun,
                             g3 = m.Bulan,
                             g4 = m.KodeBudidaya,
                             g5 = m.KodeRekening
                         }
                         into g
                         select new GajiAbsensiPerBlok
                         {
                             KodeBlok = g.Key.g1,
                             Tahun = g.Key.g2,
                             Bulan = g.Key.g3,
                             KodeBudidaya = g.Key.g4,
                             KodeRekening = g.Key.g5,
                             StatusKaryawan = "KB",
                             KodeAbsen = "1",
                             RealisasiJumlahUpah = g.Where(o => o.KodeUnsur == "G").Sum(o => o.Nilai),
                             RealisasiBiayaLain = g.Where(o => o.KodeUnsur != "G").Sum(o => o.Nilai),
                         }).ToList());


                return model;
            }
            catch
            {
                return new List<GajiAbsensiPerBlok>();
            }
        }

        public List<GajiAbsensiPerKebun> GetRecordPerKebun()
        {
            var model = ListGajiAbsensiPerKebun;
            return model;
        }

        public List<GajiAbsensiPerAfdeling> GetRecordPerAfdeling(string kodeKebun="")
        {
            var model = ListGajiAbsensiPerAfdeling.Where(o=>o.KodeAfdeling.Substring(0,2)==kodeKebun).ToList();
            return model;
        }

        public List<GajiAbsensiPerBlok> GetRecordPerBlok(string kodeAfdeling="")
        {
            var model = ListGajiAbsensiPerBlok.Where(o=>o.KodeBlok.Substring(0,4)==kodeAfdeling).ToList();
            return model;
        }

    }
}