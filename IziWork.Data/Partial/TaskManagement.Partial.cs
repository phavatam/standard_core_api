using IziWork.Data.Interface;
using System;
using System.Collections.Generic;

namespace IziWork.Data.Entities;

public partial class TaskManagement : IAuditableEntity, ISoftDeleteEntity, IAutoNumber, ISoftStatusEntity
{
}