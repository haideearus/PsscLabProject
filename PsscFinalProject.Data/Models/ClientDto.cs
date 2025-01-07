using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PsscFinalProject.Data.Models;

public partial class ClientDto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public int UserId { get; set; }

    public virtual ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();

    public virtual UserDto User { get; set; } = null!;
}
