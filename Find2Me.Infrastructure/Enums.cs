using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    public enum _EnSex: int
    {
        Male = 1,
        Female = 2,

    }

    public class _UserRolesType
    {
        public static string User = "User";
        public static string SystemAdmin = "SystemAdmin";
        public static string SystemManager = "SystemManager";
    }

    public class _SupportedLocale
    {
        public static string English = "en";
        public static string Danish = "da";

        public static List<string> GetLocales()
        {
            return new List<string> {
                English, Danish
            };
        }
    }

    public class _LogActionType
    {
        //Accounts
        public static string AccountCreated = "AccountCreated";
        public static string AccountDeleted = "AccountDeleted";
        public static string AccountLocked = "AccountLocked";
        public static string AccountUnlocked = "AccountUnlocked";
        public static string AccountSuspended = "AccountSuspended";

        //Profile 
        public static string ProfileUpdate = "ProfileUpdated";
        public static string ProfileImageUpdated = "ProfileImageUpdated";
        public static string ProfileLiked = "ProfileLiked";

        //Follow / Unfollow
        public static string FollowStart = "FollowStart";
        public static string FollowStop = "FollowStop";

        //Login Logout
        public static string UserLogin = "UserLogin";
        public static string UserLogout = "UserLogout";

        //Ads
        public static string AdDrafted = "AdDrafted";
        public static string AdUpdated = "AdUpdated";
        public static string AdPublished = "AdPublished";
        public static string AdRemoved = "AdRemoved";
        public static string AdReported = "AdReported";

    }

    public class _FileSavingPaths
    {
        public static string ProfileImage = "/images/user/profile";
    }

    public class _CookieNameStrings
    {
        public static string IsProfileWizardCompleted = "IsProfileWizardCompleted";
    }

    public class _ClaimTypes
    {
        public static string UrlUserName = "UrlUserName";
        public static string ExternalProviderUsername = "ExternalProviderUsername";
        public static string ExternalProviderType = "ExternalProviderType";
        public static string ExternalProviderAccessToken = "ExternalProviderAccessToken";
        public static string HasCompletedProfileWizard = "HasCompletedProfileWizard";
    }
}
