using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Find2Me.Infrastructure.DbModels
{
    public class Currency
    {
        [Key]
        [MaxLength(50)]
        public string Code { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }

        public decimal ExchangeRate { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}