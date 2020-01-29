using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Find2Me.Infrastructure.DbModels
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        //Extra User Fields
        public string FullName { get; set; }

        public int YearOfBirth { get; set; }

        public _EnSex Sex { get; set; }

        [ForeignKey("PreferredCurrency")]
        public Currency Currency { get; set; }
        public string PreferredCurrency { get; set; }

        public string PreferredLanguage { get; set; }

        //Username for URL
        public string UrlUsername { get; set; }

        public string ProfileImageOriginal { get; set; }

        public string ProfileImageSelected { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public UserProfileImageData ProfileImageData { get; set; }

        //public ICollection<UserFollower> UserFollowed { get; set; }
        //public ICollection<UserFollower> UserFollowedBy { get; set; }
    }

    public class UserProfileImageData
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public float CropBoxData_Left { get; set; }
        public float CropBoxData_Top { get; set; }
        public float CropBoxData_Width { get; set; }
        public float CropBoxData_Height { get; set; }

        public float CanvasData_Left { get; set; }
        public float CanvasData_Top { get; set; }
        public float CanvasData_Width { get; set; }
        public float CanvasData_Height { get; set; }
        public float CanvasData_NaturalWidth { get; set; }
        public float CanvasData_NaturalHeight { get; set; }

        public DateTime LastUpdated { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<UserProfileImageData> UserProfileImageDatas { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<UserFollower> UserFollowers { get; set; }

        public DbSet<UserAd> UserAds { get; set; }
        public DbSet<UserAdImage> UserAdImages { get; set; }
        public DbSet<UserAdInformation> UserAdInformation { get; set; }
        public DbSet<UserAdPriceReward> UserAdPriceReward { get; set; }

        public DbSet<Logs> Logs { get; set; }

        public DbSet<Configuration> Configurations { get; set; }

        public DbSet<UserConfiguration> UserConfigurations { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}