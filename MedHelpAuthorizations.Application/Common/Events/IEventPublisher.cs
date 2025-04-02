using MedHelpAuthorizations.Shared.Events;

namespace MedHelpAuthorizations.Application.Common.Events;
public interface IEventPublisher : ITransientService
{
    Task PublishAsync(IEvent @event);
}