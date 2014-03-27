﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace McSntt
{
    public enum Gender
    {
        Male,
        Female
    };

    public enum Positions
    {
        Member,
        Student,
        SupportMember,
        Admin
    };

    public abstract class Person
    {
        public string FirstName     { get; set; }
        public string LastName      { get; set; }
        public string Adress        { get; set; }
        public string Postcode      { get; set; }
        public string Cityname      { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender        { get; set; }

    }

    public class SailClubMember : Person
    {
        public int MemberID         { get; set; }
        public Positions Position   { get; set; }
    }
}
