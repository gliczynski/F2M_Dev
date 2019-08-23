using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
    public class UserAdImage
    {
        [Key]
        public long Id { get; set; }

        public int AdID { get; set; }

        [ForeignKey("AdID")]

        public UserAd UserAd { get; set; }

        public string AdImageOriginal { get; set; }

        public string AdImageSelected { get; set; }

        public int ImageNumber { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}
