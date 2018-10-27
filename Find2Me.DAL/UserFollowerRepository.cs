using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class UserFollowerRepository : DbRepository<UserFollower>
    {
        public UserFollowerRepository(ApplicationDbContext context) : base(context)
        {
        }

        public UserFollower GetFollower(string followByUserId, string followedUserId)
        {
            return dbContext.UserFollowers.FirstOrDefault(x => x.FollowByUserId.Equals(followByUserId) && x.FollowedUserId.Equals(followedUserId));
        }

        public SP_FollowersCount GetFollowersCount(string userId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId),
            };
            SP_FollowersCount result = dbContext.Database.SqlQuery<SP_FollowersCount>("SP_GetFollowersCount @UserId", parameters).FirstOrDefault();
            return result;
        }
    }
}
