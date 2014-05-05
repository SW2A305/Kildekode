using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.Odbc;
using System.Xml;
using McSntt.Models;

namespace McSntt
{
    public class McSntttContext : DbContext
    {
        public DbSet<Person> Persons { get; set; }
        public DbSet<SailClubMember> SailClubMembers { get; set; }
        public DbSet<Boat> Boats { get; set; }
        public DbSet<RegularTrip> RegularTrips { get; set; }
        public DbSet<Logbook> Logbooks { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<StudentMember> StudentMembers { get; set; } 
    }
}
