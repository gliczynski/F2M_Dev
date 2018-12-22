using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class ApplicationUserRepository : DbRepository<ApplicationUser>
    {
        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public ApplicationUser UserExists(string userIdToExclude, string username)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id.Equals(userIdToExclude) == false
                                                        &&
                                                        x.UrlUsername.ToLower().Trim().Equals(username.ToLower().Trim())
                                                 );
        }

        public bool UserExists(string username)
        {
            return dbContext.Users.Any(x => x.UrlUsername.ToLower().Trim().Equals(username.ToLower().Trim()));
        }

        public ApplicationUser UserExists(string userIdToExclude, string username, string email)
        {
            return dbContext.Users.FirstOrDefault(x => x.Id.Equals(userIdToExclude) == false
                                                        &&
                                                        (
                                                            x.UrlUsername.ToLower().Trim().Equals(username.ToLower().Trim())
                                                            ||
                                                            x.Email.ToLower().Trim().Equals(email.ToLower().Trim())
                                                        )
                                                 );
        }

        public ApplicationUser GetUserByUsername(string username)
        {
            return dbContext.Users.FirstOrDefault(x => x.UrlUsername.ToLower().Trim().Equals(username.ToLower().Trim()));
        }

        public List<string> CheckUserSuggestion(List<string> userSuggestions)
        {

            var result = (from username in userSuggestions
                          join user in dbContext.Users
                          on username.ToLower() equals (user.UrlUsername != null ? user.UrlUsername.ToLower(): "")
                          into ug
                          from u in ug.DefaultIfEmpty()
                          select new
                          {
                              Username = username,
                              User = u
                          }).Where(x => x.User == null).Select(x => x.Username).ToList();

            return result;
        }

        public ApplicationUser GetUserWithProfilePicture(string id)
        {
            return dbContext.Users.Include(x => x.ProfileImageData).SingleOrDefault(x => x.Id.Equals(id));
        }

        public UserProfileImageData GetProfileImageDataForUser(string userId)
        {
            return dbContext.UserProfileImageDatas.FirstOrDefault(x => x.UserId.Equals(userId));
        }

        /// <summary>
        /// Sored Procedure: Delete UserExternalLogins, Claims and UserFollower Data
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int SP_DeleteUserProfileData(string userId)
        {
            SqlParameter[] parameters =
            {
                new SqlParameter("@UserId", userId),
            };
            string sqlQuery = "SP_DeleteUserProfileData @UserId";
            int result = dbContext.Database.ExecuteSqlCommand(sqlQuery, parameters);
            return result;
        }
    }
}
