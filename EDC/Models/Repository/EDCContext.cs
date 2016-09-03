using System;
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

        public DbSet<UserProfile> UsersProfiles { get; set; } //профиль пользователя
        public DbSet<MedicalCenter> MedicalCenters { get; set; } //мед центры
        public DbSet<CRF> CRFs { get; set; } // CRF
        public DbSet<CRF_Section> CRFSections { get; set; } //Секции CRF
        public DbSet<CRF_Group> CRFGroups { get; set; } //Группы CRF
        public DbSet<CRF_Item> CRFItems { get; set; } //Итемы CRF
        public DbSet<Audit> Audits { get; set; } //Аудит
        public DbSet<Event> Events { get; set; } //События
        public DbSet<CRFInEvent> CRFInEvent { get; set; } //Связующая таблица
        public DbSet<Note> Notes { get; set; } // 
        public DbSet<Subject> Subjects { get; set; } // Субъекты исследования 
        public DbSet<SubjectsEvent> SubjectsEvents { get; set; } //Связующая таблица

        public DbSet<SubjectsCRF> SubjectsCRFs { get; set; }
        public DbSet<SubjectsItem> SubjectsItems { get; set; }

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



            modelBuilder.Entity<CRFInEvent>()
                .HasKey(t => new { t.CRFID, t.EventID });

            modelBuilder.Entity<SubjectsEvent>()
                .HasKey(t => new { t.SubjectID, t.EventID });

            modelBuilder.Entity<SubjectsCRF>()
                .HasKey(t => new { t.SubjectID, t.EventID, t.CRFID });

            modelBuilder.Entity<SubjectsItem>()
                .HasKey(t => new { t.SubjectID,t.EventID,t.CRFID,t.ItemID });

            

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