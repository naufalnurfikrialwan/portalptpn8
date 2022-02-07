using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.SDM.Models
{

    public partial class SDM08
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(14)]
        public string npp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(2)]
        public string no_urut { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(1)]
        public string kd_mutasi { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(2)]
        public string kd_unit { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(2)]
        public string kd_afd { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(4)]
        public string kd_bud { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(9)]
        public string jabatan { get; set; }

        [Key]
        [Column(Order = 7, TypeName = "date")]
        public DateTime tmt { get; set; }

        [Key]
        [Column(Order = 8)]
        [StringLength(30)]
        public string no_sk { get; set; }

        [Key]
        [Column(Order = 9, TypeName = "date")]
        public DateTime tgl_sk { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(1)]
        public string jns_mut { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(14)]
        public string npp_jbt { get; set; }

        [Key]
        [Column(Order = 12)]
        [StringLength(1)]
        public string tinggal { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(6)]
        public string bln_proses { get; set; }

        [Key]
        [Column(Order = 14)]
        [StringLength(1)]
        public string stat_rec { get; set; }

        public SDM08()
        {

        }

        public class CRUDSDM08
        {
            public static CRUDSDM08 CRUD = new CRUDSDM08();
            private List<SDM08> ListSDM08
            {
                get
                {
                    List<SDM08> result = (List<SDM08>)HttpContext.Current.Session["ListSDM08"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListSDM08"] = result = Read();
                    }
                    return result;
                }
            }
            private List<SDM08> Read()
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM08.ToList();
                    return model;
                }
                catch
                {
                    return new List<SDM08>();
                }
            }

            private bool Create(SDM08 sDM08)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    db.SDM08.Add(sDM08);
                    db.SaveChanges();

                    CreateList(sDM08);
                }
                catch 
                {
                    return false;
                }

                return true;
            }


            private bool Update(SDM08 sDM08)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM08.FirstOrDefault(o => o.npp == sDM08.npp);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(sDM08);
                    }
                    else
                    {
                        model.InjectFrom(sDM08);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(sDM08);
                    }
                }
                catch 
                {
                    return false;
                }

                return true;
            }
            private bool Delete(SDM08 sDM08)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM08.FirstOrDefault(o => o.npp == sDM08.npp);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(sDM08);
                }
                catch 
                {
                    return false;
                }
                return true;
            }
            private bool CreateList(SDM08 sDM08)
            {
                ListSDM08.Add(sDM08);
                HttpContext.Current.Session["ListSDM08"] = ListSDM08;
                return true;
            }

            private bool UpdateList(SDM08 sDM08)
            {
                var context = ListSDM08.FirstOrDefault(o => o.npp == sDM08.npp);
                context.InjectFrom(sDM08);
                HttpContext.Current.Session["ListSDM08"] = ListSDM08;
                return true;
            }

            private bool DeleteList(SDM08 sDM08)
            {
                try
                {
                    var context = ListSDM08.FirstOrDefault(o => o.npp == sDM08.npp);
                    ListSDM08.Remove(context);
                    HttpContext.Current.Session["ListSDM08"] = ListSDM08;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            private bool EraseAll()
            {
                HttpContext.Current.Session["ListSDM08"] = null;
                return true;
            }

            private SDM08 Get1Record(string npp)
            {
                var model = ListSDM08.FirstOrDefault(o => o.npp == npp);
                return model;
            }

            private List<SDM08> GetnRecord(string npp)
            {
                var model = ListSDM08.Where(o => o.npp == npp).ToList();
                return model;
            }

            private List<SDM08> GetAllRecord()
            {
                var model = ListSDM08;
                return model;
            }
        }

    }
}
