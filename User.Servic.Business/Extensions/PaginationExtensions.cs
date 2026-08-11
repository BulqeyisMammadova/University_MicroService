using Microsoft.EntityFrameworkCore;
using User.Servic.Business.DTOs;
using User.Service.Business.DTOs;

namespace User.Service.Business.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedResultDto<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<T>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}