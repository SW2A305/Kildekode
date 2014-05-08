using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Windows;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class TeamEfDal : ITeamDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params Team[] items) { return this.CreateOrUpdate(items); }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params Team[] items) { return this.CreateOrUpdate(items); }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params Team[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Team item in items)
                        {
                            Team t = (from x in db.Teams
                                      where x.TeamId == item.TeamId
                                      select x).First();
                            db.Teams.Remove(t);
                        }

                        db.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show("" + exception);
                        transaction.Rollback();
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Team> GetAll()
        {
            using (var db = new McSntttContext())
            {
                db.Teams.Load();
                return
                    db.Teams
                      .Include("TeamMembers")
                      .Include("Lectures")
                      .ToList();
            }
        }

        public Team GetOne(long itemId)
        {
            using (var db = new McSntttContext())
            {
                db.Teams.Load();
                return
                    db.Teams
                      .Include("TeamMembers")
                      .Include("Lectures")
                      .FirstOrDefault(team => team.TeamId == itemId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params Team[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Team item in items) { db.Teams.AddOrUpdate(t => t.TeamId, item); }

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
