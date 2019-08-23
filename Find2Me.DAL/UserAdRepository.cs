using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class UserAdRepository : DbRepository<UserAd>
    {
        public UserAdRepository(ApplicationDbContext context) : base(context)
        {
        }

        public List<UserAdImage> GetUserAdImages(int adID)
        {
            return dbContext.UserAdImages.Where(w => w.AdID == adID).ToList();
        }

        public int CreateUserAd(UserAd userAd)
        {
            dbContext.UserAds.Add(userAd);

            dbContext.SaveChanges();

            return userAd.Id;
        }

        public void AddUserAdImage(UserAdImage userAdImage)
        {
            dbContext.UserAdImages.Add(userAdImage);
        }
    }
}
