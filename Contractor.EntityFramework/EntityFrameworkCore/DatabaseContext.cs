using Contractor.Chathub;
using Contractor.Contracts;
using Contractor.Correspondences;
using Contractor.Files;
using Contractor.GeneralEntities;
using Contractor.Identities;
using Contractor.Lookups;
using Contractor.Projects;
using Contractor.Subscriptions;
using Contractor.Tenders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Contractor.EntityFrameworkCore
{
    public class DatabaseContext : IdentityDbContext<User, Role, string, UserClaim, UserRole, IdentityUserLogin<string>, RoleClaim, IdentityUserToken<string>>
    {

        public DbSet<Page> Pages { get; set; }

        public DbSet<OutGoingType> OutGoingTypes { get; set; }

        public DbSet<IncomeType> IncomeTypes { get; set; }
        
        public DbSet<ContractType> ContractTypes { get; set; }

        public DbSet<Nationality> Nationalities { get; set; }

        public DbSet<City> Cities { get; set; }

        public DbSet<Area> Areas { get; set; }

        public DbSet<ProjectType> ProjectTypes { get; set; }

        public DbSet<Contractor.Files.File> Files { get; set; }
        
        public DbSet<DraftProject> DraftProjects { get; set; }

        public DbSet<Subscription> Subscriptions { get; set; }
        
        public DbSet<AccessDefinition> AccessDefinitions { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Company> Companies { get; set; }

        public DbSet<Tender> Tenders { get; set; }

        public DbSet<Correspondence> Correspondences { get; set; }
        
        public DbSet<CorrespondenceThread> CorrespondenceThreads { get; set; }

        public DbSet<ChatMessage> ChatMessages { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();
                }
            }

            modelBuilder.Entity<TenderAccessDefinition>(b =>
            {
                b.HasKey(x => new { x.TenderId, x.AccessDefinitionId });
                b.HasOne(x => x.Tender)
                .WithMany(x => x.TenderAccessDefinitions)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<ProfileAccessDefinition>(b =>
            {
                b.HasOne(x => x.User)
                .WithMany(x => x.ProfileAccessDefinitions)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<CorrespondenceAccessDefinition>(b =>
            {
                b.HasKey(x => new { x.CorrespondenceId, x.AccessDefinitionId });
                b.HasOne(x => x.Correspondence)
                .WithMany(x => x.CorrespondenceAccessDefinitions)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<CorrespondenceRecipient>(b =>
            {
                b.HasOne(x => x.Recipient)
                .WithMany(x => x.CorrespondenceRecipients)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<Correspondence>(b =>
            {
                b.HasOne(x => x.Project)
                .WithMany(x => x.Correspondences)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<InvitationAccessDefinition>(b =>
            {
                b.HasKey(x => new { x.TenderInvitationId, x.AccessDefinitionId });
                b.HasOne(x => x.TenderInvitation)
                .WithMany(x => x.InvitationAccessDefinitions)
                .OnDelete(DeleteBehavior.NoAction);
            });

            modelBuilder.Entity<User>(b =>
            {
                b.HasIndex(p => p.PhoneNumber)
                .IsUnique();
                b.HasOne(p => p.Profile).WithOne(b=>b.User).IsRequired();
            });

            modelBuilder.Entity<Subscription>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<AccessDefinition>()
                .HasOne(x => x.Original)
                .WithMany(x => x.Copies)
                .HasForeignKey(x => x.OriginalId);

            modelBuilder.Entity<AccessDefinition>()
                .HasOne(x => x.File)
                .WithMany()
                .HasForeignKey(x => x.FileId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<DraftProject>()
                .HasOne(x => x.Owner)
                .WithMany(x => x.OwnerDraftProjects)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<DraftProject>()
                .HasOne(x => x.Consultant)
                .WithMany(x => x.ConsultantDraftProjects)
                .HasForeignKey(x => x.ConsultantId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Page>()
                .HasIndex(p => p.Name)
                .IsUnique();

            modelBuilder.Entity<User>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                b.HasMany(e => e.Roles)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();
            });

            modelBuilder.Entity<Role>(b =>
            {
                // Each Role can have many RoleClaims
                b.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.RoleId)
                    .IsRequired();

                b.HasMany(e => e.Users)
                    .WithOne(e => e.Role)
                    .HasForeignKey(uc => uc.RoleId)
                    .IsRequired();
            });

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = ChangeTracker.Entries()
                        .Where(e => e.State == EntityState.Deleted);
            foreach (var entity in entities)
            {
                if (entity.Entity is ISoftDelete)
                {
                    entity.State = EntityState.Modified;
                    var book = entity.Entity as ISoftDelete;
                    book!.IsDeleted = true;
                }

                //TODO if the entity Inherite from IAuditEntity, fill the data HERE.
            }
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
