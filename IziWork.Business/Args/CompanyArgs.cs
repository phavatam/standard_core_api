using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.Args
{
    public class CompanyArgs
    {
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

        public Guid? Ceoid { get; set; }

        public string? Ceoname { get; set; }

        public string? PositionName { get; set; }

        public string? BusinessSector { get; set; }

        public string? RegulatoryAgency { get; set; }

        public string? AccountingMethod { get; set; }

        public string? ReportingPeriodAbbreviation { get; set; }

        public string? OwnershipForm { get; set; }

        public int? TotalEmployees { get; set; }
    }
}
