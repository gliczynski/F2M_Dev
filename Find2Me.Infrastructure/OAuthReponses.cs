using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
    /// <summary>
    /// Facebook User Data returned after OAuth
    /// </summary>
    public class OAuthReponseUserFacebook
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("first_name")]
        public string FirstName { get; set; }

        [JsonProperty("last_name")]
        public string LastName { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("picture")]
        public FbPictureData Picture { get; set; }

        /// <summary>
        /// Facebook Picture Data
        /// </summary>
        public class FbPictureData
        {
            [JsonProperty("data")]
            public FbPicture Data { get; set; }
        }

        /// Facebook Picture
        public class FbPicture
        {
            [JsonProperty("width")]
            public int Width { get; set; }

            [JsonProperty("height")]
            public int Height { get; set; }

            [JsonProperty("url")]
            public string URL { get; set; }

            [JsonProperty("is_silhouette")]
            public bool is_silhouette { get; set; }
        }
    }
    /// <summary>
    /// Google User Data returned after OAuth
    /// </summary>
    public class OAuthReponseUserGoogle
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string ETag { get; set; }

        [JsonProperty("emails")]
        public List<GoogleEmail> Emails { get; set; }

        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("name")]
        public GoogleName Name { get; set; }

        [JsonProperty("image")]
        public GoogleImage Image { get; set; }

        [JsonProperty("isPlusUser")]
        public bool IsPlusUser { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("verified")]
        public bool Verified { get; set; }

        public class GoogleEmail
        {
            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }
        }

        public class GoogleName
        {
            [JsonProperty("familyName")]
            public string FamilyName { get; set; }

            [JsonProperty("givenName")]
            public string GivenName { get; set; }
        }

        public class GoogleImage
        {
            [JsonProperty("url")]
            public string URL { get; set; }

            [JsonProperty("isDefault")]
            public bool IsDefault { get; set; }
        }
    }

    /// <summary>
    /// Instagram User Data returned after OAuth
    /// </summary>
    public class OAuthReponseUserInstagram
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("profile_picture")]
        public string ProfilePicture { get; set; }

        [JsonProperty("full_name")]
        public string FullName { get; set; }

        [JsonProperty("bio")]
        public string Bio { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        [JsonProperty("is_business")]
        public bool IsBusiness { get; set; }
    }

    /// <summary>
    /// Twitter User Data returned after OAuth
    /// </summary>
    public class OAuthReponseUserTwitter
    {
        [JsonProperty("id_str")]
        public string ID { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("lang")]
        public string Language { get; set; }

        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        [JsonProperty("default_profile_image")]
        public bool DefaultProfileImage { get; set; }
    }
}
