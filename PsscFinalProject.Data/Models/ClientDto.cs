using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models;

public partial class ClientDto
{
    [JsonIgnore]
    [Key]
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    [JsonIgnore]
    public virtual UserDto? EmailNavigation { get; set; }

    //[JsonIgnore]
    //public virtual ICollection<OrderDto> Orders { get; set; } = new List<OrderDto>();
}
