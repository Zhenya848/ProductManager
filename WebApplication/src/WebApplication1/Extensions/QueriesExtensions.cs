using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> GetItemsWithPagination<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken cancellationToken)
    {
        var itemsCount = await source.CountAsync(cancellationToken); 
        var items = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<T>()
        {
            Items = items,
            TotalCount = itemsCount,
            Page = page,
            PageSize = pageSize,
        };
    }
}