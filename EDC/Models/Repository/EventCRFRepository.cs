using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class EventCRFRepository : IRepository<EventCRF>
    {
        private EDCContext db;

        public EventCRFRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<EventCRF> SelectAll()
        {
            return db.EventsCRFs;
        }
        public EventCRF SelectByID(params object[] id)
        {
            return db.EventsCRFs.Find(id);
        }
        public IEnumerable<EventCRF> GetManyByFilter(System.Linq.Expressions.Expression<Func<EventCRF, bool>> filter)
        {
            return db.EventsCRFs.Where(filter);
        }
        public EventCRF Create(EventCRF obj)
        {
            obj.CRF = db.CRFs.Find(obj.CRFID);
            obj.Event = db.Events.Find(obj.EventID);

            return db.EventsCRFs.Add(obj);
        }
        public void Update(EventCRF obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            EventCRF _EventCRF = db.EventsCRFs.Find(id);
            if (_EventCRF != null)
                db.EventsCRFs.Remove(_EventCRF);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}