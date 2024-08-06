using System.ComponentModel.DataAnnotations;

namespace Models.ModelsDto
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; }
        public string PostText { get; set; }
        public int LikesQuantity { get; set; }
    }
}
