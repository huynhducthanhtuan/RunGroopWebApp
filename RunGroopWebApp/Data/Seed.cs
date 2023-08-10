using Microsoft.AspNetCore.Identity;
using RunGroopWebApp.Data.Enum;
using RunGroopWebApp.Models;

namespace RunGroopWebApp.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                context.Database.EnsureCreated();

                // Clubs
                if (!context.Clubs.Any())
                {
                    context.Clubs.AddRange(new List<Club>()
                    {
                        new Club()
                        {
                            Title = "Bar Elba",
                            Image = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/18/58/e1/ae/bienvenue-au-paradis.jpg?w=1000&h=800&s=1",
                            Description = "Napoleon-themed rooftop bar in London Waterloo serving cocktails. Daily happy hours, perfect for after-work drinks, birthday and much more. With daily happy hours and events, there are something for everyone. Check out our rooftop movies, brunch and sunset sessions. Covered in the winter, its perfect all year round.",
                            ClubCategory = ClubCategory.City,
                            AddressId = 1,
                            Address = new Address()
                            {
                                Street = "10 Sans",
                                City = "Charlotte",
                                State = "NC"
                            }
                         },
                        new Club()
                        {
                            Title = "Be At One Greek Street",
                            Image = "https://dynamic-media-cdn.tripadvisor.com/media/photo-o/07/3d/ce/28/be-at-one.jpg?w=900&h=700&s=1",
                            Description = "A cocktail institution! Join us for over 100 amazing cocktails and the best party around! Open until 3am, Greek Street has all you need for your perfect celebration! Our specially trained bar tenders and DJs will take you through the evening!",
                            ClubCategory = ClubCategory.Endurance,
                            AddressId = 2,
                            Address = new Address()
                            {
                                Street = "20 Tons",
                                City = "Paris",
                                State = "LC"
                            }
                        },
                        new Club()
                        {
                            Title = "Alcotraz London",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "Inspired by Hollywood Movies and TV that have brought to life infamous prison stories, Alcotraz is London first immersive theatrical cocktail bar. When convicted to Alcotraz, guests, or more appropriately, inmates, are taken into an exciting world of mischief as you smuggle-in liquor to be used to make delicious, bespoke cocktails.",
                            ClubCategory = ClubCategory.Trail,
                            AddressId = 3,
                            Address = new Address()
                            {
                                Street = "30 Chelsea",
                                City = "London",
                                State = "FC"
                            }
                        }
                    });
                    context.SaveChanges();
                }

                // Races
                if (!context.Races.Any())
                {
                    context.Races.AddRange(new List<Race>()
                    {
                        new Race()
                        {
                            Title = "Running Race 1",
                            Image = "https://res.cloudinary.com/letsdothiscom/image/upload/f_auto,c_limit,w_2048,q_auto/grc-search-cards/2023-24/GSR-Landscape.jpg",
                            Description = "AJ Bell Great Scottish Run 2023",
                            RaceCategory = RaceCategory.Marathon,
                            AddressId = 4,
                            Address = new Address()
                            {
                                Street = "123 Main St",
                                City = "Charlotte",
                                State = "NC"
                            }
                        },
                        new Race()
                        {
                            Title = "Running Race 2",
                            Image = "https://www.eatthis.com/wp-content/uploads/sites/4/2020/05/running.jpg?quality=82&strip=1&resize=640%2C360",
                            Description = "Wizz Air Hackney Half 2024",
                            RaceCategory = RaceCategory.Ultra,
                            AddressId = 5,
                            Address = new Address()
                            {
                                Street = "20 Fulham",
                                City = "London",
                                State = "TC"
                            }
                        },
                        new Race()
                        {
                            Title = "Running Race 3",
                            Image = "https://res.cloudinary.com/letsdothiscom/image/upload/f_auto,c_limit,w_2048,q_auto/v1627481251/category-assets/immersive-images/210614_Lets_Do_This-12496.jpg",
                            Description = "2023 Vitality London 10,000",
                            RaceCategory = RaceCategory.FiveK,
                            AddressId = 6,
                            Address = new Address()
                            {
                                Street = "45 Couple",
                                City = "Brentford",
                                State = "FC"
                            }
                        }
                    });
                    context.SaveChanges();
                }
            }
        }

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                // Roles
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
                if (!await roleManager.RoleExistsAsync(UserRoles.User))
                    await roleManager.CreateAsync(new IdentityRole(UserRoles.User));

                // Users
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
                string adminUserEmail = "thanhtuan@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "thanhtuan",
                        Email = adminUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "200 Ta Hien",
                            City = "Ha Noi",
                            State = "VN"
                        }
                    };
                    await userManager.CreateAsync(newAdminUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAdminUser, UserRoles.Admin);
                }

                string appUserEmail = "user@etickets.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "app-user",
                        Email = appUserEmail,
                        EmailConfirmed = true,
                        Address = new Address()
                        {
                            Street = "123 Main St",
                            City = "Charlotte",
                            State = "NC"
                        }
                    };
                    await userManager.CreateAsync(newAppUser, "Coding@1234?");
                    await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
                }
            }
        }
    }
}