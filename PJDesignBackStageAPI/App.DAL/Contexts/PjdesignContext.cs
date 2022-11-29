using System;
using System.Collections.Generic;
using App.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace App.DAL.Contexts;

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

    public virtual DbSet<TblAdministratorGroup> TblAdministratorGroups { get; set; }

    public virtual DbSet<TblCategory> TblCategories { get; set; }

    public virtual DbSet<TblCategoryMapping> TblCategoryMappings { get; set; }

    public virtual DbSet<TblGroup> TblGroups { get; set; }

    public virtual DbSet<TblGroupUnit> TblGroupUnits { get; set; }

    public virtual DbSet<TblPortfolioAfter> TblPortfolioAfters { get; set; }

    public virtual DbSet<TblPortfolioBefore> TblPortfolioBefores { get; set; }

    public virtual DbSet<TblPortfolioPhotoAfter> TblPortfolioPhotoAfters { get; set; }

    public virtual DbSet<TblPortfolioPhotoBefore> TblPortfolioPhotoBefores { get; set; }

    public virtual DbSet<TblUnit> TblUnits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PJDesign;Trusted_Connection=True;Integrated Security=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblAdministrator>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblAdministrator");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CAccount)
                .HasMaxLength(50)
                .HasComment("帳號")
                .HasColumnName("cAccount");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CPassword)
                .HasMaxLength(50)
                .HasComment("密碼")
                .HasColumnName("cPassword");
        });

        modelBuilder.Entity<TblAdministratorGroup>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblAdministratorGroup");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CAdministratorId)
                .HasComment("管理員組別名稱")
                .HasColumnName("cAdministratorId");
            entity.Property(e => e.CGroupId).HasColumnName("cGroupId");
        });

        modelBuilder.Entity<TblCategory>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblCode");

            entity.ToTable("tblCategory");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("標籤/分類名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CUnitId)
                .HasComment("所屬單元")
                .HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblCategoryMapping>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblCodeMapping");

            entity.ToTable("tblCategoryMapping");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCodeId)
                .HasComment("CodeID")
                .HasColumnName("cCodeId");
            entity.Property(e => e.CTemplateId)
                .HasComment("單元內容ID")
                .HasColumnName("cTemplateId");
            entity.Property(e => e.CUnitId)
                .HasComment("單元ID")
                .HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblGroup>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("tblGroup");

            entity.Property(e => e.CId)
                .ValueGeneratedOnAdd()
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("管理員組別名稱")
                .HasColumnName("cName");
        });

        modelBuilder.Entity<TblGroupUnit>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblGroupUnitRight");

            entity.ToTable("tblGroupUnit");

            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CGroupId).HasColumnName("cGroupId");
            entity.Property(e => e.CRightType)
                .HasComment("操作權限: 0.CRUD")
                .HasColumnName("cRightType");
            entity.Property(e => e.CUnitId).HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblPortfolioAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioAfter");

            entity.Property(e => e.CId)
                .ValueGeneratedNever()
                .HasColumnName("cId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CDate)
                .HasColumnType("date")
                .HasColumnName("cDate");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasColumnName("cName");
        });

        modelBuilder.Entity<TblPortfolioBefore>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CDate)
                .HasComment("作品日期")
                .HasColumnType("date")
                .HasColumnName("cDate");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CNote)
                .HasMaxLength(200)
                .HasColumnName("cNote");
        });

        modelBuilder.Entity<TblPortfolioPhotoAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioPhotoAfter");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .HasComment("圖片檔案名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CPath)
                .HasMaxLength(200)
                .HasComment("圖片檔案路徑")
                .HasColumnName("cPath");
            entity.Property(e => e.CPortfolioId)
                .HasComment("作品集ID")
                .HasColumnName("cPortfolioId");
        });

        modelBuilder.Entity<TblPortfolioPhotoBefore>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblPortfolioPhoto");

            entity.ToTable("tblPortfolioPhotoBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CName)
                .HasMaxLength(200)
                .HasComment("圖片檔案名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CPath)
                .HasMaxLength(200)
                .HasComment("圖片檔案路徑")
                .HasColumnName("cPath");
            entity.Property(e => e.CPortfolioId)
                .HasComment("作品集ID")
                .HasColumnName("cPortfolioId");
        });

        modelBuilder.Entity<TblUnit>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblUnit");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CIsAnotherWindow)
                .HasComment("是否另開視窗")
                .HasColumnName("cIsAnotherWindow");
            entity.Property(e => e.CIsBackStage)
                .HasComment("是否為後台單元")
                .HasColumnName("cIsBackStage");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("單元名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CParent)
                .HasComment("母單元")
                .HasColumnName("cParent");
            entity.Property(e => e.CSettings)
                .HasComment("單元半結構化資料")
                .HasColumnName("cSettings");
            entity.Property(e => e.CType)
                .HasComment("模板類別: -1.固定單元 0.無 1.模板一 2.模板二")
                .HasColumnName("cType");
            entity.Property(e => e.CUrl)
                .HasMaxLength(50)
                .HasComment("單元路徑")
                .HasColumnName("cUrl");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
