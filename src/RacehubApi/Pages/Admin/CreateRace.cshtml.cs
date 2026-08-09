using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages.Admin;

public class CreateRaceModel(RacehubContext context, IWebHostEnvironment env) : PageModel
{
    [BindProperty] public string Name { get; set; } = "";
    [BindProperty] public string Description { get; set; } = "";
    [BindProperty] public DateTime Date { get; set; } = DateTime.Now.AddDays(30);
    [BindProperty] public int DistanceKm { get; set; }
    [BindProperty] public string Location { get; set; } = "";
    [BindProperty] public string? Coordinates { get; set; }
    [BindProperty] public int? Unevenness { get; set; }
    [BindProperty] public int EntryFee { get; set; }
    [BindProperty] public int AvailableSlots { get; set; } = 100;
    [BindProperty] public string Status { get; set; } = "open";
    [BindProperty] public List<string> SelectedCategories { get; set; } = new();
    [BindProperty] public IFormFile? ImageFile { get; set; }
    [BindProperty] public string? Gender { get; set; }

    public string? ErrorMessage { get; set; }

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Location))
        {
            ErrorMessage = "El nombre y la ubicación son obligatorios.";
            return Page();
        }

        string? imagePath = null;
        if (ImageFile is { Length: > 0 })
        {
            var uploadsDir = Path.Combine(env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(ImageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await ImageFile.CopyToAsync(stream);

            imagePath = $"/uploads/{fileName}";
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
            Category = SelectedCategories.Count > 0 ? string.Join(", ", SelectedCategories) : null,
            Image = imagePath,
            Gender = Gender
        };

        context.TrailRunnings.Add(race);
        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"¡Carrera \"{race.Name}\" creada con éxito! (ID: {race.Id})";
        return RedirectToPage("Index");
    }
}
