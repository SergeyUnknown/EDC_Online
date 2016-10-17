using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class AuditsEditReasonsRepository :IRepository<AuditEditReason>
    {
        EDCContext db;

        public AuditsEditReasonsRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<AuditEditReason> SelectAll()
        {
            return db.AuditsEditReason;
        }

        public AuditEditReason SelectByID(params object[] id)
        {
            return db.AuditsEditReason.Find(id);
        }

        public IEnumerable<AuditEditReason> GetManyByFilter(System.Linq.Expressions.Expression<Func<AuditEditReason, bool>> filter)
        {
            return db.AuditsEditReason.Where(filter);
        }

        public AuditEditReason Create(AuditEditReason obj)
        {
            return db.AuditsEditReason.Add(obj);
        }

        public void Update(AuditEditReason obj)
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