using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages.Admin;

public class UsersModel(RacehubContext context) : PageModel
{
    public List<User> Users { get; set; } = [];
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync()
    {
        Users = await context.Users
            .AsNoTracking()
            .OrderBy(u => u.Id)
            .ToListAsync();

        if (TempData["SuccessMessage"] is string msg)
        {
            SuccessMessage = msg;
        }
    }

    public async Task<IActionResult> OnPostToggleBanAsync(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user != null)
        {
            user.Banned = !user.Banned;
            await context.SaveChangesAsync();
            
            var status = user.Banned ? "baneado" : "desbaneado";
            TempData["SuccessMessage"] = $"Usuario \"{user.Email}\" {status} correctamente.";
        }

        return RedirectToPage();
    }
}
