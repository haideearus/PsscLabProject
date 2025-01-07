using PsscFinalProject.Domain.Models.PsscFinalProject.Domain.Models;
using System;
using System.Collections.Generic;

namespace PsscFinalProject.Domain.Models
{
    public class Client
    {
        public int ClientId { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        public Client(int clientId, string email)
        {
            ClientId = clientId;
            Email = email;
        }
    }
}