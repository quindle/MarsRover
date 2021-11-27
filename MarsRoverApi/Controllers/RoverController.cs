using MarsRoverApi.Models;
using MarsRoverApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRoverApi.Controllers
{
       
    [Route("api/[Controller]")]
    public class RoverController
    {

        private readonly IRoverService _service;

        public RoverController(IRoverService service) {
            _service = service;
        }

        // GET api/rovers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rover>>> Get()
        {
            return new ObjectResult(await _service.GetAllRovers());
        }


        // GET api/rovers/:name
        [HttpGet("{name}")]
        public async Task<ActionResult<Rover>> Get(string name)
        {
            var rover = await _service.GetRoverByName(name);
            if (rover == null)
                return new NotFoundResult();

            return new ObjectResult(rover);
        }


        // POST api/rovers
        [HttpPost]
        public async Task<ActionResult<Planet>> Post([FromBody] Rover rover)
        {            
            await _service.Launch(rover);
            return new OkObjectResult(rover);
        }

        // DELETE api/rovers/:name
        [HttpDelete("{name}")]
        public async Task<IActionResult> Delete(string name)
        {
            var post = await _service.GetRoverByName(name);
            if (post == null)
                return new NotFoundResult();
            await _service.Retrieve(name);
            return new OkResult();
        }
    }
}
