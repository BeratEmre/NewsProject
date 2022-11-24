using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {

        INewsService _newsService;
        private IHttpContextAccessor _httpContextAccessor;
        public NewsController(INewsService newsService, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
             _newsService = newsService;
        }

        [HttpPost("Add")]
        [Authorize]
        public IActionResult Add(News news)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = _newsService.Add(news, token);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("Update")]
        [Authorize]
        public IActionResult Update(News news)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = _newsService.Update(news, token);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetById")]
        public IActionResult GetById(int id)
        {            
            var result = _newsService.GetById(id);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("MakeActive")]
        [Authorize]
        public IActionResult MakeActive(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = _newsService.MakeActive(id, token);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("MakePassive")]
        [Authorize]
        public IActionResult MakePassive(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = _newsService.MakePassive(id, token);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var result = _newsService.GetAll();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("AtiveNews")]
        public IActionResult AtiveNews()
        {
            var result = _newsService.ActiveNews();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("PassiveNews")]
        public IActionResult PassiveNews()
        {
            var result = _newsService.PassiveNews();
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpPost("Delete")]
        [Authorize]
        public IActionResult Remove(int id)
        {
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            var result = _newsService.Delete(id, token);
            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
