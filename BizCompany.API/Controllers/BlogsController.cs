using BizCompany.API.DataAccess;
using BizCompany.API.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BizCompany.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogsController(IBaseRepository<Blog> repository)
        : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var values = await repository.GetAllAsync();
            if (values == null || !values.Any())
            {
                return NotFound("No blogs found.");
            }
            return Ok(values);
        }

    }
}
