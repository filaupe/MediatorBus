namespace MediatorBus.Interfaces;

public interface IRequest { }

public interface IRequest<TResponse> : IRequest { }
