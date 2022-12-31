using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using App.DAL.Models;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace App.DAL.Contexts
{
    public partial class PJDesignContext : DbContext
    {
        public PJDesignContext()
        {
        }

        public PJDesignContext(DbContextOptions<PJDesignContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAdministrator> TblAdministrator { get; set; }
        public virtual DbSet<TblAdministratorGroup> TblAdministratorGroup { get; set; }
        public virtual DbSet<TblCategory> TblCategory { get; set; }
        public virtual DbSet<TblCategoryMappingAfter> TblCategoryMappingAfter { get; set; }
        public virtual DbSet<TblCategoryMappingBefore> TblCategoryMappingBefore { get; set; }
        public virtual DbSet<TblContact> TblContact { get; set; }
        public virtual DbSet<TblGroup> TblGroup { get; set; }
        public virtual DbSet<TblGroupUnitRight> TblGroupUnitRight { get; set; }
        public virtual DbSet<TblPortfolioAfter> TblPortfolioAfter { get; set; }
        public virtual DbSet<TblPortfolioBefore> TblPortfolioBefore { get; set; }
        public virtual DbSet<TblPortfolioPhotoAfter> TblPortfolioPhotoAfter { get; set; }
        public virtual DbSet<TblPortfolioPhotoBefore> TblPortfolioPhotoBefore { get; set; }
        public virtual DbSet<TblQuestionAfter> TblQuestionAfter { get; set; }
        public virtual DbSet<TblQuestionBefore> TblQuestionBefore { get; set; }
        public virtual DbSet<TblRight> TblRight { get; set; }
        public virtual DbSet<TblSettingAfter> TblSettingAfter { get; set; }
        public virtual DbSet<TblSettingBefore> TblSettingBefore { get; set; }
        public virtual DbSet<TblType1ContentAfter> TblType1ContentAfter { get; set; }
        public virtual DbSet<TblType1ContentBefore> TblType1ContentBefore { get; set; }
        public virtual DbSet<TblType2ContentAfter> TblType2ContentAfter { get; set; }
        public virtual DbSet<TblType2ContentBefore> TblType2ContentBefore { get; set; }
        public virtual DbSet<TblUnit> TblUnit { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=PJDesign;Trusted_Connection=True;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAdministrator>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CAccount).HasComment("帳號");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CLoginAttemptCount).HasComment("嘗試登入次數");

                entity.Property(e => e.CName).HasComment("名稱");

                entity.Property(e => e.CPassword).HasComment("密碼");
            });

            modelBuilder.Entity<TblAdministratorGroup>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CAdministratorId).HasComment("管理員ID");

                entity.Property(e => e.CGroupId).HasComment("管理組別ID");
            });

            modelBuilder.Entity<TblCategory>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PK_tblCode");

                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEditDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CName).HasComment("標籤/分類名稱");

                entity.Property(e => e.CUnitId).HasComment("所屬單元ID");
            });

            modelBuilder.Entity<TblCategoryMappingAfter>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCategoryId).HasComment("分類ID");

                entity.Property(e => e.CContentId).HasComment("單元內容ID");
            });

            modelBuilder.Entity<TblCategoryMappingBefore>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PK_tblCodeMapping");

                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCategoryId).HasComment("分類ID");

                entity.Property(e => e.CContentId).HasComment("單元內容ID");
            });

            modelBuilder.Entity<TblContact>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CAutoReplyDt).HasComment("自動回覆執行時間");

                entity.Property(e => e.CAutoReplyStatus).HasComment("自動回覆執行狀態 0.未處理 1.已執行 2.未完成");

                entity.Property(e => e.CContent).HasComment("聯絡內容");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEmail).HasComment("訪客信箱");

                entity.Property(e => e.CName).HasComment("訪客名稱");

                entity.Property(e => e.CPhone)
                    .IsFixedLength()
                    .HasComment("訪客電話");
            });

            modelBuilder.Entity<TblGroup>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CName).HasComment("管理員組別名稱");
            });

            modelBuilder.Entity<TblGroupUnitRight>(entity =>
            {
                entity.Property(e => e.CRightId).HasComment("");
            });

            modelBuilder.Entity<TblPortfolioAfter>(entity =>
            {
                entity.Property(e => e.CCreateDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CDate).HasComment("作品日期");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");
            });

            modelBuilder.Entity<TblPortfolioBefore>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CDate).HasComment("作品日期");

                entity.Property(e => e.CEditDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CEditStatus)
                    .HasDefaultValueSql("((2))")
                    .HasComment("1.審核中 2.駁回 3.批准");

                entity.Property(e => e.CEditorId).HasComment("編輯人員ID");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CReviewerId).HasComment("審核人員ID");

                entity.Property(e => e.CTitle).HasComment("名稱");
            });

            modelBuilder.Entity<TblPortfolioPhotoAfter>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CPortfolioId).HasComment("作品集ID");

                entity.Property(e => e.CUrl).HasComment("圖片檔案名稱");
            });

            modelBuilder.Entity<TblPortfolioPhotoBefore>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PK_tblPortfolioPhoto");

                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CPortfolioId).HasComment("作品集ID");

                entity.Property(e => e.CUrl).HasComment("圖片檔案名稱");
            });

            modelBuilder.Entity<TblQuestionAfter>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CContent).HasComment("問題編輯器內容");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CCreatorId).HasComment("創建人員ID");

                entity.Property(e => e.CEditDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最近的編輯時間");

                entity.Property(e => e.CEditorId).HasComment("最近的編輯人員ID");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CTitle).HasComment("標題");
            });

            modelBuilder.Entity<TblQuestionBefore>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CAfterId).HasComment("對應After 表的ID");

                entity.Property(e => e.CContent).HasComment("問題編輯器內容");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEditDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最近的編輯時間");

                entity.Property(e => e.CEditStatus)
                    .HasDefaultValueSql("((2))")
                    .HasComment("1.審核中 2.駁回 3.批准");

                entity.Property(e => e.CEditorId).HasComment("編輯人員ID");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CNotes).HasComment("備註");

                entity.Property(e => e.CReviewerId).HasComment("審核人員ID");

                entity.Property(e => e.CTitle).HasComment("標題");
            });

            modelBuilder.Entity<TblRight>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CName).HasComment("權限名稱");
            });

            modelBuilder.Entity<TblSettingAfter>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CContent).HasComment("單元JSON內容");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEditDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最近的編輯時間");

                entity.Property(e => e.CEditorId).HasComment("編輯人員ID");

                entity.Property(e => e.CUnitId).HasComment("單元ID");
            });

            modelBuilder.Entity<TblSettingBefore>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PK_tblSetting");

                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CContent).HasComment("單元JSON內容");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEditDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("最近的編輯時間");

                entity.Property(e => e.CEditStatus)
                    .HasDefaultValueSql("((2))")
                    .HasComment("1.審核中 2.駁回 3.批准");

                entity.Property(e => e.CEditorId).HasComment("編輯人員ID");

                entity.Property(e => e.CNotes).HasComment("備註");

                entity.Property(e => e.CReviewerId).HasComment("審核人員ID");

                entity.Property(e => e.CUnitId).HasComment("單元ID");
            });

            modelBuilder.Entity<TblType1ContentAfter>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCreateDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CEditDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CUnitId).HasComment("單元ID");
            });

            modelBuilder.Entity<TblType1ContentBefore>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CEditStatus)
                    .HasDefaultValueSql("((2))")
                    .HasComment("1.審核中 2.駁回 3.批准");

                entity.Property(e => e.CUnitId).HasComment("單元ID");
            });

            modelBuilder.Entity<TblType2ContentAfter>(entity =>
            {
                entity.Property(e => e.CCreateDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CDescription).HasComment("敘述");

                entity.Property(e => e.CEditDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CIsEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.CIsFixed).HasComment("是否置頂");
            });

            modelBuilder.Entity<TblType2ContentBefore>(entity =>
            {
                entity.HasKey(e => e.CId)
                    .HasName("PK_tblTyp2ContentBefore");

                entity.Property(e => e.CCreateDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CDescription).HasComment("敘述");

                entity.Property(e => e.CEditDt).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CIsEnabled).HasDefaultValueSql("((1))");

                entity.Property(e => e.CIsFixed).HasComment("是否置頂");
            });

            modelBuilder.Entity<TblUnit>(entity =>
            {
                entity.Property(e => e.CId).HasComment("流水號");

                entity.Property(e => e.CBackStageUrl).HasComment("後台單元路徑");

                entity.Property(e => e.CCreateDt)
                    .HasDefaultValueSql("(getdate())")
                    .HasComment("創建時間");

                entity.Property(e => e.CFrontStageUrl).HasComment("前台單元路徑");

                entity.Property(e => e.CIsAnotherWindow).HasComment("是否另開視窗");

                entity.Property(e => e.CIsEnabled)
                    .HasDefaultValueSql("((1))")
                    .HasComment("是否啟用");

                entity.Property(e => e.CName).HasComment("單元名稱");

                entity.Property(e => e.CParent).HasComment("母單元");

                entity.Property(e => e.CSort).HasComment("排序");

                entity.Property(e => e.CStageType).HasComment("0.前後台 1.僅前台 2.僅後台");

                entity.Property(e => e.CTemplateType).HasComment("模板類別: -1.固定單元 0.無 1.模板一 2.模板二");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
