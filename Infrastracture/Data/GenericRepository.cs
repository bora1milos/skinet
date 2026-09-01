using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastracture.Data;

public class GenericRepository<T>(StoreContext context) : IGenericRepository<T> where T : Core.Entities.BaseEntity
{
    private readonly StoreContext m_context = context;

    public void Add(T entity)
    {
        m_context.Set<T>().Add(entity);
    }

    public bool Exists(int id)
    {
        return m_context.Set<T>().Any(e => e.Id == id);
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<T>(m_context.Set<T>(), e => e.Id == id);
    }

    public async Task<T?> GetEntityWithSpecAsync(ISpecification<T> spec)
    {
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<T>(ApplySpecification(spec));
    }

    public async Task<TResult?> GetEntityWithSpecAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await EntityFrameworkQueryableExtensions.FirstOrDefaultAsync<TResult>(ApplySpecification(spec));
    }

    public async Task<IReadOnlyList<T>> ListAllAsync()
    {
        return await EntityFrameworkQueryableExtensions.ToListAsync<T>(m_context.Set<T>());
    }

    public async Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public async Task<IReadOnlyList<TResult>> ListAsync<TResult>(ISpecification<T, TResult> spec)
    {
        return await ApplySpecification(spec).ToListAsync();
    }

    public void Remove(T entity)
    {
        m_context.Set<T>().Remove(entity);
    }

    public async Task<bool> SaveAllAsync()
    {
        return await m_context.SaveChangesAsync() > 0;
    }

    public void Update(T entity)
    {
        m_context.Set<T>().Update(entity);
    }

    private IQueryable<T> ApplySpecification(ISpecification<T> spec)
    {
        return SpecificationEvaluator<T>.GetQuery(m_context.Set<T>().AsQueryable(), spec);
    }

    private IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> spec)
    {
        return SpecificationEvaluator<T>.GetQuery<T, TResult>(m_context.Set<T>().AsQueryable(), spec);
    }
}
