using IziWork.Data.Interface;
using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class Document : IAuditableEntity, ISoftDeleteEntity, IAutoNumber, ISoftStatusEntity
{
    
}
