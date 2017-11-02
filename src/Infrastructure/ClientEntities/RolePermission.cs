namespace Infrastructure.ClientEntities
{
    public class RolePermission
    {
        public string PermissionId { get; set; }
        public string PermissionName { get; set; }
        public string PermissionDescription { get; set; }
        public bool Status { get; set; }
    }
}