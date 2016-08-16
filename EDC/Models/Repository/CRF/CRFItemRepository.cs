using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class CRFItemRepository:IRepository<CRF_Item>
    {
        EDCContext db;

        public CRFItemRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<CRF_Item> SelectAll()
        {
            return db.CRFItems;
        }

        public CRF_Item SelectByID(params object[] id)
        {
            return db.CRFItems.Find(id);
        }

        public IEnumerable<CRF_Item> GetManyByFilter(System.Linq.Expressions.Expression<Func<CRF_Item, bool>> filter)
        {
            return db.CRFItems.Where(filter);
        }

        public CRF_Item Create(CRF_Item obj)
        {
            return db.CRFItems.Add(obj);
        }

        public void Update(CRF_Item obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            CRF_Item crfItem = db.CRFItems.Find(id);
            if (crfItem != null)
                db.CRFItems.Remove(crfItem);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}