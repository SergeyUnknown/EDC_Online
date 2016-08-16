using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class EventRepository : IRepository<Event>
    {
        private EDCContext db;

        public EventRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Event> SelectAll()
        {
            return db.Events;
        }
        public Event SelectByID(params object[] id)
        {
            return db.Events.Find(id);
        }
        public IEnumerable<Event> GetManyByFilter(System.Linq.Expressions.Expression<Func<Event, bool>> filter)
        {
            return db.Events.Where(filter);
        }
        public Event Create(Event obj)
        {
            return db.Events.Add(obj);
        }
        public void Update(Event obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            Event _event = db.Events.Find(id);
            if (_event != null)
                db.Events.Remove(_event);
        }
        public void Save()
        {
            db.SaveChanges();
        }

    }
}