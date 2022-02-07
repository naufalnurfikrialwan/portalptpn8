using Omu.ValueInjecter;
using Ptpn8.Areas.Referensi.Models.CRUD;
using Ptpn8.Models.CRUD;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;


namespace Ptpn8.Models
{
    public class CRUDView<T1,T2>     // T1: class view, T2: class table fisik
        where T1 : class
        where T2 : class
    {
        public static CRUDView<T1,T2> crud = new CRUDView<T1,T2>();

        private string _cs;
        private string[] _csServerLain;
        private string _scriptTriggerServerLain;
        private string _tableName;
        private string _listViewName;
        private string _userName;
        private int _TahunBuku;
        private string _namaHttpContext;

        private Dictionary<string, object> _keyTable;
        public string cs                // connection string
        {      
            get
            {
                return _cs;
            }
            set
            {
                _cs = value;
            }
        }
        public string[] csServerLain                // connection string
        {
            get
            {
                return _csServerLain;
            }
            set
            {
                _csServerLain = value;
            }
        }
        public string scriptTriggerServerLain
        {
            get
            {
                return _scriptTriggerServerLain;
            }
            set
            {
                _scriptTriggerServerLain = value;
            }
        }
        public string tableName         // nama table
        {
            get
            {
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }                           
        public string listViewName      // nama list view di httpcontext
        {
            get
            {
                return _listViewName;
            }
            set
            {
                _listViewName = value;
            }
        }                               
        public Dictionary<string, object> keyTable // Dictionary yang berisi nama field key dan isinya
        {
            get
            {
                return _keyTable;
            }
            set
            {
                _keyTable = value;
            }
        }
        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        public int TahunBuku
        {
            get { return _TahunBuku; }
            set { _TahunBuku = value; }
        }
        public string namaHttpContext
        {
            get { return _namaHttpContext; }
            set { _namaHttpContext = value; }
        }
        public string NamaAplikasi;
        public string NamaMenu;
        public string MenuId;
        public List<object[]> ListView
        {
            get
            {
                if (listViewName == null) return new List<object[]>();

                List<object[]> lstResult = (List<object[]>)HttpContext.Current.Application[listViewName];
                if(lstResult==null || lstResult.Count==0)
                {
                    object[] result = new object[3];
                    result[0] = HttpContext.Current.User.Identity.Name;
                    result[1] = TahunBuku;
                    result[2] = Read();
                    lstResult = new List<object[]>();
                    lstResult.Add(result);
                    HttpContext.Current.Application[listViewName] = lstResult;
                    setOwner(true);
                    return lstResult;
                }

                object[] objResult = new object[3];
                foreach (var listView in lstResult)
                {
                    if ((int)listView[1] == TahunBuku)
                    {
                        objResult = listView;
                        break;
                    }
                }

                if (objResult[0] == null)
                {
                    lstResult.Remove(objResult);
                    object[] result = new object[3];
                    result[0] = HttpContext.Current.User.Identity.Name;
                    result[1] = TahunBuku;
                    result[2] = Read();
                    lstResult.Add(result);
                    HttpContext.Current.Application[listViewName] = lstResult;
                    var lst = new List<object[]>();
                    lst.Add(result);
                    setOwner(true);
                    return lst;
                }
                else
                {
                    lstResult = new List<object[]>();
                    lstResult.Add(objResult);
                    if (objResult[0].ToString() == HttpContext.Current.User.Identity.Name)
                        setOwner(true);
                    else
                        setOwner(false);
                    return lstResult;
                }
            }
            set {}
        }
        public void setOwner(bool st)
        {
            string namaMenu = CRUDMenu.CRUD.Get1Record(Guid.Parse(MenuId)).LinkText;
            var current = Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.GetAllRecord().Where(o => o.UserName == HttpContext.Current.User.Identity.Name && o.Menu == namaMenu).FirstOrDefault();
            if (current != null)
            {
                current.StatusOwner = st;
                Ptpn8.Models.CRUD.CRUDPenggunaPortalYangAktif.CRUD.Update(current);
            }
        }
        private string createCond()
        {
            if (keyTable != null)
            {
                string[] arr = new string[keyTable.Count];
                int i = 0;
                object[] arrObj = new object[2];
                foreach (var k in keyTable)
                {
                    arrObj = (object[]) k.Value;
                    if (arrObj[1] == null || arrObj[1].ToString() == "") arrObj[1] = "=";
                    if ((string)arrObj[1] == "in")
                        arr[i] = string.Format("{0} "+arrObj[1]+" {1}", k.Key.ToString(), arrObj[0].ToString());
                    else
                        arr[i] = string.Format("{0} " + arrObj[1] + " '{1}'", k.Key.ToString(), arrObj[0].ToString());
                    i++;
                }
                return string.Join(" and ", arr);
            }
            return "";
        }
        public List<T1> Read()
        {
            try
            {
                List<T1> result = new List<T1>();
                string readCond = createCond();
                if (tableName.IndexOf("[") == -1)
                {
                    var model = CRUDTable.crud.Read<T2>(cs, tableName, readCond);
                    foreach (var m in model)
                    {
                        var n = Activator.CreateInstance<T1>();
                        n.InjectFrom(m);
                        result.Add(n);
                    }

                    if (namaHttpContext != null && HttpContext.Current.Application[namaHttpContext]==null)
                    {
                        HttpContext.Current.Application[namaHttpContext] = result;
                    }
                }
                else
                {
                    var namaContext = tableName.Substring(tableName.IndexOf("[") + 2);
                    namaContext = namaContext.Substring(0, namaContext.IndexOf("]") - 1);
                    result = (List<T1>)HttpContext.Current.Application[namaContext];
                }
                return result;
            }
            catch
            {
                return new List<T1>();
            }
        }
        public bool Insert(object M)
        {
            string updateCond,cSQL;
            var modelInsert = ((IList)M).Cast<object>().ToList();
            T1 m2;
            T2 m3;
            try
            {
                foreach (var m1 in modelInsert)
                {
                    m2 = Activator.CreateInstance<T1>();
                    m2.InjectFrom(m1);

                    m3 = Activator.CreateInstance<T2>();
                    m3.InjectFrom(m1);

                    List<string> keys = new List<string>(keyTable.Keys);
                    foreach (string key in keys)
                    {
                        object[] ky = new object[] { };
                        ky = (object[])keyTable[key];
                        try
                        {
                            ky[0] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                            //keyTable[key] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                        }
                        catch
                        {
                            try
                            {
                                ky[0] = (string)m2.GetType().GetProperty(key).GetValue(m2);
                                //keyTable[key] = (string)m2.GetType().GetProperty(key).GetValue(m2);
                            }
                            catch
                            {

                            }
                        }
                    }

                    updateCond = createCond();
                    if (CRUDTable.crud.Insert<T2>(m3, cs, tableName))
                    {
                        CreateList(m2);
                        if (csServerLain.Length > 0)
                        {
                            for (int i = 0; i < csServerLain.Length; i++)
                            {
                                if (CRUDTable.crud.Insert<T2>(m3, csServerLain[i], tableName))
                                {
                                    cSQL = scriptTriggerServerLain + " " + updateCond;
                                    ExecSQL(cs, cSQL);
                                }
                            }
                        }
                    }
                    else
                        return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Update(object M)
        {
            string cSQL;
            var modelUpdate = ((IList)M).Cast<object>().ToList();
            string updateCond;
            T1 m2;
            T2 m3;
            var con= new SqlConnection(cs);
            DataTable tap;
            string scriptSQL;

            try
            {
                if (keyTable.Count == 0) return false;
                foreach(var m1 in modelUpdate)
                {
                    m2 = Activator.CreateInstance<T1>();
                    m2.InjectFrom(m1);

                    m3 = Activator.CreateInstance<T2>();
                    m3.InjectFrom(m1);

                    List<string> keys = new List<string>(keyTable.Keys);
                    foreach (string key in keys)
                    {
                        object[] ky = new object[] { };
                        ky = (object[])keyTable[key];
                        try
                        {
                            ky[0] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                            //keyTable[key] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                        }
                        catch
                        {
                            try
                            {
                                ky[0] = (string)m2.GetType().GetProperty(key).GetValue(m2);
                                //keyTable[key] = (string)m2.GetType().GetProperty(key).GetValue(m2);
                            }
                            catch
                            {

                            }
                        }
                    }

                    updateCond = createCond();
                    con = new SqlConnection(cs);
                    con.Open();
                    tap = new DataTable();
                    scriptSQL = string.Format("select count(*) from " + tableName + (updateCond != "" ? " where {0} " : ""), updateCond);
                    new SqlDataAdapter(scriptSQL, con).Fill(tap);
                    con.Close();
                    if ((int)tap.Rows[0][0] == 0)
                    {
                        // insert
                        if (CRUDTable.crud.Insert<T2>(m3, cs, tableName))
                        {
                            UpdateList(m2);
                            if (csServerLain.Length > 0)
                            {
                                for (int i = 0; i < csServerLain.Length; i++)
                                {
                                    if (CRUDTable.crud.Insert<T2>(m3, csServerLain[i], tableName))
                                    {
                                        updateCond = createCond();
                                        cSQL = scriptTriggerServerLain + " " + updateCond;
                                        ExecSQL(cs, cSQL);
                                    }

                                }
                            }
                        }
                        else
                            return false;
                    }
                    else
                    {
                        // update
                        if (CRUDTable.crud.Update<T2>(m3, cs, tableName, updateCond))
                        {
                            UpdateList(m2);

                            if (csServerLain.Length > 0)
                            {
                                for (int i = 0; i < csServerLain.Length; i++)
                                {
                                    if (CRUDTable.crud.Insert<T2>(m3, csServerLain[i], tableName) == true)
                                    {
                                        updateCond = createCond();
                                        cSQL = scriptTriggerServerLain + " " + updateCond;
                                        ExecSQL(cs, cSQL);
                                    }
                                    else
                                    {
                                        if (CRUDTable.crud.Update<T2>(m3, csServerLain[i], tableName, updateCond))
                                        {
                                            updateCond = createCond();
                                            cSQL = scriptTriggerServerLain + " " + updateCond;
                                            ExecSQL(cs, cSQL);
                                        }
                                    }
                                }
                            }
                        }
                        else
                            return false;
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
        public bool Delete(object M)
        {
            try
            {
                if (keyTable.Count == 0) return false;

                string deleteCond = createCond();
                var modelDelete = ((IList)M).Cast<object>().ToList();
                T1 m2;
                T2 m3;

                foreach(var m1 in modelDelete)
                {
                    m2 = Activator.CreateInstance<T1>();
                    m2.InjectFrom(m1);

                    m3 = Activator.CreateInstance<T2>();
                    m3.InjectFrom(m1);

                    List<string> keys = new List<string>(keyTable.Keys);
                    foreach (string key in keys)
                    {
                        object[] ky = new object[] { };
                        ky = (object[])keyTable[key];

                        try
                        {
                            ky[0] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                            //keyTable[key] = (Guid)m2.GetType().GetProperty(key).GetValue(m2);
                        }
                        catch
                        {
                            try
                            {
                                ky[0]= (string)m2.GetType().GetProperty(key).GetValue(m2);
                                //keyTable[key] = (string)m2.GetType().GetProperty(key).GetValue(m2);
                            }
                            catch
                            {

                            }
                        }
                    }
                    deleteCond = createCond();

                    if (CRUDTable.crud.Delete(cs, tableName, deleteCond))
                    {
                        DeleteList(m2);
                        if (csServerLain.Length > 0)
                        {
                            for (int i = 0; i < csServerLain.Length; i++)
                            {
                                CRUDTable.crud.Delete(csServerLain[i], tableName, deleteCond);
                            }
                        }
                    }
                    else
                        return false;
                }
            }
            catch 
            {
                return false;
            }
            return true;
        }
        public bool CreateList(T1 model)
        {

            if (namaHttpContext != null && HttpContext.Current.Application[namaHttpContext] != null)
            {
                var models = (List<T1>)HttpContext.Current.Application[namaHttpContext];
                models.Add(model);
                HttpContext.Current.Application[namaHttpContext] = models;
            }

            if (listViewName == null) return true;
            try
            {
                var _ListView = (List<object[]>) HttpContext.Current.Application[listViewName];
                for(int i=0; i<_ListView.Count;i++)
                {
                    var listView = _ListView[i];
                    if((int)listView[1]==TahunBuku)
                    {
                        _ListView.Remove(listView);
                        object[] result = new object[3];
                        result[0] = listView[0];
                        result[1] = listView[1];
                        var mdl = (List<T1>)listView[2];


                        string posisiLokasiKerjaId = CRUDLokasiKerja.CRUD.LoginLokasiKerja(HttpContext.Current.User.Identity.Name).PosisiLokasiKerjaId.ToString();
                        Dictionary<string, object> _app = (Dictionary<string, object>)HttpContext.Current.Application["Input" + MenuId + posisiLokasiKerjaId];
                        string noInputView = model.GetType().GetProperty((string)_app.FirstOrDefault(o => o.Key == "FieldNomorInputView")
                            .Value).GetValue(model).ToString().ToLower().Trim();

                        //string noInputView = model.GetType().GetProperty("NomorInputView").GetValue(model).ToString().ToLower().Trim();
                        if (noInputView.Contains("data baru"))
                        {
                            mdl.Add(model);
                        }
                        else
                        {
                            int idx = mdl.Count - 1;
                            var mdlNomorBaru = mdl[idx];
                            mdlNomorBaru.InjectFrom(model);
                        }
                        result[2] = mdl;
                        _ListView.Add(result);
                    }
                }
                HttpContext.Current.Application[listViewName] = _ListView;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateList(T1 model)
        {
            if (namaHttpContext != null && HttpContext.Current.Application[namaHttpContext] != null)
            {
                var models = (List<T1>)HttpContext.Current.Application[namaHttpContext];
                var modelUpd = models.SingleOrDefault(o => o.GetType().GetProperty(keyTable.SingleOrDefault().Key.ToString())
                            .GetValue(o).ToString() == keyTable.SingleOrDefault().Value.ToString());
                if (modelUpd != null)
                {
                    models.Remove(modelUpd);
                    models.Add(model);
                    HttpContext.Current.Application[namaHttpContext] = models;
                }
            }

            if (listViewName == null) return true;
            try
            {
                var _ListView = (List<object[]>)HttpContext.Current.Application[listViewName];
                for (int i = 0; i < _ListView.Count; i++)
                {
                    var listView = _ListView[i];
                    if ((int)listView[1] == TahunBuku)
                    {
                        _ListView.Remove(listView);
                        object[] result = new object[3];
                        result[0] = listView[0];
                        result[1] = listView[1];
                        var mdl = (List<T1>)listView[2];
                        var lst = mdl.SingleOrDefault(o => o.GetType().GetProperty(keyTable.SingleOrDefault().Key.ToString())
                            .GetValue(o).ToString() == keyTable.SingleOrDefault().Value.ToString());
                        if(lst!=null)
                        {
                            lst.InjectFrom(model);
                            //mdl.Remove(lst);
                            //mdl.Add(model);
                            result[2] = mdl;
                            _ListView.Add(result);
                        }
                    }
                }
                HttpContext.Current.Application[listViewName] = _ListView;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool DeleteList(T1 model)
        {
            if (namaHttpContext != null && HttpContext.Current.Application[namaHttpContext] != null)
            {
                var models = (List<T1>)HttpContext.Current.Application[namaHttpContext];
                var modelDel = models.SingleOrDefault(o => o.GetType().GetProperty(keyTable.SingleOrDefault().Key.ToString())
                            .GetValue(o).ToString() == keyTable.SingleOrDefault().Value.ToString());
                if (modelDel != null)
                {
                    models.Remove(modelDel);
                    HttpContext.Current.Application[namaHttpContext] = models;
                }
            }
            if (listViewName == null) return true;
            try
            {
                var _ListView = (List<object[]>)HttpContext.Current.Application[listViewName];
                for (int i = 0; i < _ListView.Count; i++)
                {
                    var listView = _ListView[i];
                    if ((int)listView[1] == TahunBuku)
                    {
                        _ListView.Remove(listView);
                        object[] result = new object[3];
                        result[0] = listView[0];
                        result[1] = listView[1];
                        var mdl = (List<T1>)listView[2];
                        var lst = mdl.SingleOrDefault(o => o.GetType().GetProperty(keyTable.SingleOrDefault().Key.ToString())
                            .GetValue(o).ToString() == keyTable.SingleOrDefault().Value.ToString());
                        if (lst != null)
                        {
                            mdl.Remove(lst);
                            result[2] = mdl;
                            _ListView.Add(result);
                        }
                    }
                }
                HttpContext.Current.Application[listViewName] = _ListView;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public object[] GetAllRecord(string namaAplikasi, string namaMenu, string menuId)
        {
            NamaAplikasi = namaAplikasi;
            NamaMenu = namaMenu;
            MenuId = menuId;
            var context = ListView;
            object[] result = new object[3];
            for (int i = 0; i < context.Count; i++)
            {
                var listView = context[i];
                if ((int)listView[1] == TahunBuku)
                {
                    result = context[i];
                    break;
                }
            }
            return result;
        }
        public bool ExecSQL(string cs, string scriptSQL)
        {
            SqlConnection con = new SqlConnection(cs);
            try
            {
                var cmd = con.CreateCommand();
                con.Open();
                cmd.CommandText = scriptSQL;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception)
            {
                con.Close();
                return false;
            }
            return true;
        }
    }
}
