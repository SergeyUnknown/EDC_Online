using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDC.Models.Repository
{
    public class CRFRepository : IRepository<CRF>
    {
        EDCContext db;
        public CRFRepository()
        {
            db = new EDCContext();
        }

        public IEnumerable<CRF> SelectAll()
        {
            return db.CRFs;
        }

        public CRF SelectByID(params object[] id)
        {
            return db.CRFs.Find(id);
        }

        public IEnumerable<CRF> GetManyByFilter(System.Linq.Expressions.Expression<Func<CRF, bool>> filter)
        {
            return db.CRFs.Where(filter);
        }

        public CRF Create(CRF obj)
        {
            return db.CRFs.Add(obj);
        }

        public void Update(CRF obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }

        public void Delete(params object[] id)
        {

            CRF crf = db.CRFs.Find(id);
            if (crf != null)
            {
                List<CRF_Item> items = db.CRFItems.Where(x=>x.CRFID == crf.CRFID).ToList();
                List<CRF_Section> sections = db.CRFSections.Where(x=>x.CRFID == crf.CRFID).ToList();
                List<CRF_Group> groups = db.CRFGroups.Where(x=>x.CRFID == crf.CRFID).ToList();

                foreach(var item in items)
                {
                    db.CRFItems.Remove(item);
                }

                foreach(var section in sections)
                {
                    db.CRFSections.Remove(section);
                }

                foreach(var group in groups)
                {
                    db.CRFGroups.Remove(group);
                }

                db.CRFs.Remove(crf);
            }
        }

        public void Save()
        {
            db.SaveChanges();
        }

    }
}