using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Ptpn8.Areas.Referensi.Models;
using Omu.ValueInjecter;
using Ptpn8.Areas.Referensi.Models.CRUD;

namespace Ptpn8.Areas.NDNK.Models
{
    [Table("HDRKonsepKirimNota")]
    public class HDRKonsepKirimNota
    {
        [Required]
        public Guid HDRKonsepKirimNotaId { get; set; }
        [Required]
        public Guid KebunPengirimId { get; set; }
        [NotMapped]
        public Kebun KebunPengirim { get; set; }
        [Required]
        public Guid KebunPenerimaId { get; set; }
        [NotMapped]
        [UIHint("ddlKebun")]
        public Kebun KebunPenerima { get; set; }
        public int Nomor { get; set; }
        [NotMapped]
        public string NomorNota { get; set; }
        [Required]
        [UIHint("DateTime")]
        public DateTime Tanggal { get; set; }
        [Required]
        public string Pembuat { get; set; }
        [Required]
        public string Status { get; set; }
        [Required]
        public string Keterangan { get; set; }
        [Required]
        public string StatusDK { get; set; }
        public List<DTLKonsepKirimNota> DTLKonsepKirimNota { get; set; }
        public int TahunBuku { get; set; }

        public HDRKonsepKirimNota()
        {
            HDRKonsepKirimNotaId = Guid.NewGuid();
            KebunPengirimId = Guid.Empty;
            KebunPengirim = null;
            KebunPenerimaId = Guid.Empty;
            KebunPenerima = null;
            Nomor = 0;
            NomorNota = "";
            Tanggal = DateTime.Now;
            Pembuat = "";
            Status = "0";
            Keterangan = "";
            DTLKonsepKirimNota = null;
            TahunBuku = 0;
        }

        public class CRUDHDRKonsepKirimNota
        {
            public static CRUDHDRKonsepKirimNota CRUD = new CRUDHDRKonsepKirimNota();
            public List<HDRKonsepKirimNota> ListHDRKonsepKirimNota
            {
                get
                {
                    List<HDRKonsepKirimNota> result = (List<HDRKonsepKirimNota>)HttpContext.Current.Session["ListHDRKonsepKirimNota"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListHDRKonsepKirimNota"] = result = Read();
                    }
                    return result;
                }
            }
            public List<HDRKonsepKirimNota> Read()
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.HDRKonsepKirimNota.ToList();
                    foreach(var m in model)
                    {
                        m.NomorNota = "DN-" + m.Nomor.ToString("D5") + " - [" + m.Tanggal.ToString() + "]";
                        m.KebunPenerima = CRUDKebun.CRUD.Get1Record(m.KebunPenerimaId);
                        m.KebunPengirim= CRUDKebun.CRUD.Get1Record(m.KebunPengirimId);
                    }
                    return model;
                }
                catch 
                {
                    return new List<HDRKonsepKirimNota>();
                }
            }

            public bool Create(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    db.HDRKonsepKirimNota.Add(hDRKonsepKirimNota);
                    db.SaveChanges();

                    CreateList(hDRKonsepKirimNota);
                }
                catch
                {
                    return false;
                }

