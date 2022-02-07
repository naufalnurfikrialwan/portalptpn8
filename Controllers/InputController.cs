
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models;
using Ptpn8.Models.CRUD;
using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;

namespace Ptpn8.Controllers
{
    public class InputController : Controller
    {
        private string msgInternalError = "Internal Error, Hubungi Bagian TI";
        [Authorize]
        public ActionResult Input(string aplikasiId = "", string menuId = "", string scriptSQL="")
        {
            string userName = HttpContext.User.Identity.Name;
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Get1Record(userName);
            var mdlUser = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName);
            if (current == null)
            {
                current = new Ptpn8.Models.PenggunaPortalYangAktif();
            }
            else
            {
                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", menuId).Replace("[POSISILOKASIKERJAID]", mdlUser.PosisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", mdlUser.LokasiKerjaId.ToString()).Replace("[USERNAME]", userName);

                //string listViewName = current.MenuId+ "_" + current.PosisiLokasiKerjaId;
                List<object[]> lstResult = (List<object[]>)HttpContext.Application[listViewName];
                if(lstResult!=null)
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
                    if((string)objResult[0]==userName)
                    {
                        objResult[0] = null;
                        objResult[2] = null;
                    }
                }
            }

            if (aplikasiId == "") aplikasiId = Guid.Empty.ToString();
            if (menuId == "") menuId = Guid.Empty.ToString();
            Dictionary<string, object> _app = new Dictionary<string, object>();

            _app = InisiasiInput(aplikasiId, menuId, current.PosisiLokasiKerjaId.ToString());
            if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() != "Success")
            {
                return View("Error", new List<string>() { { "Aplikasi Id: " + aplikasiId }, { "Menu Id: " + menuId }, { " ada masalah!" + " Message: " + _app.FirstOrDefault(o => o.Key == "Message").Value.ToString() } });
            }
            else
            {
                //Guid lokasiKerja = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(HttpContext.User.Identity.Name).LokasiKerjaId;
                //List<LokasiKerja> daftarSeluruhLokasiKerja = Ptpn8.Areas.Referensi.Models.CRUD.CRUDLokasiKerja.CRUD.getFlatData();
                //List<LokasiKerja> daftarLokasiKerja = daftarSeluruhLokasiKerja.Where(o => o.BagianId == lokasiKerja)
                //    .Union(daftarSeluruhLokasiKerja.Where(o => o.KebunId == lokasiKerja))
                //    .Union(daftarSeluruhLokasiKerja.Where(o => o.WilayahId == lokasiKerja))
                //    .Where(o =>(o.UrusanId != Guid.Empty && o.BagianId==lokasiKerja) || (o.BidangId != Guid.Empty && o.WilayahId==lokasiKerja) || (o.AfdelingId != Guid.Empty && o.KebunId==lokasiKerja))
                //    .ToList();
                ViewBag.AlamatIPLocal = GetLocalIPAddress();
                ViewBag.AlamatIPPublic = GetPublicIPAddress();

                current.PenggunaPortalYangAktifId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Id);
                current.UserName = userName;
                current.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).Nama;
                current.LokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).LokasiKerjaId;
                current.PosisiLokasiKerjaId = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName).PosisiLokasiKerjaId;
                current.Tanggal = DateTime.Now;
                current.AplikasiId = Guid.Parse(aplikasiId);
                current.Aplikasi = _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString();
                current.MenuId = Guid.Parse(menuId);
                current.Menu = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();
                current.StatusBaru = true;
                var xxx = _app.FirstOrDefault(o => o.Key == "ModelViewHeader").Value;
                if (xxx != null)
                {
                    current.TahunBuku = xxx.GetType().GetProperty("TahunBuku") != null ? (int)xxx.GetType().GetProperty("TahunBuku").GetValue(xxx):DateTime.Now.Year;
                }
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);

                if (scriptSQL != "")
                {
                    scriptSQL = scriptSQL.Replace('!', '+');
                    string strClassView = _app.FirstOrDefault(o => o.Key == "strClassHeaderView").Value.ToString();
                    var mdl = getDataFrom(strClassView, scriptSQL, menuId);
                    return View(@"\" + _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString() +
                        @"\" + _app.FirstOrDefault(o => o.Key == "NamaView").Value.ToString(),mdl[0]);
                }
                else
                {
                    return View(@"\" + _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString() +
                        @"\" + _app.FirstOrDefault(o => o.Key == "NamaView").Value.ToString(),
                        (_app.FirstOrDefault(o => o.Key == "ModelViewHeader").Value != null ? _app.FirstOrDefault(o => o.Key == "ModelViewHeader").Value : _app.FirstOrDefault(o => o.Key == "ModelViewDetail").Value));
                }
            }
        }

        public Dictionary<string, object> InisiasiInput(string aplikasiId, string menuId, string posisiLokasiKerjaId)
        {
            string userName = HttpContext.User.Identity.Name;
            var mdlUser = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName);
            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", menuId).Replace("[POSISILOKASIKERJAID]", mdlUser.PosisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", mdlUser.LokasiKerjaId.ToString()).Replace("[USERNAME]", userName);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];
            if (_app == null)
            {
                _app = readApp(aplikasiId, menuId, posisiLokasiKerjaId);
                if (_app.FirstOrDefault(o => o.Key == "Message").Value.ToString() == "Success")
                {
                    //HttpContext.Application["Input" + menuId + posisiLokasiKerjaId] = _app;
                    HttpContext.Application["Input" + listViewName] = _app;
                }
            }

            var riwayatAkses = new RiwayatAkses
            {
                UserName = HttpContext.User.Identity.Name,
                TanggalAkses = DateTime.Now,
                PageYangDiakses = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString(),
                Id = Guid.Parse(_app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString())
            };
            RiwayatAkses.CRUDRiwayatAkses.CRUD.Update(riwayatAkses);

            ViewBag.Menu = (from m in CRUDMenu.CRUD.GetAllRecord().Where(o => o.AplikasiId == Guid.Parse(aplikasiId)) select m).OrderBy(o => o.KodeMenu).ToList();
            ViewBag.Title = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();
            ViewBag.LokasiKerja = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerja;
            ViewBag.Message = "";
            ViewBag.MenuId = _app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString();
            return _app;
        }
        public Dictionary<string, object> readApp(string aplikasiId, string menuId, string posisiLokasiKerjaId)
        {

            string userName = HttpContext.User.Identity.Name;
            var mdlUser = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(userName);

            Type Ttools, T;
            object classToolsHeader, classCRUDViewHeader, classInisiasiHeader;
            object classToolsDetail, classCRUDViewDetail, classInisiasiDetail;
            object ModelViewHeader, ModelViewDetail;

            Dictionary<string, object> result = new Dictionary<string, object>();
            HttpContext.Session["LOKASIKEBUNID"] = CRUDApplicationUser.CRUD.Get1Record(User.Identity.Name).PosisiLokasiKerjaId; // CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId;

            var menuAplikasi = from m in CRUDMenu.CRUD.GetAllRecord().Where(o => o.AplikasiId == Guid.Parse(aplikasiId)) select m;
            if (menuAplikasi == null)
            {
                result.Add("Message", "Tidak Ditemukan!");
                return result;
            }
            var menu = (from m in menuAplikasi
                        join n in CRUDAplikasi.CRUD.GetAllRecord() on m.AplikasiId equals n.AplikasiId into g
                        where m.AplikasiId == Guid.Parse(aplikasiId) && m.MenuId == Guid.Parse(menuId)
                        select new
                        {
                            classHeaderView = m.classHeaderView,
                            classHeaderTable = m.classHeaderTable,
                            ARRclassDetailView = m.classDetailView.ToString().Split(';'),
                            ARRclassDetailTable = m.classDetailTable.ToString().Split(';'),
                            NamaAplikasi = g.Select(o => o.NamaAplikasi).FirstOrDefault(),
                            NamaModul = m.LinkText,
                            ARRConnectionString = m.ConnectionString.ToString().Split(';'),
                            NamaTabelHeader = m.NamaTabelHeader,
                            ARRNamaTabelDetail = m.NamaTabelDetail.ToString().Split(';'),
                            NamaView = m.NamaView,
                            FieldTahunBuku = m.FieldTahunBuku,
                            FieldNomorInput = m.FieldNomorInput,
                            FieldNomorInputView = m.FieldNomorInputView,
                            ARRFieldKeyHeader = m.FieldKeyHeader.ToString().Split(';'),
                            ARRFieldKeyDetail = m.FieldKeyDetail.ToString().Split(';'),
                            bolehBuatNoBaru = m.bolehBuatBaru,
                            ListViewName = m.ListViewName.Replace("[MENUID]", menuId).Replace("[POSISILOKASIKERJAID]", mdlUser.PosisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", mdlUser.LokasiKerjaId.ToString()).Replace("[USERNAME]", userName), //menuId + "_" + posisiLokasiKerjaId,
                            ARRNamaHttpContext = m.NamaHttpContext != null ? m.NamaHttpContext.ToString().Split(';') : null,
                            PemilikAplikasi = g.First().Pemilik.ToLower(),
                            ConnectionStringServerLain = m.ConnectionStringServerLain.ToString().Split(';'),
                            ScriptTriggerHeaderServerLain = m.ScriptTriggerHeaderServerLain,
                            ScriptTriggerDetailServerLain = m.ScriptTriggerDetailServerLain
                        }).FirstOrDefault();

            if (menu == null)
            {
                result.Add("Message", "Tidak Ditemukan!");
                return result;
            }

            string[] arrcs = new string[menu.ARRConnectionString.Length];
            string[] arrcsServerLain = new string[menu.ConnectionStringServerLain.Length];

            for (int j = 0; j < menu.ARRConnectionString.Length; j++)
            {
                arrcs[j] = System.Configuration.ConfigurationManager.ConnectionStrings[menu.ARRConnectionString[j]].ConnectionString;
            }

            if (menu.ConnectionStringServerLain.Length == 1 && menu.ConnectionStringServerLain[0] == "")
            {
                arrcsServerLain = new string[] { };
            }
            else
            {
                for (int j = 0; j < menu.ConnectionStringServerLain.Length; j++)
                {
                    if (menu.ConnectionStringServerLain[j] != "")
                    {
                        arrcsServerLain[j] = System.Configuration.ConfigurationManager.ConnectionStrings[menu.ConnectionStringServerLain[j]].ConnectionString;
                    }
                }
            }
            result.Add("AplikasiId", aplikasiId);
            result.Add("MenuId", menuId);
            result.Add("cs", arrcs);
            result.Add("strClassHeaderView", menu.classHeaderView.Trim());
            result.Add("strClassHeaderTable", menu.classHeaderTable.Trim());
            result.Add("ARRClassDetailView", menu.ARRclassDetailView);
            result.Add("ARRClassDetailTable", menu.ARRclassDetailTable);
            result.Add("NamaAplikasi", menu.NamaAplikasi.Trim());
            result.Add("NamaModul", menu.NamaModul.Trim());
            result.Add("NamaTabelHeader", menu.NamaTabelHeader.Trim());
            result.Add("ARRNamaTabelDetail", menu.ARRNamaTabelDetail);
            result.Add("NamaView", menu.NamaView.Trim());
            result.Add("FieldTahunBuku", menu.FieldTahunBuku);
            result.Add("FieldNomorInput", menu.FieldNomorInput);
            result.Add("FieldNomorInputView", menu.FieldNomorInputView);
            result.Add("ARRFieldKeyHeader", menu.ARRFieldKeyHeader);
            result.Add("ARRFieldKeyDetail", menu.ARRFieldKeyDetail);
            result.Add("bolehBuatNoBaru", menu.bolehBuatNoBaru);
            result.Add("ListViewName", menu.ListViewName);
            result.Add("ARRNamaHttpContext", menu.ARRNamaHttpContext);
            result.Add("PemilikAplikasi", menu.PemilikAplikasi);
            result.Add("csServerLain", arrcsServerLain);

            #region ClassHeaderView
            if (result.FirstOrDefault(o => o.Key == "strClassHeaderView").Value.ToString() != "")
            {
                // create class Tools Header
                try
                {
                    Ttools = Type.GetType(string.Format("Ptpn8.Models.Tools`1[{0}]", result.FirstOrDefault(o => o.Key == "strClassHeaderView").Value.ToString()));
                    classToolsHeader = Activator.CreateInstance(Ttools);
                }
                catch
                {
                    result.Add("Message", "Err create class Tools Header");
                    return result;
                }

                // create class CRUD View Header
                try
                {

                    T = null;
                    T = Type.GetType(string.Format("Ptpn8.Models.CRUDView`2[[{0}],[{1}]]", result.FirstOrDefault(o => o.Key == "strClassHeaderView").Value.ToString(), result.FirstOrDefault(o => o.Key == "strClassHeaderTable").Value.ToString()));
                    classCRUDViewHeader = Activator.CreateInstance(T);
                    T.GetProperty("cs").SetValue(classCRUDViewHeader, arrcs[0].ToString());
                    T.GetProperty("tableName").SetValue(classCRUDViewHeader, result.FirstOrDefault(o => o.Key == "NamaTabelHeader").Value.ToString());
                    T.GetProperty("listViewName").SetValue(classCRUDViewHeader, result.FirstOrDefault(o => o.Key == "ListViewName").Value.ToString());
                    T.GetProperty("namaHttpContext").SetValue(classCRUDViewHeader, null);
                    T.GetProperty("userName").SetValue(classCRUDViewHeader, HttpContext.User.Identity.Name);

                    T.GetProperty("csServerLain").SetValue(classCRUDViewHeader, arrcsServerLain);


                    if (menu.ScriptTriggerHeaderServerLain == null || menu.ScriptTriggerHeaderServerLain == "")
                        T.GetProperty("scriptTriggerServerLain").SetValue(classCRUDViewHeader, "");
                    else
                        T.GetProperty("scriptTriggerServerLain").SetValue(classCRUDViewHeader, menu.ScriptTriggerHeaderServerLain);

                }
                catch
                {
                    result.Add("Message", "Error create class CRUD View Header");
                    return result;
                }

                // create Model View Header
                try
                {
                    T = null;
                    T = Type.GetType(result.FirstOrDefault(o => o.Key == "strClassHeaderView").Value.ToString());
                    ModelViewHeader = Activator.CreateInstance(T);
                    classInisiasiHeader = InisiasiScriptHeader(menuId);
                    if (classInisiasiHeader != null)
                    {
                        ModelViewHeader = InisiasiProperty(classInisiasiHeader.GetType(), ModelViewHeader, Guid.Parse(menuId), "header", "");
                        if (result.FirstOrDefault(o => o.Key == "FieldTahunBuku").Value.ToString() != "") {
                            classCRUDViewHeader.GetType().GetProperty(result.FirstOrDefault(o => o.Key == "FieldTahunBuku").Value.ToString()).SetValue(classCRUDViewHeader,
                                ModelViewHeader.GetType().GetProperty(result.FirstOrDefault(o => o.Key == "FieldTahunBuku").Value.ToString()).GetValue(ModelViewHeader));
                        }
                        result.Add("ModelViewHeader", ModelViewHeader);
                        result.Add("classInisiasiHeader", classInisiasiHeader);
                        result.Add("classCRUDViewHeader", classCRUDViewHeader);
                        result.Add("classToolsHeader", classToolsHeader);
                    }
                    else
                    {
                        result.Add("Message", "Error create Script Inisiasi View Header");
                        return result;
                    }
                }
                catch (Exception X)
                {
                    result.Add("Message", "Error create Model View Header");
                    return result;
                }
            }
            else
            {
                classToolsHeader = null;
                classCRUDViewHeader = null;
                ModelViewHeader = null;
                classInisiasiHeader = null;
                result.Add("ModelViewHeader", ModelViewHeader);
                result.Add("classInisiasiHeader", classInisiasiHeader);
                result.Add("classCRUDViewHeader", classCRUDViewHeader);
                result.Add("classToolsHeader", classToolsHeader);
            }
            #endregion

            #region ClassDetailView
            string[] arrClassDetailView = (string[])result.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrClassDetailTable = (string[])result.FirstOrDefault(o => o.Key == "ARRClassDetailTable").Value;
            string[] arrNamaTabelDetail = (string[])result.FirstOrDefault(o => o.Key == "ARRNamaTabelDetail").Value;
            string[] arrCS = (string[])result.FirstOrDefault(o => o.Key == "cs").Value;
            string[] arrFieldKeyHeader = (string[])result.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])result.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
            string[] arrNamaHttpContext = (string[])result.FirstOrDefault(o => o.Key == "ARRNamaHttpContext").Value;

            if (arrClassDetailView.Length > 0 && arrClassDetailView[0]!="")
            {
                // create class Tools Detail
                try
                {
                    string typeName;
                    for (int ii = 0; ii < arrClassDetailView.Length; ii++)
                    {
                        typeName = string.Format("Ptpn8.Models.Tools`1[{0}]", arrClassDetailView[ii]);
                        Ttools = Type.GetType(typeName);
                        classToolsDetail = Activator.CreateInstance(Ttools);
                        result.Add("classToolsDetail_" + arrClassDetailView[ii], classToolsDetail);
                        result.Add("FieldKeyHeader_" + arrClassDetailView[ii], arrFieldKeyHeader[ii]);
                        result.Add("FieldKeyDetail_" + arrClassDetailView[ii], arrFieldKeyDetail[ii]);
                    }

                }
                catch 
                {
                    result.Add("Message", "Error create class Tools Detail");
                    return result;
                }

                // create class CRUD View Detail
                try
                {
                    for (int ii = 0; ii < arrClassDetailView.Length; ii++)
                    {
                        T = Type.GetType(string.Format("Ptpn8.Models.CRUDView`2[[{0}],[{1}]]", arrClassDetailView[ii], arrClassDetailTable[ii]));
                        classCRUDViewDetail = Activator.CreateInstance(T);
                        T.GetProperty("cs").SetValue(classCRUDViewDetail, arrCS[ii]);
                        T.GetProperty("tableName").SetValue(classCRUDViewDetail, arrNamaTabelDetail[ii]);
                        T.GetProperty("listViewName").SetValue(classCRUDViewDetail, null);
                        T.GetProperty("namaHttpContext").SetValue(classCRUDViewDetail, arrNamaHttpContext == null ? null : arrNamaHttpContext[ii]);
                        T.GetProperty("csServerLain").SetValue(classCRUDViewDetail, arrcsServerLain);

                        if (menu.ScriptTriggerDetailServerLain == null || menu.ScriptTriggerDetailServerLain == "")
                            T.GetProperty("scriptTriggerServerLain").SetValue(classCRUDViewDetail, "");
                        else
                            T.GetProperty("scriptTriggerServerLain").SetValue(classCRUDViewDetail, menu.ScriptTriggerDetailServerLain);

                        result.Add("classCRUDViewDetail_" + arrClassDetailView[ii], classCRUDViewDetail);
                    }
                }
                catch 
                {
                    result.Add("Message", "Error create class CRUD View Detail");
                    return result;
                }

                // create model View Detail
                try
                {
                    for (int ii = 0; ii < arrClassDetailView.Length; ii++)
                    {
                        T = Type.GetType(arrClassDetailView[ii]);
                        ModelViewDetail = Activator.CreateInstance(T);
                        classInisiasiDetail = InisiasiScriptDetail(menuId, arrClassDetailView[ii]);
                        if (classInisiasiDetail != null)
                        {
                            ModelViewDetail = InisiasiProperty(classInisiasiDetail.GetType(), ModelViewDetail, Guid.Parse(menuId), "detail", arrClassDetailView[ii]);
                            result.Add("ModelViewDetail_" + arrClassDetailView[ii], ModelViewDetail);
                            result.Add("classInisiasiDetail_" + arrClassDetailView[ii], classInisiasiDetail);
                        }
                        else
                        {
                            result.Add("Message", "Error create model Script Inisiasi View Detail");
                            return result;
                        }
                    }
                }
                catch
                {
                    result.Add("Message", "Error create model View Detail");
                    return result;
                }
            }
            else
            {
                classToolsDetail = null;
                classCRUDViewDetail = null;
                ModelViewDetail = null;
                classInisiasiDetail = null;
                result.Add("ModelViewDetail", ModelViewDetail);
                result.Add("classInisiasiDetail", classInisiasiDetail);
                result.Add("classCRUDViewDetail", classCRUDViewDetail);
                result.Add("classToolsDetail", classToolsDetail);
            }
            #endregion

            result.Add("Message", "Success");
            return result;
        }
        public object InisiasiScriptHeader(string menuId)
        {
            try
            {
                var model = CRUDInisiasiInputProperty.CRUD.GetAllRecord().Where(o => o.MenuId == Guid.Parse(menuId) &&
                    o.AreaInput.ToLower() == "header").Distinct().OrderBy(o=>o.NoUrut);
                string script = @"using System; namespace Ptpn8.Models { public class InisiasiHeader {";
                foreach (var m in model)
                {
                    script = script + m.CSharpScript;
                }
                script = script + @" } }";
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters cp = new CompilerParameters
                {
                    GenerateExecutable = false, // create dll
                    GenerateInMemory = true, // create it in memory
                    WarningLevel = 3, // default warning level
                    CompilerOptions = "/optimize",  // optimize code
                    TreatWarningsAsErrors = false //better to false to avoid in warnings
                };
                cp.ReferencedAssemblies.Add("system.dll");
                cp.ReferencedAssemblies.Add("system.data.dll");
                cp.ReferencedAssemblies.Add("system.configuration.dll");
                cp.ReferencedAssemblies.Add("system.web.dll");
                cp.ReferencedAssemblies.Add("system.drawing.dll");
                cp.ReferencedAssemblies.Add(Path.Combine(Server.MapPath("~/bin"), "Ptpn8.dll"));
                CompilerResults results = provider.CompileAssemblyFromSource(cp, script);
                if (results.Errors.Count > 0)
                {
                    return null;
                }
                return results.CompiledAssembly.CreateInstance("Ptpn8.Models.InisiasiHeader");

            }
            catch 
            {
                return null;
            }
        }
        public object InisiasiScriptDetail(string menuId, string namaClass)
        {
            try
            {
                var model = CRUDInisiasiInputProperty.CRUD.GetAllRecord().Where(o => o.MenuId == Guid.Parse(menuId) &&
                    o.AreaInput.ToLower() == "detail" && o.NamaClass.ToLower() == namaClass.ToLower()).Distinct().OrderBy(o=>o.NoUrut);
                string script = @"using System; using System.Data.SqlClient; using System.Data; using System.Configuration; namespace Ptpn8.Models { public class InisiasiDetail {";
                foreach (var m in model)
                {
                    script = script + m.CSharpScript;
                }
                script = script + @" } }";
                CodeDomProvider provider = CodeDomProvider.CreateProvider("CSharp");
                CompilerParameters cp = new CompilerParameters
                {
                    GenerateExecutable = false, // create dll
                    GenerateInMemory = true, // create it in memory
                    WarningLevel = 3, // default warning level
                    CompilerOptions = "/optimize",  // optimize code
                    TreatWarningsAsErrors = false //better to false to avoid in warnings
                };
                cp.ReferencedAssemblies.Add("system.dll");
                cp.ReferencedAssemblies.Add("system.configuration.dll");
                cp.ReferencedAssemblies.Add("system.data.dll");
                cp.ReferencedAssemblies.Add("system.web.dll");
                cp.ReferencedAssemblies.Add(Path.Combine(Server.MapPath("~/bin"), "Microsoft.AspNet.Identity.EntityFramework.dll"));
                cp.ReferencedAssemblies.Add(Path.Combine(Server.MapPath("~/bin"), "Ptpn8.dll"));

                
                CompilerResults results = provider.CompileAssemblyFromSource(cp, script);
                if (results.Errors.Count > 0)
                {
                    return null;
                }
                return results.CompiledAssembly.CreateInstance("Ptpn8.Models.InisiasiDetail");

            }
            catch 
            {
                return null;
            }

        }
        public object InisiasiProperty(Type t, object modelView, Guid menuId, string areaInput, string namaClass)
        {
            try
            {
                object value1 = null;
                object value2 = null;
                object value3 = null;
                dynamic result = null;

                List<PropertyInfo> modelProperties = modelView.GetType().GetProperties().ToList();
                var model = CRUDInisiasiInputProperty.CRUD.GetAllRecord().Where(o => o.MenuId == menuId)
                    .Where(o => o.AreaInput.ToLower() == areaInput.ToLower())
                    .Where(o => namaClass == "" ? true : o.NamaClass.ToLower() == namaClass.ToLower())
                    .OrderBy(o => o.AreaInput).OrderBy(o => o.NoUrut);

                foreach (var m in model)
                {
                    if (m.Param3 != "")
                    {
                        var a = modelProperties.First(o => o.Name.ToLower() == m.Param3.ToLower());
                        if (a != null)
                        {
                            if (a.PropertyType.Name == "Byte[]")
                            {
                                value3 = modelView.GetType().GetProperty(m.Param3).GetValue(modelView);
                            }
                            else if (a.PropertyType.Name == "DateTime")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param3).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value3 = DateTime.Parse(str);
                            }
                            else if (a.PropertyType.Name == "String")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param3).GetValue(modelView);
                                string str = obj.ToString();
                                value3 = str;
                            }
                            else
                            {
                                object obj = modelView.GetType().GetProperty(m.Param3).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value3 = TypeDescriptor.GetConverter(a.PropertyType).ConvertFromInvariantString(str);
                            }
                        }
                    }

                    if (m.Param2 != "")
                    {
                        var a = modelProperties.First(o => o.Name.ToLower() == m.Param2.ToLower());
                        if (a != null)
                        {
                            if (a.PropertyType.Name == "Byte[]")
                            {
                                value2 = modelView.GetType().GetProperty(m.Param2).GetValue(modelView);
                            }
                            else if(a.PropertyType.Name == "DateTime")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param2).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value2 = DateTime.Parse(str);
                            }
                            else if (a.PropertyType.Name == "String")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param2).GetValue(modelView);
                                string str = obj.ToString();
                                value2 = str;
                            }
                            else
                            {
                                object obj = modelView.GetType().GetProperty(m.Param2).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value2 = TypeDescriptor.GetConverter(a.PropertyType).ConvertFromInvariantString(str);
                            }
                        }
                    }

                    if (m.Param1 != "")
                    {
                        var a = modelProperties.First(o => o.Name.ToLower() == m.Param1.ToLower());
                        if (a != null)
                        {
                            if (a.PropertyType.Name == "Byte[]")
                            {
                                value1 = modelView.GetType().GetProperty(m.Param1).GetValue(modelView);
                            }
                            else if (a.PropertyType.Name == "DateTime")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param1).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value1 = DateTime.Parse(str);
                            }
                            else if (a.PropertyType.Name == "String")
                            {
                                object obj = modelView.GetType().GetProperty(m.Param1).GetValue(modelView);
                                string str = obj.ToString();
                                value1 = str;
                            }
                            else
                            {
                                object obj = modelView.GetType().GetProperty(m.Param1).GetValue(modelView);
                                string str = obj.ToString().Replace(",", ".");
                                value1 = TypeDescriptor.GetConverter(a.PropertyType).ConvertFromInvariantString(str);
                            }
                        }
                        else
                        {
                            value1 = "";
                        }
                    }

                    var methods = t.GetRuntimeMethods().Where(o => o.Name == m.Method).ToList();
                    if (!methods.Any()) return null;
                    MethodInfo method = methods[0];
                    if (value3 != null)
                    {
                        result = method.Invoke(t, new object[] { value1, value2, value3 });
                    }
                    else if (value2 != null)
                        result = method.Invoke(t, new object[] { value1, value2 });
                    else if (value1 != null)
                        result = method.Invoke(t, new object[] { value1 });
                    else
                        result = method.Invoke(t, null);

                    if (m.NamaProperty != "")
                    {
                        try
                        {
                            modelView.GetType().GetProperty(m.NamaProperty).SetValue(modelView, result);
                        }
                        catch(Exception ex)
                        {

                        }
                    }
                    result = value1 = value2 = value3 = null;
                }
                return modelView;
            }
            catch (Exception ex)
            {
                return modelView;
            }
        }
        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult getNomorInput(string _model, string _menuId, string[] _filter = null, int _top = 0)
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                char[] delimiters = new char[] { ',', ';' };
                string[] pemilikAplikasi = _app.FirstOrDefault(o => o.Key == "PemilikAplikasi").Value.ToString().Split(delimiters);

                MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("JsonToClass");
                var Model = method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new string[] { _model });
                CRUDKondisiCRUD.CRUD.EraseAll();
                var mdlKondisiRead = CRUDKondisiCRUD.CRUD.GetAllRecord().Where(o => o.CRUD.ToLower() == "read"
                    && o.MenuId == Guid.Parse(_menuId) && o.AreaInput.ToLower() == "header");
                Dictionary<string, object> key = new Dictionary<string, object>();
                foreach (var m in mdlKondisiRead)
                {
                    try
                    {
                        if (m.Value == "{PosisiLokasiKerjaId}")
                        {
                            m.Value = posisiLokasiKerjaId;
                            key.Add(m.Key, new object[] {m.Value,m.Operator});
                        }
                        else if (m.Value == "{LokasiKerjaId}")
                        {
                            m.Value = lokasiKerjaId;
                            key.Add(m.Key, new object[] { m.Value, m.Operator });
                        }
                        else
                        {
                            var x = Model.GetType().GetProperty(m.Value);
                            if (x != null)
                                key.Add(m.Key, new object[] { x.GetValue(Model), m.Operator});
                            else
                                key.Add(m.Key, new object[] { m.Value,m.Operator });
                        }
                    }
                    catch 
                    {
                        key.Add(m.Key, new object[] { m.Value, m.Operator });
                    }
                }

                if (_filter != null)
                {
                    int nJ = 0;
                    for (int nI = 0; nI < _filter.Length / 2; nI++)
                    {
                        key.Add(_filter[nJ], new object[] { _filter[nJ + 1], "=" });
                        nJ = nJ + 2;
                    }
                }

                var obj = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value;
                string fieldTahunBuku = _app.FirstOrDefault(o => o.Key == "FieldTahunBuku").Value.ToString();
                obj.GetType().GetProperty("keyTable").SetValue(obj, key);
                obj.GetType().GetProperty(fieldTahunBuku).SetValue(obj, Model.GetType().GetProperty(fieldTahunBuku).GetValue(Model));
                method = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetMethod("GetAllRecord");

                object[] objDict = (object[])method.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value,
                    new string[] {
                        _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString(),
                        _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString(),
                        _app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString()
                    });
                var pemilikData = (string)objDict[0];
                var tahunBuku = (int)objDict[1];
                var currentData = ((IList)objDict[2]).Cast<object>().OrderByDescending(o => o.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).GetValue(o)).ToList();

                if(_top>0)
                {
                    currentData = currentData.Take(_top).ToList();
                }

                var result = new List<object>();

                // cari apakah di currentData ada data baru yang sedang diinput operator lain
                // jika ya, current user hanya diperkenankan untuk edit data yang sudah ada / tidak dapat menambah baru
                bool adaDataBaru = false;
                bool bolehBuatDataBaru = false;
                int NoBaru = 1;
                int NoTerakhir = 0;

                if (pemilikData != User.Identity.Name ||
                    (bool)_app.FirstOrDefault(o => o.Key == "bolehBuatNoBaru").Value == false ||
                    (pemilikAplikasi[0] != "" && Array.IndexOf(pemilikAplikasi, posisiLokasiKerjaId.ToLower()) < 0))
                {
                    for (int i = 0; i < currentData.Count; i++)
                    {
                        var m = currentData[i];
                        string noInputView = m.GetType().GetProperty((string)_app.FirstOrDefault(o => o.Key == "FieldNomorInputView")
                            .Value).GetValue(m).ToString().ToLower().Trim();
                        if (!noInputView.Contains("data baru"))
                        {
                            m = InisiasiProperty(_app.FirstOrDefault(o => o.Key == "classInisiasiHeader").Value.GetType(), m,
                                Guid.Parse(_menuId), "header", "");
                            result.Add(m);
                        }
                    }
                }
                else
                {
                    bolehBuatDataBaru = true;
                    for (int i = 0; i < currentData.Count; i++)
                    {
                        var m = currentData[i];
                        string noInputView = m.GetType().GetProperty((string)_app.FirstOrDefault(o => o.Key == "FieldNomorInputView").Value).GetValue(m).ToString().ToLower().Trim();
                        if (noInputView.Contains("data baru"))
                        {
                            adaDataBaru = true;
                        }
                        else
                        {
                            m = InisiasiProperty(_app.FirstOrDefault(o => o.Key == "classInisiasiHeader").Value.GetType(), m, Guid.Parse(_menuId), "header", "");
                        }
                        if ((int)m.GetType().GetProperty((string)_app.FirstOrDefault(o => o.Key == "FieldNomorInput").Value).GetValue(m) > NoTerakhir)
                            NoTerakhir = (int)m.GetType().GetProperty((string)_app.FirstOrDefault(o => o.Key == "FieldNomorInput").Value).GetValue(m);

                        result.Add(m);
                    }
                }

                if (currentData.Count > 0)
                {
                    NoBaru = NoTerakhir + 1;
                }

                if (bolehBuatDataBaru && !adaDataBaru)
                {
                    var newData = Activator.CreateInstance(_app.FirstOrDefault(m => m.Key == "ModelViewHeader").Value.GetType());
                    newData.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).SetValue(newData, NoBaru);
                    newData.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).SetValue(newData,
                        Model.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).GetValue(Model));

                    newData = InisiasiProperty(_app.FirstOrDefault(m => m.Key == "classInisiasiHeader").Value.GetType(),
                        newData, Guid.Parse(_menuId), "header", "");
                    result.Add(newData);
                    // insert ke Application Context
                    method = _app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value.GetType().GetMethod("CreateList");
                    method.Invoke(_app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value, new object[] { newData });

                    //for(int i=1;i<=NoBaru;i++)
                    //{
                    //    var no = result.Where(o => o.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).GetValue(o).ToString() == i.ToString()).ToList();
                    //    if (no.Count == 0)
                    //    {
                    //        var newData = Activator.CreateInstance(_app.FirstOrDefault(m => m.Key == "ModelViewHeader").Value.GetType());
                    //        newData.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).SetValue(newData, i);
                    //        newData.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).SetValue(newData,
                    //            Model.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).GetValue(Model));

                    //        newData = InisiasiProperty(_app.FirstOrDefault(m => m.Key == "classInisiasiHeader").Value.GetType(),
                    //            newData, Guid.Parse(_menuId), "header","");
                    //        result.Add(newData);
                    //        // insert ke Application Context
                    //        method = _app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value.GetType().GetMethod("CreateList");
                    //        method.Invoke(_app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value, new object[] { newData });
                    //    }
                    //}
                }

                var model = result.OrderByDescending(o => o.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).GetValue(o)).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult getLastNomorInput(string _model, string _menuId)
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("JsonToClass");
                var Model = method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new string[] { _model });

                var newData = Activator.CreateInstance(_app.FirstOrDefault(m => m.Key == "ModelViewHeader").Value.GetType());
                newData.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).SetValue(newData,
                    Model.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldTahunBuku").Value).GetValue(Model));

                newData = InisiasiProperty(_app.FirstOrDefault(m => m.Key == "classInisiasiHeader").Value.GetType(),
                    newData, Guid.Parse(_menuId), "header", "");

                var result = new List<object>();
                result.Add(newData);
                // insert ke Application Context
                method = _app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value.GetType().GetMethod("CreateList");
                method.Invoke(_app.FirstOrDefault(m => m.Key == "classCRUDViewHeader").Value, new object[] { newData });
                var model = result.OrderByDescending(o => o.GetType().GetProperty((string)_app.FirstOrDefault(m => m.Key == "FieldNomorInput").Value).GetValue(o)).ToList();
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
        } 

        [Authorize]
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult simpanHeader(string _model, bool _baru = false, string _menuId = "", string _SQLLanjut = "")
        {
            try
            {

                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                char[] delimiters = new char[] { ',', ';' };
                string[] pemilikAplikasi = _app.FirstOrDefault(o => o.Key == "PemilikAplikasi").Value.ToString().Split(delimiters);
                // kalo bukan pemilik aplikasi, tidak bisa nyimpan header
                if (pemilikAplikasi[0] != "" && Array.IndexOf(pemilikAplikasi, posisiLokasiKerjaId.ToLower()) < 0)
                {
                    return Json(new object[] { true, "" }, JsonRequestBehavior.AllowGet);
                }

                MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("JsonToClass");
                var Model = method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new string[] { _model });
                MethodInfo methodInsert = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetMethod("Insert");
                MethodInfo methodUpdate = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetMethod("Update");
                if (TryValidateModel(Model))
                {

                    CRUDKondisiCRUD.CRUD.EraseAll();
                    if (Model.GetType().GetProperty("Lampiran_MemoDinas") != null)
                    {
                        string namaFileLampiran = (string) Model.GetType().GetProperty("Lampiran_MemoDinas").GetValue(Model);
                        namaFileLampiran.Replace("&amp;", "dan");
                        Model.GetType().GetProperty("Lampiran_MemoDinas").SetValue(Model, namaFileLampiran);
                    }
                    var mdlKondisiUpdate = CRUDKondisiCRUD.CRUD.GetAllRecord().Where(o => o.CRUD.ToLower() == "update" &&
                        o.MenuId == Guid.Parse(_menuId) && o.AreaInput.ToLower() == "header");
                    Dictionary<string, object> key = new Dictionary<string, object>();
                    foreach (var m in mdlKondisiUpdate)
                    {
                        key.Add(m.Key, new object[] { Model.GetType().GetProperty(m.Value).GetValue(Model), m.Operator});
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType()
                        .GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value, key);

                    List<object> _Model=new List<object>();
                    _Model.Add(Model);

                    if (_baru)
                    {
                        if ((bool)methodInsert.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value, new object[] { _Model }))
                        {
                            if (_SQLLanjut != "") ExecSQL(_SQLLanjut, _menuId);
                            return Json(new object[] { true, "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new object[] { false, msgInternalError }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        if ((bool)methodUpdate.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value, new object[] { _Model }))
                        {
                            if (_SQLLanjut != "") ExecSQL(_SQLLanjut, _menuId); 
                            return Json(new object[] { true, "" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                            return Json(new object[] { false, msgInternalError }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new object[] { false, message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new object[] { false, msgInternalError }, JsonRequestBehavior.AllowGet);
            }
        }


        [Authorize]
        [HttpPost]
        public ActionResult hapusHeader(string _model, string _menuId)
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("JsonToClass");
                var Model = method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new string[] { _model });
                MethodInfo methodDelete = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetMethod("Delete");
                if (TryValidateModel(Model))
                {
                    CRUDKondisiCRUD.CRUD.EraseAll();
                    var mdlKondisiDelete = CRUDKondisiCRUD.CRUD.GetAllRecord().Where(o => o.CRUD.ToLower() == "delete" && o.MenuId == Guid.Parse(_menuId) &&
                        o.AreaInput.ToLower() == "header");
                    Dictionary<string, object> key = new Dictionary<string, object>();
                    foreach (var m in mdlKondisiDelete)
                    {
                        key.Add(m.Key, new object[] { Model.GetType().GetProperty(m.Value).GetValue(Model), m.Operator});
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value, key);

                    List<object> _Model = new List<object>();
                    _Model.Add(Model);

                    if ((bool)methodDelete.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value, new object[] { _Model }))
                        return Json(new object[] { true, "" }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new object[] { false, msgInternalError }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new object[] { false, message }, JsonRequestBehavior.AllowGet);
                }
            }
            catch
            {
                return Json(new object[] { false, msgInternalError }, JsonRequestBehavior.AllowGet);
            }
        }

        public string detailInsert(List<object> _model, string _menuId, string _classDetailView = "")
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
                string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
                string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
                if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

                MethodInfo methodInsert = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Insert");
                Dictionary<string, object> key = new Dictionary<string, object>();
                var __model = _model.FirstOrDefault();

                if (_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString() != "")
                {
                    try
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(), 
                            new object[] { (Guid)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "="}
                            );
                    }
                    catch
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(),
                            new object[] { (string)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "=" }
                            );
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                }

                if (!(bool)methodInsert.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { _model }))
                {
                    return msgInternalError;
                }
            }
            catch
            {
                return msgInternalError;
            }

            return "Success";
        }
        public string detailUpdate(List<object> _model, string _menuId, string _classDetailView = "")
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
                string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
                string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
                if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

                MethodInfo methodUpdate = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Update");
                Dictionary<string, object> key = new Dictionary<string, object>();
                var __model = _model.FirstOrDefault();
                if (_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString() != "")
                {
                    try
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(),
                            new object[] { (Guid)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "=" }
                            );
                    }
                    catch
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(),
                            new object[] { (string)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "=" }
                            );
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                }

                if (!(bool)methodUpdate.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { _model } ))
                {
                    return msgInternalError;
                }

            }
            catch 
            {
                return msgInternalError;
            }

            return "Success";
        }
        public string detailDelete(List<object> _model, string _menuId, string _classDetailView = "")
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
                string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
                string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
                if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

                MethodInfo methodDelete = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Delete");
                Dictionary<string, object> key = new Dictionary<string, object>();
                var __model = _model.FirstOrDefault();
                try
                {
                    key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(),
                        new object[] { (Guid)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "="}
                        );
                }
                catch 
                {
                    key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString(),
                        new object[] { (string)__model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "FieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(__model), "=" }
                        );
                }
                _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                if (!(bool)methodDelete.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { _model }))
                {
                    return msgInternalError;
                }
            }
            catch
            {
                return msgInternalError;
            }

            return "Success";
        }
        [Authorize]
        [HttpPost]
        public ActionResult detailRead([DataSourceRequest] DataSourceRequest request,
            string id = "", string _menuId = "", string _classDetailView = "", string[] _filter = null, string[] _filterlike = null)
        {
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                if (id == "") id = Guid.Empty.ToString();

                string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
                string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
                string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;

                if (_classDetailView == "") _classDetailView = arrclassDetailView[0];
                Dictionary<string, object> key = new Dictionary<string, object>();
                if (_app.FirstOrDefault(o => o.Key == "FieldKeyHeader_" + _classDetailView).Value != null && 
                    _app.FirstOrDefault(o => o.Key == "FieldKeyHeader_" + _classDetailView).Value.ToString() != "")
                {
                    try
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyHeader_" + _classDetailView).Value.ToString(), new object[] { Guid.Parse(id), "=" });
                    }
                    catch
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "FieldKeyHeader_" + _classDetailView).Value.ToString(), new object[] { id, "=" });
                    }

                }

                if (_filter != null)
                {
                    int nJ = 0;
                    for (int nI = 0; nI < _filter.Length / 2; nI++)
                    {
                        key.Add(_filter[nJ], new object[] { _filter[nJ + 1], "=" });
                        nJ = nJ + 2;
                    }
                }

                if (_filterlike != null)
                {
                    int nJ = 0;
                    for (int nI = 0; nI < _filterlike.Length / 2; nI++)
                    {
                        key.Add(_filterlike[nJ], new object[] { _filterlike[nJ + 1], "like" });
                        nJ = nJ + 2;
                    }
                }

                CRUDKondisiCRUD.CRUD.EraseAll();
                var mdlKondisiRead = CRUDKondisiCRUD.CRUD.GetAllRecord().Where(o => o.CRUD.ToLower() == "read" && 
                    o.MenuId == Guid.Parse(_menuId) && o.AreaInput.ToLower() == "detail" && 
                    o.ClassView.ToString().ToLower().Trim() == _classDetailView.ToString().ToLower().Trim());
                if (mdlKondisiRead.Count() > 0)
                {
                    foreach (var m in mdlKondisiRead)
                    {
                        try
                        {
                            if (m.Value == "{PosisiLokasiKerjaId}")
                            {
                                m.Value = posisiLokasiKerjaId;
                                key.Add(m.Key, new object[] { m.Value, m.Operator });
                            }
                            else if (m.Value == "{LokasiKerjaId}")
                            {
                                m.Value = lokasiKerjaId;
                                key.Add(m.Key, new object[] { m.Value, m.Operator });
                            }
                            else
                                key.Add(m.Key, new object[] { m.Value, m.Operator });
                        }
                        catch
                        {
                            key.Add(m.Key, new object[] { m.Value, m.Operator });
                        }
                    }
                }
                _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView)
                    .Value.GetType().GetProperty("keyTable")
                    .SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);

                MethodInfo methodRead = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Read");
                object x1 = methodRead.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, null);
                var currentData = ((IList)x1).Cast<object>().ToList();
                for (int i = 0; i < currentData.Count; i++)
                {
                    var m = currentData[i];
                    m = InisiasiProperty(_app.FirstOrDefault(o => o.Key == "classInisiasiDetail_" + _classDetailView).Value.GetType(), m, Guid.Parse(_menuId), "detail", _classDetailView);
                }

                var serializer = new JavaScriptSerializer();

                // For simplicity just use Int32's max value.
                // You could always read the value from the config section mentioned above.
                serializer.MaxJsonLength = Int32.MaxValue;

                var resultData = currentData.ToDataSourceResult(request);
                var result = new ContentResult
                {
                    Content = serializer.Serialize(resultData),
                    ContentType = "application/json"
                };

                //return Json(currentData.ToDataSourceResult(request));
                return result;
                
            }
            catch
            {
                return Json(new object[] { }.ToDataSourceResult(request));
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult detailCreate([DataSourceRequest] DataSourceRequest request, string _model,
            string _menuId, string _classDetailView = "")
        {

            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;

            if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

            MethodInfo methodJsonToClass = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value.GetType().GetMethod("JsonToClass");
            var Model = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { _model });
            try
            {
                MethodInfo methodInsert = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Insert");
                if (TryValidateModel(Model))
                {
                    Dictionary<string, object> key = new Dictionary<string, object>();

                    if (_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString() != "")
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString(), 
                            new object[] { (Guid)Model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value.ToString()).GetValue(Model), "=" }
                            );
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                    if ((bool)methodInsert.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { Model }))
                        return Json(new[] { Model }.ToDataSourceResult(request, ModelState));
                    else
                    {
                        try
                        {
                            Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                        }
                        catch
                        {
                        }
                        return View(Model);
                    }
                }
                else
                {
                    try
                    {
                        var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                        Model.GetType().GetProperty("ErrorMessage").SetValue(Model, message);
                    }
                    catch
                    {
                    }
                    return View(Model);
                }
            }
            catch
            {
                try
                {
                    Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                }
                catch
                {
                }
                return View(Model);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult detailUpdate([DataSourceRequest] DataSourceRequest request, string _model,
            bool _baru = false, string _menuId = "", string _classDetailView = "")
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
            if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

            MethodInfo methodJsonToClass = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value.GetType().GetMethod("JsonToClass");
            var Model = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { _model });
            try
            {
                MethodInfo methodInsert = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Insert");
                MethodInfo methodUpdate = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Update");
                if (TryValidateModel(Model))
                {
                    Dictionary<string, object> key = new Dictionary<string, object>();
                    if (_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString() != "")
                    {
                        key.Add(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString(), 
                            new object[] { (Guid)Model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(Model), "="}
                            );
                    }
                    _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                    if (_baru)
                    {
                        if (!(bool)methodInsert.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { Model }))
                        {
                            try
                            {
                                Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                            }
                            catch
                            {
                            }
                            return View(Model);
                        }
                    }
                    else
                    {
                        if (!(bool)methodUpdate.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { Model }))
                        {
                            try
                            {
                                Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                            }
                            catch
                            {
                            }
                            return View(Model);
                        }
                    }
                }
                else
                {
                    try
                    {
                        var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                        Model.GetType().GetProperty("ErrorMessage").SetValue(Model, message);
                    }
                    catch
                    {
                    }
                    return View(Model);
                }
            }
            catch
            {
                try
                {
                    Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                }
                catch
                {
                }
                return View(Model);
            }
            return Json(new[] { Model }.ToDataSourceResult(request, ModelState));
        }
        [Authorize]
        [HttpPost]
        public ActionResult detailDelete([DataSourceRequest] DataSourceRequest request, string _model,
            string _menuId, string _classDetailView = "")
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
            if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

            MethodInfo methodJsonToClass = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value.GetType().GetMethod("JsonToClass");
            var Model = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { _model });
            try
            {
                if (TryValidateModel(Model))
                {
                    MethodInfo methodDelete = _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetMethod("Delete");
                    Dictionary<string, object> key = new Dictionary<string, object>();

                    key.Add(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString(), 
                        new object[] { (Guid)Model.GetType().GetProperty(_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail_" + _classDetailView).Value.ToString()).GetValue(Model), "="}
                        );

                    _app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value.GetType().GetProperty("keyTable").SetValue(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, key);
                    if ((bool)methodDelete.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewDetail_" + _classDetailView).Value, new object[] { Model }))
                        return Json(new[] { Model }.ToDataSourceResult(request, ModelState));
                    else
                    {
                        try
                        {
                            Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                        }
                        catch
                        {
                        }
                        return View(Model);
                    }
                }
                else
                {
                    try
                    {
                        var message = string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                        Model.GetType().GetProperty("ErrorMessage").SetValue(Model, message);
                    }
                    catch
                    {
                    }
                    return View(Model);
                }
            }
            catch
            {
                try
                {
                    Model.GetType().GetProperty("ErrorMessage").SetValue(Model, msgInternalError);
                }
                catch
                {
                }
                return View(Model);
            }
        }
        [Authorize]
        [HttpPost]
        public ActionResult detailDestroy([DataSourceRequest] DataSourceRequest request, string _model, string _menuId)
        {
            return Json(ModelState.ToDataSourceResult());
        }
        [HttpPost]
        public ActionResult hapusContext(int tahunBuku = 0, string _menuId = "", string mainBudidayaId = "")
        {
            if (mainBudidayaId == "")
                return Json(true, JsonRequestBehavior.AllowGet);
            else
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string xlistViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + xlistViewName];

                string listViewName = _app.FirstOrDefault(o => o.Key == "ListViewName").Value.ToString();

                HttpContext.Application[listViewName] = null;
                return Json(true, JsonRequestBehavior.AllowGet);

                //List<object[]> lstResult = (List<object[]>)HttpContext.Application[listViewName];
                //if (lstResult == null || lstResult.Count == 0)
                //    return Json(true, JsonRequestBehavior.AllowGet);
                //else
                //{
                //    object[] objResult = new object[3];
                //    foreach (var listView in lstResult)
                //    {
                //        if ((int)listView[1] == tahunBuku)
                //        {
                //            //listView[2]=null;
                //            lstResult.Remove(listView);
                //            break;
                //        }
                //    }
                //    return Json(true, JsonRequestBehavior.AllowGet);
                //}


            }
        }

        [HttpPost]
        public ActionResult hapusContextNomorInput(string _menuId = "")
        {
            if (_menuId == "")
                return Json(true, JsonRequestBehavior.AllowGet);
            else
            {
                HttpContext.Session["ListKondisiCRUD"] = null;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize]
        public ActionResult GetHeaderHistory(string key, string value, string _menuId)
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            MethodInfo method = _app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value.GetType().GetMethod("GetAllRecord");
            object[] objDict = (object[])method.Invoke(_app.FirstOrDefault(o => o.Key == "classCRUDViewHeader").Value,
                    new string[] {
                        _app.FirstOrDefault(o => o.Key == "NamaAplikasi").Value.ToString(),
                        _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString(),
                        _app.FirstOrDefault(o => o.Key == "MenuId").Value.ToString()
                    });
            var pemilikData = (string)objDict[0];
            var tahunBuku = (int)objDict[1];
            var currentData = ((IList)objDict[2]).Cast<object>().ToList();
            var model = (from m in currentData.Where(o => o.GetType().GetProperty(key).GetValue(o).ToString().ToLower().Contains(value.ToLower()))
                         select m
                         ).Distinct();
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult GetDataFrom(string strClassView, string scriptSQL, string _menuId, string _cs = "")
        {
            List<object> Results = new List<object>();
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrCS;
                if (_cs == "")
                {
                    arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;
                    arrCS = arrCS.Distinct().ToArray();
                }
                else
                {
                    arrCS = new string[] { System.Configuration.ConfigurationManager.ConnectionStrings[_cs].ConnectionString };
                }

                if (scriptSQL.ToLower().IndexOf("delete ") > 0 ||
                    //scriptSQL.ToLower().IndexOf("update ") > 0 ||
                    scriptSQL.ToLower().IndexOf("drop tab") > 0
                    )
                {
                    return Json(Results, JsonRequestBehavior.AllowGet);
                }


                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                for (int x = 0; x < arrCS.Length; x++)
                {
                    using (var conn = new SqlConnection(arrCS[x]))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = scriptSQL;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Result = Activator.CreateInstance(T);
                                for (int i = 0; i < modelProperties.Count; i++)
                                    try
                                    {
                                        modelProperties[i].SetValue(Result, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                Results.Add(Result);
                            };
                        }
                        conn.Close();
                    }
                }
            }
            catch 
            {
            }
            return Json(Results, JsonRequestBehavior.AllowGet);
        }

        public List<object> getDataFrom(string strClassView, string scriptSQL, string _menuId, string _cs = "")
        {
            List<object> Results = new List<object>();
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrCS;
                if (_cs == "")
                {
                    arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;
                    arrCS = arrCS.Distinct().ToArray();
                }
                else
                {
                    arrCS = new string[] { System.Configuration.ConfigurationManager.ConnectionStrings[_cs].ConnectionString };
                }

                if (scriptSQL.ToLower().IndexOf("delete ") > 0 ||
                    //scriptSQL.ToLower().IndexOf("update ") > 0 ||
                    scriptSQL.ToLower().IndexOf("drop tab") > 0
                    )
                {
                    return Results;
                }


                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                for (int x = 0; x < arrCS.Length; x++)
                {
                    using (var conn = new SqlConnection(arrCS[x]))
                    using (var cmd = conn.CreateCommand())
                    {
                        conn.Open();
                        cmd.CommandText = scriptSQL;
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var Result = Activator.CreateInstance(T);
                                for (int i = 0; i < modelProperties.Count; i++)
                                    try
                                    {
                                        modelProperties[i].SetValue(Result, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                    }
                                    catch
                                    {
                                        continue;
                                    }

                                Results.Add(Result);
                            };
                        }
                        conn.Close();
                    }
                }
            }
            catch
            {
            }
            return Results;
        }


        [Authorize]
        [HttpPost]
        public ActionResult GetDataFromGRID([DataSourceRequest] DataSourceRequest request,
            string strClassView, string scriptSQL, string _menuId, string _cs = "")
        {
            List<object> Results = new List<object>();
            try
            {
                string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
                string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

                string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
                string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

                //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
                Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

                string[] arrCS;
                if (_cs == "")
                {
                    arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;
                }
                else
                {
                    arrCS = new string[] { System.Configuration.ConfigurationManager.ConnectionStrings[_cs].ConnectionString };
                }

                if (scriptSQL.ToLower().IndexOf("delete ") > 0 ||
                    //scriptSQL.ToLower().IndexOf("update ") > 0 ||
                    scriptSQL.ToLower().IndexOf("drop tab") > 0
                    )
                {
                    return Json(Results, JsonRequestBehavior.AllowGet);
                }


                Type T = Type.GetType(strClassView);
                var Model = Activator.CreateInstance(T);
                List<PropertyInfo> modelProperties = Model.GetType().GetProperties().ToList();

                using (var conn = new SqlConnection(arrCS[0]))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var Result = Activator.CreateInstance(T);
                            for (int i = 0; i < modelProperties.Count; i++)
                                try
                                {
                                    modelProperties[i].SetValue(Result, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                }
                                catch
                                {
                                    continue;
                                }

                            Results.Add(Result);
                        };
                    }
                    conn.Close();
                }

                for (int i = 0; i < Results.Count; i++)
                {
                    var m = Results[i];
                    m = InisiasiProperty(_app.FirstOrDefault(o => o.Key == "classInisiasiDetail_" + strClassView).Value.GetType(), m, Guid.Parse(_menuId), "detail", strClassView);
                }
            }
            catch 
            {
            }

            var serializer = new JavaScriptSerializer();

            // For simplicity just use Int32's max value.
            // You could always read the value from the config section mentioned above.
            serializer.MaxJsonLength = Int32.MaxValue;

            var resultData = Results.ToDataSourceResult(request);
            var result = new ContentResult
            {
                Content = serializer.Serialize(resultData),
                ContentType = "application/json"
            };

            //return Json(Results.ToDataSourceResult(request));
            return result;
        }

        [Authorize]
        public ActionResult ExecSQL(string scriptSQL, string _menuId)
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            List<object[]> Results = new List<object[]>();
            string[] arrCS = (string[])_app.FirstOrDefault(o => o.Key == "cs").Value;
            if (scriptSQL.ToLower().IndexOf("delete ") >= 0 ||
                //scriptSQL.ToLower().IndexOf("update ") >= 0 ||
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
                        if(dr[j].GetType().Name=="Byte[]")
                        {
                            CRUDImage.CRUD.RemoveFileView(HttpContext.User.Identity.Name.ToString() + "_" + tap.Columns[j].ToString() + ".jpg");
                            CRUDImage.CRUD.ReadToView(HttpContext.User.Identity.Name.ToString()+"_"+tap.Columns[j].ToString()+".jpg",(byte[])dr[j]);
                        }
                        else if(tap.Columns[j].ToString().ToLower().Contains("img"))
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


        [Authorize]
        public ActionResult GetUserName()
        {
            return Json(HttpContext.User.Identity.Name, JsonRequestBehavior.AllowGet);
        }
        [Authorize]
        [HttpPost]
        public ActionResult UpdateCreateDelete(
            [Bind(Prefix = "updated")]string[] updatedModels,
            [Bind(Prefix = "new")]string[] newModels,
            [Bind(Prefix = "deleted")]string[] deletedModels,
            [Bind(Prefix = "spupdated")]string[] spupdated,
            [Bind(Prefix = "spnew")]string[] spnew,
            [Bind(Prefix = "spdeleted")]string[] spdeleted,
            [Bind(Prefix = "mnid")]string _menuId,
            [Bind(Prefix = "classdv")]string _classDetailView = ""
            )
        {

            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            char[] delimiters = new char[] { ',', ';' };
            string[] pemilikAplikasi = _app.FirstOrDefault(o => o.Key == "PemilikAplikasi").Value.ToString().Split(delimiters);
            // kalo bukan pemilik aplikasi, tidak bisa nyimpan detail
            if (pemilikAplikasi[0] != "" && Array.IndexOf(pemilikAplikasi, posisiLokasiKerjaId.ToLower()) < 0)
            {
                return Json("Tidak dapat menyimpan data, karena bukan pemilik data!!!");
            }

            string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
            if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

            MethodInfo methodJsonToClass = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value.GetType().GetMethod("JsonToClass");
            List<object> UpdatedModels = new List<object>();
            List<object> NewModels = new List<object>();
            List<object> DeletedModels = new List<object>();
            bool validUpdatedModels = true;
            bool validNewModels = true;
            bool validDeletedModels = true;
            string msgUpdatedModels = "";
            string msgNewModels = "";
            string msgDeletedModels = "";

            if (updatedModels != null && updatedModels.Count() > 0)
            {
                for (int i = 0; i < updatedModels.Count(); i++)
                {
                    var updatedModel = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { updatedModels[i] });
                    UpdatedModels.Add(updatedModel);
                }

                foreach (var updatedModel in UpdatedModels)
                {
                    if (!TryValidateModel(updatedModel))
                    {
                        msgUpdatedModels = "No Baris : " + updatedModel.GetType().GetProperty("NoBaris").GetValue(updatedModel).ToString() + " ; " +
                            string.Join(" | ", ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage));
                        validUpdatedModels = false;
                        break;
                    }
                }
            }

            if (newModels != null && newModels.Count() > 0)
            {
                for (int i = 0; i < newModels.Count(); i++)
                {
                    var newModel = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { newModels[i] });
                    NewModels.Add(newModel);
                }

                foreach (var newModel in NewModels)
                {
                    if (!TryValidateModel(newModel))
                    {
                        if (newModel.GetType().GetProperty("NoBaris") != null)
                        {
                            msgNewModels = "No Baris : " + newModel.GetType().GetProperty("NoBaris").GetValue(newModel).ToString() + " ; " +
                                string.Join(" | ", ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));
                            validNewModels = false;
                            break;
                        }
                        else
                        {
                            msgNewModels = string.Join(" | ", ModelState.Values
                                .SelectMany(v => v.Errors)
                                .Select(e => e.ErrorMessage));
                            validNewModels = false;
                            break;
                        }
                    }
                }
            }

            if (deletedModels != null && deletedModels.Count() > 0)
            {
                for (int i = 0; i < deletedModels.Count(); i++)
                {
                    var deletedModel = methodJsonToClass.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { deletedModels[i] });
                    DeletedModels.Add(deletedModel);
                }
            }

            if (validUpdatedModels && validNewModels && validDeletedModels)
            {
                if (UpdatedModels.Count > 0)
                {
                    msgUpdatedModels = detailUpdate(UpdatedModels, _menuId, _classDetailView);
                    if (spupdated != null && spupdated.Count() > 0)
                    {
                        foreach (var sp in spupdated)
                        {
                            ExecSQL(sp, _menuId);
                        }
                    }
                }

                if (NewModels.Count > 0)
                {
                    msgNewModels = detailInsert(NewModels, _menuId, _classDetailView);
                    if (spnew != null && spnew.Count() > 0)
                    {
                        foreach (var sp in spnew)
                        {
                            ExecSQL(sp, _menuId);
                        }
                    }
                }

                if(DeletedModels.Count>0)
                {
                    msgDeletedModels = detailDelete(DeletedModels, _menuId, _classDetailView);
                    if (spdeleted != null && spdeleted.Count() > 0)
                    {
                        foreach (var sp in spdeleted)
                        {
                            ExecSQL(sp, _menuId);
                        }
                    }
                }
                return Json("Success");
            }
            else
            {
                string msg = msgUpdatedModels + msgNewModels + msgDeletedModels;
                return Json(msg);
            }
        }

        public ActionResult SaveFoto(IEnumerable<HttpPostedFileBase> files, string fn="")
        {
            //string[] listFile;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath="";
                    if (fn == "") {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(100, 100);
                        }

                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto(string[] fileNames, string fn="")
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "") {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult Save(IEnumerable<HttpPostedFileBase> files, string fn="")
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult Remove(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult SaveSMPNVIII(IEnumerable<HttpPostedFileBase> files, string fn = "")
        {
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document/SMPNVIII"), fileName);

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveSMPNVIII(string[] fileNames)
        {
            // The parameter of the Remove action must be called "fileNames"

            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document/SMPNVIII"), fileName);

                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult SaveFile(IEnumerable<HttpPostedFileBase> files, string fn = "", string namaFile="")
        {
            // contoh fn : "~/FileKerjasama/Minat"
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath=""; 
                    if(fn=="")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document"), fileName);
                    } 
                    else
                    {
                        if(namaFile!="")
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), namaFile);
                        }
                        else
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                        }
                        
                    }

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveFile(string[] fileNames, string fn = "", string namaFile="")
        {
            // contoh fn : "~/FileKerjasama/Minat"
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Document/PerjanjianKerjasama"), fileName);
                    }
                    else
                    {
                        if (namaFile != "")
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), namaFile);
                        }
                        else
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                        }
                        
                    }
                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult SaveFilePK(IEnumerable<HttpPostedFileBase> files, string fn = "", string namaFile = "")
        {
            // contoh fn : "~/FileKerjasama/Minat"
            // The Name of the Upload component is "files"
            if (files != null)
            {
                foreach (var file in files)
                {

                    // Some browsers send file names with full path.
                    // We are only interested in the file name.
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/LampiranFileKerjasama"), fileName);
                    }
                    else
                    {
                        if (namaFile != "")
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), namaFile);
                        }
                        else
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                        }

                    }

                    // The files are not actually saved in this demo
                    file.SaveAs(physicalPath);
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        public ActionResult RemoveFilePK(string[] fileNames, string fn = "", string namaFile = "")
        {
            // contoh fn : "~/FileKerjasama/Minat"
            // The parameter of the Remove action must be called "fileNames"
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/LampiranFileKerjasama"), fileName);
                    }
                    else
                    {
                        if (namaFile != "")
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), namaFile);
                        }
                        else
                        {
                            physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                        }

                    }
                    // TODO: Verify user permissions

                    if (System.IO.File.Exists(physicalPath))
                    {
                        // The files are not actually removed in this demo
                        System.IO.File.Delete(physicalPath);
                    }
                }
            }

            // Return an empty string to signify success
            return Content("");
        }

        [HttpPost]
        public ActionResult GetByteFromUploadH(string _filename, string _id, string _menuId, string _classHeaderView = "")
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            byte[] result;
            string[] arrclassHeaderView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassHeaderView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            if (_classHeaderView == "") _classHeaderView = arrclassHeaderView[0];

            MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("GetFromUpload");
            result = (byte[])method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new string[] { _filename });

            var physicalPath1 = Path.Combine(Server.MapPath("~/Content/Images/Upload"), _filename);
            Image img = Image.FromFile(physicalPath1);
            var physicalPath2 = Path.Combine(Server.MapPath("~/Content/Images/View"), _id + ".jpg");
            System.IO.File.Delete(physicalPath2);
            img.Save(physicalPath2);

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetByteFromUpload(string _filename, string _id, string _menuId, string _classDetailView = "")
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            byte[] result;
            string[] arrclassDetailView = (string[])_app.FirstOrDefault(o => o.Key == "ARRClassDetailView").Value;
            string[] arrFieldKeyHeader = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyHeader").Value;
            string[] arrFieldKeyDetail = (string[])_app.FirstOrDefault(o => o.Key == "ARRFieldKeyDetail").Value;
            if (_classDetailView == "") _classDetailView = arrclassDetailView[0];

            MethodInfo method = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value.GetType().GetMethod("GetFromUpload");
            result = (byte[])method.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + _classDetailView).Value, new string[] { _filename });

            var physicalPath1 = Path.Combine(Server.MapPath("~/Content/Images/Upload"), _filename);
            Image img = Image.FromFile(physicalPath1);
            var physicalPath2 = Path.Combine(Server.MapPath("~/Content/Images/View"), _id + ".jpg");
            System.IO.File.Delete(physicalPath2);
            img.Save(physicalPath2);

            return Json(result);
        }
        [HttpPost]
        public ActionResult CariUserYangLagiNgedit(string _menuId)
        {
            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            char[] delimiters = new char[] { ',', ';' };
            string[] pemilikAplikasi = _app.FirstOrDefault(o => o.Key == "PemilikAplikasi").Value.ToString().Split(delimiters);

            string result;

            if (pemilikAplikasi[0] != "" && Array.IndexOf(pemilikAplikasi, posisiLokasiKerjaId.ToLower()) < 0)
            {
                result = "user lain atau bukan bagian/unit pemilik data.";
            }
            else
            {
                string namaMenu = _app.FirstOrDefault(o => o.Key == "NamaModul").Value.ToString();
                var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.GetAllRecord().Where(o => o.Menu == namaMenu && o.UserName != HttpContext.User.Identity.Name && o.StatusOwner == true).FirstOrDefault();
                if (current != null)
                    result = current.Nama.Trim();
                else
                    result = "";
            }
            return Json(result);
        }

        [HttpPost]
        public ActionResult ReadXML(string url, string strClassHeader, string strClassDetail, string _menuId)
        {

            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            MethodInfo methodHeader = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("DataTableToModelList");
            MethodInfo methodDetail = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + strClassDetail).Value.GetType().GetMethod("DataTableToModelList");
            XmlDocument doc = new XmlDocument();
            DataSet ds = new DataSet();
            try
            {
                doc.Load(url);
                ds.ReadXml(new XmlNodeReader(doc));
                var ModelHDR = methodHeader.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new DataTable[] { ds.Tables[0] });
                var ModelDTL = methodDetail.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + strClassDetail).Value, new DataTable[] { ds.Tables[1] });

                return Json(new { ModelHDR, ModelDTL }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(url + "||" + ex.Message);
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ReadIsiXML(string isiXML, string strClassHeader, string strClassDetail, string _menuId)
        {

            string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).PosisiLokasiKerjaId.ToString();
            string lokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(User.Identity.Name).LokasiKerjaId.ToString();

            string lViewName = Ptpn8.Models.CRUD.CRUDMenu.CRUD.Get1Record(Guid.Parse(_menuId)).ListViewName;
            string listViewName = lViewName.Replace("[MENUID]", _menuId).Replace("[POSISILOKASIKERJAID]", posisiLokasiKerjaId.ToString()).Replace("[LOKASIKERJAID]", lokasiKerjaId.ToString()).Replace("[USERNAME]", HttpContext.User.Identity.Name);

            //Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + _menuId + posisiLokasiKerjaId];
            Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Application["Input" + listViewName];

            MethodInfo methodHeader = _app.FirstOrDefault(o => o.Key == "classToolsHeader").Value.GetType().GetMethod("DataTableToModelList");
            MethodInfo methodDetail = _app.FirstOrDefault(o => o.Key == "classToolsDetail_" + strClassDetail).Value.GetType().GetMethod("DataTableToModelList");
            XmlDocument doc = new XmlDocument();
            DataSet ds = new DataSet();
            try
            {
                doc.PreserveWhitespace = false;
                doc.LoadXml(isiXML);
                ds.ReadXml(new XmlNodeReader(doc));
                var ModelHDR = methodHeader.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsHeader").Value, new DataTable[] { ds.Tables[0] });
                var ModelDTL = methodDetail.Invoke(_app.FirstOrDefault(o => o.Key == "classToolsDetail_" + strClassDetail).Value, new DataTable[] { ds.Tables[1] });

                return Json(new { ModelHDR, ModelDTL }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }
        }

        [Authorize]
        [HttpPost, ValidateInput(false)]
        public ActionResult KirimEmail(string _from, string _to, string _subject, string _body)
        {
            try
            {
                if (_to.IndexOf("@") == -1 || _to.IndexOf(",") > -1)
                {
                    return Json("true", JsonRequestBehavior.AllowGet);
                }

                //_to = "bdhendra2012@gmail.com";
                SmtpClient mySmtpClient = new SmtpClient();
                mySmtpClient.Credentials = new System.Net.NetworkCredential(_from, "Initial.08");
                mySmtpClient.Port = 587;
                //mySmtpClient.Timeout= 10000;
                mySmtpClient.Host = "mail.ptpn8.co.id";
                mySmtpClient.EnableSsl = true;
                mySmtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;

                MailMessage mailMessage = new MailMessage();
                String[] addr = _to.Split(';');
                mailMessage.From = new MailAddress(_from);
                for (byte i = 0; i < addr.Length; i++)
                {
                    if (addr[i].Trim() != "")
                    {
                        mailMessage.To.Add(addr[i]);
                        mailMessage.ReplyToList.Add(addr[i]);
                    }
                }
                mailMessage.Subject = _subject;
                mailMessage.Body = _body;
                mailMessage.IsBodyHtml = true;
                mailMessage.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

                mySmtpClient.Send(mailMessage);

                return Json("true", JsonRequestBehavior.AllowGet);
            }
            catch (Exception x)
            {
                return Json("false", JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SaveUploadFile(IEnumerable<HttpPostedFileBase> files, string folder="", string unit="")
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder==""?"~/UploadFile":folder), unit!=""?unit+"_"+fileName:fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                        
                    }
                    catch
                    {
                        
                    }
                }
            }
            return Content("");
        }

        public ActionResult RemoveUploadFile(string[] fileNames, string folder="", string unit = "")
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/UploadFile" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveBuktiBayar(IEnumerable<HttpPostedFileBase> files, string folder = "", string unit = "")
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/UploadFile" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveBuktiBayar(string[] fileNames, string folder = "", string unit = "")
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/UploadFile" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveLampiranFile(IEnumerable<HttpPostedFileBase> files, string folder = "", string unit = "")
        {
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile2(IEnumerable<HttpPostedFileBase> files2, string folder = "", string unit = "")
        {
            if (files2 != null)
            {
                foreach (var file in files2)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile3(IEnumerable<HttpPostedFileBase> files3, string folder = "", string unit = "")
        {
            if (files3 != null)
            {
                foreach (var file in files3)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile4(IEnumerable<HttpPostedFileBase> files4, string folder = "", string unit = "")
        {
            if (files4 != null)
            {
                foreach (var file in files4)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile5(IEnumerable<HttpPostedFileBase> files5, string folder = "", string unit = "")
        {
            if (files5 != null)
            {
                foreach (var file in files5)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile6(IEnumerable<HttpPostedFileBase> files6, string folder = "", string unit = "")
        {
            if (files6 != null)
            {
                foreach (var file in files6)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile7(IEnumerable<HttpPostedFileBase> files7, string folder = "", string unit = "")
        {
            if (files7 != null)
            {
                foreach (var file in files7)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile8(IEnumerable<HttpPostedFileBase> files8, string folder = "", string unit = "")
        {
            if (files8 != null)
            {
                foreach (var file in files8)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult SaveLampiranFile9(IEnumerable<HttpPostedFileBase> files9, string folder = "", string unit = "")
        {
            if (files9 != null)
            {
                foreach (var file in files9)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    try
                    {
                        file.SaveAs(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveLampiranFile(string[] fileNames, string folder = "", string unit = "")
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    fileName = fileName.Replace("&", "dan").Replace(",", "-").Replace("'", "");
                    var physicalPath = Path.Combine(Server.MapPath(folder == "" ? "~/LampiranFileKerjasama" : folder), unit != "" ? unit + "_" + fileName : fileName);
                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        {
                        }
                    }
                }
            }

            return Content("");
        }

        [Authorize]
        [HttpPost]
        public ActionResult StringDecode(string str)
        {
            string result = HttpUtility.HtmlDecode(str);
            return Json(result);
        }

        [HttpPost]
        public ActionResult hapusContextParameter(string namaContext)
        {
            HttpContext.Session[namaContext] = null;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult buatContextParameter(string namaContext, string isiContext)
        {
            HttpContext.Session[namaContext] = isiContext;
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult bacaContextParameter(string namaContext)
        {
            return Json((string)HttpContext.Session[namaContext], JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult terkoneksiAtauTidak(string namaUser)
        {
            string result;
            PenggunaPortalYangAktif model = CRUDPenggunaPortalYangAktif.CRUD.Get1Record(namaUser);
            if(model!=null)
            {
                result = "TERKONEKSI";
            }
            else
            {
                result = "TIDAK TERKONEKSI";
            }

            return Json((string)result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult hapusContextAssesment(string namaContext)
        {
            string result="ok";
            HttpContext.Application[namaContext] = null;
            return Json((string)result, JsonRequestBehavior.AllowGet);
        }

        public static string GetLocalIPAddress()
        {
            var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "";
        }

        public string GetPublicIPAddress()
        {
            string ipAddress = HttpContext.Request.UserHostAddress.ToString();
            return ipAddress;
        }

        public ActionResult SaveFoto1(IEnumerable<HttpPostedFileBase> files, string fn = "")
        {
            //string[] listFile;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 500 || img.Height > 500)
                        {
                            img.Resize(1000, 1000);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto1(string[] fileNames, string fn = "")
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveFoto2(IEnumerable<HttpPostedFileBase> files2, string fn = "")
        {
            //string[] listFile;
            if (files2 != null)
            {
                foreach (var file in files2)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 500 || img.Height > 500)
                        {
                            img.Resize(1000, 1000);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto2(string[] fileNames2, string fn = "")
        {
            if (fileNames2 != null)
            {
                foreach (var fullName in fileNames2)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveFoto3(IEnumerable<HttpPostedFileBase> files3, string fn = "")
        {
            //string[] listFile;
            if (files3 != null)
            {
                foreach (var file in files3)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 500 || img.Height > 500)
                        {
                            img.Resize(1000, 1000);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto3(string[] fileNames3, string fn = "")
        {
            if (fileNames3 != null)
            {
                foreach (var fullName in fileNames3)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveFoto4(IEnumerable<HttpPostedFileBase> files4, string fn = "")
        {
            //string[] listFile;
            if (files4 != null)
            {
                foreach (var file in files4)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 500 || img.Height > 500)
                        {
                            img.Resize(1000, 1000);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto4(string[] fileNames4, string fn = "")
        {
            if (fileNames4 != null)
            {
                foreach (var fullName in fileNames4)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveFoto5(IEnumerable<HttpPostedFileBase> files5, string fn = "")
        {
            //string[] listFile;
            if (files5 != null)
            {
                foreach (var file in files5)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 500 || img.Height > 500)
                        {
                            img.Resize(1000, 1000);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto5(string[] fileNames5, string fn = "")
        {
            if (fileNames5 != null)
            {
                foreach (var fullName in fileNames5)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }
		
		public ActionResult SaveFoto6(IEnumerable<HttpPostedFileBase> files6, string fn = "")
        {
            //string[] listFile;
            if (files6 != null)
            {
                foreach (var file in files6)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(750, 750);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto6(string[] fileNames6, string fn = "")
        {
            if (fileNames6 != null)
            {
                foreach (var fullName in fileNames6)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }
        public ActionResult SaveFoto7(IEnumerable<HttpPostedFileBase> files7, string fn = "")
        {
            //string[] listFile;
            if (files7 != null)
            {
                foreach (var file in files7)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(750, 750);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto7(string[] fileNames7, string fn = "")
        {
            if (fileNames7 != null)
            {
                foreach (var fullName in fileNames7)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

        public ActionResult SaveFoto8(IEnumerable<HttpPostedFileBase> files8, string fn = "")
        {
            //string[] listFile;
            if (files8 != null)
            {
                foreach (var file in files8)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(750, 750);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto8(string[] fileNames8, string fn = "")
        {
            if (fileNames8 != null)
            {
                foreach (var fullName in fileNames8)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }
        public ActionResult SaveFoto9(IEnumerable<HttpPostedFileBase> files9, string fn = "")
        {
            //string[] listFile;
            if (files9 != null)
            {
                foreach (var file in files9)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(750, 750);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto9(string[] fileNames9, string fn = "")
        {
            if (fileNames9 != null)
            {
                foreach (var fullName in fileNames9)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }
        public ActionResult SaveFoto10(IEnumerable<HttpPostedFileBase> files10, string fn = "")
        {
            //string[] listFile;
            if (files10 != null)
            {
                foreach (var file in files10)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }
                    try
                    {
                        WebImage img = new WebImage(file.InputStream);
                        if (img.Width > 100 || img.Height > 100)
                        {
                            img.Resize(750, 750);
                        }
                        img.Save(physicalPath);
                    }
                    catch
                    { }
                }
            }
            return Content("");
        }

        public ActionResult RemoveFoto10(string[] fileNames10, string fn = "")
        {
            if (fileNames10 != null)
            {
                foreach (var fullName in fileNames10)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = "";
                    if (fn == "")
                    {
                        physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
                    }
                    else
                    {
                        physicalPath = Path.Combine(Server.MapPath(fn), fileName);
                    }


                    if (System.IO.File.Exists(physicalPath))
                    {
                        try
                        {
                            System.IO.File.Delete(physicalPath);
                        }
                        catch
                        { }
                    }
                }
            }

            return Content("");
        }

    }
}
