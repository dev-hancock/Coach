using Coach.Domain.Common;
using MediatR;

namespace Coach.Application.Common;

/// <summary>
/// Wrapper to adapt domain events to MediatR notifications.
/// Allows infrastructure to publish domain events without domain layer depending on MediatR.
/// </summary>
public sealed record DomainEventNotification<TDomainEvent>(TDomainEvent DomainEvent) : INotification
    where TDomainEvent : DomainEvent;
