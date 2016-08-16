using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class MedicalCenterRepository: IRepository<MedicalCenter>
    {
        private EDCContext db;

        public MedicalCenterRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<MedicalCenter> SelectAll()
        {
            return db.MedicalCenters;
        }
        public MedicalCenter SelectByID(params object[] id)
        {
            return db.MedicalCenters.Find(id);
        }
        public IEnumerable<MedicalCenter> GetManyByFilter(System.Linq.Expressions.Expression<Func<MedicalCenter, bool>> filter)
        {
            return db.MedicalCenters.Where(filter);
        }
        public MedicalCenter Create(MedicalCenter obj)
        {
            return db.MedicalCenters.Add(obj);
        }
        public void Update(MedicalCenter obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(params object[] id)
        {
            MedicalCenter mc = db.MedicalCenters.Find(id);
            if (mc != null)
                db.MedicalCenters.Remove(mc);
        }
        public void Save()
        {
            db.SaveChanges();
        }
    }
}