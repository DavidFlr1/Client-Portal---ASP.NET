using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ClientServicePortal.Web.Pages.Auth
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public LoginInputModel Input { get; set; } = new();

        public void OnGet()
        {
        }

        public class LoginInputModel
        {
            [Required(ErrorMessage = "Username is required")]
            [Display(Name = "Username")]
            public string Username { get; set; } = "";

            [Required(ErrorMessage = "Password is required")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = "";
        }
    }
}