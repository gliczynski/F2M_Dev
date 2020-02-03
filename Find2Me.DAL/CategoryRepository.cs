using Find2Me.Infrastructure.DbModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.DAL
{
  public class CategoryRepository : DbRepository<Category>
  {
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
  }
}
