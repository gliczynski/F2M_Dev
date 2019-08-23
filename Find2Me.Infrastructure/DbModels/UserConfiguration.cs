using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
    public class UserConfiguration
    {
        [Key]
        public int ID { get; set; }

        public string UserID { get; set; }
        [ForeignKey("UserID")]
        public ApplicationUser ApplicationUser { get; set; }

        public int ConfiguratonID { get; set; }
        [ForeignKey("ConfiguratonID")]
        public Configuration Configuration { get; set; }

        public string Value { get; set; }

        public DateTime CreatedTime { get; set; }
    }
}
