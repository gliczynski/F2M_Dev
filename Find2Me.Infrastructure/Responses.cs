using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    public class UserValidationSuggestions
    {
        public bool UserExists { get; set; }

        public List<string> UserSuggestion { get; set; }
    }

    public class ResponseResult<T>
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string MessageCode { get; set; }

        public string SuccessCode { get; set; }

        public T Data { get; set; }
    }

    public static class ResponseResultMessageCode
    {
        public const string UserNameExists = "UserNameExists";
        public const string EmailExists = "EmailExists";
        public const string UserNotFound = "UserNotFound";
        public const string EmailNotConfirmed = "EmailNotConfirmed";
        public const string UserNameUpdated = "UserNameUpdated";
        public const string EmailUpdated = "EmailUpdated";
        public const string UserNameAndEmailUpdated = "UserNameAndEmailUpdated";
        public const string ProfileImageUploaded = "ProfileImageUploaded";

        public static string GetMessageFromCode(string message)
        {
            switch (message)
            {
                case UserNameExists:
                    return "A User exists with a similar username";

                case EmailExists:
                    return "A User exists with a similar email address.";

                case UserNotFound:
                    return "User does not exists in the system.";

                default:
                    return "An error occured while processing your request.";
            }
        }
    }
}
