using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

/// <summary>
/// Tài sản thiếu chờ xử lý (Chi tiết từng loại tài sản thiếu)
/// </summary>
public partial class MissingAsset
{
    public Guid Id { get; set; }

    public Guid ExplanationDetailId { get; set; }

    public Guid? MissingAssetParentId { get; set; }

    public string TargetName { get; set; } = null!;

    public int Index { get; set; }

    public string IndexName { get; set; } = null!;

    public double? StartOfYearQty { get; set; }

    public decimal? StartOfYearValue { get; set; }

    public double? EndOfYearQty { get; set; }

    public decimal? EndOfYearValue { get; set; }

    public Guid? FinancialAccountId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTimeOffset? Created { get; set; }

    public DateTimeOffset? Modified { get; set; }

    public Guid? CreatedById { get; set; }

    public Guid? ModifiedById { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? CreatedByFullName { get; set; }

    public string? ModifiedByFullName { get; set; }

    public virtual ExplanationDetail ExplanationDetail { get; set; } = null!;

    public virtual FinancialAccount? FinancialAccount { get; set; }

    public virtual ICollection<MissingAsset> InverseMissingAssetParent { get; set; } = new List<MissingAsset>();

    public virtual MissingAsset? MissingAssetParent { get; set; }
}
