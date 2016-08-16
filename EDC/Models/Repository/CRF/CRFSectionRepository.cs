using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class CRFSectionRepository : IRepository<CRF_Section>
    {
        EDCContext db;
        public CRFSectionRepository()
        {
            db = new EDCContext();
        }
        public IEnumerable<CRF_Section> SelectAll()
        {
            return db.CRFSections;
        }

        public CRF_Section SelectByID(params object[] id)
        {
            return db.CRFSections.Find(id);
        }

        public IEnumerable<CRF_Section> GetManyByFilter(System.Linq.Expressions.Expression<Func<CRF_Section, bool>> filter)
        {
            return db.CRFSections.Where(filter);
        }

        public CRF_Section Create(CRF_Section obj)
        {
            return db.CRFSections.Add(obj);
        }

        public void Update(CRF_Section obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {
            CRF_Section crfSection = db.CRFSections.Find(id);
            if (crfSection != null)
                db.CRFSections.Remove(crfSection);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}