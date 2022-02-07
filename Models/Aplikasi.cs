using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ptpn8.Models
{
    [Table("Aplikasi")]
    public class Aplikasi
    {
        public Guid AplikasiId { get; set; }
        public string NamaAplikasi { get; set; }
        public string Pemilik { get; set; }
        public List<Menu> Menu { get; set; }
    }

    [Table("Menu")]
    public class Menu
    {
        public Guid MenuId { get; set; }
        public Guid AplikasiId { get; set; }
        public Guid ParentId { get; set; }
        public string KodeMenu { get; set; }
        public string LinkText { get; set; }
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public string Area { get; set; }
        public string HtmlAttribute { get; set; }
        public string InRole { get; set; }
        public string ConnectionString { get; set; }
        public string classHeaderView { get; set; }
        public string classHeaderTable { get; set; }
        public string classDetailView { get; set; }
        public string classDetailTable { get; set; }
        public string NamaTabelHeader { get; set; }
        public string NamaTabelDetail { get; set; }
        public string NamaView { get; set; }
        public string FieldTahunBuku { get; set; }
        public string FieldNomorInput { get; set; }
        public string FieldNomorInputView { get; set; }
        public string FieldKeyHeader { get; set; }
        public string FieldKeyDetail { get; set; }
        public bool bolehBuatBaru { get; set; }
        public string ConnectionStringServerLain { get; set; } = "";
        public string ScriptTriggerHeaderServerLain { get; set; } = "";
        public string ScriptTriggerDetailServerLain { get; set; } = "";
        public string NamaHttpContext { get; set; } = "";
        public DateTime TanggalKadaluarsa { get; set; }
        public string ListViewName { get; set; } = "";

    }
    
    [Table("InisiasiInputProperty")]
    public class InisiasiInputProperty
    {
        public Guid InisiasiInputPropertyId { get; set; } = Guid.Empty;
        public Guid MenuId { get; set; } = Guid.Empty;
        public string AreaInput { get; set; } = "header";
        public string NamaProperty { get; set; } = "";
        public string CSharpScript { get; set; } = "";
        public string Method { get; set; } = "";
        public string Param1 { get; set; } = "";
        public string Param2 { get; set; } = "";
        public string Param3 { get; set; } = "";
        public string NamaClass { get; set; } = "";
        public int NoUrut { get; set; } = 0;
    }

    [Table("KondisiCRUD")]
    public class KondisiCRUD
    {
        public Guid KondisiCRUDId { get; set; }
        public Guid MenuId { get; set; }
        public string CRUD { get; set; }
        public string AreaInput { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
        public string ClassView { get; set; }
        public string Operator { get; set; }
    }

    //[Table("MenuTab")]
    //public class MenuTab
    //{
    //    public Guid MenuTabId { get; set; }
    //    public Guid MenuId { get; set; }
    //    public string NamaTab { get; set; }
    //    public string ActionName { get; set; }
    //    public string ControllerName { get; set; }
    //    public string Area { get; set; }
    //    public string HtmlAttribute { get; set; }
    //    public string ConnectionString { get; set; }
    //    public string classDetailView { get; set; }
    //    public string classDetailTable { get; set; }
    //    public string NamaTabelDetail { get; set; }
    //    public string FieldKeyDetail { get; set; }
    //    public string ConnectionStringServerLain { get; set; } = "";
    //    public string ScriptTriggerDetailServerLain { get; set; } = "";
    //    public string NamaHttpContext { get; set; } = "";
    //}
}