using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IziWork.Business.DTO
{
    public class AttachmentFileDTO
    {
        public Guid Id { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Modified { get; set; }
        public string Type { get; set; }
        public string FileName { get; set; }
        public string Extension { get; set; }
        public string FileDisplayName { get; set; }
        public string FileUniqueName { get; set; }
        public long Size { get; set; }
        public byte[] Base64ImageValue { get; set; }
    }

    public class FileResultDTO
    {
        public string FileName { get; set; } // tên file muốn save xuống // có kèm đuôi luôn nha
        public object Content { get; set; } // byte array // base64, cái gì cũng được, miễn content của file trả về
        public string Type { get; set; } // MimeType
    }
}