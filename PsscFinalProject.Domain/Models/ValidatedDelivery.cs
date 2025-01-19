using System;

namespace PsscFinalProject.Domain.Models
{
    public record ValidatedDelivery(TrackingNumber TrackingNumber, Courier Courier);
}
