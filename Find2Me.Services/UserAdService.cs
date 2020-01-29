using AutoMapper;
using Find2Me.DAL;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Services
{
  public interface IUserAdService
  {
    List<UserAdVM> GetAllUserAds(string user);
    List<UserAdImageVM> GetUserAdImages(int adID);
  }

  public class UserAdService : IUserAdService
  {
    private UserAdRepository _userAdRepository;
    public UserAdService(ApplicationDbContext dbContext)
    {
      _userAdRepository = new UserAdRepository(dbContext);
    }

    public List<UserAdVM> GetAllUserAds(string user)
    {
      List<UserAd> userAds = _userAdRepository.GetAll().ToList();
      List<UserAdVM> userAdsVM = new List<UserAdVM>();
      Mapper.Map(userAds, userAdsVM);
      return userAdsVM;
    }

    public UserAdInformationVM GetUserAdInfoByAdId(int adId)
    {
      UserAdInformation userAdsInfo = _userAdRepository.GetUserAdInfoByAdId(adId);
      UserAdInformationVM userAdsInfoVM = new UserAdInformationVM();
      Mapper.Map(userAdsInfo, userAdsInfoVM);
      return userAdsInfoVM;
    }

    public UserAdPriceRewardVM GetUserAdPriceRewardByAdId(int adId)
    {
      UserAdPriceReward userAdsPriceReward = _userAdRepository.GetUserAdPriceRewardByAdId(adId);
      UserAdPriceRewardVM userAdsPriceRewardVM = new UserAdPriceRewardVM();
      Mapper.Map(userAdsPriceReward, userAdsPriceRewardVM);
      return userAdsPriceRewardVM;
    }

    public int CreateAd(UserAdVM userAd)
    {
      var newlyCraetedAdID = _userAdRepository.CreateUserAd(new UserAd
      {
        CreatedOn = userAd.CreatedOn,
        UserId = userAd.UserId,
        IsDraft = userAd.IsDraft
      });

      return newlyCraetedAdID;
    }

    public List<UserAdImageVM> GetUserAdImages(int adID)
    {
      var userAdImages = _userAdRepository.GetUserAdImages(adID);

      var userAdImagesVM = new List<UserAdImageVM>();
      Mapper.Map(userAdImages, userAdImagesVM);
      return userAdImagesVM;
    }

    public long AddUserAdImage(UserAdImageVM userAdImageVM)
    {
      var imageId = _userAdRepository.AddUserAdImage(new UserAdImage
      {
        AdID = userAdImageVM.AdID,
        AdImageOriginal = userAdImageVM.AdImageOriginal,
        AdImageSelected = userAdImageVM.AdImageSelected,
        ImageNumber = userAdImageVM.ImageNumber,
        CreatedOn = userAdImageVM.CreatedOn
      });

      return imageId;

    }    
    
    public UserAdImageVM GetUserAdImageByAdImageId(int imageId)
    {
      var userAdImage = _userAdRepository.GetUserAdImageByAdImageId(imageId);

      var userAdImageVM = new UserAdImageVM();
      Mapper.Map(userAdImage, userAdImageVM);
      return userAdImageVM;

    }    
    
    public int DeleteUserAdImage(int imageId)
    {
      return _userAdRepository.DeleteUserAdImage(imageId);
    }

    public void UpdateUserAdInfo(UserAdInformationVM userAdInfoVM)
    {
      _userAdRepository.UpdateUserAdInfo(userAdInfoVM);
    }    
    public void UpdateUserAdPriceReward(UserAdPriceRewardVM userAdPriceRewardVM)
    {
      _userAdRepository.UpdateUserAdPriceReward(userAdPriceRewardVM);
    }
  }
}
