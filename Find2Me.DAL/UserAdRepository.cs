using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
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

    public UserAdInformation GetUserAdInfoByAdId(int adID)
    {
      return dbContext.UserAdInformation.FirstOrDefault(w => w.AdID == adID);
    }

    public UserAdPriceReward GetUserAdPriceRewardByAdId(int adID)
    {
      return dbContext.UserAdPriceReward.FirstOrDefault(w => w.AdID == adID);
    }

    public int CreateUserAd(UserAd userAd)
    {
      dbContext.UserAds.Add(userAd);

      dbContext.SaveChanges();

      return userAd.Id;
    }

    public long AddUserAdImage(UserAdImage userAdImage)
    {
      dbContext.UserAdImages.Add(userAdImage);
      dbContext.SaveChanges();

      return userAdImage.Id;
    }

    public UserAdImage GetUserAdImageByAdImageId(int imageId)
    {
      return dbContext.UserAdImages.FirstOrDefault(x => x.Id == imageId);
    }

    public int DeleteUserAdImage(int imageId)
    {
      var dbItem = dbContext.UserAdImages.FirstOrDefault(x => x.Id == imageId);

      if (dbItem != null)
      {
        dbContext.UserAdImages.Remove(dbItem);
        dbContext.SaveChanges();

        return imageId;
      }

      return -1;
    }

    public void UpdateUserAdInfo(UserAdInformationVM userAdInfoVM)
    {
      // get boolean  if user information already exists or not
      var dbItem = dbContext.UserAdInformation.FirstOrDefault(x => x.AdID == userAdInfoVM.AdID);
      if (dbItem != null)
      {
        dbItem.AdID = userAdInfoVM.AdID;
        dbItem.IsOriginal = userAdInfoVM.IsOriginal;
        dbItem.IsState = userAdInfoVM.IsState;
        dbItem.Headline = userAdInfoVM.Headline;
        dbItem.Description = userAdInfoVM.Description;
        dbItem.Tags = "N/A";
        dbItem.CategoryID = userAdInfoVM.CategoryId;

        dbContext.SaveChanges();
      }
      else
      {
        // if user ad info does not exits then save it in db
        var userAdInfo = new UserAdInformation()
        {
          AdID = userAdInfoVM.AdID,
          IsOriginal = userAdInfoVM.IsOriginal,
          IsState = userAdInfoVM.IsState,
          Headline = userAdInfoVM.Headline,
          Description = userAdInfoVM.Description,
          Tags = "N/A",
          CategoryID = userAdInfoVM.CategoryId,
        };

        dbContext.UserAdInformation.Add(userAdInfo);
        dbContext.SaveChanges();
      }
    }

    public void UpdateUserAdPriceReward(UserAdPriceRewardVM userAdPriceReward)
    {
      // get boolean  if user information already exists or not
      var dbItem = dbContext.UserAdPriceReward.FirstOrDefault(x => x.AdID == userAdPriceReward.AdID);
      if (dbItem != null)
      {
        dbItem.AdID = userAdPriceReward.AdID;
        dbItem.PriceCategoryId = userAdPriceReward.PriceCategoryId;
        dbItem.Price = userAdPriceReward.Price;
        dbItem.IsFree = userAdPriceReward.IsFree;
        dbItem.PriceMin = userAdPriceReward.PriceMin;
        dbItem.PriceMax = userAdPriceReward.PriceMax;
        dbItem.AdStartDate = userAdPriceReward.AdStartDate;
        dbItem.AdEndDate = userAdPriceReward.AdEndDate;
        dbItem.AdDuration = userAdPriceReward.AdDuration;
        dbItem.IsReward = userAdPriceReward.IsReward;
        dbItem.RewardAmount = userAdPriceReward.RewardAmount;
        dbItem.RewardExpiration = userAdPriceReward.RewardExpiration;
        dbItem.IsExclusiveAd = userAdPriceReward.IsExclusiveAd;
        dbItem.ExclusiveAdPeriod = userAdPriceReward.ExclusiveAdPeriod;

        dbContext.SaveChanges();
      }
      else
      {
        var newUserAdPriceReward = new UserAdPriceReward
        {
          AdID = userAdPriceReward.AdID,
          PriceCategoryId = userAdPriceReward.PriceCategoryId,
          Price = userAdPriceReward.Price,
          IsFree = userAdPriceReward.IsFree,
          PriceMin = userAdPriceReward.PriceMin,
          PriceMax = userAdPriceReward.PriceMax,
          AdStartDate = userAdPriceReward.AdStartDate,
          AdEndDate = userAdPriceReward.AdEndDate,
          AdDuration = userAdPriceReward.AdDuration,
          IsReward = userAdPriceReward.IsReward,
          RewardAmount = userAdPriceReward.RewardAmount,
          RewardExpiration = userAdPriceReward.RewardExpiration,
          IsExclusiveAd = userAdPriceReward.IsExclusiveAd,
          ExclusiveAdPeriod = userAdPriceReward.ExclusiveAdPeriod,
        };

        dbContext.UserAdPriceReward.Add(newUserAdPriceReward);
        dbContext.SaveChanges();
      }
    }
  }
}
