using System.Web.Mvc;

namespace Ptpn8.Areas.NDNK
{
    public class NDNKAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "NDNK";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "NDNK_default",
                "NDNK/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}