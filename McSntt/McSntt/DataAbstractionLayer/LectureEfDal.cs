using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using McSntt.Models;

namespace McSntt.DataAbstractionLayer
{
    public class LectureEfDal : ILectureDal
    {
        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Create(params Lecture[] items) { return this.CreateOrUpdate(items); }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Update(params Lecture[] items) { return this.CreateOrUpdate(items); }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public bool Delete(params Lecture[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Lecture item in items) { db.Lectures.Remove(item); }

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

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Lecture> GetAll()
        {
            using (var db = new McSntttContext())
            {
                db.Lectures.Load();
                return
                    db.Lectures
                      .Include("PresentMembers")
                      .ToList();
            }
        }

        public Lecture GetOne(int itemId)
        {
            using (var db = new McSntttContext())
            {
                db.Lectures.Load();
                return
                    db.Lectures
                      .Include("PresentMembers")
                      .FirstOrDefault(lecture => lecture.LectureId == itemId);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        private bool CreateOrUpdate(params Lecture[] items)
        {
            using (var db = new McSntttContext())
            {
                using (DbContextTransaction transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (Lecture item in items) { db.Lectures.AddOrUpdate(p => p.LectureId, item); }

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
