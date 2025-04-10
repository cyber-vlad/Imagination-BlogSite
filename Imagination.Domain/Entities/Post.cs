using Imagination.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public CategoryPost Category { get; set; }
        public DateTime DateOfCreation { get; set; }
        public int NrLikes {  get; set; }
        //public int NrComments { get; set; }
        public int AuthorId { get; set; }
        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
