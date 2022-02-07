using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web;

namespace Ptpn8.Hubs
{
    public class ProgressHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.hubReceived("Selamat datang " + Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama + "!");
        }

        public void Join()
        {
            string userName = HttpContext.Current.User.Identity.Name;

        }

        //public void addProgress(string progressMessage, int progressCount, int totalItems)
        //{
        //    var percentage = (progressCount * 100) / totalItems;
        //    Clients.Caller.AddProgress(progressMessage, percentage);
        //}
    }

    public class Functions
    {
        public static void addProgress(string progressMessage, int progressCount, int totalItems)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            var percentage = (progressCount * 100) / totalItems;
            string userName = HttpContext.Current.User.Identity.Name;
            hubContext.Clients.User(userName).addProgress(progressMessage, percentage);
        }

        public static void setDataSource1(string ds)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            string userName = HttpContext.Current.User.Identity.Name;
            hubContext.Clients.User(userName).setDataSource1(ds);
        }

        public static void setDataSource2(string ds)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<ProgressHub>();
            string userName = HttpContext.Current.User.Identity.Name;
            hubContext.Clients.User(userName).setDataSource2(ds);
        }

       
    }

}