using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class SubjectsItemRepoitory : IRepository<SubjectsItem>
    {
        private EDCContext db;

        public SubjectsItemRepoitory()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<SubjectsItem> SelectAll()
        {
            return db.SubjectsItems;
        }

        public SubjectsItem SelectByID(params object[] id)
        {
            return db.SubjectsItems.Find(id);
        }

        public IEnumerable<SubjectsItem> GetManyByFilter(System.Linq.Expressions.Expression<Func<SubjectsItem, bool>> filter)
        {
            return db.SubjectsItems.Where(filter);
        }

        public SubjectsItem Create(SubjectsItem obj)
        {
            return db.SubjectsItems.Add(obj);
        }

        public void Update(SubjectsItem obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            SubjectsCRF subjectCRF = db.SubjectsCRFs.Find(id);
            if (subjectCRF != null)
                db.SubjectsCRFs.Remove(subjectCRF);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}