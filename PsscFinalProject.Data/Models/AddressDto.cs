using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PsscFinalProject.Data.Models
{
    public class AddressDto
    {
        [JsonIgnore]
        [Key]
        public int AddressId { get; set; }
        public string ClientEmail { get; set; } = null!;
        public string ClientAddress { get; set; } = null!;
    }
}
