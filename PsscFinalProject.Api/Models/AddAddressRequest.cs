using PsscFinalProject.Domain.Models;

namespace PsscFinalProject.Api.Models
{
    public class AddAddressRequest
    {
        public string ClientEmail { get; set; } = null!;
        public string ClientAddress { get; set; } = null!;
    }

    public class ModifyAddressRequest
    {
        public string ClientEmail { get; set; } = null!;
        public int AddressId { get; set; }
        public string NewAddress { get; set; } = null!;
    }
}
