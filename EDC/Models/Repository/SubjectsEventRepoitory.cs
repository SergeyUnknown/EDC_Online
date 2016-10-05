using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class SubjectsEventRepoitory : IRepository<SubjectsEvent>
    {
        private EDCContext db;

        public SubjectsEventRepoitory()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<SubjectsEvent> SelectAll()
        {
            return db.SubjectsEvents;
        }

        public SubjectsEvent SelectByID(params object[] id)
        {
            return db.SubjectsEvents.Find(id);
        }

        public IEnumerable<SubjectsEvent> GetManyByFilter(System.Linq.Expressions.Expression<Func<SubjectsEvent, bool>> filter)
        {
            return db.SubjectsEvents.Where(filter);
        }

        public SubjectsEvent Create(SubjectsEvent obj)
        {
            var subject = db.Subjects.Find(obj.SubjectID);
            obj.IsDeleted = subject.IsDeleted;
            return db.SubjectsEvents.Add(obj);
        }

        public void Update(SubjectsEvent obj)
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