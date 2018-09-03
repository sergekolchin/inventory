using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Inventory.Data
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Task&lt;T&gt;</returns>
        Task<T> AddAsync(T entity);

        /// <summary>
        /// Add entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt;></param>
        /// <returns>Task</returns>
        Task AddRangeAsync(IEnumerable<T> entities);

        /// <summary>
        /// Gets first or default entity based on a predicate, orderBy delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="noTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>Task&lt;T&gt;</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        /// <example>
        /// <code>
        /// var affiliate = await MyRepository.GetFirstOrDefaultAsync(
        /// predicate: x => x.Id == id,
        /// orderBy: x => x.OrderBy(x => x.Col1).ThenBy(x => x.Col2)
        /// include: s => s.Include(b => b.Col1).ThenInclude(e => e.Col2));
        /// </code>
        /// </example>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool noTracking = true);

        /// <summary>
        /// Gets entities based on a predicate, orderby delegate and include delegate. This method default no-tracking query.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="orderBy">A function to order elements.</param>
        /// <param name="include">A function to include navigation properties.</param>
        /// <param name="noTracking"><c>True</c> to disable changing tracking; otherwise, <c>false</c>. Default to <c>true</c>.</param>
        /// <returns>Task&lt;IEnumerable&lt;T&gt;&gt;</returns>
        /// <remarks>This method default no-tracking query.</remarks>
        /// <example>
        /// <code>
        /// var affiliate = await MyRepository.GetFirstOrDefaultAsync(
        /// predicate: x => x.Id == id,
        /// orderBy: x => x.OrderBy(x => x.Col1).ThenBy(x => x.Col2)
        /// include: s => s.Include(b => b.Col1).ThenInclude(e => e.Col2));
        /// </code>
        /// </example>
        Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool noTracking = true);

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>Task&lt;T&gt;</returns>
        Task<T> RemoveAsync(T entity);

        /// <summary>
        /// Remove entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt; entities</param>
        /// <returns>Task</returns>
        Task RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Task&lt;T&gt;</returns>
        Task<T> UpdateAsync(T entity);

        /// <summary>
        /// Update the entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt;</param>
        /// <returns>Task</returns>
        Task UpdateRangeAsync(IEnumerable<T> entities);
    }
}