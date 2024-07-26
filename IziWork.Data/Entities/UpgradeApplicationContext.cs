using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IziWork.Data.Entities;

public partial class UpgradeApplicationContext : DbContext
{
    public UpgradeApplicationContext()
    {
    }

    public UpgradeApplicationContext(DbContextOptions<UpgradeApplicationContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountingBalanceSheet> AccountingBalanceSheets { get; set; }

    public virtual DbSet<AccountingBalanceSheetDetail> AccountingBalanceSheetDetails { get; set; }

    public virtual DbSet<AccountingBalanceSheetTemplate> AccountingBalanceSheetTemplates { get; set; }

    public virtual DbSet<AccountingBalanceSheetTemplateDetail> AccountingBalanceSheetTemplateDetails { get; set; }

    public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }

    public virtual DbSet<BadDebt> BadDebts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CategoryDetail> CategoryDetails { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<CompanyInfo> CompanyInfos { get; set; }

    public virtual DbSet<CustomerReceivable> CustomerReceivables { get; set; }

    public virtual DbSet<DebtLedger> DebtLedgers { get; set; }

    public virtual DbSet<DebtLedgerDetail> DebtLedgerDetails { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<DeptManagement> DeptManagements { get; set; }

    public virtual DbSet<Discussion> Discussions { get; set; }

    public virtual DbSet<Document> Documents { get; set; }

    public virtual DbSet<DocumentAttachmentMapping> DocumentAttachmentMappings { get; set; }

    public virtual DbSet<DocumentDiscussionMapping> DocumentDiscussionMappings { get; set; }

    public virtual DbSet<DocumentForwarding> DocumentForwardings { get; set; }

    public virtual DbSet<DocumentHistory> DocumentHistories { get; set; }

    public virtual DbSet<DocumentProfileMapping> DocumentProfileMappings { get; set; }

    public virtual DbSet<EquityInvestment> EquityInvestments { get; set; }

    public virtual DbSet<Explanation> Explanations { get; set; }

    public virtual DbSet<ExplanationAddionalInformationTradingSecurity> ExplanationAddionalInformationTradingSecurities { get; set; }

    public virtual DbSet<ExplanationAdditionalInformationMoney> ExplanationAdditionalInformationMoneys { get; set; }

    public virtual DbSet<ExplanationDetail> ExplanationDetails { get; set; }

    public virtual DbSet<FinancialAccount> FinancialAccounts { get; set; }

    public virtual DbSet<GeneralJournal> GeneralJournals { get; set; }

    public virtual DbSet<GeneralJournalDetail> GeneralJournalDetails { get; set; }

    public virtual DbSet<HeldToMaturityInvestment> HeldToMaturityInvestments { get; set; }

    public virtual DbSet<IncomeStatement> IncomeStatements { get; set; }

    public virtual DbSet<Menu> Menus { get; set; }

    public virtual DbSet<MenuDepartmentMapping> MenuDepartmentMappings { get; set; }

    public virtual DbSet<MenuRoleMapping> MenuRoleMappings { get; set; }

    public virtual DbSet<MenuUserMapping> MenuUserMappings { get; set; }

    public virtual DbSet<MetadataItem> MetadataItems { get; set; }

    public virtual DbSet<MetadataType> MetadataTypes { get; set; }

    public virtual DbSet<MissingAsset> MissingAssets { get; set; }

    public virtual DbSet<Navigation> Navigations { get; set; }

    public virtual DbSet<OtherReceivable> OtherReceivables { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<Profile> Profiles { get; set; }

    public virtual DbSet<ProfileAttachmentFileMapping> ProfileAttachmentFileMappings { get; set; }

    public virtual DbSet<ProvinceMasterDatum> ProvinceMasterData { get; set; }

    public virtual DbSet<ReceivingDepartmentDocument> ReceivingDepartmentDocuments { get; set; }

    public virtual DbSet<ReferenceDocument> ReferenceDocuments { get; set; }

    public virtual DbSet<ReferenceNumber> ReferenceNumbers { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<TaskAttachmentMapping> TaskAttachmentMappings { get; set; }

    public virtual DbSet<TaskDepartmentMapping> TaskDepartmentMappings { get; set; }

    public virtual DbSet<TaskExtend> TaskExtends { get; set; }

    public virtual DbSet<TaskExtendAttachmentMapping> TaskExtendAttachmentMappings { get; set; }

    public virtual DbSet<TaskManagement> TaskManagements { get; set; }

    public virtual DbSet<TaskManagementHistory> TaskManagementHistories { get; set; }

    public virtual DbSet<TrackingLog> TrackingLogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserDepartmentMapping> UserDepartmentMappings { get; set; }

    public virtual DbSet<UserDepartmentRoleMapping> UserDepartmentRoleMappings { get; set; }

    public virtual DbSet<WardMasterDatum> WardMasterData { get; set; }

    public virtual DbSet<WorkflowInstance> WorkflowInstances { get; set; }

    public virtual DbSet<WorkflowProcessing> WorkflowProcessings { get; set; }

    public virtual DbSet<WorkflowRole> WorkflowRoles { get; set; }

    public virtual DbSet<WorkflowStep> WorkflowSteps { get; set; }

    public virtual DbSet<WorkflowTemplate> WorkflowTemplates { get; set; }

    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("data source=192.168.1.50;initial catalog=UpgradeApplication;persist security info=True;user id=sa;password=Net$1234;MultipleActiveResultSets=True;encrypt=false");
*/
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountingBalanceSheet>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Bảng cân đối kế toán"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.CurrencyUnit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.TemplateNo)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.AccountingBalanceSheetTemplate).WithMany(p => p.AccountingBalanceSheets)
                .HasForeignKey(d => d.AccountingBalanceSheetTemplateId)
                .HasConstraintName("FK_AccountingBalanceSheets_AccountingBalanceSheetTemplates");
        });

        modelBuilder.Entity<AccountingBalanceSheetDetail>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Bảng cân đối kế toán chi tiết"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Asset).IsUnicode(false);
            entity.Property(e => e.CodeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.IndexName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.AccountingBalanceSheetDetailParent).WithMany(p => p.InverseAccountingBalanceSheetDetailParent)
                .HasForeignKey(d => d.AccountingBalanceSheetDetailParentId)
                .HasConstraintName("FK_AccountingBalanceSheetDetails_AccountingBalanceSheetDetails");

            entity.HasOne(d => d.AccountingBalanceSheet).WithMany(p => p.AccountingBalanceSheetDetails)
                .HasForeignKey(d => d.AccountingBalanceSheetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountingBalanceSheetDetails_AccountingBalanceSheets");

            entity.HasOne(d => d.Code).WithMany(p => p.AccountingBalanceSheetDetails)
                .HasForeignKey(d => d.CodeId)
                .HasConstraintName("FK_AccountingBalanceSheetDetails_Codes");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.AccountingBalanceSheetDetails)
                .HasForeignKey(d => d.ExplanationDetailId)
                .HasConstraintName("FK_AccountingBalanceSheetDetails_ExplanationDetails");
        });

