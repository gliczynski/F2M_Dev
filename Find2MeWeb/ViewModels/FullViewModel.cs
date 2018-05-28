using System;

namespace Find2MeWeb.ViewModels
{
    public class FullViewModel
    {
        public FullViewModel()
        {
            CreationDateTime = DateTime.Now;
        }

        /// <summary>
        /// This will contain localised string value
        /// </summary>
        public string LocalisedString { get; set; }

        public string CreateAd { get; set; }

        /// <summary>
        /// For see difference of cretion time
        /// </summary>
        public DateTime CreationDateTime { get; set; }
    }
}