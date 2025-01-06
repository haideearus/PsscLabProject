using PsscFinalProject.Domain.Models.PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;

namespace PsscFinalProject.Domain.Models
{
    public class Client
    {
        // Primary Key
        public int ClientId { get; set; }

        // First Name of the client
        public string FirstName { get; set; } = null!;

        // Last Name of the client
        public string LastName { get; set; } = null!;

        // Email of the client
        public string Email { get; set; } = null!;

        // Phone Number of the client
        public string PhoneNumber { get; set; } = null!;

        // UserId (Foreign key to the User entity)
        public int UserId { get; set; }

        // Optional: Navigation property to the User entity
        public virtual User User { get; set; } = null!;

        // Navigation property for Orders related to this Client
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