        modelBuilder.Entity<AccountingBalanceSheetTemplate>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.CurrencyUnit)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
        });

        modelBuilder.Entity<AccountingBalanceSheetTemplateDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Asset).IsUnicode(false);
            entity.Property(e => e.CodeName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ExplanationCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.IndexName)
                .HasMaxLength(30)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.AccountingBalanceSheetTemplate).WithMany(p => p.AccountingBalanceSheetTemplateDetails)
                .HasForeignKey(d => d.AccountingBalanceSheetTemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AccountingBalanceSheetTemplateDetails_AccountingBalanceSheetTemplates");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_AccountingBalanceSheetTemplateDetails_AccountingBalanceSheetTemplateDetails");
        });

        modelBuilder.Entity<AttachmentFile>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Extension).HasMaxLength(50);
            entity.Property(e => e.FileDisplayName).HasMaxLength(255);
            entity.Property(e => e.FileName).HasMaxLength(50);
            entity.Property(e => e.FileUniqueName).HasMaxLength(255);
            entity.Property(e => e.LinkDownload).HasMaxLength(1000);
            entity.Property(e => e.NameEng)
                .HasMaxLength(250)
                .HasColumnName("NameENG");
            entity.Property(e => e.Type).HasMaxLength(255);
        });

        modelBuilder.Entity<BadDebt>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Nợ xấu"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.IndexName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RecoverableValue).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.BadDebtParent).WithMany(p => p.InverseBadDebtParent)
                .HasForeignKey(d => d.BadDebtParentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BadDebts_BadDebts");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.BadDebts)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BadDebts_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.BadDebts)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_BadDebts_FinancialAccounts");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Module)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<CategoryDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);

            entity.HasOne(d => d.Category).WithMany(p => p.CategoryDetails)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CategoryDetails_Categories");
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Mã số"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Code");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CompanyInfo>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountantName).HasMaxLength(100);
            entity.Property(e => e.AccountingMethod).HasMaxLength(100);
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BusinessSector).HasMaxLength(50);
            entity.Property(e => e.Ceoid).HasColumnName("CEOId");
            entity.Property(e => e.Ceoname)
                .HasMaxLength(100)
                .HasColumnName("CEOName");
            entity.Property(e => e.CompanyName).IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.OwnershipForm).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PositionName).HasMaxLength(100);
            entity.Property(e => e.PreparedByName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.RegulatoryAgency).HasMaxLength(100);
            entity.Property(e => e.ReportingDateInWords)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ReportingPeriod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReportingPeriodAbbreviation).HasMaxLength(100);
            entity.Property(e => e.TaxNo)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Province).WithMany(p => p.CompanyInfos)
                .HasForeignKey(d => d.ProvinceId)
                .HasConstraintName("FK_CompanyInfos_ProvinceMasterData");

            entity.HasOne(d => d.Ward).WithMany(p => p.CompanyInfos)
                .HasForeignKey(d => d.WardId)
                .HasConstraintName("FK_CompanyInfos_WardMasterData");
        });

        modelBuilder.Entity<CustomerReceivable>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Phải thu của khách hàng"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYear).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYear).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CustomerReceivableParent).WithMany(p => p.InverseCustomerReceivableParent)
                .HasForeignKey(d => d.CustomerReceivableParentId)
                .HasConstraintName("FK_CustomerReceivables_CustomerReceivables");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.CustomerReceivables)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CustomerReceivables_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.CustomerReceivables)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_CustomerReceivables_FinancialAccounts");
        });

        modelBuilder.Entity<DebtLedger>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Công nợ"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.TemplateNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Year)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.CompanyInfo).WithMany(p => p.DebtLedgers)
                .HasForeignKey(d => d.CompanyInfoId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DebtLedgers_CompanyInfos");
        });

        modelBuilder.Entity<DebtLedgerDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.Dt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasComment("Đối tượng")
                .HasColumnName("DT");
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.NameDt).HasColumnName("NameDT");

            entity.HasOne(d => d.DebtLedger).WithMany(p => p.DebtLedgerDetails)
                .HasForeignKey(d => d.DebtLedgerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DebtLedgerDetails_DebtLedgers");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Departments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(500);

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_dbo.Departments_dbo.Departments_ParentId");

            entity.HasOne(d => d.Profile).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ProfileId)
                .HasConstraintName("FK_Departments_Profiles");
        });

        modelBuilder.Entity<DeptManagement>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Công nợ"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Dt)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("DT");
            entity.Property(e => e.NameDt).HasColumnName("NameDT");
        });

        modelBuilder.Entity<Discussion>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.ParentDiscussion).WithMany(p => p.InverseParentDiscussion)
                .HasForeignKey(d => d.ParentDiscussionId)
                .HasConstraintName("FK_Discussions_Discussions1");
        });

        modelBuilder.Entity<Document>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ArrivalNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RegistryId).HasColumnName("RegistryID");
            entity.Property(e => e.SendingDepartmentCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SendingDepartmentName).HasMaxLength(255);
            entity.Property(e => e.SentBy)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.DocumentType).WithMany(p => p.DocumentDocumentTypes)
                .HasForeignKey(d => d.DocumentTypeId)
                .HasConstraintName("FK_Documents_CategoryDetails1");

            entity.HasOne(d => d.Registry).WithMany(p => p.DocumentRegistries)
                .HasForeignKey(d => d.RegistryId)
                .HasConstraintName("FK_Documents_CategoryDetails");

            entity.HasOne(d => d.SecurityLevel).WithMany(p => p.DocumentSecurityLevels)
                .HasForeignKey(d => d.SecurityLevelId)
                .HasConstraintName("FK_Documents_CategoryDetails2");

            entity.HasOne(d => d.UrgencyLevel).WithMany(p => p.DocumentUrgencyLevels)
                .HasForeignKey(d => d.UrgencyLevelId)
                .HasConstraintName("FK_Documents_CategoryDetails3");
        });

        modelBuilder.Entity<DocumentAttachmentMapping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.AttachmentFile).WithMany(p => p.DocumentAttachmentMappings)
                .HasForeignKey(d => d.AttachmentFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentAttachmentMappings_AttachmentFiles");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentAttachmentMappings)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentAttachmentMappings_Documents");
        });

        modelBuilder.Entity<DocumentDiscussionMapping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Discussion).WithMany(p => p.DocumentDiscussionMappings)
                .HasForeignKey(d => d.DiscussionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDiscussionMappings_Discussions1");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentDiscussionMappings)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentDiscussionMappings_Documents1");
        });

        modelBuilder.Entity<DocumentForwarding>(entity =>
        {
            entity.ToTable("DocumentForwarding");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.ProcessorCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProcessorName).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.DocumentForwardings)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_DocumentForwarding_Departments");

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentForwardings)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentForwarding_Documents");

            entity.HasOne(d => d.Processor).WithMany(p => p.DocumentForwardings)
                .HasForeignKey(d => d.ProcessorId)
                .HasConstraintName("FK_DocumentForwarding_Users");
        });

        modelBuilder.Entity<DocumentHistory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.DepartmentCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DepartmentName).HasMaxLength(255);
            entity.Property(e => e.ProcessorCode)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ProcessorName).HasMaxLength(255);

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentHistories)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentHistories_Documents");
        });

        modelBuilder.Entity<DocumentProfileMapping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Document).WithMany(p => p.DocumentProfileMappings)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProfileMappings_Documents");

            entity.HasOne(d => d.Profile).WithMany(p => p.DocumentProfileMappings)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DocumentProfileMappings_Profiles");
        });

        modelBuilder.Entity<EquityInvestment>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Đầu tư góp vốn vào đơn vị khác (chi tiết theo từng khoản đầu tư theo tỷ lệ vốn nắm giữ và tỷ lệ quyền biểu quyết)"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYearFairValue)
                .HasComment("Giá trị hợp lý")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYearFairValue)
                .HasComment("Giá trị hợp lý")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.EquityInvestmentParent).WithMany(p => p.InverseEquityInvestmentParent)
                .HasForeignKey(d => d.EquityInvestmentParentId)
                .HasConstraintName("FK_EquityInvestments_EquityInvestments");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.EquityInvestments)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_EquityInvestments_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.EquityInvestments)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_EquityInvestments_FinancialAccounts");
        });

        modelBuilder.Entity<Explanation>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Thuyết minh"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.TemplateNo).HasMaxLength(200);
            entity.Property(e => e.Year)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.Explanations)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Explanations_CompanyInfos");
        });

        modelBuilder.Entity<ExplanationAddionalInformationTradingSecurity>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Chứng khoán kinh doanh"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYearFairValue)
                .HasComment("Giá trị hợp lý")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYearFairValue)
                .HasComment("Giá trị hợp lý")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExplanationAddionalInformationTradingSecuritiesParent).WithMany(p => p.InverseExplanationAddionalInformationTradingSecuritiesParent)
                .HasForeignKey(d => d.ExplanationAddionalInformationTradingSecuritiesParentId)
                .HasConstraintName("FK_ExplanationAddionalInformationTradingSecurities_ExplanationAddionalInformationTradingSecurities");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.ExplanationAddionalInformationTradingSecurities)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExplanationAddionalInformationTradingSecurities_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.ExplanationAddionalInformationTradingSecurities)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_ExplanationAddionalInformationTradingSecurities_FinancialAccounts");
        });

        modelBuilder.Entity<ExplanationAdditionalInformationMoney>(entity =>
        {
            entity.ToTable("ExplanationAdditionalInformationMoney", tb => tb.HasComment("Chi phí khác về tiền"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYear).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYear).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExplanationAdditionalInformationMoneyParent).WithMany(p => p.InverseExplanationAdditionalInformationMoneyParent)
                .HasForeignKey(d => d.ExplanationAdditionalInformationMoneyParentId)
                .HasConstraintName("FK_ExplanationAdditionalInformationMoney_ExplanationAdditionalInformationMoney");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.ExplanationAdditionalInformationMoneys)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExplanationAdditionalInformationMoney_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.ExplanationAdditionalInformationMoneys)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_ExplanationAdditionalInformationMoney_FinancialAccounts");
        });

        modelBuilder.Entity<ExplanationDetail>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Chi tiết thuyết minh"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.IndexName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Explanation).WithMany(p => p.ExplanationDetails)
                .HasForeignKey(d => d.ExplanationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ExplanationDetails_Explanations");
        });

        modelBuilder.Entity<FinancialAccount>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.AccountNo).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.ParentFinanceAccount).WithMany(p => p.InverseParentFinanceAccount)
                .HasForeignKey(d => d.ParentFinanceAccountId)
                .HasConstraintName("FK_FinancialAccounts_FinancialAccounts");
        });

        modelBuilder.Entity<GeneralJournal>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Nhật ký chung"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Year)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Company).WithMany(p => p.GeneralJournals)
                .HasForeignKey(d => d.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GeneralJournals_CompanyInfos");
        });

        modelBuilder.Entity<GeneralJournalDetail>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.DocumentNo)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.CreditAccount).WithMany(p => p.GeneralJournalDetailCreditAccounts)
                .HasForeignKey(d => d.CreditAccountId)
                .HasConstraintName("FK_GeneralJournalDetails_FinancialAccounts1");

            entity.HasOne(d => d.DebitAccount).WithMany(p => p.GeneralJournalDetailDebitAccounts)
                .HasForeignKey(d => d.DebitAccountId)
                .HasConstraintName("FK_GeneralJournalDetails_FinancialAccounts");

            entity.HasOne(d => d.GeneralJournal).WithMany(p => p.GeneralJournalDetails)
                .HasForeignKey(d => d.GeneralJournalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GeneralJournalDetails_GeneralJournals");
        });

        modelBuilder.Entity<HeldToMaturityInvestment>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Đầu tư nắm giữ đến ngày đáo hạn"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearValue)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYearOriginalPrice)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearValue)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.HeldToMaturityInvestments)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_HeldToMaturityInvestments_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.HeldToMaturityInvestments)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_HeldToMaturityInvestments_FinancialAccounts");

            entity.HasOne(d => d.HeldToMaturityInvestmentParent).WithMany(p => p.InverseHeldToMaturityInvestmentParent)
                .HasForeignKey(d => d.HeldToMaturityInvestmentParentId)
                .HasConstraintName("FK_HeldToMaturityInvestments_HeldToMaturityInvestments");
        });

        modelBuilder.Entity<IncomeStatement>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Hoạt động kinh doanh"));

            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.IconUrl).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(500);
            entity.Property(e => e.Url).HasMaxLength(255);
            entity.Property(e => e.VnName)
                .HasMaxLength(255)
                .HasColumnName("Vn_Name");

            entity.HasOne(d => d.Group).WithMany(p => p.Menus)
                .HasForeignKey(d => d.GroupId)
                .HasConstraintName("FK_Menus_MetadataItems");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Menus_Menus");
        });

        modelBuilder.Entity<MenuDepartmentMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_DepartmentMenuMapping");

            entity.ToTable("MenuDepartmentMapping");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.MenuDepartmentMappings)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuDepartmentMapping_Departments");

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuDepartmentMappings)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuDepartmentMapping_Menus");
        });

        modelBuilder.Entity<MenuRoleMapping>(entity =>
        {
            entity.ToTable("MenuRoleMapping");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuRoleMappings)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRoleMapping_Menus");

            entity.HasOne(d => d.Role).WithMany(p => p.MenuRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuRoleMapping_Roles");
        });

        modelBuilder.Entity<MenuUserMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_UserMenuMapping");

            entity.ToTable("MenuUserMapping");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Menu).WithMany(p => p.MenuUserMappings)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuUserMapping_Menus");

            entity.HasOne(d => d.User).WithMany(p => p.MenuUserMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MenuUserMapping_Users");
        });

        modelBuilder.Entity<MetadataItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MetadataItems");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Type).WithMany(p => p.MetadataItems)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_dbo.MetadataItems_dbo.MetadataTypes_TypeId");
        });

        modelBuilder.Entity<MetadataType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.MetadataTypes");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<MissingAsset>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Tài sản thiếu chờ xử lý (Chi tiết từng loại tài sản thiếu)"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYearValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYearValue).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.MissingAssets)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_MissingAssets_ExplanationDetails");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.MissingAssets)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_MissingAssets_FinancialAccounts");

            entity.HasOne(d => d.MissingAssetParent).WithMany(p => p.InverseMissingAssetParent)
                .HasForeignKey(d => d.MissingAssetParentId)
                .HasConstraintName("FK_MissingAssets_MissingAssets");
        });

        modelBuilder.Entity<Navigation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Navigation");

            entity.ToTable("Navigation");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.IsActived).HasDefaultValue(false);
            entity.Property(e => e.IsBlank).HasDefaultValue(false);
            entity.Property(e => e.IsDeleted).HasDefaultValue(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedById).HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.NavigationParentId).HasColumnName("NavigationParentID");
            entity.Property(e => e.Url).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.Navigations)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK_Navigation_Users");

            entity.HasOne(d => d.NavigationParent).WithMany(p => p.InverseNavigationParent)
                .HasForeignKey(d => d.NavigationParentId)
                .HasConstraintName("FK_Navigation_Navigation");
        });

        modelBuilder.Entity<OtherReceivable>(entity =>
        {
            entity.ToTable(tb => tb.HasComment("Phải thu khác"));

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.EndOfYear)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IndexName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.StartOfYear)
                .HasComment("Giá gốc")
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StartOfYearProvision)
                .HasComment("Dự phòng")
                .HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ExplanationDetail).WithMany(p => p.OtherReceivables)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtherReceivables_ExplanationDetails");

            entity.HasOne(d => d.ExplanationDetailNavigation).WithMany(p => p.InverseExplanationDetailNavigation)
                .HasForeignKey(d => d.ExplanationDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_OtherReceivables_OtherReceivables");

            entity.HasOne(d => d.FinancialAccount).WithMany(p => p.OtherReceivables)
                .HasForeignKey(d => d.FinancialAccountId)
                .HasConstraintName("FK_OtherReceivables_FinancialAccounts");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Department).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_Permissions_Departments");
        });

        modelBuilder.Entity<Profile>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<ProfileAttachmentFileMapping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.AttachmentFile).WithMany(p => p.ProfileAttachmentFileMappings)
                .HasForeignKey(d => d.AttachmentFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfileAttachmentFileMappings_AttachmentFiles");

            entity.HasOne(d => d.Profile).WithMany(p => p.ProfileAttachmentFileMappings)
                .HasForeignKey(d => d.ProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProfileAttachmentFileMappings_Profiles");
        });

        modelBuilder.Entity<ProvinceMasterDatum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code).HasMaxLength(200);
        });

        modelBuilder.Entity<ReceivingDepartmentDocument>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Department).WithMany(p => p.ReceivingDepartmentDocuments)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivingDepartmentDocuments_Departments");

            entity.HasOne(d => d.Document).WithMany(p => p.ReceivingDepartmentDocuments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReceivingDepartmentDocuments_Documents");
        });

        modelBuilder.Entity<ReferenceDocument>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Document).WithMany(p => p.ReferenceDocuments)
                .HasForeignKey(d => d.DocumentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ReferenceDocuments_Documents");
        });

        modelBuilder.Entity<ReferenceNumber>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.ReferenceNumbers");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ModuleType).HasMaxLength(255);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Role");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("Status");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<TaskAttachmentMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssignmentAttachmentMappings");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.AttachmentFile).WithMany(p => p.TaskAttachmentMappings)
                .HasForeignKey(d => d.AttachmentFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AssignmentAttachmentMappings_AttachmentFiles");

            entity.HasOne(d => d.TaskManagement).WithMany(p => p.TaskAttachmentMappings)
                .HasForeignKey(d => d.TaskManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskAttachmentMappings_TaskManagements");
        });

        modelBuilder.Entity<TaskDepartmentMapping>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_AssignmentDepartments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.TaskDepartmentMappings)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_AssignmentDepartments_Departments");

            entity.HasOne(d => d.TaskManagement).WithMany(p => p.TaskDepartmentMappings)
                .HasForeignKey(d => d.TaskManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskDepartmentMappings_TaskManagements");

            entity.HasOne(d => d.User).WithMany(p => p.TaskDepartmentMappings)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_TaskDepartmentMappings_Users");
        });

        modelBuilder.Entity<TaskExtend>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApproverNote).HasMaxLength(255);
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Note).HasMaxLength(255);

            entity.HasOne(d => d.TaskManagement).WithMany(p => p.TaskExtends)
                .HasForeignKey(d => d.TaskManagementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_TaskExtends_TaskManagements");
        });

        modelBuilder.Entity<TaskExtendAttachmentMapping>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.AttachmentFile).WithMany(p => p.TaskExtendAttachmentMappings)
                .HasForeignKey(d => d.AttachmentFileId)
                .HasConstraintName("FK_TaskExtendAttachmentMappings_AttachmentFiles");

            entity.HasOne(d => d.TaskExtend).WithMany(p => p.TaskExtendAttachmentMappings)
                .HasForeignKey(d => d.TaskExtendId)
                .HasConstraintName("FK_TaskExtendAttachmentMappings_TaskExtends");
        });

        modelBuilder.Entity<TaskManagement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Assignments");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.Classify).WithMany(p => p.TaskManagements)
                .HasForeignKey(d => d.ClassifyId)
                .HasConstraintName("FK_TaskManagements_CategoryDetails");

            entity.HasOne(d => d.Document).WithMany(p => p.TaskManagements)
                .HasForeignKey(d => d.DocumentId)
                .HasConstraintName("FK_Assignments_Documents");

            entity.HasOne(d => d.ParentTask).WithMany(p => p.InverseParentTask)
                .HasForeignKey(d => d.ParentTaskId)
                .HasConstraintName("FK_TaskManagements_TaskManagements");

            entity.HasOne(d => d.Processor).WithMany(p => p.TaskManagements)
                .HasForeignKey(d => d.ProcessorId)
                .HasConstraintName("FK_Assignments_Users");
        });

        modelBuilder.Entity<TaskManagementHistory>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Status).HasMaxLength(100);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.TaskManagement).WithMany(p => p.TaskManagementHistories)
                .HasForeignKey(d => d.TaskManagementId)
                .HasConstraintName("FK_TaskManagementHistories_TaskManagements");
        });

        modelBuilder.Entity<TrackingLog>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ItemType)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.Users");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        modelBuilder.Entity<UserDepartmentMapping>(entity =>
        {
            entity.ToTable("UserDepartmentMapping");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Department).WithMany(p => p.UserDepartmentMappings)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDepartmentMapping_Departments");

            entity.HasOne(d => d.User).WithMany(p => p.UserDepartmentMappings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserDepartmentMapping_Users");
        });

        modelBuilder.Entity<UserDepartmentRoleMapping>(entity =>
        {
            entity.ToTable("UserDepartmentRoleMapping");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);

            entity.HasOne(d => d.Role).WithMany(p => p.UserDepartmentRoleMappings)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserDepartmentRoleMapping_Roles");

            entity.HasOne(d => d.UserDepartmentMapping).WithMany(p => p.UserDepartmentRoleMappings)
                .HasForeignKey(d => d.UserDepartmentMappingId)
                .HasConstraintName("FK_UserDepartmentRoleMapping_UserDepartmentMapping");
        });

        modelBuilder.Entity<WardMasterDatum>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<WorkflowInstance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WorkflowInstances");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ItemReferenceNumber).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedById).HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));

            entity.HasOne(d => d.Template).WithMany(p => p.WorkflowInstances)
                .HasForeignKey(d => d.TemplateId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowInstances_WorkflowTemplates");
        });

        modelBuilder.Entity<WorkflowProcessing>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WorkflowHistories");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ItemType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ReferenceNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.RequestedDepartmentName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.RequestedUserName).HasMaxLength(1255);
            entity.Property(e => e.Status).HasMaxLength(100);

            entity.HasOne(d => d.AssignedToDepartment).WithMany(p => p.WorkflowProcessings)
                .HasForeignKey(d => d.AssignedToDepartmentId)
                .HasConstraintName("FK_dbo.WorkflowHistories_dbo.Departments_AssignedToDepartmentId");

            entity.HasOne(d => d.AssignedToUser).WithMany(p => p.WorkflowProcessings)
                .HasForeignKey(d => d.AssignedToUserId)
                .HasConstraintName("FK_dbo.WorkflowHistories_dbo.Users_AssignedToUserId");

            entity.HasOne(d => d.Instance).WithMany(p => p.WorkflowProcessings)
                .HasForeignKey(d => d.InstanceId)
                .HasConstraintName("FK_dbo.WorkflowHistories_dbo.WorkflowInstances_InstanceId");
        });

        modelBuilder.Entity<WorkflowRole>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.Role).WithMany(p => p.WorkflowRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkflowRoles_Roles");

            entity.HasOne(d => d.WorkflowStep).WithMany(p => p.WorkflowRoles)
                .HasForeignKey(d => d.WorkflowStepId)
                .HasConstraintName("FK_WorkflowRoles_WorkflowSteps");
        });

        modelBuilder.Entity<WorkflowStep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Steps");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FailureVote).HasMaxLength(255);
            entity.Property(e => e.StepName).HasMaxLength(255);
            entity.Property(e => e.SuccessVote).HasMaxLength(255);

            entity.HasOne(d => d.AssignToDepartment).WithMany(p => p.WorkflowSteps)
                .HasForeignKey(d => d.AssignToDepartmentId)
                .HasConstraintName("FK_WorkflowSteps_Departments");

            entity.HasOne(d => d.AssignToUser).WithMany(p => p.WorkflowSteps)
                .HasForeignKey(d => d.AssignToUserId)
                .HasConstraintName("FK_WorkflowSteps_Users");

            entity.HasOne(d => d.WorkflowTemplate).WithMany(p => p.WorkflowSteps)
                .HasForeignKey(d => d.WorkflowTemplateId)
                .HasConstraintName("FK_WorkflowSteps_WorkflowTemplates");
        });

        modelBuilder.Entity<WorkflowTemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_dbo.WorkflowTemplates");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedBy).HasMaxLength(255);
            entity.Property(e => e.CreatedByFullName).HasMaxLength(255);
            entity.Property(e => e.ItemType).HasMaxLength(255);
            entity.Property(e => e.ModifiedBy).HasMaxLength(255);
            entity.Property(e => e.ModifiedByFullName).HasMaxLength(255);
            entity.Property(e => e.ModifiedById).HasDefaultValue(new Guid("00000000-0000-0000-0000-000000000000"));
            entity.Property(e => e.WorkflowName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
