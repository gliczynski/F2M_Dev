using AutoMapper;
using Find2Me.DAL;
using Find2Me.Infrastructure;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Services
{

    public interface IUserAccountService
    {
        bool UserExists(string currentUserIdToExclude, string username);
        UserProfileVM GetUserProfile(string username);
        UserProfileVM GetUserProfileById(string id);
        UserProfilePictureVM GetUserProfilePicture(string id);
        UserProfileVM UpdateUserProfile(UserProfileVM userProfileVM);
        List<string> GetUserSuggestion(string username, int suggestionToTake = 3);
    }

    public class UserAccountService : IUserAccountService
    {
        private ApplicationUserRepository applicationUserRepository;
        public UserAccountService(ApplicationDbContext dbContext)
        {
            applicationUserRepository = new ApplicationUserRepository(dbContext);
        }

        public bool UserExists(string currentUserIdToExclude, string username)
        {
            ApplicationUser applicationUser = applicationUserRepository.UserExists(currentUserIdToExclude, username);
            if (applicationUser != null)
            {
                return true;
            }
            return false;
        }

        public UserProfileVM GetUserProfile(string username)
        {
            ApplicationUser applicationUser = applicationUserRepository.GetUserByUsername(username);
            if (applicationUser != null)
            {
                UserProfileVM userProfileVM = new UserProfileVM();
                Mapper.Map(applicationUser, userProfileVM);
                return userProfileVM;
            }
            return null;
        }

        public UserProfileVM GetUserProfileById(string id)
        {
            ApplicationUser applicationUser = applicationUserRepository.GetSingle(id);
            if (applicationUser != null)
            {
                UserProfileVM userProfileVM = new UserProfileVM();
                Mapper.Map(applicationUser, userProfileVM);
                return userProfileVM;
            }
            return null;
        }

        public UserProfilePictureVM GetUserProfilePicture(string id)
        {
            ApplicationUser applicationUser = applicationUserRepository.GetProfilePicture(id);
            if (applicationUser != null)
            {
                UserProfilePictureVM userProfilePictureVM = new UserProfilePictureVM();
                Mapper.Map(applicationUser, userProfilePictureVM);
                return userProfilePictureVM;
            }
            return null;
        }

        public UserProfileVM UpdateUserProfile(UserProfileVM userProfileVM)
        {
            ApplicationUser applicationUser = applicationUserRepository.GetSingle(userProfileVM.Id);
            if (applicationUser != null)
            {
                applicationUser.PreferredCurrency = userProfileVM.PreferredCurrency;
                applicationUser.PreferredLanguage = userProfileVM.PreferredLanguage;
                applicationUser.Sex = userProfileVM.Sex;
                applicationUser.UpdatedOn = DateTime.UtcNow;
                applicationUser.UrlUsername = userProfileVM.UrlUsername;
                applicationUser.YearOfBirth = userProfileVM.YearOfBirth;
                applicationUser.PreferredCurrency = userProfileVM.PreferredCurrency;
                applicationUser.PreferredLanguage = userProfileVM.PreferredLanguage;
                applicationUserRepository.Update(applicationUser);
                applicationUserRepository.SaveChanges();

                userProfileVM.Id = applicationUser.Id;
                return userProfileVM;
            }

            return null;
        }

        public List<string> GetUserSuggestion(string username, int suggestionToTake = 3)
        {
            List<string> filteredSuggestion = new List<string>();
            Random _rdm = new Random();

            GOTO_RetrySuggestions:
            //Generate User Suggestions
            List<String> userSuggestions = new List<string>();
            userSuggestions.Add(username + _rdm.Next(1000, 9999));
            userSuggestions.Add(username + _rdm.Next(1000, 9999));
            userSuggestions.Add(username + _rdm.Next(1000, 9999));
            userSuggestions.Add(username + _rdm.Next(1000, 9999));
            userSuggestions.Add(username + _rdm.Next(1000, 9999));

            //Check All of these in DB
            filteredSuggestion.AddRange(applicationUserRepository.CheckUserSuggestion(userSuggestions));

            //If atleast 3 suggestion are filtered/found, then return
            //otherwise retry and find suggestions
            if (filteredSuggestion.Count < 3)
            {
                goto GOTO_RetrySuggestions;
            }

            return filteredSuggestion.Take(suggestionToTake).ToList();
        }

        public UserProfilePictureVM UpdateUserProfileImage(UserProfilePictureVM userProfilePictureVM, bool updateCropData = true)
        {
            ApplicationUser applicationUser = applicationUserRepository.GetProfilePicture(userProfilePictureVM.Id);
            if (applicationUser != null)
            {
                applicationUser.ProfileImageOriginal = userProfilePictureVM.ProfileImageOriginal;
                applicationUser.ProfileImageSelected = userProfilePictureVM.ProfileImageSelected;
                
                //If there is profile Data, then updateit. 
                if (userProfilePictureVM.ProfileImageData != null && updateCropData)
                {
                    if (applicationUser.ProfileImageData == null)
                    {
                        applicationUser.ProfileImageData = new UserProfileImageData();
                    }

                    applicationUser.ProfileImageData.LastUpdated = DateTime.UtcNow;
                    applicationUser.ProfileImageData.CanvasData_Height = userProfilePictureVM.ProfileImageData.CanvasData_Height;
                    applicationUser.ProfileImageData.CanvasData_Left = userProfilePictureVM.ProfileImageData.CanvasData_Left;
                    applicationUser.ProfileImageData.CanvasData_NaturalHeight = userProfilePictureVM.ProfileImageData.CanvasData_NaturalHeight;
                    applicationUser.ProfileImageData.CanvasData_NaturalWidth = userProfilePictureVM.ProfileImageData.CanvasData_NaturalWidth;
                    applicationUser.ProfileImageData.CanvasData_Top = userProfilePictureVM.ProfileImageData.CanvasData_Top;
                    applicationUser.ProfileImageData.CanvasData_Width = userProfilePictureVM.ProfileImageData.CanvasData_Width;
                    applicationUser.ProfileImageData.CropBoxData_Height = userProfilePictureVM.ProfileImageData.CropBoxData_Height;
                    applicationUser.ProfileImageData.CropBoxData_Left = userProfilePictureVM.ProfileImageData.CropBoxData_Left;
                    applicationUser.ProfileImageData.CropBoxData_Top = userProfilePictureVM.ProfileImageData.CropBoxData_Top;
                    applicationUser.ProfileImageData.CropBoxData_Width = userProfilePictureVM.ProfileImageData.CropBoxData_Width;
                    applicationUser.ProfileImageData.UserId = userProfilePictureVM.Id;
                }
                else
                {
                    applicationUser.ProfileImageData = null;
                }

                applicationUserRepository.Update(applicationUser);
                applicationUserRepository.SaveChanges();
                return userProfilePictureVM;
            }
            return null;
        }

    }
}
