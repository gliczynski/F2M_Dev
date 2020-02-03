using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
  public enum _EnSex : int
  {
    Male = 1,
    Female = 2,
  }

  public enum _EnOriginal : int
  {
    Original = 1,
    Fake = 2,
  }

  public enum _EnState : int
  {
    New = 1,
    Used = 2,
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

  public static class _LogActionType
  {
    //Accounts
    public const string AccountCreated = "AccountCreated";
    public const string AccountDeleted = "AccountDeleted";
    public const string AccountLocked = "AccountLocked";
    public const string AccountUnlocked = "AccountUnlocked";
    public const string AccountSuspended = "AccountSuspended";

    //Login Logout
    public const string UserLogin = "UserLogin";
    public const string UserLogout = "UserLogout";

    //Profile 
    public const string ProfileUpdate = "ProfileUpdated";
    public const string ProfileImageUpdated = "ProfileImageUpdated";
    public const string ProfileNewImage = "ProfileNewImage";

    //Follow / Unfollow
    public const string Follow = "Follow";
    public const string UnFollow = "UnFollow";



    //Ads
    public const string AdDrafted = "AdDrafted";
    public const string AdUpdated = "AdUpdated";
    public const string AdPublished = "AdPublished";
    public const string AdRemoved = "AdRemoved";
    public const string AdReported = "AdReported";

    public static string GetActionMessage(string actionType)
    {
      switch (actionType)
      {
        case AccountCreated:
          return "create an account through";
        case UserLogin:
          return "logged in";
        case UserLogout:
          return "logged out";
        case ProfileUpdate:
          return "updated his/her profile data";
        case ProfileImageUpdated:
          return "updated the profile image";
        case ProfileNewImage:
          return "uploaded a new profile image";
        case Follow:
          return "is now following";
        case UnFollow:
          return "has stop following";
        default:
          return null;
      }
    }
  }

  public class _FileSavingPaths
  {
    public static string ProfileImage = "/images/user/profile";
    public static string UserAdsPath = "/images/user/ad";
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
    public static string PreferredLanguage = "PreferredLanguage";
  }
}
