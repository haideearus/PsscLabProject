using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    namespace PsscFinalProject.Domain.Models
    {
        public class User
        {
            // Primary Key
            public int UserId { get; set; }

            // Username
            public string? UserName { get; set; }

            // Email of the user
            public string? Email { get; set; }

            // Password (or other details)
            public string?  Password { get; set; }
        }
    }

}
