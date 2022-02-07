
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ptpn8.Models
{
    public class MigrasiDbContext : DbContext
    {
        public MigrasiDbContext() : base("name=csERP")
        {
        }

        


    }
}