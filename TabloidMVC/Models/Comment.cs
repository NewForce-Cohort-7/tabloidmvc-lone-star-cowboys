using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace TabloidMVC.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public int UserProfileId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        [DisplayName("Date Created")]
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        public List<Comment> Comments { get; set; }

        public Post Post { get; set; }
        [DisplayName("Author Name")]
        public string UserDisplayName { get; set; }
    }
}
