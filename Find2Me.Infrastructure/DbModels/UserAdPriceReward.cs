using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
  public class UserAdPriceReward
  {
    [Key]
    public long Id { get; set; }

    public int AdID { get; set; }
    [ForeignKey("AdID")]

    public UserAd UserAd { get; set; }
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
