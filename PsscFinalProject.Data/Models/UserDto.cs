using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models
{

    public partial class UserDto
    {
        public string Username { get; set; }

        public string Password { get; set; }

        [Key] public string Email { get; set; } = null!;

        [JsonIgnore] public virtual ClientDto? Client { get; set; }
    }
}
