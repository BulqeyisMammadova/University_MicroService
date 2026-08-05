using Microsoft.EntityFrameworkCore;
using University.Service.Business.DTOs;

namespace University.Service.Business.Extensions;

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