using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
    public class UserAdRepository : DbRepository<UserAd>
    {
        public UserAdRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
