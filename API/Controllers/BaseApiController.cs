using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected async Task<IActionResult> CreatePageResult<T>(IGenericRepository<T> genericRepository, ISpecification<T> spec, int pageIndex, int pageSize) where T : BaseEntity
        {
            var totalItems = await genericRepository.CountAsync(spec);
            var items = await genericRepository.ListAsync(spec);
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            var result = new
            {
                PageIndex = pageIndex,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                Items = items
            };
            return Ok(result);
        }
    }
}
