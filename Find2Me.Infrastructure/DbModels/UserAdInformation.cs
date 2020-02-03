using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
  public class UserAdInformation
  {
    [Key]
    public long Id { get; set; }

    public int AdID { get; set; }
    [ForeignKey("AdID")]

    public UserAd UserAd { get; set; }

    public int CategoryID { get; set; }
    [ForeignKey("CategoryID")]

    public Category Category { get; set; }

    public bool IsOriginal { get; set; }

    public bool IsState { get; set; }

    public string Headline { get; set; }

    public string Description { get; set; }

    public string Tags { get; set; }
  }
}
