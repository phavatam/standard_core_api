using System;
using System.Collections.Generic;

namespace IziWork.Business.DTO;

public partial class ProfileAttachmentFileMappingDTO
{
    public Guid Id { get; set; }
    public Guid ProfileId { get; set; }
    public Guid AttachmentFileId { get; set; }
    public string? FileName { get; set; }
    public string? FileDisplayName { get; set; }
    public string? FileUniqueName { get; set; }
    public int? Size { get; set; }
    public string? Type { get; set; }
    public string? Extension { get; set; }
}
