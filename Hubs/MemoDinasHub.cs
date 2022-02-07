using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Ptpn8.Models;
namespace Ptpn8.Hubs
{
    [Authorize]
    public class MemoDinasHub : Hub
    {
        public override Task OnConnected()
        {
            return Clients.Caller.hubReceived("Selamat datang " + Ptpn8.Models.CRUDApplicationUser.CRUD.Get1Record(Context.User.Identity.Name).Nama + "!");
        }

        public void KirimMemoDinas(string aplikasiId)
        {
            List<PenggunaPortalYangAktif> model = (List<PenggunaPortalYangAktif>)System.Web.HttpContext.Current.Application["PenggunaPortalYangAktif"];
            var clientAplikasi = model.Where(o => o.AplikasiId == Guid.Parse(aplikasiId)).ToList();
            for(int i=0;i<clientAplikasi.Count;i++)
            {
                //Clients.User(clientAplikasi[i].UserName).terimaMemoDinas();
                //Clients.User(clientAplikasi[i].UserName).suratKeluarMemoDinas();
            }
        }

        public void TerimaMemoDinas(string aplikasiId)
        {
            List<PenggunaPortalYangAktif> model = (List<PenggunaPortalYangAktif>)System.Web.HttpContext.Current.Application["PenggunaPortalYangAktif"];
            var clientAplikasi = model.Where(o => o.AplikasiId == Guid.Parse(aplikasiId)).ToList();
            for (int i = 0; i < clientAplikasi.Count; i++)
            {
                //Clients.User(clientAplikasi[i].UserName).disposisiMemoDinas();
                //Clients.User(clientAplikasi[i].UserName).tindakLanjutMemoDinas();
                //Clients.User(clientAplikasi[i].UserName).suratMasukMemoDinas();
            }
        }
    }
}