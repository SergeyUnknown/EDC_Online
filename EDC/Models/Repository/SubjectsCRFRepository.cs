using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class SubjectsCRFRepository : IRepository<SubjectsCRF>
    {
        private EDCContext db;

        public SubjectsCRFRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<SubjectsCRF> SelectAll()
        {
            return db.SubjectsCRFs;
        }
        public SubjectsCRF SelectByID(params object[] id)
        {
            return db.SubjectsCRFs.Find(id);
        }
        public IEnumerable<SubjectsCRF> GetManyByFilter(System.Linq.Expressions.Expression<Func<SubjectsCRF, bool>> filter)
        {
            return db.SubjectsCRFs.Where(filter);
        }
        public SubjectsCRF Create(SubjectsCRF obj)
        {
            var subject = db.Subjects.Find(obj.SubjectID);
            obj.IsDeleted = subject.IsDeleted;
            return db.SubjectsCRFs.Add(obj);
        }
        public void Update(SubjectsCRF obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            SubjectsEvent subjectEvent = db.SubjectsEvents.Find(id);
            if (subjectEvent != null)
                db.SubjectsEvents.Remove(subjectEvent);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}