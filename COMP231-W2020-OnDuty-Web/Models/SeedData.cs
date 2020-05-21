using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using COMP231_W2020_OnDuty_Web.Models;

namespace COMP231_W2020_OnDuty_Web.Models
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app) 
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();

            context.Database.Migrate();

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category
                    {
                        Name = "Snow Removal",
                        Price = 50.00m

                    },
                    new Category
                    {
                        Name = "Lawn Mower",
                        Price = 100.00m

                    },
                    new Category
                    {
                        Name = "Dog Walking",
                        Price = 25.00m
                    });
            }

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "John Wick",
                        UserName = "John Wick",
                        Address = "900 Progress Ave, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.781839",
                        Longitude = "-79.234656",
                        Category = "Snow Removal",
                        Evaluation = 4.78,
                        numberEvaluations = 23
                    },
                    new User
                    {
                        Name = "John Constantine",
                        UserName = "John Constantine",
                        Address = "23 Wonderland Dr, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.771942",
                        Longitude = "-79.216125",
                        Category = "Dog Walking",
                        Evaluation = 5,
                        numberEvaluations = 13
                    },
                    new User
                    {
                        Name = "Kevin Lomax",
                        UserName = "Kevin Lomax",
                        Address = "20 Pineslope crescent, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.787832",
                        Longitude = "-79.216674",
                        Category = "Lawn Mower",
                        Evaluation = 3.98,
                        numberEvaluations = 32
                    },
                    new User
                    {
                        Name = "John McClane",
                        UserName = "John McClane",
                        Address = "775 Steeles Avenue West, Toronto, ON",
                        IsProvider = true,
                        Latitude = "43.779843",
                        Longitude = "-79.232485",
                        Category = "Snow Removal",
                        Evaluation = 2.97,
                        numberEvaluations = 1597
                    },
                    new User
                    {
                        Name = "Rocky Balboa",
                        UserName = "Rocky Balboa",
                        Address = "20 Teesdale Place, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.790653",
                        Longitude = "-79.238557",
                        Category = "Dog Walking",
                        Evaluation = 4.9,
                        numberEvaluations = 159
                    },
                    new User
                    {
                        Name = "Sarah Connor",
                        UserName = "Sarah Connor",
                        Address = "75 Gorsey Square, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.799020",
                        Longitude = "-79.225231",
                        Category = "Lawn Mower",
                        Evaluation = 4.93,
                        numberEvaluations = 15
                    },
                    new User
                    {
                        Name = "Clarice Starling",
                        UserName = "Clarice Starling",
                        Address = "73 Gorsey Square, Scarborough, ON",
                        IsProvider = true,
                        Latitude = "43.788202",
                        Longitude = "-79.236371",
                        Category = "Snow Removal",
                        Evaluation = 4.32,
                        numberEvaluations = 235
                    });
            }

            context.SaveChanges();
        }
    }
}
