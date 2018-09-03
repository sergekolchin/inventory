using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Inventory.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;

namespace Inventory.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _entities;
        private readonly ILogger<IRepository<T>> _logger;

        public Repository(ApplicationDbContext context, ILogger<IRepository<T>> logger)
        {
            _context = context;
            _entities = context.Set<T>();
            _logger = logger;
        }

        /// <summary>
        /// Add entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Task&lt;T&gt;</returns>
        public async Task<T> AddAsync(T entity)
        {
            CheckNullForSingle(entity);
            _entities.Add(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"New {typeof(T)} with key was successfully created");
            return entity;
        }

        /// <summary>
        /// Add entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt;></param>
        /// <returns>Task</returns>
        public async Task AddRangeAsync(IEnumerable<T> entities)
        {
            var entitiesList = entities as IList<T> ?? entities.ToList();
            CheckNullForList(entitiesList);
            _entities.AddRange(entitiesList);
            await _context.SaveChangesAsync();

            var entityKeys = entitiesList.Select(e => e.Id);
            _logger.LogInformation($"Collection of new {typeof(T)} with keys [{entityKeys.Aggregate(string.Empty, (current, key) => current + key)}] was successfully created");
        }

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
        public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool noTracking = true)
        {
            return await GetQueryWithConditions(predicate, orderBy, include, noTracking).FirstOrDefaultAsync();
        }

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
        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool noTracking = true)
        {
            return await GetQueryWithConditions(predicate, orderBy, include, noTracking).ToListAsync();
        }

        /// <summary>
        /// Remove entity
        /// </summary>
        /// <param name="T entity"></param>
        /// <returns>Task&lt;T&gt;</returns>
        public async Task<T> RemoveAsync(T entity)
        {
            CheckNullForSingle(entity);
            _entities.Remove(entity);
            await _context.SaveChangesAsync();
            _logger.LogInformation($"{typeof(T)} with key {entity.Id} was successfully deleted");
            return entity;
        }

        /// <summary>
        /// Remove entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt; entities</param>
        /// <returns>Task</returns>
        public async Task RemoveRange(IEnumerable<T> entities)
        {
            var entitiesList = entities as IList<T> ?? entities.ToList();
            CheckNullForList(entitiesList);
            _entities.RemoveRange(entitiesList);
            await _context.SaveChangesAsync();
            var entityKeys = entitiesList.Select(e => e.Id);
            _logger.LogInformation($"Collection of {typeof(T)} with keys [{entityKeys.Aggregate(string.Empty, (current, key) => current + key)}] was successfully deleted");
        }

        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity">T entity</param>
        /// <returns>Task&lt;T&gt;</returns>
        public async Task<T> UpdateAsync(T entity)
        {
            CheckNullForSingle(entity);
            entity.LastModifiedOn = DateTime.UtcNow;
            _entities.Update(entity);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"{typeof(T)} with key {entity.Id} was successfully updated");
            return entity;
        }

        /// <summary>
        /// Update the entity range
        /// </summary>
        /// <param name="entities">IEnumerable&lt;T&gt;</param>
        /// <returns>Task</returns>
        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            var entitiesList = entities as IList<T> ?? entities.ToList();
            CheckNullForList(entitiesList);
            foreach (var entity in entitiesList)
            {
                entity.LastModifiedOn = DateTime.UtcNow;
            }
            _entities.UpdateRange(entitiesList);
            await _context.SaveChangesAsync();

            var entityKeys = entitiesList.Select(e => e.Id);
            _logger.LogInformation($"Collection of {typeof(T)} with keys [{entityKeys.Aggregate(string.Empty, (current, key) => current + key)}] was successfully updated");
        }

        private IQueryable<T> GetQueryWithConditions(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include,
            bool noTracking)
        {
            var query = _entities.AsQueryable();
            if (noTracking)
            {
                query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null ? orderBy(query) : query;
        }

        private void CheckNullForSingle(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        private void CheckNullForList(IList<T> entitiesList)
        {
            if (entitiesList.Any(e => e == null))
            {
                var first = entitiesList.FirstOrDefault(e => e == null);
                throw new ArgumentNullException(nameof(first));
            }
        }
    }
}
