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
    }

    public class UserAdService : IUserAdService
    {
        private UserAdRepository userAdRepository;
        public UserAdService(ApplicationDbContext dbContext)
        {
            userAdRepository = new UserAdRepository(dbContext);
        }

        public List<UserAdVM> GetAllUserAds(string user)
        {
            List<UserAd> userAds = userAdRepository.GetAll().ToList();
            List<UserAdVM> userAdsVM = new List<UserAdVM>();
            Mapper.Map(userAds, userAdsVM);
            return userAdsVM;
        }

        public void CreateAd(UserAdVM userAd)
        {
            userAdRepository.Insert(new UserAd
            {
                CreatedOn = userAd.CreatedOn,
                UserId = userAd.UserId
            });
            userAdRepository.SaveChanges();
        }
    }
}
