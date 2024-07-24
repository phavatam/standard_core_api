using IziWork.Data.Entities;

namespace IziWork.Business.DTO
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string? FullName { get; set; }
        public string LoginName { get; set; } = null!;
        public int? Gender { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public bool? IsActivated { get; set; }
        public int? Type { get; set; }
        public bool? IsBlocked { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public virtual ICollection<UserDepartmentMappingDTO> UserDepartmentMappings { get; set; } = new List<UserDepartmentMappingDTO>();
    }
}
