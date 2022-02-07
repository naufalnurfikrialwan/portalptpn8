using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web;

namespace Ptpn8.Hubs
{
    public class BPDHub :Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.hubReceived("Selamat datang " + Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama + "!");
        }

        public void TambahData()
        {
            Clients.User("ujang.gdd@gmail.com").klikButtonProses();
            Clients.User("hanggariawan@gmail.com").klikButtonProses();
            Clients.User("ahmadsukmi@gmail.com").klikButtonProses();
        }

    }

}