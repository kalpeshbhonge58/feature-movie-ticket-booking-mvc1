using CineBookerEnterprise.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CineBookerEnterprise.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            context.Database.Migrate();

            // Seed Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Seed Admin User
            var adminEmail = "admin@cinebooker.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "System Admin",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            // Seed Movies
            if (!context.Movies.Any())
            {
                var movies = new List<Movie>
                {
                    new Movie { Title = "Inception", Genre = "Sci-Fi", DurationInMinutes = 148, Description = "A thief who steals corporate secrets through the use of dream-sharing technology.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg" },
                    new Movie { Title = "The Dark Knight", Genre = "Action", DurationInMinutes = 152, Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_.jpg" },
                    new Movie { Title = "Interstellar", Genre = "Sci-Fi", DurationInMinutes = 169, Description = "A team of explorers travel through a wormhole in space in an attempt to ensure humanity's survival.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BZjdkOTU3MDktN2IxOS00OGEyLWFmMjktY2FiMmZkNWIyODZiXkEyXkFqcGdeQXVyMTMxODk2OTU@._V1_.jpg" },
                    new Movie { Title = "The Matrix", Genre = "Action", DurationInMinutes = 136, Description = "A computer hacker learns from mysterious rebels about the true nature of his reality.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BNzQzOTk3OTAtNDQ0Zi00ZTVkLWI0MTEtMDllZjNkYzNjNTc4L2ltYWdlXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" },
                    new Movie { Title = "Gladiator", Genre = "Action", DurationInMinutes = 155, Description = "A former Roman General sets out to exact vengeance against the corrupt emperor who murdered his family.", PosterUrl = "https://m.media-amazon.com/images/M/MV5BMDliMmNhNDEtODUyOS00MjNlLTgxODEtN2U3NzcyMGFlZTAyXkEyXkFqcGdeQXVyNjU0OTQ0OTY@._V1_.jpg" }
                };
                context.Movies.AddRange(movies);
                context.SaveChanges();
            }

            // Seed Theater and Screens
            if (!context.Theaters.Any())
            {
                var theater = new Theater { Name = "Grand Cinema", Location = "Downtown Metro" };
                context.Theaters.Add(theater);
                context.SaveChanges();

                var screen1 = new Screen { Name = "Screen 1", TheaterId = theater.Id, Capacity = 100 };
                var screen2 = new Screen { Name = "Screen 2", TheaterId = theater.Id, Capacity = 80 };
                context.Screens.AddRange(screen1, screen2);
                context.SaveChanges();

                // Seed Seats for Screen 1 (10x10)
                for (int i = 0; i < 10; i++)
                {
                    char row = (char)('A' + i);
                    for (int j = 1; j <= 10; j++)
                    {
                        context.Seats.Add(new Seat { Row = row.ToString(), Number = j, ScreenId = screen1.Id });
                    }
                }

                // Seed Seats for Screen 2 (8x10)
                for (int i = 0; i < 8; i++)
                {
                    char row = (char)('A' + i);
                    for (int j = 1; j <= 10; j++)
                    {
                        context.Seats.Add(new Seat { Row = row.ToString(), Number = j, ScreenId = screen2.Id });
                    }
                }
                context.SaveChanges();

                // Seed Shows
                var movie1 = context.Movies.First();
                var show = new Show
                {
                    MovieId = movie1.Id,
                    ScreenId = screen1.Id,
                    StartTime = DateTime.Now.AddHours(5),
                    Price = 12.50m
                };
                context.Shows.Add(show);
                context.SaveChanges();
            }
        }
    }
}
