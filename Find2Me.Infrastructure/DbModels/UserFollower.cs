using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.DbModels
{
    public class UserFollower
    {
        [Key]
        public int Id { get; set; }

        public string FollowByUserId { get; set; }
        [ForeignKey("FollowByUserId")]
        public ApplicationUser FollowByUser { get; set; }

        public string FollowedUserId { get; set; }
        [ForeignKey("FollowedUserId")]
        public ApplicationUser FollowedUser { get; set; }

        public DateTime FollowedOn { get; set; }
    }
}
