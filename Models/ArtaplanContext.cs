using System;
using Artaplan.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

#nullable disable

namespace Artaplan.Models
{
    public partial class ArtaplanContext : DbContext
    {
        private AppSettings _appSettings;

        public ArtaplanContext()
        {
        }

        public ArtaplanContext(DbContextOptions<ArtaplanContext> options, IOptions<AppSettings> appSettings)
       : base(options)
        {
            _appSettings = appSettings.Value;

        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Job> Jobs { get; set; }
        public virtual DbSet<JobStage> JobStages { get; set; }
        public virtual DbSet<ScheduleEntry> ScheduleEntries { get; set; }
        public virtual DbSet<Slot> Slots { get; set; }
        public virtual DbSet<Stage> Stages { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_appSettings.ConnectionString);

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(e => e.UserId, "fkIdx_92");

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Customers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_92");
            });

            modelBuilder.Entity<Job>(entity =>
            {
                entity.HasIndex(e => e.SlotId, "fkIdx_43");

                entity.HasIndex(e => e.UserId, "fkIdx_46");

                entity.HasIndex(e => e.CustomerId, "fkIdx_82");

                entity.Property(e => e.Color)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasDefaultValueSql("(getdate())")
                    .IsFixedLength(true);

                entity.Property(e => e.CustomerId).HasColumnName("CustomerID");

                entity.Property(e => e.InsertedAt).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.StartedAt).HasColumnType("date");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ToStartAt).HasColumnType("date");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_82");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_43");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Jobs)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_46");
            });

            modelBuilder.Entity<JobStage>(entity =>
            {
                entity.HasIndex(e => e.JobId, "fkIdx_71");

                entity.HasIndex(e => e.StageId, "fkIdx_74");

                entity.Property(e => e.Deadline).HasColumnType("datetime");

                entity.HasOne(d => d.Job)
                    .WithMany(p => p.JobStages)
                    .HasForeignKey(d => d.JobId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_71");

                entity.HasOne(d => d.Stage)
                    .WithMany(p => p.JobStages)
                    .HasForeignKey(d => d.StageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_74");
            });

            modelBuilder.Entity<ScheduleEntry>(entity =>
            {
                entity.HasKey(e => e.ScheduleEntriesId)
                    .HasName("PK_schedule");

                entity.HasIndex(e => e.JobStageId, "fkIdx_102");

                entity.Property(e => e.ScheduleEntriesId)
                    .ValueGeneratedNever()
                    .HasColumnName("scheduleEntriesID");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");

                entity.Property(e => e.DateTo).HasColumnType("datetime");

                entity.HasOne(d => d.JobStage)
                    .WithMany(p => p.ScheduleEntries)
                    .HasForeignKey(d => d.JobStageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_102");
            });

            modelBuilder.Entity<Slot>(entity =>
            {
                entity.HasIndex(e => e.UserId, "fkIdx_20");

                entity.Property(e => e.Description)
                    .HasMaxLength(800)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80)
                    .IsUnicode(false);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Slots)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_20");
            });

            modelBuilder.Entity<Stage>(entity =>
            {
                entity.HasIndex(e => e.UserId, "fkIdx_57");

                entity.HasIndex(e => e.SlotId, "fkIdx_60");

                entity.Property(e => e.Description).HasMaxLength(800);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(80);

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Slot)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.SlotId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_60");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Stages)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_57");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .IsUnicode(false);

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
