using System.ComponentModel.DataAnnotations;

namespace Models.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }

        [MaxLength(140)]
        public string CommentText { get; set; }

        public int LikeQuantity { get; set; }

        public int PostId { get; set; }

        public Post Post { get; set; }

        public Comment()
        {
            LikeQuantity = 0;
        }
    }
}
