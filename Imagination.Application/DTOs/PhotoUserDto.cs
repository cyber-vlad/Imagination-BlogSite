using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.DTOs
{
    public class PhotoUserDto
    {
        public int IdUser { get; set; } = 0;
        public IFormFile ProfileImage { get; set; }
    }
}
