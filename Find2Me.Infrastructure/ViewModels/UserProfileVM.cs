using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
    public class UserProfileVM
    {
        public string Id { get; set; }

        [Display(Name = "EmailAddress", ResourceType =typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName ="EmailRequired")]
        [EmailAddress(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "EmailRequired")]
        [DataType(DataType.EmailAddress, ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "EmailRequired")]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Fullname", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "FullNameRequired")]
        [RegularExpression(@"^((\b[a-zA-Z]{2,40}\b)\s*){2,}$", ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "FullNameRegex")]
        public string FullName { get; set; }

        [Display(Name = "YearOfBirth", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "YearOfBirthRequired")]
        public int YearOfBirth { get; set; }

        [Display(Name = "Sex", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "SexRequired")]
        public _EnSex Sex { get; set; }

        public CurrencyVM Currency { get; set; }
        [Display(Name = "PreferredCurrency", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "PreferredCurrencyRequired")]
        public string PreferredCurrency { get; set; }

        [Display(Name = "PreferredLanguage", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "PreferredLanguageRequired")]
        public string PreferredLanguage { get; set; }

        //Username for URL
        [Display(Name = "Username", ResourceType = typeof(Find2Me.Resources.Strings))]
        [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "UsernameRequired")]
        [MinLength(5, ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "UsernameMinLength")]
        public string UrlUsername { get; set; }

        public bool EmailConfirmed { get; set; }

        public string ProfileImageOriginal { get; set; }

        public string ProfileImageSelected { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }

        public UserProfileImageDataVM ProfileImageData { get; set; }
    }

    public class UserProfilePictureVM
    {
        public string Id { get; set; }

        public string FullName { get; set; }

        public string ProfileImageOriginal { get; set; }

        public string ProfileImageSelected { get; set; }

        public UserProfileImageDataVM ProfileImageData { get; set; }
    }

    public class UserProfileImageDataVM
    {
        public string UserId { get; set; }
        public UserProfileVM User { get; set; }

        public float CropBoxData_Left { get; set; }
        public float CropBoxData_Top { get; set; }
        public float CropBoxData_Width { get; set; }
        public float CropBoxData_Height { get; set; }

        public float CanvasData_Left { get; set; }
        public float CanvasData_Top { get; set; }
        public float CanvasData_Width { get; set; }
        public float CanvasData_Height { get; set; }
        public float CanvasData_NaturalWidth { get; set; }
        public float CanvasData_NaturalHeight { get; set; }

        public DateTime LastUpdated { get; set; }

        public CropperJsCropData GetCroppingData()
        {
            return new CropperJsCropData
            {
                ImageCanvasData = new CropperJsCropData.CanvasData
                {
                    height = CanvasData_Height,
                    left = CanvasData_Left,
                    naturalHeight = CanvasData_NaturalHeight,
                    naturalWidth = CanvasData_NaturalWidth,
                    top= CanvasData_Top,
                    width = CanvasData_Width,
                },

                ImageCropBoxData= new CropperJsCropData.CropBoxData
                {
                    height = CropBoxData_Height,
                    left = CropBoxData_Left,
                    top = CropBoxData_Top,
                    width = CropBoxData_Width,
                }
            };
        }

        public string GetCroppingDataString()
        {
            return JsonConvert.SerializeObject(GetCroppingData());
        }
    }

    public class CropperJsCropData
    {
        public CropBoxData ImageCropBoxData { get; set; }

        public CanvasData ImageCanvasData { get; set; }

        public class CropBoxData
        {
            public float left { get; set; }
            public float top { get; set; }
            public float width { get; set; }
            public float height { get; set; }
        }

        public class CanvasData
        {
            public float left { get; set; }
            public float top { get; set; }
            public float width { get; set; }
            public float height { get; set; }
            public float naturalWidth { get; set; }
            public float naturalHeight { get; set; }
        }

    }
}
