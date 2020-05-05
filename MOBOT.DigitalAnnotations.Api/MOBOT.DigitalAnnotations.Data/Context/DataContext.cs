using Microsoft.EntityFrameworkCore;
using MOBOT.DigitalAnnotations.Data.Entities;
using MOBOT.DigitalAnnotations.Data.Interfaces;

namespace MOBOT.DigitalAnnotations.Data.Context
{
    public class DataContext : DbContext, IUnitOfWork
    {
        public DataContext(DbContextOptions<DataContext> opts) : base(opts) { }

        public DbSet<AnnotationSource> AnnotationSources { get; set; }
        public DbSet<Annotation> Annotations { get; set; }
        public DbSet<AnnotationTarget> AnnotationTargets { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<License> Licenses { get; set; }
        public DbSet<AnnotationType> AnnotationTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<AnnotationTag> AnnotationTags { get; set; }
        public DbSet<VocabularyListType> VocabularyListTypes { get; set; }
        public DbSet<VocabularyTerm> VocabularyTerms { get; set; }
        public DbSet<VocabularyList> VocabularyLists { get; set; }
        public DbSet<VocabularyListView> VocabularyListViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            base.OnModelCreating(builder);

            builder.Entity<Annotation>()
                .HasOne(a => a.CreatedUser)
                .WithMany(u => u.CreatedAnnotations).HasForeignKey(a => a.CreatedUserId)
                .IsRequired();

            builder.Entity<Annotation>()
                .HasOne(a => a.UpdatedUser)
                .WithMany(u => u.UpdatedAnnotations).HasForeignKey(a => a.UpdatedUserId)
                .IsRequired(false);

            builder.Entity<Annotation>()
                .HasOne(a => a.Group)
                .WithMany(g => g.Annotations).HasForeignKey(a => a.GroupId)
                .IsRequired();

            builder.Entity<Annotation>()
                .HasOne(a => a.License)
                .WithMany(l => l.Annotations).HasForeignKey(a => a.LicenseId)
                .HasConstraintName("Fk_Annotation_License")
                .IsRequired(false);

            builder.Entity<Annotation>()
                .HasOne(a => a.AnnotationType)
                .WithMany(at => at.Annotations)
                .HasForeignKey(a => a.AnnotationTypeId)
                .HasConstraintName("Fk_Annotation_AnnotationType")
                .IsRequired(false);

            builder.Entity<Annotation>()
                .HasOne(a => a.Parent)
                .WithMany(p => p.Annotations)
                .HasForeignKey(a => a.ParentId)
                .HasConstraintName("Fk_Annotation_AnnotationParent")
                .IsRequired(false);

            builder.Entity<AnnotationTag>().HasIndex(at => new { at.AnnotationId, at.TagId }).IsUnique(true).HasName("Ux_AnnotationTags_AnnotationId_TagId");
            builder.Entity<AnnotationTag>()
                .HasOne(at => at.Annotation)
                .WithMany(a => a.Tags)
                .HasForeignKey(at => at.AnnotationId)
                .HasConstraintName("Fk_AnnotationTags_Annotation");
            builder.Entity<AnnotationTag>()
                .HasOne(at => at.Tag)
                .WithMany(t => t.Annotations)
                .HasForeignKey(at => at.TagId)
                .HasConstraintName("Fk_AnnotationTags_Tag");

            builder.Entity<Tag>().HasIndex(a => a.TagName).IsUnique(true).HasName("Ux_Tag_TagName");

            builder.Entity<User>().HasIndex(e => new { e.Email }).IsUnique(true);

            builder.Entity<UserGroup>().HasAlternateKey(ug => new { ug.UserId, ug.GroupId } ).HasName("UX_UserGroups_UserId_GroupId");
            builder.Entity<UserGroup>().HasOne(ug => ug.User).WithMany(u => u.Groups).HasForeignKey(ug => ug.UserId);
            builder.Entity<UserGroup>().HasOne(ug => ug.Group).WithMany(g => g.Users).HasForeignKey(ug => ug.GroupId);

            builder.Entity<License>().HasKey(l => l.LicenseId).HasName("Pk_License");
            builder.Entity<License>().Property(l => l.Code).IsFixedLength(true).HasMaxLength(10);
            builder.Entity<License>().HasIndex(l => l.Code).IsUnique(true).HasName("Ux_License_Code");

            builder.Entity<VocabularyListType>().HasIndex(v => v.TypeTerm).IsUnique().HasName("Ux_VocabularyListType_Term");
            builder.Entity<VocabularyTerm>().HasIndex(v => v.Term).IsUnique().HasName("Ux_VocabularyTerm_Term");

            builder.Entity<VocabularyList>().HasIndex(v => new { v.ListTypeId, v.TermId }).IsUnique().HasName("Ux_VocabularyList_ListTypeId_TermId");
            builder.Entity<VocabularyList>()
                .HasOne(vl => vl.ListType)
                .WithMany(lt => lt.Terms)
                .HasForeignKey(vl => vl.ListTypeId)
                .HasConstraintName("Fk_VocabularyList_ListType");
            builder.Entity<VocabularyList>()
                .HasOne(vl => vl.Term)
                .WithMany(t => t.Lists)
                .HasForeignKey(vl => vl.TermId)
                .HasConstraintName("Fk_VocabularyList_Term");
        }
    }
}
