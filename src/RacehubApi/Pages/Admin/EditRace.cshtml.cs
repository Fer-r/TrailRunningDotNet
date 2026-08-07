using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages.Admin;

public class EditRaceModel(RacehubContext context) : PageModel
{
    [BindProperty] public int RaceId { get; set; }
    [BindProperty] public string Name { get; set; } = "";
    [BindProperty] public string Description { get; set; } = "";
    [BindProperty] public DateTime Date { get; set; }
    [BindProperty] public int DistanceKm { get; set; }
    [BindProperty] public string Location { get; set; } = "";
    [BindProperty] public string? Coordinates { get; set; }
    [BindProperty] public int? Unevenness { get; set; }
    [BindProperty] public int EntryFee { get; set; }
    [BindProperty] public int AvailableSlots { get; set; }
    [BindProperty] public string Status { get; set; } = "open";
    [BindProperty] public string? Category { get; set; }
    [BindProperty] public string? Image { get; set; }
    [BindProperty] public string? Gender { get; set; }

    public string? ErrorMessage { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var race = await context.TrailRunnings.FindAsync(id);
        if (race == null) return RedirectToPage("Index");

        RaceId = race.Id;
        Name = race.Name;
        Description = race.Description;
        Date = race.Date;
        DistanceKm = race.DistanceKm;
        Location = race.Location;
        Coordinates = race.Coordinates;
        Unevenness = race.Unevenness;
        EntryFee = race.EntryFee;
        AvailableSlots = race.AvailableSlots;
        Status = race.Status;
        Category = race.Category;
        Image = race.Image;
        Gender = race.Gender;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var race = await context.TrailRunnings.FindAsync(RaceId);
        if (race == null)
        {
            ErrorMessage = "Carrera no encontrada.";
            return Page();
        }

        race.Name = Name;
        race.Description = Description;
        race.Date = Date;
        race.DistanceKm = DistanceKm;
        race.Location = Location;
        race.Coordinates = Coordinates;
        race.Unevenness = Unevenness;
        race.EntryFee = EntryFee;
        race.AvailableSlots = AvailableSlots;
        race.Status = Status;
        race.Category = Category;
        race.Image = Image;
        race.Gender = Gender;

        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Carrera \"{race.Name}\" actualizada correctamente.";
        return RedirectToPage("Index");
    }
}
