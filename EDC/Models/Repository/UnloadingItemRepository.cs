using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class UnloadingItemRepository :IRepository<UnloadingItem>
    {
        private EDCContext db;
        public UnloadingItemRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<UnloadingItem> SelectAll()
        {
            return db.UnloadingItems;
        }

        public UnloadingItem SelectByID(params object[] id)
        {
            return db.UnloadingItems.Find(id);
        }

        public IEnumerable<UnloadingItem> GetManyByFilter(System.Linq.Expressions.Expression<Func<UnloadingItem, bool>> filter)
        {
            return db.UnloadingItems.Where(filter);
        }

        public UnloadingItem Create(UnloadingItem obj)
        {
            return db.UnloadingItems.Add(obj);
        }
        public IEnumerable<UnloadingItem> Create(IEnumerable<UnloadingItem> objs)
        {
            return db.UnloadingItems.AddRange(objs);
        }

        public void Update(UnloadingItem obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            var i = db.UnloadingItems.Find(id);
            if (i != null)
                db.UnloadingItems.Remove(i);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void SaveAsync()
        {
            db.SaveChangesAsync();
        }
    }
}