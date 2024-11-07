namespace WebApplication1.Abstractions;

public interface IQueryHandler<TQuery, TResult>
{
    public Task<TResult> Handle(TQuery query, CancellationToken cancellationToken = default);
}