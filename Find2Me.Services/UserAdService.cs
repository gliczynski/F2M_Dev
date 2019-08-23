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

        public int CreateAd(UserAdVM userAd)
        {
            var newlyCraetedAdID = _userAdRepository.CreateUserAd(new UserAd
            {
                CreatedOn = userAd.CreatedOn,
                UserId = userAd.UserId
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

        public void AddUserAdImage(UserAdImageVM userAdImageVM)
        {
            _userAdRepository.AddUserAdImage(new UserAdImage
            {
                AdID = userAdImageVM.AdID,
                AdImageOriginal = userAdImageVM.AdImageOriginal,
                AdImageSelected = userAdImageVM.AdImageSelected,
                ImageNumber = userAdImageVM.ImageNumber,
                CreatedOn = userAdImageVM.CreatedOn
            });
            _userAdRepository.SaveChanges();

        }
    }
}
