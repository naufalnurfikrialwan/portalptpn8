using System.Web.Mvc;

namespace Ptpn8.Areas.Areal
{
    public class ArealAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Areal";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Areal_default",
                "Areal/{controller}/{action}/{id}",
                new { Controller = "Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "Ptpn8.Areas.Areal.Controllers" }
            );
        }
    }
}