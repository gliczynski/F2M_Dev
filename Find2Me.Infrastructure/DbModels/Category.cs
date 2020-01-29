using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
  public class Category
  {
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }
  }
}
