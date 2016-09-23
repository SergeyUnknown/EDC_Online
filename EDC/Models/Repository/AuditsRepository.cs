using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class AuditsRepository :IRepository<Audit>
    {
        EDCContext db;

        public AuditsRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<Audit> SelectAll()
        {
            return db.Audits;
        }

        public Audit SelectByID(params object[] id)
        {
            return db.Audits.Find(id);
        }

        public IEnumerable<Audit> GetManyByFilter(System.Linq.Expressions.Expression<Func<Audit, bool>> filter)
        {
            return db.Audits.Where(filter);
        }

        public Audit Create(Audit obj)
        {
            return db.Audits.Add(obj);
        }

        public void Update(Audit obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}