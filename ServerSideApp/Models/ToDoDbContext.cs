using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ServerSideApp.Models;

public partial class ToDoDbContext : DbContext
{
    public ToDoDbContext()
    {
    }

    public ToDoDbContext(DbContextOptions<ToDoDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cpr> Cprs { get; set; }

    public virtual DbSet<Todolist> Todolists { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=LAPTOP-GNCUVPL0\\MSSQLSERVER2022;Database=ToDoDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cpr>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Cpr__3214EC0798AA6108");

            entity.ToTable("Cpr");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CprNr).HasMaxLength(200);
            entity.Property(e => e.User).HasMaxLength(450);
        });

        modelBuilder.Entity<Todolist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__todolist__3214EC07660DD177");

            entity.ToTable("todolist");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Task).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.Todolists)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_todolist_Cpr");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
