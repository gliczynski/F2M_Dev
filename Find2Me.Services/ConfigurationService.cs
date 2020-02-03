using AutoMapper;
using Find2Me.DAL;
using Find2Me.Infrastructure.DbModels;
using Find2Me.Infrastructure.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Find2Me.Services
{
  public interface IConfigurationService
  {
    ConfigurationVM GetConfigurationByConfigurationName(string name);
  }

  public class ConfigurationService : IConfigurationService
  {
    private ConfigurationRepository configurationRepository;
    public ConfigurationService(ApplicationDbContext dbContext)
    {
      configurationRepository = new ConfigurationRepository(dbContext);
    }

    public ConfigurationVM GetConfigurationByConfigurationName(string name)
    {
      Configuration configuration = configurationRepository.GetConfigurationByConfigurationName(name);
      ConfigurationVM configurationVM = new ConfigurationVM();
      Mapper.Map(configuration, configurationVM);
      return configurationVM;
    }
  }
}
