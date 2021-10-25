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
        public virtual DbSet<ImageInPost> ImageInPosts { get; set; }
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

            modelBuilder.Entity<ImageInPost>(entity =>
            {
                entity.HasKey(e => new { e.ImageId, e.PostId })
                    .HasName("PK__ImageInP__EFB7D10D89F1BD63");

                entity.ToTable("ImageInPost");

                entity.HasOne(d => d.Image)
                    .WithMany(p => p.ImageInPosts)
                    .HasForeignKey(d => d.ImageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImageInPo__Image__5FB337D6");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.ImageInPosts)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ImageInPo__PostI__60A75C0F");
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
                    .HasConstraintName("FK__Post__AuthorId__5CD6CB2B");
            });

            modelBuilder.Entity<PostInTopic>(entity =>
            {
                entity.HasKey(e => new { e.TopicId, e.PostId })
                    .HasName("PK__PostInTo__988F295C94CE6EA5");

                entity.ToTable("PostInTopic");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.PostInTopics)
                    .HasForeignKey(d => d.PostId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostInTop__PostI__6477ECF3");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.PostInTopics)
                    .HasForeignKey(d => d.TopicId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PostInTop__Topic__6383C8BA");
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
