using Contoso.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contoso.Data
{
    public class ContosoContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        
    }
}
