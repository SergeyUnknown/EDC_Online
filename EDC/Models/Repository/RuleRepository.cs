using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class RuleRepository: IRepository<Rule>
    {
        private EDCContext db;

        public RuleRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<Rule> SelectAll()
        {
            return db.Rules;
        }

        public Rule SelectByID(params object[] id)
        {
            return db.Rules.Find(id);
        }

        public IEnumerable<Rule> GetManyByFilter(System.Linq.Expressions.Expression<Func<Rule, bool>> filter)
        {
            return db.Rules.Where(filter);
        }

        public Rule Create(Rule obj)
        {
            return db.Rules.Add(obj);
        }

        public void Update(Rule obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            Rule rule = db.Rules.Find(id);
            if (rule != null)
                db.Rules.Remove(rule);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}