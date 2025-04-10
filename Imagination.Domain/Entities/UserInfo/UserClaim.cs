using Imagination.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Domain.Entities.UserInfo
{
    public class UserClaim
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public UserRole UserRole { get; set; }
        public string PhotoUrl { get; set; }
    }
}
