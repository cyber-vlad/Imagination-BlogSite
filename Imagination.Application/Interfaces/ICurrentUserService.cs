using Imagination.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Application.Interfaces
{
    public interface ICurrentUserService
    {
        public int UserId { get; }
        public string Email { get; set; }
        public string Username { get; set; }
        public UserRole UserRole { get; set; }
        public string PhotoUrl { get; set; }
    }
}
