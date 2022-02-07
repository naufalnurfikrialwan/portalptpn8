using Ptpn8.Areas.Konsolidasi.Models.CRUD;
using Ptpn8.Areas.Referensi.Models;
using Ptpn8.Areas.Referensi.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Areal.Models.CRUD
{
    public class CRUDDataViewKebun
    {
        public static CRUDDataViewKebun CRUD = new CRUDDataViewKebun();

        public List<DataViewKebun> ListDataViewKebun
        {
            get
            {
                List<DataViewKebun> result = (List<DataViewKebun>)HttpContext.Current.Session["ListDataViewKebun"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListDataViewKebun"] = result = Read();
                }
                return result;
            }
        }
        public List<DataViewKebun> Read()
        {
            try
            {
                List<DataViewKebun> model = new List<DataViewKebun> { };
                string scriptSQL = "exec CreateDataViewKebun "+DateTime.Now.Year.ToString();
                var modelx = ExecSQL(scriptSQL);
                DataViewKebun mm;
                for (int i=0;i<modelx.Count;i++)
                {
                    mm = new DataViewKebun();
                    mm.Id = Guid.Parse((string)modelx[i][0]);
                    mm.Wilayah = (string)modelx[i][1];
                    mm.Nama = (string)modelx[i][2];
                    mm.DaftarBudidaya = new List<string> { (string)modelx[i][3] };
                    mm.DaftarTahunTanam = new List<string> { (string)modelx[i][4] };
                    mm.DaftarSKHGU = new List<string> { (string)modelx[i][5] };
                    mm.DaftarStatusAreal = new List<string> { (string)modelx[i][6] };
                    mm.TanggalBerlaku = new List<string> { (string)modelx[i][7] };
                    mm.TanggalBerakhir = new List<string> { (string)modelx[i][8] };
                    mm.LuasAreal = Decimal.Parse((string)modelx[i][9]);
                    mm.SisaWaktu = new List<int> { };
                    model.Add(mm);
                }

                //CRUDBlokRealisasi.CRUD.TahunBuku = DateTime.Now.Year;
                //var model = (from blkR in CRUDBlokRealisasi.CRUD.GetAllRecord()
                //             join hgu in CRUDHGU.CRUD.GetAllRecord() on blkR.HGUId equals hgu.HGUId
                //             group blkR by new
                //             {
                //                 a = blkR.Blok.Afdeling.KebunId,
                //                 b = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).Nama,
                //                 c = CRUDStatusAreal.CRUD.Get1Record(blkR.StatusArealId).Nama,
                //                 //d=hgu.SisaWaktu,
                //                 e = blkR.Blok.kd_blok.Substring(0, 2),
                //                 f = CRUDBudidaya.CRUD.Get1Record(blkR.MainBudidayaId).kd_bud
                //             } into g
                //             select new DataViewKebun
                //             {
                //                 Id = g.Key.a,
                //                 Wilayah = g.Select(o => o.Blok.Afdeling.Kebun.Wilayah.Nama).FirstOrDefault(),
                //                 Nama = g.Select(o => o.Blok.Afdeling.Kebun.Nama).FirstOrDefault(),
                //                 DaftarBudidaya = new List<string> { g.Key.b }, // + " "},
                //                 DaftarTahunTanam = g.Where(o => o.Blok.TahunTanam.Trim() != "").Select(o => o.Blok.TahunTanam + " ").ToList(),
                //                 DaftarSKHGU = g.Where(o => o.HGU.No_SK.Trim() != "").Select(o => o.HGU.No_SK + " ").ToList(),
                //                 DaftarStatusAreal = { g.Key.c },
                //                 TanggalBerlaku = g.Where(o => o.HGU.TanggalTerbit.Year != 1900).Select(o => o.HGU.TanggalTerbit.ToString("dd/MM/yyyy") + " ").ToList(),
                //                 TanggalBerakhir = g.Where(o => o.HGU.TanggalBerakhir.Year != 1900).Select(o => o.HGU.TanggalBerakhir.ToString("dd/MM/yyyy") + " ").ToList(),
                //                 SisaWaktu = g.Select(o => o.HGU.SisaWaktu).ToList(), //new List<int> { g.Key.d },
                //                                                                      //TM = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e,g.Key.f,"TM"),
                //                                                                      ////g.Where(o => o.StatusAreal == "1.1").Sum(o => o.LuasAreal),
                //                                                                      //TBM = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TBM"),
                //                                                                      ////g.Where(o => o.StatusAreal == "1.2").Sum(o => o.LuasAreal),
                //                                                                      //TTI = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TTI"),
                //                                                                      ////g.Where(o => o.StatusAreal == "1.3").Sum(o => o.LuasAreal),
                //                                                                      //TTAD = g.Where(o => o.StatusAreal == "1.4").Sum(o => o.LuasAreal),
                //                                                                      //Pesemaian = CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "Pesemaian"),
                //                                                                      ////g.Where(o => o.StatusAreal == "1.5").Sum(o => o.LuasAreal),
                //                                                                      //Entrys = g.Where(o => o.StatusAreal == "1.6").Sum(o => o.LuasAreal),
                //                                                                      //Reboisasi = g.Where(o => o.StatusAreal == "1.7").Sum(o => o.LuasAreal),
                //                                                                      //Monocrop = g.Where(o => o.StatusAreal == "1.8").Sum(o => o.LuasAreal),
                //                                                                      //Intercrop = g.Where(o => o.StatusAreal == "1.9").Sum(o => o.LuasAreal),
                //                                                                      //TPJ = g.Where(o => o.StatusAreal == "1.10").Sum(o => o.LuasAreal),
                //                                                                      //Emplasemen = g.Where(o => o.StatusAreal == "2.1").Sum(o => o.LuasAreal),
                //                                                                      //Jalan = g.Where(o => o.StatusAreal == "2.2").Sum(o => o.LuasAreal),
                //                                                                      //Fasos = g.Where(o => o.StatusAreal == "2.3").Sum(o => o.LuasAreal),
                //                                                                      //Marginal = g.Where(o => o.StatusAreal == "2.4").Sum(o => o.LuasAreal),
                //                                                                      //CadanganMurni = g.Where(o => o.StatusAreal == "2.5.1").Sum(o => o.LuasAreal),
                //                                                                      //CadanganPokok = g.Where(o => o.StatusAreal == "2.5.2").Sum(o => o.LuasAreal),
                //                                                                      //Agrowisata = g.Where(o => o.StatusAreal == "2.6").Sum(o => o.LuasAreal),
                //                                                                      //KerjasamaBisnis = g.Where(o => o.StatusAreal == "3.1").Sum(o => o.LuasAreal),
                //                                                                      //PinjamPakai = g.Where(o => o.StatusAreal == "3.2").Sum(o => o.LuasAreal),
                //                                                                      //Okupasi = g.Where(o => o.StatusAreal == "4.1").Sum(o => o.LuasAreal),
                //                 LuasAreal = g.Key.c == "1.1" ? CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TM") :
                //                      g.Key.c == "1.2" ? CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TBM") :
                //                      g.Key.c == "1.3" ? CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "TTI") :
                //                      g.Key.c == "1.5" ? CRUDRef_Areal.CRUD.HitungLuasArealKebun(g.Key.e, g.Key.f, "PESEMAIAN") :
                //                      g.Sum(o => o.LuasAreal)
                //             }).ToList();

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
                return model.ToList();
            }
            catch (Exception x)
            {
                return new List<DataViewKebun>();
            }
        }

        public List<DataViewKebun> GetAllRecord()
        {
            var model = ListDataViewKebun;
            return model;
        }

        public List<object[]> ExecSQL(string scriptSQL)
        {
            List<object[]> Results = new List<object[]>();
            string arrCS = System.Configuration.ConfigurationManager.ConnectionStrings["csReferensi"].ConnectionString;
            SqlConnection con = new SqlConnection(arrCS);
            try
            {
                con.Open();
                DataTable tap = new DataTable();
                new SqlDataAdapter(scriptSQL, con).Fill(tap);
                for (int i = 0; i < tap.Rows.Count; i++)
                {
                    object[] result = new object[tap.Columns.Count];
                    DataRow dr = tap.Rows[i];
                    for (int j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (dr[j].GetType().Name == "Byte[]")
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.Current.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.Current.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                        }
                        else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.Current.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                        }
                        result[j] = dr[j].ToString();
                    }
                    Results.Add(result);
                }
            }
            catch (Exception ex)
            {
                //Exception   
            }
            finally
            {
                if (con != null)
                    con.Close();
            }
            return Results;
        }


    }
}