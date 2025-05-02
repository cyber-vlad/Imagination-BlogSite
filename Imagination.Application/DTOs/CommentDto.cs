using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.DTOs
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime DateOfCreation { get; set; }
        public AuthorDto Author { get; set; }
        public List<CommentDto>? Replies { get; set; }
    }
}
