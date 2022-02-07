using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace Ptpn8.Models
{
    public class CRUDTable
    {
        public static CRUDTable crud = new CRUDTable();
        public List<T> Read<T>(string cs = "", string namaTable="", string where="") where T : class
        {
            
            var result = new List<T>();
            try
            {
                if (cs == "" || namaTable=="") return result;
                string scriptSQL = "select * from " + namaTable + (where != "" ? " where " + where : "");
                List<PropertyInfo> modelProperties = Activator.CreateInstance<T>().GetType().GetProperties().OrderBy(m => m.MetadataToken).ToList();
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var classResult = Activator.CreateInstance<T>();
                            for (int i = 0; i < modelProperties.Count; i++)
                                try
                                {
                                    modelProperties[i].SetValue(classResult, reader.GetValue(reader.GetOrdinal(modelProperties[i].Name)), null);
                                }
                                catch (Exception x)
                                {
                                    continue;
                                }

                            result.Add((T)classResult);
                        };
                    }
                    conn.Close();
                }
            }
            catch (Exception x)
            {
            }
            return result;  
        }
        public bool Delete(string cs = "", string namaTable = "", string where = "")
        {
            try
            {
                if (cs == "" || namaTable == "") return false;
                string nmTable = namaTable.IndexOf(" ") > 0 ? namaTable.Substring(0, namaTable.IndexOf(" ")) : namaTable;
                string scriptSQL = "delete " + nmTable + (where != "" ? " where " + where : "");
                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.CommandText = scriptSQL;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Insert<T>(T classObj, string cs = "", string namaTable = "") where T : class
        {
            try
            {
                if (cs == "" || namaTable == "") return false;
                List<PropertyInfo> modelProperties = classObj.GetType().GetProperties().ToList();
                Dictionary<string, object> modelDictionary = new Dictionary<string, object>();
                for (int i = 0; i < modelProperties.Count; i++)
                {
                    var attr = modelProperties[i].CustomAttributes.Where(o => o.AttributeType.Name == "NotMappedAttribute").ToList();
                    if (attr.Count() == 0 && modelProperties[i].CanWrite == true)
                    {
                        modelDictionary.Add(modelProperties[i].Name, modelProperties[i].GetValue(classObj, null));
                    }
                }

                var fields = new List<string>();
                var parameters = new List<SqlParameter>();
                var j = 0;
                foreach (var item in modelDictionary)
                {
                    if (item.Value != null)
                    {
                        fields.Add("["+item.Key+"]");
                        parameters.Add(new SqlParameter("@sp" + j.ToString(), item.Value));
                    }
                    j++;
                }

                string nmTable = namaTable.IndexOf(" ") > 0 ? namaTable.Substring(0, namaTable.IndexOf(" ")) : namaTable;
                string scriptSQL = string.Format("insert into " + nmTable  + " ({0}) values ({1})",
                    string.Join(", ", fields.ToArray()),
                    string.Join(", ", parameters.Select(o => o.ParameterName).ToArray()));
                

                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.CommandText = scriptSQL;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                    return false;
            }
        }
        public bool Update<T>(T classObj, string cs = "", string namaTable = "", string where = "") where T : class
        {
            try
            {
                if (cs == "" || namaTable == "") return false;
                List<PropertyInfo> modelProperties = classObj.GetType().GetProperties().ToList();
                Dictionary<string, object> modelDictionary = new Dictionary<string, object>();

                for (int i = 0; i < modelProperties.Count; i++)
                {
                    var attr = modelProperties[i].CustomAttributes.Where(o => o.AttributeType.Name == "NotMappedAttribute").ToList();
                    if (attr.Count() == 0 && modelProperties[i].CanWrite==true)
                    {
                        modelDictionary.Add(modelProperties[i].Name, modelProperties[i].GetValue(classObj, null));
                    }
                }

                var equals = new List<string>();
                var parameters = new List<SqlParameter>();
                var j = 0;
                foreach (var item in modelDictionary)
                {
                    if (item.Value != null)
                    {
                        var pn = "@sp" + j.ToString();
                        equals.Add(string.Format("{0}={1}", "["+item.Key+"]", pn));
                        parameters.Add(new SqlParameter(pn, item.Value));
                    }
                    j++;
                }

                string nmTable = namaTable.IndexOf(" ") > 0 ? namaTable.Substring(0, namaTable.IndexOf(" ")) : namaTable;
                string scriptSQL = string.Format("update " + nmTable + " set {0} " + (where!="" ? " where {1} " : ""),
                    string.Join(", ", equals.ToArray()), where);

                using (var conn = new SqlConnection(cs))
                using (var cmd = conn.CreateCommand())
                {
                    conn.Open();
                    cmd.Parameters.AddRange(parameters.ToArray());
                    cmd.CommandText = scriptSQL;
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}