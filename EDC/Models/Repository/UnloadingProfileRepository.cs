using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class UnloadingProfileRepository :IRepository<UnloadingProfile>
    {
        private EDCContext db;
        public UnloadingProfileRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<UnloadingProfile> SelectAll()
        {
            return db.UnloadingProfiles;
        }

        public UnloadingProfile SelectByID(params object[] id)
        {
            return db.UnloadingProfiles.Find(id);
        }

        public IEnumerable<UnloadingProfile> GetManyByFilter(System.Linq.Expressions.Expression<Func<UnloadingProfile, bool>> filter)
        {
            return db.UnloadingProfiles.Where(filter);
        }

        public UnloadingProfile Create(UnloadingProfile obj)
        {
            return db.UnloadingProfiles.Add(obj);
        }

        public void Update(UnloadingProfile obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            var p = db.UnloadingProfiles.Find(id);
            if (p != null)
                db.UnloadingProfiles.Remove(p);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public void AddCenterToProfile(int unloadingProfileID,int medicalCenterID)
        {
            var up = db.UnloadingProfiles.Find(unloadingProfileID);
            var mc = db.MedicalCenters.Find(medicalCenterID);
            var upc = db.UnloadingProfilesCenters.Find(unloadingProfileID,medicalCenterID);
            if(up != null && mc != null && upc == null)
            {
                db.UnloadingProfilesCenters.Add(new UnloadingProfileCenter() { UnloadingProfileID = unloadingProfileID, MedicalCenterID = medicalCenterID });
            }
        }

        public void RemoveCenterFromProfile(int unloadingProfileID, int medicalCenterID)
        {
            var upc = db.UnloadingProfilesCenters.Find(unloadingProfileID, medicalCenterID);
            if (upc != null)
            {
                db.UnloadingProfilesCenters.Remove(upc);
            }
        }
    }
}