namespace ProductManager.Abstractions;

public interface ICommandHandler<TCommand, TResult>
{
    public Task<TResult> Handle(TCommand command, CancellationToken cancellationToken = default);
}