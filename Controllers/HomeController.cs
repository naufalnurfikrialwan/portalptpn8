using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models.CRUD;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    //[RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index(string aplikasi="")
        {
            string userName = User.Identity.Name;
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Get1Record(userName);
            if (current != null)
            {
                string listViewName = current.MenuId + "_" + current.PosisiLokasiKerjaId;
                List<object[]> lstResult = (List<object[]>)HttpContext.Application[listViewName];
                if (lstResult != null)
                {
                    object[] objResult = new object[3];
                    foreach (var listView in lstResult)
                    {
                        if ((int)listView[1] == current.TahunBuku)
                        {
                            objResult = listView;
                            break;
                        }
                    }
                    if ((string)objResult[0] == userName)
                    {
                        objResult[0] = null;
                        objResult[2] = null;
                    }
                }
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Erase(current);
            }

            if (userName != "" || (userName!="" && current==null))
            {
                current = new Ptpn8.Models.PenggunaPortalYangAktif();
                current.PenggunaPortalYangAktifId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Id);
                current.UserName = userName;
                current.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Nama;
                current.LokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).LokasiKerjaId;
                current.PosisiLokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).PosisiLokasiKerjaId;
                current.Tanggal = DateTime.Now;
                current.AplikasiId = Guid.Empty;
                current.Aplikasi = "Login";
                current.MenuId = Guid.Empty;
                current.Menu = "Login";
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);
                var lokasi = ExecSQL("select NamaUnit from [ReferensiEF]..Unit where OrgId=(select posisilokasikerjaid from [Identity]..[AspNetUsers] where username='" + User.Identity.Name + "')");
                ViewBag.LokasiKerja = (string)lokasi[0][0];
                var jmlUltah = ExecSQL("declare @bulan int; declare @tahun int; set @bulan = month(GETDATE()); set @tahun = year(GETDATE()); if @bulan-1 = 0 begin set @bulan = 12; set @tahun = @tahun-1 end else begin set @bulan=@bulan-1 end; SELECT count(*) FROM [ERP].[dbo].[CheckRoll_DIK_SAP] where bulan=@bulan and tahun=@tahun and MONTH([Birth date])=MONTH(GETDATE()) and DAY([Birth date])=DAY(GETDATE()) and [PS group] in ('IIIA','IIIB','IIIC','IIID','IVA','IVB','IVC','IVD')");
                ViewBag.JumlahYangUltah = (string)jmlUltah[0][0];
            }
            else
            {
                ViewBag.JumlahYangUltah = "0";
            }


            if (aplikasi == "")
            {
                ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("PerjanjianKerjasama");
                return View("PerjanjianKerjasama");
            }
            else
            {
                ViewBag.Menu = CRUDMenu.CRUD.BuildMenu(aplikasi);
                return View(aplikasi);
            }

        }

        public ActionResult About()
        {
            ViewBag.Message = "";
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("PerjanjianKerjasama");
            try
            {
                var lokasi = ExecSQL("select NamaUnit from [ReferensiEF]..Unit where OrgId=(select posisilokasikerjaid from [Identity]..[AspNetUsers] where username='" + User.Identity.Name + "')");
                ViewBag.LokasiKerja = (string)lokasi[0][0];
            }
            catch
            {
                ViewBag.LokasiKerja = "";
            }
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            ViewBag.Menu = CRUDMenu.CRUD.BuildMenu("PerjanjianKerjasama");
            try
            {
                var lokasi = ExecSQL("select NamaUnit from [ReferensiEF]..Unit where OrgId=(select posisilokasikerjaid from [Identity]..[AspNetUsers] where username='" + User.Identity.Name + "')");
                ViewBag.LokasiKerja = (string)lokasi[0][0];
            }
            catch
            {
                ViewBag.LokasiKerja = "";
            }

            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult readPenggunaPortal([DataSourceRequest] DataSourceRequest request)
        {
            List<Ptpn8.Models.PenggunaPortalYangAktif> isiHttp = (List<Ptpn8.Models.PenggunaPortalYangAktif>)HttpContext.Application["PenggunaPortalYangAktif"];
            List<Ptpn8.Models.PenggunaPortalYangAktif> result = new List<Ptpn8.Models.PenggunaPortalYangAktif>();
            if (isiHttp != null)
            {
                foreach (var m in isiHttp)
                {
                    if (m.UserName != HttpContext.User.Identity.Name) result.Add(m);
                }
            }
            return Json(result.ToDataSourceResult(request));
        }

        public List<object[]> ExecSQL(string scriptSQL)
        {
            List<object[]> Results = new List<object[]>();

            string connStr = ConfigurationManager.ConnectionStrings["csIdentity"].ConnectionString;
            string[] arrCS = new string[] { connStr };
            if (scriptSQL.ToLower().IndexOf("delete ") >= 0 ||
                scriptSQL.ToLower().IndexOf("drop tab") >= 0
                )
            {
                return Results;
            }

            SqlConnection con = new SqlConnection(arrCS[0]);
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
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                        }
                        else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
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

        [Authorize]
        [System.Web.Mvc.HttpPost]
        public ActionResult ExecSQL1(string scriptSQL, string _menuId)
        {
            List<object[]> Results = new List<object[]>();

            string connStr = ConfigurationManager.ConnectionStrings["csIdentity"].ConnectionString;
            string[] arrCS = new string[] { connStr };
            if (scriptSQL.ToLower().IndexOf("delete ") >= 0 ||
                scriptSQL.ToLower().IndexOf("drop tab") >= 0
                )
            {
                return Json(Results, JsonRequestBehavior.AllowGet);
            }

            SqlConnection con = new SqlConnection(arrCS[0]);
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
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg", (byte[])dr[j]);
                        }
                        else if (tap.Columns[j].ToString().ToLower().Contains("img"))
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
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
            return Json(Results, JsonRequestBehavior.AllowGet);
        }


    }
}