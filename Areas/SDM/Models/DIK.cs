using Omu.ValueInjecter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Ptpn8.Areas.SDM.Models
{
    [Table("dik")]
    public class DIK
    {
        [Key]
        [Required]
        public string reg { get; set; }
        public string nama { get; set; }
        public string nm_pgl { get; set; }
        public string glr_dpn { get; set; }
        public string glr_blk { get; set; }
        public string tpt_lahir { get; set; }
        public DateTime tgl_lahir { get; set; }
        public string kelamin { get; set; }
        public string gol_darah { get; set; }
        public string agama { get; set; }
        public string alamat { get; set; }
        public string kota { get; set; }
        public string tinggal { get; set; }
        public string sipil { get; set; }
        public string stat_is { get; set; }
        public DateTime tgl_nikah { get; set; }
        public DateTime tgl_cerai { get; set; }
        public decimal kandung { get; set; }
        public decimal angkat { get; set; }
        public decimal tanggung { get; set; }
        public decimal tgg_pph { get; set; }
        public string kd_pend { get; set; }
        public DateTime tgl_sk { get; set; }
        public string no_sk { get; set; }
        public string kd_kelas { get; set; }
        public DateTime kls_tmt { get; set; }
        public string kls_sk { get; set; }
        public string gol { get; set; }
        public string mk { get; set; }
        public DateTime gol_tmt { get; set; }
        public string gol_sk { get; set; }
        public decimal gpo { get; set; }
        public string kd_kbn { get; set; }
        public string kd_afd { get; set; }
        public string kd_jab { get; set; }
        public string nama_jab { get; set; }
        public string kd_bud { get; set; }
        public DateTime jab_tmt { get; set; }
        public string jab_sk { get; set; }
        public string astek { get; set; }
        public string taspen { get; set; }
        public DateTime tgl_mpp { get; set; }
        public DateTime tgl_pen { get; set; }
        public decimal mkthn { get; set; }
        public decimal mkbln { get; set; }
        public decimal mkhr { get; set; }
        public string mpp { get; set; }
        public string st_dap { get; set; }
        public string stat_rec { get; set; }
        public string stat_dap { get; set; }
        public string no_hp { get; set; }
        public string password { get; set; }
        public string fotodiri { get; set; }

        public DIK()
        {

        }

        public class CRUDDIK
        {
            public static CRUDDIK CRUD = new CRUDDIK();
            private List<DIK> ListDIK
            {
                get
                {
                    List<DIK> result = (List<DIK>)HttpContext.Current.Session["ListDIK"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListDIK"] = result = Read();
                    }
                    return result;
                }
            }
            private List<DIK> Read()
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.DIK.ToList();
                    return model;
                }
                catch
                {
                    return new List<DIK>();
                }
            }

            private bool Create(DIK dIK)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    db.DIK.Add(dIK);
                    db.SaveChanges();

                    CreateList(dIK);
                }
                catch 
                {
                    return false;
                }

                return true;
            }


            private bool Update(DIK dIK)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.DIK.FirstOrDefault(o => o.reg == dIK.reg);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(dIK);
                    }
                    else
                    {
                        model.InjectFrom(dIK);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(dIK);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
            private bool Delete(DIK dIK)
            {
                SDMDbContext db = new SDMDbContext();
                try
                {
                    var model = db.DIK.FirstOrDefault(o => o.reg == dIK.reg);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(dIK);
                }
                catch 
                {
                    return false;
                }
                return true;
            }
            private bool CreateList(DIK dIK)
            {
                ListDIK.Add(dIK);
                HttpContext.Current.Session["ListDIK"] = ListDIK;
                return true;
            }

            private bool UpdateList(DIK dIK)
            {
                var context = ListDIK.FirstOrDefault(o => o.reg == dIK.reg);
                context.InjectFrom(dIK);
                HttpContext.Current.Session["ListDIK"] = ListDIK;
                return true;
            }

            private bool DeleteList(DIK dIK)
            {
                try
                {
                    var context = ListDIK.FirstOrDefault(o => o.reg == dIK.reg);
                    ListDIK.Remove(context);
                    HttpContext.Current.Session["ListDIK"] = ListDIK;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            private bool EraseAll()
            {
                HttpContext.Current.Session["ListDIK"] = null;
                return true;
            }

            private DIK Get1Record(string reg)
            {
                var model = ListDIK.FirstOrDefault(o => o.reg == reg);
                return model;
            }

            private List<DIK> GetnRecord(string reg)
            {
                var model = ListDIK.Where(o => o.reg == reg).ToList();
                return model;
            }

            private List<DIK> GetAllRecord()
            {
                var model = ListDIK;
                return model;
            }
            
        }

    }
}