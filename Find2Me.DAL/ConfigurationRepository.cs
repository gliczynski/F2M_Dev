using Find2Me.Infrastructure.DbModels;
using System.Linq;

namespace Find2Me.DAL
{
  public class ConfigurationRepository : DbRepository<Configuration>
  {
    public ConfigurationRepository(ApplicationDbContext context): base(context)
    {

    }

    public Configuration GetConfigurationByConfigurationName(string name)
    {
      return dbContext.Configurations.FirstOrDefault(x => x.Name.ToLower().Trim() == name.ToLower().Trim());
    }
  }
}
