using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Data.Models;

public partial class UserDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    [Key]
    public string Email { get; set; } = null!;

    public virtual ClientDto? Client { get; set; }
}
