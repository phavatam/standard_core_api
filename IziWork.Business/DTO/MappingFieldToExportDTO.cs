using IziWork.Common.Enums;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class MappingFieldToExportDTO
    {
        public string Name { get; set; } // Property Name of ViewModel
        public string DisplayName { get; set; } // Name to show on header and get value form ViewModel
        public bool Visible { get; set; } // Config to show or hidden on Excel
        public FieldTypeEnums Type { get; set; } // Type of Value
    }

    public class FieldMappingDTO
    {
        public string SourceField { get; set; }
        public string TargetField { get; set; }
        public FieldTypeEnums Type { get; set; }
    }
}
