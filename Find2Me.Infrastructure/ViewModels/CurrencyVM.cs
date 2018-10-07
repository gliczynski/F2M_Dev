using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
    public class CurrencyVM
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
