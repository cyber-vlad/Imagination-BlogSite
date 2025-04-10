using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imagination.Domain.Enum
{
    public enum ErrorCode
    {
        Internal_error = -1,
        NoError = 0,
        Username_or_password_is_incorrect = 2,
        User_name_not_found_or_incorrect_password = 3,
        User_already_exists = 4,
        User_name_not_found_or_incorrect_Email = 5,
        Email_already_used = 6,
        HttpClient_error = 7,
        User_not_found = 8,
        Upload_image_failed = 9,
        Create_post_failed = 10,
    }

    public enum UserRole
    {
        None = 0, 
        User = 1,
        Admin = 2
    }

    public enum CategoryPost
    {
        PersonalBlog = 0,
        Lifestyle = 1,
        Technology = 2,
        Business = 3,
        News = 4
    }
}
