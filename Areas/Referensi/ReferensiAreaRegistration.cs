using System.Web.Mvc;

namespace Ptpn8.Areas.Referensi
{
    public class ReferensiAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Referensi";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Referensi_default",
                "Referensi/{controller}/{action}/{id}",
                new { Controller="Home", action = "Index", id = UrlParameter.Optional }
                , new[] { "Ptpn8.Areas.Referensi.Controllers" }
            );
        }
    }
}