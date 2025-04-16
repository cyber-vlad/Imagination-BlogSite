using Imagination.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Responses
{
    public class ToggledLikeResponse : BaseResponse
    {
        public Post Post { get; set; }
        public bool IsLiked { get; set; }
    }
}
