using System.Threading.Tasks;

namespace PsscFinalProject.Events
{
    public interface IEventSender
    {
        Task SendAsync<T>(string topicName, T @event);
    }
}