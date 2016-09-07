using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class NoteRepository : IRepository<Note>
    {
        private EDCContext db;

        public NoteRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Note> SelectAll()
        {
            return db.Notes;
        }
        public Note SelectByID(params object[] id)
        {
            return db.Notes.Find(id);
        }
        public IEnumerable<Note> GetManyByFilter(System.Linq.Expressions.Expression<Func<Note, bool>> filter)
        {
            return db.Notes.Where(filter);
        }
        public Note Create(Note obj)
        {
            return db.Notes.Add(obj);
        }
        public void Update(Note obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            Note Note = db.Notes.Find(id);
            if (Note != null)
                db.Notes.Remove(Note);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}