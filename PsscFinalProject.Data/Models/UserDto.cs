using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class UserDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Key]
    public string Email { get; set; } = null!;

    [JsonIgnore]
    public virtual ClientDto? Client { get; set; }
}