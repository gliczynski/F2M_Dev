using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
  public class UserAd
  {
    [Key]
    public int Id { get; set; }

    public string UserId { get; set; }

    public DateTime CreatedOn { get; set; }

    public bool IsDraft { get; set; }
  }
}
