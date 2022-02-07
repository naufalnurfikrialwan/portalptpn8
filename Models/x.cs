using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ptpn8.Models
{
    public class x
    {
        public static System.Data.DataTable ExecSQL(string scriptSQL) { string strcs = System.Configuration.ConfigurationManager.ConnectionStrings["csSPDKKanpus"].ConnectionString; System.Data.SqlClient.SqlConnection con = new System.Data.SqlClient.SqlConnection(strcs); System.Data.DataTable tap = new System.Data.DataTable(); try { con.Open(); new System.Data.SqlClient.SqlDataAdapter(scriptSQL, con).Fill(tap); } catch (Exception) { } finally { if (con != null) con.Close(); } return tap; }

        public static float GetJumlah(System.Guid migrasi_AlokasiId, System.Decimal hargaSatuanDitawarkan) { float result = 0; string strSQL = "select top 1 B.Quantity*" + hargaSatuanDitawarkan.ToString() + " as Jumlah FROM ERP..Migrasi_Alokasi A JOIN ERP..Migrasi_ArusProduksi B ON A.Migrasi_ArusProduksiId=B.Migrasi_ArusProduksiId where A.Migrasi_AlokasiId='" + migrasi_AlokasiId.ToString() + "'"; System.Data.DataTable tbl = new System.Data.DataTable(); tbl = ExecSQL(strSQL); if (tbl.Rows.Count > 0) result = float.Parse(tbl.Rows[0][0].ToString()); else result = 0; return result; }

    }
}