namespace IziWork.Business.DTO
{
    public class CompanyInfoImportFileDTO
    {
        public string LineOfExcel { get; set; }
        public Guid Id { get; set; }

        public string? TaxNo { get; set; }

        public string? CompanyName { get; set; }

        public string? Address { get; set; }

        public Guid? WardId { get; set; }

        public Guid? ProvinceId { get; set; }

        public string? FullAddress { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ReportingPeriod { get; set; }

        public DateTimeOffset? ReportingDate { get; set; }

        public string? ReportingDateInWords { get; set; }

        public string? PreparedByName { get; set; }

        public Guid? AccountantId { get; set; }

        public string? AccountantName { get; set; }

        public Guid? CeoId { get; set; }

        public string? Ceoname { get; set; }

        public string? PositionName { get; set; }

        public string? BusinessSector { get; set; }

        public string? RegulatoryAgency { get; set; }

        public string? AccountingMethod { get; set; }

        public string? ReportingPeriodAbbreviation { get; set; }

        public string? OwnershipForm { get; set; }

        public int? TotalEmployees { get; set; }

        public DateTimeOffset? Created { get; set; }

        public DateTimeOffset? Modified { get; set; }

        public Guid? CreatedById { get; set; }

        public Guid? ModifiedById { get; set; }

        public string? CreatedBy { get; set; }

        public string? ModifiedBy { get; set; }

        public string? CreatedByFullName { get; set; }

        public string? ModifiedByFullName { get; set; }
    }
}
