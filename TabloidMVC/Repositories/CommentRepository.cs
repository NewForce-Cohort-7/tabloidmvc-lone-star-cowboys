using Microsoft.Data.SqlClient;
using TabloidMVC.Models;


namespace TabloidMVC.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IConfiguration _config;

        public CommentRepository(IConfiguration config) 
        { 
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Comment> GetAllComments()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"
                        SELECT Id, 
                               PostId, 
                               UserProfileId, 
                               Subject,     
                               Content, 
                               CreateDateTime 
                        FROM Comment
                        ";

                    SqlDataReader reader = cmd.ExecuteReader();
                    
                    List<Comment> comments = new List<Comment>();
                    while (reader.Read()) 
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };
                    comments.Add(comment);
                    }

                    reader.Close();
                    return comments;
                }
            }
        } //End of GetAllComments

        public Comment GetCommentById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, 
                               PostId, 
                               UserProfileId, 
                               Subject,     
                               Content, 
                               CreateDateTime 
                        FROM Comment
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime"))
                        };

                        reader.Close();
                        return comment;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        } //End of GetCommentById

        public List<Comment> GetCommentByPostId(int postId) 
        {
            using (SqlConnection conn = Connection) 
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand()) 
                {
                    cmd.CommandText = @"
                        SELECT c.Id AS CommentID, 
                               c.PostId, 
                               c.UserProfileId, 
                               c.Subject,     
                               c.Content, 
                               c.CreateDateTime,
                               Post.Title,
                               u.DisplayName
                        FROM Comment c
                        JOIN Post ON c.PostId = Post.Id
                        JOIN UserProfile u ON c.UserProfileId = u.id
                        WHERE PostId = @postId
                        ";

                    cmd.Parameters.AddWithValue("postId", postId);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                    while (reader.Read()) 
                    {
                        Comment comment = new Comment
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("CommentID")),
                            PostId = reader.GetInt32(reader.GetOrdinal("PostId")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("UserProfileId")),
                            Subject = reader.GetString(reader.GetOrdinal("Subject")),
                            Content = reader.GetString(reader.GetOrdinal("Content")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            Post = new Post(),
                            UserDisplayName = reader.GetString(reader.GetOrdinal("DisplayName"))
                        };

                        comments.Add(comment);
                    }
                        reader.Close();
                        return comments;
                }
            }
        } //End of GetCommentByPostId

        public void AddComment(Comment comment)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                INSERT INTO Dog (PostId, UserProfileId, Subject, Content, CreateDateTime)
                OUTPUT INSERTED.ID
                VALUES (@postId, @userProfileId, @subject, @content, createDateTime);
            ";

                    cmd.Parameters.AddWithValue("@postId", comment.PostId);
                    cmd.Parameters.AddWithValue("@userProfileId", comment.UserProfileId);
                    cmd.Parameters.AddWithValue("@subject", comment.Subject);
                    cmd.Parameters.AddWithValue("@content", comment.Content);
                    cmd.Parameters.AddWithValue("@createDateTime", comment.CreateDateTime);

                    // nullable columns

                    int newlyCreatedId = (int)cmd.ExecuteScalar();

                    comment.Id = newlyCreatedId;

                }
            }
        }


    }
}
