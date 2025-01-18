using System;
using System.Collections.Generic;
using System.Linq;

namespace PsscFinalProject.Domain.Models
{
    public static class OrderPublishEvent
    {
        public interface IOrderPublishEvent { }

        public record OrderPublishSucceededEvent : IOrderPublishEvent
        {
            public OrderPublishSucceededEvent(ClientEmail clientEmail, string csv, ProductPrice productPrice, DateTime publishDate)
            {
                ClientEmail = clientEmail ?? throw new ArgumentNullException(nameof(clientEmail));
                Csv = csv ?? throw new ArgumentNullException(nameof(csv));
                ProductPrice = productPrice ?? throw new ArgumentNullException(nameof(productPrice));
                PublishDate = publishDate;
            }

            public ClientEmail ClientEmail { get; } // Make sure this is public
            public string Csv { get; }
            public ProductPrice ProductPrice { get; }
            public DateTime PublishDate { get; }
        }


        public record OrderPublishFailedEvent : IOrderPublishEvent
        {
            internal OrderPublishFailedEvent(IEnumerable<string> reasons)
            {
                Reasons = reasons;
            }
            public IEnumerable<string> Reasons { get; }
        }

    }
}
