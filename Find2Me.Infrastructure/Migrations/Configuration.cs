namespace Find2Me.Infrastructure.Migrations
{
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Find2Me.Infrastructure.DbModels.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Find2Me.Infrastructure.DbModels.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //DB User Roles
            var roles = new IdentityRole[] {
                new IdentityRole { Name = _UserRolesType.SystemAdmin },
                new IdentityRole { Name = _UserRolesType.SystemManager },
                new IdentityRole { Name = _UserRolesType.User },
            };
            context.Roles.AddOrUpdate(x => x.Name, roles);

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
