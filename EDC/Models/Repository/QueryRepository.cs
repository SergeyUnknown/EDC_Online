using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class QueryRepository : IRepository<Query>
    {
        private EDCContext db;

        public QueryRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Query> SelectAll()
        {
            return db.Queries;
        }
        public Query SelectByID(params object[] id)
        {
            return db.Queries.Find(id);
        }
        public IEnumerable<Query> GetManyByFilter(System.Linq.Expressions.Expression<Func<Query, bool>> filter)
        {
            return db.Queries.Where(filter);
        }
        public Query Create(Query obj)
        {
            return db.Queries.Add(obj);
        }
        public void Update(Query obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            Query Note = db.Queries.Find(id);
            if (Note != null)
                db.Queries.Remove(Note);
        }

        public void AddMessage(long queryID, QueryMessage message,bool header)
        {
            var query = db.Queries.Find(queryID);
            if (query == null)
                return;
            if(!header)
                query.Status = Core.QueryStatus.Updated;
            QueryMessage qm = new QueryMessage();
            qm.Text = message.Text;
            qm.From = message.From;
            qm.To = message.To;
            qm.CreationDate = DateTime.Now;
            query.Messages.Add(qm);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}