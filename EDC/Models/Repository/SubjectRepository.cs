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
        public IEnumerable<Subject> SelectAllForUser(System.Security.Principal.IPrincipal User)
        {
            var up = db.UsersProfiles.Find(Membership.GetUser(User.Identity.Name).ProviderUserKey);
            long? centerID = up != null ? up.GetCurrentCenterID() : null; 
            if (centerID != null)
                return db.Subjects.Where(x => x.MedicalCenterID == centerID);
            else
                return new List<Subject>();
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

            if (subject == null)
                return;

            subject.IsDeleted = true;
            subject.IsDeletedBy = User.Identity.Name;
            subject.IsDeletedDate = DateTime.Now;

            foreach (var item in subject.Events)
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
                item.IsDeletedBy = User.Identity.Name;
                item.IsDeletedDate = DateTime.Now;

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

        public void StopStart(long id, System.Security.Principal.IPrincipal User, string editReason, bool stopState)
        {
            Subject subject = db.Subjects.Find(id);
            var mU = Membership.GetUser(User.Identity.Name);

            if (subject == null)
                return;

            AuditEditReason aer = new AuditEditReason();
            aer.ActionDate = DateTime.Now;
            aer.SubjectID = id;
            aer.UserName = User.Identity.Name;
            aer.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;
            
            subject.IsStopped = stopState;
            if (stopState)
            {
                subject.IsStoppedBy = User.Identity.Name;
                subject.IsStoppedDate = DateTime.Now;
                aer.EditReason = "Остановка. Причина: "+editReason;
            }
            else
            {
                subject.IsStoppedBy = "";
                subject.IsStoppedDate = null;
                aer.EditReason = "Возобновление ввода данных. Причина: "+editReason;
            }

            foreach (var item in subject.Events)
            {
                item.IsStopped = stopState;
            }

            foreach (var item in subject.CRFs)
            {
                item.IsStopped = stopState;
                if (stopState)
                {
                    item.IsStoppedBy = User.Identity.Name;
                    item.IsStoppedDate = DateTime.Now;
                }
                else
                {
                    item.IsStoppedBy = "";
                    item.IsStoppedDate = null;
                }
            }

            foreach (var item in subject.Items)
            {
                item.IsStopped = stopState;
            }
            db.AuditsEditReason.Add(aer);
        }

        public void LockUnlock(long id, System.Security.Principal.IPrincipal User, string editReason, bool lockState)
        {
            Subject subject = db.Subjects.Find(id);
            var mU = Membership.GetUser(User.Identity.Name);

            if (subject == null)
                return;

            AuditEditReason aer = new AuditEditReason();
            aer.ActionDate = DateTime.Now;
            aer.SubjectID = id;
            aer.UserName = User.Identity.Name;
            aer.UserID = (Guid)System.Web.Security.Membership.GetUser(User.Identity.Name).ProviderUserKey;

            subject.IsLock = lockState;
            if (lockState)
            {
                subject.IsLockBy = User.Identity.Name;
                subject.IsLockDate = DateTime.Now;
                aer.EditReason = "Блокировка. Причина: "+editReason;
            }
            else
            {
                subject.IsLockBy = "";
                subject.IsLockDate = null;
                aer.EditReason = "Разблокировка. Причина: "+editReason;
            }

            foreach (var item in subject.Events)
            {
                item.IsLock = lockState;
            }
            foreach (var item in subject.CRFs)
            {
                item.IsLock = lockState;
                if (lockState)
                {
                    item.IsLockBy = User.Identity.Name;
                    item.IsLockDate = DateTime.Now;
                }
                else
                {
                    item.IsLockBy = "";
                    item.IsLockDate = null;
                }
            }
            foreach (var item in subject.Items)
            {
                item.IsLock = lockState;
            }

            db.AuditsEditReason.Add(aer);
        }

        public void Delete(params object[] id)
        {
            
        }

        public void Save()
        {
            db.SaveChanges();
        }
    }
}