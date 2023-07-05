using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }

        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT id, name FROM Category";
                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();

                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        });
                    }

                    reader.Close();

                    return categories;
                }
            }
        }

        public void Add(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Category (Name)
                        OUTPUT INSERTED.ID
                        VALUES (@Name)";
                    cmd.Parameters.AddWithValue("@Name", category.Name);

                    category.Id = (int)cmd.ExecuteScalar();
                }
            }
        }

        public void Update(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE Category
                        SET Name = @Name
                        WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.Parameters.AddWithValue("@Id", category.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int categoryId)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM Category WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", categoryId);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Category GetCategoryById(int id)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT Id, Name FROM Category WHERE Id = @Id";
                    cmd.Parameters.AddWithValue("@Id", id);

                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return new Category()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                    }

                    reader.Close();

                    return null;
                }
            }
        }
    }
}
