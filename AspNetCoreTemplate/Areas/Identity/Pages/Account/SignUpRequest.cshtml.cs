using AspNetMvcTemplate.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetMvcTemplate.Areas.Identity.Pages.Account
{
    public class SignUpRequestModel : PageModel
    {
        private readonly UserManager<AppUser> _userManager;

        public SignUpRequestModel(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var username = Request.Form["username"];
            if (string.IsNullOrEmpty(username)) return Page();

            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                ModelState.AddModelError("Username", "This username is already taken");
                return Page(); // Re-render the page with the error message
            }

            return RedirectToPage("SignUp", new { username = username});
        }
    }
}
