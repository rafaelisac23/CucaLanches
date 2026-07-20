using CucaLanches.Domain.Entities;
using CucaLanches.Domain.Enums;

namespace CucaLanches.Infrastructure;

public static class DataSeeder
{

    public static async Task SeedAsync(AppDbContext db)
    {

        if (!db.Users.Any())
        {
            db.Users.Add(new User
            {
                Name =  "Admin",
                Email = "Admin@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
            });

            if (!db.StoreSettings.Any())
            {
                db.StoreSettings.Add(new StoreSettings
                {
                    IsOpen =  true
                });
            }

            if (!db.Neighborhoods.Any())
            {
              db.AddRange( 
                  new Neighborhood { Name = "Centro", DeliveryFee = 5.00m },
                  new Neighborhood { Name = "Jardim América", DeliveryFee = 8.00m }
                  );
            }

            if (!db.Products.Any())
            {
                db.Products.AddRange(
                    new Product { Name = "X-Burguer", Type = ProductType.Lunch, Price = 18.00m },
                    new Product { Name = "Coca-Cola Lata", Type = ProductType.Drink, Price = 6.00m },
                    new Product { Name = "Batata Frita", Type = ProductType.Portion, Price = 12.00m });
            }

            
            
        }

        await db.SaveChangesAsync();
    }
    
}