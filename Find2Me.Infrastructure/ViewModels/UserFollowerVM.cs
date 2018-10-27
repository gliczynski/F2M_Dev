using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Infrastructure.ViewModels
{
    public class UserFollowerVM
    {
        public int Id { get; set; }

        public string FollowByUserId { get; set; }
        public UserProfileVM FollowByUser { get; set; }

        public string FollowedUserId { get; set; }
        public UserProfileVM FollowedUser { get; set; }

        public DateTime FollowedOn { get; set; }
    }
}
