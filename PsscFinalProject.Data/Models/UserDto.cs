using System;
using System.Collections.Generic;

namespace PsscFinalProject.Data.Models;

public partial class UserDto
{
    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Email { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<ClientDto> Clients { get; set; } = new List<ClientDto>();
}
