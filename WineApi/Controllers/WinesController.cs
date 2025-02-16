using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WineApi.Database.Entities;
using WineApi.Database.Repositories;
using WineApi.Models;

namespace WineApi.Controllers
{
    [ApiController]
    [Route("api/wines")]
    public class WinesController(ILogger<WinesController> logger, IRepository<Wine> repository, IValidator<WineDto> validator, IMapper mapper) : ControllerBase
    {
        private readonly IRepository<Wine> _repository = repository;
        private readonly ILogger<WinesController> _logger = logger;
        private readonly IValidator<WineDto> _validator = validator;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public ActionResult<IEnumerable<WineDto>> GetAllWines()
        {
            var winesDto = _mapper.Map<IEnumerable<WineDto>>(_repository.GetAll());

            return Ok(winesDto);
        }

        [HttpGet("{id}")]
        public ActionResult<WineDto> GetWineById(int id)
        {
            var wineDto = _mapper.Map<WineDto>(_repository.GetById(id));

            if (wineDto is null)
                return NotFound();
            else
                return Ok(wineDto);
        }

        [HttpPost]
        public ActionResult<WineDto> AddWine([FromBody] WineDto wineDto)
        {
            var result = _validator.Validate(wineDto);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            var dbEntity = _mapper.Map<Wine>(wineDto);
            _repository.Add(dbEntity);
            _repository.Save();

            return CreatedAtAction(nameof(GetWineById), new { id = dbEntity.Id }, wineDto);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateWine(int id, [FromBody] WineDto wineDto)
        {
            var result = _validator.Validate(wineDto);

            if (!result.IsValid)
                return BadRequest(result.Errors);

            if (_repository.GetById(id) == null)
                return NotFound();

            var dbEntity = _mapper.Map<Wine>(wineDto);
            dbEntity.Id = id;

            _repository.Update(dbEntity);
            _repository.Save();

            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteWine(int id)
        {
            if (_repository.GetById(id) == null)
                return NotFound();

            _repository.Delete(id);
            _repository.Save();

            return Ok();
        }
    }
}
