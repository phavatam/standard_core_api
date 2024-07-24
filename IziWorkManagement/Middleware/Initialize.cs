using IziWork.Business.Enums;
using IziWork.Data.Entities;

namespace IziWorkManagement.Middleware
{
    public class Initialize
    {
        public static void AddMasterData(UpgradeApplicationContext dbContext)
        {
            AddCategory(dbContext);
            AddReferenceNumber(dbContext);
            AddStatus(dbContext);
            AddRole(dbContext);
            AddWorkflow(dbContext);

            AccountingBalanceSheetTemplates(dbContext);
            dbContext.SaveChanges();
        }

        private static void AddCategory(UpgradeApplicationContext dbContext)
        {
            #region Document
            // So van ban
            var findCategory = dbContext.Categories.Find(new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"));
            if (findCategory == null)
            {
                var newRegistry = new Category
                {
                    Id = new Guid("3f2504e0-4f89-41d3-9a0c-0305e82c3301"),
                    Name = "Sổ văn bản",
                    Module = "Document",
                    Type = (int)DefineEnums.CATEGORY_TYPE.Registry,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Categories.Add(newRegistry);
            }

            // Loai van ban
            findCategory = dbContext.Categories.Find(new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"));
            if (findCategory == null)
            {
                var newDocumentType = new Category
                {
                    Id = new Guid("7c9e6679-7425-40de-944b-e07fc1f90ae7"),
                    Name = "Loại văn bản",
                    Module = "Document",
                    Type = (int)DefineEnums.CATEGORY_TYPE.DocumentType,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Categories.Add(newDocumentType);
            }

            // Do mat
            findCategory = dbContext.Categories.Find(new Guid("8e2f0136-1dbf-4e3a-9c34-72d7a75fb3df"));
            if (findCategory == null)
            {
                var newSecurityLevel = new Category
                {
                    Id = new Guid("8e2f0136-1dbf-4e3a-9c34-72d7a75fb3df"),
                    Name = "Độ mật",
                    Module = "Document",
                    Type = (int)DefineEnums.CATEGORY_TYPE.SecurityLevel,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Categories.Add(newSecurityLevel);
            }

            // Do khan
            findCategory = dbContext.Categories.Find(new Guid("e2a8f784-6f93-4998-b8e7-f66f6c1b2d7d"));
            if (findCategory == null)
            {
                var newUrgencyLevel = new Category
                {
                    Id = new Guid("e2a8f784-6f93-4998-b8e7-f66f6c1b2d7d"),
                    Name = "Độ khẩn",
                    Module = "Document",
                    Type = (int)DefineEnums.CATEGORY_TYPE.UrgencyLevel,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Categories.Add(newUrgencyLevel);
            }
            #endregion

            #region Task 
            // Do khan
            findCategory = dbContext.Categories.Find(new Guid("e2a7f781-6f93-4918-b8e7-f66f6c1b2d7d"));
            if (findCategory == null)
            {
                var newClassifyTask = new Category
                {
                    Id = new Guid("e2a7f781-6f93-4918-b8e7-f66f6c1b2d7d"),
                    Name = "Phân loại công việc",
                    Module = "Task",
                    Type = (int)DefineEnums.CATEGORY_TYPE.ClassifyTask,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Categories.Add(newClassifyTask);
            }
            #endregion
        }

        private static void AddReferenceNumber(UpgradeApplicationContext dbContext)
        {
            var findRef = dbContext.ReferenceNumbers.Find(new Guid("e2a8f784-6f93-b8e7-4998-f66f6c1b2d7d"));
            if (findRef == null)
            {
                var refDocument = new ReferenceNumber
                {
                    Id = new Guid("e2a8f784-6f93-b8e7-4998-f66f6c1b2d7d"),
                    ModuleType = "Document",
                    CurrentNumber = 0,
                    IsNewYearReset = true,
                    Formula = "DOC-{AutoNumberLength:7}-{Year}",
                    CurrentYear = DateTime.Now.Year,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.ReferenceNumbers.Add(refDocument);
            }

            findRef = dbContext.ReferenceNumbers.Find(new Guid("e2a8f785-6f93-b8e7-4998-f66f6c1b2d9d"));
            if (findRef == null)
            {
                var refTaskManagement = new ReferenceNumber
                {
                    Id = new Guid("e2a8f785-6f93-b8e7-4998-f66f6c1b2d9d"),
                    ModuleType = "TaskManagement",
                    CurrentNumber = 0,
                    IsNewYearReset = true,
                    Formula = "T-{AutoNumberLength:7}-{Year}",
                    CurrentYear = DateTime.Now.Year,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.ReferenceNumbers.Add(refTaskManagement);
            }
        }

        private static void AddStatus(UpgradeApplicationContext dbContext)
        {
            /*var findStatus = dbContext.Statuses.Find(new Guid("e5a8f784-6f93-b8e7-4998-f66f6c2b2d7d"));
            if (findStatus == null)
            {
                var newStatus = new Status
                {
                    Id = new Guid("e5a8f784-6f93-b8e7-4998-f66f6c2b2d7d"),
                    Code = "CO",
                    Name = "Completed",
                    IsActive = true,
                    IsDefault = true,
                    Type = 0,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Statuses.Add(newStatus);
            }

            findStatus = dbContext.Statuses.Find(new Guid("e2a8f726-6f94-b8e7-4998-f77f6c1b2d9d"));
            if (findStatus == null)
            {
                var newStatus = new Status
                {
                    Id = new Guid("e2a8f726-6f94-b8e7-4998-f77f6c1b2d9d"),
                    Code = "CO",
                    Name = "Completed",
                    IsActive = true,
                    IsDefault = true,
                    Type = 0,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Statuses.Add(newStatus);
            }
            dbContext.SaveChanges();*/
        }

        private static void AddRole(UpgradeApplicationContext dbContext)
        {
            var findRole = dbContext.Roles.Find(new Guid("e5a8f784-5f93-b8e3-4998-f66f7c2b2d7d"));
            if (findRole == null)
            {
                var newRole = new Role
                {
                    Id = new Guid("e5a8f784-5f93-b8e3-4998-f66f7c2b2d7d"),
                    Code = "Member",
                    Name = "Nhân viên",
                    IsActivated = true,
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Roles.Add(newRole);
            }

            findRole = dbContext.Roles.Find(new Guid("e2a8f726-4998-b8e7-b8e3-f77f6c1b2d9d"));
            if (findRole == null)
            {
                var newRole = new Role
                {
                    Id = new Guid("e2a8f726-4998-b8e7-b8e3-f77f6c1b2d9d"),
                    Code = "HOD",
                    Name = "Trưởng phòng",
                    IsActivated = true,
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Roles.Add(newRole);
            }

            findRole = dbContext.Roles.Find(new Guid("e2a8f726-4098-b2e7-b3e3-f77f6c1b2d9d"));
            if (findRole == null)
            {
                var newRole = new Role
                {
                    Id = new Guid("e2a8f726-4098-b2e7-b3e3-f77f6c1b2d9d"),
                    Code = "RecordsClerk",
                    Name = "Văn thư",
                    IsActivated = true,
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Roles.Add(newRole);
            }

            findRole = dbContext.Roles.Find(new Guid("e1a8f721-4092-b2e8-b3e3-f78f6c1b1d0d"));
            if (findRole == null)
            {
                var newRole = new Role
                {
                    Id = new Guid("e1a8f721-4092-b2e8-b3e3-f78f6c1b1d0d"),
                    Code = "Accounting",
                    Name = "Kế toán",
                    IsActivated = true,
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.Roles.Add(newRole);
            }
        }

        private static void AddWorkflow(UpgradeApplicationContext dbContext)
        {
            var findStatus = dbContext.WorkflowTemplates.Find(new Guid("e5a8f784-4998-b6e7-6f93-f66f6c2b2d7d"));
            if (findStatus == null)
            {
                var newWorkflowTemplate = new WorkflowTemplate
                {
                    Id = new Guid("e5a8f784-4998-b6e7-6f93-f66f6c2b2d7d"),
                    WorkflowName = "Document for Personal Approval",
                    ItemType = "Document",
                    Order = 0,
                    DefaultCompletedStatus = "Completed",
                    IsActivated = true,
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.WorkflowTemplates.Add(newWorkflowTemplate);

                dbContext.WorkflowSteps.AddRange(
                    new WorkflowStep
                    {
                        Id = new Guid("e7a8f784-7998-b6e7-6f93-f66f6c2b7d7d"),
                        WorkflowTemplateId = newWorkflowTemplate.Id,
                        StepName = "Submit",
                        StepNumber = 1,
                        SuccessVote = "Approve",
                        FailureVote = "Reject",
                        DueDateNumber = 3,
                        StatusId = null,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new WorkflowStep
                    {
                        Id = new Guid("e5a8f782-4998-b6e7-6f73-f66f6c2b2d2d"),
                        WorkflowTemplateId = newWorkflowTemplate.Id,
                        StepName = "Specific User Approval",
                        StepNumber = 2,
                        SuccessVote = "Approve",
                        FailureVote = "Reject",
                        DueDateNumber = 3,
                        StatusId = null,
                        Created = DateTime.Now.AddMilliseconds(10),
                        Modified = DateTime.Now.AddMilliseconds(10),
                    });
            }
        }

        #region Init Template cân đối kế toán
        private static void AccountingBalanceSheetTemplates(UpgradeApplicationContext dbContext)
        {
            var findStatus = dbContext.AccountingBalanceSheetTemplates.Find(new Guid("e5a8f784-6f93-4998-b6e7-f66f6c2b2d7d"));
            if (findStatus == null)
            {
                var newAccountingBalanceSheetTemplates = new AccountingBalanceSheetTemplate
                {
                    Id = new Guid("e5a8f784-6f93-4998-b6e7-f66f6c2b2d7d"),
                    Name = "Bảng cân đối kế toán",
                    CurrencyUnit = "VND",
                    IsDeleted = false,
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                dbContext.AccountingBalanceSheetTemplates.Add(newAccountingBalanceSheetTemplates);
                #region Tài sản
                #region A
                var A_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    IsDeleted = false,
                    Type = 1,
                    Index = 1,
                    IndexName = "A",
                    Name = "TÀI SẢN NGẮN HẠN",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_AccountingBalanceSheetTemplateDetails);

                #region A.I
                var A_I_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Type = 1,
                    Index = 1,
                    IndexName = "I",
                    ParentId = A_AccountingBalanceSheetTemplateDetails.Id,
                    IsDeleted = false,
                    Name = "Tiền và các khoản tương đương",
                    Created = DateTime.Now,
                    Modified = DateTime.Now
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_I_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        IsDeleted = false,
                        ParentId = A_I_AccountingBalanceSheetTemplateDetails.Id,
                        Type = 1,
                        Name = "Tiền",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        ParentId = A_I_AccountingBalanceSheetTemplateDetails.Id,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        IsDeleted = false,
                        Type = 1,
                        Name = "Tiền và các khoản tương đương tiền",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region A.II
                var A_II_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Type = 1,
                    Index = 1,
                    IndexName = "II",
                    ParentId = A_AccountingBalanceSheetTemplateDetails.Id,
                    IsDeleted = false,
                    Name = "Các khoản đầu tư tài chính ngắn hạn",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_II_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_II_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Đầu tư ngắn hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_II_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Dự phòng giảm giá chứng khoán kinh doanh (*)",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region A.III
                var A_III_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IsDeleted = false,
                    IndexName = "III",
                    Type = 1,
                    ParentId = A_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Các khoản phải thu ngắn hạn",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_III_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Phải thu ngắn hạn của khách hàng",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Trả trước cho người bán ngắn hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 3,
                        IndexName = "3",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Phải thu nội bộ ngắn hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 4,
                        IndexName = "4",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Phải thu theo tiến độ kế hoạch hợp đồng xây dựng",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 5,
                        IndexName = "5",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Phải thu về cho vay ngắn hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 6,
                        IndexName = "6",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Các khoản phải thu ngắn hạn khác",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 6,
                        IndexName = "6",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_III_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Tài sản thiếu chờ xử lý",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region A.IV
                var A_IV_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    IsDeleted = false,
                    Index = 1,
                    IndexName = "IV",
                    Type = 1,
                    ParentId = A_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Hàng tồn kho",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_IV_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_IV_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Hàng tồn kho",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_IV_AccountingBalanceSheetTemplateDetails.Id,
                        IsDeleted = false,
                        Name = "Dự phòng giảm giá hàng tồn kho (*)",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region A.V
                var A_V_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    IsDeleted = false,
                    Index = 1,
                    IndexName = "V",
                    Type = 1,
                    ParentId = A_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Hàng tồn kho",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(A_V_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        IsDeleted = false,
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Chi phí trả trước ngắn hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Thuế GTGT được khấu trừ",
                        IsDeleted = false,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 3,
                        IndexName = "3",
                        Type = 1,
                        IsDeleted = false,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Thuế và các khoản khác phải thu Nhà nước",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 4,
                        IndexName = "4",
                        IsDeleted = false,
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Giao dịch mua bán lại trái phiếu Chính phủ",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 5,
                        IndexName = "5",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = A_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Tài sản ngắn hạn khác",
                        IsDeleted = false,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #endregion
                #region B
                var B_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    Type = 1,
                    IndexName = "B",
                    IsDeleted = false,
                    Name = "TÀI SẢN DÀI HẠN",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_AccountingBalanceSheetTemplateDetails);

                #region B.I
                var B_I_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "I",
                    Type = 1,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Các khoản phải thu dài hạn",
                    Created = DateTime.Now,
                    IsDeleted = false,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_I_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        Type = 1,
                        IndexName = "1",
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        Name = "Phải thu dài hạn của khách hàng",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Trả trước cho người bán dài hạn",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        IsDeleted = false,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 3,
                        IndexName = "3",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Vốn kinh doanh ở đơn vị trực thuộc",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        IsDeleted = false,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 4,
                        IndexName = "4",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Phải thu dài hạn nội bộ",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 5,
                        IndexName = "5",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = " Phải thu về cho vay dài hạn",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 6,
                        IndexName = "6",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Phải thu dài hạn khác",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 7,
                        IndexName = "7",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_I_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Dự phòng phải thu dài hạn khó đòi (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B.II
                var B_II_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "II",
                    IsDeleted = false,
                    Type = 1,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản cố định",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_II_AccountingBalanceSheetTemplateDetails);

                #region B_II_1
                var B_II_1_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "1",
                    Type = 1,
                    IsDeleted = false,
                    ParentId = B_II_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản cố định hữu hình",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_II_1_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "-",
                        Type = 1,
                        IsDeleted = false,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_II_1_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Nguyên giá",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        IndexName = "2",
                        ParentId = B_II_1_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Giá trị hao mòn lũy kế (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B_II_2
                var B_II_2_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 2,
                    IsDeleted = false,
                    IndexName = "2",
                    Type = 1,
                    ParentId = B_II_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản cố định thuê tài chính",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_II_2_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "-",
                        IsDeleted = false,
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_II_2_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Nguyên giá",
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_II_2_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Giá trị hao mòn lũy kế (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B_II_3
                var B_II_3_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 3,
                    IndexName = "3",
                    IsDeleted = false,
                    Type = 1,
                    ParentId = B_II_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản cố định vô hình",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_II_3_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "-",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_II_3_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Nguyên giá",
                        IsDeleted = false,
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_II_3_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Giá trị hao mòn lũy kế (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #endregion
                #region B.III
                var B_III_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    Type = 1,
                    IndexName = "III",
                    IsDeleted = false,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Bất động sản đầu tư",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_III_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "-",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_III_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Nguyên giá",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "-",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_III_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Giá trị hao mòn lũy kế (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B.IV
                var B_IV_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "IV",
                    Type = 1,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản dở dang dài hạn",
                    Created = DateTime.Now,
                    IsDeleted = false,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_IV_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "-",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_IV_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Chi phí sản xuất kinh doanh dở dang dài hạn",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "-",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_IV_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Chi phí xây dựng cơ bản dở dang",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B.V
                var B_V_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "V",
                    Type = 1,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Đầu tư tài chính dài hạn",
                    Created = DateTime.Now,
                    IsDeleted = false,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_V_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Đầu tư vào công ty con",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Đầu tư vào công ty liên kết, liên doanh",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 3,
                        IndexName = "3",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Đầu tư góp vốn vào đơn vị khác",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 4,
                        IndexName = "4",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Dự phòng giảm giá đầu tư tài chính dài hạn (*)",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 5,
                        IndexName = "5",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_V_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Đầu tư nắm giữ đến ngày đáo hạn",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    }
                    );
                #endregion
                #region B.VI
                var B_VI_AccountingBalanceSheetTemplateDetails = new AccountingBalanceSheetTemplateDetail
                {
                    Id = Guid.NewGuid(),
                    AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                    Index = 1,
                    IndexName = "VI",
                    Type = 1,
                    ParentId = B_AccountingBalanceSheetTemplateDetails.Id,
                    Name = "Tài sản dài hạn khác",
                    Created = DateTime.Now,
                    IsDeleted = false,
                    Modified = DateTime.Now,
                };
                dbContext.AccountingBalanceSheetTemplateDetails.Add(B_VI_AccountingBalanceSheetTemplateDetails);

                dbContext.AccountingBalanceSheetTemplateDetails.AddRange(
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 1,
                        IndexName = "1",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_VI_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Chi phí trả trước dài hạn",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 2,
                        IndexName = "2",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_VI_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Tài sản thuế thu nhập hoãn lại",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 3,
                        IndexName = "3",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_VI_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Thiết bị, vật tư, phụ tùng thay thế dài hạn",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    },
                    new AccountingBalanceSheetTemplateDetail
                    {
                        Id = Guid.NewGuid(),
                        Index = 4,
                        IndexName = "4",
                        Type = 1,
                        AccountingBalanceSheetTemplateId = newAccountingBalanceSheetTemplates.Id,
                        ParentId = B_VI_AccountingBalanceSheetTemplateDetails.Id,
                        Name = "Tài sản dài hạn khác",
                        Created = DateTime.Now,
                        IsDeleted = false,
                        Modified = DateTime.Now,
                    });
                #endregion
                #endregion
                #endregion

                #region Nguồn vốn

                #endregion

                #region Các chỉ tiêu ngoài bản cân đối kế toán

                #endregion
            }
        }
        #endregion

    }
}