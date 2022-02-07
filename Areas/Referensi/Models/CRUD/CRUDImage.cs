using System.Drawing;
using System.IO;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDImage
    {
        public static CRUDImage CRUD = new CRUDImage();

        public void ReadToView(string fileName, byte[] img)
        {
            try
            {
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    Image i = Image.FromStream(ms);
                    string physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/View"), fileName);
                    if (File.Exists(physicalPath)) File.Delete(physicalPath);
                    i.Save(physicalPath);
                }
            }
            catch
            {
            }
        }

        public void ReadToView(string fileName, byte[] img, string path)
        {
            try
            {
                if (img != null)
                {
                    MemoryStream ms = new MemoryStream(img);
                    Image i = Image.FromStream(ms);
                    string physicalPath = Path.Combine(HttpContext.Current.Server.MapPath(path), fileName);
                    if (File.Exists(physicalPath)) File.Delete(physicalPath);
                    i.Save(physicalPath);
                }
            }
            catch
            {
            }
        }

        public byte[] GetFromUpload(string fileName, byte[] img)
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

        public void RemoveFileView(string fileName)
        {
            try
            {
                string physicalPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Content/Images/View"), fileName);
                System.IO.File.Delete(physicalPath);
            }
            catch
            {
            }
        }
    }
}