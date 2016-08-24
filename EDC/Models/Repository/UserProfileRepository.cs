using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class UserProfileRepository : IRepository<UserProfile>
    {
        private EDCContext db;

        public UserProfileRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<UserProfile> SelectAll()
        {
            return db.UsersProfiles;
        }
        public UserProfile SelectByID(params object[] id)
        {
            return db.UsersProfiles.Find(id);
        }
        public UserProfile SelectByUserID(Guid id)
        {
            return db.UsersProfiles.FirstOrDefault(x=>x.UserID==id);
        }
        public IEnumerable<UserProfile> GetManyByFilter(System.Linq.Expressions.Expression<Func<UserProfile, bool>> filter)
        {
            return db.UsersProfiles.Where(filter);
        }
        public UserProfile Create(UserProfile obj)
        {
            return db.UsersProfiles.Add(obj);
        }
        public void Update(UserProfile obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            UserProfile profile = db.UsersProfiles.Find(id);
            if (profile != null)
                db.UsersProfiles.Remove(profile);
        }
        public void Save()
        {
            db.SaveChanges();
        }

    }
}