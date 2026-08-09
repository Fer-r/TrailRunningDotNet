using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;

namespace RacehubApi.Pages.Admin;

public class LoginModel(RacehubContext context) : PageModel
{
    [BindProperty]
    public string Email { get; set; } = string.Empty;

    [BindProperty]
    public string Password { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public IActionResult OnGet()
    {
        // Si ya está logueado y es admin, redirigir al panel
        if (User.Identity?.IsAuthenticated == true && User.IsInRole("ROLE_ADMIN"))
        {
            return RedirectToPage("/Admin/Index");
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
        {
            ErrorMessage = "Debe introducir email y contraseña.";
            return Page();
        }

        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == Email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(Password, user.Password))
        {
            ErrorMessage = "Credenciales incorrectas.";
            return Page();
        }

        if (user.Banned)
        {
            ErrorMessage = "El usuario está baneado.";
            return Page();
        }

        if (!user.Roles.Contains("ROLE_ADMIN"))
        {
            ErrorMessage = "Acceso denegado: Se requiere rol de administrador.";
            return Page();
        }

        // Crear Claims para la Cookie
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Name)
        };

        var cleanRoles = user.Roles.Replace("[", "").Replace("]", "").Replace("\"", "").Replace(" ", "");
        var rolesList = cleanRoles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        foreach (var role in rolesList)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true, // Recordar sesión
            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToPage("/Admin/Index");
    }
}
