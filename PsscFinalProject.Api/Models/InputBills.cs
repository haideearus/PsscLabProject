using PsscFinalProject.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace PsscFinalProject.Api.Models
{
    public class InputBills
    {

        [Required]
        public required ClientEmail ClientEmail { get; set; }
        public required string BillAddress { get; set; }

        public required string BillNumber { get; set; }
    }
}
