using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models.CRUD
{
    public class CRUDChat
    {
        public static CRUDChat CRUD = new CRUDChat();

        public List<Chat> ListChat
        {
            get
            {
                List<Chat> result = (List<Chat>)HttpContext.Current.Application["Chat"];
                if (result == null)
                {
                    HttpContext.Current.Application["Chat"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(Chat chat)
        {
            ListChat.Add(chat);
            HttpContext.Current.Application["Chat"] = ListChat;
            return true;
        }
        public List<Chat> Read()
        {
            return new List<Chat>();
        }
        public bool Update(Chat chat)
        {
            var model = ListChat.FirstOrDefault(o => o.ChatId == chat.ChatId);
            if (model == null)
            {
                // harus create record baru
                return Create(chat);
            }
            else
            {
                var context = ListChat.FirstOrDefault(o => o.ChatId == chat.ChatId);
                context.InjectFrom(chat);
                HttpContext.Current.Application["Chat"] = ListChat;
            }
            return true;
        }
        public bool Delete(List<Chat> listChat)
        {
            foreach (var model in listChat)
            {
                Erase(model);
            }
            return true;
        }

        public bool Erase(Chat chat)
        {
            var context = ListChat.FirstOrDefault(o => o.ChatId == chat.ChatId);
            ListChat.Remove(context);
            HttpContext.Current.Application["Chat"] = ListChat;
            return true;
        }

        public bool EraseAll()
        {
            HttpContext.Current.Application["Chat"] = null;
            return true;
        }

        public Chat Get1Record(Guid chatId)
        {
            var model = ListChat.FirstOrDefault(o => o.ChatId == chatId);
            return model;
        }

        public Chat Get1Record(string userName)
        {
            var model = ListChat.FirstOrDefault(o => o.UserName == userName);
            return model;
        }

        public List<Chat> GetnRecord(Guid chatId)
        {
            var model = ListChat.Where(o => o.ChatId == chatId).ToList();
            return model;
        }

        public List<Chat> GetAllRecord()
        {
            var model = ListChat;
            return model;
        }
    }
}