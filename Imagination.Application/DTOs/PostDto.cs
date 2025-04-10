using Imagination.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.DTOs
{
    public class PostDto
    {
        public int PostId { get; set; }
        public string Content { get; set; }
        public string Title { get; set; }
        public CategoryPost Category{ get; set; }
        public DateTime DateOfCreation { get; set; }
        public int NrLikes { get; set; } 
        public AuthorPostDto Author { get; set; }
        public bool IsLikedByCurrentUser { get; set; }
    }
}
