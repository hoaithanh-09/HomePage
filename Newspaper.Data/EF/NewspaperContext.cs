using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Newspaper.Data.Entities;

#nullable disable

namespace Newspaper.Data.EF
{
    public partial class NewspaperContext : DbContext
    {
        public NewspaperContext()
        {
        }

        public NewspaperContext(DbContextOptions<NewspaperContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostInTopic> PostInTopics { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Author>(entity =>
            {
                entity.ToTable("Author");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<Image>(entity =>
            {
                entity.ToTable("Image");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.ImagePath)
                    .HasMaxLength(2000)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Author)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.AuthorId)
                    .HasConstraintName("FK__Post__AuthorId__3C69FB99");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.ImageId)
                    .HasConstraintName("FK__Post__ImageId__3D5E1FD2");
            });

            modelBuilder.Entity<PostInTopic>(entity =>
            {
                entity.HasKey(e => new { e.TopicId, e.PostId })
                    .HasName("PK__PostInTo__988F295CE2D4C5A8");

                entity.ToTable("PostInTopic");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostInTopics)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostInTop__PostI__412EB0B6");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.PostInTopics)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostInTop__Topic__403A8C7D");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("Topic");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
