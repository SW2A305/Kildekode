using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using McSntt.Models;

namespace McSntt
{
    class McSntttContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<SailClubMember> SailClubMembers { get; set; }
    }
}
