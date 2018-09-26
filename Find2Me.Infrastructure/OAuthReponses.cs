using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure
{
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
    }

    public class FbPictureData
    {
        [JsonProperty("data")]
        public FbPicture Data { get; set; }
    }

    public class FbPicture
    {
        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("url")]
        public string URL { get; set; }

        [JsonProperty("is_silhouette")]
        public bool is_silhouette  { get; set; }
    }
}
