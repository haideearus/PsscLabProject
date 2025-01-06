using System.Threading.Tasks;
using CloudNative.CloudEvents;
using PsscFinalProject.Events.Models;

namespace PsscFinalProject.Events
{
    public interface IEventHandler
    {
        string[] EventTypes { get; }

        Task<EventProcessingResult> HandleAsync(CloudEvent cloudEvent);
    }
}