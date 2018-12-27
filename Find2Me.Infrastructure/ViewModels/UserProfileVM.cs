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

        [Display(Name = "Email address")]
        [Required(ErrorMessage ="Please provide a valid email address.")]
        [EmailAddress(ErrorMessage = "Please provide a valid email address.")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Please provide a valid email address.")]
        public string Email { get; set; }

        public string UserName { get; set; }

        [Display(Name = "Fullname")]
        [Required(ErrorMessage = "Please provide a valid full name.")]
        [MinLength(2, ErrorMessage = "Full name should contain minimum 2 characters.")]
        public string FullName { get; set; }

        [Display(Name = "Year of birth")]
        [Required(ErrorMessage = "Please provide a valid year of birth.")]
        public int YearOfBirth { get; set; }

        [Display(Name = "Sex")]
        [Required(ErrorMessage = "Please provide a valid sex.")]
        public _EnSex Sex { get; set; }

        public CurrencyVM Currency { get; set; }
        [Display(Name = "Preferred currency")]
        [Required(ErrorMessage = "Please provide a valid currency.")]
        public string PreferredCurrency { get; set; }

        [Display(Name = "Preferred language")]
        [Required(ErrorMessage = "Please provide a valid language.")]
        public string PreferredLanguage { get; set; }

        //Username for URL
        [Display(Name = "Username")]
        [Required(ErrorMessage = "Please provide a valid username.")]
        [MinLength(5, ErrorMessage = "Username must contains atleast 5 characters.")]
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
