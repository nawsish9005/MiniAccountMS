using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MiniAccountMS.Models;
using MiniAccountMS.Services;

namespace MiniAccountMS.Pages.Vouchar
{

    public class CreateModel : PageModel
    {
        private readonly VoucharService _voucherService;

        public CreateModel(VoucharService voucherService)
        {
            _voucherService = voucherService;
        }
        [BindProperty]
        public Voucher Voucher { get; set; } = new Voucher { Items = new List<VoucherItem> { new VoucherItem() } };

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            Voucher.CreatedBy = User.Identity.Name;

            await _voucherService.CreateAsync(Voucher);
            return RedirectToPage("Index");
        }
    }
}
