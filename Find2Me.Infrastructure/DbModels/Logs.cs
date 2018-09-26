using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
    public class Logs
    {
        [Key]
        public int ID { get; set; }

        public string ActionType { get; set; }

        public string ActionMessage { get; set; }

        public string IpAddress { get; set; }

        public DateTime CreateTime { get; set; }

        [ForeignKey("ActionFromUserId")]
        public ApplicationUser ActionFromUser { get; set; }
        public string ActionFromUserId { get; set; }

        [ForeignKey("ActionToUserId")]
        public ApplicationUser ActionToUser { get; set; }
        public string ActionToUserId { get; set; }
    }

}
