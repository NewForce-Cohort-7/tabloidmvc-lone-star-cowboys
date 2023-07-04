using System.Collections.Generic;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IConfiguration _config;

        public CategoryRepository(IConfiguration config)
        {
            _config = config;
        }

        public List<Category> GetAll()
        {
            using (SqlConnection conn = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Category";

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<Category> categories = new List<Category>();

                        while (reader.Read())
                        {
                            Category category = new Category
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Name = reader.GetString(reader.GetOrdinal("Name"))
                            };

                            categories.Add(category);
                        }

                        return categories;
                    }
                }
            }
        }

        public void Add(Category category)
        {
            // Code for adding a new category to the database
        }
    }
}
