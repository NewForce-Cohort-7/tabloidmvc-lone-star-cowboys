using TabloidMVC.Models;

public interface ICategoryRepository
{
    List<Category> GetAll();
    void Add(Category category);
    void Update(Category category);
    Category GetCategoryById(int id);
}
}