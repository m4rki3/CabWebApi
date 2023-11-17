using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CabWebApi.Infrastructure.Data.Extensions;
public static class IQueryableExtensions
{
	public static Task<List<TModel>> ToListSafeAsync<TModel>(this IQueryable<TModel> query)
	{
		if (query is null)
			throw new ArgumentException($"{nameof(IQueryable<TModel>)} is null");

		if (query is not IAsyncEnumerable<TModel>)
			return Task.FromResult(query.ToList());

		return query.ToListAsync();
	}
}