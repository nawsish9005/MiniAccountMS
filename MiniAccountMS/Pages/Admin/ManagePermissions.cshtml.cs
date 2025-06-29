using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountMS.Services;

namespace MiniAccountMS.Pages.Admin
{
    public class ManagePermissionsModel : PageModel
    {
        private readonly PermissionService _permissionService;

        public ManagePermissionsModel(PermissionService permissionService)
        {
            _permissionService = permissionService;
        }

        [BindProperty]
        public string RoleId { get; set; }

        [BindProperty]
        public string ModuleName { get; set; }

        [BindProperty]
        public bool CanAccess { get; set; }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            await _permissionService.AssignPermissionAsync(RoleId, ModuleName, CanAccess);

            TempData["Success"] = "Permission assigned successfully!";
            return Page();
        }
    }
}
