using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class CRFInEventRepository : IRepository<CRFInEvent>
    {
        private EDCContext db;

        public CRFInEventRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<CRFInEvent> SelectAll()
        {
            return db.CRFInEvent;
        }
        public CRFInEvent SelectByID(params object[] id)
        {
            return db.CRFInEvent.Find(id);
        }
        public IEnumerable<CRFInEvent> GetManyByFilter(System.Linq.Expressions.Expression<Func<CRFInEvent, bool>> filter)
        {
            return db.CRFInEvent.Where(filter);
        }
        public CRFInEvent Create(CRFInEvent obj)
        {
            obj.CRF = db.CRFs.Find(obj.CRFID);
            obj.Event = db.Events.Find(obj.EventID);

            return db.CRFInEvent.Add(obj);
        }
        public void Update(CRFInEvent obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            CRFInEvent _EventCRF = db.CRFInEvent.Find(id);
            if (_EventCRF != null)
                db.CRFInEvent.Remove(_EventCRF);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}