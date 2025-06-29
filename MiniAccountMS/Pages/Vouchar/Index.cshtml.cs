using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountMS.Models;
using MiniAccountMS.Services;

namespace MiniAccountMS.Pages.Vouchar
{
    public class IndexModel : PageModel
    {
        private readonly VoucharService _voucherService;

        public IndexModel(VoucharService voucherService)
        {
            _voucherService = voucherService;
        }
        public List<Voucher> Vouchers { get; set; }

        public async Task OnGetAsync()
        {
            Vouchers = await _voucherService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            await _voucherService.DeleteAsync(id);
            return RedirectToPage();
        }
    }
}
