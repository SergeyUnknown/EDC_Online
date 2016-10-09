using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class AccessToCenterRepository : IRepository<AccessToCenter>
    {
        private EDCContext db;

        public AccessToCenterRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<AccessToCenter> SelectAll()
        {
            return db.AccessToCenter;
        }

        public AccessToCenter SelectByID(params object[] id)
        {
            var accessToCenter = db.AccessToCenter.Find(id);
            return accessToCenter;
        }

        public IEnumerable<AccessToCenter> GetManyByFilter(System.Linq.Expressions.Expression<Func<AccessToCenter, bool>> filter)
        {
            return db.AccessToCenter.Where(filter);
        }

        public AccessToCenter Create(AccessToCenter obj)
        {
            return db.AccessToCenter.Add(obj);
        }

        public void Update(AccessToCenter obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            AccessToCenter aS = db.AccessToCenter.Find(id);
            if (aS != null)
                db.AccessToCenter.Remove(aS);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}