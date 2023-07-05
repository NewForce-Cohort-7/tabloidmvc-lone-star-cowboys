using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void Add(Category category);
        void Update(Category category);
        void Delete(int categoryId);
        Category GetCategoryById(int id);
    }
}
