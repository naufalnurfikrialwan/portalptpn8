using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi.Controllers
{
    public class imgController : Controller
    {
        public ActionResult SaveFoto(IEnumerable<HttpPostedFileBase> files)
        {
            //string[] listFile;
            if (files != null)
            {
                foreach (var file in files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);
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

        public ActionResult RemoveFoto(string[] fileNames)
        {
            if (fileNames != null)
            {
                foreach (var fullName in fileNames)
                {
                    var fileName = Path.GetFileName(fullName);
                    var physicalPath = Path.Combine(Server.MapPath("~/Content/Images/Upload"), fileName);


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