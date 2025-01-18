namespace PsscFinalProject.Domain.Models
{
    public record UnvalidatedOrder(string ClientEmail, string ProductCode, int Quantity);
}