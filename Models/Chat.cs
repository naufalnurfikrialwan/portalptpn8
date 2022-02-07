using System;

namespace Ptpn8.Models
{
    public class Chat
    {
        public Guid ChatId { get; set; }
        public string ChatRoom { get; set; }
        public DateTime Tanggal { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Nama { get; set; }
        public string TextMessage { get; set; }
    }
}