using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Xml;
using McSntt.Models;

namespace McSntt
{
    internal class McSntttContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<SailClubMember> SailClubMembers { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<RegularTrip> RegularTrips { get; set; }
        public DbSet<Logbook> Logbooks { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}
