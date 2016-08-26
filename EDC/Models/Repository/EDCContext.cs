using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

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
        public DbSet<SubjectEvent> SubjectsEvent { get; set; } //Связующая таблица

        public DbSet<SubjectsCRF> SubjectsCRFs { get; set; }
        public DbSet<SubjectsItem> SubjectsItems { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CRFInEvent>()
                .HasKey(t => new { t.CRFID, t.EventID });

            modelBuilder.Entity<SubjectEvent>()
                .HasKey(t => new { t.EventID,t.SubjectID});

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