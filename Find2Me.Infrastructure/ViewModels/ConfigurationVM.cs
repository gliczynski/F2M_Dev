using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
  public class ConfigurationVM
  {
    public int ID { get; set; }

    public string Name { get; set; }
    public string Value { get; set; }

    public string Description { get; set; }

    public string ValueType { get; set; }

    public DateTime CreateTime { get; set; }
  }
}
