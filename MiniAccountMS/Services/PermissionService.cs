using MiniAccountMS.Data;
using System.Data.SqlClient;

namespace MiniAccountMS.Services
{
    public class PermissionService
    {
        private readonly ApplicationDbContext _context;

        public PermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AssignPermissionAsync(string roleId, string moduleName, bool canAccess)
        {
            using (var conn = _context.GetConnection())
            {
                var cmd = new SqlCommand("sp_AssignModulePermission", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@ModuleName", moduleName);
                cmd.Parameters.AddWithValue("@CanAccess", canAccess);

                await conn.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<bool> HasPermissionAsync(string userId, string moduleName)
        {
            using (var conn = _context.GetConnection())
            {
                var cmd = new SqlCommand("sp_CheckPermission", conn)
                {
                    CommandType = System.Data.CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@ModuleName", moduleName);

                await conn.OpenAsync();
                var result = await cmd.ExecuteScalarAsync();

                return result != null;
            }
        }
    }
}
