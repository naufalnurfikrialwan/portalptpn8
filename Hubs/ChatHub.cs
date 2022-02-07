using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace Ptpn8.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.hubReceived("Selamat datang " + Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama + "!");
        }

        public void Broadcast(string roomName, string message)
        {
            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o=>o.Tanggal.Day<DateTime.Now.Day).ToList();
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Delete(models);

            var model = new Ptpn8.Models.Chat();
            model.ChatId = Guid.NewGuid();
            model.ChatRoom = roomName;
            model.Tanggal = DateTime.Now;
            model.UserId = Guid.Parse(Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Id);
            model.UserName = Context.User.Identity.Name;
            model.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama;
            model.TextMessage = message;
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Create(model);

            Clients.All.broadcastMessage(Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().OrderByDescending(o=>o.Tanggal));
        }

        public void BroadcastMessage(string message)
        {
            Clients.All.broadcastMessage(message);
        }

        public void AjakChat(string yangDiajak, string namaYangDiajak)
        {
            string namaYangMengajak = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama;
            string yangMengajak = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).UserName;
            string chatRoom = yangMengajak + "_DENGAN_" + yangDiajak;

            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o => o.ChatRoom == chatRoom && o.Tanggal.Day < DateTime.Now.Day).ToList(); // hapus chat hari lalu
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Delete(models);
            models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o => o.ChatRoom == chatRoom && o.Tanggal.Day == DateTime.Now.Day).ToList();
            if (models.Count > 0 || yangMengajak=="bdhendra@yahoo.com")
            {
                Clients.User(yangDiajak).forceSetujuChat(namaYangMengajak, yangMengajak, namaYangDiajak, yangDiajak);
            }
            else
            {
                Clients.User(yangDiajak).ajakChat(namaYangMengajak, yangMengajak, namaYangDiajak, yangDiajak);
            }
        }

        public void ForceSetujuChat(string yangMengajak, string yangDiajak, string chatRoom)
        {
            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o => o.ChatRoom == chatRoom && o.Tanggal.Day < DateTime.Now.Day).ToList(); // hapus chat hari lalu
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Delete(models);
            Clients.User(yangMengajak).setujuChat(chatRoom);
        }

        public void SetujuChat(string yangMengajak, string yangDiajak, string chatRoom)
        {
            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o =>o.ChatRoom==chatRoom && o.Tanggal.Day < DateTime.Now.Day).ToList(); // hapus chat hari lalu
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Delete(models);
            Clients.User(yangMengajak).setujuChat(chatRoom);
            Clients.User(yangDiajak).setujuChat(chatRoom);
        }

        public void TolakChat(string yangMengajak)
        {
            Clients.User(yangMengajak).tolakChat();
        }

        public void SendMessage(string chatRoom, string txt)
        {
            var model = new Ptpn8.Models.Chat();
            //model.ChatId = Guid.NewGuid();
            model.ChatRoom = chatRoom;
            model.Tanggal = DateTime.Now;
            model.Nama = Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama;
            model.UserName = Context.User.Identity.Name;
            model.TextMessage = txt;
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Create(model);
            string yangMengajak = chatRoom.Substring(0, chatRoom.IndexOf("_DENGAN_"));
            string yangDiajak= chatRoom.Substring(chatRoom.IndexOf("_DENGAN_")+8);
            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o => o.ChatRoom == chatRoom).OrderByDescending(o => o.Tanggal).ToList();
            var json = JsonConvert.SerializeObject(models, Formatting.Indented, new JsonSerializerSettings() { DateFormatString = "dd-MM-yyyy" });

            Clients.User(yangMengajak).gridMessage(json);
            Clients.User(yangDiajak).gridMessage(json);

        }

        public void LeaveMessage(string chatRoom)
        {
            var models = Ptpn8.Models.CRUD.CRUDChat.CRUD.GetAllRecord().Where(o => o.ChatRoom == chatRoom && o.Tanggal.Day == DateTime.Now.Day).ToList(); // hapus chat hari ini
            Ptpn8.Models.CRUD.CRUDChat.CRUD.Delete(models);
            string yangMengajak = chatRoom.Substring(0, chatRoom.IndexOf("_DENGAN_"));
            string yangDiajak = chatRoom.Substring(chatRoom.IndexOf("_DENGAN_") + 8);
            Clients.User(yangMengajak).leaveMessage();
            Clients.User(yangDiajak).leaveMessage();

        }
    }
}