using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System.Linq;
using System.Web.Mvc;

namespace Ptpn8.Controllers
{
    public class ChatController : Controller
    {
        // GET: Chat
        public ActionResult Read([DataSourceRequest] DataSourceRequest request, string roomName)
        {
            try
            {
                var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o=>o.ChatRoom==roomName);
                return Json(models.OrderByDescending(o=>o.Tanggal).ToDataSourceResult(request));
            }
            catch
            {
                return Json(new object[] { }.ToDataSourceResult(request));
            }
        }
    }
}