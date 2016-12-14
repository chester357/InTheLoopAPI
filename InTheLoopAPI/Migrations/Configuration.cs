namespace InTheLoopAPI.Migrations
{
    using InTheLoopAPI.Migrations.Seeds;
    using InTheLoopAPI.Models;
    using Models.Database;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Models.Request;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            // Add Default Loopstir User

            var existingDefaultUser = context.MyUsers.SingleOrDefault(x => x.UserName == "loopstir");

            if (existingDefaultUser == null)
            {
                var userStore = new UserStore<ApplicationUser>(context);
                var userManager = new UserManager<ApplicationUser>(userStore);
                var userToInsert = new User { UserName = "loopstir", Email = "chester35711@gmail.com" };
                userManager.Create(userToInsert, "brutloop");

                context.SaveChanges();
            }

            existingDefaultUser = context.MyUsers.Single(x => x.UserName == "Loopstir");

            // Add Starter Loops

            var starterLoops = LoopModel.GetStarterLoops();

            var firstName = starterLoops.ElementAt(0).Name;

            var starterLoopsExist = context.Loops.Any(x => x.Name == firstName);

            if (!starterLoopsExist)
            {
                foreach (var loop in starterLoops)
                {
                    loop.UserId = existingDefaultUser.Id;
                    context.Loops.Add(loop);
                }

                context.SaveChanges();
            }
        }
    }
}
