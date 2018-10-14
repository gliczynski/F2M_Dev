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
        ResponseResult<ApplicationUser> UserExists(string currentUserIdToExclude, string username, string email);
        UserProfileVM GetUserProfile(string username);
        UserProfileVM GetUserProfileById(string id);
        UserProfilePictureVM GetUserProfilePicture(string id);
        UserProfileImageDataVM GetUserProfileImageData(string id);
        ResponseResult<UserProfileVM> UpdateUserProfile(UserProfileVM userProfileVM, bool skipValidation);
        List<string> GetUserSuggestion(string username, int suggestionToTake = 3);
    }

    public class UserAccountService : IUserAccountService
    {
        private ApplicationUserRepository applicationUserRepository;
        public UserAccountService(ApplicationDbContext dbContext)
        {
            applicationUserRepository = new ApplicationUserRepository(dbContext);
        }

        public ResponseResult<ApplicationUser> UserExists(string currentUserIdToExclude, string username, string email)
        {
            ApplicationUser applicationUser = applicationUserRepository.UserExists(currentUserIdToExclude, username, email);
            if (applicationUser != null)
            {
                if (applicationUser.Email.ToLower().Trim().Equals(email.ToLower().Trim()))
                {
                    return new ResponseResult<ApplicationUser> { Success = true, Data = applicationUser, MessageCode = ResponseResultMessageCode.EmailExists };
                }
                else
                {
                    return new ResponseResult<ApplicationUser> { Success = true, Data = applicationUser, MessageCode = ResponseResultMessageCode.UserNameExists };
                }
            }

            return new ResponseResult<ApplicationUser> { Success = false };
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

        public UserProfileImageDataVM GetUserProfileImageData(string id)
        {
            UserProfileImageData applicationUser = applicationUserRepository.GetProfileImageDataForUser(id);
            if (applicationUser != null)
            {
                UserProfileImageDataVM userProfilePictureVM = new UserProfileImageDataVM();
                Mapper.Map(applicationUser, userProfilePictureVM);
                return userProfilePictureVM;
            }
            return null;
        }

        /// <summary>
        /// Update User Profile Information
        /// </summary>
        /// <param name="userProfileVM">User Profile Data</param>
        /// <param name="skipValidation">Skip Email and Username Validation</param>
        /// <returns></returns>
        public ResponseResult<UserProfileVM> UpdateUserProfile(UserProfileVM userProfileVM, bool skipValidation)
        {
            ResponseResult<UserProfileVM> responseResult = new ResponseResult<UserProfileVM>
            {
                Success = true
            };

            ApplicationUser someExistedUser = applicationUserRepository.UserExists(userProfileVM.Id, userProfileVM.UrlUsername, userProfileVM.Email);
            if (someExistedUser != null)
            {
                if (skipValidation == false)
                {
                    if (someExistedUser.Email.ToLower().Trim().Equals(userProfileVM.Email.ToLower().Trim()))
                    {
                        responseResult.MessageCode = ResponseResultMessageCode.EmailExists;
                    }
                    if (someExistedUser.UrlUsername.ToLower().Trim().Equals(userProfileVM.UrlUsername.ToLower().Trim()))
                    {
                        responseResult.MessageCode = ResponseResultMessageCode.UserNameExists;
                    }
                    responseResult.Data = userProfileVM;
                    responseResult.Success = false;
                    return responseResult;
                }
                else
                {
                    if (someExistedUser.Email.ToLower().Trim().Equals(userProfileVM.Email.ToLower().Trim()))
                    {
                        userProfileVM.Email= null;
                    }
                    if (someExistedUser.UrlUsername.ToLower().Trim().Equals(userProfileVM.UrlUsername.ToLower().Trim()))
                    {
                        userProfileVM.UrlUsername= null;
                    }
                }
            }

            ApplicationUser applicationUser = applicationUserRepository.GetSingle(userProfileVM.Id);
            if (applicationUser != null)
            {
                applicationUser.PreferredCurrency = userProfileVM.PreferredCurrency;
                applicationUser.PreferredLanguage = userProfileVM.PreferredLanguage;
                applicationUser.Sex = userProfileVM.Sex;
                applicationUser.UrlUsername = userProfileVM.UrlUsername;
                applicationUser.YearOfBirth = userProfileVM.YearOfBirth;
                applicationUser.PreferredCurrency = userProfileVM.PreferredCurrency;
                applicationUser.PreferredLanguage = userProfileVM.PreferredLanguage;
                applicationUser.UpdatedOn = DateTime.UtcNow;
                if (!string.IsNullOrEmpty(userProfileVM.Email))
                {
                    applicationUser.Email = userProfileVM.Email;
                    responseResult.SuccessCode = ResponseResultMessageCode.EmailUpdated;
                }
                if (!string.IsNullOrEmpty(userProfileVM.UrlUsername))
                {
                    applicationUser.UrlUsername = userProfileVM.UrlUsername;
                    responseResult.SuccessCode = ResponseResultMessageCode.UserNameUpdated;
                }
                applicationUserRepository.Update(applicationUser);
                applicationUserRepository.SaveChanges();
                userProfileVM.Id = applicationUser.Id;
            }
            else
            {
                responseResult.Success = false;
                responseResult.MessageCode = ResponseResultMessageCode.UserNotFound;
                responseResult.MessageCode = ResponseResultMessageCode.UserNotFound;
            }

            responseResult.Data = userProfileVM;
            return responseResult;
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
