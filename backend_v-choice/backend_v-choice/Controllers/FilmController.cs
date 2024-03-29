﻿using BLL.DTO;
using BLL.Interface;
using BLL.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace backend_v_choice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilmController : Controller
    {
        private readonly ICrudService _crudService;
        private readonly IPaginationService _paginationService;
        private readonly ILogger _logger;
        private readonly IWebHostEnvironment _appEnvironment;

        public FilmController(ICrudService cs, IPaginationService ps, ILogger<FilmController> logger, IWebHostEnvironment ae)
        {
            _crudService = cs;
            _paginationService = ps;
            _logger = logger;
            _appEnvironment = ae;
        }

        [HttpGet]
        public async Task<IActionResult> GetFilmsPagination([FromQuery] PaginationQueryFilms query)
        {
            _logger.LogInformation("Get pagination films");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Get pagination films: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            var res = await _paginationService.GetFilmsPagination(query);
            if (res == null) return StatusCode(500);

            return Ok(res);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFilm([FromRoute] int id)
        {
            _logger.LogInformation($"Get film with Id equal {id}."); // 1
            if (id < 0) // 2
            {
                _logger.LogWarning("Get film: model state is not valid."); // 3
                
                return BadRequest(ModelState); // 4
            }

            var film = await _crudService.GetFilmAsync(id); // 5
            if (film == null) // 6
            {
                return NotFound(); // 4
            } // выход из if - аналогично else // 7

            return Ok(film); // 4
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] FilmDTO film)
        {
            _logger.LogInformation("Create film.");
            
            var filmDTO = await _crudService.CreateFilmAsync(film, _appEnvironment);
            if (filmDTO == null) return StatusCode(500);

            return CreatedAtAction("CreateFilm", new { id = filmDTO.Id }, filmDTO);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromForm] FilmDTO film)
        {
            _logger.LogInformation($"Update film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Update film: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            string path = await _crudService.UpdateFilmAsync(id, film, _appEnvironment);
            
            return Ok(new{ path = path });
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            _logger.LogInformation($"Delete film with Id equal {id}.");
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Delete film: model state is not valid.");
                
                return BadRequest(ModelState);
            }

            await _crudService.DeleteFilmAsync(id, _appEnvironment);
            
            return NoContent();
        }
    }
}
