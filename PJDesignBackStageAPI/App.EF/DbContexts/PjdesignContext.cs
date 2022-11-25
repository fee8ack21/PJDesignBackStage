using System;
using System.Collections.Generic;
using App.EF.Models;
using Microsoft.EntityFrameworkCore;

namespace App.EF.DbContexts;

public partial class PjdesignContext : DbContext
{
    public PjdesignContext()
    {
    }

    public PjdesignContext(DbContextOptions<PjdesignContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblEftest> TblEftests { get; set; }

    public virtual DbSet<TblEftest2> TblEftest2s { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PJDesign;Trusted_Connection=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblEftest>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblEFTest");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<TblEftest2>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblEFTest2");

            entity.Property(e => e.Id)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name");
            entity.Property(e => e.Name2)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("name2");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
