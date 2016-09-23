using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace EDC.Models.Repository
{
    public class SubjectRepository : IRepository<Subject>
    {
        private EDCContext db;

        public SubjectRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Subject> SelectAll()
        {
            return db.Subjects;
        }
        public Subject SelectByID(params object[] id)
        {
            return db.Subjects.Find(id);
        }
        public IEnumerable<Subject> GetManyByFilter(System.Linq.Expressions.Expression<Func<Subject, bool>> filter)
        {
            return db.Subjects.Where(filter);
        }
        public Subject Create(Subject obj)
        {
            return db.Subjects.Add(obj);
        }
        public void Update(Subject obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            Subject Subject = db.Subjects.Find(id);
            if (Subject != null)
                db.Subjects.Remove(Subject);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}