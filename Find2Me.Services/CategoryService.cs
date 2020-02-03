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
  public interface ICategoryService
  {
    List<CategoryVM> GetAllCategories();
  }

  public class CategoryService : ICategoryService
  {
    private CategoryRepository categoryRepository;
    public CategoryService(ApplicationDbContext dbContext)
    {
      categoryRepository = new CategoryRepository(dbContext);
    }

    public List<CategoryVM> GetAllCategories()
    {
      List<Category> categories = categoryRepository.GetAll().ToList();
      List<CategoryVM> categoryVMs = new List<CategoryVM>();
      Mapper.Map(categories, categoryVMs);
      return categoryVMs;
    }
  }
}
