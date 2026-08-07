using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages;

public class CreateRaceModel(RacehubContext context) : PageModel
{
    [BindProperty]
    public string Name { get; set; } = "";

    [BindProperty]
    public string Description { get; set; } = "";

    [BindProperty]
    public DateTime Date { get; set; } = DateTime.Now.AddDays(30);

    [BindProperty]
    public int DistanceKm { get; set; }

    [BindProperty]
    public string Location { get; set; } = "";

    [BindProperty]
    public string? Coordinates { get; set; }

    [BindProperty]
    public int? Unevenness { get; set; }

    [BindProperty]
    public int EntryFee { get; set; }

    [BindProperty]
    public int AvailableSlots { get; set; } = 100;

    [BindProperty]
    public string Status { get; set; } = "open";

    [BindProperty]
    public string? Category { get; set; }

    [BindProperty]
    public string? Image { get; set; }

    [BindProperty]
    public string? Gender { get; set; }

    public string? ErrorMessage { get; set; }
    public string? SuccessMessage { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location))
        {
            ErrorMessage = "El nombre y la ubicación son obligatorios.";
            return Page();
        }

        var race = new TrailRunning
        {
            Name = Name,
            Description = Description,
            Date = Date,
            DistanceKm = DistanceKm,
            Location = Location,
            Coordinates = Coordinates,
            Unevenness = Unevenness,
            EntryFee = EntryFee,
            AvailableSlots = AvailableSlots,
            Status = Status,
            Category = Category,
            Image = Image,
            Gender = Gender
        };

        context.TrailRunnings.Add(race);
        await context.SaveChangesAsync();

        SuccessMessage = $"¡Carrera \"{race.Name}\" creada con éxito! (ID: {race.Id})";

        // Reset form
        Name = "";
        Description = "";
        Date = DateTime.Now.AddDays(30);
        DistanceKm = 0;
        Location = "";
        Coordinates = null;
        Unevenness = null;
        EntryFee = 0;
        AvailableSlots = 100;
        Status = "open";
        Category = null;
        Image = null;
        Gender = null;

        return Page();
    }
}
