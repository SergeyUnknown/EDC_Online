using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class UnloadingFileRepository :IRepository<UnloadingFile>
    {
        private EDCContext db;
        public UnloadingFileRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<UnloadingFile> SelectAll()
        {
            return db.UnloadingFiles;
        }

        public UnloadingFile SelectByID(params object[] id)
        {
            return db.UnloadingFiles.Find(id);
        }

        public IEnumerable<UnloadingFile> GetManyByFilter(System.Linq.Expressions.Expression<Func<UnloadingFile, bool>> filter)
        {
            return db.UnloadingFiles.Where(filter);
        }

        public UnloadingFile Create(UnloadingFile obj)
        {
            return db.UnloadingFiles.Add(obj);
        }

        public void Update(UnloadingFile obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            var p = db.UnloadingFiles.Find(id);
            if (p != null)
                db.UnloadingFiles.Remove(p);
        }

        public void Save()
        {
            db.SaveChanges();
        }

    }
}