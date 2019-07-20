using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace API.Model
{
    static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TnRContext(
                serviceProvider.GetRequiredService<DbContextOptions<TnRContext>>()))
            {
                InitUsers(context);
                InitProjects(context);
                context.SaveChanges();
            }
        }

        static void InitUsers(TnRContext context) {
            if (context.Users.Any()) return;

            context.Users.Add(
                new User
                {
                    Name = "Admin",
                    Password = "123456"
                });
        }

        static void InitProjects(TnRContext context)
        {
            if (context.Projects.Any()) return;

            context.Projects.Add(
                new Project
                {
                    Name = "Secret Project",
                    Description = "classified",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                });

            context.Projects.Add(
                new Project
                {
                    Name = "Undercover Project",
                    Description = "confidential",
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                });
        }


    }
}
