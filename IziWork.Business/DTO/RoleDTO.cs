namespace IziWork.Business.DTO
{
    public class RoleDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty!;
        public string Code { get; set; } = string.Empty!;
        public bool IsActivated { get; set; }
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Modified { get; set; }
    }
}
