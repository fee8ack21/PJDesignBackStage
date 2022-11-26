using System;
using System.Collections.Generic;
using App.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.DbContexts;

public partial class PjdesignContext : DbContext
{
    public PjdesignContext()
    {
    }

    public PjdesignContext(DbContextOptions<PjdesignContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TblAdministrator> TblAdministrators { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAdministrator>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblAdministrator");

            entity.Property(e => e.CAccount)
                .HasMaxLength(50)
                .HasComment("帳號")
                .HasColumnName("cAccount");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CId)
                .ValueGeneratedOnAdd()
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CLevel)
                .HasComment("權限等級 1:系統管理員 2:一般用戶")
                .HasColumnName("cLevel");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CPassword)
                .HasMaxLength(50)
                .HasComment("密碼")
                .HasColumnName("cPassword");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
