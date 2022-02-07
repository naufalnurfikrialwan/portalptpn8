using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Models
{
    [Table("RiwayatAkses")]
    public class RiwayatAkses
    {
        [Key]
        public Guid RiwayatAksesId { get; set; }
        public string UserName { get; set; }
        public DateTime TanggalAkses { get; set; }
        public string PageYangDiakses { get; set; }
        public Guid Id { get; set; } = Guid.Empty;

        public RiwayatAkses()
        {
            RiwayatAksesId = Guid.NewGuid();
            UserName = "";
            TanggalAkses = DateTime.Now;
            PageYangDiakses = "";
            //Id = Guid.NewGuid();
        }

        public class CRUDRiwayatAkses
        {
            public static CRUDRiwayatAkses CRUD = new CRUDRiwayatAkses();

            public List<RiwayatAkses> ListRiwayatAkses
            {
                get
                {
                    List<RiwayatAkses> result = (List<RiwayatAkses>)HttpContext.Current.Session["ListRiwayatAkses"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListRiwayatAkses"] = result = Read();
                    }
                    return result;
                }
            }
            public bool Create(RiwayatAkses riwayatAkses)
            {
                Ptpn8dbContext db = new Ptpn8dbContext();
                try
                {
                    //riwayatAkses.RiwayatAksesId = Guid.NewGuid();
                    db.RiwayatAkses.Add(riwayatAkses);
                    db.SaveChanges();

                    //ListRiwayatAkses.Add(riwayatAkses);
                    //HttpContext.Current.Session["ListRiwayatAkses"] = ListRiwayatAkses;
                }
                catch
                {
                    return false;
                }

                return true;
            }
            public List<RiwayatAkses> Read()
            {
                Ptpn8dbContext db = new Ptpn8dbContext();
                try
                {
                    var model = db.RiwayatAkses.ToList();
                    return model;
                }
                catch
                {
                    return new List<RiwayatAkses>();
                }
            }
            public bool Update(RiwayatAkses riwayatAkses)
            {
                return Create(riwayatAkses);

                //Ptpn8dbContext db = new Ptpn8dbContext();
                //try
                //{
                //    var model = db.RiwayatAkses.FirstOrDefault(o => o.PageYangDiakses == riwayatAkses.PageYangDiakses && o.TanggalAkses.Day == riwayatAkses.TanggalAkses.Day
                //        && o.TanggalAkses.Month == riwayatAkses.TanggalAkses.Month && o.TanggalAkses.Year == riwayatAkses.TanggalAkses.Year);
                //    if (model == null)
                //    {
                //        // harus create record baru
                //        return Create(riwayatAkses);
                //    }
                //    else
                //    {
                //        riwayatAkses.RiwayatAksesId = model.RiwayatAksesId;
                //        model.InjectFrom(riwayatAkses);
                //        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                //        db.SaveChanges();

                //        //var context = ListRiwayatAkses.FirstOrDefault(o => o.RiwayatAksesId == riwayatAkses.RiwayatAksesId);
                //        //context.InjectFrom(riwayatAkses);
                //        //HttpContext.Current.Session["ListRiwayatAkses"] = ListRiwayatAkses;
                //    }
                //}
                //catch 
                //{
                //    return false;
                //}

                //return true;
            }

            public bool Delete(RiwayatAkses riwayatAkses)
            {
                Ptpn8dbContext db = new Ptpn8dbContext();
                try
                {
                    var model = db.RiwayatAkses.FirstOrDefault(o => o.RiwayatAksesId == riwayatAkses.RiwayatAksesId);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    Erase(riwayatAkses);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            public bool Erase(RiwayatAkses riwayatAkses)
            {
                try
                {
                    var context = ListRiwayatAkses.FirstOrDefault(o => o.RiwayatAksesId == riwayatAkses.RiwayatAksesId);
                    ListRiwayatAkses.Remove(context);
                    HttpContext.Current.Session["ListRiwayatAkses"] = ListRiwayatAkses;
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            public bool EraseAll()
            {
                HttpContext.Current.Session["ListRiwayatAkses"] = null;
                return true;
            }

            public RiwayatAkses Get1Record(Guid riwayatAksesId)
            {
                var model = ListRiwayatAkses.FirstOrDefault(o => o.RiwayatAksesId == riwayatAksesId);
                return model;
            }

            public List<RiwayatAkses> GetnRecord(Guid riwayatAksesId)
            {
                var model = ListRiwayatAkses.Where(o => o.RiwayatAksesId == riwayatAksesId).ToList();
                return model;
            }

            public List<RiwayatAkses> GetAllRecord()
            {
                var model = ListRiwayatAkses;
                return model;
            }

        }
    }
}