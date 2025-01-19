using System.Collections.Generic;
using System;

namespace PsscFinalProject.Domain.Models
{
    public static class BillPublishEvent
    {
        public interface IBillPublishEvent { }

        public record BillPublishSucceededEvent(string Csv, DateTime PublishDate) : IBillPublishEvent;

        public record BillPublishFailedEvent(IEnumerable<string> Reasons) : IBillPublishEvent;
    }
}
