using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi.Pages.Admin;

public class EditRaceModel(RacehubContext context, IWebHostEnvironment env) : PageModel
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
    [BindProperty] public List<string> SelectedCategories { get; set; } = new();
    [BindProperty] public IFormFile? ImageFile { get; set; }
    [BindProperty] public string? Gender { get; set; }

    public string? CurrentImage { get; set; }
    public string? ErrorMessage { get; set; }
    public bool HasParticipants { get; set; }
    public int ParticipantCount { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var race = await context.TrailRunnings
            .Include(r => r.TrailRunningParticipants)
            .FirstOrDefaultAsync(r => r.Id == id);
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
        if (!string.IsNullOrEmpty(race.Category))
        {
            SelectedCategories = race.Category.Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
        }
        CurrentImage = race.Image;
        Gender = race.Gender;
        HasParticipants = race.TrailRunningParticipants.Any();
        ParticipantCount = race.TrailRunningParticipants.Count;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var race = await context.TrailRunnings
            .Include(r => r.TrailRunningParticipants)
            .FirstOrDefaultAsync(r => r.Id == RaceId);
        if (race == null)
        {
            ErrorMessage = "Carrera no encontrada.";
            return Page();
        }

        HasParticipants = race.TrailRunningParticipants.Any();
        ParticipantCount = race.TrailRunningParticipants.Count;

        if (AvailableSlots < ParticipantCount)
        {
            ErrorMessage = $"No puedes reducir las plazas por debajo del número de inscritos ({ParticipantCount}).";
            return Page();
        }

        // Handle image upload
        if (ImageFile is { Length: > 0 })
        {
            // Delete old image if it exists
            if (!string.IsNullOrEmpty(race.Image))
            {
                var oldPath = Path.Combine(env.WebRootPath, race.Image.TrimStart('/'));
                if (System.IO.File.Exists(oldPath))
                    System.IO.File.Delete(oldPath);
            }

            var uploadsDir = Path.Combine(env.WebRootPath, "uploads");
            Directory.CreateDirectory(uploadsDir);

            var ext = Path.GetExtension(ImageFile.FileName);
            var fileName = $"{Guid.NewGuid()}{ext}";
            var filePath = Path.Combine(uploadsDir, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await ImageFile.CopyToAsync(stream);

            race.Image = $"/uploads/{fileName}";
        }

        race.Name = Name;
        race.Description = Description;
        race.Location = Location;
        race.Coordinates = Coordinates;
        race.AvailableSlots = AvailableSlots;
        race.Status = Status;
        race.Category = SelectedCategories.Count > 0 ? string.Join(", ", SelectedCategories) : null;

        if (!HasParticipants)
        {
            race.Date = Date;
            race.DistanceKm = DistanceKm;
            race.Unevenness = Unevenness;
            race.EntryFee = EntryFee;
            race.Gender = Gender;
        }

        await context.SaveChangesAsync();

        TempData["SuccessMessage"] = $"Carrera \"{race.Name}\" actualizada correctamente.";
        return RedirectToPage("Index");
    }
}
