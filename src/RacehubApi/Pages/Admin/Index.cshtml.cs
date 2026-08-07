using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages.Admin;

public class IndexModel(RacehubContext context) : PageModel
{
    public List<TrailRunning> TrailRunnings { get; set; } = [];
    public string? SuccessMessage { get; set; }

    public async Task OnGetAsync()
    {
        TrailRunnings = await context.TrailRunnings
            .AsNoTracking()
            .OrderByDescending(r => r.Date)
            .ToListAsync();

        if (TempData["SuccessMessage"] is string msg)
        {
            SuccessMessage = msg;
        }
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var race = await context.TrailRunnings
            .Include(r => r.TrailRunningParticipants)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (race != null)
        {
            context.TrailRunningParticipants.RemoveRange(race.TrailRunningParticipants);
            context.TrailRunnings.Remove(race);
            await context.SaveChangesAsync();
            TempData["SuccessMessage"] = $"Carrera \"{race.Name}\" eliminada correctamente.";
        }

        return RedirectToPage();
    }
}
