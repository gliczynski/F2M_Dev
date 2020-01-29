using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
  public class UserAdVM
  {
    public int Id { get; set; }

    public string UserId { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsDraft { get; set; }
  }

  public class UserAdImageVM
  {
    public UserAdImageVM()
    {
      UserAd = new UserAdVM();
    }

    public long Id { get; set; }

    public int AdID { get; set; }

    public int ImageNumber { get; set; }

    public UserAdVM UserAd { get; set; }

    public string AdImageOriginal { get; set; }

    public string AdImageSelected { get; set; }

    public DateTime CreatedOn { get; set; }
  }

  public class UserAdInformationVM
  {
    public UserAdInformationVM()
    {
      UserAd = new UserAdVM();
    }

    public long Id { get; set; }

    public int AdID { get; set; }

    public UserAdVM UserAd { get; set; }

    [Display(Name = "Category", ResourceType = typeof(Resources.Strings))]
    [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings),
      ErrorMessageResourceName = "CategoryRequired")]
    public int CategoryId { get; set; }

    [Display(Name = "Original", ResourceType = typeof(Find2Me.Resources.Strings))]
    [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "OriginalRequired")]
    public bool IsOriginal { get; set; }

    [Display(Name = "State", ResourceType = typeof(Find2Me.Resources.Strings))]
    [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "StateRequired")]
    public bool IsState { get; set; }


    [Display(Name = "Headline", ResourceType = typeof(Find2Me.Resources.Strings))]
    [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "HeadlineRequired")]
    public string Headline { get; set; }

    [Display(Name = "Description", ResourceType = typeof(Find2Me.Resources.Strings))]
    [Required(ErrorMessageResourceType = typeof(Find2Me.Resources.Strings), ErrorMessageResourceName = "HeadlineDescriptionRequired")]
    public string Description { get; set; }

    public string Tags { get; set; }
  }

  public class UserAdPriceRewardVM
  {
    public UserAdPriceRewardVM()
    {
      UserAd = new UserAdVM();
    }

    public long Id { get; set; }

    public int AdID { get; set; }

    public UserAdVM UserAd { get; set; }
    public int PriceCategoryId { get; set; }
    public double Price { get; set; }
    public bool IsFree { get; set; }
    public double PriceMin { get; set; }
    public double PriceMax { get; set; }
    public DateTime AdStartDate { get; set; }
    public DateTime AdEndDate { get; set; }
    public int AdDuration { get; set; }
    public bool IsReward { get; set; }
    public double RewardAmount { get; set; }
    public double RewardExpiration { get; set; }
    public bool IsExclusiveAd { get; set; }
    public double ExclusiveAdPeriod { get; set; }

  }
}
