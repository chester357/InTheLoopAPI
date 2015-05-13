namespace InTheLoopAPI.Migrations
{
    using InTheLoopAPI.Migrations.Seeds;
    using InTheLoopAPI.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<DatabaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(DatabaseContext context)
        {
            //  This method will be called after migrating to the latest version.

            //BaseEventSeed.Run(context);
            UserSeed.Run(context);

            context.SaveChanges();
        }
    }
}
