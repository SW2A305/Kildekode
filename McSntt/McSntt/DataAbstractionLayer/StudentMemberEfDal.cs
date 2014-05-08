﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    internal class StudentMemberEfDal : IStudentMemberDal
    {
        public bool Create(params StudentMember[] items) { return this.CreateOrUpdate(items); }

        public bool Update(params StudentMember[] items) { return this.CreateOrUpdate(items); }

        public bool Delete(params StudentMember[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (StudentMember item in items) { db.StudentMembers.Remove(item); }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        public IEnumerable<StudentMember> GetAll()
        {
            using (var db = new McSntttContext())
            {
                db.StudentMembers.Load();
                return
                    db.StudentMembers
                      .Include("AssociatedTeam")
                      .ToList();
            }
        }

        public StudentMember GetOne(long itemId)
        {
            using (var db = new McSntttContext())
            {
                db.StudentMembers.Load();
                return
                    db.StudentMembers
                      .Include("AssociatedTeam")
                      .FirstOrDefault(sm => sm.SailClubMemberId == itemId);
            }
        }

        private bool CreateOrUpdate(params StudentMember[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (StudentMember item in items) {
                            db.StudentMembers.AddOrUpdate(p => p.SailClubMemberId, item);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
