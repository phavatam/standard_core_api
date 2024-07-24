namespace IziWork.Business.DTO
{
    public class FinancialAccountDTO
    {
        public Guid Id { get; set; }
        public Guid? ParentFinanceAccountId { get; set; }
        public string AccountNo { get; set; } = null!;
        public string? AccountName { get; set; }
        public string? Description { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public Guid? CreatedById { get; set; }
        public Guid? ModifiedById { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public string? CreatedByFullName { get; set; }
        public string? ModifiedByFullName { get; set; }
        public List<FinancialAccountDTO> Items { get; set; } = new List<FinancialAccountDTO>();
    }
}
