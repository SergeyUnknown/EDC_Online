using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class CRFGroupRepository:IRepository<CRF_Group>
    {
        EDCContext db;
        public CRFGroupRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<CRF_Group> SelectAll()
        {
            return db.CRFGroups;
        }

        public CRF_Group SelectByID(params object[] id)
        {
            return db.CRFGroups.Find(id);
        }

        public IEnumerable<CRF_Group> GetManyByFilter(System.Linq.Expressions.Expression<Func<CRF_Group, bool>> filter)
        {
            return db.CRFGroups.Where(filter);
        }

        public CRF_Group Create(CRF_Group obj)
        {
            return db.CRFGroups.Add(obj);
        }

        public void Update(CRF_Group obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            CRF_Group crfGroup = db.CRFGroups.Find(id);
            if (crfGroup != null)
                db.CRFGroups.Remove(crfGroup);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}