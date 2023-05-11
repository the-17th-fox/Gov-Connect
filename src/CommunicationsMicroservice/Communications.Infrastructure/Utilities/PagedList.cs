using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Communications.Infrastructure.Utilities;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalItemsCount { get; private set; }

    public bool HasPreviousPage => CurrentPage > 1;
        
    public bool HasNextPage => CurrentPage < TotalPages;

    private PagedList(List<T> items, int itemsCount, int pageNumber, int pageSize)
    {
        TotalPages = (int)Math.Ceiling(itemsCount / (double)pageSize);
        CurrentPage = pageNumber;
        PageSize = pageSize;
        TotalItemsCount = itemsCount;
        AddRange(items);
    }

    /// <summary>
    /// Used to form a pagedList from provided query and page parameters.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="pageNumber"></param>
    /// <param name="pageSize"></param>d
    /// <returns></returns>
    public static async Task<PagedList<T>> ToPagedListAsync<TKey>(
        IQueryable<T> query, 
        int pageNumber, 
        int pageSize, 
        Expression<Func<T, TKey>> sortingExpression,
        bool orderByDescending = true)
    {
        var itemsCount = query.Count();

        query = orderByDescending 
            ? query.OrderByDescending(sortingExpression) 
            : query.OrderBy(sortingExpression);

        var items = await (query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync());

        return new PagedList<T>(items, itemsCount, pageNumber, pageSize);
    }
}