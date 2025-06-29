namespace MiniAccountMS.Models
{
    public class RolePermission
    {
        public int Id { get; set; }

        public string RoleId { get; set; }

        public string ModuleName { get; set; }

        public bool CanAccess { get; set; }
    }
}
