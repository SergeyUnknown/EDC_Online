using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class AppSettingRepository : IRepository<AppSetting>
    {
        private EDCContext db;

        public AppSettingRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<AppSetting> SelectAll()
        {
            return db.AppSettings;
        }

        public AppSetting SelectByID(params object[] id)
        {
            if (db.AppSettings.Find(id) == null)
            {
                db.AppSettings.Add(new AppSetting(id.ToString(), ""));
                db.SaveChanges();
                return db.AppSettings.Find(id);
            }
            else
                return db.AppSettings.Find(id);
        }

        public IEnumerable<AppSetting> GetManyByFilter(System.Linq.Expressions.Expression<Func<AppSetting, bool>> filter)
        {
            return db.AppSettings.Where(filter);
        }

        public AppSetting Create(AppSetting obj)
        {
            return db.AppSettings.Add(obj);
        }

        public void Update(AppSetting obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            AppSetting aS = db.AppSettings.Find(id);
            if (aS != null)
                db.AppSettings.Remove(aS);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}