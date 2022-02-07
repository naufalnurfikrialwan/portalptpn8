
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ptpn8.Models
{
    public class KerjasamaDbContext : DbContext
    {
        public KerjasamaDbContext() : base("name=csKerjasama")
        {

        }

        
    }
}