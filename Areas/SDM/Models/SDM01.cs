using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.SDM.Models
{
    public partial class SDM01
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(14)]
        public string npp { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(40)]
        public string nama { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(15)]
        public string glr_dpn { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(15)]
        public string glr_blk { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(10)]
        public string nm_pgl { get; set; }

        [Key]
        [Column(Order = 5)]
        [StringLength(20)]
        public string kota_lhr { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(20)]
        public string prop_lhr { get; set; }

        [Key]
        [Column(Order = 7)]
        [StringLength(20)]
        public string ngr_lhr { get; set; }

        [Key]
        [Column(Order = 8, TypeName = "date")]
        public DateTime tgl_lhr { get; set; }

        [Key]
        [Column(Order = 9)]
        [StringLength(1)]
        public string kelamin { get; set; }

        [Key]
        [Column(Order = 10)]
        [StringLength(2)]
        public string gol_darah { get; set; }

        [Key]
        [Column(Order = 11)]
        [StringLength(1)]
        public string agama { get; set; }

        [Key]
        [Column(Order = 12, TypeName = "date")]
        public DateTime tgl_masuk { get; set; }

        [Key]
        [Column(Order = 13)]
        [StringLength(1)]
        public string st_sipil { get; set; }

        [Key]
        [Column(Order = 14)]
        public decimal jml_anak { get; set; }

        [Key]
        [Column(Order = 15)]
        [StringLength(21)]
        public string no_astek { get; set; }

        [Key]
        [Column(Order = 16)]
        [StringLength(21)]
        public string no_pens { get; set; }

        [Key]
        [Column(Order = 17)]
        [StringLength(21)]
        public string npwp { get; set; }

        [Key]
        [Column(Order = 18)]
        [StringLength(50)]
        public string alm_tgl { get; set; }

        [Key]
        [Column(Order = 19)]
        [StringLength(8)]
        public string kd_lokasi { get; set; }

        [Key]
        [Column(Order = 20)]
        [StringLength(5)]
        public string kode_pos { get; set; }

        [Key]
        [Column(Order = 21)]
        [StringLength(11)]
        public string no_telp { get; set; }

        [Key]
        [Column(Order = 22)]
        [StringLength(11)]
        public string no_faks { get; set; }

        [Key]
        [Column(Order = 23)]
        [StringLength(20)]
        public string no_ktp { get; set; }

        [Key]
        [Column(Order = 24)]
        [StringLength(10)]
        public string user_id { get; set; }

        [Key]
        [Column(Order = 25)]
        [StringLength(6)]
        public string bln_proses { get; set; }

        [Key]
        [Column(Order = 26)]
        [StringLength(1)]
        public string tinggal { get; set; }

        [Key]
        [Column(Order = 27)]
        [StringLength(50)]
        public string ket { get; set; }

        [Key]
        [Column(Order = 28)]
        public DateTime tglpen { get; set; }

        [Key]
        [Column(Order = 29)]
        [StringLength(30)]
        public string noskpen { get; set; }

        [Key]
        [Column(Order = 30)]
        public DateTime tglskpen { get; set; }

        [Key]
        [Column(Order = 31)]
        [StringLength(1)]
        public string jns_pen { get; set; }

        [Key]
        [Column(Order = 32)]
        [StringLength(1)]
        public string stat_rec { get; set; }

        [Key]
        [Column(Order = 33)]
        [StringLength(1)]
        public string st_dap { get; set; }

        [StringLength(50)]
        public string no_hp { get; set; }

        [StringLength(50)]
        public string password { get; set; }

        [StringLength(200)]
        public string fotodiri { get; set; }

        public SDM01()
        {
        }


        public class CRUDSDM01
        {
            public static CRUDSDM01 CRUD = new CRUDSDM01();
            private List<SDM01> ListSDM01
            {
                get
                {
                    List<SDM01> result = (List<SDM01>)HttpContext.Current.Session["ListSDM01"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListSDM01"] = result = Read();
                    }
                    return result;
                }
            }
            private List<SDM01> Read()
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM01.ToList();
                    return model;
                }
                catch 
                {
                    return new List<SDM01>();
                }
            }

            private bool Create(SDM01 sDM01)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    db.SDM01.Add(sDM01);
                    db.SaveChanges();

                    CreateList(sDM01);
                }
                catch 
                {
                    return false;
                }

                return true;
            }


            private bool Update(SDM01 sDM01)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM01.FirstOrDefault(o => o.npp == sDM01.npp);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(sDM01);
                    }
                    else
                    {
                        model.InjectFrom(sDM01);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(sDM01);
                    }
                }
                catch 
                {
                    return false;
                }

                return true;
            }
            private bool Delete(SDM01 sDM01)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.SDM01.FirstOrDefault(o => o.npp == sDM01.npp);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(sDM01);
                }
                catch 
                {
                    return false;
                }
                return true;
            }
            private bool CreateList(SDM01 sDM01)
            {
                ListSDM01.Add(sDM01);
                HttpContext.Current.Session["ListSDM01"] = ListSDM01;
                return true;
            }

            private bool UpdateList(SDM01 sDM01)
            {
                var context = ListSDM01.FirstOrDefault(o => o.npp == sDM01.npp);
                context.InjectFrom(sDM01);
                HttpContext.Current.Session["ListSDM01"] = ListSDM01;
                return true;
            }

            private bool DeleteList(SDM01 sDM01)
            {
                try
                {
                    var context = ListSDM01.FirstOrDefault(o => o.npp == sDM01.npp);
                    ListSDM01.Remove(context);
                    HttpContext.Current.Session["ListSDM01"] = ListSDM01;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            private bool EraseAll()
            {
                HttpContext.Current.Session["ListSDM01"] = null;
                return true;
            }

            private SDM01 Get1Record(string npp)
            {
                var model = ListSDM01.FirstOrDefault(o => o.npp == npp);
                return model;
            }

            private List<SDM01> GetnRecord(string npp)
            {
                var model = ListSDM01.Where(o => o.npp == npp).ToList();
                return model;
            }

            private List<SDM01> GetAllRecord()
            {
                var model = ListSDM01;
                return model;
            }
        }
    }
}
