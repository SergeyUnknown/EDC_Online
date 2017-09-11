﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDC.Models
{
    public class EDCContext : DbContext
    {
        public DbSet<AppSetting> AppSettings { get; set; } //настройки приложения
        public DbSet<UserProfile> UsersProfiles { get; set; } //профиль пользователя
        public DbSet<MedicalCenter> MedicalCenters { get; set; } //мед центры
        public DbSet<CRF> CRFs { get; set; } // CRF
        public DbSet<CRF_Section> CRFSections { get; set; } //Секции CRF
        public DbSet<CRF_Group> CRFGroups { get; set; } //Группы CRF
        public DbSet<CRF_Item> CRFItems { get; set; } //Итемы CRF
        public DbSet<Audit> Audits { get; set; } //Аудит
        public DbSet<AuditEditReason> AuditsEditReason { get; set; } //Аудит
        public DbSet<Event> Events { get; set; } //События
        public DbSet<CRFInEvent> CRFInEvent { get; set; } //Связующая таблица
        public DbSet<Query> Queries { get; set; } // 
        public DbSet<QueryMessage> QueryMessages { get; set; } // 
        public DbSet<Subject> Subjects { get; set; } // Субъекты исследования 
        public DbSet<SubjectsEvent> SubjectsEvents { get; set; } //Связующая таблица
        public DbSet<SubjectsCRF> SubjectsCRFs { get; set; }
        public DbSet<SubjectsItem> SubjectsItems { get; set; }
        public DbSet<AccessToCenter> AccessToCenter { get; set; }
        public DbSet<Rule> Rules { get; set; }
        public DbSet<UnloadingProfile> UnloadingProfiles { get; set; }
        public DbSet<UnloadingProfileCenter> UnloadingProfilesCenters { get; set; }
        public DbSet<UnloadingItem> UnloadingItems { get; set; }
        public DbSet<UnloadingFile> UnloadingFiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>()
                .HasMany(x => x.Items)
                .WithRequired(x => x.Subject)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Event>()
                .HasMany(x => x.Items)
                .WithRequired(x => x.Event)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRF>()
                .HasMany(x => x.Values)
                .WithRequired(x => x.CRF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRF>()
                .HasMany(x => x.Sections)
                .WithRequired(x => x.CRF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRF>()
                .HasMany(x => x.Rules)
                .WithRequired(x => x.CRF)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRF_Group>()
                .HasMany(x => x.Rules)
                .WithRequired(x => x.Group)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRF_Item>()
                .HasMany(x => x.Notes)
                .WithRequired(x => x.CRFItem)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MedicalCenter>()
                .HasMany(x => x.Subjects)
                .WithRequired(x => x.MedicalCenter)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<CRFInEvent>()
                .HasKey(t => new { t.CRFID, t.EventID });

            modelBuilder.Entity<SubjectsEvent>()
                .HasKey(t => new { t.SubjectID, t.EventID });

            modelBuilder.Entity<SubjectsCRF>()
                .HasKey(t => new { t.SubjectID, t.EventID, t.CRFID });

            modelBuilder.Entity<SubjectsItem>()
                .HasKey(t => new { t.SubjectID,t.EventID,t.CRFID,t.ItemID,t.IndexID });

            modelBuilder.Entity<SubjectsItem>()
                .HasRequired(x => x.SubjectCRF)
                .WithMany(x => x.Items)
                .HasForeignKey(t => new { t.SubjectID, t.EventID, t.CRFID })
                .WillCascadeOnDelete(false);
                

            modelBuilder.Entity<Query>()
                .HasRequired(x => x.SubjectItem)
                .WithMany(x => x.Queries)
                .HasForeignKey(t => new { t.SubjectID, t.EventID, t.CRFID, t.ItemID, t.IndexID })
                .WillCascadeOnDelete(false);

            /////////////////ForeingKey Аудит//////////
            modelBuilder.Entity<Audit>()
                .HasOptional(x => x.Subject)
                .WithMany(x => x.Audits)
                .HasForeignKey(t => new { t.SubjectID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Audit>()
                .HasOptional(x => x.SubjectEvent)
                .WithMany(x => x.Audits)
                .HasForeignKey(t => new { t.SubjectID, t.EventID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Audit>()
                .HasOptional(x => x.SubjectCRF)
                .WithMany(x => x.Audits)
                .HasForeignKey(t => new { t.SubjectID, t.EventID, t.CRFID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Audit>()
                .HasOptional(x => x.SubjectItem)
                .WithMany(x => x.Audits)
                .HasForeignKey(t => new { t.SubjectID, t.EventID, t.CRFID, t.ItemID,t.IndexID })
                .WillCascadeOnDelete(false);
            ///////////////////////////////////////////

            modelBuilder.Entity<AuditEditReason>()
                .HasOptional(x => x.SubjectCRF)
                .WithMany(x => x.EditReasons)
                .HasForeignKey(t => new { t.SubjectID, t.EventID, t.CRFID })
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AccessToCenter>()
                .HasKey(t => new { t.UserID, t.MedicalCenterID });

            modelBuilder.Entity<UnloadingItem>()
                .HasKey(t => new { t.UnloadingProfileID, t.EventID, t.CRFID, t.ItemID });

            modelBuilder.Entity<UnloadingItem>()
                .HasRequired(x => x.CRFInEvent)
                .WithMany(x => x.UnloadingItems)
                .HasForeignKey(t => new { t.CRFID, t.EventID })
                .WillCascadeOnDelete(false);
                

            modelBuilder.Entity<UnloadingItem>()
                .HasRequired(x => x.Event)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnloadingItem>()
                .HasRequired(x => x.CRF)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnloadingItem>()
                .HasRequired(x => x.Item)
                .WithMany()
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UnloadingProfileCenter>()
                .HasKey(t => new { t.UnloadingProfileID, t.MedicalCenterID });
        }
    }
    public class EDCInitializer : MigrateDatabaseToLatestVersion<EDCContext, Configuration>
    {

    }

    public sealed class Configuration : System.Data.Entity.Migrations.DbMigrationsConfiguration<EDCContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(EDCContext context)
        {

        }
    }

}