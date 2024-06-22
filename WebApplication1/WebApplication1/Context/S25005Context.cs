using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using Task = WebApplication1.Models.Task;

namespace WebApplication1.Context;

public partial class S25005Context : DbContext
{
    public S25005Context()
    {
    }

    public S25005Context(DbContextOptions<S25005Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Access> Accesses { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Task> Tasks { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db-mssql16;Database=s25005;Trusted_Connection=True;Encrypt=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Access>(entity =>
        {
            entity.HasKey(e => new { e.IdUser, e.IdProject }).HasName("Access_pk");

            entity.ToTable("Access");

            entity.Property(e => e.ColumnName).HasColumnName("column_name");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Accesses)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProjectAccess_Project");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Accesses)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("ProjectAccess_User");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.IdProject).HasName("Project_pk");

            entity.ToTable("Project");

            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdDefaultAssigneeNavigation).WithMany(p => p.Projects)
                .HasForeignKey(d => d.IdDefaultAssignee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Project_User");
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => e.IdTask).HasName("Task_pk");

            entity.ToTable("Task");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);

            entity.HasOne(d => d.IdAssigneeNavigation).WithMany(p => p.TaskIdAssigneeNavigations)
                .HasForeignKey(d => d.IdAssignee)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_User_Assignee");

            entity.HasOne(d => d.IdProjectNavigation).WithMany(p => p.Tasks)
                .HasForeignKey(d => d.IdProject)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_Project");

            entity.HasOne(d => d.IdReporterNavigation).WithMany(p => p.TaskIdReporterNavigations)
                .HasForeignKey(d => d.IdReporter)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Task_User_Reporter");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("User_pk");

            entity.ToTable("User");

            entity.Property(e => e.FirstName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(200)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