                return true;
            }


            public bool Update(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.HDRKonsepKirimNota.FirstOrDefault(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNota.HDRKonsepKirimNotaId);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(hDRKonsepKirimNota);
                    }
                    else
                    {
                        model.InjectFrom(hDRKonsepKirimNota);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(hDRKonsepKirimNota);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
            public bool Delete(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.HDRKonsepKirimNota.FirstOrDefault(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNota.HDRKonsepKirimNotaId);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(hDRKonsepKirimNota);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            public bool CreateList(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                ListHDRKonsepKirimNota.Add(hDRKonsepKirimNota);
                HttpContext.Current.Session["ListHDRKonsepKirimNota"] = ListHDRKonsepKirimNota;
                return true;
            }

            public bool UpdateList(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                var context = ListHDRKonsepKirimNota.FirstOrDefault(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNota.HDRKonsepKirimNotaId);
                context.InjectFrom(hDRKonsepKirimNota);
                HttpContext.Current.Session["ListHDRKonsepKirimNota"] = ListHDRKonsepKirimNota;
                return true;
            }

            public bool DeleteList(HDRKonsepKirimNota hDRKonsepKirimNota)
            {
                try
                {
                    var context = ListHDRKonsepKirimNota.FirstOrDefault(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNota.HDRKonsepKirimNotaId);
                    ListHDRKonsepKirimNota.Remove(context);
                    HttpContext.Current.Session["ListHDRKonsepKirimNota"] = ListHDRKonsepKirimNota;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            public bool EraseAll()
            {
                HttpContext.Current.Session["ListHDRKonsepKirimNota"] = null;
                return true;
            }

            public HDRKonsepKirimNota Get1Record(Guid hDRKonsepKirimNotaId)
            {
                var model = ListHDRKonsepKirimNota.FirstOrDefault(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNotaId);
                return model;
            }

            public List<HDRKonsepKirimNota> GetnRecord(Guid hDRKonsepKirimNotaId)
            {
                var model = ListHDRKonsepKirimNota.Where(o => o.HDRKonsepKirimNotaId == hDRKonsepKirimNotaId).ToList();
                return model;
            }

            public List<HDRKonsepKirimNota> GetAllRecord()
            {
                var model = ListHDRKonsepKirimNota;
                return model;
            }
        }
    }

    [Table("DTLKonsepKirimNota")]
    public class DTLKonsepKirimNota
    {
        [Required]
        public Guid DTLKonsepKirimNotaId { get; set; }
        [Required]
        public Guid HDRKonsepKirimNotaId { get; set; }
        [Required]
        public Guid RekeningId { get; set; }
        [NotMapped]
        [UIHint("ddlRekening")]
        public Rekening Rekening { get; set; }
        [Required]
        [UIHint("TextEditor")]
        public string Uraian { get; set; }
        [Required]
        [UIHint("Number")]
        [Range(0, int.MaxValue)]
        public decimal JumlahFisik { get; set; }
        [Required]
        [UIHint("Number")]
        [Range(0, int.MaxValue)]
        public decimal JumlahNilai { get; set; }

        public DTLKonsepKirimNota()
        {
            DTLKonsepKirimNotaId = Guid.NewGuid();
            HDRKonsepKirimNotaId = Guid.Empty;
            RekeningId = Guid.Empty;
            Rekening = null;
            Uraian = "";
            JumlahFisik = 0;
            JumlahNilai = 0;
        }
        public class CRUDDTLKonsepKirimNota
        {
            public static CRUDDTLKonsepKirimNota CRUD = new CRUDDTLKonsepKirimNota();
            public List<DTLKonsepKirimNota> ListDTLKonsepKirimNota
            {
                get
                {
                    List<DTLKonsepKirimNota> result = (List<DTLKonsepKirimNota>)HttpContext.Current.Session["ListDTLKonsepKirimNota"];
                    if (result == null)
                    {
                        HttpContext.Current.Session["ListDTLKonsepKirimNota"] = result = Read();
                    }
                    return result;
                }
            }
            public List<DTLKonsepKirimNota> Read()
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.DTLKonsepKirimNota.ToList();
                    foreach(var m in model)
                    {
                        m.Rekening = CRUDRekening.CRUD.Get1Record(m.RekeningId);
                    }
                    return model;
                }
                catch
                {
                    return new List<DTLKonsepKirimNota>();
                }
            }

            public bool Create(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    db.DTLKonsepKirimNota.Add(dTLKonsepKirimNota);
                    db.SaveChanges();

                    CreateList(dTLKonsepKirimNota);
                }
                catch 
                {
                    return false;
                }

                return true;
            }


            public bool Update(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.DTLKonsepKirimNota.FirstOrDefault(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNota.DTLKonsepKirimNotaId);
                    if (model == null)
                    {
                        // harus create record baru
                        return Create(dTLKonsepKirimNota);
                    }
                    else
                    {
                        model.InjectFrom(dTLKonsepKirimNota);
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        db.SaveChanges();
                        UpdateList(dTLKonsepKirimNota);
                    }
                }
                catch 
                {
                    return false;
                }

                return true;
            }
            public bool Delete(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                NDNKDbContext db = new NDNKDbContext();
                try
                {
                    var model = db.DTLKonsepKirimNota.FirstOrDefault(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNota.DTLKonsepKirimNotaId);
                    db.Entry(model).State = System.Data.Entity.EntityState.Deleted;
                    db.SaveChanges();
                    DeleteList(dTLKonsepKirimNota);
                }
                catch
                {
                    return false;
                }
                return true;
            }

            public bool CreateList(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                ListDTLKonsepKirimNota.Add(dTLKonsepKirimNota);
                HttpContext.Current.Session["ListDTLKonsepKirimNota"] = ListDTLKonsepKirimNota;
                return true;
            }

            public bool UpdateList(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                var context = ListDTLKonsepKirimNota.FirstOrDefault(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNota.DTLKonsepKirimNotaId);
                context.InjectFrom(dTLKonsepKirimNota);
                HttpContext.Current.Session["ListDTLKonsepKirimNota"] = ListDTLKonsepKirimNota;
                return true;
            }

            public bool DeleteList(DTLKonsepKirimNota dTLKonsepKirimNota)
            {
                try
                {
                    var context = ListDTLKonsepKirimNota.FirstOrDefault(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNota.DTLKonsepKirimNotaId);
                    ListDTLKonsepKirimNota.Remove(context);
                    HttpContext.Current.Session["ListDTLKonsepKirimNota"] = ListDTLKonsepKirimNota;
                    return true;
                }
                catch 
                {
                    return false;
                }
            }

            public bool EraseAll()
            {
                HttpContext.Current.Session["ListDTLKonsepKirimNota"] = null;
                return true;
            }

            public DTLKonsepKirimNota Get1Record(Guid dTLKonsepKirimNotaId)
            {
                var model = ListDTLKonsepKirimNota.FirstOrDefault(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNotaId);
                return model;
            }

            public List<DTLKonsepKirimNota> GetnRecord(Guid dTLKonsepKirimNotaId)
            {
                var model = ListDTLKonsepKirimNota.Where(o => o.DTLKonsepKirimNotaId == dTLKonsepKirimNotaId).ToList();
                return model;
            }

            public List<DTLKonsepKirimNota> GetAllRecord()
            {
                var model = ListDTLKonsepKirimNota;
                return model;
            }
        }
    }
}