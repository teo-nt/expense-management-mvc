using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ExpenseManagementMVC.Models;

public partial class ExpensesDbContext : DbContext
{
    public ExpensesDbContext()
    {
    }

    public ExpensesDbContext(DbContextOptions<ExpensesDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__EXPENSES__ID");

            entity.ToTable("EXPENSES");

            entity.HasIndex(e => e.UserId, "IX_USER_ID");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Amount)
                .HasColumnType("numeric(12, 2)")
                .HasColumnName("AMOUNT");
            entity.Property(e => e.Category)
                .HasMaxLength(20)
                .HasColumnName("CATEGORY");
            entity.Property(e => e.Date).HasColumnName("DATE");
            entity.Property(e => e.Name)
                .HasMaxLength(40)
                .HasColumnName("NAME");
            entity.Property(e => e.UserId).HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_USERS");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("USERS");

            entity.HasIndex(e => e.Username, "IX_USERNAME").IsUnique();

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Firstname)
                .HasMaxLength(50)
                .HasColumnName("FIRSTNAME");
            entity.Property(e => e.Lastname)
                .HasMaxLength(50)
                .HasColumnName("LASTNAME");
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("USERNAME");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
