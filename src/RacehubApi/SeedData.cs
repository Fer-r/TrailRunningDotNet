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
                Name = "Administrador",
                Email = "admin@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Roles = "[\"ROLE_ADMIN\", \"ROLE_USER\"]",
                Age = 35,
                Gender = "Male",
                Banned = false
            },
            new User
            {
                Name = "Carlos Atleta",
                Email = "carlos@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("runner123"),
                Roles = "[\"ROLE_USER\"]",
                Age = 28,
                Gender = "Male",
                Banned = false
            },
            new User
            {
                Name = "Maria Montaña",
                Email = "maria@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("runner123"),
                Roles = "[\"ROLE_USER\"]",
                Age = 32,
                Gender = "Female",
                Banned = false
            },
            new User
            {
                Name = "Laura Senderos",
                Email = "laura@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("runner123"),
                Roles = "[\"ROLE_USER\"]",
                Age = 24,
                Gender = "Female",
                Banned = false
            },
            new User
            {
                Name = "David Desnivel",
                Email = "david@racehub.com",
                Password = BCrypt.Net.BCrypt.HashPassword("runner123"),
                Roles = "[\"ROLE_USER\"]",
                Age = 41,
                Gender = "Male",
                Banned = false
            }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        var races = new List<TrailRunning>
        {
            new TrailRunning
            {
                Name = "Ultra Sierra Nevada",
                Description = "Una de las carreras más duras de la Península con altitudes que superan los 3.000 metros.",
                Date = DateTime.Now.AddMonths(1),
                DistanceKm = 100,
                Location = "Pradollano, Granada",
                Coordinates = "37.0934, -3.3970",
                Unevenness = 6000,
                EntryFee = 110,
                AvailableSlots = 400,
                Status = "open",
                Category = "Ultra Trail",
                Gender = "all"
            },
            new TrailRunning
            {
                Name = "Ultra Trail Bosques del Sur",
                Description = "Disfruta del Parque Natural de las Sierras de Cazorla, Segura y las Villas.",
                Date = DateTime.Now.AddMonths(2),
                DistanceKm = 99,
                Location = "Cazorla, Jaén",
                Coordinates = "37.9100, -3.0036",
                Unevenness = 4700,
                EntryFee = 95,
                AvailableSlots = 350,
                Status = "open",
                Category = "Ultra Trail",
                Gender = "all"
            },
            new TrailRunning
            {
                Name = "Gran Vuelta al Valle del Genal",
                Description = "Recorre los espectaculares bosques de castaños del Valle del Genal en otoño.",
                Date = DateTime.Now.AddMonths(3),
                DistanceKm = 130,
                Location = "Alpandeire, Málaga",
                Coordinates = "36.6347, -5.2014",
                Unevenness = 6000,
                EntryFee = 130,
                AvailableSlots = 300,
                Status = "open",
                Category = "Ultra Trail",
                Gender = "all"
            },
            new TrailRunning
            {
                Name = "Euráfrica Trail",
                Description = "La única carrera intercontinental del mundo, uniendo Europa y África a través del Estrecho de Gibraltar.",
                Date = DateTime.Now.AddMonths(4),
                DistanceKm = 50,
                Location = "Algeciras, Cádiz",
                Coordinates = "36.1408, -5.4562",
                Unevenness = 2500,
                EntryFee = 70,
                AvailableSlots = 500,
                Status = "open",
                Category = "Trail Medio",
                Gender = "m"
            },
            new TrailRunning
            {
                Name = "Costa Blanca Trails",
                Description = "Recorrido por la Sierra de Aitana y Puig Campana con vistas increíbles al Mediterráneo.",
                Date = DateTime.Now.AddMonths(5),
                DistanceKm = 101,
                Location = "Finestrat, Alicante",
                Coordinates = "38.5606, -0.2164",
                Unevenness = 6000,
                EntryFee = 115,
                AvailableSlots = 450,
                Status = "open",
                Category = "Ultra Trail",
                Gender = "f"
            },
            new TrailRunning
            {
                Name = "Ultramediterrània: Terres de Trail",
                Description = "Una gran prueba invernal cruzando los pueblos rurales y las montañas del interior de Alicante.",
                Date = DateTime.Now.AddMonths(6),
                DistanceKm = 46,
                Location = "Alcoi, Alicante",
                Coordinates = "38.6975, -0.4739",
                Unevenness = 2300,
                EntryFee = 60,
                AvailableSlots = 600,
                Status = "open",
                Category = "Maratón de Montaña",
                Gender = "all"
            }
        };
        context.TrailRunnings.AddRange(races);
        context.SaveChanges();
    }
}
