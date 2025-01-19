using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsscFinalProject.Domain.Models
{
    public static class DeliveryPublishedEvent
    {
        public interface IDeliveryPublishedEvent { }

        public record DeliveryPublishedEventSucceededEvent(string Csv, DateTime PublishDate) : IDeliveryPublishedEvent;

        public record DeliveryPublishedEventFailedEvent(IEnumerable<string> Reasons) : IDeliveryPublishedEvent;
    }
}
