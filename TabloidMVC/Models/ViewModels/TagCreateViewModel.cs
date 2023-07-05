// Purpose: Model for the Tag Create View. Contains a Tag object and a list of Tag objects for the dropdown menu.

namespace TabloidMVC.Models.ViewModels
{
    public class TagCreateViewModel
    {
        public Tag Tag { get; set; }
        public List<Tag> TagOptions { get; set; }
    }
}
