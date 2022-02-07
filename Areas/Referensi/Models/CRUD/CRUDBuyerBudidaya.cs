using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.Referensi.Models.CRUD
{
    public class CRUDBuyerBudidaya
    {
        public static CRUDBuyerBudidaya CRUD = new CRUDBuyerBudidaya();

        public List<BuyerBudidaya> ListBuyerBudidaya
        {
            get
            {
                List<BuyerBudidaya> result = (List<BuyerBudidaya>)HttpContext.Current.Session["ListBuyerBudidaya"];
                if (result == null)
                {
                    HttpContext.Current.Session["ListBuyerBudidaya"] = result = Read();
                }
                return result;
            }
        }
        public bool Create(BuyerBudidaya buyerBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                db.BuyerBudidaya.Add(buyerBudidaya);
                db.SaveChanges();

                ListBuyerBudidaya.Add(buyerBudidaya);
                HttpContext.Current.Session["ListBuyerBudidaya"] = ListBuyerBudidaya;
            }
            catch
            {
                return false;
            }

            return true;
        }
        public List<BuyerBudidaya> Read()
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BuyerBudidaya.ToList();
                foreach(var m in model)
                {
                    m.Budidaya = new MainBudidaya();
                    m.Budidaya = CRUDBudidaya.CRUD.Get1Record(m.MainBudidayaId);

                    m.Rekening = new Rekening();
                    m.Rekening= CRUDRekening.CRUD.Get1Record(m.RekeningId);

                    m.Buyer = new Buyer();
                    m.Buyer = CRUDBuyer.CRUD.Get1Record(m.BuyerId);
                }
                return model;
            }
            catch
            {
                return new List<BuyerBudidaya>();
            }
        }
        public bool Update(BuyerBudidaya buyerBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BuyerBudidaya.FirstOrDefault(o => o.BuyerBudidayaId == buyerBudidaya.BuyerBudidayaId);
                if (model == null)
                {
                    // harus create record baru
                    return Create(buyerBudidaya);
                }
                else
                {
                    model.InjectFrom(buyerBudidaya);
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    var context = ListBuyerBudidaya.FirstOrDefault(o => o.BuyerBudidayaId == buyerBudidaya.BuyerBudidayaId);
                    context.InjectFrom(buyerBudidaya);
                    HttpContext.Current.Session["ListBuyerBudidaya"] = ListBuyerBudidaya;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }
        public bool Delete(BuyerBudidaya buyerBudidaya)
        {
            ReferensiDbContext db = new ReferensiDbContext();
            try
            {
                var model = db.BuyerBudidaya.FirstOrDefault(o => o.BuyerBudidayaId == buyerBudidaya.BuyerBudidayaId);
                db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();
                Erase(buyerBudidaya);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool Erase(BuyerBudidaya buyerBudidaya)
        {
            try
            {
                var context = ListBuyerBudidaya.FirstOrDefault(o => o.BuyerBudidayaId == buyerBudidaya.BuyerBudidayaId);
                ListBuyerBudidaya.Remove(context);
                HttpContext.Current.Session["ListBuyerBudidaya"] = ListBuyerBudidaya;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Erase(Guid mainBudidayaId)
        {
            try
            {
                var context = ListBuyerBudidaya.Where(o => o.MainBudidayaId == mainBudidayaId).ToList();
                foreach (var m in context)
                {
                    ListBuyerBudidaya.Remove(m);
                }
                HttpContext.Current.Session["ListBuyerBudidaya"] = ListBuyerBudidaya;
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool ErasebyBuyer(Guid buyerId)
        {
            try
            {
                var context = ListBuyerBudidaya.Where(o => o.BuyerId == buyerId).ToList();
                foreach (var m in context)
                {
                    ListBuyerBudidaya.Remove(m);
                }
                HttpContext.Current.Session["ListBuyerBudidaya"] = ListBuyerBudidaya;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EraseAll()
        {
            HttpContext.Current.Session["ListBuyerBudidaya"] = null;
            return true;
        }

        public BuyerBudidaya Get1Record(Guid buyerBudidayaId)
        {
            var model = ListBuyerBudidaya.FirstOrDefault(o => o.BuyerBudidayaId == buyerBudidayaId);
            return model;
        }

        public List<BuyerBudidaya> GetnRecord(Guid buyerId)
        {
            var model = ListBuyerBudidaya.Where(o => o.BuyerId == buyerId).ToList();
            return model;
        }

        public List<BuyerBudidaya> GetAllRecord()
        {
            var model = ListBuyerBudidaya;
            return model;
        }
    }
}