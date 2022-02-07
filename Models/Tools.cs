using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Script.Serialization;

namespace Ptpn8.Models
{
    public class Tools<T> where T : class
    {
        public static Tools<T> toolsFunction = new Tools<T>();
        public T InisiasiMember(T content) 
        {
            List<PropertyInfo> modelProperties = content.GetType().GetProperties().OrderBy(m => m.MetadataToken).ToList();
            string memberType = "";
            object memberValue = null;
            for (int i = 0; i < modelProperties.Count; i++)
            {
                memberType = modelProperties[i].PropertyType.Name.ToLower();
                switch (memberType)
                {
                    case "long":
                        memberValue = (long)0;
                        break;
                    case "decimal":
                        memberValue = (decimal)0;
                        break;

                    case "float":
                        memberValue = (float)0;
                        break;

                    case "int16":
                        memberValue = (Int16)0;
                        break;

                    case "int32":
                        memberValue = (Int32)0;
                        break;

                    case "int64":
                        memberValue = (Int64)0;
                        break;

                    case "double":
                        memberValue = (double)0;
                        break;

                    case "short":
                        memberValue = (short)0;
                        break;
                    case "guid":
                        memberValue = Guid.Empty;
                        break;
                    case "datetime":
                        memberValue = DateTime.Now;
                        break;
                    case "char":
                    case "string":
                        memberValue = "";
                        break;
                    case "bool":
                        memberValue = false;
                        break;
                    case "byte[]":
                        memberValue = new byte { };
                        break;

                }
                if (memberValue != null) modelProperties[i].SetValue(content, memberValue, null);
            }
            return content;
        }

        public T JsonToClass(string jsonString) 
        {
            T result = Activator.CreateInstance<T>();
            if (jsonString == null) return result;
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            Dictionary<string, object> mdl = (Dictionary<string, object>)json_serializer.DeserializeObject(jsonString);

            List<PropertyInfo> modelProperties = result.GetType().GetProperties().OrderBy(m => m.MetadataToken).ToList();
            for (int i = 0; i < modelProperties.Count; i++)
            {
                try
                {
                    if (modelProperties[i].PropertyType.Name == "Byte[]")
                    {
                        string str = mdl.FirstOrDefault(o => o.Key == modelProperties[i].Name).Value.ToString();
                        string[] arrstr = str.Split(',');
                        byte[] bytes = new byte[arrstr.Length];
                        for(int ii=0;ii<arrstr.Length;ii++)
                        {
                            bytes[ii] = byte.Parse(arrstr[ii]);
                        }
                        modelProperties[i].SetValue(result, bytes);
                    }
                    else
                    {
                        modelProperties[i].SetValue(result, TypeDescriptor.GetConverter(modelProperties[i].PropertyType)
                            //.ConvertFromInvariantString(mdl.FirstOrDefault(o => o.Key == modelProperties[i].Name).Value.ToString()), null);
                            .ConvertFromString(mdl.FirstOrDefault(o => o.Key == modelProperties[i].Name).Value.ToString()));
                    }
                }
                catch
                {
                    continue;
                }
            }
            return result;
        }

        public List<T> DataTableToModelList(DataTable table) 
        {
            try
            {
                List<T> list = new List<T>();

                foreach (var row in table.AsEnumerable())
                {
                    T obj = Activator.CreateInstance<T>();
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        try
                        {
                            PropertyInfo propertyInfo = obj.GetType().GetProperty(prop.Name);
                            propertyInfo.SetValue(obj, Convert.ChangeType(row[prop.Name], propertyInfo.PropertyType), null);
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    list.Add(obj);
                }

                return list;
            }
            catch
            {
                return null;
            }
        }

        public byte[] GetFromUpload(string fileName)
        {
            string physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/Upload"), fileName);
            if (File.Exists(physicalPath))
                return System.IO.File.ReadAllBytes(physicalPath);
            else
            {
                physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/View"), fileName);
                if (!File.Exists(physicalPath))
                    physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/"), "NoPhoto.jpg");

                return System.IO.File.ReadAllBytes(physicalPath);
            }
        }
    }
}