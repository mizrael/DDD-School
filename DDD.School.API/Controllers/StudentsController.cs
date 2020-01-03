using System;
using System.Threading.Tasks;
using DDD.School.API.DTOs;
using DDD.School.Commands;
using DDD.School.Queries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DDD.School.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StudentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Get(int page = 0, int pageSize = 10)
        {
            var query = new StudentsArchive(page, pageSize);
            var results = await _mediator.Send(query);
            return Ok(results);
        }

        [HttpGet, Route("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute]Guid id)
        {
            var query = new StudentById(id);
            var result = await _mediator.Send(query);
            if (null == result) return NotFound();
            return Ok(result);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody]CreateStudentDTO dto)
        {
            if (null == dto) return BadRequest();
            var command = new CreateStudent(dto.Id, dto.FirstName, dto.LastName);
            await _mediator.Publish(command);
            return CreatedAtAction("GetById", new { id = dto.Id }, dto.Id);
        }
    }
}
