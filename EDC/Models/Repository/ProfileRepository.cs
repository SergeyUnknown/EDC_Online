using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class ProfileRepository : IRepository<Profile>
    {
        private EDCContext db;

        public ProfileRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Profile> SelectAll()
        {
            return db.UserProfiles;
        }
        public Profile SelectByID(params object[] id)
        {
            return db.UserProfiles.Find(id);
        }
        public Profile SelectByUserID(Guid id)
        {
            return db.UserProfiles.FirstOrDefault(x=>x.UserID==id);
        }
        public IEnumerable<Profile> GetManyByFilter(System.Linq.Expressions.Expression<Func<Profile, bool>> filter)
        {
            return db.UserProfiles.Where(filter);
        }
        public Profile Create(Profile obj)
        {
            return db.UserProfiles.Add(obj);
        }
        public void Update(Profile obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            Profile profile = db.UserProfiles.Find(id);
            if (profile != null)
                db.UserProfiles.Remove(profile);
        }
        public void Save()
        {
            db.SaveChanges();
        }

    }
}