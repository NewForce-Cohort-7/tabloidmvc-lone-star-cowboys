using TabloidMVC.Models;
namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
        List<Comment> GetAllComments();
        Comment GetCommentById(int id);
        List<Comment> GetCommentByPostId(int postId);
        void AddComment(Comment comment);
        void UpdateComment(Comment comment);
    }
}
