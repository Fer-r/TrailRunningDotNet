using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages;

public class RegisterModel(RacehubContext context) : PageModel
{
    [BindProperty]
    public string Email { get; set; } = "";

    [BindProperty]
    public string Password { get; set; } = "";

    [BindProperty]
    public string Name { get; set; } = "";

    [BindProperty]
    public string? Gender { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (context.Users.Any(u => u.Email == Email))
        {
            ErrorMessage = "El email ya está registrado.";
            return Page();
        }

        var user = new User
        {
            Email = Email,
            Password = BCrypt.Net.BCrypt.HashPassword(Password),
            Name = Name,
            Gender = Gender,
            Roles = "[\"ROLE_USER\"]"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // Redirect to the React app's login page
        return Redirect("http://localhost:5173/login");
    }
}
