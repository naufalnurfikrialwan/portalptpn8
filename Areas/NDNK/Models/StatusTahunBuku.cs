using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.NDNK.Models
{
    [Table("StatusTahunBuku")]
    public class StatusTahunBuku
    {
        [Required]
        public Guid StatusTahunBukuId { get; set; }
        [Required]
        public Guid KebunId { get; set; }
        [Required]
        [UIHint("Number")]
        [Range(2016, 2100)]
        public int TahunBuku { get; set; }
        public StatusTahunBuku()
        {
            StatusTahunBukuId = Guid.NewGuid();
            KebunId = Guid.Empty;
            TahunBuku = DateTime.Now.Year;
        }
        public class CRUDStatusTahunBuku
        {
            public static CRUDStatusTahunBuku CRUD = new CRUDStatusTahunBuku();
            private List<StatusTahunBuku> ListStatusTahunBuku
            {
                get
                {
                    List<StatusTahunBuku> result = (List<StatusTahunBuku>)HttpContext.Current.Session["ListStatusTahunBuku"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListStatusTahunBuku"] = result = Read();
                    }
                    return result;
                }
            }
            private List<StatusTahunBuku> Read()
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.StatusTahunBuku.ToList();
                    return model;
                }
                catch
                {
                    return new List<StatusTahunBuku>();
                }
            }

            private bool Create(StatusTahunBuku statusTahunBuku)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    db.StatusTahunBuku.Add(statusTahunBuku);
                    db.SaveChanges();

                    CreateList(statusTahunBuku);
                }
                catch
                {
                    return false;
                }

                return true;
            }


            private bool Update(StatusTahunBuku statusTahunBuku)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.StatusTahunBuku.FirstOrDefault(o => o.StatusTahunBukuId == statusTahunBuku.StatusTahunBukuId);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(statusTahunBuku);
                    }
                    else
                    {
                        model.InjectFrom(statusTahunBuku);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(statusTahunBuku);
                    }
                }
                catch 
                {
                    return false;
                }

                return true;
            }
            private bool Delete(StatusTahunBuku statusTahunBuku)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.StatusTahunBuku.FirstOrDefault(o => o.StatusTahunBukuId == statusTahunBuku.StatusTahunBukuId);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(statusTahunBuku);
                }
                catch
                {
                    return false;
                }
                return true;
            }
            private bool CreateList(StatusTahunBuku statusTahunBuku)
            {
                ListStatusTahunBuku.Add(statusTahunBuku);
                HttpContext.Current.Session["ListStatusTahunBuku"] = ListStatusTahunBuku;
                return true;
            }

            private bool UpdateList(StatusTahunBuku statusTahunBuku)
            {
                var context = ListStatusTahunBuku.FirstOrDefault(o => o.StatusTahunBukuId == statusTahunBuku.StatusTahunBukuId);
                context.InjectFrom(statusTahunBuku);
                HttpContext.Current.Session["ListStatusTahunBuku"] = ListStatusTahunBuku;
                return true;
            }

            private bool DeleteList(StatusTahunBuku statusTahunBuku)
            {
                try
                {
                    var context = ListStatusTahunBuku.FirstOrDefault(o => o.StatusTahunBukuId == statusTahunBuku.StatusTahunBukuId);
                    ListStatusTahunBuku.Remove(context);
                    HttpContext.Current.Session["ListStatusTahunBuku"] = ListStatusTahunBuku;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            private bool EraseAll()
            {
                HttpContext.Current.Session["ListStatusTahunBuku"] = null;
                return true;
            }

            private StatusTahunBuku Get1Record(Guid statusTahunBukuId)
            {
                var model = ListStatusTahunBuku.FirstOrDefault(o => o.StatusTahunBukuId == statusTahunBukuId);
                return model;
            }

            private List<StatusTahunBuku> GetnRecord(Guid statusTahunBukuId)
            {
                var model = ListStatusTahunBuku.Where(o => o.StatusTahunBukuId == statusTahunBukuId).ToList();
                return model;
            }

            private List<StatusTahunBuku> GetAllRecord()
            {
                var model = ListStatusTahunBuku;
                return model;
            }

            public int TahunBuku(Guid lokasiKerjaId)
            {
                var model = ListStatusTahunBuku.FirstOrDefault(o => o.KebunId == lokasiKerjaId);
                if (model != null) return model.TahunBuku; else return 0;            }
        }
    }
}