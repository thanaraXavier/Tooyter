using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Post
    {
        [Key]
        public int PostId { get; set; }

        [MaxLength(50)]
        public string PostTitle { get; set; }

        [MaxLength(140)]
        public string PostText { get; set; }

        public int LikesQuantity { get; set; }

        public int CommentsQuantity { get; set; }

        public ICollection<Comment>? Comments { get; set; }

        public Post()
        {
            LikesQuantity = 0;
            CommentsQuantity = 0;
        }
    }
}
