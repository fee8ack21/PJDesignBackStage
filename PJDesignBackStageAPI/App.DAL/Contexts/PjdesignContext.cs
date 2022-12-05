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

    public virtual DbSet<TblCategoryMappingAfter> TblCategoryMappingAfters { get; set; }

    public virtual DbSet<TblCategoryMappingBefore> TblCategoryMappingBefores { get; set; }

    public virtual DbSet<TblContact> TblContacts { get; set; }

    public virtual DbSet<TblGroup> TblGroups { get; set; }

    public virtual DbSet<TblGroupUnitRight> TblGroupUnitRights { get; set; }

    public virtual DbSet<TblPortfolioAfter> TblPortfolioAfters { get; set; }

    public virtual DbSet<TblPortfolioBefore> TblPortfolioBefores { get; set; }

    public virtual DbSet<TblPortfolioPhotoAfter> TblPortfolioPhotoAfters { get; set; }

    public virtual DbSet<TblPortfolioPhotoBefore> TblPortfolioPhotoBefores { get; set; }

    public virtual DbSet<TblQuestionAfter> TblQuestionAfters { get; set; }

    public virtual DbSet<TblQuestionBefore> TblQuestionBefores { get; set; }

    public virtual DbSet<TblRight> TblRights { get; set; }

    public virtual DbSet<TblSettingAfter> TblSettingAfters { get; set; }

    public virtual DbSet<TblSettingBefore> TblSettingBefores { get; set; }

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
            entity.Property(e => e.CLoginAttemptCount)
                .HasComment("嘗試登入次數")
                .HasColumnName("cLoginAttemptCount");
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
                .HasComment("管理員ID")
                .HasColumnName("cAdministratorId");
            entity.Property(e => e.CGroupId)
                .HasComment("管理組別ID")
                .HasColumnName("cGroupId");
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
            entity.Property(e => e.CEditDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("標籤/分類名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CUnitId)
                .HasComment("所屬單元ID")
                .HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblCategoryMappingAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblCategoryMappingAfter");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCategoryId)
                .HasComment("分類ID")
                .HasColumnName("cCategoryId");
            entity.Property(e => e.CContentId)
                .HasComment("單元內容ID")
                .HasColumnName("cContentId");
        });

        modelBuilder.Entity<TblCategoryMappingBefore>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblCodeMapping");

            entity.ToTable("tblCategoryMappingBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CCategoryId)
                .HasComment("分類ID")
                .HasColumnName("cCategoryId");
            entity.Property(e => e.CContentId)
                .HasComment("單元內容ID")
                .HasColumnName("cContentId");
        });

        modelBuilder.Entity<TblContact>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblContact");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CAutoReplyDt)
                .HasComment("自動回覆執行時間")
                .HasColumnType("datetime")
                .HasColumnName("cAutoReplyDt");
            entity.Property(e => e.CAutoReplyStatus)
                .HasComment("自動回覆執行狀態 0.未處理 1.已執行 2.未完成")
                .HasColumnName("cAutoReplyStatus");
            entity.Property(e => e.CContent)
                .HasMaxLength(200)
                .HasComment("聯絡內容")
                .HasColumnName("cContent");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CEmail)
                .HasMaxLength(200)
                .HasComment("訪客信箱")
                .HasColumnName("cEmail");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("訪客名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CPhone)
                .HasMaxLength(200)
                .IsFixedLength()
                .HasComment("訪客電話")
                .HasColumnName("cPhone");
        });

        modelBuilder.Entity<TblGroup>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblGroup");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("管理員組別名稱")
                .HasColumnName("cName");
        });

        modelBuilder.Entity<TblGroupUnitRight>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblGroupUnitRight");

            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CGroupId).HasColumnName("cGroupId");
            entity.Property(e => e.CRightId)
                .HasComment("")
                .HasColumnName("cRightId");
            entity.Property(e => e.CUnitId).HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblPortfolioAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioAfter");

            entity.Property(e => e.CId).HasColumnName("cId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CDate)
                .HasColumnType("date")
                .HasColumnName("cDate");
            entity.Property(e => e.CEditDt)
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasColumnName("cName");
            entity.Property(e => e.CStatus)
                .HasDefaultValueSql("((1))")
                .HasComment("0.停用 1.啟用 2.審核中 3.駁回")
                .HasColumnName("cStatus");
        });

        modelBuilder.Entity<TblPortfolioBefore>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CAfterId).HasColumnName("cAfterId");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CDate)
                .HasComment("作品日期")
                .HasColumnType("date")
                .HasColumnName("cDate");
            entity.Property(e => e.CEditAdministratorId)
                .HasComment("編輯人員ID")
                .HasColumnName("cEditAdministratorId");
            entity.Property(e => e.CEditDt)
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("名稱")
                .HasColumnName("cName");
            entity.Property(e => e.CNote)
                .HasMaxLength(200)
                .HasColumnName("cNote");
            entity.Property(e => e.CReviewAdministratorId)
                .HasComment("審核人員ID")
                .HasColumnName("cReviewAdministratorId");
            entity.Property(e => e.CStatus)
                .HasComment("0.停用 1.啟用 2.審核中 3.駁回")
                .HasColumnName("cStatus");
        });

        modelBuilder.Entity<TblPortfolioPhotoAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblPortfolioPhotoAfter");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CContentHash)
                .HasMaxLength(200)
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cContentHash");
            entity.Property(e => e.CFilename)
                .HasMaxLength(200)
                .HasComment("圖片檔案名稱")
                .HasColumnName("cFilename");
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
            entity.Property(e => e.CContentHash)
                .HasMaxLength(200)
                .HasColumnName("cContentHash");
            entity.Property(e => e.CFilename)
                .HasMaxLength(200)
                .HasComment("圖片檔案名稱")
                .HasColumnName("cFilename");
            entity.Property(e => e.CPath)
                .HasMaxLength(200)
                .HasComment("圖片檔案路徑")
                .HasColumnName("cPath");
            entity.Property(e => e.CPortfolioId)
                .HasComment("作品集ID")
                .HasColumnName("cPortfolioId");
        });

        modelBuilder.Entity<TblQuestionAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblQuestionAfter");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CContent)
                .HasComment("問題編輯器內容")
                .HasColumnName("cContent");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CCreatorId)
                .HasComment("創建人員ID")
                .HasColumnName("cCreatorId");
            entity.Property(e => e.CEditDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("最近的編輯時間")
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CEditorId)
                .HasComment("最近的編輯人員ID")
                .HasColumnName("cEditorId");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CTitle)
                .HasMaxLength(50)
                .HasComment("標題")
                .HasColumnName("cTitle");
        });

        modelBuilder.Entity<TblQuestionBefore>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblQuestionBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CAfterId)
                .HasComment("對應After 表的ID")
                .HasColumnName("cAfterId");
            entity.Property(e => e.CContent)
                .HasComment("問題編輯器內容")
                .HasColumnName("cContent");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CEditDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("最近的編輯時間")
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CEditStatus)
                .HasDefaultValueSql("((2))")
                .HasComment("1.審核中 2.駁回 3.批准")
                .HasColumnName("cEditStatus");
            entity.Property(e => e.CEditorId)
                .HasComment("編輯人員ID")
                .HasColumnName("cEditorId");
            entity.Property(e => e.CIsEnabled)
                .IsRequired()
                .HasDefaultValueSql("((1))")
                .HasComment("是否啟用")
                .HasColumnName("cIsEnabled");
            entity.Property(e => e.CNotes)
                .HasComment("備註")
                .HasColumnName("cNotes");
            entity.Property(e => e.CReviewerId)
                .HasComment("審核人員ID")
                .HasColumnName("cReviewerId");
            entity.Property(e => e.CTitle)
                .HasMaxLength(50)
                .HasComment("標題")
                .HasColumnName("cTitle");
        });

        modelBuilder.Entity<TblRight>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblRight");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CName)
                .HasMaxLength(50)
                .HasComment("權限名稱")
                .HasColumnName("cName");
        });

        modelBuilder.Entity<TblSettingAfter>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblSettingAfter");

            entity.Property(e => e.CId)
                .ValueGeneratedNever()
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CContent)
                .HasComment("單元JSON內容")
                .HasColumnName("cContent");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CEditDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("最近的編輯時間")
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CEditorId)
                .HasComment("編輯人員ID")
                .HasColumnName("cEditorId");
            entity.Property(e => e.CUnitId)
                .HasComment("單元ID")
                .HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblSettingBefore>(entity =>
        {
            entity.HasKey(e => e.CId).HasName("PK_tblSetting");

            entity.ToTable("tblSettingBefore");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CContent)
                .HasComment("單元JSON內容")
                .HasColumnName("cContent");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CEditDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("最近的編輯時間")
                .HasColumnType("datetime")
                .HasColumnName("cEditDt");
            entity.Property(e => e.CEditStatus)
                .HasDefaultValueSql("((2))")
                .HasComment("1.審核中 2.駁回 3.批准")
                .HasColumnName("cEditStatus");
            entity.Property(e => e.CEditorId)
                .HasComment("編輯人員ID")
                .HasColumnName("cEditorId");
            entity.Property(e => e.CNotes)
                .HasComment("備註")
                .HasColumnName("cNotes");
            entity.Property(e => e.CReviewerId)
                .HasComment("審核人員ID")
                .HasColumnName("cReviewerId");
            entity.Property(e => e.CUnitId)
                .HasComment("單元ID")
                .HasColumnName("cUnitId");
        });

        modelBuilder.Entity<TblUnit>(entity =>
        {
            entity.HasKey(e => e.CId);

            entity.ToTable("tblUnit");

            entity.Property(e => e.CId)
                .HasComment("流水號")
                .HasColumnName("cId");
            entity.Property(e => e.CBackStageUrl)
                .HasMaxLength(200)
                .HasComment("後台單元路徑")
                .HasColumnName("cBackStageUrl");
            entity.Property(e => e.CCreateDt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("創建時間")
                .HasColumnType("datetime")
                .HasColumnName("cCreateDt");
            entity.Property(e => e.CFrontStageUrl)
                .HasMaxLength(200)
                .HasComment("前台單元路徑")
                .HasColumnName("cFrontStageUrl");
            entity.Property(e => e.CIsAnotherWindow)
                .HasComment("是否另開視窗")
                .HasColumnName("cIsAnotherWindow");
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
            entity.Property(e => e.CSort)
                .HasComment("排序")
                .HasColumnName("cSort");
            entity.Property(e => e.CStageType)
                .HasDefaultValueSql("((2))")
                .HasComment("0.前後台 1.僅前台 2.僅後台")
                .HasColumnName("cStageType");
            entity.Property(e => e.CTemplateType)
                .HasComment("模板類別: -1.固定單元 0.無 1.模板一 2.模板二")
                .HasColumnName("cTemplateType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
