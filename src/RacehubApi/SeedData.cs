using RacehubApi.Data;
using RacehubApi.Models;

namespace RacehubApi;

public static class SeedData
{
    public static void Initialize(RacehubContext context)
    {
        if (context.Users.Any() || context.TrailRunnings.Any())
        {
            return; // DB has been seeded
        }

        var users = new List<User>
        {
            new User
            {
                Name = "Admin User",
                Email = "admin@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Roles = "[\"ROLE_ADMIN\", \"ROLE_USER\"]",
                Age = 35,
                Gender = "Male",
                Banned = false
            },
            new User
            {
                Name = "Test Runner",
                Email = "runner@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("runner123"),
                Roles = "[\"ROLE_USER\"]",
                Age = 28,
                Gender = "Female",
                Banned = false
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        var races = new List<TrailRunning>
        {
            new TrailRunning
            {
                Name = "Ultra Pirineu",
                Description = "A challenging 100km race in the Pyrenees.",
                Date = DateTime.Now.AddMonths(2),
                DistanceKm = 100,
                Location = "Bagà, Catalonia",
                Coordinates = "42.2519, 1.8659",
                Unevenness = 6600,
                EntryFee = 120,
                AvailableSlots = 500,
                Status = "open",
                Category = "Ultra"
            },
            new TrailRunning
            {
                Name = "Transvulcania",
                Description = "Volcanic landscapes and steep climbs.",
                Date = DateTime.Now.AddMonths(3),
                DistanceKm = 73,
                Location = "La Palma, Canary Islands",
                Coordinates = "28.6019, -17.8931",
                Unevenness = 4320,
                EntryFee = 90,
                AvailableSlots = 300,
                Status = "open",
                Category = "Ultra"
            }
        };
        context.TrailRunnings.AddRange(races);
        context.SaveChanges();
    }
}
