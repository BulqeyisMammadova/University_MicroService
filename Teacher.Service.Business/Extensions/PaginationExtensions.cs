using Microsoft.EntityFrameworkCore;
using Teacher.Service.Business.DTOs;

namespace Teacher.Service.Business.Extensions;

public static class PaginationExtensions
{
    public static async Task<IEnumerable<T>> ToPagedAsync<T>(
        this IQueryable<T> query, PaginationParams p)
    {
        return await query
            .Skip((p.PageNumber - 1) * p.PageSize)
            .Take(p.PageSize)
            .ToListAsync();
    }
}