namespace Find2Me.Infrastructure.Migrations
{
  using Find2Me.Infrastructure.DbModels;
  using Microsoft.AspNet.Identity.EntityFramework;
  using System;
  using System.Collections.Generic;
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

      //Add Initial Currency Values
      var currencies = new Currency[]
      {
                new Currency { Code = "DKK", Name="Kroner", Country="Denmark", Symbol="kr", LastUpdated= DateTime.UtcNow },
                new Currency { Code = "EUR", Name="Euro", Country="Euro", Symbol="€", LastUpdated= DateTime.UtcNow },
        // Removed as specified in document
        //new Currency { Code = "USD", Name="Dollars", Country="America", Symbol="$", LastUpdated= DateTime.UtcNow },
      };
      context.Currencies.AddOrUpdate(x => x.Code, currencies);

      var configurations = new DbModels.Configuration[]
      {
                new DbModels.Configuration { Name = "wiz_ad_p1_min_image_upload_req", Value="1",Description="Minimum limit of images allowed for a ad to be added on step 1 of Ad Wizard", ValueType="int", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p1_max_images_allowed", Value="10",Description="Maximum limit of images allowed for a ad to be added on step 1 of Ad Wizard", ValueType="int", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_ad_price", Value="0.00",Description="Ad Price", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_one_day", Value="0.00",Description="Ad Exlusive Price Day One", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_two_days", Value="0.00",Description="Ad Exlusive Price Two Days", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_one_week", Value="0.00",Description="Ad Exlusive Price One Week", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_two_weeks", Value="0.00",Description="Ad Exlusive Price Two Weeks", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_one_month", Value="0.00",Description="Ad Exlusive Price One Month", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_two_months", Value="0.00",Description="Ad Exlusive Price Two Months", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_three_months", Value="0.00",Description="Ad Exlusive Price Three Months", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_six_months", Value="0.00",Description="Ad Exlusive Price Six Months", ValueType="double", CreateTime=DateTime.UtcNow },
                new DbModels.Configuration { Name = "wiz_ad_p3_exlusive_ad_one_year", Value="0.00",Description="Ad Exlusive Price One Year", ValueType="double", CreateTime=DateTime.UtcNow },
      };
      context.Configurations.AddOrUpdate(x => x.Name, configurations);

      var categories = new Category[]
      {
        new Category {Name = "Fashion"},
        new Category {Name = "Electronics"},
        new Category {Name = "Collectibles & Art"},
        new Category {Name = "Home & Garden"},
        new Category {Name = "Sporting Goods"},
        new Category {Name = "Motors"},
        new Category {Name = "Relation"},
      };
      context.Category.AddOrUpdate(x => x.Name, categories);
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
