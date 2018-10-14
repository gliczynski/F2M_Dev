using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class UserProfileImageDataRepository : DbRepository<UserProfileImageData>
    {
        public UserProfileImageDataRepository(ApplicationDbContext context) : base(context)
        {
        }

        
    }
}
