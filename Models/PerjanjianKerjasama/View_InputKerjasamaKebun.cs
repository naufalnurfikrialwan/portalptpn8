using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Ptpn8.Areas.Referensi.Models.CRUD;

namespace Ptpn8.PerjanjianKerjasama.Models
{
    
    public class View_InputKerjasamaKebun_HDR
    {
        public Guid InputKerjasamaKebun_HDRId { get; set; }= Guid.NewGuid();
        public Guid KebunId { get; set; } = CRUDLokasiKerja.CRUD.LoginLokasiKerja(HttpContext.Current.User.Identity.Name).PosisiLokasiKerjaId;
        public Guid MitraId { get; set; } = Guid.Empty;
        public int NomorInput { get; set; } = 0;
        public int TahunBuku { get; set; } = 2022;
        public String NomorPermohonan { get; set; } = "";
        public DateTime TanggalPermohonan { get; set; } = DateTime.Now;
        public String NomorInputView { get; set; } = "";
        public String NamaKebun { get; set; } = "";
        public String NamaMitra { get; set; } = "";
        public String AlamatMitra { get; set; } = "";
        public String EmailMitra { get; set; } = "";
        public String NomorTelepon { get; set; } = "";
        public string UserName { get; set; }
        public List<InputKerjasamaKebun_DTL> InputKerjasamaKebun_DTL { get; set; }

    }
    public class View_InputKerjasamaKebun_DTL
    {
        public Guid InputKerjasamaKebun_HDRId { get; set; } = Guid.NewGuid();
        public Guid InputKerjasamaKebun_DTLId { get; set; } = Guid.Empty;
        public string Lampiran { get; set; } = "";
        public string Nama_File { get; set; } = "";
        public string StatusDokumen { get; set; } = "";

    }
}