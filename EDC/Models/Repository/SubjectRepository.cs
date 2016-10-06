using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Principal;
using System.Web.Security;

namespace EDC.Models.Repository
{
    public class SubjectRepository : IRepository<Subject>
    {
        private EDCContext db;

        public SubjectRepository()
        {
            this.db = new EDCContext();
        }
        public IEnumerable<Subject> SelectAll()
        {
            return db.Subjects;
        }
        public Subject SelectByID(params object[] id)
        {
            return db.Subjects.Find(id);
        }
        public IEnumerable<Subject> GetManyByFilter(System.Linq.Expressions.Expression<Func<Subject, bool>> filter)
        {
            return db.Subjects.Where(filter);
        }
        public Subject Create(Subject obj)
        {
            return db.Subjects.Add(obj);
        }
        public void Update(Subject obj)
        {
            db.Entry(obj).State = System.Data.Entity.EntityState.Modified;
        }
        public void Delete(long id, System.Security.Principal.IPrincipal User)
        {
            Subject subject = db.Subjects.Find(id);
            var mU = Membership.GetUser(User.Identity.Name);
            if (subject != null)
            {
                subject.IsDeleted = true;
                foreach(var item in subject.Events)
                {
                    item.IsDeleted = true;

                    Audit audit = new Audit();
                    audit.UserID = (Guid)mU.ProviderUserKey;
                    audit.SubjectID = item.SubjectID;
                    audit.EventID = item.EventID;
                    audit.UserName = User.Identity.Name;
                    audit.ActionDate = DateTime.Now;
                    audit.ActionType = Core.AuditActionType.SubjectEvent;
                    audit.ChangesType = Core.AuditChangesType.Delete;
                    db.Audits.Add(audit);
                }
                foreach (var item in subject.CRFs)
                {
                    item.IsDeleted = true;

                    Audit audit = new Audit();
                    audit.UserID = (Guid)mU.ProviderUserKey;
                    audit.SubjectID = item.SubjectID;
                    audit.EventID = item.EventID;
                    audit.CRFID = item.CRFID;
                    audit.UserName = User.Identity.Name;
                    audit.ActionDate = DateTime.Now;
                    audit.ActionType = Core.AuditActionType.SubjectEvent;
                    audit.ChangesType = Core.AuditChangesType.Delete;
                    db.Audits.Add(audit);
                }
                foreach (var item in subject.Items)
                {
                    item.IsDeleted = true;

                    Audit audit = new Audit();
                    audit.UserID = (Guid)mU.ProviderUserKey;
                    audit.SubjectID = item.SubjectID;
                    audit.EventID = item.EventID;
                    audit.CRFID = item.CRFID;
                    audit.ItemID = item.ItemID;
                    audit.UserName = User.Identity.Name;
                    audit.ActionDate = DateTime.Now;
                    audit.ActionType = Core.AuditActionType.SubjectEvent;
                    audit.ChangesType = Core.AuditChangesType.Delete;
                    db.Audits.Add(audit);
                }

                Audit _audit = new Audit();
                _audit.UserID = (Guid)mU.ProviderUserKey;
                _audit.SubjectID = id;
                _audit.UserName = User.Identity.Name;
                _audit.ActionDate = DateTime.Now;
                _audit.ActionType = Core.AuditActionType.SubjectEvent;
                _audit.ChangesType = Core.AuditChangesType.Delete;
                db.Audits.Add(_audit);
            }
        }
        public void Delete(params object[] id)
        {
            Subject Subject = db.Subjects.Find(id);
            if (Subject != null)
                db.Subjects.Remove(Subject);
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}