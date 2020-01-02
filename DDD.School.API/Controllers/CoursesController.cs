using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDD.School.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DDD.School.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CoursesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(int page=0, int pageSize=10)
        {
            var query = new CoursesArchive(page, pageSize);
            var results = await _mediator.Send(query);
            return Ok(results);
        }
    }
}
